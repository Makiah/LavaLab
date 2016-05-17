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

	//Static instance, makes it more easy to access.  Also called a singleton.  
	public static LevelGenerator instance;
	public int currentLevel = 1;

	private string[] levelDescriptions = new string[]{
		"Just give up already.",
		"You stink at this.",
		"Are you still trying to keep going?",
		"Just lay down in front of a turret.",
		"You'll never get out.", 
		"Your friends have already died.", 
		"You were too slow.", 
		"I am sick of you.", 
		"Time to send in some friends..."
	};

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

		//Add the level description and the floor level so that people can brag to their friends, enjoy the signs, etc.  
		if (currentLevel > 1) {
			TextMesh description = level [0].transform.FindChild ("Floor Description").GetComponent <TextMesh> ();
			description.text = levelDescriptions [currentLevel - 2];
			description.gameObject.GetComponent <MeshRenderer> ().sortingLayerName = "Terrain";
			description.gameObject.GetComponent <MeshRenderer> ().sortingOrder = 2;
			TextMesh name = level [0].transform.FindChild ("Floor Name").GetComponent <TextMesh> ();
			name.text = "Floor " + currentLevel;
			name.gameObject.GetComponent <MeshRenderer> ().sortingLayerName = "Terrain";
			name.gameObject.GetComponent <MeshRenderer> ().sortingOrder = 2;
			Debug.Log ("Set");
		}

		//Add the turrets to the level.  
		PlaceEnemies ();
	}

	//Make the player define the turret objects.  
	[SerializeField] private GameObject rotatingTurret = null, fixedTurret = null, roboGuard = null;

	//Goes through the whole level and adds turrets equally spread through the level in a random config.  
	private void PlaceEnemies() {

		//Create a "folder" for the turrets.  
		Transform enemyParent = new GameObject ("Enemies").transform;
		int desiredTurrets = (int)((1 + Mathf.Log (currentLevel)) * 5f + currentLevel / 2f);

		//Create a random turret at different points through the level.  
		//Determine whether the first or last object in the array would be farther right based on the level id.  
		float posOffset;
		if (currentLevel % 2 == 1)
			posOffset = currentActiveObjects [currentActiveObjects.Length - 1].transform.position.x;
		else
			posOffset = currentActiveObjects [0].transform.position.x;

		float xComponent;
		GameObject toInstantiate;

		//Place the turrets into the GameObject.  
		for (int i = 0; i < desiredTurrets; i++) {
			
			//Instantiate the turrets through the level using the Turret.Create method.  
			xComponent = posOffset + (Mathf.Abs (currentActiveObjects[0].transform.position.x - currentActiveObjects[currentActiveObjects.Length - 1].transform.position.x) / (desiredTurrets)) * i;

			//Choose the enemy to instantiate at the given point.  
			toInstantiate = null;
			int rand = Random.Range (0, 3);
			if (rand == 0)
				toInstantiate = fixedTurret;
			else if (rand == 1)
				toInstantiate = rotatingTurret;
			else if (rand == 2)
				toInstantiate = roboGuard;

			if (toInstantiate != null) {
				GameObject createdEnemy = (GameObject)(Instantiate (toInstantiate, new Vector3 (xComponent, 0, 0), Quaternion.identity));
				createdEnemy.GetComponent <Enemy> ().Initialize ();
				createdEnemy.transform.SetParent (enemyParent);
				//Do something with the enemy here
			} else
				Debug.LogError("Please give valid enemies on LevelGenerator!");

		}

		//Recreate the array with the turrets.  
		GameObject[] oldObjects = currentActiveObjects;
		currentActiveObjects = new GameObject[oldObjects.Length + 1];
		for (int i = 0; i < oldObjects.Length; i++) {
			currentActiveObjects [i] = oldObjects [i];
		}
		currentActiveObjects [currentActiveObjects.Length - 1] = enemyParent.gameObject;
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
