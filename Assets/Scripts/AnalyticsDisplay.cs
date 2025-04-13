using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AnalyticsDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text Display;
    int Icon1;
    int Icon2;
    int Icon3;
    int matchStart;
    int matchEnd;


    void Start()
    {
        Invoke("GetAnalytics", 2);
    }

    async Task GetAnalytics()
    {
        Icon1 = await FirestoreManager.Instance.GetAnalytics("Icon1", "purchasedDlc");
        Icon2 = await FirestoreManager.Instance.GetAnalytics("Icon2", "purchasedDlc");
        Icon3 = await FirestoreManager.Instance.GetAnalytics("Icon3", "purchasedDlc");
        matchStart = await FirestoreManager.Instance.GetAnalytics("matchStart", "matchStates");
        matchEnd = await FirestoreManager.Instance.GetAnalytics("matchEnd", "matchStates");

        //Find most purchased icon, in string
        int mostPurchasedIcon = Mathf.Max(Icon1, Icon2, Icon3);
        string mostPurchasedIconString = "";
        if (mostPurchasedIcon == Icon1)
            mostPurchasedIconString = "Icon1";
        else if (mostPurchasedIcon == Icon2)
            mostPurchasedIconString = "Icon2";
        else if (mostPurchasedIcon == Icon3)
            mostPurchasedIconString = "Icon3";


        Display.text = $"Most Purchased DLC: {mostPurchasedIconString}\n" +
                       $"Matches Started: {matchStart}\n" +
                       $"Matches Ended: {matchEnd}";
    }
}
