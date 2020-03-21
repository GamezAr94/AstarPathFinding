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

    private void Algorithm()
    {
        AStarDebbuger.myInstance.CreateTiles(startPos,goalPos);
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
