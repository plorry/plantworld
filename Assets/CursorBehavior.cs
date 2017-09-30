using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour {
    private Vector2 pos;
    private Vector3 size;
    public GameObject map;

	// Use this for initialization
	void Start () {
        pos = transform.position;
        size = GetComponent<SpriteRenderer>().bounds.size;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = pos;

        pos.x = Mathf.Floor(Input.mousePosition.x / 16) - map.transform.position.x;
        pos.y = Mathf.Floor(Input.mousePosition.y / 16) - map.transform.position.y + (size.y / 2);

        print(pos);
    }
}
