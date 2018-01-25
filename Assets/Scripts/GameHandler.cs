using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameHandler : MonoBehaviour {
	public SmartGridBehavior smartGrid;
	private PlayerManager playerManager;
	private List<Player> players;
	private CursorBehavior myCursor;
	private Camera camera;
	// For now, hard-coding unit values into code - eventually will extract from Tile files

	// Use this for initialization
	void Start () {
		camera = Camera.main;

		InitPlayers();
		InitMap();
		InitUnits();
		InitCursor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitPlayers() {
		players = new List<Player>();

		players.Add(Player.MakePlayer("player1"));
		players.Add(Player.MakePlayer("player2"));

		playerManager = PlayerManager.MakePlayerManager(players);
	}

	public void InitUnits () {
		foreach(Player p in players) {
			smartGrid.InstantiateUnits(p);
		}
	}

	public void InitMap () {
		
	}

	public void InitCursor () {
		myCursor = CursorBehavior.MkCursor(smartGrid, camera);
	}
}
