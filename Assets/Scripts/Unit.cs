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
	public int speed = 5;
	private TileBehavior homeTile;
	private TileBehavior currentTile;
	private TileBehavior destinationTile;
	private bool moving = false;
	public bool exhausted = false;
	private int movesLeft = 0;
	// Stats
	private int maxHitPoints = 6;
	private int hitPoints;
	private AudioPlayer audioPlayer;

	// Use this for initialization
	void Start () {
		id = Guid.NewGuid();
		transform.position = currentTile.transform.position;
		destinationTile = currentTile = homeTile;

		hitPoints = maxHitPoints;
		audioPlayer = AudioPlayer.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePosition();
	}

	public bool IsMoving () {
		return moving;
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
			movesLeft--;
		}
	}

	public void Exhaust () {
		exhausted = true;
	}

	public void WakeUp () {
		exhausted = false;
		movesLeft = speed;
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
	
	public void Attack (Unit enemy) {
		print(string.Format("{0} has attacked {1}", this, enemy));
		enemy.Hit(2);
		audioPlayer.PlayOnce("hit");
	}

	public float DistanceTo (Unit otherUnit) {
		return Math.Abs((transform.position - otherUnit.transform.position).magnitude);
	}

	public Unit ClosestAlly () {
		return GetAllies().Aggregate(
			(curMin, x) => (curMin == null || (DistanceTo(x) < DistanceTo(curMin)) ? x : curMin)
		);
	}

	public Unit ClosestEnemy () {
		return GetEnemies().Aggregate(
			(curMin, x) => (curMin == null || (DistanceTo(x) < DistanceTo(curMin)) ? x : curMin)
		);
	}

	public int MovesLeft () {
		return movesLeft;
	}

	public bool IsOutOfMoves () {
		return movesLeft == 0;
	}

	public void Hit (int damage) {
		hitPoints -= damage;
		UpdateStatus();
	}
	// catch-all method to update a unit's status in the event that the state has changed
	private void UpdateStatus () {
		if (hitPoints == 0) Kill();
	}

	private void Kill () {
		Debug.Log("I am dead");
	}
}