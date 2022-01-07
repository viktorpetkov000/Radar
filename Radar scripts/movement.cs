using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class movement : NetworkBehaviour {
	// Get ship object
	public UnityEngine.GameObject ship;
	// Ship controls
	private UnityEngine.UI.Slider hSlider;
	private UnityEngine.UI.Slider hfSlider;
	private UnityEngine.UI.Slider vSlider;
	private UnityEngine.UI.Slider vfSlider;
	// Ship indicators
	private UnityEngine.UI.Text vIndicator;
	private UnityEngine.UI.Text vfIndicator;
	private UnityEngine.UI.Text gIndicator;
	private UnityEngine.UI.Text gfIndicator;
	// Ship velocity
	public float speed = 0;
	// Acceleration speed
	private float accspeed = 0;
	// Rotation
	public float desiredRot;
	// Speed of rotation acceleration
	private float rotspeed = 0.5f;
	// Damping of rotation
	private float dampingR = 0.15f;
	// Info indicator
	private UnityEngine.UI.Text info;
	// Ship engine
	private float scaleEngine = 400;
	// Ship max knots
	private float shipScale = 6;
	// Rotation speed
	public float rot = 0;
	// World configuration
	private worldConfig world;
	// Spawn configuration
	private SpawnController spawn;
	// Acceleration
	private float acc = 0;
	// Rotation
	private float turn = 0;
	// How fast to change rotation acceleration
	public float turnScale = 6.5f;
	// Speed in knots
	public float speedKn = 0;
	// Speed in kilometres
	public float speedKm = 0;
	// Speed in metres per second
	public float mps = 0;
	// Reference to the Refresh script
	public Refresh refresh;

	// Remove collision on newly created ship
	[SyncVar]
	public bool col = false;

	// When simulation begins
	private void OnEnable() {
		// Get the actual objects
		hSlider = GameObject.Find("Gyro").GetComponent<Slider>();
		hfSlider = GameObject.Find("Gyro Follow").GetComponent<Slider>();
		vSlider = GameObject.Find("Velocity").GetComponent<Slider>();
		vfSlider = GameObject.Find("Velocity Follow").GetComponent<Slider>();
		info = GameObject.Find("Info").GetComponent<Text>();
		vIndicator = GameObject.Find("vIndicator").GetComponent<Text>();
		vfIndicator = GameObject.Find("vfIndicator").GetComponent<Text>();
		gIndicator = GameObject.Find("gIndicator").GetComponent<Text>();
		gfIndicator = GameObject.Find("gfIndicator").GetComponent<Text>();
		spawn = GameObject.Find("Spawn container").GetComponent<SpawnController>();
		refresh = GameObject.Find("refresh").GetComponent<Refresh>();
		// Get the ship's rotation
		desiredRot = ship.transform.eulerAngles.z;
	}

	// Execute each frame
	void Update() {
		// Check if this is the client's script
		if (!isLocalPlayer)
			return;

		// Get value of velocity slider
		float accRaw = vSlider.value - 4;
		// Velocity acceleration
		// Change velocity acceleration each second
		// Depending on whether slider value is high or low
		// Time.deltaTime = 1 second
		if (acc < accRaw)
			acc += 1 * Time.deltaTime;
		else if (acc > accRaw)
			acc -= 1 * Time.deltaTime;
		// Normalize velocity acceleration and remove flicker
		else if (accRaw == 0) {
			if (Mathf.RoundToInt(acc) == 0)
				acc = 0;
			else if (acc < 0)
				acc += 1 * Time.deltaTime;
			else if (acc > 0)
				acc -= 1 * Time.deltaTime;
		}
		if (Mathf.RoundToInt(acc) == accRaw)
			acc = accRaw;
		if (acc > 4)
			acc = 4;
		else if (acc < -4)
			acc = -4;
		// Update velocity acceleration slider
		vfSlider.value = Mathf.RoundToInt(acc+4);
		// Update velocity indicator
		vIndicator.text = accRaw.ToString();
		// Update velocity acceleration indicator
		vfIndicator.text = Mathf.RoundToInt(acc).ToString();

		// Velocity
		// Increase/decrease velocity depending on the acceleration
		if ((acc > 0 && speed < acc) || (acc < 0 && speed > acc))
			speed += (accspeed + (acc / scaleEngine)) * Time.deltaTime;
		else if ((acc > 0 && speed > acc) || (acc < 0 && speed < acc))
			speed -= (accspeed + (acc / scaleEngine)) * Time.deltaTime;
		// Normalize velocity and remove flicker
		else if (Mathf.RoundToInt(acc) == 0) {
			if (System.Math.Round(speed,2) == 0)
				speed = 0;
			else if (speed < 0)
				speed += (accspeed + (4 / scaleEngine)) * Time.deltaTime;
			else if (speed > 0)
				speed -= (accspeed + (4 / scaleEngine)) * Time.deltaTime;
		}
		if (System.Math.Round(speed,2) == acc)
			speed = acc;
		if (speed > 4)
			speed = 4;
		else if (speed < -4)
			speed = -4;
		// Get the speed in knots
		speedKn = speed * shipScale;
		// Get the speed in kilometres
		speedKm = 1.852f * speedKn;
		// Get the speed in miles per second
		mps = (speedKm * 1000) / 3600;
		// Move ship forward relative to rotation by miles per second each second
		ship.transform.position += transform.up * Time.deltaTime * (mps/1000);

		// Rotation acceleration
		// Get value of gyroscope slider
		float turnRaw = hSlider.value - 35;
		// Fix flicker on faster timescale
		if (Time.timeScale == 100)
			turnScale = 0.1f;
		else if (Time.timeScale > 32)
			turnScale = 0.825f;
		else if (Time.timeScale > 16)
			turnScale = 1.625f;
		else if (Time.timeScale > 8)
			turnScale = 3.25f;
		else
			turnScale = 6.5f;
		// Increase/decrease rotation acceleration
		if (turn < turnRaw)
			turn += turnScale * Time.deltaTime;
		else if (turn > turnRaw)
			turn -= turnScale * Time.deltaTime;
		// Normalize rotation acceleration
		if (Mathf.RoundToInt(turn) == turnRaw)
			turn = turnRaw;
		if (turn > 35)
			turn = 35;
		else if (turn < -35)
			turn = -35;
		// Update rotation acceleration slider
		hfSlider.value = Mathf.RoundToInt(turn+35);
		// Update gyroscope indicator
		gIndicator.text = turnRaw.ToString();
		// Update rotation/gyroscope acceleration indicator
		gfIndicator.text = Mathf.RoundToInt(turn).ToString();

		// Rotate when ship is in movement
		if (turn < 0 && mps != 0)
			desiredRot -= (rotspeed + Mathf.Abs(turn) / 7) * Time.deltaTime;
		else if (turn > 0 && mps != 0)
			desiredRot += (rotspeed + Mathf.Abs(turn) / 7) * Time.deltaTime;
		else {}
		// Invert rotation if ship's velocity is reversed
		if (mps < 0 && desiredRot > 0)
			rot = -Mathf.Abs(desiredRot);
		else if (mps < 0 && desiredRot < 0)
			rot = Mathf.Abs(desiredRot);
		else
			rot = desiredRot;

		// Get rotation in a quaternion object
		var desiredRotQ = Quaternion.Euler(ship.transform.eulerAngles.x, ship.transform.eulerAngles.y, rot * -1);
		// Rotate the ship via lerping between the current rotation and the calculated rotation, slowed down by dampingR
		ship.transform.rotation = Quaternion.Lerp(ship.transform.rotation, desiredRotQ, Time.deltaTime * dampingR);
		// Get speed in knots rounded to 2 decimals
		double speedinfo = System.Math.Round(speedKn,2);
		// Get gyroscope rotation rounded to 1 decimal
		double rotationInfo = System.Math.Round(ship.transform.eulerAngles.z - 360, 1);
		// Invert rotation values
		if (Mathf.Round(ship.transform.eulerAngles.z) == 0)
			rotationInfo = 0;

		// Display information
		info.text = "Gyro: " + (rotationInfo*-1).ToString("0.0") + "° | " + speedinfo.ToString("0.00") + " knots";

	}

	// Detect collisions
	void OnCollisionEnter2D() {
		
	}

	// Called when game is started
	public override void OnStartLocalPlayer() {
		// Update scripts to reference client's ship
		spawn.playerScript = this;
		refresh.playerScript = this;
		// Make camera follow the ship
		Camera.main.GetComponent<CameraController>().setTarget(gameObject.transform);
	}

	// Activate collision on this ship for every client
	[Command]
	public void CmdChange() {
		col = true;
	}
}
