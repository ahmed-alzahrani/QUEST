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
	
    void StartGame()
    {
        SceneManager.LoadScene("GameTable" , LoadSceneMode.Single);
    }
}
