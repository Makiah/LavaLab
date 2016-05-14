
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script controls the camera.  Right now, it is not entirely effective, but it does work.    
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public static CameraControl instance;
	void Awake() {
		instance = this;
	}

	private bool cameraFunctionsEnabled = false, moveLock = false, cameraCentering = false;
	public Transform playerTransform;
	public float moveSpeed;

	private IEnumerator listenForEnable, mouseControl, centerCamera;

	public void EnableCameraFunctions() {
		if (cameraCentering) {
			StopCoroutine (centerCamera);
			cameraCentering = false;
		}
		if (cameraFunctionsEnabled == false) {
			playerTransform = Player.instance.transform;
			listenForEnable = ListenForCameraFunctionEnable ();
			mouseControl = MouseControl ();
			StartCoroutine (listenForEnable);
			StartCoroutine (mouseControl);
			cameraFunctionsEnabled = true;
		}
	}

	public void DisableCameraFunctions() {
		if (cameraCentering) {
			StopCoroutine (centerCamera);
			cameraCentering = false;
		}
		if (cameraFunctionsEnabled) {
			StopCoroutine (listenForEnable);
			StopCoroutine (mouseControl);
			cameraFunctionsEnabled = false;
		}
	}

	public void CenterCamera() {
		if (cameraCentering == false) {
			centerCamera = CenterCameraCoroutine ();
			StartCoroutine (centerCamera);
			cameraCentering = true;
		}
	}

	private IEnumerator ListenForCameraFunctionEnable() {
		while (true) {
			if (Input.GetKeyDown (KeyCode.C)) {
				moveLock = !moveLock;
			}
			yield return null;
		}
	}

	private IEnumerator MouseControl() {
		//The required variables.  
		Vector3 mousePosition, optimalCameraPosition;
		float speed;
		while (true) {
			//The Screen point is remaining constant, yet the world point is changing as the object moves.  

			//As long as the camera movement is enabled (could be disabled by pressing 'c'.  
			if (moveLock) {
				//Get the mouse and calculate the optimal camera position given the current mouse position.  
				mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				optimalCameraPosition = (playerTransform.position + mousePosition) / 2;
				optimalCameraPosition = new Vector3 (optimalCameraPosition.x, optimalCameraPosition.y, -10);

				//The camera should move less quickly the further it gets away from the player.  The camera has a base movement speed of 30.  
				speed = moveSpeed * Time.deltaTime * (30f / (Vector2.Distance (mousePosition, playerTransform.position) + 1));

				//Set the position of the camera based of off the other variables which had already been calculated.  
				transform.position = Vector3.MoveTowards (transform.position, optimalCameraPosition, speed);
			}

			//Wait one frame.  
			yield return null;
		}
	}

	private IEnumerator CenterCameraCoroutine() {
		//The required variables.  
		Vector3 opPosition, optimalCameraPosition;
		float speed;

		while (true) {
			opPosition = playerTransform.position;

			//The Screen point is remaining constant, yet the world point is changing as the object moves.  
			optimalCameraPosition = (playerTransform.position + opPosition) / 2;
			optimalCameraPosition = new Vector3 (optimalCameraPosition.x, optimalCameraPosition.y, -10);

			//The camera should move less quickly the further it gets away from the player.  The camera has a base movement speed of 30.  
			speed = moveSpeed * Time.deltaTime * (30f / (Vector2.Distance (opPosition, playerTransform.position) + 1));

			//Set the position of the camera based of off the other variables which had already been calculated.  
			transform.position = Vector3.MoveTowards (transform.position, optimalCameraPosition, speed);

			//Wait one frame.  
			yield return null;
		}
	}

}
