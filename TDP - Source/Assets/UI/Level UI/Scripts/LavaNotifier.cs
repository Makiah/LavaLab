using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CatchCo;

public class LavaNotifier : MonoBehaviour {

	public static LavaNotifier instance;

	void Awake() {
		instance = this;
	}

	void OnEnable() {
		UIInitializationSequence.InitializeLavaNotifier += Initialize;
	}

	void OnDisable() {
		UIInitializationSequence.InitializeLavaNotifier -= Initialize;
	}

	private Slider lavaSlider;
	private Image lavaFill;

	void Initialize() {
		lavaSlider = transform.FindChild ("Lava Slider").GetComponent <Slider> ();
		lavaSlider.value = 0;
		lavaFill = lavaSlider.transform.FindChild ("Fill Area").GetChild (0).GetComponent<Image> ();
		lavaFill.color = Color.red;

		StartLavaTimer ();
	}

	private IEnumerator lavaCountdown, lavaRestart;
	private float currentValue = 0;

	[ExposeMethodInEditor] 
	public void StartLavaTimer() {
		if (lavaRestart != null)
			StopCoroutine (lavaRestart);

		lavaCountdown = LavaCountdown ();
		StartCoroutine (lavaCountdown);
	}

	private IEnumerator LavaCountdown() {
		float totalTime = 40 + 6 * LevelGenerator.instance.currentLevel;
		float initialTime = Time.time;

		while (Time.time - initialTime <= totalTime) {
			currentValue = (Time.time - initialTime) / totalTime;

			//Linearly interpolates between green and red over the given time.  
			lavaFill.color = Color.Lerp(Color.green, Color.red, currentValue);
			//Slider value (between 0 and 1).  
			lavaSlider.value = currentValue;

			yield return null;
		}

		//Make the lava rise (bring doom to the player).  
		OnCompleted ();
	}

	void OnCompleted() {
		LavaMover.instance.RiseLava ();
		Debug.Log ("Rose lava");
	}

	[ExposeMethodInEditor]
	public void RestartLavaTimer() {
		if (lavaCountdown != null)
			StopCoroutine (lavaCountdown);

		lavaRestart = RestartLava ();
		StartCoroutine (lavaRestart);
	}

	private IEnumerator RestartLava() {
		float initialValue = currentValue;
		//Time will reduce based on the fraction of the bar already shaded in.  
		float totalTime = 8 * initialValue;
		float initialTime = Time.time;

		while (Time.time - initialTime <= totalTime) {
			//Calculate the currentValue (not really sure what this does but it works so...
			currentValue = (totalTime * initialValue - (Time.time - initialTime)) / totalTime;

			//Linearly interpolates between green and red over the given time.  
			lavaFill.color = Color.Lerp(Color.green, Color.red, currentValue);
			//Slider value (between 0 and 1).  
			lavaSlider.value = currentValue;

			yield return null;
		}
	}

}
