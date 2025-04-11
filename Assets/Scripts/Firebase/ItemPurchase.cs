
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
        if (ShopManager.Instance.PurchaseItem(Item.Price))
        {
            DisablePurchase();
            Download();
            FirestoreManager.Instance.SetDlcOwnership(userId, Item.Name.Replace(" ", ""), true);
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
    }

    void Download()
    {
        string internalUrl = Item.ThumbnailUrl.Split("firebasestorage.app/")[1];
        string filename = Item.Name.Replace(" ", "");
        string filepath = Path.Combine(Application.persistentDataPath, filename + "." + internalUrl.Split(".")[1]);
        FirebaseStorageManager.Instance.DownloadToFile(internalUrl, filepath);

        transform.GetChild(5).GetComponent<Button>().interactable = true;
    }

    void DisablePurchase()
    {
        GetComponent<Button>().enabled = false;
        transform.GetChild(0).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    public void EquipItem() { }
}