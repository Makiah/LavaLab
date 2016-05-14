using UnityEngine;
using System.Collections;

public class Drop : MonoBehaviour {

	//Used to instantiate a dropped item.  
	public static Drop Create(ResourceReferenceWithStack itemReference, Vector3 initialPosition, float xForce) {
		//Attempt to load the basic drop prefab from the Resources folder.  
		GameObject basicDrop = Resources.Load ("Prefabs/Items/Other/BasicDrop") as GameObject;
		//If it exists, then initialize the object.  
		if (basicDrop != null) {
			//Instantiate the basic drop.  
			GameObject createdObject = (GameObject)(Instantiate (basicDrop, initialPosition, Quaternion.identity));
			//Set the object sprite.  
			createdObject.transform.GetChild(0).GetComponent <SpriteRenderer> ().sprite = itemReference.uiSlotContent.itemIcon;
			//Add the object info to the created object.  
			createdObject.AddComponent <Drop> ();
			//Drop one of the items.  
			createdObject.GetComponent <Drop> ().localResourceReference = new ResourceReferenceWithStack (itemReference.uiSlotContent, 1);
			//Give the rigidbody a bit of initial velocity.  
			createdObject.GetComponent <Rigidbody2D> ().AddForce (new Vector2 (xForce, 0));
			//Add 1 or -1 to the position based on the force of the object.  
			createdObject.transform.position = initialPosition + new Vector3 (Mathf.Sign (xForce), 0, 0);
			//Initialize the droppd item.  
			createdObject.GetComponent <Drop> ().Initialize ();
			//Return the created drop.  
			return createdObject.GetComponent <Drop> ();
		} else {
			Debug.Log ("Drop could not be created: BasicDrop prefab does not exist!!");
			return null;
		}
	}

	[HideInInspector] public ResourceReferenceWithStack localResourceReference;
	private Transform player;

	public void Initialize() {
		player = Player.instance.transform;
		StartCoroutine (MoveTowardsPlayer());
	}

	IEnumerator MoveTowardsPlayer() {
		while (true) {
			if (Mathf.Abs(player.transform.position.x - transform.position.x) < 5) {
				if (player.transform.position.x > transform.position.x)
					transform.position += new Vector3(0.02f, 0, 0);
				else 
					transform.position += new Vector3(-0.02f, 0, 0);
			}

			yield return null;
		}
	}


}
