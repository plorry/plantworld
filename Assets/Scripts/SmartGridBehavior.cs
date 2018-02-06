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
    public Unit testItem;
    private Tiled2Unity.ObjectLayer objectLayer;
    private Tiled2Unity.ObjectLayer unitLayer;
    public Tiled2Unity.TiledMap tiledMap;
    public GameObject debugPanel;

    private TileBehavior[][] myTiles;
    private List<Unit> myUnits;

    public static SmartGridBehavior Instance { get; private set; }

    [System.Serializable]
	public struct UnitPrefab {
		public string name;
		public Unit prefab;
	}

    public List<UnitPrefab> unitPrefabs;

    void Awake () {
        Instance = this;
    }

	void Start () {
		Init();
	}

    // Basically copying over a few settings from Tiled2Unity object
    private void SetTileSettings () {
        tileSize = tiledMap.TileWidth;
        xSize = tiledMap.NumTilesWide;
        ySize = tiledMap.NumTilesHigh;
        objectLayer = tiledMap.transform.Find("Reactive").GetComponent<Tiled2Unity.ObjectLayer>();
        unitLayer = tiledMap.transform.Find("UnitData").GetComponent<Tiled2Unity.ObjectLayer>();
        // PlayerUnits layer is only for visual aid // to be destroyed at start of game
        Destroy(tiledMap.transform.Find("PlayerUnits").gameObject);
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

    public void InstantiateUnits (Player p) {
        myUnits = new List<Unit>();

        foreach(Transform r in unitLayer.transform) {
            Tiled2Unity.RectangleObject rect = r.GetComponent<Tiled2Unity.RectangleObject>();

            if (p.GetName() != rect.TmxType) continue;

            TileBehavior t = GetTileFromTMXRectangle(rect);
            Unit unit = Instantiate(
                unitPrefabs.Find(x => x.name == rect.TmxName).prefab
            );
            unit.myName = "Morton";
            p.AddUnit(unit);
            AddItemToTile(unit, t);
        }
    }

    // Initialize the grid from X-Y grid size and tilesize
    void Init () {
        SetTileSettings();
        InstantiateTiles();
        SetTileProperties();
        // InstantiateUnits(unitLayer);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateDebug();
	}

    private void UpdateDebug () {
        TileBehavior t = myCursor.GetTile();

        if (t == null) return;

        string selected = (myCursor.GetSelected()) ? myCursor.GetSelected().ToString() : "";

        debugPanel.transform.Find("Xdisplay").GetComponent<Text>().text = t.GetCoords().x.ToString();
        debugPanel.transform.Find("Ydisplay").GetComponent<Text>().text = t.GetCoords().y.ToString();
        debugPanel.transform.Find("PropertiesDisplay").GetComponent<Text>().text = string.Join(" ", t.GetProperties().ToArray());
        debugPanel.transform.Find("ContentsDisplay").GetComponent<Text>().text = string.Join(" ", t.GetContentNames().ToArray());
        // debugPanel.transform.Find("SelectedDisplay").GetComponent<Text>().text = selected;
    }

    public TileBehavior GetTileAt (int x, int y) {
        if (x < 0 || y < 0 || x >= xSize || y >= ySize) {
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

    public void AddItemToTileAt(Unit item, int x, int y) {
        AddItemToTile(item, GetTileAt(x, y));
    }

    public void AddItemToTile(Unit item, TileBehavior tile) {
        item.SetHomeTile(tile);
        item.SetCurrentTile(tile);
    }

    public List<TileBehavior> GetAvailableTiles (TileBehavior tile, int distance, Unit u = null) {
        List<TileBehavior> availableTiles = new List<TileBehavior>();

        if (distance <= 0) return availableTiles;

        List<TileBehavior> lastLayer = new List<TileBehavior>();
        List<TileBehavior> thisLayer;
        lastLayer.Add(tile);

        while (distance > 0) {
            thisLayer = new List<TileBehavior>();
            foreach(TileBehavior thisTile in lastLayer) {
                foreach(TileBehavior neighbour in thisTile.GetNeighbours()) {
                    if (neighbour.IsTraversable(u) && !thisLayer.Contains(neighbour) && !availableTiles.Contains(neighbour)) {
                        thisLayer.Add(neighbour);
                    }
                }
            }

            lastLayer = thisLayer;
            availableTiles = availableTiles.Concat(thisLayer).ToList();

            distance--;
        }
        // remove ally tiles from available list
        if (u != null) availableTiles = availableTiles.Except(GetUnitListTiles(u.GetAllies())).ToList();
        // just a quickfix, since we'll have removed the home tile from the above list
        availableTiles.Add(tile);

        return availableTiles;
    }

    private List<TileBehavior> GetUnitListTiles (List<Unit> unitList) {
        return unitList.Select(x => x.GetCurrentTile()).ToList();
    }

}
