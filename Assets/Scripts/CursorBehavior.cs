﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour {
    public SmartGridBehavior myGrid;
    public Camera myCamera;

    private Vector3 mouseCoords;
    private TileBehavior currentTile;
    private TileBehavior destinationTile;
    private bool moving = false;
    private Unit selected;
    private List<TileBehavior> availableTiles;

    private Vector3 lastMousePosition;

    private int actionDelay;
    private int numRepeats;

    private Player currentPlayer;

    public static CursorBehavior MkCursor (SmartGridBehavior grid, Camera camera) {
        CursorBehavior cursor = new CursorBehavior();

        cursor.myGrid = grid;
        cursor.myCamera = camera;

        cursor.currentTile = cursor.destinationTile = grid.GetTileAt(0, 0);

        return cursor;
    }

	// Use this for initialization
	void Start () {
        destinationTile = currentTile;
	}

    void Awake () {
        availableTiles = new List<TileBehavior>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.mousePosition != lastMousePosition) {
            if (GetCurrentTile() != null && GetCurrentTile() != currentTile) {
                currentTile = GetCurrentTile();
                destinationTile = currentTile;
                transform.position = new Vector2(currentTile.transform.position.x, currentTile.transform.position.y);
            }
            lastMousePosition = Input.mousePosition;
        }
        if (selected) {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        } else {
            this.gameObject.GetComponent<Renderer>().enabled = true;
        }
        UpdatePosition();
    }

    private void UpdatePosition () {
        if (destinationTile != currentTile) {
            transform.position = Vector2.MoveTowards(transform.position, destinationTile.transform.position, 0.1f);
        } else {
            transform.position = currentTile.transform.position;
        }

        if (AtDestination() && moving == true) {
            currentTile = destinationTile;
            moving = false;
        }
    }

    private bool AtDestination () {
        return (transform.position - destinationTile.transform.position).sqrMagnitude < 1.01;
    }

    private TileBehavior GetCurrentTile() {
        mouseCoords = myCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mouseCoords.x < 0) mouseCoords.x = 0;
        if (mouseCoords.y > 0) mouseCoords.y = 0;
        return myGrid.GetTileAt(Mathf.FloorToInt(mouseCoords.x), Mathf.FloorToInt(-mouseCoords.y));
    }

    public TileBehavior GetTile() {
        return currentTile;
    }

    public Unit GetSelected() {
        return selected;
    }

    public void Select () {
        if (!selected && GetTile().ContainsSelectable(currentPlayer)) {
            Deselect();
            SetSelected(GetTile().GetSelectable(currentPlayer));
            availableTiles = myGrid.GetAvailableTiles(GetTile(), selected.GetSpeed());
        } else if (selected) {
            selected.LockIn();
            Deselect();
        }

        foreach(TileBehavior tile in availableTiles) {
            tile.Highlight();
        }
    }

    public void Deselect () {
        if (selected) {
            selected.Deselect();
        
            currentTile = destinationTile = selected.GetCurrentTile();
            transform.position = currentTile.transform.position;
        }

        selected = default(Unit);
        foreach(TileBehavior tile in availableTiles) {
            tile.Unhighlight();
        }
        availableTiles = new List<TileBehavior>();
    }

    private void SetSelected (Unit item) {
        selected = item;
        item.MyTile();
    }

    public void Move (string direction) {
        if (selected) {
            if (availableTiles.Contains(selected.GetCurrentTile().GetNeighbour(direction))) {
                selected.Move(direction);
            }
        } else {
            if (!moving) {
                moving = true;
                destinationTile = currentTile.GetNeighbour(direction);
            }
        }
    }

    public void Left () {
        Move("left");
    }

    public void Right () {
        Move("right");
    }

    public void Up () {
        Move("up");
    }

    public void Down () {
        Move("down");
    }
}
