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

    #region ����

    [Header("�� as PlayerCharacter")]
    public Transform RightHandTransform;

    [SerializeField] public Collider assasinRange;
    [SerializeField] private Collider checkFightRange;
    [SerializeField] private List<Collider> fightAttackRange;

    [HideInInspector]public PlayerController playerController => controller as PlayerController;

    #region ����
    private PlayerActions input;
    private Vector2 input_move;
    [Header("����")]
    [SerializeField, Tooltip("������½Ƕ�����")]
    public float CamTopClamp = 70.0f;
    public float CamBottomClamp = -30.0f;
    #endregion

    #region �ƶ�����
    [SerializeField, Header("�ƶ�")] private float walkMaxSpeed;
    [SerializeField] private float runMaxSpeed, xAcc, zAcc;
    [SerializeField] private float injury_walkMaxSpeed, injury_backMaxSpeed, injury_runMaxSpeed;
    [SerializeField] public float rotateSpeed;
    private const float walkThres = 1.5f, runThres = 3.3f;
    private const float injury_walkThres = 0.9f, injury_runThres = 1.47f;
    #endregion

    #region �˶�״̬
    private bool bCanMove = true;
    private bool bRuning;
    private bool bInjured;
    //public bool bAiming;
    #endregion

    #region �ƶ�״̬����
    private float moveSpeedX, moveSpeedZ;
    #endregion

    #region Rig����
    [SerializeField, Header("IK Rig")] public Rig twoHandRig;
    [SerializeField] public Rig chestRig;
    [SerializeField] public TwoBoneIKConstraint leftHandRig;
    [HideInInspector] public Transform leftFollow;
    #endregion

    #region ��������

    [Header("��������")]
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
        
        if(!setControlled)//һ��ʼ��������������ģ���дSetControll�������������˸��ְ�ʵ�ں��鷳
        {
            controller.control.Player.Interact.started += OnInteractPressed;
            controller.control.Player.Squat.started += OnCrouchPressed;
            controller.control.Player.Slot.started += OnSlotPressed;

            //�ֲ�Rig��Ϊ�������ǰ��
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
        //Ӧ��Ҫ��controller��Ϊnull�ģ�����һ��ʼû�������������ܻᱨ���ܶ�ط���������������Ū��
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

    //TODO �ܲ�ʱ��FullBodyAction���϶�������Ϊû������жϡ������ǲ�Ӱ�죬����������
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
        //�������١�magni  �õ��������ٶȺ��ټ��١�magni
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

    #region Dead����
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

    //���ᱻ��Ѫ��ɱ
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
