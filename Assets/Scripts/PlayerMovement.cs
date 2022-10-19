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
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.touchCount == 0)
        {
            // Keyboard
            MouseHeadMovement();
            KeyboardPlayerMove();
        }
        else
        {
            // Touch
            TouchHeadMovement();
            TouchPlayerMove();
        }
    }

    private void KeyboardPlayerMove()
    {
        var axis = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        MovePlayer(axis);
    }

    private void TouchPlayerMove()
    {
        var axis = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        MovePlayer(axis);
    }

    void MouseHeadMovement()
    {
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * _sensetive * 100 * Time.deltaTime;
        RotateCamera(delta);
    }

    void TouchHeadMovement()
    {
        foreach (var touch in Input.touches)
        {
            // if touch in right side of the screen
            if (touch.position.x > Screen.width / 2)
            {
                Vector2 delta = touch.deltaPosition * _sensetive * Time.deltaTime;
                RotateCamera(delta);
            }
        }
    }

    void MovePlayer(Vector2 axis)
    {
        float horizontalAxis = axis.x;
        float verticalAxis = axis.y;

        Vector3 moveVector = (transform.right.normalized * horizontalAxis + transform.forward.normalized * verticalAxis);
        _characterController.Move(moveVector * Time.deltaTime * _movementSpeed);
    }

    void RotateCamera(Vector2 delta)
    {
        float mouseX = delta.x;
        float mouseY = -delta.y;

        transform.rotation *= Quaternion.EulerAngles(new Vector3(0, mouseX, 0));

        Quaternion newHeadRotation = _headObject.transform.rotation * Quaternion.EulerAngles(new Vector3(mouseY, 0, 0));
        Quaternion bodyRotation = transform.rotation;

        float verticalDegreeDiff = Quaternion.Angle(newHeadRotation, bodyRotation);
        if (verticalDegreeDiff <= _verticalCameraLimit)
            SetHeadRotation(newHeadRotation);
    }

    private void SetHeadRotation(Quaternion newRotation)
    {
        _headObject.transform.rotation = newRotation;
    }

}
