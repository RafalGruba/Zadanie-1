using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    public Transform playerObject;
    
    [SerializeField] float mouseSensivity = 200f;
    [SerializeField] float maxDownCameraView = -90f;
    [SerializeField] float maxUpCameraView = 90f;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        float mouseAxisX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseAxisY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        xRotation -= mouseAxisY;
        xRotation = Mathf.Clamp(xRotation, maxDownCameraView, maxUpCameraView);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerObject.Rotate(Vector3.up * mouseAxisX);
        
        
    }
}
