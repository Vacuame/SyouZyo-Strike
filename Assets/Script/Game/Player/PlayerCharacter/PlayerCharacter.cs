using UnityEngine;
using UnityEngine.Animations.Rigging;
using static Control;
using static UnityEngine.InputSystem.InputAction;
using GameBasic;
using MoleMole;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

public class PlayerCharacter : Character
{
    [HideInInspector] public readonly string abilityRootPath = "ScriptObjectData/Player/Ability/";

    #region 变量

    [Header("绑定 as PlayerCharacter")]
    public Transform RightHandTransform;

    [SerializeField] public Collider assasinRange;
    [SerializeField] private Collider checkFightRange;
    [SerializeField] private List<Collider> fightAttackRange;

    [HideInInspector]public PlayerController playerController => controller as PlayerController;

    #region 输入
    private PlayerActions input;
    private Vector2 input_move;
    [Header("输入")]
    [SerializeField, Tooltip("相机上下角度限制")]
    public float CamTopClamp = 70.0f;
    public float CamBottomClamp = -30.0f;
    #endregion

    #region 移动设置
    [SerializeField, Header("移动")] private float walkMaxSpeed;
    [SerializeField] private float runMaxSpeed, xAcc, zAcc;
    [SerializeField] private float injury_walkMaxSpeed, injury_backMaxSpeed, injury_runMaxSpeed;
    [SerializeField] public float rotateSpeed;
    private const float walkThres = 1.5f, runThres = 3.3f;
    private const float injury_walkThres = 0.9f, injury_runThres = 1.47f;
    #endregion

    #region 运动状态
    private bool bCanMove = true;
    private bool bRuning;
    private bool bInjured;
    //public bool bAiming;
    #endregion

    #region 移动状态变量
    private float moveSpeedX, moveSpeedZ;
    #endregion

    #region Rig变量
    [SerializeField, Header("IK Rig")] public Rig twoHandRig;
    [SerializeField] public Rig chestRig;
    [SerializeField] public TwoBoneIKConstraint leftHandRig;
    [HideInInspector] public Transform leftFollow;
    #endregion

    #region 动画参数

    [Header("动画参数")]
    [SerializeField] private float injuryProportion;

    #endregion

    #endregion

    private bool setControlled = false;
    public override void SetController(Controller controller)
    {
        base.SetController(controller);

        controller.playCamera.SwitchCamera(Tags.Camera.Normal);
        controller.SetRotateLimit(CamTopClamp, CamBottomClamp, 360);

        controller.control.Player.Enable();
        
        if(!setControlled)//一开始想做多个控制器的，还写SetControll，后来东西多了各种绑定实在很麻烦
        {
            controller.control.Player.Interact.started += OnInteractPressed;
            controller.control.Player.Squat.started += OnCrouchPressed;
            controller.control.Player.Slot.started += OnSlotPressed;

            //手部Rig绑定为跟随相机前方
            chestRig.GetComponent<MultiAimConstraint>().data.sourceObjects =
            new WeightedTransformArray() { new WeightedTransform(controller.playCamera.frontTransform, 1) };
            GetComponent<RigBuilder>().Build();

            setControlled = true;
        }

        HUDManager.GetHUD<PlayerHUD>().SetVisiable(true);
        HUDManager.GetHUD<AimHUD>().SetVisiable(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public override void RemoveController()
    {
        //应该要把controller设为null的，但是一开始没有设计这个，可能会报错（很多地方都读了它），不弄了
        controller.control.Player.Disable();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        HUDManager.GetHUD<PlayerHUD>().SetVisiable(false);
        HUDManager.GetHUD<AimHUD>().SetVisiable(false);
    }
    private void OnCrouchPressed(CallbackContext context)
    {
        if (!ABS.HasTag("Crouch"))
            ABS.TryActivateAbility("Crouch");
        else
            ABS.TryEndAbility("Crouch");
    }
    private void OnInteractPressed(CallbackContext context)
    {
        if (ABS.TryActivateAbility("Fight"))
            return;
        if (ABS.TryActivateAbility("Interact", this))
            return;
        if (ABS.TryActivateAbility("Climb", anim, cc, transform))
            return;
    }
    private void OnSlotPressed(CallbackContext context)
    {
        int index = (int)context.ReadValue<float>() - 1;
        if (playerController.shortCutSlot[index]!=null)
        {
            ItemSave itemSave = playerController.shortCutSlot[index];
            ItemInfo itemInfo = ItemManager.Instance.GetItemInfo(itemSave.id);
            ABS.TryActivateAbility("EquipItem", itemInfo, itemSave);
        }
        
        HUDManager.GetHUD<SlotHUD>().DisplayEquipSlot(playerController.shortCutSlot, playerController.equipingItem);

    }
    private void Start()
    {
        Climb_SO climbAsset = Resources.Load<Climb_SO>("ScriptObjectData/ClimbData");
        ABS.GrandAbility(new Climb(climbAsset));

        EquipItemAsset equipAsset = Resources.Load<EquipItemAsset>(abilityRootPath+"EquipData");
        ABS.GrandAbility(new EquipItem(equipAsset,this));

        Interact_SO interactAsset = Resources.Load<Interact_SO>(abilityRootPath + "InteractData");
        ABS.GrandAbility(new Interact(interactAsset, centerTransform, controller.playCamera.transform));

        ABS.GrandAbility(new Crouch(Resources.Load<AbilityAsset>(abilityRootPath + "CrouchAsset"),this));

        ABS.GrandAbility(new RandomFight(Resources.Load<RandomFightAsset>(abilityRootPath + "FightAsset"),
            this, checkFightRange, fightAttackRange));

        ABS.AttrSet<CharaAttr>().health.onPreCurrentValueChange += OnHealthPre;
        HUDManager.GetHUD<PlayerHUD>().SetHpValue(ABS.AttrSet<CharaAttr>().health.GetProportion());
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

    //TODO 跑步时做FullBodyAction会打断动作，因为没做相关判断。（但是不影响，看不出来）
    #region MOVE

    private void Move()
    {
        if(!cc.enabled)return;

        Vector2 targetSpeed = input_move;
        float moveSpdThres;

        if (input_move != Vector2.zero)
        {
            if(bRuning||ABS.HasTag("Crouch")) 
                RotateToMove();
            else
                RotateToCamera();
        }


        if (bRuning || ABS.HasTag("Crouch"))
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

    #endregion

    #region Dead周期
    protected override void Dead()
    {
        
    }

    protected override void OnDeadEnd()
    {
        
    }
    #endregion

    private void IKUpdate()
    {
        if (leftFollow!=null)
            leftHandRig.data.target.transform.position = leftFollow.position;
        twoHandRig.weight = anim.GetFloat("rigWeight");
    }

    //不会被黄血秒杀
    private float OnHealthPre(AttributeBase health, float value)
    {
        value = Mathf.Clamp(value,0, health.BaseValue);
        if (health.GetProportion() > injuryProportion && value <= 0)
            value = 1;
        return value;
    }
    protected override void OnHealthPost(AttributeBase health, float old, float now)
    {
        float proportion = ABS.AttrSet<CharaAttr>().health.GetProportion();
        float injuryHealth = health.BaseValue * injuryProportion;
        bInjured = now <= injuryHealth;
        if (old > injuryHealth && now <= injuryHealth)
            anim.SetLayerWeight(anim.GetLayerIndex("Injury"), 1);
        else if (old <= injuryHealth && now > injuryHealth)
            anim.SetLayerWeight(anim.GetLayerIndex("Injury"), 0);

        if (now < old)
        {
            if(!ABS.HasTag("Endure"))
                ABS.ApplyGameplayEffectToSelf(new GameplayEffect(
                    Resources.Load<GameplayEffectAsset>("ScriptObjectData/Effect/PlayerOnHit")));
        }
        HUDManager.GetHUD<PlayerHUD>().SetHpValue(proportion);

        base.OnHealthPost(health, old, now);
    }
    protected override void OnHit(HitInfo hitInfo)
    {
        if (!ABS.HasTag("Invincible"))
            ABS.AttrSet<CharaAttr>().health.SetValueRelative(hitInfo.damage, Tags.Calc.Sub);
    }
}
