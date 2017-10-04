using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiled2Unity;

public class CameraBehavior : MonoBehaviour {
	public TiledMap myMap;

	private int left;
	private int right;
	private int top;
	private int bottom;
	private int width;
	private int height;

	private RectTransform myRect;

	private int xDiff;

	// Use this for initialization
	void Start () {
		InitBoundary();
	}
	
	// Update is called once per frame
	void Update () {
		if (left < myMap.transform.position.x) {
			xDiff = (int)(myMap.transform.position.x - left);
			transform.Translate(new Vector3(xDiff, 0, 0));
		}
	}

	private void InitBoundary() {
		height = (int)Camera.main.orthographicSize;
		width = (int)(Camera.main.aspect * height);
		left = (int)(transform.position.x - width);
		right = left + (width * 2);
		top = (int)(transform.position.y - height);
		bottom = top + (height * 2);
	}
}
