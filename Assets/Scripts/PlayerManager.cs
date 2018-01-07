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

	public void NextTurn () {
		turnQueue.Enqueue(currentPlayer);
		currentPlayer = turnQueue.Dequeue();
		currentPlayer.StartTurn();
	}
}
