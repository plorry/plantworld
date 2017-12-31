using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileItemBehavior : MonoBehaviour {
	public string myName;
	public SmartGridBehavior myGrid;
	private Vector2 myCoords;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string GetName() {
		return myName;
	}

	override public string ToString() {
		return GetName();
	}

	public TileBehavior GetTile() {
		return myGrid.GetTileAt((int)myCoords.x, (int)myCoords.y);
	}
}
