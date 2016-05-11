using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterHealthPanelManager))]

public class FixedTurret : Turret {

	//Have to be taken from the resources folder.  
	private static GameObject turret;
	private static GameObject rollingTurret;

	public static FixedTurret Create (float xLocation, TurretPosition position) {
		//Get the turret from the Resources folder.  
		if (turret == null) {
			turret = Resources.Load ("Enemies/Turrets/FixedTurret/FixedTurret") as GameObject;
			if (turret == null)
				Debug.LogError ("Enemies/Turrets/FixedTurret/FixedTurret could not be loaded!");
		}

		//Determine the y location of the turret based on the position.  
		if (turret != null) {
			float yLocation = 0;
			switch (position) {
			case TurretPosition.BOTTOM: 
				yLocation = -3.2f;
				break;
			case TurretPosition.TOP: 
				yLocation = 3.2f;
				break;
			}
			//Instantiate the turret
			GameObject createdTurret = (GameObject)(Instantiate (turret, new Vector3 (xLocation, yLocation, 0), Quaternion.identity));
			//If the turret is on the turret, rotate the turret mount.  
			createdTurret.GetComponent <FixedTurret> ().SetPosition (position);
			//Initialize the turret.  
			createdTurret.GetComponent <FixedTurret> ().InitializeCharacter ();
			//Return the turret.  
			return createdTurret.GetComponent <FixedTurret> ();
		} else {
			Debug.LogError ("FixedTurret does not exist!");
			return null;
		}
	}

	protected override void InitializeEnemy() {
		base.InitializeEnemy ();
		//Change fireRate and bolt color depending on the current level.  
		fireRate = Mathf.Clamp((fireRate) / (1 + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30) / 3f;
		fireSpeed = Mathf.Clamp(fireSpeed * (1f + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30) / 2f;
	}

	public override void SetPosition(TurretPosition position) {
		localPosition = position;
		if (localPosition == TurretPosition.BOTTOM)
			transform.localScale = new Vector3 (transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}


}