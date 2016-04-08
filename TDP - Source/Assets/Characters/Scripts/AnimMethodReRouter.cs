using UnityEngine;
using System.Collections;

/*
 * This class is used for when the animator is on a child class of the actual object that has the script, but events occur in the animation that have 
 * to be seen by the script.  If you have a method that needs to be rerouted, just place it in the animation, make the class that you want it to see
 * implement the interface below and all three methods (which can be empty), and use as much as need be.  It will go through all parents in ascending 
 * order and determine which one has the script.  
 */

public interface IMethodReroute1 {
	void ReRoute1();
}

public interface IMethodReroute2 {
	void ReRoute2 ();
}

public interface IMethodReroute3 {
	void ReRoute3 ();
}

public class AnimMethodReRouter : MonoBehaviour {
	public void ReRoute1() {
		Transform current = transform;
		while (current.parent != null) {
			//Check the presence of the desired reroute.  
			if (current.GetComponent <IMethodReroute1> () != null) {
				//Call the method and exit.  
				current.GetComponent <IMethodReroute1> ().ReRoute1 ();
				return;
			}
		}
		//Display an error if nothing is found.  
		Debug.Log ("No reroute1 found for " + gameObject.name);
	}

	public void ReRoute2() {
		Transform current = transform;
		while (current.parent != null) {
			//Check the presence of the desired reroute.  
			if (current.GetComponent <IMethodReroute2> () != null) {
				//Call the method and exit.  
				current.GetComponent <IMethodReroute2> ().ReRoute2 ();
				return;
			}
		}
		//Display an error if nothing is found.  
		Debug.Log ("No reroute2 found for " + gameObject.name);
	}

	public void ReRoute3() {
		Transform current = transform;
		while (current.parent != null) {
			//Check the presence of the desired reroute.  
			if (current.GetComponent <IMethodReroute3> () != null) {
				//Call the method and exit.  
				current.GetComponent <IMethodReroute3> ().ReRoute3 ();
				return;
			}
		}
		//Display an error if nothing is found.  
		Debug.Log ("No reroute3 found for " + gameObject.name);
	}
}
