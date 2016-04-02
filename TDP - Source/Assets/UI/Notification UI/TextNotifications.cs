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
	private static bool initialized = false;
	private static bool currentlyInUse = false;
	private static IEnumerator coroutine;

	public void Initialize() {
		text = GetComponent <Text> ();
		initialized = true;
	}

	public static IEnumerator Create(NotificationTypes type, Color color, string message) {
		if (initialized) {
			if (currentlyInUse) {
				Debug.LogError ("Text is already in use error!");
				Clear ();
			}
			currentlyInUse = true;

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
		text.text = "";
		text.color = Color.black;
		currentlyInUse = false;
	}

}
