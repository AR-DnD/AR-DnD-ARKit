using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Back : MonoBehaviour {
	public AnimationClip fadeColorAnimationClip;

	// Use this for initialization
	void Start () {

	}

	public void BackButtonClicked() {
		//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
//		Invoke ("LoadDelayed", fadeColorAnimationClip.length * .5f);
		SceneManager.LoadScene (0);
	}


	public void LoadDelayed()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.

		//Hide the main menu UI element


		//Load the selected scene, by scene index number in build settings
		SceneManager.LoadScene (0);
	}

	// Update is called once per frame
	void Update () {

	}
}
