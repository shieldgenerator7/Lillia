using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.InputSystem.Controls;
using static PlayerActionControls;

public class PlayerInput : MonoBehaviour
{
    public Transform center;

    private PlayerActionControls playerActionControls;

    private InputState inputState;
    public delegate void OnInputStateChanged(InputState inputState);
    public event OnInputStateChanged onInputStateChanged;

    //-----------------

    private void Awake()
    {
        playerActionControls = new PlayerActionControls();
    }

    private void OnEnable()
    {
        playerActionControls.Enable();
        PlayerActions input = playerActionControls.Player;
        //Movement
        input.Movement.performed += ctx =>
        {
            inputState.movementDirection = ctx.ReadValue<Vector2>();
            onInputStateChanged?.Invoke(inputState);
        };
        //Jump
        input.Jump.performed += _ =>
        {
            inputState.jump = true;
            onInputStateChanged?.Invoke(inputState);
        };
        input.Jump.canceled += _ =>
        {
            inputState.jump = false;
            onInputStateChanged?.Invoke(inputState);
        };
        //Run
        input.Run.performed += _ =>
        {
            inputState.run = true;
            onInputStateChanged?.Invoke(inputState);
        };
        input.Run.canceled += _ =>
        {
            inputState.run = false;
            onInputStateChanged?.Invoke(inputState);
        };
        //Interact
        input.Interact.performed += _ =>
        {
            inputState.interact = true;
            onInputStateChanged?.Invoke(inputState);
        };
        input.Interact.canceled += _ =>
        {
            inputState.interact = false;
            onInputStateChanged?.Invoke(inputState);
        };
        //Ability1
        input.Ability1.performed += _ =>
        {
            inputState.ability1 = true;
            onInputStateChanged?.Invoke(inputState);
        };
        input.Ability1.canceled += _ =>
        {
            inputState.ability1 = false;
            onInputStateChanged?.Invoke(inputState);
        };
        //Ability2
        input.Ability2.performed += _ =>
        {
            inputState.ability2 = true;
            onInputStateChanged?.Invoke(inputState);
        };
        input.Ability2.canceled += _ =>
        {
            inputState.ability2 = false;
            onInputStateChanged?.Invoke(inputState);
        };
    }

    private void OnDisable()
    {
        playerActionControls.Disable();
    }

    private void Update()
    {
        Vector2 centerPos = center?.position ?? transform.position;
        Vector2 prevLookDirection = inputState.lookDirection;
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector2 lookPosition = Utility.ScreenToWorldPoint(mousePos);

        inputState.lookDirection = (lookPosition - centerPos).normalized;

        if (inputState.lookDirection != prevLookDirection)
        {
            onInputStateChanged?.Invoke(inputState);
        }
    }

}
