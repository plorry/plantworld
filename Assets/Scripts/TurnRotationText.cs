using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnRotationText : MonoBehaviour {
	private Text t;

	// Use this for initialization
	void Start () {
		t = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y);
	}

	public void AnnounceEndOfTurn () {
		t.text = "End of Turn";
	}

	public void AnnounceNextTurn () {
		t.text = "Next Turn";
	}

	public void SlideAcross () {
		transform.position = new Vector2(-200, 200);
	}
}
