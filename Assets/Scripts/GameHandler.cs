using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameHandler : MonoBehaviour {
	[System.Serializable]
	public class UnitPrefab {
		public string name;
		public Unit prefab;
	}

	private SmartGridBehavior smartGrid;
	private PlayerManager playerManager;
	private List<Player> players;

	public List<UnitPrefab> unitPrefabs;

	// For now, hard-coding unit values into code - eventually will extract from Tile files

	// Use this for initialization
	void Start () {
		InitPlayers();
		InitUnits();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitPlayers() {
		Player player1 = Player.MakePlayer("player1");
		Player player2 = Player.MakePlayer("player2");
		players = new List<Player>() { player1, player2 };
		playerManager = PlayerManager.MakePlayerManager(players);
	}

	public void InitUnits () {

	}

	public void InitMap () {
		
	}
}
