using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIPlayer : Player {
    static string[] dirs = {"up", "down", "left", "right"};
    static System.Random rnd = new System.Random();

    void Update () {
        RunAI();
    }

    override public void StartTurn () {
        base.StartTurn();
    }

    override public void EndTurn() {
        base.EndTurn();
    }

    private void RunAI () {
        // not my turn, sit pretty
        if (!IsMyTurn()) return;
        // my turn; annihilate! kill!
        myUnits.ForEach(x => {
            x.Move(dirs[rnd.Next(dirs.Length)]);
            x.Exhaust();
            CheckGridState();
        });
    }
}