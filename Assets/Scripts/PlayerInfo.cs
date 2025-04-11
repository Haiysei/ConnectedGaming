using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : NetworkBehaviour
{
    private string _playerIcon;
    public string userId = "client";

    public string playerIcon
    {
        get => _playerIcon;
        set
        {
            _playerIcon = value;
            onIconChange();
        }
    }

    void onIconChange()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, playerIcon) + ".png";
        //Convert image at path to Texture2D
        byte[] imageData = System.IO.File.ReadAllBytes(path);
        Texture2D icon = new Texture2D(200, 200);
        icon.LoadImage(imageData);

        byte[] iconData = icon.EncodeToPNG();
        SetPlayerIconServerRpc(iconData, userId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerIconServerRpc(byte[] iconData, string userId)
    {
        // Send the icon data to all clients
        SetPlayerIconClientRpc(iconData, userId);
    }

    [ClientRpc]
    public void SetPlayerIconClientRpc(byte[] iconData, string userId)
    {
        // Deserialize the byte array back into a Texture2D
        Texture2D texture = new Texture2D(200, 200);
        if (texture.LoadImage(iconData))
        {
            Debug.Log("Player icon received and successfully loaded.");
            if (userId == "host")
                GameObject.Find("HostIcon").GetComponent<RawImage>().texture = texture;
            else
                GameObject.Find("ClientIcon").GetComponent<RawImage>().texture = texture;
        }
        else
        {
            Debug.LogError("Failed to load player icon from received data.");
        }
    }
}
