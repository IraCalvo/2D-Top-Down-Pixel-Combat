using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirutalCamera;

    public void Start()
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        cinemachineVirutalCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirutalCamera.Follow = PlayerController.Instance.transform;
    }
}
