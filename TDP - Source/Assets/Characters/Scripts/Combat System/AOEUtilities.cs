using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//As much as I dislike having a static thing that a bunch of different classes to reference to cast over an area, it really is the best way since the player, 
//enemies, and lasers have to use it.  

public class AOEUtilities : MonoBehaviour {

	//Apparently generic types cannot use the == operator.  Thanks StackOverflow!
	public static T[] GetComponentsInArea <T> (Collider2D aoe) where T : class {
		Collider2D[] results = null;
		if (aoe is BoxCollider2D) {
			BoxCollider2D box = (BoxCollider2D)aoe;
			results = Physics2D.OverlapAreaAll (new Vector2 (box.bounds.center.x + box.bounds.extents.x, box.bounds.center.y + box.bounds.extents.y), 
				new Vector2 (box.bounds.center.x - box.bounds.extents.x, box.bounds.center.y - box.bounds.extents.y), 1 << LayerMask.NameToLayer("Fighting"));
		} else if (aoe is CircleCollider2D) {
			CircleCollider2D circle = (CircleCollider2D)aoe;
			results = Physics2D.OverlapCircleAll (circle.bounds.center, circle.radius, 1 << LayerMask.NameToLayer("Fighting"));
		} else {
			Debug.LogError ("Invalid collider passed!");
			return null;
		}

		//Construct the actual list, getting rid of any duplicates.  
		List <T> componentList = new List <T> ();
		foreach (Collider2D c in results) {
			Debug.Log ("Found " + c.gameObject.name);
			//Populate the list.
			if (c.gameObject.GetComponent <T> () != null)
				componentList.Add (c.gameObject.GetComponent <T> ());
			else if (c.gameObject.GetComponentInParent <T> () != null)
				componentList.Add (c.gameObject.GetComponentInParent <T> ());
		}

		List <T> finishedList = new List<T> ();
		while (componentList.Count > 0) {
			//Add the first one.  
			finishedList.Add (componentList [0]);
			componentList.RemoveAt (0);

			//Remove duplicates from the other list before continuing.  
			for (int i = 0; i < componentList.Count; i++) {
				if (finishedList [finishedList.Count - 1] == componentList [i]) {
					componentList.RemoveAt (i);
					i--;
				}
			}
		}

		//Why doesn't java have a ToArray() method?
		return finishedList.ToArray ();
	}

}
