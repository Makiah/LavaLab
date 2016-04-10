using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LavaNotifier : MonoBehaviour {

	public static LavaNotifier instance;

	void Awake() {
		instance = this;
	}

	void Start() {
		StartCoroutine (LavaNotification ());
	}

	IEnumerator LavaNotification() {
		float totalTime = 40;
		float initialTime = Time.time;
		Slider lavaSlider = transform.FindChild ("Lava Slider").GetComponent <Slider> ();
		Image lavaFill = lavaSlider.transform.FindChild ("Fill Area").GetChild (0).GetComponent<Image> ();

		while (Time.time - initialTime <= totalTime) {
			//Linearly interpolates between green and red over the given time.  
			lavaFill.color = Color.Lerp(Color.green, Color.red, ((Time.time - initialTime) / totalTime));
			//Slider value (between 0 and 1).  
			lavaSlider.value = (Time.time - initialTime) / totalTime;

			yield return null;
		}
	}

}
