using UnityEngine;

namespace GameBasic
{
    public class Controller : MonoBehaviour
    {
        [HideInInspector] public float pawnInjuryMultiplier = 1f;

        [SerializeField] private Pawn presetPawn;
        [HideInInspector] public Pawn controlledPawn;
        [HideInInspector] public PlayCamera playCamera;

        //设计失误了，不应该把Control写这的。因为不同角色的控制不同，所以应该放到角色那里
        //不过反正我就一个角色，这次就懒得改了
        public Control control;

        [SerializeField] private float CamTopClamp ;
        [SerializeField] private float CamBottomClamp;
        [SerializeField] private float CamLeftClamp;
        [SerializeField] private float CamRightClamp;
        

        [HideInInspector]
        public bool bCamLock = false;

        #region 控制Controller自身的变量
        [SerializeField, Header("输入")] protected float mouseSpeed = 1.5f;
        [HideInInspector] public float mouseSpeedMul = 1f;
        protected float mouseLockTimer;
        protected float yaw;
        protected float pitch;
        [Header("EX")]
        [SerializeField]protected float exYaw;
        [SerializeField] protected float exPitch;
        #endregion

        #region 生命周期
        protected virtual void Awake()
        {
            control = new Control();
        }
        private void OnEnable()
        {
            control.Enable();
        }
        private void OnDisable()
        {
            control.Disable();
        }
        protected virtual void Start()
        {
            if (playCamera == null)
            {
                playCamera = Camera.main.GetComponent<PlayCamera>();
            }

            if (presetPawn != null)
            {
                ControlPawn(presetPawn);
            }
            else
            {
                transform.position = playCamera.transform.position;
                playCamera.SetCameraTarget(transform);
            }
                
        }
        protected virtual void Update()
        {
            mouseLockTimer.TimerTick();
        }
        private void LateUpdate()
        {
            UpdateRotation();
        }
        #endregion

        public virtual void ControlPawn(Pawn newPawn)
        {
            if (controlledPawn == newPawn)
                return;
            ReleasePawn();
            controlledPawn = newPawn;
            
            playCamera.SetCameraTarget(controlledPawn.centerTransform);
            yaw = 0;
            pitch = 0;

            exPitch = controlledPawn.transform.eulerAngles.x;
            exYaw = controlledPawn.transform.eulerAngles.y;

            controlledPawn.SetController(this);
        }

        public virtual void ReleasePawn()
        {
            if (controlledPawn != null)
            {
                controlledPawn.RemoveController();
            }
            controlledPawn = null;
        }

        /// <summary>
        /// Pitch  >0是向下看，<0是向上看
        /// </summary>
        public void SetRotateLimit(float topClamp, float bottonClamp, float yawClamp,float exPitch = 0,float exYaw = 0)
        {
            CamTopClamp = topClamp;
            CamBottomClamp = bottonClamp;
            CamLeftClamp = -yawClamp;
            CamRightClamp = yawClamp;

            this.exPitch += exPitch;
            this.exYaw += exYaw;
        }

        private void UpdateRotation()
        {
            Vector2 look = control.Controller.Look.ReadValue<Vector2>();
            if (mouseLockTimer <= 0 && look.sqrMagnitude >= 10)
            {
                float deltaTimeMove = Time.deltaTime * mouseSpeed * mouseSpeedMul;
                yaw += look.x * deltaTimeMove;
                pitch += look.y * deltaTimeMove;
            }

            yaw = Calc.ClampAngle(yaw, CamLeftClamp, CamRightClamp);
            pitch = Calc.ClampAngle(pitch, CamBottomClamp, CamTopClamp);

            transform.rotation = Quaternion.Euler(pitch+exPitch, yaw+exYaw, 0.0f);
            if (!bCamLock&&controlledPawn!=null)
                controlledPawn.centerTransform.rotation = Quaternion.Euler(pitch + exPitch, yaw+exYaw, 0.0f);
        }

    }

}
