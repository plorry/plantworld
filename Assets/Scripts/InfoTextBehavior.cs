using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoTextBehavior : MonoBehaviour {
	public CursorBehavior myCursor;
	private TileBehavior currentTile;
	private Text myText;

	// Use this for initialization
	void Start () {
		myText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		string newText = "";
		myCursor.GetTile().GetContents().ForEach(item => newText += item.ToString());
		myText.text = newText;
	}
}
