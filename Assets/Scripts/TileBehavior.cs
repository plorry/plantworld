using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour {
    private SmartGridBehavior myGrid;
    private Vector2 myCoords;
    private List<GameObject> myContents;
    

	// Use this for initialization
	void Start () {
		myContents = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Setup initial properties
    public void Init (SmartGridBehavior smartGrid, Vector2 coords) {
        myGrid = smartGrid;
        myCoords = coords;
       // GUI.Box
    }

    // Return this tile's contents
    public List<GameObject> GetContents () {
        return myContents;
    }

    // Add a GameObject to this tile
    public void AddContent (GameObject obj) {
        myContents.Add(obj);
    }
}
