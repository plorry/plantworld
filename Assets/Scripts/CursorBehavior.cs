using System.Collections;
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

    private Animator animator;
    private GameObject interactCursor;
    private bool interactVisible;

    public static CursorBehavior Instance { get; private set; }

    void Awake () {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        availableTiles = new List<TileBehavior>();
        myCamera = Camera.main;
        destinationTile = currentTile = myGrid.GetTileAt(0, 0);
        animator = GetComponent<Animator>();
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
        if (selected && animator.GetInteger("ActionIcon") == 0) {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        } else {
            this.gameObject.GetComponent<Renderer>().enabled = true;
        }

        //if (interactVisible == true) {
        //    interactCursor.GetComponent<Renderer>().enabled = true;
        //} else {
        //    interactCursor.GetComponent<Renderer>().enabled = false;
        //}
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
        if (animator.GetInteger("ActionIcon") == 1) {
            // Attack
            selected.Attack(GetTile().GetUnit());
            selected.LockIn();
            Deselect();
        }
        if (animator.GetInteger("ActionIcon") == 2) {
            // rescue
            selected.Rescue(GetTile().GetUnit());
            selected.LockIn();
            Deselect();
        }

        if (!selected && GetTile().ContainsSelectable(PlayerManager.Instance.GetCurrentPlayer())) {
            Deselect();
            SetSelected(GetTile().GetSelectable(PlayerManager.Instance.GetCurrentPlayer()));
            availableTiles = myGrid.GetAvailableTiles(GetTile(), selected.GetSpeed(), selected);
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
            animator.SetInteger("ActionIcon", 0);
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
            TileBehavior desiredTile = selected.GetCurrentTile().GetNeighbour(direction);
            if (availableTiles.Contains(desiredTile)) {
                selected.Move(direction);
                animator.SetInteger("ActionIcon", 0);
                this.gameObject.GetComponent<Renderer>().enabled = false;
                return;
            } else if (selected.IsAlly(desiredTile.GetUnit()) && desiredTile.GetUnit().isCaptured == false && availableTiles.Contains(desiredTile.GetNeighbour(direction))) {
                // Ally on desired tile - we can jump over if the next tile is also available
                selected.MoveTo(desiredTile.GetNeighbour(direction));
                return;
            }
            
            if (selected.IsEnemy(desiredTile.GetUnit())) {
                // Enemy on tile - render InteractCursor
                currentTile = destinationTile = desiredTile;
                animator.SetInteger("ActionIcon", 1);
            } else if (selected.IsAlly(desiredTile.GetUnit()) && desiredTile.GetUnit().isCaptured == true) {
                currentTile = destinationTile = desiredTile;
                animator.SetInteger("ActionIcon", 2);
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
