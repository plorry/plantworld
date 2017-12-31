using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiled2Unity;
using System;

public class CameraBehavior : MonoBehaviour {
	public TiledMap myMap;

	private int mapTop;
	private int mapBottom;
	private int mapLeft;
	private int mapRight;

	private int left;
	private int right;
	private int top;
	private int bottom;
	private int width;
	private int height;

	private RectTransform myRect;

	private int xDiff;
	private int yDiff;

	// Use this for initialization
	void Start () {
		InitBoundary();		
	}
	
	// Update is called once per frame
	void Update () {
		CorrectBoundary();
	}

	private void CorrectBoundary() {
		xDiff = 0;
		yDiff = 0;
		CorrectLeft();
		CorrectRight();
		CorrectTop();
		CorrectBottom();
	}

    private void CorrectBottom()
    {
        if (bottom < mapBottom) {
			yDiff = mapBottom - bottom;
			transform.Translate(new Vector3(0, yDiff, 0));
			CalculateBoundary();
		}
    }

    private void CorrectTop()
    {
        if (top > mapTop) {
			yDiff = mapTop - top;
			transform.Translate(new Vector3(0, yDiff, 0));
			CalculateBoundary();
		}
    }

    private void CorrectRight()
    {
        if (right > mapRight) {
			xDiff = mapRight - right;
			transform.Translate(new Vector3(xDiff, 0, 0));
			CalculateBoundary();
		}
    }

    private void CorrectLeft(){
		if (left < mapLeft) {
			xDiff = mapLeft - left;
			transform.Translate(new Vector3(xDiff, 0, 0));
			CalculateBoundary();
		}
	}

	private void InitBoundary() {
		height = (int)Camera.main.orthographicSize;
		width = (int)(Camera.main.aspect * height);

		mapTop = (int)myMap.transform.position.y;
		mapBottom = mapTop - (int)myMap.GetMapHeightInPixelsScaled();
		mapLeft = (int)myMap.transform.position.x;
		mapRight = mapLeft + (int)myMap.GetMapWidthInPixelsScaled();

		CalculateBoundary();
	}

	private void CalculateBoundary() {
		left = (int)(transform.position.x - width);
		right = left + (width * 2);
		top = (int)(transform.position.y + height);
		bottom = top - (height * 2);
	}
}
