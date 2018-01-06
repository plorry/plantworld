using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/*
    The SmartGrid manages the Grid state.
 */

public class SmartGridBehavior : MonoBehaviour {
    private int tileSize;
    private int xSize;
    private int ySize;

    public CursorBehavior myCursor;
    public TileBehavior tilePrefab;
    public TileItemBehavior testItem;
    private Tiled2Unity.ObjectLayer objectLayer;
    public Tiled2Unity.TiledMap tiledMap;
    public GameObject debugPanel;

    private TileBehavior[][] myTiles;    

	void Start () {
		Init();
        AddItemToTileAt(testItem, 5, 5);
	}

    // Basically copying over a few settings from Tiled2Unity object
    private void SetTileSettings () {
        tileSize = tiledMap.TileWidth;
        xSize = tiledMap.NumTilesWide;
        ySize = tiledMap.NumTilesHigh;
        objectLayer = tiledMap.transform.Find("Reactive").GetComponent<Tiled2Unity.ObjectLayer>();
    }

    // Make those tiles
    private void InstantiateTiles () {
        myTiles = new TileBehavior[ySize][];
        for (int i=0; i < ySize; i++) {
            myTiles[i] = new TileBehavior[xSize];
            for (int j=0; j < xSize; j++) {
                // Probably some values here we should extract into variables
                TileBehavior newTile = Instantiate(tilePrefab, new Vector3((tileSize * j / 16) + 0.5f, -(tileSize * i / 16) - 0.5f, -1), new Quaternion());
                myTiles[i][j] = newTile;
                newTile.Init(this, new Vector2(j, i));
                newTile.transform.SetParent(transform);
            }
        }
    }

    // A little more rigorous tile-by-tile copying of properties
    private void SetTileProperties () {
        foreach(Transform r in objectLayer.transform) {
            Tiled2Unity.RectangleObject rect = r.GetComponent<Tiled2Unity.RectangleObject>();
            TileBehavior t = GetTileFromTMXRectangle(rect);
            t.AddProperty(rect.TmxType);
        }
    }

    private TileBehavior GetTileFromTMXRectangle (Tiled2Unity.RectangleObject rect) {
        int x = (int)(rect.TmxPosition.x / rect.TmxSize.x);
        int y = (int)(rect.TmxPosition.y / rect.TmxSize.y);
        return GetTileAt(x, y);
    }

    // Initialize the grid from X-Y grid size and tilesize
    void Init () {
        SetTileSettings();
        InstantiateTiles();
        SetTileProperties();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateDebug();
	}

    private void UpdateDebug () {
        TileBehavior t = myCursor.GetTile();

        string selected = (myCursor.GetSelected()) ? myCursor.GetSelected().ToString() : "";

        debugPanel.transform.Find("Xdisplay").GetComponent<Text>().text = t.GetCoords().x.ToString();
        debugPanel.transform.Find("Ydisplay").GetComponent<Text>().text = t.GetCoords().y.ToString();
        debugPanel.transform.Find("PropertiesDisplay").GetComponent<Text>().text = string.Join(" ", t.GetProperties().ToArray());
        debugPanel.transform.Find("ContentsDisplay").GetComponent<Text>().text = string.Join(" ", t.GetContentNames().ToArray());
        // debugPanel.transform.Find("SelectedDisplay").GetComponent<Text>().text = selected;
    }

    public TileBehavior GetTileAt (int x, int y) {
        if (x < 0 || y < 0 || x > xSize || y > ySize) {
            return null;
        }
        return myTiles[y][x];
    }

    public TileBehavior GetTileAt (float x, float y) {
        return GetTileAt((int)x, (int)y);
    }

    public TileBehavior GetTileAt (Vector2 coords) {
        return GetTileAt(coords.x, coords.y);
    }

    public void AddItemToTileAt(TileItemBehavior item, int x, int y) {
        item.SetHomeTile(GetTileAt(x, y));
        item.SetCurrentTile(GetTileAt(x, y));
        // GetTileAt(x, y).AddContent(item);
    }

    public List<TileBehavior> GetAvailableTiles (TileBehavior tile, int distance) {
        List<TileBehavior> availableTiles = new List<TileBehavior>();

        if (distance <= 0) return availableTiles;

        List<TileBehavior> lastLayer = new List<TileBehavior>();
        List<TileBehavior> thisLayer;
        lastLayer.Add(tile);

        while (distance > 0) {
            thisLayer = new List<TileBehavior>();
            foreach(TileBehavior thisTile in lastLayer) {
                foreach(TileBehavior neighbour in thisTile.GetNeighbours()) {
                    if (neighbour.IsTraversable() && !thisLayer.Contains(neighbour) && !availableTiles.Contains(neighbour)) {
                        thisLayer.Add(neighbour);
                    }
                }
            }

            lastLayer = thisLayer;
            availableTiles = availableTiles.Concat(thisLayer).ToList();

            distance--;
        }
        return availableTiles;
    }
}
