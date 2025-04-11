using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LoadScene : MonoBehaviour
{
    [SerializeField] GameObject shopCanvas;
    [SerializeField] GameObject shopManager;

    public void GoToScene(string sceneName)
    {
        if (sceneName == "Shop")
        {
            //Load the Shop Scene
            shopCanvas.GetComponent<Canvas>().enabled = true;
            shopManager.SetActive(true);
        }
        else
        {
            shopCanvas.GetComponent<Canvas>().enabled = false;
        }
    }
}
