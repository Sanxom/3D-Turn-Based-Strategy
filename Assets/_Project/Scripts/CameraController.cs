using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = 1f;
        }

        float moveSpeed = 10f;

        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveSpeed * Time.deltaTime * moveVector;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = Vector3.zero;
        float mouseRotationSpeed = 200f;
        float keyboardRotationSpeed = 100f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = -1f;
            transform.eulerAngles += keyboardRotationSpeed * Time.deltaTime * rotationVector;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = 1f;
            transform.eulerAngles += keyboardRotationSpeed * Time.deltaTime * rotationVector;
        }

        if (Input.GetMouseButton(1))
        {
            transform.eulerAngles += mouseRotationSpeed * Time.deltaTime * new Vector3(0, Input.GetAxis("Mouse X"), 0);
        }
    }

    private void HandleZoom()
    {
        float zoomAmount = 1f;

        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}