using System.Collections;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    private TMP_Text notifaction;

    void Awake()
    {
        notifaction = GameObject.Find("Notification").GetComponent<TMP_Text>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            // Subscribe to connection failure events
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        if (IsServer)
        {
            // Handle invalid session codes or other server-side errors
            Debug.Log("Server started successfully.");
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            // Unsubscribe from events to avoid memory leaks
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }

        Debug.Log("PlayerNetwork: OnNetworkDespawn() called");
    }

    /// <summary>
    /// Handles client disconnection events.
    /// </summary>
    /// <param name="clientId">The ID of the disconnected client.</param>
    private void OnClientDisconnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.LogError("Disconnected from the server. Please check your connection or session code.");
        }
        else
        {
            Debug.Log($"Client {clientId} disconnected.");
            //Pause the game
            GameManager.Instance.paused = true;
            notifaction.text = "Opponent disconnected. Game paused.";
        }
    }

    /// <summary>
    /// Handles invalid session codes or connection failures.
    /// </summary>
    public void HandleInvalidSessionCode()
    {
        Debug.LogError("Invalid session code. Please try again.");
    }

    /// <summary>
    /// Handles connection failures.
    /// </summary>
    public void HandleConnectionFailure()
    {
        Debug.LogError("Failed to connect to the server. Please check your network connection.");
    }
}