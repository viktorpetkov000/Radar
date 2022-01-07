using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;


public class MenuController : MonoBehaviour {

	// When exit is selected
	public void Exit() {
		// Exit the application
		Application.Quit();
	}
		
	// When instructor mode is selected
	public void hostMenuStart() {
		// Change scene to "test"
		NetworkManager.singleton.ServerChangeScene("test");
		// Start server
		NetworkManager.singleton.StartHost();
	}
		
	// When student mode is selected
	public void joinMenuStart() {
		// Join server
		NetworkManager.singleton.StartClient();
	}
}
