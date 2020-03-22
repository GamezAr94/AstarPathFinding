using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarDebbuger : MonoBehaviour
{
    private static AStarDebbuger instance;
    public static AStarDebbuger myInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AStarDebbuger>();
            }

            return instance;
        }
    }
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tile tile;
    [SerializeField]
    private Color openColor, closedColor, pathColor, currenColor, startColor, goalColor;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject debugTextPrefab;
    private List<GameObject> gameObjects = new List<GameObject>();
    public void CreateTiles(HashSet<Node> openList, Vector3Int start, Vector3Int goal)
    {
        foreach(Node node in openList)
        {
            ColorTile(node.Position, openColor);
        }

        ColorTile(start, startColor);
        ColorTile(goal, goalColor);
    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }
}
