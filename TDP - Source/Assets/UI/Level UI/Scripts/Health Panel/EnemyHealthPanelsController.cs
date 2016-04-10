
/*
 * Author: Makiah Bennett
 * Created 9/15
 * Last edited: 18 November 2015
 * 
 * 
 * This script controls the health bars and icons of the UI, and includes a public function for adding enemies to the system.  
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthPanelsController : MonoBehaviour {

	public static EnemyHealthPanelsController instance;

	void Awake() {
		//Set the static instance variable.  
		instance = this;
	}

	//Initialization
	void OnEnable() {
		UIInitializationSequence.InitializeUIHealthController += InitializeUIHealthController;
	}

	void OnDisable() {
		UIInitializationSequence.InitializeUIHealthController -= InitializeUIHealthController;
	}

	//Define Health controller components
	HealthPanelReference enemyHealthPanel1, enemyHealthPanel2, enemyHealthPanel3;

	//Set references to the health panel references.  
	void InitializeUIHealthController() {
		enemyHealthPanel1 = transform.GetChild(0).GetComponent <HealthPanelReference> ();
		enemyHealthPanel2 = transform.GetChild(1).GetComponent <HealthPanelReference> ();
		enemyHealthPanel3 = transform.GetChild(2).GetComponent <HealthPanelReference> ();	
	}
	
	public HealthPanelReference GetEnemyHealthPanelReference() {
		return GetBestAvailableEnemyHealthPanelReference ();
	}

	//Choose the best available health panel reference (in order of 1-3)
	HealthPanelReference GetBestAvailableEnemyHealthPanelReference() {
		if (enemyHealthPanel1.IsEmpty ())
			return enemyHealthPanel1;
		else if (enemyHealthPanel2.IsEmpty ())
			return enemyHealthPanel2;
		else if (enemyHealthPanel3.IsEmpty ())
			return enemyHealthPanel3;
		else
			return null;
	}

}
