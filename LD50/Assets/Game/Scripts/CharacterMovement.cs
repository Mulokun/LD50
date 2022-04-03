using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

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
    [Title("Movement Data")]
    [SerializeField] private Rigidbody currentRigidbody;
    [SerializeField, Range(3, 30)] private float speed;
    [SerializeField, Range(0.01f, 0.5f)] private float acceleration;

    private MeshRenderer mesh;

    private InputAction actionUp;
    private InputAction actionDown;
    private InputAction actionLeft;
    private InputAction actionRight;

    private Direction currentInput = 0;
    private Vector2 requestedMovement = Vector2.zero;
    private Vector3 currentMovement = Vector3.zero;
    private Vector3 oldPosition = Vector3.zero;

    private void Awake()
    {
        mesh = GetComponentInChildren<MeshRenderer>();

        if(actionUp == null)
        {
            actionUp = inputAsset.FindAction("Character/Up");
            actionUp.started += (InputAction.CallbackContext _) => RequestMovement(Direction.Up, true);
            actionUp.canceled += (InputAction.CallbackContext _) => RequestMovement(Direction.Up, false);
        }
        if (actionDown == null)
        {
            actionDown = inputAsset.FindAction("Character/Down");
            actionDown.started += (InputAction.CallbackContext _) => RequestMovement(Direction.Down, true);
            actionDown.canceled += (InputAction.CallbackContext _) => RequestMovement(Direction.Down, false);
        }
        if (actionLeft == null)
        {
            actionLeft = inputAsset.FindAction("Character/Left");
            actionLeft.started += (InputAction.CallbackContext _) => RequestMovement(Direction.Left, true);
            actionLeft.canceled += (InputAction.CallbackContext _) => RequestMovement(Direction.Left, false);
        }
        if (actionRight == null)
        {
            actionRight = inputAsset.FindAction("Character/Right");
            actionRight.started += (InputAction.CallbackContext _) => RequestMovement(Direction.Right, true);
            actionRight.canceled += (InputAction.CallbackContext _) => RequestMovement(Direction.Right, false);
        }
    }

    private void OnEnable()
    {
        actionUp.Enable();
        actionDown.Enable();
        actionLeft.Enable();
        actionRight.Enable();
    }

    private void OnDisable()
    {
        actionUp.Disable();
        actionDown.Disable();
        actionLeft.Disable();
        actionRight.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 movement = Vector3.Normalize(new Vector3(requestedMovement.x, 0, requestedMovement.y)) * speed * Time.deltaTime;
        currentMovement = transform.position - oldPosition;
        currentMovement = Vector3.Lerp(currentMovement, movement, acceleration);

        oldPosition = transform.position;
        currentRigidbody.position = (transform.position + currentMovement);
    }

    private void Update()
    {
        if (requestedMovement != Vector2.zero)
        {
            Quaternion q = Quaternion.identity;
            q.SetLookRotation(new Vector3(requestedMovement.x, 0, requestedMovement.y));
            q = Quaternion.Lerp(mesh.transform.rotation, q, 0.04f);
            mesh.transform.rotation = q;
        }
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
