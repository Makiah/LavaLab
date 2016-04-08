using UnityEngine;
using System.Collections;

/*
 * Here is the way that I am envisioning levels being generated.  To begin, only ONE level is created.  The player completes the level, then moves into the elevator.  
 * Then the level is destroyed, and while the animation is playing to make it look like the player is moving upward, the elevator is actually below where it was 
 * while the new level is being loaded.  When the level is done loading and at least 6 seconds have passed, then the elevator "arrives" at its destination on the 
 * same vertical level as the previous one.  
 * 
 * Also, just a thought, why not make it so that turrets can be instantiated by going new Turret(location, fireRate, etc.), which will instantiate a turret at the 
 * given location and initialize it?  It would have to be a bit weird since the already existent class could not be added (I think).  
 */

public class LevelGenerator : MonoBehaviour {

	public static LevelGenerator instance;
	public int currentLevel = 1;

	private GameObject[] currentActiveObjects;

	//By right elevator room and left elevator room I mean the direction you have to walk to get into the elevator.  
	[SerializeField] private GameObject startRoom = null, midRoom = null, leftElevatorRoom = null, rightElevatorRoom = null, leftReceiverRoom = null, rightReceiverRoom = null;

	void Awake() {
		instance = this;
	}

	public void Initialize() {
		CreateLevel (1);
	}

	//Keep track of the current x location across all classes.  
	private float currentXLocation = 0f;

	public void CreateLevel(int levelID) {
		//The main level array.  
		RemoveCurrentLevel();

		GameObject[] level = new GameObject[3 + levelID / 2];

		if (levelID < 1) {
			Debug.LogError ("Only levels 1 to infinity are valid level IDs");
			levelID = 1;
		}

		//Count the number of segments placed.  
		int segmentsPlaced = 0;
		int direction = (levelID % 2 == 0 ? 1 : -1);

		if (levelID == 1) {
			//Place the receiver left or right based on the level.  Remember than even levels move left to right and odd levels move right to left.  
			level[0] = PlaceTerrain (startRoom, new Vector2(currentXLocation, 0));
			//Displace the x location by either a negative or a positive value based on the level ID.  
			currentXLocation += GetSpriteSize (startRoom).x / 2f * direction;
		} else {
			//Place the receiver left or right based on the level.  Remember than even levels move left to right and odd levels move right to left.  
			level[0] = PlaceTerrain (levelID % 2 == 0 ? rightReceiverRoom : leftReceiverRoom, new Vector2(currentXLocation, 0));
			//Displace the x location by either a negative or a positive value based on the level ID.  
			currentXLocation += GetSpriteSize (rightReceiverRoom).x / 2f * direction;
		}
		//Increment the number of segments placed by 1 (for the receiver).  
		segmentsPlaced++;

		//Level length increases by 1 every 2 levels.  
		while (segmentsPlaced < 3 + levelID / 2 - 1) {
			currentXLocation += GetSpriteSize (midRoom).x / 2f * direction;
			level[segmentsPlaced] = PlaceTerrain (midRoom, new Vector3 (currentXLocation, 0, 0));
			currentXLocation += GetSpriteSize (midRoom).x / 2f * direction;

			//Increment segmentsPlaced.  
			segmentsPlaced++;
		}

		currentXLocation += GetSpriteSize (rightElevatorRoom).x / 2f * direction;
		//Place the receiver left or right based on the level.  Remember than even levels move left to right and odd levels move right to left.  
		level[segmentsPlaced] = PlaceTerrain (levelID % 2 == 0 ? rightElevatorRoom : leftElevatorRoom, new Vector2(currentXLocation, 0));
		//Increment the number of segments placed by 1 (for the receiver).  
		segmentsPlaced++;

		//Set the instance variables.  
		currentLevel = levelID;
		currentActiveObjects = level;

		//Add the turrets to the level.  
		AddTurretsToLevel ();
	}

	//Goes through the whole level and adds turrets equally spread through the level in a random config.  
	private void AddTurretsToLevel() {
		//Create a "folder" for the turrets.  
		Transform turretParent = new GameObject ("Turrets").transform;
		//Place the turrets into the folder.  
		for (int i = 0; i < currentLevel * 5; i++) {
			//Create a random turret at different points through the level.  
			float posOffset;
			if (currentLevel % 2 == 1)
				posOffset = currentActiveObjects [currentActiveObjects.Length - 1].transform.position.x;
			else
				posOffset = currentActiveObjects [0].transform.position.x;
			Turret createdTurret = Turret.Create (
				(posOffset + 1.0f * currentActiveObjects [0].transform.position.x - currentActiveObjects [currentActiveObjects.Length - 1].transform.position.x) / (currentLevel * 5f) * i, 
				Random.Range (0, 2) == 0 ? Turret.TurretPosition.BOTTOM : Turret.TurretPosition.TOP);
			createdTurret.transform.SetParent (turretParent);
		}

		//Recreate the array with the turrets.  
		GameObject[] oldObjects = currentActiveObjects;
		currentActiveObjects = new GameObject[oldObjects.Length + 1];
		for (int i = 0; i < oldObjects.Length; i++) {
			currentActiveObjects [i] = oldObjects [i];
		}
		currentActiveObjects [currentActiveObjects.Length - 1] = turretParent.gameObject;
	}

	private void RemoveCurrentLevel() {
		if (currentActiveObjects != null && currentActiveObjects.Length > 0) {
			foreach (GameObject obj in currentActiveObjects) {
				Destroy (obj);
			}
			currentActiveObjects = null;
		}
	}

	//Used to get the size of a sprite from a GameObject without the irritation factor.  
	private Vector2 GetSpriteSize(GameObject obj) {
		if (obj.GetComponent <SpriteRenderer> () != null) {
			return obj.GetComponent <SpriteRenderer> ().bounds.size;
		} else {
			Debug.LogError (obj.name + " does not have a SpriteRenderer attached!");
			return Vector2.zero;
		}
	}

	private GameObject PlaceTerrain(GameObject terrain, Vector2 location) {
		GameObject createdTerrain = (GameObject)(Instantiate (terrain, location, Quaternion.identity));
		createdTerrain.transform.localScale = terrain.transform.localScale;
		return createdTerrain;
	}

}
