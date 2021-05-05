using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

[Serializable]
public class Player_MouseLook : MonoBehaviourPunCallbacks
{
    [SerializeField] private float XSensitivity = 2f;
    [SerializeField] private float YSensitivity = 2f;
    [SerializeField] private bool ClampVerticalRotation = true;
    [SerializeField] private float MinimumX = -90f;
    [SerializeField] private float MaximumX = 90f;
    [SerializeField] private bool Smooth = false;
    [SerializeField] private float SmoothTime = 5f;
    [SerializeField] private bool LockCursor = true;

    private Quaternion CharacterTargetRotation;
    private Quaternion CameraTargetRotation;
    private bool CursorIsLocked;

    public void Init(Transform character, Transform camera)
    {
        CharacterTargetRotation = character.localRotation;
        CameraTargetRotation = character.localRotation;
    }

    public void LookRotation(Transform character, Transform camera)
    {
        float yRotation = Input.GetAxis("Mouse X") * XSensitivity;
        float xRotation = Input.GetAxis("Mouse Y") * YSensitivity;

        CharacterTargetRotation *= Quaternion.Euler(0f, yRotation, 0f);
        CameraTargetRotation *= Quaternion.Euler(-xRotation, 0f, 0f);

        if (ClampVerticalRotation)
        {
            CameraTargetRotation = ClampRotationAroundXAxis(CameraTargetRotation);
        }

        if (Smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, CharacterTargetRotation, SmoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, CameraTargetRotation, SmoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = CharacterTargetRotation;
            camera.localRotation = CameraTargetRotation;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        LockCursor = value;
        if (!LockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        if (LockCursor)
        {
            InternalLockUpdate();
        }
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            CursorIsLocked = false;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            CursorIsLocked = true;
        }

        if (CursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        else if (!CursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion quaternion)
    {
        quaternion.x /= quaternion.w;
        quaternion.y /= quaternion.w;
        quaternion.z /= quaternion.w;
        quaternion.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(quaternion.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        quaternion.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return quaternion;
    }
}
