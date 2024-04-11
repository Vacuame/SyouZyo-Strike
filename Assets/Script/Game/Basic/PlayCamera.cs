using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBasic;
public class PlayCamera : MonoBehaviour
{
    public Camera cameraComponent { get; private set; }
    public CinemachineBrain cameraBrain { get; private set; }

    [SerializeField] public Transform frontTransform;
    [SerializeField] private MultiSetting<Tags.Camera, CinemachineVirtualCamera> cameras;
    private Tags.Camera _curCameraTag = Tags.Camera.Normal;
    private CinemachineVirtualCamera curCamera;
    
    private Transform target;

    private Tags.Camera curCameraTag
    {
        get => _curCameraTag;
        set
        {
            _curCameraTag = value;
            curCamera = cameras.Get(_curCameraTag);
        }
    }
    
    private void Awake()
    {
        curCameraTag = Tags.Camera.Normal;

        cameraComponent = GetComponent<Camera>();
        cameraBrain = GetComponent<CinemachineBrain>();
    }

    public void SetCameraTarget(Transform target)
    {
        this.target = target;
        curCamera.Follow = target;
        curCamera.LookAt = target;
    }

    public void SwitchCamera(Tags.Camera tag)
    {
        if (tag != curCameraTag)
        {
            curCamera.gameObject.SetActive(false);
            curCameraTag = tag;
            curCamera.gameObject.SetActive(true);
            curCamera.Follow = target;
            curCamera.LookAt = target;
        }

    }


}
