using UnityEngine;
using System.Collections;

public class UpdateButton : MonoBehaviour {

	[SerializeField] private Sprite greenButton = null;

	public void UpdateButtonState() {
		GetComponent <SpriteRenderer> ().sprite = greenButton;
	}

}
