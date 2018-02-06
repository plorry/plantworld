using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	private List<Player> players;
	private Queue<Player> turnQueue;
	private Player currentPlayer;

	public static PlayerManager Instance { get; private set; }

	[System.Serializable]
	public struct PlayerPrefab {
		public Player prefab;
		public string title;
	}

	public List<PlayerPrefab> playerPrefabs;

	void Awake () {
		Instance = this;
		players = new List<Player>();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (currentPlayer.IsMyTurn() == false) {
			NextTurn();
		}
	}

	public List<Player> GetPlayers () {
		return players;
	}

	public void NextTurn () {
		turnQueue.Enqueue(currentPlayer);
		currentPlayer = turnQueue.Dequeue();
		currentPlayer.StartTurn();

		GameHandler.Instance.DisplayTurnMessage(
			string.Format("{0}'s turn", currentPlayer.ToString())
		);
	}

	public void InitQueue () {
		turnQueue = new Queue<Player>();

		foreach (Player p in players) {
			turnQueue.Enqueue(p);
		}
		currentPlayer = turnQueue.Dequeue();
		currentPlayer.StartTurn();
	}

	public Player GetCurrentPlayer () {
		return currentPlayer;
	}

	public void AddNewPlayer (string name, bool human = false) {
		Player newPlayer;
		newPlayer = (human == true) ?
			Instantiate(playerPrefabs.Find(x => x.title == "human").prefab) :
			Instantiate(playerPrefabs.Find(x => x.title == "ai").prefab);
		newPlayer.SetName(name);

		players.Add(newPlayer);
	}
}
