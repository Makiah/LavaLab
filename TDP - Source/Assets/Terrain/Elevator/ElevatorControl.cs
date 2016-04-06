using UnityEngine;
using System.Collections;
using CatchCo;

public class ElevatorControl : MonoBehaviour {

	[ExposeMethodInEditor]
	public void MoveElevator() {
		Debug.Log ("Starting");
		StartCoroutine (MoveElevatorCoroutine ());
	}

	private bool movedUp = false;

	IEnumerator MoveElevatorCoroutine() {
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
		Debug.Log (transform.GetChild (0).FindChild ("Elevator").transform.localPosition);

		yield return new WaitForSeconds (6);

		Debug.Log (transform.GetChild (0).FindChild ("Elevator").transform.localPosition);

		//Somewhere around here the level would be created.  

		//Move back up to the receiver.  
		anim.SetBool ("AppearMoving", false);
		anim.SetTrigger ("MoveBack");
	}

	public void MovedUp() {
		movedUp = true;
	}

}
