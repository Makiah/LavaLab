using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterHealthPanelManager))]

public class FixedTurret : Turret {

	protected override void InitializeEnemy() {
		base.InitializeEnemy ();
		//Change fireRate and bolt color depending on the current level.  
		fireRate = Mathf.Clamp((fireRate) / (1 + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30) / 2f;
		fireSpeed = Mathf.Clamp(fireSpeed * (1f + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30) / 2f;
		//Setting the turret position randomly.  
		if (Random.Range (0, 2) == 0)
			SetPosition (TurretPosition.BOTTOM);
		else
			SetPosition (TurretPosition.TOP);
	}

	public override void SetPosition(TurretPosition position) {
		base.SetPosition (position);
		if (localPosition == TurretPosition.BOTTOM)
			transform.localScale = new Vector3 (transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}


}