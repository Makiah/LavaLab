using UnityEngine;
using System.Collections;
using CatchCo;

public class ElevatorControl : MonoBehaviour, IMethodReroute1, IMethodReroute2 {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.GetComponent <Player> () != null || other.transform.GetComponentInParent <Player> () != null)
			MoveElevator ();
	}

	[ExposeMethodInEditor]
	public void MoveElevator() {
		Debug.Log ("Starting elevator");
		//Disable the elevator so that the player can't use it again.  
		GetComponent <Collider2D> ().enabled = false;
		StartCoroutine (MoveElevatorCoroutine ());
	}

	//Variables that are changed by outside methods.  
	private bool movedUp = false;
	private bool movedToNextLevel = false;

	IEnumerator MoveElevatorCoroutine() {
		//Reset the lava timer so that the player does not die while on the elevator.  
		LavaNotifier.instance.RestartLavaTimer();
		//Make sure that the player moves with the elevator.  
		InstanceDatabase.GetPlayerReference ().transform.SetParent (transform.GetChild(0).FindChild("Elevator"));

		Animator anim = transform.GetChild(0).GetComponent <Animator> ();

		//Wait until the elevator is above the terrain.  
		anim.SetTrigger ("MoveUp");
		while (movedUp == false) {
			yield return null;
		}

		//Make it look to the player that the elevator is still moving.  
		anim.SetBool("AppearMoving", true);

		//Move the elevator below the terrain
		transform.position = new Vector3 (transform.position.x, -35.13f, 0);

		transform.GetChild (0).FindChild ("Elevator").transform.localPosition = Vector3.zero;

		transform.SetParent (null);

		//Create the level.  HAS TO BE PREINCREMENT NOT POSTINCREMENT
		LevelGenerator.instance.CreateLevel(++LevelGenerator.instance.currentLevel);

		yield return new WaitForSeconds (4);

		//Move back up to the receiver.  
		anim.SetBool ("AppearMoving", false);
		anim.SetTrigger ("MoveBack");

		//Unparent the player so that he/she will not be destroyed as well.  
		while (movedToNextLevel == false) {
			yield return null;
		}

		InstanceDatabase.GetPlayerReference ().transform.SetParent (null);

		//Start the lava timer again.  
		LavaNotifier.instance.StartLavaTimer();

		//Wait until the player leaves the level.  
		int currentLevel = LevelGenerator.instance.currentLevel;
		while (LevelGenerator.instance.currentLevel == currentLevel) {
			yield return null;
		}

		//Remove the elevator after the level changes so that they are not cluttering the scene.  
		Destroy (gameObject);
	}

	public void ReRoute1() {
		movedUp = true;
	}

	public void ReRoute2() {
		movedToNextLevel = true;
	}

}
