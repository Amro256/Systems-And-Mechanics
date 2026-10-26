using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharge : MonoBehaviour
{
    PlayerInputs input;

    #region Variables
    //Variables
    [SerializeField] private int regularChargeSpeed = 5;
    [SerializeField] private int sparkingModeChargeSpeed = 10;
    [SerializeField] private float currentKi;
    [SerializeField] private float currentSparkingKi;
    private float sparkingModeDuration = 20f;
    private float sparkingModeKiDrainRate = 10f;
    private float startingKi = 0;
    private float baseMaxKi = 500f;
    private float sparkingModeKi = 700f;
    #endregion

    //Bools
    private bool isCharging = false;
    private bool inSparkingMode;

    void Awake()
    {
        input = new PlayerInputs();
    }

    void OnEnable()
    {
        input.Charging.Enable();

        input.Charging.Charge.performed += OnCharge;
        input.Charging.Charge.canceled += OnCharge;
    }

    void OnDisable()
    {
        input.Charging.Disable();

        input.Charging.Charge.performed -= OnCharge;
        input.Charging.Charge.canceled -= OnCharge;
    }

    void Start()
    {
        //Set the starting ki amount
        currentKi = startingKi;
        currentSparkingKi = 0f;
    }

    //Input Action
    private void OnCharge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCharging = true;
            Debug.Log("Currently charging Ki");
        }
        else if (context.canceled)
        {
            isCharging = false;
            Debug.Log("No longer charging");
        }
    }

    void Update()
    {
        Charging();
    }

    #region Charging Method
    private void Charging()
    {
        if (isCharging && !inSparkingMode) //Sparking mode bool value is false
        {
            if (currentKi < baseMaxKi) //Check to see if the player's current ki is less than their max ki
            {
                Debug.Log("Regular Ki Charging");
                currentKi += regularChargeSpeed * Time.deltaTime;
                currentKi = Mathf.Min(currentKi, baseMaxKi); //Ensures the current ki doesnt go above the base max ki value
                UIManager.Instance.UpdateKI(currentKi, baseMaxKi);
            }
            else //Else, if the current ki is greater or equal to the max ki, start the sparking mode charging
            {
                //Check to see if the player has a blast stocks first before allowing the player to charge into sparking
                if (BlastStock.instance.HasBlastStock)
                {
                    Debug.Log("Sparking Mode Ki Charging");
                    currentSparkingKi += sparkingModeChargeSpeed * Time.deltaTime;
                    currentSparkingKi = Mathf.Min(currentSparkingKi, sparkingModeKi); //Ensures the current ki doesnt go above the base max ki value
                    UIManager.Instance.UpdateSparkingModeKi(currentSparkingKi, sparkingModeKi);
                }
                else //Otherwise, the player currently has no blast stocks to enter sparking mode
                {
                    Debug.Log("No blast stock available to enter Sparking mode");
                }

                if (currentSparkingKi >= sparkingModeKi) //If the current (Sparking) ki is greater or equal to the sparking mode Ki, the player has entered into sparking mode
                {
                    EnteredSparkingMode();
                }
              
            }
        }
    }
    #endregion

    private void EnteredSparkingMode()
    {
        Debug.Log("Entered SPARKING Mode");
        //Decrease the player's blast stock by 1
        BlastStock.instance.DecreaseBlastStockValue(1, UIManager.Instance.blastStockText);
        //Set the current Ki to current sparking KI
        currentKi = currentSparkingKi;

        //Set some bools to true
        inSparkingMode = true;
        isCharging = false;

        //Start the coroutine to decrease the bar when the player enters sparking mode
        StartCoroutine(SparkingDuration());

    }

    private void OutOfSparkingMode()
    {
        Debug.Log("Out of Sparking Mode");
        inSparkingMode = false;
        isCharging = false;

        //Reset the sparking ki amount
        currentSparkingKi = 0f;
    }

    public IEnumerator SparkingDuration()
    {
        float timeremaning = sparkingModeDuration;

        while (timeremaning > 0)
        {
            float fillamount = Mathf.Max(timeremaning / sparkingModeDuration);
            UIManager.Instance.KiBarFill(fillamount);

            timeremaning -= Time.deltaTime;

            //Reduce the current Ki back to the max base ki value
            currentKi -= Time.deltaTime * sparkingModeKiDrainRate;
            currentKi = Mathf.Max(currentKi, baseMaxKi); //Returns the base max ki value - prevents the current ki value from going below the base max ki value
            yield return null;
        }

        //Reset the bar fill amount and colour
        UIManager.Instance.ResetKiBar();

        //Call the above method
        OutOfSparkingMode();
    }

    
}
