using System.Collections;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public GameObject CurrencyAmount;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public bool PurchaseItem(float price)
    {
        TMP_Text currencyText = CurrencyAmount.GetComponent<TMP_Text>();
        float.TryParse(currencyText.text, out float availableCurrency);
        if (availableCurrency >= price)
        {
            currencyText.text = (availableCurrency - price).ToString();
            return true;
        }

        return false;
    }
}
