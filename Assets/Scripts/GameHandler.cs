using System.Collections;
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

		InitCursor();
		InitPlayers();
		InitMap();
		InitUnits();

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
			p.CacheMyUnits();
		}
	}

	public void InitMap () {
		smartGrid = SmartGridBehavior.Instance;
	}

	public void InitCursor () {
		myCursor = CursorBehavior.Instance;
	}

	public CursorBehavior GetCursor() {
		return myCursor;
	}

	public void DisplayTurnMessage (string message) {
		turnIdicator.text = message;
		turnIdicator.transform.position = new Vector2(-2, -8);
	}

	public List<Unit> GetAllUnits () {
		return allUnits;
	}

	public void AddUnit (Unit unit) {
		allUnits.Add(unit);
		playerManager.GetPlayers().ForEach(p => p.CacheMyUnits());
	}

	public void DestroyUnit (Unit unit) {
		allUnits.Remove(unit);
		playerManager.GetPlayers().ForEach(p => p.CacheMyUnits());
	}
}
