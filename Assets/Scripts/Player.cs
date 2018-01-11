using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private List<Unit> myUnits;
	private bool myTurn;
	private string myName = "emptyName";
	private List<Player> enemies;
	private List<Player> allies;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static Player MakePlayer(string name) {
		Player p = new Player();
		p.myName = name;
		return p;
	}

	public void InitPlayer (string name) {
		myName = name;
	}

	public void AddUnit (Unit unit) {
		myUnits.Add(unit);
	}

	public List<Unit> GetUnits () {
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
		foreach (Unit unit in GetUnits()) {
			unit.WakeUp();
		}
	}

	public void ExhaustMyUnits () {
		foreach (Unit unit in GetUnits()) {
			unit.Exhaust();
		}
	}

	public override string ToString() {
		return myName;
	}

	public bool IsMyUnit (Unit unit) {
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
