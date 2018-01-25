using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
	public string myName;
	public SmartGridBehavior myGrid;
	private Vector2 myCoords;
	public Player belongsTo;
	private int speed = 5;
	private TileBehavior homeTile;
	private TileBehavior currentTile;
	private TileBehavior destinationTile;
	private bool moving = false;
	public bool exhausted = false;

	// Use this for initialization
	void Start () {
		transform.position = currentTile.transform.position;
		destinationTile = currentTile = homeTile;
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePosition();
	}

	private void UpdatePosition () {
		if (AtDestination() && moving == true) {
			moving = false;
			SetCurrentTile(destinationTile);
		}

		if (currentTile != destinationTile) {
			moving = true;
			transform.position = Vector2.MoveTowards(transform.position, destinationTile.transform.position, 0.05f);
		} else {
			transform.position = currentTile.transform.position;
		}
	}

	public string GetName() {
		return myName;
	}

	override public string ToString() {
		return GetName();
	}

	public void SetOwner (Player player) {
		belongsTo = player;
	}

	public TileBehavior MyTile() {
		return currentTile;
	}

	public void SetDestinationTile (TileBehavior tile) {
		destinationTile = tile;
	}

	private bool AtDestination () {
		return (transform.position - destinationTile.transform.position).sqrMagnitude < 1.01;
	}

	private bool CanMove () {
		return moving == false;
	}

	public void SetHomeTile (TileBehavior tile) {
		homeTile = tile;
	}

	public void SetCurrentTile (TileBehavior tile) {
		if (currentTile) currentTile.RemoveContent(this);
		currentTile = tile;
		currentTile.AddContent(this);
	}

	public TileBehavior GetCurrentTile () {
		return currentTile;
	}

	public void MoveTo(TileBehavior tile) {
		SetDestinationTile(tile);
	}

	public void Move (string direction) {
		if (CanMove()) {
			MoveTo(currentTile.GetNeighbour(direction));
		}
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

	public void Deselect () {
		currentTile.RemoveContent(this);
		destinationTile = currentTile = homeTile;
		homeTile.AddContent(this);
	}

	public void LockIn () {
		SetHomeTile(currentTile);
		Deselect();
		Exhaust();
	}
}
