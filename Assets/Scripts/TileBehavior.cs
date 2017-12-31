using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileBehavior : MonoBehaviour {
    private SmartGridBehavior myGrid;
    private Vector2 myCoords;
    private List<TileItemBehavior> myContents;
    private List<string> myProperties;

    public GameObject guiPrefab;
    public Texture myTexture;
    

    void Awake() {
        myContents = new List<TileItemBehavior>();
        myProperties = new List<string>();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Setup initial properties
    public void Init (SmartGridBehavior smartGrid, Vector2 coords) {
        myGrid = smartGrid;
        myCoords = coords;
    }

    public void AddProperty (string prop) {
        myProperties.Add(prop);
    }

    public List<string> GetProperties () {
        return myProperties;
    }

    // Return this tile's contents
    public List<TileItemBehavior> GetContents () {
        return myContents;
    }

    public List<string> GetContentNames () {
        return myContents.Select(x => x.ToString()).ToList();
    }

    // Add a GameObject to this tile
    public void AddContent (TileItemBehavior obj) {
        myContents.Add(obj);
    }

    public void RemoveContent (TileItemBehavior obj) {
        myContents.Remove(obj);
    }

    public bool Contains (TileItemBehavior obj) {
        return myContents.Contains(obj);
    }

    public List<TileBehavior> GetNeighbours () {
        List<TileBehavior> neighbours = new List<TileBehavior>();
        neighbours.Add(myGrid.GetTileAt((int)myCoords.x, (int)myCoords.y - 1));
        neighbours.Add(myGrid.GetTileAt((int)myCoords.x, (int)myCoords.y + 1));
        neighbours.Add(myGrid.GetTileAt((int)myCoords.x - 1, (int)myCoords.y));
        neighbours.Add(myGrid.GetTileAt((int)myCoords.x + 1, (int)myCoords.y));
        // Some values might be null. Let's remove those
        neighbours.RemoveAll(item => item == null);
        return neighbours;
    }

    public Vector2 GetCoords () {
        return myCoords;
    }
}
