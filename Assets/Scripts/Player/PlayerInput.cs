﻿using UnityEngine;
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
        input.BloomingBlows.performed += _ =>
        {
            inputState.bloomingblows = true;
            onInputStateChanged?.Invoke(inputState);
        };
        input.BloomingBlows.canceled += _ =>
        {
            inputState.bloomingblows = false;
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
        //Reset
        input.Reset.performed += _ =>
        {
            onReset?.Invoke();
        };
    }
    public delegate void OnReset();
    public event OnReset onReset;

    private void OnDisable()
    {
        playerActionControls.Disable();
    }

}
