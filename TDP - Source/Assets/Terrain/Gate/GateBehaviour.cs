using UnityEngine;
using System.Collections;

public class GateBehaviour : MonoBehaviour {

	bool completed = false;

	void OnTriggerEnter2D (Collider2D other) {
		if (!completed) {
			StartCoroutine (CloseGateAndSpeakToPlayer ());
			completed = true;
		}
	}

	IEnumerator CloseGateAndSpeakToPlayer() {
		GetComponent <Animator> ().SetTrigger ("MoveUp");
		IEnumerator coroutine = TextNotifications.Create (TextNotifications.NotificationTypes.NORMAL, Color.green, "OH NO YOU DON'T!");
		StartCoroutine (coroutine);
		yield return new WaitForSeconds (2);
		StopCoroutine (coroutine);
		TextNotifications.Clear ();
	}

}
