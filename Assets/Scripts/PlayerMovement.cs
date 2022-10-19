using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _sensetive;
    [SerializeField] private GameObject _headObject;
    [SerializeField] private float _verticalCameraLimit = 75f;
    [SerializeField] private Joystick _joystick;

    private CharacterController _characterController;
    private float _xCameraRotation;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        SetHeadRotation(transform.rotation);
        _sensetive /= 1000;
    }

    private void Update()
    {
        HandlePlayerRotate();
        HandlePlayerMove();
    }

    private void HandlePlayerMove()
    {
        float horizontalAxis = _joystick.Horizontal;
        float verticalAxis = _joystick.Vertical;

        Vector3 moveVector = (transform.right.normalized * horizontalAxis + transform.forward.normalized * verticalAxis);
        _characterController.Move(moveVector * Time.deltaTime * _movementSpeed);
    }

    private void HandlePlayerRotate()
    {
        foreach (var touch in Input.touches)
        {
            // if touch in right side of the screen
            if (touch.position.x > Screen.width / 2)
            {
                Vector2 delta = touch.deltaPosition;
                float mouseX = delta.x * _sensetive * Time.deltaTime;
                float mouseY = -delta.y * _sensetive * Time.deltaTime;

                transform.rotation *= Quaternion.EulerAngles(new Vector3(0, mouseX, 0));

                Quaternion newHeadRotation = _headObject.transform.rotation * Quaternion.EulerAngles(new Vector3(mouseY, 0, 0));
                Quaternion bodyRotation = transform.rotation;

                float verticalDegreeDiff = Quaternion.Angle(newHeadRotation, bodyRotation);
                if (verticalDegreeDiff <= _verticalCameraLimit)
                    SetHeadRotation(newHeadRotation);
            }
        }
    }

    private void SetHeadRotation(Quaternion newRotation)
    {
        _headObject.transform.rotation = newRotation;
    }

}
