using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LoadScene : MonoBehaviour
{
    [SerializeField] GameObject shopCanvas;
    public void GoToScene(string sceneName)
    {
        if (sceneName == "Shop")
        {
            //Load the Shop Scene
            shopCanvas.SetActive(true);
        }
        else
        {
            shopCanvas.SetActive(false);
        }
    }
}
