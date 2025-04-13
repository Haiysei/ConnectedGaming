using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Data.Common;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

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

    public async void GetGameState()
    {
        DocumentReference docRef = db.Collection("gamestate").Document("states");
        string gameState = "";
        await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
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
                    gameState = snapshot.GetValue<string>("state");
                    print("Game State: " + gameState);
                }
                catch (System.Exception ex)
                {
                    gameState = "";
                }
            }
            else
            {
                gameState = "";
            }
        });
        GameObject.Find("InputField").GetComponent<InputField>().text = gameState;
    }

    public async void SetGameState()
    {
        DocumentReference docRef = db.Collection("gamestate").Document("states");
        string state = GameObject.Find("InputField").GetComponent<InputField>().text;
        await docRef.SetAsync(new Dictionary<string, object>
        {
            { "state", state }
        }, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error setting document: " + task.Exception);
                return;
            }
            Debug.Log("Game State set to: " + state);
        });
    }

    public async Task<int> GetAnalytics(string field, string document)
    {
        DocumentReference docRef = db.Collection("analytics").Document(document);
        int count = 0;
        await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
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
                    count = snapshot.GetValue<int>(field);
                    print("DLC Analytics: " + count);
                }
                catch (System.Exception ex)
                {
                    count = 0;
                }
            }
            else
            {
                count = 0;
            }
        });
        return count;
    }

    public async void UpdateAnalytics(string field, string document)
    {

        DocumentReference docRef = db.Collection("analytics").Document(document);
        int count = await GetAnalytics(field, document);
        count++;
        await docRef.SetAsync(new Dictionary<string, object>
        {
            { field, count }
        }, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error setting document: " + task.Exception);
                return;
            }
            Debug.Log("DLC Analytics set to: " + count);
        });
    }
}
