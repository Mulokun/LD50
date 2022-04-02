using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Flags]
    enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,

        All = -1
    }

    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField, Range(3, 20)] private float speed;
    [SerializeField, Range(0.001f, 0.1f)] private float acceleration;

    private InputAction actionUp;
    private InputAction actionDown;
    private InputAction actionLeft;
    private InputAction actionRight;

    private Direction currentInput = 0;
    private Vector2 requestedMovement = Vector2.zero;
    private Vector3 currentMovement = Vector3.zero;

    private void Awake()
    {
        actionUp = inputAsset.FindAction("Character/Up");
        actionUp.started += (InputAction.CallbackContext _) => RequestMovement(Direction.Up, true);
        actionUp.canceled += (InputAction.CallbackContext _) => RequestMovement(Direction.Up, false);
        actionUp.Enable();

        actionDown = inputAsset.FindAction("Character/Down");
        actionDown.started += (InputAction.CallbackContext _) => RequestMovement(Direction.Down, true);
        actionDown.canceled += (InputAction.CallbackContext _) => RequestMovement(Direction.Down, false);
        actionDown.Enable();

        actionLeft = inputAsset.FindAction("Character/Left");
        actionLeft.started += (InputAction.CallbackContext _) => RequestMovement(Direction.Left, true);
        actionLeft.canceled += (InputAction.CallbackContext _) => RequestMovement(Direction.Left, false);
        actionLeft.Enable();

        actionRight = inputAsset.FindAction("Character/Right");
        actionRight.started += (InputAction.CallbackContext _) => RequestMovement(Direction.Right, true);
        actionRight.canceled += (InputAction.CallbackContext _) => RequestMovement(Direction.Right, false);
        actionRight.Enable();
    }

    private void Update()
    {
        Vector3 movement = Vector3.Normalize(new Vector3(requestedMovement.x, 0, requestedMovement.y)) * speed * Time.deltaTime;
        currentMovement = Vector3.Lerp(currentMovement, movement, acceleration);
        transform.position += currentMovement;
    }

    private void RequestMovement(Direction direction, bool active)
    {
        if (active)
        {
            currentInput |= direction;
            switch(direction)
            {
                case Direction.Up:
                    requestedMovement.y = 1;
                    break;
                case Direction.Down:
                    requestedMovement.y = -1;
                    break;
                case Direction.Left:
                    requestedMovement.x = -1;
                    break;
                case Direction.Right:
                    requestedMovement.x = 1;
                    break;
            }
        }
        else
        {
            currentInput &= Direction.All ^ direction;
            switch (direction)
            {
                case Direction.Up:
                    requestedMovement.y = (currentInput & Direction.Down) != 0 ? -1 : 0;
                    break;
                case Direction.Down:
                    requestedMovement.y = (currentInput & Direction.Up) != 0 ? 1 : 0;
                    break;
                case Direction.Left:
                    requestedMovement.x = (currentInput & Direction.Right) != 0 ? 1 : 0;
                    break;
                case Direction.Right:
                    requestedMovement.x = (currentInput & Direction.Left) != 0 ? -1 : 0;
                    break;
            }
        }
    }
}
