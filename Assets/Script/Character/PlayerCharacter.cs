using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;
using static Control;

public class PlayerCharacter : Character
{
    [SerializeField,Header("DEBUG")]
    protected ItemInstance itemInHand;

    #region 输入
    private PlayerActions input;
    private Vector2 input_move;
    #endregion

    #region 移动设置
    [SerializeField, Header("移动")] private float walkMaxSpeed;
    [SerializeField] private float runMaxSpeed,xAcc, zAcc;
    [SerializeField] private float injury_walkMaxSpeed, injury_backMaxSpeed, injury_runMaxSpeed;
    [SerializeField] private float rotateSpeed;
    private const float walkThres = 1.5f, runThres = 3.3f;
    private const float injury_walkThres = 0.9f, injury_runThres = 1.47f;
    #endregion

    #region 运动状态
    private bool bRuning;
    private bool bInjured;
    private bool bAiming;
    #endregion

    #region 移动状态变量
    private float moveSpeedX, moveSpeedZ;
    #endregion

    protected override void Update()
    {
        base.Update();
        if(bControlable && controller != null)
        {
            input = controller.control.Player;
            input_move = input.Move.ReadValue<Vector2>();
        }
        else
            input = new PlayerActions();

        bRuning = input.Run.IsPressed();//TODO 以后改逻辑

        anim.SetBool("runing", bRuning);
        anim.SetFloat("inputX", input_move.x);
        anim.SetFloat("inputY", input_move.y);
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
        //动画加速√magni  得到动画的速度后再加速√magni
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
/*        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Climb") && !anim.IsInTransition(0))
            climbType = 0;

        if (climbType != 0)
            Climb();
        else
        {
            climbType = 0;
            anim.SetInteger("climbType", climbType);
            //col.enabled = true;
            //rd.useGravity = true;
            cc.enabled = true;
            Move();
        }*/
        Move();
    }

    protected override void OnDead()
    {
        
    }

    protected override void OnDeadEnd()
    {
        
    }

    protected override void OnDeadStart()
    {
        
    }

}
