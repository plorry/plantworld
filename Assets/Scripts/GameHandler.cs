﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GameHandler : MonoBehaviour {
	private SmartGridBehavior smartGrid;
	private PlayerManager playerManager;
	private List<Player> players;
	private CursorBehavior myCursor;
	private List<Unit> allUnits;
	private Camera camera;
	public Text turnIdicator;

	public static GameHandler Instance { get; private set; }
	// For now, hard-coding unit values into code - eventually will extract from Tile files

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		allUnits = new List<Unit>();
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
		playerManager = PlayerManager.Instance;

		playerManager.AddNewPlayer("player1", true);
		playerManager.AddNewPlayer("player2", false);

		playerManager.InitQueue();
	}

	public void InitUnits () {
		foreach(Player p in playerManager.GetPlayers()) {
			smartGrid.InstantiateUnits(p);
			allUnits = allUnits.Concat(p.GetUnits()).ToList();
		}
	}

	public void InitMap () {
		smartGrid = SmartGridBehavior.Instance;
	}

	public void InitCursor () {
		myCursor = CursorBehavior.Instance;
	}

	public void DisplayTurnMessage (string message) {
		turnIdicator.text = message;
		turnIdicator.transform.position = new Vector2(-2, -8);
	}

	public List<Unit> GetAllUnits () {
		return allUnits;
	}
}
