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

public class PlayerCharacter : Character
{
    [SerializeField,Header("DEBUG")]
    protected ItemInstance itemInHand;

    [SerializeField] public Gun curGun;

    #region ����
    private PlayerActions input;
    private Vector2 input_move;
    #endregion

    #region �ƶ�����
    [SerializeField, Header("�ƶ�")] private float walkMaxSpeed;
    [SerializeField] private float runMaxSpeed,xAcc, zAcc;
    [SerializeField] private float injury_walkMaxSpeed, injury_backMaxSpeed, injury_runMaxSpeed;
    [SerializeField] private float rotateSpeed;
    private const float walkThres = 1.5f, runThres = 3.3f;
    private const float injury_walkThres = 0.9f, injury_runThres = 1.47f;
    #endregion

    #region �˶�״̬
    private bool bCanMove = true;
    private bool bRuning;
    private bool bInjured;
    private bool bAiming;
    #endregion

    #region �ƶ�״̬����
    private float moveSpeedX, moveSpeedZ;
    #endregion

    #region Rig����
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
        ABS.TryActivateAbility("EquipItem",this, chestRig);
    }

    private void OnInteractPressed(CallbackContext context)=>
        ABS.TryActivateAbility("Climb",anim,cc,transform);

    protected override void Awake()
    {
        base.Awake();

        //֮����Щ�������������ļ�д
        Climb_SO so = Resources.Load<Climb_SO>("ScriptObjectData/ClimbData");
        ABS.GrandAbility(new Climb(so));
        AbilityAsset asset = Resources.Load<AbilityAsset>("ScriptObjectData/EquipData");
        ABS.GrandAbility(new EquipItem(asset));

        CharaAtrr s = new CharaAtrr(Resources.Load<CharaAttr_SO>("ScriptObjectData/CharaData"));
        ABS.AttributeSetContainer.AddAttributeSet(s);

        //ABS.ApplyGameplayEffectToSelf(new TestEffect(Resources.Load<TestEffect_SO>("ScriptObjectData/TestData")));
    }

    protected void Update()
    {
        if(bControlable && controller != null)
            input = controller.control.Player;
        else
            input = new PlayerActions();

        ABS.Tick();

        if (bCanMove)
        {
            //������ܾ���Move�����ˣ��Ժ�����ĵ�Ability��
            input_move = input.Move.ReadValue<Vector2>();
            bRuning = input.Run.IsPressed();//TODO �Ժ���߼�
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
            RotationInWalk();
            //RotationInAim();
        else if (bRuning)
            RotationInRun();
        else
            RotationInWalk();

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
        //�������١�magni  �õ��������ٶȺ��ټ��١�magni
        anim.SetFloat("moveAnimSpeed", speedMagni);
        Vector3 animVelo = anim.velocity * speedMagni;
        cc.SimpleMove(animVelo);
    }

    private void RotationInWalk()
    {
        if (input_move != Vector2.zero)
        {
            Vector3 cameraAngle = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            Quaternion targetRotation = Quaternion.LookRotation(cameraAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }
    }

    private void RotationInRun()
    {
        if (input_move == Vector2.zero) return;
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            forward.y = 0; right.y = 0;
            forward *= input_move.y;
            right *= input_move.x;
            Quaternion targetRotation = Quaternion.LookRotation(forward + right);

            float rad = Mathf.Acos(Vector3.Dot(forward, transform.forward));

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rad * rotateSpeed * Time.fixedDeltaTime);
        }


    }

    private void OnAnimatorMove()
    {
        Move();
    }


    #region Dead����
    protected override void OnDead()
    {
        
    }

    protected override void OnDeadEnd()
    {
        
    }

    protected override void OnDeadStart()
    {
        
    }
    #endregion

    #region ����ʹ�õĺ���
    private void SetWeapon(int active)
    {
        bool isActive = active == 0 ? false : true;
        curGun.gameObject.SetActive(isActive);
    }
    #endregion

    private void IKUpdate()
    {
        if (leftFollow.gameObject.activeSelf)
            leftHandRig.data.target.transform.position = leftFollow.transform.position;
        twoHandRig.weight = anim.GetFloat("rigWeight");
    }
}
