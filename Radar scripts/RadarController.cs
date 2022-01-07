using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour {
	public GameObject refresh;
	public GameObject radar;
	public Camera mainCam;
	public float scale = 100;

	// Execute each frame
	void Update () {
		// Scan for ships
		// HOW IT WORKS:
		// 	Object has a a small width and huge height like this "|"
		// 	It rotates around itself constantly and whenever it collides with an object
		// 	It triggers the object's collide event
		refresh.transform.RotateAround(radar.transform.position, Vector3.back, scale * Time.deltaTime);
	}
}
