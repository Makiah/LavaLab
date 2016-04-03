/*
 * Author: Makiah Bennett
 * Date Created: 13 November 2015
 * 
 * Description: This script controls the main events that occur during the game, such as new level loading, etc.  Similar to GameData.  
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

	void Start() {
		StartCoroutine (BeginGame());
	}

	IEnumerator BeginGame() {
		//Initialize the database.  
		ResourceDatabase.InitializeDatabase ();

		//Default profession (TEMPORARY)
		GameData.SetPlayerProfession (ResourceDatabase.GetRaceByParameter ("Agent"));

		//Begin the initialization sequence.  
		yield return StartCoroutine(GameObject.Find ("Initialization Sequence").GetComponent <InitializationSequence> ().LoadEverything ());

		//After the initialization is complete, tell the player to press the button.  
		IEnumerator coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.OSCILLATING, Color.green, 80, "PRESS THE BUTTON!");
		StartCoroutine (coroutine);

		//Prevent the player from moving until they do so.  
		CurrentLevelVariableManagement.GetPlayerReference ().GetComponent <PlayerAction> ().DisablePlayerActions ();

		//Wait until the player presses the button.  
		while (!Input.GetKeyDown (KeyCode.Space)) {
			yield return null;
		}

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		//Show the player that the button has been pressed.  
		GameObject.Find ("Game Start Room").transform.FindChild ("Button").GetComponent <UpdateButton> ().UpdateButtonState ();

		//Evil professor says "My lab!  My weapon!  NOOOO!"
		//After the initialization is complete, tell the player to press the button.  
		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.red, -1, "MY LAB! MY WEAPON! NOOOO!!!");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(1.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.green, -1, "Nice job, Agent 22!");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(1.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.red, -1, "I'll DESTROY YOU!");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(1.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.green, -1, "Uh, sorry, but we can't help a whole lot from here.");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(1.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.green, -1, "Good luck! I'll tell your family you did a good job.");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(1.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.red, -1, "How does it feel to be BETRAYED, 22?");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(1.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.red, -1, "Hope you enjoyed what you had for dinner last night.");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(1.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();


		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.red, -1, "Seeing as how it was your last meal...");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(1.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.red, 80, "AH");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.red, 80, "AH-HA");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.red, 80, "AH-HA-HA");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(.25f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.NORMAL, Color.red, 80, "AH-HA-HA-HAAA!!!");
		StartCoroutine (coroutine);

		yield return new WaitForSeconds(2f);

		StopCoroutine (coroutine);
		//Then stop the coroutines and clear the notification.  
		TextNotifications.Clear ();

		//Allow the player to move again.  
		CurrentLevelVariableManagement.GetPlayerReference ().GetComponent <PlayerAction> ().EnablePlayerActions ();
	}

}
