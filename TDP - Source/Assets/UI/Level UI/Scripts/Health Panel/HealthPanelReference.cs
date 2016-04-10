
/*
 * Author: Makiah Bennett
 * Created September 15
 * Last edited: 15 September 2015 
 * 
 * This script should be used as a reference to the health bars displayed on-screen, mainly for the UIHealthController script.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthPanelReference : MonoBehaviour {

	protected void OnEnable() {
		UIInitializationSequence.InitializeHealthPanels += InitializeHealthPanelReference;
	}

	protected void OnDisable() {
		UIInitializationSequence.InitializeHealthPanels -= InitializeHealthPanelReference;
	}

	protected GameObject panel;
	protected Image headIcon;
	protected Slider healthBar;
	protected Image healthBarFillImage;

	bool occupied = false;

	protected EnemyHealthPanelsController masterController;

	protected virtual void InitializeHealthPanelReference() {
		//Setting basic component references.  
		panel = gameObject;
		headIcon = transform.FindChild ("Icon").gameObject.GetComponent <Image> ();
		healthBar = transform.FindChild ("Health Bar").gameObject.GetComponent <Slider> ();
		masterController = transform.parent.parent.GetComponent <EnemyHealthPanelsController> ();
		healthBarFillImage = healthBar.transform.FindChild ("Fill Area").FindChild ("Fill").GetComponent <Image> ();
		//Make sure that the panels do not have any initial value.  
		Clear ();
	}

	public bool IsEmpty() {
		return !occupied;
	}

	//Called when a panel is cleared.  
	public void Clear() {
		headIcon.sprite = null;
		healthBar.maxValue = 1;
		healthBar.value = 0;
		occupied = false;
		panel.SetActive (false);
	}

	//Called by CharacterHealthPanelManager when it is given a panel.  
	public void InitializePanel(Sprite image, float totalHealth, float currentHealth) {
		headIcon.sprite = image;
		healthBar.maxValue = totalHealth;
		healthBar.value = currentHealth;
		occupied = true;
		panel.SetActive (true);
		healthBarFillImage.color = Color.green;
	}

	//Used when a potion is added or object is attacked (called by CharacterHealthPanelManager).  
	public void UpdateHealth(float currentHealth) {
		if (occupied) {
			healthBar.value = currentHealth;
			//Linearly interpolates between green and red based on fraction filled.  
			healthBarFillImage.color = Color.Lerp (Color.green, Color.red, (healthBar.maxValue - healthBar.value) / healthBar.maxValue);
		}
		else
			Debug.LogError ("Cannot update empty health panel");
	}

}
