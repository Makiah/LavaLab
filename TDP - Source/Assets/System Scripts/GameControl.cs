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
		IEnumerator coroutine = TextNotifications.Create(TextNotifications.NotificationTypes.OSCILLATING, Color.red, "PRESS THE BUTTON!");
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

		//Allow the player to move again.  
		CurrentLevelVariableManagement.GetPlayerReference ().GetComponent <PlayerAction> ().EnablePlayerActions ();
	}

}
