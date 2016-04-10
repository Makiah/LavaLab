using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextNotifications : MonoBehaviour {

	void OnEnable() {
		InitializationSequence.InitializeNotificationUI += Initialize;
	}

	void OnDisable() {
		InitializationSequence.InitializeNotificationUI -= Initialize;
	}

	public enum NotificationTypes
	{
		OSCILLATING, 
		NORMAL
	}

	private static Text text = null;
	private static bool currentlyInUse = false;

	private static TextNotifications instance = null;
	private static IEnumerator coroutine = null;

	public void Initialize() {
		text = GetComponent <Text> ();
		instance = this;
	}

	public static void Create(NotificationTypes type, Color color, int fontSize, string message) {
		coroutine = CreateMessage (type, color, fontSize, message);
		instance.StartCoroutine (coroutine);
	}

	public static IEnumerator CreateMessage(NotificationTypes type, Color color, int fontSize, string message) {
		if (instance != null) {
			if (currentlyInUse) {
				Debug.LogError ("Text is already in use error!");
				Clear ();
			}
			currentlyInUse = true;

			if (fontSize == -1) {
				text.resizeTextMaxSize = 99999;
				text.resizeTextForBestFit = true;
			} else {
				text.fontSize = fontSize;
			}

			switch (type) {
			case NotificationTypes.OSCILLATING: 
				bool largeColor = true;
				text.text = message;
				while (true) {
					//Oscillate between large red text and small white text.  
					text.color = largeColor ? Color.white : color;
					text.fontSize = largeColor ? (int)(text.fontSize / 2f) : (int)(text.fontSize * 2f);
					//Switch the boolean.  
					largeColor = !largeColor;
					yield return new WaitForSeconds (.2f);
				}
			case NotificationTypes.NORMAL: 
				text.text = message;
				text.color = color;
				break;
			}
		} else {
			Debug.LogError ("TextNotifications must first be initialized");
		}
	}

	public static void Clear() {
		instance.StopCoroutine (coroutine);
		//Reset the text values.  
		text.text = "";
		text.color = Color.black;
		text.resizeTextForBestFit = false;
		text.fontSize = 80;
		//Make it so the class no longer thinks that the text is in use.  
		currentlyInUse = false;
	}

}
