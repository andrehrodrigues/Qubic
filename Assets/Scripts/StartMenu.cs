using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour {

    public Canvas menu;
    public Button startText;
    public Button exitText;

	// Use this for initialization
	void Start () {
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ExitPress() {


    }

    public void StartLevel() {
        Application.LoadLevel("main");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
