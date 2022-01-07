using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class ZoomController : MonoBehaviour, IPointerClickHandler {
	// Get the main camera
	public Camera mainCam;
	// Get the zoom button text
	public UnityEngine.UI.Text text;
	// Store the zoom scale
	private int scale = 4;
	// Store the zoom scale in nautical metres
	private float nm = 3;
	// Get the Refresh script
	public Refresh rf;

	// KM = NM / 1.852
	// NM = KM * 1.852

	// Executes when activated
	public void OnEnable() {
		changeZoom(5);
	}

	// Executes when mouse button is pressed
	public void OnPointerClick(PointerEventData eventData) {
		// Zoom out if left button is pressed
		if (eventData.pointerId == -1)
			changeZoom(1);
		// Zoom in if right button is pressed
		else if (eventData.pointerId == -2)
			changeZoom(-1);
	}

	// Control the zoom
	void changeZoom(int n) {
		if ((scale == 9 && n > 0) || (scale == 1 && n < 0)) return;
		if (n > 0) scale += 1;
		else scale -= 1;
		if (scale == 1) {
			mainCam.orthographicSize = 0.56f;
			nm = 0.25f;
		}
		else if (scale == 2) {
			mainCam.orthographicSize = 1.12f;
			nm = 0.5f;
		}
		else if (scale == 3) {
			mainCam.orthographicSize = 1.675f;
			nm = 0.75f;
		}
		else if (scale == 4) {
			mainCam.orthographicSize = 3.35f;
			nm = 1.5f;
		}
		else if (scale == 5) {
			mainCam.orthographicSize = 6.7f;
			nm = 3;
		}
		else if (scale == 6) {
			mainCam.orthographicSize = 13.4f;
			nm = 6;
		}
		else if (scale == 7) {
			mainCam.orthographicSize = 26.8f;
			nm = 12;
		}
		else if (scale == 8) {
			mainCam.orthographicSize = 53.6f;
			nm = 24;
		}
		else if (scale == 9) {
			mainCam.orthographicSize = 107.2f;
			nm = 48;
		}
			
		// Display the zoom level
		text.text = nm + " nm";
	}
}
