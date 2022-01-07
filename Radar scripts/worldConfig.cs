using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class worldConfig : NetworkBehaviour {
	// Get the time button text
	public UnityEngine.UI.Text timeInfo;
	// Get the movement script
	public movement playerScript;
	// Track time.timeScale
	public float timeScale = 1;

	// Executes each frame
	void Update() {
		// Quit if escape is pressed
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	// Set the time flow for all players
	[ClientRpc]
	public void RpcTimeScale(float scale) {
		// Check if time is multiplied more than 64 or less than 1
		if (timeScale * scale <= 64 && timeScale * scale >= 1) {
			// Increase/decrease time flow
			timeScale *= scale;
			Time.timeScale = timeScale;
			// Change button text to indicate current time flow rate
			changeButton();
		}
	}

	// Change button text only on server
	[Server]
	public void changeButton() {
		timeInfo.text = "Time: " + timeScale + "x";
	}

}
