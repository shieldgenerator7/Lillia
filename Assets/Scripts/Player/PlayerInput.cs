using UnityEngine;
using static PlayerActionControls;

public class PlayerInput : MonoBehaviour
{
    [Tooltip("Movement values that are nonzero and at or below this threshold are ignored")]
    public float ignoreThreshold = 0.1f;

    private PlayerActionControls playerActionControls;

    private InputState inputState;
    public delegate void OnInputStateChanged(InputState inputState);
    public event OnInputStateChanged onInputStateChanged;

    private bool magWasValid = false;

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
            //ignore slight thumbstick movements
            float magnitude = inputState.movementDirection.magnitude;
            if (magnitude > ignoreThreshold
            || (magnitude == 0 && magWasValid))
            {
                if (magnitude > ignoreThreshold)
                {
                    magWasValid = true;
                }
                else if (magnitude == 0)
                {
                    magWasValid = false;
                }
                onInputStateChanged?.Invoke(inputState);
            }
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
        //BloomingBlows
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
        //SwirlSeed
        input.SwirlSeed.performed += _ =>
        {
            inputState.swirlseed = true;
            onInputStateChanged?.Invoke(inputState);
        };
        input.SwirlSeed.canceled += _ =>
        {
            inputState.swirlseed = false;
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
        //Pause
        input.Pause.performed += _ =>
        {
            onPause?.Invoke();
        };
        //Reset
        input.Reset.performed += _ =>
        {
            onReset?.Invoke();
        };
        //Prev Level
        input.PrevLevel.performed += _ =>
        {
            onPrevLevel?.Invoke();
        };
        //Next Level
        input.NextLevel.performed += _ =>
        {
            onNextLevel?.Invoke();
        };
    }
    public delegate void OnUIAction();
    public event OnUIAction onPause;
    public event OnUIAction onReset;
    public event OnUIAction onPrevLevel;
    public event OnUIAction onNextLevel;

    private void OnDisable()
    {
        playerActionControls.Disable();
    }

}
