using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	private List<Player> players;
	private Queue<Player> turnQueue;
	private Player currentPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!currentPlayer.IsMyTurn()) {
			NextTurn();
		}
	}

	public static PlayerManager MakePlayerManager (List<Player> players) {
		PlayerManager p = new PlayerManager();
		p.InitPlayers(players);
		return p;
	}

	public void NextTurn () {
		turnQueue.Enqueue(currentPlayer);
		currentPlayer = turnQueue.Dequeue();
		currentPlayer.StartTurn();
	}

	private void InitPlayers (List<Player> ps) {
		turnQueue = new Queue<Player>();
		players = ps;

		foreach (Player p in ps) {
			turnQueue.Enqueue(p);
		}
		currentPlayer = turnQueue.Dequeue();
	}
}
