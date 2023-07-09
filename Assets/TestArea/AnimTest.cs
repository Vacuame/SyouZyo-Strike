using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static Calc;
using static Control;
using static RootMotion.FinalIK.FBIKChain;
using static UnityEngine.Rendering.DebugUI;

public class AnimTest : MonoBehaviour
{
    [SerializeField] Animator anim;
    [Range(0,1)]
    //public float weight;
    public Control control;
    private Vector2 input_move;
    public float moveSpeedX, moveSpeedZ;
    private Rigidbody rd;
    private const float walkThres=1.5f,runThres=3.3f;
    private const float injury_walkThres = 0.9f, injury_runThres = 1.47f;
    [SerializeField]private float walkMaxSpeed,runMaxSpeed,xAcc,zAcc;
    [SerializeField] private float injury_walkMaxSpeed,injury_backMaxSpeed, injury_runMaxSpeed;
    [SerializeField] private float rotateSpeed;
    private bool _runing;

    [SerializeField] private Rig twoHandRig,chestRig;
    [SerializeField] private TwoBoneIKConstraint leftHandRig;
    [SerializeField] private Transform leftFollow;

    [SerializeField] private CinemachineBrain camera;
    [SerializeField] private CinemachineVirtualCamera normalCamera;
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private Transform centerTransform;
    [SerializeField] private float mouseSpeed;
    private float cameraTargetYaw;
    private float cameraTargetPitch;
    private float mouseLockTimer;

    [SerializeField] private Gun curGun;
    
    private bool runing
    {
        get { return _runing; }
        set { if (aiming) _runing = false; else _runing = value; }
    }
    private bool aiming;
    private bool injured;
    [SerializeField] private float injuryAnimSwitchSpeed;
    private float injuryAnimWeight;
    private int weaponType;

    private int animLayerIndex_Injury;
    //听说调用transform影响效率，故保存一下
    private Transform playerTransform;
    private void Awake()
    {
        control=new Control();
        playerTransform = transform;
        rd=GetComponent<Rigidbody>();

        animLayerIndex_Injury = anim.GetLayerIndex("Injury");
    }
    private void Start()
    {
        cameraTargetYaw = centerTransform.rotation.eulerAngles.y;
        cameraTargetPitch = centerTransform.rotation.eulerAngles.x;
        mouseLockTimer = 1f;
    }

    private void OnEnable()
    {
        control.Enable();
    }
    private void OnDisable()
    {
        control.Disable();
    }
    private void Update()
    {
        mouseLockTimer.TimePassBy();

        //control
        input_move = control.Player.Move.ReadValue<Vector2>();
        runing = control.Player.Run.IsPressed();

        TrySetAiming();

        if (control.Player.Fire.WasPressedThisFrame())
            curGun.TrySetShooting(true);
        else if (control.Player.Fire.WasReleasedThisFrame())
            curGun.TrySetShooting(false);

        if (control.Player.Reload.WasPressedThisFrame())
        {
            curGun.curAmmo = curGun.fullAmmo;
        }

        if (Input.GetKeyDown(KeyCode.F))
            injured = !injured;

        int a = ClimbCheck();
        Debug.Log(a);

        float injuryAnimTarget = injured? 1.0f : 0.0f;
        injuryAnimWeight = Mathf.MoveTowards(injuryAnimWeight, injuryAnimTarget, injuryAnimSwitchSpeed * Time.deltaTime);
        anim.SetLayerWeight(animLayerIndex_Injury, injuryAnimWeight);

        //anim
        anim.SetBool("aiming", aiming);
        anim.SetBool("runing", runing);
        anim.SetFloat("inputX", input_move.x);
        anim.SetFloat("inputY", input_move.y);

        SwitchWeapon();

        IKChange();
    }

    [SerializeField]private float minClimbHeight,maxClimbHeight,climbStep;
    [SerializeField] private float climbCheckDistance, climbOverDistance;
    [SerializeField]private LayerMask climbLayer;
    [SerializeField] private float climbTowardAngle;
    


    private int ClimbCheck()
    {
        RaycastHit climbHit;
        if(Physics.Raycast(transform.position+Vector3.up*minClimbHeight,transform.forward,out climbHit, climbCheckDistance, climbLayer))
        {
            Vector3 toWallDire = -climbHit.normal;
            if (Vector3.Angle(transform.forward, toWallDire) > climbTowardAngle) return 0;

            float wallHeight= maxClimbHeight + climbStep;

            RaycastHit lastClimbHit=climbHit;

            for(float height = minClimbHeight+climbStep; height <= maxClimbHeight+climbStep; height+=climbStep)
            {
                if (Physics.Raycast(transform.position + Vector3.up * height, toWallDire, out climbHit, climbCheckDistance, climbLayer))
                {
                    lastClimbHit = climbHit;
                }
                else if (Physics.Raycast(lastClimbHit.point+Vector3.up*climbStep+toWallDire*0.2f,Vector3.down,out climbHit,climbStep))
                {
                    wallHeight = height - climbStep + climbHit.point.y-lastClimbHit.point.y;
                    break;
                }
            }
            if (wallHeight > maxClimbHeight) return 0;

            if (!Physics.Raycast(lastClimbHit.point + Vector3.up * climbStep + toWallDire * climbOverDistance, Vector3.down, climbStep))
                    return 2;
            else
                return 1;

        }
        return 0;
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void Move()
    {
        Vector2 targetSpeed = input_move;
        float moveSpdThres;

        if (aiming)
            RotationInAim();
        else if (runing) 
            RotationInRun();
        else
            RotationInWalk(); 

        if (runing)
        {
            moveSpdThres = injured? injury_runThres:runThres;
            if(input_move.sqrMagnitude!=0)
                targetSpeed = new Vector2(0, injured ? injury_runMaxSpeed : runMaxSpeed);
        }  
        else
        {
            moveSpdThres =injured?injury_walkThres: walkThres;
            targetSpeed *= injured ? (input_move.y>=0?injury_walkMaxSpeed:injury_backMaxSpeed) :walkMaxSpeed;
        }
        moveSpeedX = Mathf.MoveTowards(moveSpeedX, targetSpeed.x, xAcc*Time.fixedDeltaTime);
        moveSpeedZ = Mathf.MoveTowards(moveSpeedZ, targetSpeed.y, zAcc * Time.fixedDeltaTime);
        anim.SetFloat("moveSpeedX", moveSpeedX);
        anim.SetFloat("moveSpeedZ", moveSpeedZ);
        float speedMagni = Mathf.Max(1, new Vector2(moveSpeedX,moveSpeedZ).magnitude / moveSpdThres);
        speedMagni = Mathf.Sqrt(speedMagni);
        //动画加速√magni  得到动画的速度后再加速√magni
        anim.SetFloat("moveAnimSpeed", speedMagni);
        Vector3 animVelo = anim.velocity * speedMagni;
        Vector3 rdVelo = rd.velocity;
        rdVelo.x = animVelo.x;
        rdVelo.z=animVelo.z;
        rd.velocity=rdVelo;

        if(weaponType !=0 ) curGun.moving = (targetSpeed != Vector2.zero);
    }

    
    private void RotationInAim()
    {
        Vector3 cameraAngle = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        Quaternion targetRotation = Quaternion.LookRotation(cameraAngle);
        playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, 2*rotateSpeed * Time.fixedDeltaTime);
    }

    private void RotationInWalk()
    {
        if (input_move != Vector2.zero)
        {
            Vector3 cameraAngle = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
            Quaternion targetRotation = Quaternion.LookRotation(cameraAngle);
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
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

            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rad * rotateSpeed * Time.fixedDeltaTime);
        }

            
    }
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    private void CameraRotation()
    {
        Vector2 look = control.Player.Look.ReadValue<Vector2>();
        if (mouseLockTimer<=0&&look.sqrMagnitude >= 10)
        {
            float deltaTimeMove =  Time.deltaTime * mouseSpeed;
            cameraTargetYaw += look.x * deltaTimeMove;
            cameraTargetPitch += look.y * deltaTimeMove;
        }
        cameraTargetYaw = ClampAngle(cameraTargetYaw, float.MinValue, float.MaxValue);
        cameraTargetPitch = ClampAngle(cameraTargetPitch, BottomClamp, TopClamp);
        Transform followTarget = camera.ActiveVirtualCamera.Follow.transform;
        followTarget.rotation = Quaternion.Euler(cameraTargetPitch + CameraAngleOverride,
            cameraTargetYaw, 0.0f);
    }

    private void TrySetAiming()
    {
        if (weaponType == 0) return;

        bool was=aiming;
        if (control.Player.Aim.WasPressedThisFrame())
            aiming = true;
        if (control.Player.Aim.WasReleasedThisFrame())
            aiming = false;

        if (was != aiming)
        {
            SwitchCamera(aiming ? 1 : 0);
            if (aiming)
            {
                runing = false;
                chestRig.weight = 1;
            }
            else
            {
                chestRig.weight = 0;
            }

            curGun.SetAiming(aiming);
        }
    }

    void SwitchCamera(int c)
    {
        normalCamera.gameObject.SetActive(false);
        aimCamera.gameObject.SetActive(false);
        if (c==0)
            normalCamera.gameObject.SetActive(true);
        else if(c==1)
            aimCamera.gameObject.SetActive(true);
    }


    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        return Mathf.Clamp(lfAngle%360, lfMin, lfMax);
    }
    private void SetWeapon(int active)
    {
        bool isActive = active==0?false:true;
        curGun.gameObject.SetActive(isActive);
    }


    private void SwitchWeapon()
    {
        if (control.Player.Weapon1.WasPressedThisFrame())
        {
            weaponType = weaponType == 1 ? 0 : 1;
        }

        anim.SetInteger("weaponType", weaponType);
    }

    private void OnAnimatorMove()
    {
        Move();
    }

    private void IKChange()
    {
        if (leftFollow.gameObject.activeSelf)
            leftHandRig.data.target.transform.position = leftFollow.transform.position;
        twoHandRig.weight = anim.GetFloat("rigWeight");
    }

/*    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPosition(AvatarIKGoal.LeftHand, Vector3.zero);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
    }*/
}
