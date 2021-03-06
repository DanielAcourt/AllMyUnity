﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager> {

    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private Transform map;

    public Dictionary<Point, TileScript> Tiles { get; set; }    //dictionary containing all tiles in the game

    private Point portalSpawn, coinSpawn;

    [SerializeField]
    private GameObject portalPrefab, coinPrefab;

    public Portal SpawnPortal { get; set; }

    private Point mapSize;

    private Stack<Node> path;  //given to monsters 

    public Stack<Node> Path //read only
    {
        get
        {
            if (path == null)
            {
                GeneratePath();
            }

            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    public float TileSize
    {
        get {return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

	// Use this for initialization
	void Start () {
        CreateLevel();
        SpawnPortals();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        mapSize = new global::Point(mapData[0].ToCharArray().Length, mapData.Length);

        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        Vector3 maxTile = Vector3.zero;

        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString() ,x,y,worldStart);
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
    }

    private void PlaceTile(string tileType,int x, int y, Vector3 worldStart)
    {
        int test, tileIndex = 0;

        if (int.TryParse(tileType, out test))
        {
            tileIndex = int.Parse(tileType);
        }

        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        newTile.Setup(new global::Point(x, y), new Vector3(worldStart.x + TileSize * x, worldStart.y - TileSize * y, 0), map);

        
      //  Tiles.Add(new Point(x, y), newTile);

    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    private void SpawnPortals()
    {
        portalSpawn = new Point(0, 0);
        GameObject tmp = (GameObject)Instantiate(portalPrefab, Tiles[portalSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);   //spawn portal
        SpawnPortal = tmp.GetComponent<Portal>();   //get the script off it
        SpawnPortal.name = "SpawnPortal";

        coinSpawn = new Point(11, 6);

        Instantiate(coinPrefab, Tiles[coinSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }

    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }

    public void GeneratePath()  //from start to finish
    {
        path = AStar.GetPath(portalSpawn, coinSpawn);
    }
}