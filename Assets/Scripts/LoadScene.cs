using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LoadScene : MonoBehaviour
{
    [SerializeField] GameObject shopCanvas;
    [SerializeField] GameObject analyticsCanvas;
    [SerializeField] GameObject shopManager;

    public void GoToScene(string sceneName)
    {
        if (sceneName == "Shop")
        {
            //Load the Shop Scene
            shopCanvas.GetComponent<Canvas>().enabled = true;
            shopManager.SetActive(true);

            //Load the Store
            if (shopManager.GetComponent<FirebaseStorageManager>().loaded == false)
            {
                FirebaseStorageManager.Instance.DownloadToByteArray("StoreItems.xml", FirebaseStorageManager.DownloadType.MANIFEST);
                shopManager.GetComponent<FirebaseStorageManager>().loaded = true;
            }
        }
        else
        {
            shopCanvas.GetComponent<Canvas>().enabled = false;
        }

        if (sceneName == "Analytics")
        {
            //Load the Analytics Scene
            analyticsCanvas.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            analyticsCanvas.GetComponent<Canvas>().enabled = false;
        }
    }
}
