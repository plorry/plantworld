using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileItemBehavior : MonoBehaviour {
	public string myName;
	public SmartGridBehavior myGrid;
	private Vector2 myCoords;
	public readonly string belongsTo = "player";
	private int speed = 5;
	private TileBehavior homeTile;
	private TileBehavior currentTile;
	public bool exhausted = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = currentTile.transform.position;
	}

	public string GetName() {
		return myName;
	}

	override public string ToString() {
		return GetName();
	}

	public TileBehavior MyTile() {
		return currentTile;
	}

	public void SetCurrentTile (TileBehavior tile) {
		if (currentTile) currentTile.RemoveContent(this);
		currentTile = tile;
		currentTile.AddContent(this);
	}

	public void MoveTo(TileBehavior tile) {
		SetCurrentTile(tile);
	}

	public void Move (string direction) {
		MoveTo(currentTile.GetNeighbour(direction));
	}

	public void Exhaust () {
		exhausted = true;
	}

	public void WakeUp () {
		exhausted = false;
	}

	public int GetSpeed () {
		return speed;
	}
}
