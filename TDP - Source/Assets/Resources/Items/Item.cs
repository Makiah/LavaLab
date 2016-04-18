
/*
 * Author: Makiah Bennett
 * Last edited: 12 September 2015
 * 
 * This script works similarly to an interface, yet has a few properties that make it useful as an abstract class.  This class is used solely as a base 
 * for game tools, such as pickaxes, hatchets, etc.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//As of right now, this class could work just as well as an interface, and probably simplify a few things.  However, if additional functionality is 
//later added, this class makes more sense.  
public abstract class Item : MonoBehaviour {

	//Used for base classes.  
	protected bool initialized = false;

	//Attack and move works by creating a serializable system for defining MovementAndMethod[].  This class HAS to be present, even though it is pretty much
	//just a copy of the MovementAndMethod class, since it is the way that it will work in the inspector.  
	[System.Serializable]
	public class AttackAndMove {
		public MovementAndMethod.PossibleTriggers method;
		public MovementAndMethod.PossibleMovements attack;
		public bool worksMidair;
	}

	//Unity has a weird issue that only certain ways that classes can be put together appears in the Inspector.  
	[SerializeField] protected AttackAndMove[] movementTriggerPair = null;

	//The class methods.  
	protected ICanHoldItems attachedCharacterInput;

	//Has to be called before being initialized.  
	public void SetAttachedCharacterInput(ICanHoldItems ctorCharacterInput) {
		//Set the attached character (simpler than searching through the whole parent for the character action class).  
		attachedCharacterInput = ctorCharacterInput;
		//Tell the child classes that the object is initialized.  
		initialized = true;
		InitializeItem ();
	}

	//Called by CharacterBaseActionClass when a new item is being used.  
	public abstract void InitializeItem();
	public abstract MovementAndMethod[] GetPossibleActionsForItem ();
	public abstract void InfluenceEnvironment(MovementAndMethod.PossibleMovements actionKey);

}
