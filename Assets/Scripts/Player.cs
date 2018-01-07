using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private List<TileItemBehavior> myUnits;
	private bool myTurn;
	private string myName;
	private List<Player> enemies;
	private List<Player> allies;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddUnit (TileItemBehavior unit) {
		myUnits.Add(unit);
	}

	public List<TileItemBehavior> GetUnits () {
		return myUnits;
	}

	public void StartTurn () {
		myTurn = true;
		WakeMyUnits ();
	}

	public void EndTurn () {
		myTurn = false;
		ExhaustMyUnits ();
	}

	public bool IsMyTurn () {
		return myTurn;
	}

	public void WakeMyUnits () {
		foreach (TileItemBehavior unit in GetUnits()) {
			unit.WakeUp();
		}
	}

	public void ExhaustMyUnits () {
		foreach (TileItemBehavior unit in GetUnits()) {
			unit.Exhaust();
		}
	}

	public override string ToString() {
		return myName;
	}

	public bool IsMyUnit (TileItemBehavior unit) {
		return myUnits.Contains(unit);
	}

	public bool IsEnemy (Player p) {
		return enemies.Contains(p);
	}

	public bool IsAlly (Player p) {
		return allies.Contains(p);
	}

	public List<Player> GetEnemies () {
		return enemies;
	}

	public List<Player> GetAllies () {
		return allies;
	}
}
