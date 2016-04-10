using UnityEngine;
using System.Collections;

public class InstanceDatabase : MonoBehaviour {
	//Level
	static GameObject playerObject;
	static GameObject mainCamera;

	public static void SetLevelReferences() {
		playerObject = GameObject.Find ("Player");
		mainCamera = playerObject.transform.FindChild ("Main Camera").gameObject;
	}

	public static GameObject GetPlayerReference() {
		if (playerObject == null) 
			Debug.LogError ("Player reference was null!!!");
		return playerObject;
	}

	public static GameObject GetMainCameraReference() {
		if (mainCamera == null) 
			Debug.LogError ("Main Camera was null!!!");
		return mainCamera;
	}
}
