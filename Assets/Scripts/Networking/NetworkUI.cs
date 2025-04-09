using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button ClientButton;
    [SerializeField] private Button HostButton;
    private UnityTransport transport;

    private void Awake()
    {
        ClientButton.onClick.AddListener(StartClient);
        HostButton.onClick.AddListener(StartHost);
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Client connected successfully.");
        }
        else
        {
            Debug.LogError("Failed to connect as a client. Please check your connection or session code.");
        }
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log($"Server started listening on {transport.ConnectionData.ServerListenAddress} and port {transport.ConnectionData.Port}");
        CheckIfRunningLocally();
    }

    private void CheckIfRunningLocally()
    {
        if (transport.ConnectionData.ServerListenAddress == "127.0.0.1")
        {
            Debug.LogWarning("Server is listening locally (127.0.0.1) ONLY!");
        }
    }

    private void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("Game is running in build mode");
        }
        else if (Application.isEditor)
        {
            Debug.Log("Game is running in Unity Editor");
        }
        else if (Application.platform == RuntimePlatform.LinuxServer && Application.isBatchMode &&
                  !Application.isEditor)
        {
            Debug.Log("Game is running on Linux Dedicated Server");
        }

        if (NetworkManager.Singleton != null)
        {
            Debug.Log($"UTP working with IP:{transport.ConnectionData.Address} and Port:{transport.ConnectionData.Port}");
        }
    }


}
