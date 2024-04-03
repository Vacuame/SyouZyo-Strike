using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBasic
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Pawn presetPawn;
        [HideInInspector] public Pawn controlledPawn;
        [HideInInspector] public PlayCamera playCamera;
        public Control control;

        [SerializeField, Tooltip("相机向上角度限制")]
        private float CamTopClamp = 70.0f;
        [SerializeField, Tooltip("相机向下角度限制")]
        private float CamBottomClamp = -30.0f;
        [HideInInspector]
        public float CamAngleOverride = 0.0f;
        [HideInInspector]
        public bool bCamLock = false;

        #region 控制Controller自身的变量
        [SerializeField, Header("输入")] protected float mouseSpeed = 1.5f;
        protected float mouseLockTimer;
        protected float Yaw;
        protected float Pitch;
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

        protected virtual void ControlPawn(Pawn pawn)
        {
            if (this.controlledPawn == pawn)
                return;
            if (pawn != null)
                pawn.RemoveController();
            this.controlledPawn = pawn;
            pawn.SetController(this);
            Yaw = pawn.transform.rotation.eulerAngles.y;
            Pitch = pawn.transform.rotation.eulerAngles.x;
            playCamera.SetCameraTarget(pawn.camTraceTransform);
        }

        private void UpdateRotation()
        {
            Vector2 look = control.Player.Look.ReadValue<Vector2>();
            if (mouseLockTimer <= 0 && look.sqrMagnitude >= 10)
            {
                float deltaTimeMove = Time.deltaTime * mouseSpeed;
                Yaw += look.x * deltaTimeMove;
                Pitch += look.y * deltaTimeMove;
            }
            Yaw = Calc.ClampAngle(Yaw, float.MinValue, float.MaxValue);
            Pitch = Calc.ClampAngle(Pitch, CamBottomClamp, CamTopClamp);

            transform.rotation = Quaternion.Euler(Pitch, Yaw, 0.0f);
            if (!bCamLock&&controlledPawn!=null)
                controlledPawn.camTraceTransform.rotation = Quaternion.Euler(Pitch + CamAngleOverride, Yaw, 0.0f);
        }

    }

}
