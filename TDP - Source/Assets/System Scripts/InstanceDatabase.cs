using UnityEngine;
using System.Collections;

public class InstanceDatabase : MonoBehaviour {
	//Level
	static GameObject playerObject;
	static GameObject mainCamera;
	static GameObject levelUI;
	static GameObject inventory;
	static float levelLengthX;

	public static void SetLevelReferences() {
		playerObject = GameObject.Find ("Player");
		mainCamera = playerObject.transform.FindChild ("Main Camera").gameObject;
		levelUI = GameObject.Find ("UI");
		inventory = levelUI.transform.FindChild ("Inventory").gameObject;
	}

	public static GameObject GetPlayerReference() {
		if (playerObject == null) 
			Debug.LogError ("Player reference was null!!!");
		return playerObject;
	}

	public static GameObject GetMainInventoryReference() {
		if (inventory == null)
			Debug.LogError ("Player inventory reference was null!!!");
		return inventory;
	}

	public static GameObject GetMainCameraReference() {
		if (mainCamera == null) 
			Debug.LogError ("Main Camera was null!!!");
		return mainCamera;
	}

	public static GameObject GetLevelUIReference() {
		if (levelUI == null) 
			Debug.LogError ("Level UI was null!!!");
		return levelUI;
	}

	public static void SetLevelLengthX(float value) {
		levelLengthX = value;
	}

	public static float GetLevelLengthX () {
		if (levelLengthX == 0)
			Debug.LogError ("WARNING: Returning 0 as level length!");
		return levelLengthX;
	}
}
