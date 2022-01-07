using UnityEngine;

public class CameraController : MonoBehaviour {
	// Get the player's ship transform
	public Transform playerTransform;
	// Camera depth
	public int depth = -20;

	// Update is called once per frame
	void Update() {
		// If there is a player object
		if (playerTransform != null) {
			// Set camera position
			transform.position = playerTransform.position + new Vector3(0, 0, depth);
		}
	}

	// Set the target to the player transform
	public void setTarget(Transform target) {
		playerTransform = target;
	}
}