using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIPlayer : Player {
    static string[] dirs = {"up", "down", "left", "right"};
    static System.Random rnd = new System.Random();
    // The player will target an enemy, rather than the unit itself.
    // Helps to separate the logic of AI Player from units
    private Unit currentUnit;
    private Unit target;
    private List<TileBehavior> availableTiles;
    private List<string> desiredDirs;
    private bool canGo = true;
    private bool choseMove = false;

    void Update () {
        if (currentUnit == null && GetAwakeUnits().Count > 0) {
            currentUnit = GetAwakeUnits().First();
        }

        if (currentUnit != null && !currentUnit.IsMoving() && choseMove == true) {
            if (currentUnit.IsOutOfMoves()) {
               DeselectUnit();
            }
            canGo = true;
            choseMove = false;
        }

        if (IsMyTurn() && canGo == true && currentUnit != null) {
            RunAI();
        }
    }

    override public void StartTurn () {
        base.StartTurn();
    }

    override public void EndTurn() {
        base.EndTurn();
    }

    private void RunAI () {
        // my turn; annihilate! kill!
        if (!target) target = currentUnit.ClosestEnemy();

        if (currentUnit.GetCurrentTile().GetNeighbours().Contains(target.GetCurrentTile())) {
            // the target is next to me! I will attack.
            currentUnit.Attack(target);
            DeselectUnit();
        } else {
            MoveUnit(currentUnit, target);
            canGo = false;
            choseMove = true;
        }
    }

    private void DeselectUnit () {
        currentUnit.Exhaust();
        currentUnit = null;
        target = null;
        CheckGridState();
    }

    private void MoveUnit (Unit unit, Unit target) {
        SetDesiredDirs(unit, target);
        availableTiles = SmartGridBehavior.Instance.GetAvailableTiles(
            unit.GetCurrentTile(),
            1,
            unit
        );
        string chosenDir = "";
        foreach (string dir in desiredDirs) {
            TileBehavior neighb = unit.GetCurrentTile().GetNeighbour(dir);
            if (availableTiles.Contains(neighb)) {
                chosenDir = dir;
                break;
            } 
        }
        if (chosenDir == "") {
            // nothing desired available - just pick one
            foreach (string dir in dirs) {
                TileBehavior neighb = unit.GetCurrentTile().GetNeighbour(dir);
                if (availableTiles.Contains(neighb)) {
                    chosenDir = dir;
                    break;
                }
            }
        }
        //nothing available - just bail
        if (chosenDir == "") return;

        unit.Move(chosenDir);
    }

    private void SetDesiredDirs (Unit unit, Unit target) {
        desiredDirs = new List<string>();

        if (unit.GetCurrentTile().GetCoords().x > target.GetCurrentTile().GetCoords().x)
            desiredDirs.Add("left");
        if (unit.GetCurrentTile().GetCoords().x < target.GetCurrentTile().GetCoords().x)
            desiredDirs.Add("right");
        if (unit.GetCurrentTile().GetCoords().y > target.GetCurrentTile().GetCoords().y)
            desiredDirs.Add("up");
        if (unit.GetCurrentTile().GetCoords().y < target.GetCurrentTile().GetCoords().y)
            desiredDirs.Add("down");

        desiredDirs = desiredDirs.OrderBy(x => rnd.Next()).ToList();
    }

    private string PickRandomDirection () {
        return (desiredDirs.Count > 0) ?
            desiredDirs[rnd.Next(desiredDirs.Count)] :
            dirs[rnd.Next(dirs.Length)];
    }
}