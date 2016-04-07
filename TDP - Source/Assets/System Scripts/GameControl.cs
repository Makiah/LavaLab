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

	//There will be only one of these.  
	public static GameControl instance;

	void Start() {
		instance = this;
		StartCoroutine (BeginGame());
	}

	IEnumerator BeginGame() {
		//Wait one frame so everything is initialized.  
		yield return null;

		//Initialize the database.  
		ResourceDatabase.InitializeDatabase ();

		//Default profession (TEMPORARY)
		GameData.SetPlayerProfession (ResourceDatabase.GetRaceByParameter ("Agent"));

		//Use the init sequence to initialize everything.  
		StartCoroutine(InitializationSequence.instance.LoadEverything ());

	}

}
