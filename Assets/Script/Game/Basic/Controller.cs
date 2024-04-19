using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;
using static UnityEditor.PlayerSettings;

namespace GameBasic
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Pawn presetPawn;
        [HideInInspector] public Pawn controlledPawn;
        [HideInInspector] public PlayCamera playCamera;
        public Control control;

        [SerializeField] private float CamTopClamp ;
        [SerializeField] private float CamBottomClamp;
        [SerializeField] private float CamLeftClamp;
        [SerializeField] private float CamRightClamp;
        

        [HideInInspector]
        public bool bCamLock = false;

        #region 控制Controller自身的变量
        [SerializeField, Header("输入")] protected float mouseSpeed = 1.5f;
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

            playCamera.SetCameraTarget(transform);

            if (presetPawn != null)
                ControlPawn(presetPawn);
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
            if (controlledPawn != null)
                controlledPawn.RemoveController();
            controlledPawn = newPawn;
            
            playCamera.SetCameraTarget(controlledPawn.centerTransform);
            yaw = 0;
            pitch = 0;

            exPitch = controlledPawn.transform.eulerAngles.x;
            exYaw = controlledPawn.transform.eulerAngles.y;

            controlledPawn.SetController(this);
        }

        /// <summary>
        /// Pitch  >0是向下看，<0是向上看
        /// </summary>
        public void SetRotateLimit(float topClamp, float bottonClamp, float yawClamp,float exPitch = Consts.NullFloat,float exYaw = Consts.NullFloat)
        {
            CamTopClamp = topClamp;
            CamBottomClamp = bottonClamp;
            CamLeftClamp = -yawClamp;
            CamRightClamp = yawClamp;

            if(exPitch != Consts.NullFloat)
                this.exPitch = exPitch;
            if(exYaw != Consts.NullFloat)
                this.exYaw = exYaw;
        }

        private void UpdateRotation()
        {
            Vector2 look = control.Controller.Look.ReadValue<Vector2>();
            if (mouseLockTimer <= 0 && look.sqrMagnitude >= 10)
            {
                float deltaTimeMove = Time.deltaTime * mouseSpeed;
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
