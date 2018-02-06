using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {
	protected List<Unit> myUnits;
	protected bool myTurn;
	protected string myName = "emptyName";
	protected List<Player> enemies;
	protected List<Player> allies;

	void Awake () {
		myUnits = new List<Unit>();
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetName (string name) {
		myName = name;
	}

	public string GetName () {
		return myName;
	}

	public static Player MakePlayer(string name) {
		Player p = new Player();
		p.Start();
		p.myName = name;
		return p;
	}

	public void InitPlayer (string name) {
		myName = name;
	}

	public void AddUnit (Unit unit) {
		unit.belongsTo = this;
		myUnits.Add(unit);
	}

	public List<Unit> GetUnits () {
		return myUnits;
	}

	public virtual void StartTurn () {
		myTurn = true;
		WakeMyUnits ();
	}

	public virtual void EndTurn () {
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

	public void CheckGridState () {
		if (AreAllUnitsExhausted() == true) EndTurn();
	}

	public bool AreAllUnitsExhausted() { 
		bool value = myUnits.TrueForAll(
			x => x.exhausted == true
		);
		return value;
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
