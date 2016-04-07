using UnityEngine;
using System.Collections;
using CatchCo;

public class ElevatorEvents : MonoBehaviour {

	public void MovedUp() {
		transform.parent.GetComponent <ElevatorControl> ().MovedUp ();
	}

	public void ReachedNextLevel() {
		transform.parent.GetComponent <ElevatorControl> ().ReachedNextLevel ();
	}

}
