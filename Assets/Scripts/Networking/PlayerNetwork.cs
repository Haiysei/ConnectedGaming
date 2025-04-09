using System.Collections;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    private TMP_Text notification;
    private const int MaxPlayers = 2; // Maximum number of players allowed

    void Awake()
    {
        notification = GameObject.Find("Notification").GetComponent<TMP_Text>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Subscribe to client connection and disconnection events
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

            Debug.Log("Server started successfully.");
        }

        if (IsClient)
        {
            // Subscribe to disconnection events
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            // Unsubscribe from events to avoid memory leaks
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }

        if (IsClient)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }

        Debug.Log("PlayerNetwork: OnNetworkDespawn() called");
    }

    /// <summary>
    /// Handles client connection events.
    /// </summary>
    /// <param name="clientId">The ID of the connected client.</param>
    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count > MaxPlayers)
        {
            Debug.LogWarning($"Client {clientId} was rejected because the maximum player limit ({MaxPlayers}) was reached.");
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
        else
        {
            Debug.Log($"Client {clientId} connected. Total players: {NetworkManager.Singleton.ConnectedClients.Count}");
        }
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
            Debug.Log($"Client {clientId} disconnected. Total players: {NetworkManager.Singleton.ConnectedClients.Count - 1}");
            // Pause the game if an opponent disconnects
            GameManager.Instance.paused = true;
            notification.text = "Opponent disconnected. Game paused.";
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