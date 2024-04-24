using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void ExitButton(){
        Debug.Log("Game closed");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
	}

	public void StartButton(){
		SceneManager.LoadScene("SampleScene");
	}    

}
