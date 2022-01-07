using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class netManager : NetworkManager {

	public override void OnClientConnect(NetworkConnection conn) {
		// I actually don't know why this has to be here but at this point
		// i'm too afraid to ask
	}

}
