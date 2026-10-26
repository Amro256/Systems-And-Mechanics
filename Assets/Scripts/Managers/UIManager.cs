using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } //Singleton Pattern
    [Header("Ability Wheel")]
    [SerializeField] private Image[] slotOutlines;
    [SerializeField] private Image[] coolDownSlots;
    [SerializeField] private TMP_Text wheelStatusText;

    [Header("Blast Stock")]
    [SerializeField] private Image blastStockFill;
    public TMP_Text blastStockText;


    [Header("Ki Bar")]
    [SerializeField] private Image KiBarFil;
    [SerializeField] private Color regularKiChargeColour = Color.yellow;
    [SerializeField] private Color SparkingKiChargeColour = Color.darkBlue;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    
    private void Update()
    {
        UpdateBlastStockBar();
    }


    #region  Ability System
    //Ability System Methods:
    public IEnumerator AbilityCooldownFlash(int index)
    {
        slotOutlines[index].color = Color.red;

        yield return new WaitForSeconds(0.4f);

        slotOutlines[index].color = Color.black;
    }

    public void UpdateAbilityFill(int index, float fillamount)
    {
        coolDownSlots[index].fillAmount = fillamount;
    }

    public void UpdateWheelStatus(bool value)
    {
        wheelStatusText.text = AbilityWheel.Instance.IsAbilityWheelActive.ToString();

    }
    #endregion

    #region  Ki System
    //Ki Systems Methods:
    public void UpdateBlastStockBar() //Method for updating the blast stock bar
    {
        if (BlastStock.instance.OutOfBlastStock)
        {
            //Fill the bar up over time
            blastStockFill.fillAmount += BlastStock.instance.ChargeTime * Time.deltaTime;

            if (blastStockFill.fillAmount >= 1f)
            {
                blastStockFill.fillAmount = 0f;
                BlastStock.instance.UpdateBlastStockValue(blastStockText);
            }
        }
    }
    #region Ki Bar Methods
    public void KiBarFill(float fill)
    {
        KiBarFil.fillAmount = fill;
    }

    //Method for resting the bar's fill amount and colour when sparking mode runs out
    public void ResetKiBar()
    {
        KiBarFil.fillAmount = 1; //Sets the bar to full
        KiBarFil.color = regularKiChargeColour;
    }

    public void UpdateKI(float currentki, float maxKi)
    {
        float fill = currentki / maxKi;

        KiBarFil.fillAmount = fill;
        KiBarFil.color = regularKiChargeColour;
    }

    public void UpdateSparkingModeKi(float currentki, float sparkingModeKi)
    {

        float fill = currentki / sparkingModeKi;
        KiBarFil.fillAmount = fill;

        KiBarFil.color = SparkingKiChargeColour;
    }
    #endregion

    #endregion
}
