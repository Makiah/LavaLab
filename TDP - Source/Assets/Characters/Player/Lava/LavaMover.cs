using UnityEngine;
using System.Collections;

public class LavaMover : MonoBehaviour {

	public static LavaMover instance;

	void Awake() {
		instance = this;
	}

	public void RiseLava() {
		transform.GetChild (0).GetComponent <Animator> ().SetTrigger ("Move");
		transform.GetChild (0).GetChild(0).GetComponent <Animator> ().SetTrigger ("Move");
	}

}
