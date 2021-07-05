using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerHome : MonoBehaviour
{
	// load main menu
    public void LoadMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	// restart game or start game
	public void Reload()
	{
		SceneManager.LoadScene(1);
	}

	// load how to play page
	public void HowToPlay()
	{
		SceneManager.LoadScene(2);
	}

	// load options page
	public void Options()
	{
		SceneManager.LoadScene(3);
	}

	// load credits page
	public void Credits()
	{
		SceneManager.LoadScene(4);
	}

	// quit application 
	public void Quit()
	{
		Application.Quit();
	}

}
