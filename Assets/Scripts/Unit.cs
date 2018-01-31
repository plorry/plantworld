using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour {
	private Guid id;
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
		id = Guid.NewGuid();
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
		if (CanMove()) {
			SetDestinationTile(tile);
		}
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
		SetCurrentTile(destinationTile);
		SetHomeTile(currentTile);
		Deselect();
		Exhaust();
		belongsTo.CheckGridState();
	}

	public bool BelongsTo (Player player) {
		return belongsTo.GetName() == player.GetName();
	}

	public bool IsAlly (Unit otherUnit) {
		return otherUnit.belongsTo.GetName() == belongsTo.GetName();
	}

	public bool IsEnemy (Unit otherUnit) {
		return !IsAlly(otherUnit);
	}

	public List<Unit> GetAllies () {
		return belongsTo.GetUnits();
	}

	public List<Unit> GetEnemies () {
		return GameHandler.Instance.GetAllUnits()
			.Except(GetAllies())
			.ToList();
	}

	public Guid GetId () {
		return id;
	}
}

public class UnitComparer : IEqualityComparer<Unit> {
	public bool Equals(Unit x, Unit y)
    {
		return x.GetId().Equals(y.GetId());
    }

    public int GetHashCode(Unit obj)
    {
		return obj.GetId().GetHashCode();
    }
}