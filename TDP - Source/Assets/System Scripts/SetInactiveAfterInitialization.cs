using UnityEngine;
using System.Collections;

public class SetInactiveAfterInitialization : MonoBehaviour {
	void OnEnable() {
		InitializationSequence.SetInactiveObjects += SetState;
	}

	void OnDisable() {
		InitializationSequence.SetInactiveObjects -= SetState;
	}

	void SetState() {
		gameObject.SetActive (false);
	}
}
