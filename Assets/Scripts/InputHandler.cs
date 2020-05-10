using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour {
	public CursorBehavior myCursor;

	public static InputHandler Instance { get; private set; }

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		myCursor = CursorBehavior.Instance;
	}
	
	// Update is called once per frame
	void Update () {

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

	private void OnMove(InputValue value) {
		Vector2 dir = value.Get<Vector2>();
		if (dir.x > 0.1) {
			Right();
		} else if (dir.x < -0.1) {
			Left();
		}

		if (dir.y > 0.1) {
			Up();
		} else if (dir.y < -0.1) {
			Down();
		}
	}

	private void OnSubmit() {
		Select();
	}

	private void OnCancel() {
		Deselect();
	}
}
