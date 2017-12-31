using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour {
    public SmartGridBehavior myGrid;
    public Camera myCamera;

    private Vector3 mouseCoords;
    private TileBehavior currentTile;
    private TileItemBehavior selected;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (GetCurrentTile() != null && GetCurrentTile() != currentTile) {
            currentTile = GetCurrentTile();
            transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y, currentTile.transform.position.z);
        }
    }

    private TileBehavior GetCurrentTile() {
        mouseCoords = myCamera.ScreenToWorldPoint(Input.mousePosition);
        return myGrid.GetTileAt(Mathf.FloorToInt(mouseCoords.x), Mathf.FloorToInt(-mouseCoords.y));
    }

    public TileBehavior GetTile() {
        return currentTile;
    }

    public TileItemBehavior GetSelected() {
        return selected;
    }

    public void Select (TileItemBehavior item) {
        
    }

    private void SetSelected (TileItemBehavior item) {
        selected = item;
        item.GetTile();
    }
}
