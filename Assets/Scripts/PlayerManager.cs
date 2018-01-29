using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	private List<Player> players;
	private Queue<Player> turnQueue;
	private Player currentPlayer;

	public static PlayerManager Instance { get; private set; }

	void Awake () {
		Instance = this;
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

	public static PlayerManager MakePlayerManager (List<Player> players, GameHandler gh) {
		PlayerManager p = new PlayerManager();

		p.transform.SetParent(gh.transform);
		p.InitPlayers(players);
		return p;
	}

	public void NextTurn () {
		turnQueue.Enqueue(currentPlayer);
		currentPlayer = turnQueue.Dequeue();
		currentPlayer.StartTurn();

		GameHandler.Instance.DisplayTurnMessage(
			string.Format("{0}'s turn", currentPlayer.ToString())
		);
	}

	public void InitPlayers (List<Player> players) {
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
}
