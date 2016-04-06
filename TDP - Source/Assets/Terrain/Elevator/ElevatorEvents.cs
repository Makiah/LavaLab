using UnityEngine;
using System.Collections;
using CatchCo;

public class ElevatorEvents : MonoBehaviour {

	public void MovedUp() {
		transform.parent.GetComponent <ElevatorControl> ().MovedUp ();
	}

}
