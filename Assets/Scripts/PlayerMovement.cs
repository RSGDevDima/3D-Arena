using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] float _sensetive;
    [SerializeField] GameObject _headObject;
    [SerializeField] float _verticalCameraLimit = 75f;
    [SerializeField] Joystick _joystick;

    CharacterController characterController;
    float xCameraRotation;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        setHeadRotation(transform.rotation);
        _sensetive /= 1000;
    }

    private void Update()
    {
        handlePlayerRotate();
        handlePlayerMove();
    }

    void handlePlayerMove()
    {
        float horizontalAxis = _joystick.Horizontal;
        float verticalAxis = _joystick.Vertical;

        Vector3 moveVector = (transform.right.normalized * horizontalAxis + transform.forward.normalized * verticalAxis);
        characterController.Move(moveVector * Time.deltaTime * _movementSpeed);
    }

    void handlePlayerRotate()
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
                    setHeadRotation(newHeadRotation);
            }
        }
    }

    void setHeadRotation(Quaternion newRotation)
    {
        _headObject.transform.rotation = newRotation;
    }

}
