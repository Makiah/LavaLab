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
		TextNotifications.Create (TextNotifications.NotificationTypes.NORMAL, Color.red, 80, "OH NO YOU DON'T!");
		yield return new WaitForSeconds (2);
		TextNotifications.Clear ();

		TextNotifications.Create (TextNotifications.NotificationTypes.NORMAL, Color.red, -1, "You'll have to get through the WHOLE maze now!");
		yield return new WaitForSeconds (2);
		TextNotifications.Clear ();

		TextNotifications.Create (TextNotifications.NotificationTypes.NORMAL, Color.red, -1, "My guards should hold you off until the lava breaches your floor.  ");
		yield return new WaitForSeconds (3);
		TextNotifications.Clear ();

		TextNotifications.Create (TextNotifications.NotificationTypes.NORMAL, Color.red, -1, "Actually, why don't we make this a bit more interesting...");
		yield return new WaitForSeconds (2);
		TextNotifications.Clear ();

		//Crush the player (attempt to)
		transform.parent.FindChild("Bottom").GetComponent<Animator> ().SetTrigger("Crush");
		TextNotifications.Create (TextNotifications.NotificationTypes.NORMAL, Color.red, -1, "Let's get you out of that pesky exit passage, shall we?");
	}

}
