using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }
    private bool isInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        isInitialized = true;
    }

    public void PurchasedDLC(string dlcName)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics not initialized. DLC purchase event not sent.");
            return;
        }

        CustomEvent dlcEvent = new CustomEvent("purchasedDlc")
        {
            { "dlc", dlcName },
        };

        AnalyticsService.Instance.RecordEvent(dlcEvent);
        AnalyticsService.Instance.Flush();
        FirestoreManager.Instance.UpdateAnalytics(dlcName, "purchasedDlc");
        Debug.Log($"DLC purchase event sent: {dlcName}");
    }

    public void MatchStart()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics not initialized. Match start event not sent.");
            return;
        }

        AnalyticsService.Instance.RecordEvent("matchStart");
        AnalyticsService.Instance.Flush();
        FirestoreManager.Instance.UpdateAnalytics("matchStart", "matchStates");
        Debug.Log("Match start event sent.");
    }

    public void MatchEnd()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Analytics not initialized. Match end event not sent.");
            return;
        }

        AnalyticsService.Instance.RecordEvent("matchEnd");
        AnalyticsService.Instance.Flush();
        FirestoreManager.Instance.UpdateAnalytics("matchEnd", "matchStates");
        Debug.Log("Match end event sent.");
    }
}
