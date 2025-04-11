using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Data.Common;
using System.Threading.Tasks;

public class FirestoreManager : MonoBehaviour
{
    public static FirestoreManager Instance;
    FirebaseFirestore db;

    void Awake()
    {
        FirebaseFirestore.DefaultInstance.Settings.PersistenceEnabled = false;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    async Task Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        //Example usage
        // print(await GetDlcOwnership("host", "dlc1"));
        // SetDlcOwnership("host", "dlc2", true);
    }

    //Get the ownership of the DLC
    public async Task<bool> GetDlcOwnership(string userId, string dlcId)
    {
        bool hasDlc = false;
        DocumentReference userDlc = db.Collection("users").Document(userId);
        await userDlc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting document: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                try
                {
                    hasDlc = snapshot.GetValue<bool>(dlcId);
                    print("DLC Ownership: " + hasDlc);
                }
                catch (System.Exception ex)
                {
                    hasDlc = false;
                }
            }
            else
            {
                hasDlc = false;
            }
        });

        return hasDlc;
    }

    //Set the ownership of the DLC
    public void SetDlcOwnership(string userId, string dlcId, bool status)
    {
        DocumentReference userDlc = db.Collection("users").Document(userId);
        userDlc.SetAsync(new Dictionary<string, object>
        {
            { dlcId, status }
        }, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error setting document: " + task.Exception);
                return;
            }
            Debug.Log("DLC Ownership set to: " + status);
        });
    }
}
