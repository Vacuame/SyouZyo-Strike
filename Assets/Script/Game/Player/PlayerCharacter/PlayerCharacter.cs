using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;
using static Control;
using static UnityEngine.InputSystem.InputAction;
using GameBasic;

public class PlayerCharacter : Character
{
    [SerializeField, Header("DEBUG")]
    protected InstanceItem itemInHand;

    [SerializeField] public Gun curGun;

    #region 输入
    private PlayerActions input;
    private Vector2 input_move;
    #endregion

    #region 移动设置
    [SerializeField, Header("移动")] private float walkMaxSpeed;
    [SerializeField] private float runMaxSpeed, xAcc, zAcc;
    [SerializeField] private float injury_walkMaxSpeed, injury_backMaxSpeed, injury_runMaxSpeed;
    [SerializeField] private float rotateSpeed;
    private const float walkThres = 1.5f, runThres = 3.3f;
    private const float injury_walkThres = 0.9f, injury_runThres = 1.47f;
    #endregion

    #region 运动状态
    private bool bCanMove = true;
    private bool bRuning;
    private bool bInjured;
    public bool bAiming;
    #endregion

    #region 移动状态变量
    private float moveSpeedX, moveSpeedZ;
    #endregion

    #region Rig变量
    [SerializeField, Header("IK Rig")] private Rig twoHandRig;
    [SerializeField] private Rig chestRig;
    [SerializeField] private TwoBoneIKConstraint leftHandRig;
    [SerializeField] private Transform leftFollow;
    #endregion

    public override void SetController(Controller controller)
    {
        base.SetController(controller);

        controller.control.Player.Interact.started += OnInteractPressed;

        controller.control.Player.Weapon1.started += (CallbackContext context) =>
        ABS.TryActivateAbility("EquipItem", this, chestRig);
    }

    private void OnInteractPressed(CallbackContext context)
    {
        Climb.ClimbSpec climb;
        climb = ABS.AbilityContainer.GetAbility("Climb") as Climb.ClimbSpec;
        ABS.TryActivateAbility("Climb", anim, cc, transform);
    }


    protected override void Awake()
    {
        base.Awake();

        Climb_SO so = Resources.Load<Climb_SO>("ScriptObjectData/ClimbData");
        ABS.GrandAbility(new Climb(so));
        AbilityAsset asset = Resources.Load<AbilityAsset>("ScriptObjectData/EquipData");
        ABS.GrandAbility(new EquipItem(asset));
    }

    protected void Update()
    {
        if(bControlable && controller != null)
            input = controller.control.Player;
        else
            input = new PlayerActions();

        if (bCanMove)
        {
            //这个可能就是Move动作了，以后把它改到Ability里
            input_move = input.Move.ReadValue<Vector2>();
            bRuning = input.Run.IsPressed();//TODO 以后改逻辑
        }
        else
        {
            input_move = Vector2.zero;
            bRuning = false;
        }
       
        anim.SetBool("runing", bRuning);
        anim.SetFloat("inputX", input_move.x);
        anim.SetFloat("inputY", input_move.y);

        bool falling = false;
        bool landing = false;
        Vector3 feetPos = feetTransform.position;
        Ray ray = new Ray(feetPos,Vector3.down);

        if(cc.velocity.y < -1f && Physics.Raycast(ray,out RaycastHit hitInfo,10))
        {
            if (hitInfo.distance > 0.4f)
                falling = true;
            if (hitInfo.distance < 0.3f)
                landing = true;
        }

        if (!landing) landing = cc.isGrounded;
        anim.SetBool("falling", falling);
        anim.SetBool("landing", landing);

        IKUpdate();
    }

    private void Move()
    {
        Vector2 targetSpeed = input_move;
        float moveSpdThres;

        if (bAiming)
        {
            RotateToCamera();
        }
        else if (input_move != Vector2.zero)
        {
            if(bRuning) 
                RotateToMove();
            else
                RotateToCamera();
        }
                

        if (bRuning)
        {
            moveSpdThres = bInjured ? injury_runThres : runThres;
            if (input_move.sqrMagnitude != 0)
                targetSpeed = new Vector2(0, bInjured ? injury_runMaxSpeed : runMaxSpeed);
        }
        else
        {
            moveSpdThres = bInjured ? injury_walkThres : walkThres;
            targetSpeed *= bInjured ? (input_move.y >= 0 ? injury_walkMaxSpeed : injury_backMaxSpeed) : walkMaxSpeed;
        }
        moveSpeedX = Mathf.MoveTowards(moveSpeedX, targetSpeed.x, xAcc * Time.fixedDeltaTime);
        moveSpeedZ = Mathf.MoveTowards(moveSpeedZ, targetSpeed.y, zAcc * Time.fixedDeltaTime);
        anim.SetFloat("moveSpeedX", moveSpeedX);
        anim.SetFloat("moveSpeedZ", moveSpeedZ);
        float speedMagni = Mathf.Max(1, new Vector2(moveSpeedX, moveSpeedZ).magnitude / moveSpdThres);
        speedMagni = Mathf.Sqrt(speedMagni);
        //动画加速√magni  得到动画的速度后再加速√magni
        anim.SetFloat("moveAnimSpeed", speedMagni);
        Vector3 animVelo = anim.velocity * speedMagni;
        cc.SimpleMove(animVelo);
    }

    private void RotateToCamera()
    {
        Transform cameraTrans = controller.playCamera.transform;
        Vector3 cameraAngle = new Vector3(cameraTrans.forward.x, 0, cameraTrans.forward.z);
        Quaternion targetRotation = Quaternion.LookRotation(cameraAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
    }

    private void RotateToMove()
    {
        Transform cameraTrans = controller.playCamera.transform;
        Vector3 forward = cameraTrans.forward;
        Vector3 right = cameraTrans.right;
        forward.y = 0; right.y = 0;
        forward *= input_move.y;
        right *= input_move.x;
        Quaternion targetRotation = Quaternion.LookRotation(forward + right);

        float rad = Mathf.Acos(Vector3.Dot(forward, transform.forward));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rad * rotateSpeed * Time.fixedDeltaTime);
    }

    private void OnAnimatorMove()
    {
        Move();
    }


    #region Dead周期
    protected override void Dead()
    {
        
    }

    protected override void OnDeadEnd()
    {
        
    }
    #endregion

    #region 动画使用的函数
    private void SetWeapon(int active)
    {
        bool isActive = active == 0 ? false : true;
        curGun.user = this.gameObject;
        curGun.gameObject.SetActive(isActive);
    }
    #endregion

    private void IKUpdate()
    {
        if (leftFollow.gameObject.activeSelf)
            leftHandRig.data.target.transform.position = leftFollow.transform.position;
        twoHandRig.weight = anim.GetFloat("rigWeight");
    }

    protected override void OnHit(HitInfo hitInfo)
    {
        Debug.Log("Chara Be Hit");
    }
}
