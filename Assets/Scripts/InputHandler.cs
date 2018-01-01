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
		if (Input.GetMouseButtonDown(0)) {
			Select();
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Deselect();
		}
		if (Input.GetAxis("Horizontal") > 0.5) {
			myCursor.GetSelected().Move("right");
			print("right");
		}
	}

	private void Select () {
		myCursor.Select();
	}

	private void Deselect() {
		myCursor.Deselect();
	}
}
