using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class PlayerAbilities : MonoBehaviour
{
    PlayerInputs input;
    [SerializeField] private float[] abilityCooldowns = { 10f, 10f, 10f, 10f }; //An array to hold the cooldowns for the four abilities


    //Bool
    private bool[] abilityOnCooldowns = { false, false, false, false };

    void Awake()
    {
        input = new PlayerInputs();
    }

    void OnEnable()
    {
        input.AbilityWheel.Enable();

        input.AbilityWheel.Ability01.performed += ActivateAbility01;
        input.AbilityWheel.Ability02.performed += ActivateAbility02;
        input.AbilityWheel.Ability03.performed += ActivateAbility03;
        input.AbilityWheel.Ability04.performed += ActivateAbility04;
    }

    void OnDisable()
    {
        input.AbilityWheel.Disable();

        input.AbilityWheel.Ability01.performed -= ActivateAbility01;
        input.AbilityWheel.Ability02.performed -= ActivateAbility02;
        input.AbilityWheel.Ability03.performed -= ActivateAbility03;
        input.AbilityWheel.Ability04.performed -= ActivateAbility04;
    }

    public void ActivateAbilities(int abilityIndex)
    {
        if (AbilityWheel.Instance.IsAbilityWheelActive) //Check to see if the Ability wheel is open
        {
            if (abilityOnCooldowns[abilityIndex]) //Check to see if any abilities have already been activated and are on a cooldown
            {
                Debug.Log("Ability " + (abilityIndex + 1) + " Is on cooldown");
                StartCoroutine(UIManager.Instance.AbilityCooldownFlash(abilityIndex));
                return;
            }

            //if not, activate the ability and start the cooldown coroutine
            Debug.Log("Ability " + (abilityIndex + 1) + " Has been activated");
            StartCoroutine(AbilityCooldown(abilityIndex, abilityCooldowns[abilityIndex]));
        }
    }

    private void ActivateAbility01(InputAction.CallbackContext context)
    {
        //Call the above method with an index of 0 
        ActivateAbilities(0);
    }
    private void ActivateAbility02(InputAction.CallbackContext context)
    {
        //Call the above method with an index of 1
        ActivateAbilities(1);
        
    }
    private void ActivateAbility03(InputAction.CallbackContext context)
    {
        //Call the above method with an index of 2 
        ActivateAbilities(2);
    }

    private void ActivateAbility04(InputAction.CallbackContext context)
    {
        //Call the above method with an index of 3 
        ActivateAbilities(3);
    }

    private IEnumerator AbilityCooldown(int index, float cooldown)
    {
        abilityOnCooldowns[index] = true;

        Debug.Log("Cooldown Started for ability: " + (index + 1));

        float timeremaning = cooldown;

        while (timeremaning > 0)
        {
            float fillamount = timeremaning / cooldown;

            UIManager.Instance.UpdateAbilityFill(index, fillamount);

            timeremaning -= Time.deltaTime;
            yield return null;
        }

        //Reset the fill amount
        UIManager.Instance.UpdateAbilityFill(index, 1f);
        
        abilityOnCooldowns[index] = false;
       
        Debug.Log("Cooldown ended for ability: " + (index +1));
    }


}
