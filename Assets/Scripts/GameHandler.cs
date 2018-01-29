using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GameHandler : MonoBehaviour {
	public SmartGridBehavior smartGrid;
	private PlayerManager playerManager;
	private List<Player> players;
	private CursorBehavior myCursor;
	private Camera camera;
	public Text turnIdicator;
	// For now, hard-coding unit values into code - eventually will extract from Tile files

	// Use this for initialization
	void Start () {
		camera = Camera.main;

		InitPlayers();
		InitMap();
		InitUnits();
		InitCursor();

		DisplayTurnMessage("Player 1 turn");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitPlayers() {
		playerManager = gameObject.AddComponent<PlayerManager>() as PlayerManager;
		playerManager.SetGameHandler(this);

		players = new List<Player>();

		players.Add(Player.MakePlayer("player1"));
		players.Add(Player.MakePlayer("player2"));

		playerManager.InitPlayers(players);
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

	public void DisplayTurnMessage (string message) {
		turnIdicator.text = message;
		turnIdicator.transform.position = new Vector2(-2, -8);
	}
}
