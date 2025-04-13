
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.Netcode;
using System.Threading.Tasks;

public class ItemPurchase : MonoBehaviour
{
    public StoreItem Item;
    bool confirmed = false;

    string userId = "client";

    public async Task Start()
    {
        //If user is host
        if (NetworkManager.Singleton.IsServer)
            userId = "host";

        bool hasItem = await FirestoreManager.Instance.GetDlcOwnership(userId, Item.Name.Replace(" ", ""));

        if (hasItem)
        {
            DisablePurchase();
            Download();
        }
    }
    public void DownloadItem()
    {
        if (!confirmed)
        {
            TMP_Text priceText = transform.GetChild(3).GetComponent<TMP_Text>();
            priceText.text = "Confirm?";
            StartCoroutine(RevertTextAfterDelay(priceText));
            confirmed = true;
            return;
        }
        if (ShopManager.Instance.PurchaseItem(Item.Price))
        {
            DisablePurchase();
            Download();
            FirestoreManager.Instance.SetDlcOwnership(userId, Item.Name.Replace(" ", ""), true);
            AnalyticsManager.Instance.PurchasedDLC(Item.Name.Replace(" ", ""));
            confirmed = false;
        }
        else
        {
            TMP_Text priceText = transform.GetChild(3).GetComponent<TMP_Text>();
            priceText.text = "Insufficient Balance";
            Debug.LogWarning("Insuffient balance!!");

            StartCoroutine(RevertTextAfterDelay(priceText));
        }

    }

    private IEnumerator RevertTextAfterDelay(TMP_Text priceText)
    {
        yield return new WaitForSeconds(2f);
        priceText.text = Item.Price.ToString();
        confirmed = false;
    }

    void Download()
    {
        transform.GetChild(5).GetComponent<Button>().interactable = true;
        //Check if file already exists at C:/Users/user/AppData/LocalLow/DefaultCompany/UnityChess\
        if (File.Exists(Path.Combine(Application.persistentDataPath, Item.Name.Replace(" ", "") + ".png")))
        {
            Debug.Log("File already exists, no need to download again.");
            return;
        }
        string filepath = System.IO.Path.Combine(Application.persistentDataPath, Item.Name.Replace(" ", "")) + ".png";
        FirebaseStorageManager.Instance.DownloadToFile("dlc/" + Item.Name.Replace(" ", "_").ToLower() + ".png", filepath);
    }

    void DisablePurchase()
    {
        GetComponent<Button>().enabled = false;
        transform.GetChild(0).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    public void EquipItem()
    {
        //Get Network Player object of the player
        PlayerInfo playerNetwork = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerInfo>();

        playerNetwork.userId = userId;
        playerNetwork.playerIcon = Item.Name.Replace(" ", "");
    }
}