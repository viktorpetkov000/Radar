using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ARPAController : NetworkBehaviour {
	// Get Refresh script
	public Refresh radar;
	// Get dropdown list of ships
	public UnityEngine.UI.Dropdown shipOption;
	// Get bearing text
	public UnityEngine.UI.Text bearingText;
	// Get VRM text
	public UnityEngine.UI.Text vrmText;
	// Get speed text
	public UnityEngine.UI.Text speedText;
	// Get trail toggle
	public UnityEngine.UI.Toggle trailToggle;
	// Store the list of ships' configuration
	public List<Ship> ships = new List<Ship>();
	// Store the dropdown id's of the ships
	public List<string> shipNames;

	// Add a "No Ship" option to the dropdown
	void OnEnable() {
		shipNames.Add("No ship");
	}

	// Ship object
	public class Ship {
		// Getters and setters for the properties
		public int Id { get; set; }
		public double Bearing { get; set; }
		public double Vrm { get; set; }
		public double Speed { get; set; }
		// Ship object constructor
		public Ship(int id, double bearing, double vrm, double speed) {
			// Id of the ship
			Id = id;
			// Ship's bearing
			Bearing = bearing;
			// Ship's distance
			Vrm = vrm;
			// Ship's speed
			Speed = speed;
		}
	}

	// Add a ship to the dropdown list
	public void addShip(int id, double bearing, double vrm, double speed) {
		// Check if "No ship" option is selected
		if (shipOption.captionText.text.Equals("No ship")) {
			// Remove all data from ARPA indicators
			bearingText.text = "Bearing: ";
			vrmText.text = "VRM: ";
			speedText.text = "Speed: ";
			return;
		}
		// Go through all the ship objects
		for (int i = 0; i < ships.Count; i++) {
			// Check if ship already exists in dropdown list
			if (ships[i].Id == id) {
				// Check if ship's distance has changed
				if (ships[i].Vrm == vrm)
					return;
				// Set the new data
				ships[i].Bearing = bearing;
				ships[i].Vrm = vrm;
				ships[i].Speed = speed;
				bearingText.text = "Bearing: " + ships[i].Bearing + "°";
				vrmText.text = "VRM: " + ships[i].Vrm + "nm";
				speedText.text = "Speed: " + ships[i].Speed + " knots";
				return;
			}
		}
		// Create a new ship object
		ships.Add(new Ship(id, bearing, vrm, speed));
		// Add the new ship object to the dropdown list
		shipNames.Add("Ship " + (id + 1));
		// Clear the dropdown list
		shipOption.ClearOptions();
		// Add all the ship objects to the dropdown list
		shipOption.AddOptions(shipNames);
	}

	// Selected a ship to monitor
	public void chooseShip() {
		// Check if "No ship" option is selected
		if (shipOption.captionText.text.Equals("No ship")) {
			// Remove all data from ARPA indicators
			bearingText.text = "Bearing: ";
			vrmText.text = "VRM: ";
			speedText.text = "Speed: ";
			return;
		}
		// Go through all the ship objects
		for (int i = 0; i < ships.Count; i++) {
			// Check if ship's id matches the option id
			if (("Ship " + (ships[i].Id + 1)).Equals(shipOption.captionText.text)) {
				// Display all the data
				bearingText.text = "Bearing: " + ships[i].Bearing + "°";
				vrmText.text = "VRM: " + ships[i].Vrm + "nm";
				speedText.text = "Speed: " + ships[i].Speed + " knots";
				break;
			}
		}
	}
}
