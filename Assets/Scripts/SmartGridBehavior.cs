using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartGridBehavior : MonoBehaviour {
    public int tileSize;
    public int xSize;
    public int ySize;
    public TileBehavior tilePrefab;
    private TileBehavior[][] myTiles;

	// Use this for initialization
	void Start () {
		Init();
	}

    // Initialize the grid from X-Y grid size and tilesize
    void Init () {
        myTiles = new TileBehavior[xSize][];
        for (int i=0; i < xSize; i++) {
            myTiles[i] = new TileBehavior[ySize];
            for (int j=0; j < ySize; j++) {
                TileBehavior newTile = Instantiate(tilePrefab, new Vector3(tileSize * j / 16, -tileSize * i / 16, 0), new Quaternion());
                myTiles[i][j] = newTile;
                newTile.Init(this, new Vector2(j, i));
                newTile.transform.SetParent(transform);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public TileBehavior GetTileAt (int x, int y) {
        return myTiles[y][x];
    }
}
