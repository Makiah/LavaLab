/*
 * This interface is only so that any enemy or player that will be involved in fighting has a GUID so that it does not attack itself. 
 */

using UnityEngine;
using System.Collections;

public interface ICombatant : IMethodReroute1, IMethodReroute2 {
	bool CheckCurrentAttackAnimationState();
	string GetCombatantID();
	Character GetActualClass();
	void ApplyKnockback(Vector2 force);
}
