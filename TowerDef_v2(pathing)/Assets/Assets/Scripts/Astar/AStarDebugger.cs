﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour {
    //debugging script DELETE LATER

    [SerializeField]
    private TileScript start, goal;
    [SerializeField]
    private Sprite blankTile;
    [SerializeField]
    private Sprite stoneTile;
    [SerializeField]
    private GameObject arrowPrefab;


    [SerializeField]
    private GameObject debugTilePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ClickTile();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AStar.GetPath(start.GridPosition);
        }
	}

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);    // makes a raycast from mouse position to the start

            if(hit.collider != null )
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();   //equal to tile hit by mouse
                if (tmp != null)    //if actually hit a tile not a tower etc
                {
                    if (start == null)
                    {
                        start = tmp;    //make selected tile start point
                    //    start.SpriteRenderer.sprite = stoneTile;
                        start.Debugging = true;
                      //  start.SpriteRenderer.color = new Color32(255, 132, 0, 255);
                    }
                    else if (goal == null)
                    {
                        goal = tmp;     //second click = goal
                      //  goal.SpriteRenderer.sprite = stoneTile;
                        goal.Debugging = true;
                      //  goal.SpriteRenderer.color = new Color32(255, 0, 0, 255);
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> openList)
    {
        foreach (Node node in openList)
        {
            if(node.TileRef != start)
            {
            //   node.TileRef.SpriteRenderer.color = Color.cyan;
            //    node.TileRef.SpriteRenderer.sprite = blankTile;
            }
            PointToParent(node, node.TileRef.WorldPosition);
        }
    }

    private void PointToParent(Node node, Vector2 position)     //point the arrow to the parent
    {
        if (node.Parent != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);

            if (node.GridPosition.X < node.Parent.GridPosition.X && node.GridPosition.Y == node.Parent.GridPosition.Y)  //point right
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (node.GridPosition.X < node.Parent.GridPosition.X && node.GridPosition.Y > node.Parent.GridPosition.Y)  //point top right
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            }
            else if (node.GridPosition.X == node.Parent.GridPosition.X && node.GridPosition.Y > node.Parent.GridPosition.Y)  //point top up
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (node.GridPosition.X > node.Parent.GridPosition.X && node.GridPosition.Y > node.Parent.GridPosition.Y)  //point top left
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            }
            else if (node.GridPosition.X > node.Parent.GridPosition.X && node.GridPosition.Y == node.Parent.GridPosition.Y)  //point right
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (node.GridPosition.X < node.Parent.GridPosition.X && node.GridPosition.Y < node.Parent.GridPosition.Y)  //point bottom right
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -45);
            }
            else if (node.GridPosition.X == node.Parent.GridPosition.X && node.GridPosition.Y < node.Parent.GridPosition.Y)  //point down
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -90);
            }
            else if (node.GridPosition.X > node.Parent.GridPosition.X && node.GridPosition.Y < node.Parent.GridPosition.Y)  //point bottom left
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -135);
            }
        }

    }

    private void CreateDebugTIle(Vector3 worldPos, Color32 color)
    {
        GameObject debugTile = Instantiate(debugTilePrefab, worldPos,Quaternion.identity);

        debugTile.GetComponent<SpriteRenderer>().color = color;
    }
}