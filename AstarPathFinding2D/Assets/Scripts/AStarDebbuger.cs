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
    private List<GameObject> debugObjects = new List<GameObject>();
    public void CreateTiles(HashSet<Node> openList,HashSet<Node> closedList,Dictionary<Vector3Int,Node> allNodes, Vector3Int start, Vector3Int goal)
    {
        foreach(Node node in openList)
        {
            ColorTile(node.Position, openColor);
        }
        foreach(Node node in closedList){
            ColorTile(node.Position, closedColor);
        }

        ColorTile(start, startColor);
        ColorTile(goal, goalColor);

        foreach(KeyValuePair<Vector3Int,Node> node in allNodes){
            if(node.Value.Parent != null)
            {
                GameObject go = Instantiate(debugTextPrefab, canvas.transform);
                go.transform.position = grid.CellToWorld(node.Key);
                debugObjects.Add(go);
                GenerateDebugText(node.Value, go.GetComponent<DebugText>());
            }
        }
    }

    private void GenerateDebugText(Node node, DebugText debugText)
    {

        debugText.F.text = $"F: {node.F}";
        debugText.G.text = $"G: {node.G}";
        debugText.H.text = $"H: {node.H}";
        debugText.P.text = $"P: {node.Position.x},{node.Position.y}";
        if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y == node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 135));
        }
        else if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 225));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y == node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, -45));
        }
        else if (node.Parent.Position.x == node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (node.Parent.Position.x == node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        }
    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }
}
