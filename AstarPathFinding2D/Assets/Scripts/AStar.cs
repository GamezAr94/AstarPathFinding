using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private Stack<Vector3Int> path;

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

        while(openList.Count > 0 && path == null)
        {
            List<Node> neighbors = findNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
        }

        AStarDebbuger.myInstance.CreateTiles(openList, closedList, allNodes, startPos, goalPos, path);

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
            Node neighbor = neighbors[i];

            if (!ConnectedDiagonally(current,neighbor))
            {
                continue;
            }

            int gScore = DetermineGScore(neighbors[i].Position, current.Position);

            if (openList.Contains(neighbor))
            {
                if(current.G + gScore < neighbor.G)
                {
                    CalcValue(current, neighbor, gScore);
                }
            }else if (!closedList.Contains(neighbor))
            {
                CalcValue(current, neighbor, gScore);

                openList.Add(neighbor);
            }
        }
    }

    private void CalcValue(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;

        neighbor.G = parent.G + cost;

        neighbor.H = ((Math.Abs((neighbor.Position.x - goalPos.x)) + Math.Abs((neighbor.Position.y - goalPos.y))) * 10);

        neighbor.F = neighbor.G + neighbor.H;

    }

    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if (Math.Abs(x - y) % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }
        return gScore;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);

        closedList.Add(current);

        if(openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
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
        //Ignoring all the tiles tipe Water in the Enum list
        if (tileType == TileType.WATER)
        {
            waterTiles.Add(clickPos);
        }
        tileMap.SetTile(clickPos, tiles[(int)tileType]);
    }

    //Avoid Walk in Diagonall
    private bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        Vector3Int direct = currentNode.Position - neighbor.Position;

        Vector3Int first = new Vector3Int(current.Position.x + (direct.x * -1), current.Position.y, current.Position.z);
        Vector3Int second = new Vector3Int(current.Position.x, current.Position.y + (direct.y * -1), current.Position.z);

        if(waterTiles.Contains(first)|| waterTiles.Contains(second))
        {
            return false;
        }
        return true; ;
    }

    private Stack<Vector3Int> GeneratePath(Node current)
    {
        if(current.Position == goalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

            while(current.Position != startPos)
            {
                finalPath.Push(current.Position);

                current = current.Parent;
            }
            return finalPath;
        }
        return null;
    }
}
