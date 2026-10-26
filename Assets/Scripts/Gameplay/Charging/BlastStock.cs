using UnityEngine;
using TMPro;

public class BlastStock : MonoBehaviour
{
    public static BlastStock instance;
    //Variables
    [Header("Blast Stock")]
    [SerializeField] private float chargeTime = 0.05f;
    [SerializeField] private int maxBlastStocks = 5;
    private bool hasBlastStock = false;
    [SerializeField] private int blastStockValue = 0;
    private bool outOfBlastStock = true;

    #region Properties
    public bool OutOfBlastStock
    {
        get { return outOfBlastStock; }
    }

    public bool HasBlastStock
    {
        get { return hasBlastStock; }
    }

    public float ChargeTime
    {
        get { return chargeTime; }
    }
    #endregion

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void UpdateBlastStockValue(TMP_Text blastStockText)
    {
        if (blastStockValue < maxBlastStocks)
        {
            //Add a bool here to say that the player has blast Stocks
            blastStockValue++;
            hasBlastStock = true;
            Debug.Log("Player has: " + blastStockValue + " Blast stock");
            blastStockText.text = blastStockValue.ToString();
        }
        if (blastStockValue >= maxBlastStocks)
        {
            outOfBlastStock = false;
        }
    }

    public void DecreaseBlastStockValue(int amount, TMP_Text blastStockText)
    {
        blastStockValue -= amount;

        blastStockText.text = blastStockValue.ToString();
    }
}
