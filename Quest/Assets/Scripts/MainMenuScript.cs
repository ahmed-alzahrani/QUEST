using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Button button;

	// Use this for initialization
	void Start ()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(StartGame);
	}

    //CHANGE THE NAMES ACCORDINGLY
    void StartGame()
    {
        if (gameObject.name == "HotSeat")
        {
            SceneManager.LoadScene("GameTable", LoadSceneMode.Single);
        }
        else if (gameObject.name == "Lobby")
        {
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
        else if (gameObject.name == "OnlineGame")
        {
            SceneManager.LoadScene("OnlineGameTable", LoadSceneMode.Single);
        }
        else if (gameObject.name == "Credits")
        {
            SceneManager.LoadScene("Credits", LoadSceneMode.Single);
        }
    }
}
