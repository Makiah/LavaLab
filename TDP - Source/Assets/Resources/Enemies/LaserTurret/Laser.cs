using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class Laser : MonoBehaviour  {
//	Vector2 mouse;
//	RaycastHit hit;
//	float range = 100.0f;
//	LineRenderer line;
//	[SerializeField] private Material lineMaterial;
//
//	void Start() {
//		line = GetComponent<LineRenderer>();
//		line.SetVertexCount(2);
//		line.renderer.material = lineMaterial;
//		line.SetWidth(0.1f, 0.25f);
//	}
//
//	void Update() {
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		if (Physics.Raycast(ray, out hit, range)) {
//			if (Input.GetMouseButton(0)) {
//				line.enabled = true;
//				line.SetPosition(0, transform.position);
//				line.SetPosition(1, hit.point + hit.normal);
//			}else
//				line.enabled = false;
//		}
//
//	}

	//Private components.  
//	private Vector2 mouse;
//	private RaycastHit hit;
//	private LineRenderer line;
//
//	//Fields that should be filled in the Inspector.  
//	[SerializeField] private float maxRange = 100.0f;
//	[SerializeField] private Material lineMaterial = null;
//	[SerializeField] private bool targetMouse = false;
//
//	public void Enable() {
//		line = GetComponent<LineRenderer>();
//		line.SetVertexCount(2);
//		line.GetComponent <Renderer> ().material = lineMaterial;
//		line.SetWidth(0.1f, 0.25f);
//		if (targetMouse)
//			StartCoroutine (LaserToMouse ());
//		else
//			StartCoroutine (EnableLaser ());
//	}
//
//	IEnumerator LaserToMouse() {
//		//Create the ray.  
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		//Raycast returns true if the ray hits a collider.  
//		if (Physics2D.Raycast(ray, out hit, maxRange)) {
//			if (Input.GetMouseButton(0)) {
//				line.enabled = true;
//				line.SetPosition(0, transform.position);
//				line.SetPosition(1, hit.point + hit.normal);
//			} else
//				line.enabled = false;
//		}
//
//		yield return null;
//
//	}
//
//	IEnumerator EnableLaser() {
//		//Create the ray.  
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		//Raycast returns true if the ray hits a collider.  
//		if (Physics2D.Raycast(ray, out hit, maxRange)) {
//			if (Input.GetMouseButton(0)) {
//				line.enabled = true;
//				line.SetPosition(0, transform.position);
//				line.SetPosition(1, hit.point + hit.normal);
//			} else
//				line.enabled = false;
//		}
//
//		yield return null;
//
//	}
}