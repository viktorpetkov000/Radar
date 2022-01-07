using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class SpawnController : NetworkBehaviour {
	// Get the spawn container
	public GameObject spawn;
	// Get the movement script
	public movement playerScript;
	// Get the target cursor
	public GameObject target;
	// Get the coordinate's text
	public UnityEngine.UI.Text coordInfo;
	// Get the coordinate's input field
	public UnityEngine.UI.InputField coordInput;
	// Get the rotation's input field
	public UnityEngine.UI.InputField rotInput;
	// Store an empty position for later use to get mouse position
	private Vector3 pos;
	// Rotation value
	private float rot = 0;
	// State of getting the mouse position
	private bool gettingPos = false;
	// State of decreasing the rotation
	private bool decR = false;
	// State of increasing the rotation
	private bool incR = false;
	// Get the worldConfig script
	public worldConfig cfg;

	// Executes each frame
	void Update() {
		// Check if user is getting the mouse position
		if (gettingPos) {
			// Check if mouse is outside window or out of bounds
			if (Input.mousePosition.x > Camera.main.pixelWidth || Input.mousePosition.x < 0 || Input.mousePosition.y > Camera.main.pixelHeight || Input.mousePosition.y < 0)
				return;
			// Get the mouse position as coordinates
			pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			// Set the position to the mouse position
			target.transform.position = pos;
			// Display the new coordinates
			coordInfo.text = pos.ToString();
			// Check if mouse is clicked
			if (Input.GetMouseButtonDown(0)) {
				// Show the spawn container and hide target
				spawn.SetActive(true);
				target.SetActive(false);
				coordInput.text = pos.ToString();
				gettingPos = false;
			}
		}
		// Decrease rotation
		if (decR)
			rot -= 1 * Time.deltaTime*2;
		// Increase rotation
		if (incR)
			rot += 1 * Time.deltaTime*2;

		// Get rotation as quaternion
		var rotQ = Quaternion.Euler(playerScript.ship.transform.eulerAngles.x, playerScript.ship.transform.eulerAngles.y, rot * -1);
		// Set the ship's rotation
		playerScript.ship.transform.rotation = rotQ;
		// Format the ship's rotation as a string
		double rotStr = System.Math.Round(playerScript.ship.transform.eulerAngles.z - 360, 1);
		// Display the ship's rotation
		if (incR || decR)
			rotInput.text = (rotStr * -1).ToString() + "°";
	}

	// Hide the spawn container and show target when getting position
	public void choosePositionStart() {
		target.SetActive(true);
		spawn.SetActive(false);
		gettingPos = true;
	}

	// Spawn the player
	public void spawnPlayer() {
		// Set ship's position
		playerScript.ship.transform.position = pos;
		// Set ship's rotation variables
		playerScript.desiredRot = rot;
		playerScript.rot = rot;
		// Turn on ship's collision
		playerScript.CmdChange();
		// Hide the spawn container
		spawn.SetActive(false);
		// Activate ship
		playerScript.ship.SetActive(true);
		// Disable the update function
		enabled = false;
	}

	// Methods for checking rotation increase or decrease
	public void incDown() { incR = true; }
	public void incUp() { incR = false; }
	public void decDown() { decR = true; }
	public void decUp() { decR = false; }

	// Validate text for rotation input field
	public void validateGyro() {
		if (int.Parse(rotInput.text) > 360)
			rotInput.text = "360";
		else if (int.Parse(rotInput.text) < 0)
			rotInput.text = "0";
		else {}
		rot = float.Parse(rotInput.text);
	}

}
