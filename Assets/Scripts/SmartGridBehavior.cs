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
        myTiles = new TileBehavior[ySize][];
        print(myTiles);
        for (int i=0; i < ySize; i++) {
            myTiles[i] = new TileBehavior[xSize];
            print(myTiles[i]);
            for (int j=0; j < xSize; j++) {
                TileBehavior newTile = Instantiate(tilePrefab, new Vector3(tileSize * j / 16, -tileSize * i / 16, 0), new Quaternion());
                print(newTile);
                myTiles[i][j] = newTile;
                newTile.Init(this, new Vector2(j, i));
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
