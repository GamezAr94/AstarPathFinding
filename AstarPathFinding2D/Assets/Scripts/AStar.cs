using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { START, GOAL, WATER, GRASS, PATH}

public class AStar : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Tile[] tiles;

    [SerializeField]
    private Tilemap tileMap;

    [SerializeField]
    private LayerMask layerMask;

    private TileType tileType;

    private Vector3Int startPos, goalPos;

    private Node current;

    private HashSet<Node> openList;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
            if(hit.collider != null)
            {
                Vector3 mouseWorlPos = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPos = tileMap.WorldToCell(mouseWorlPos);
                ChangeTile(clickPos);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Run Algorithm
            Algorithm();
        }
    }

    private void Initialize()
    {
        current = GetNode(startPos);

        openList = new HashSet<Node>();

        //Adding start to the open list
        openList.Add(current);
    }

    private void Algorithm()
    {
        if (current == null)
        {
            Initialize();
        }

        AStarDebbuger.myInstance.CreateTiles(openList, startPos,goalPos);
    }

    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            Node node = new Node(position);
            allNodes.Add(position,node);
            return node;
        }
    }

    public void ChangeTileType(TileButton button)
    {
        tileType = button.MyTileType;
    }

    private void ChangeTile(Vector3Int clickPos)
    {
        if(tileType == TileType.START)
        {
            startPos = clickPos;
        }else if(tileType==TileType.GOAL){
            goalPos = clickPos;
        }
        tileMap.SetTile(clickPos, tiles[(int)tileType]);
    }
}
