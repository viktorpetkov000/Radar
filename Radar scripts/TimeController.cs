using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class TimeController : NetworkBehaviour, IPointerClickHandler {
	// Get the worldConfig script
	public worldConfig cfg;
	// When mouse is clicked
	public void OnPointerClick(PointerEventData eventData) {
		// Speed up time twice if left button is clicked
		if (eventData.pointerId == -1)
			cfg.RpcTimeScale(2);
		// Slow down time by half if right button is clicked
		else if (eventData.pointerId == -2)
			cfg.RpcTimeScale(0.5f);
	}
}