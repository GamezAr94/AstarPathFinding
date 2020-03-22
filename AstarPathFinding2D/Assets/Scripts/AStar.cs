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

    private HashSet<Node> closedList;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    //Ignoring Water
    private List<Vector3Int> waterTiles = new List<Vector3Int>();

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

        closedList = new HashSet<Node>();

        //Adding start to the open list
        openList.Add(current);
    }

    private void Algorithm()
    {
        if (current == null)
        {
            Initialize();
        }

        List<Node> neighbors = findNeighbors(current.Position);

        ExamineNeighbors(neighbors,current);

        UpdateCurrentTile(ref current);

        AStarDebbuger.myInstance.CreateTiles(openList,closedList, allNodes, startPos,goalPos);

    }

    private List<Node> findNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                if(y != 0 || x != 0)
                {
                    if (neighborPos != startPos && !waterTiles.Contains(neighborPos) && tileMap.GetTile(neighborPos))
                    {
                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                }
            }
        }
        return neighbors;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for(int i = 0; i < neighbors.Count; i++)
        {
            openList.Add(neighbors[i]);

            CalcValue(current, neighbors[i], 0);
        }
    }

    private void CalcValue(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);

        closedList.Add(current);
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
        if (tileType == TileType.WATER)
        {
            waterTiles.Add(clickPos);
        }
        tileMap.SetTile(clickPos, tiles[(int)tileType]);
    }
}
