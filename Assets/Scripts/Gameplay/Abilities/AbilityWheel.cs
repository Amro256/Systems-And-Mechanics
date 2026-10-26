using UnityEngine;
using UnityEngine.InputSystem;
using System;
using NUnit.Framework.Internal;

public class AbilityWheel : MonoBehaviour
{
    public static AbilityWheel Instance { get; private set; }
    PlayerInputs input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isAbilityWheelOpen = false;

    public bool IsAbilityWheelActive
    {
        get { return isAbilityWheelOpen; }
    }

    void Awake()
    {
        input = new PlayerInputs();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    void Start()
    {
        UIManager.Instance.UpdateWheelStatus(isAbilityWheelOpen);
    }

    void OnEnable()
    {
        input.AbilityWheel.Enable();

        input.AbilityWheel.Activate.performed += OnActivate;
        input.AbilityWheel.Activate.canceled += OnActivate;

    }

    void OnDisable()
    {
        input.AbilityWheel.Disable();
        input.AbilityWheel.Activate.performed -= OnActivate;
        input.AbilityWheel.Activate.canceled -= OnActivate;
    }


    //Input methods

    private void OnActivate(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (context.started) return;
                break;
            case InputActionPhase.Performed:
                if (context.performed)
                {
                    isAbilityWheelOpen = true;
                    UIManager.Instance.UpdateWheelStatus(isAbilityWheelOpen);
                    Debug.Log("Wheel Activated!");
                    //Play animation here
                    AnimationManager.Instance.StartAnimation("IsAbilityWheelActive");

                }
                break;
            case InputActionPhase.Canceled:
                if (context.canceled)
                {
                    isAbilityWheelOpen = false;
                    UIManager.Instance.UpdateWheelStatus(isAbilityWheelOpen);
                    Debug.Log("Wheel Deactivated");
                    //Deactivate Wheel
                    AnimationManager.Instance.StopAnimation("IsAbilityWheelActive");
                }
                break;
        }
    }

}
