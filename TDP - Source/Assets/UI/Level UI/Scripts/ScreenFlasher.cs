using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFlasher : MonoBehaviour {

	public static ScreenFlasher instance;

	void Awake() {
		instance = this;
	}

	private bool completed = true;

	public void Flash(float time) {
		if (completed)
			StartCoroutine (FlashColor (time));
	}

	IEnumerator FlashColor(float time) {
		completed = false;
		float initialTime = Time.time;
		Image uiImage = GetComponent <Image> ();
		float maxAlpha = .5f;
		//Ranges between 0 and 1 to represent the degree of completion.  
		float cPercentage = 0;
		while (cPercentage < 1) {
			//Basic absolute value function (Go math! :))
			if (cPercentage < 0.5f)
				uiImage.color = new Color (uiImage.color.r, uiImage.color.g, uiImage.color.b, maxAlpha * 2 * cPercentage);
			else 
				uiImage.color = new Color (uiImage.color.r, uiImage.color.g, uiImage.color.b, maxAlpha - maxAlpha * cPercentage);
			cPercentage = (Time.time - initialTime) / time;
			yield return null;
		}
		//Make sure that the color is reset.  
		uiImage.color = new Color (uiImage.color.r, uiImage.color.g, uiImage.color.b, 0);
		completed = true;
	}

}
