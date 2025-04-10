
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ItemPurchase : MonoBehaviour
{
    public StoreItem Item;
    public void DownloadItem()
    {
        if (ShopManager.Instance.PurchaseItem(Item.Price))
        {
            GetComponent<Button>().enabled = false;
            transform.GetChild(0).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            string internalUrl = Item.ThumbnailUrl.Split("firebasestorage.app/")[1];
            string filename = Item.Name.Replace(" ", "");
            string filepath = Path.Combine(Application.persistentDataPath, filename + "." + internalUrl.Split(".")[1]);
            FirebaseStorageManager.Instance.DownloadToFile(internalUrl, filepath);
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
}