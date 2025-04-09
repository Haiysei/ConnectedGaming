using UnityEngine;
using Unity.Netcode;

public class PingManager : NetworkBehaviour
{
    // Stores the time (in seconds) when we sent the ping request.
    private float pingRequestTime = 0f;

    // The most recently measured ping value in milliseconds.
    public float latestPingMs = 0f;

    // How frequently (in seconds) to perform ping measurement.
    public float pingInterval = 5f;
    private float pingTimer = 0f;

    void Update()
    {
        // Only the client should measure ping.
        if (!IsClient) return;

        pingTimer += Time.deltaTime;
        if (pingTimer >= pingInterval)
        {
            RequestPing();
            pingTimer = 0f;
        }
    }

    //Request a ping from the server.
    private void RequestPing()
    {
        // Record the time at which the ping was sent.
        pingRequestTime = Time.realtimeSinceStartup;
        // Call the ServerRPC to request a pong.
        SendPingServerRpc();
    }

    //Send a ping request to the server.
    [ServerRpc(RequireOwnership = false)]
    private void SendPingServerRpc(ServerRpcParams rpcParams = default)
    {
        // Immediately send a pong response back to the client.
        SendPongClientRpc(rpcParams.Receive.SenderClientId);
    }

    //Send a pong response to the client that requested it.
    [ClientRpc]
    private void SendPongClientRpc(ulong targetClientId, ClientRpcParams rpcParams = default)
    {
        // This message might be received by multiple clients, so ensure we only process
        // the ping response on the client that originally sent it.
        if (!IsOwner) return;

        // Calculate round-trip time in milliseconds.
        float pingTime = (Time.realtimeSinceStartup - pingRequestTime) * 1000f;
        latestPingMs = pingTime;
        Debug.Log($"Ping: {latestPingMs:F0} ms");
    }
}
