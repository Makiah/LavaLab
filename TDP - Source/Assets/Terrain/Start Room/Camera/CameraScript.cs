using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	void OnEnable() {
		InitializationSequence.InitializeCameras += Initialize;
	}

	void OnDisable() {
		InitializationSequence.InitializeCameras -= Initialize;
	}

	private Transform player;
	[SerializeField] private float activeDistance = 20f;

	void Initialize() {
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform.FindChild ("FlippingItem").FindChild ("Character");
		StartCoroutine (LookAtPlayer ());
	}

	IEnumerator LookAtPlayer() {
		//Prevent boxing and unboxing delay.  
		Vector2 directionVector;
		float zVal;
		Vector3 initialScale = transform.localScale;

		while (true) {
			if (Vector2.Distance (player.position, transform.position) < activeDistance) {
				//Calculate the direction vector, and normalize it (make 1 the largest value, and scale the opposite value appropriately).  
				directionVector = player.position - transform.position;
				directionVector.Normalize ();

				//Calculate the correct direction to point.  
				zVal = Mathf.Atan2 (directionVector.y, directionVector.x) * Mathf.Rad2Deg + 90;
				if (zVal < 0)
					transform.localScale = new Vector3 (initialScale.x * -1, initialScale.y, initialScale.z);
				else if (zVal >= 0)
					transform.localScale = new Vector3 (initialScale.x, initialScale.y, initialScale.z);
				transform.GetChild (0).rotation = Quaternion.Euler (0f, 0f, Mathf.Abs (zVal));
				yield return null;
			} else {
				yield return null;
			}
		}
	}

}
