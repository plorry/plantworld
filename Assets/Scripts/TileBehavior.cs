using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileBehavior : MonoBehaviour {
    private SmartGridBehavior myGrid;
    private Vector2 myCoords;
    private List<Unit> myContents;
    private List<string> myProperties;

    public GameObject guiPrefab;
    public Texture myTexture;

    private float alpha = 0;
    private bool alphaUp = true;
    private bool highlight = false;
    

    void Awake() {
        myContents = new List<Unit>();
        myProperties = new List<string>();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (highlight) UpdateHighlight();
	}

    private void UpdateHighlight () {
        if (alpha >= 0.25f) alphaUp = false;
        if (alpha <= 0.1f) alphaUp = true;
        alpha += (alphaUp) ? 0.003f : -0.003f;
        Highlight(new Color(1f, 1f, 1f, alpha));
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
    public List<Unit> GetContents () {
        return myContents;
    }

    public List<string> GetContentNames () {
        return myContents.Select(x => x.ToString()).ToList();
    }

    // Add a GameObject to this tile
    public void AddContent (Unit obj) {
        myContents.Add(obj);
    }

    public void RemoveContent (Unit obj) {
        myContents.Remove(obj);
    }

    public bool Contains (Unit obj) {
        return myContents.Contains(obj);
    }

    public bool IsOccupied () {
        return myContents.Count > 0;
    }

    public bool ContainsSelectable (Player player) {
        return myContents.Any(obj => (obj.BelongsTo(player) && obj.exhausted == false));
    }

    public Unit GetSelectable (Player player) {
        return myContents.First(obj => obj.belongsTo == player && obj.exhausted == false);
    }

    public Unit GetUnit () {
        return myContents.First();
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

    public TileBehavior GetNeighbour(string direction) {
        TileBehavior tile = default(TileBehavior);
        switch (direction) {
            case "up":
                tile = myGrid.GetTileAt(myCoords.x, myCoords.y - 1);
                break;
            case "down":
                tile = myGrid.GetTileAt(myCoords.x, myCoords.y + 1);
                break;
            case "left":
                tile = myGrid.GetTileAt(myCoords.x - 1, myCoords.y);
                break;
            case "right":
                tile = myGrid.GetTileAt(myCoords.x + 1, myCoords.y);
                break;
        }
        return tile;
    }

    public Vector2 GetCoords () {
        return myCoords;
    }

    override public string ToString() {
        return string.Format("Tile ({0}, {1})", myCoords.x, myCoords.y);
    }

    public void Highlight() {
        highlight = true;
    }

    public void Highlight (Color color) {
        this.GetComponent<SpriteRenderer>().color = color;
    }

    public void Unhighlight () {
        highlight = false;
        alpha = 0;
        Highlight(new Color(1f, 1f, 1f, 0f));
    }

    public bool IsTraversable (Unit u = null) {
        List<bool> conditionList = new List<bool>();
        conditionList.Add(!myProperties.Contains("water"));
        conditionList.Add(!myProperties.Contains("rock"));

        if (u != null && IsOccupied()) conditionList.Add(!u.IsEnemy(GetUnit()));

        return conditionList.All(x => x == true);
    }
}
