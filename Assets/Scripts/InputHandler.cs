using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	public CursorBehavior myCursor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Joystick1Button1)) {
			Select();
		} else if (Input.GetKeyDown(KeyCode.Joystick1Button2)) {
			Deselect();
		}
		if (Input.GetMouseButtonDown(0)) {
			Select();
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Deselect();
		}
		if (Input.GetAxis("Horizontal") > 0.5) {
			Right();
		} else if (Input.GetAxis("Vertical") > 0.5) {
			Up();
		} else if (Input.GetAxis("Horizontal") < -0.5) {
			Left();
		} else if (Input.GetAxis("Vertical") < -0.5) {
			Down();
		}

		if (Input.GetAxis("DPadX") > 0.5) {
			Right();
		} else if (Input.GetAxis("DPadY") > 0.5) {
			Up();
		} else if (Input.GetAxis("DPadX") < -0.5) {
			Left();
		} else if (Input.GetAxis("DPadY") < -0.5) {
			Down();
		}
	}

	private void Select () {
		myCursor.Select();
	}

	private void Deselect() {
		myCursor.Deselect();
	}

	private void Left() {
		myCursor.Left();
	}

	private void Right() {
		myCursor.Right();
	}

	private void Up() {
		myCursor.Up();
	}

	private void Down() {
		myCursor.Down();
	}
}
