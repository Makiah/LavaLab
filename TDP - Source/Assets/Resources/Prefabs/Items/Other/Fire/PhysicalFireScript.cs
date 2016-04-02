using UnityEngine;
using System.Collections;

public class PhysicalFireScript : MonoBehaviour {

	//No initialization is required, when the thing is instantiated the particle system should start.  The only real 
	//required functionality is linecasting.  
	public void OnFireCreated() {
	}

	//Linecasting.  
	IEnumerator LookForCookableFood() {
		while (true) {

			yield return null;
		}
	}

}
