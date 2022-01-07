using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Refresh : NetworkBehaviour {
	// Store the newly scanned ship
	public GameObject mark;
	// Store all the current ships' markers
	public List<GameObject> marks = new List<GameObject>();
	// Store all the current ships' data
	public List<GameObject> props = new List<GameObject>();
	// Get a reference to the movement script
	public movement playerScript;
	// Get a reference to the ARPAController script
	public ARPAController arpa;

	// When a collision is detected
	public void OnCollisionEnter2D (Collision2D col) {
		// Check if the collider is the local player
		if (col.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
			return;
		// Check if the collider has collision turned on
		if (!col.gameObject.GetComponent<movement>().col)
			return;
		// Check if the player has collision turned on
		if (!playerScript.col)
			return;
		// Store the new ship in marks
		marks.Add(Instantiate(mark, col.transform.position, col.transform.rotation));
		// Go through all the props
		for (int i = 0; i < props.Count; i++) {
			// Check if a ship has already been scanned before
			if (props[i].GetComponent<NetworkIdentity>().netId == col.gameObject.GetComponent<NetworkIdentity>().netId) {
				// Destroy the marker
				Destroy(marks[i]);
				// Remove the ship from markers and props
				marks.RemoveAt(i);
				props.RemoveAt(i);
			}
		}
		// Get ship's bearing through the scanner's rotation
		double bearing = System.Math.Round(((transform.rotation.eulerAngles.z - 360) * -1), 1);
		// Add the object to props
		props.Add(col.gameObject);
		// Get the distance between the player's ship and the newly scanned ship
		float distance = Vector2.Distance(playerScript.ship.transform.position, col.gameObject.transform.position);
		// Convert the distance to nautical miles
		double vrm = System.Math.Round(distance / 1.852f, 2);
		// Get the newly scanned ship's current speed in knots (!NOT WORKING)
		double speed = System.Math.Round(col.gameObject.GetComponent<movement>().speedKn, 2);
		// Add the newly scanned ship to the ARPA dropdown list of ships
		arpa.addShip(((int.Parse(col.gameObject.GetComponent<NetworkIdentity>().netId.ToString())) - 5), bearing, vrm, speed);
	}
}
