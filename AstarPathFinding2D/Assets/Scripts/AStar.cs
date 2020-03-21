using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { STAR, GOAL, WATER, GRASS, PATH}

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
    }

    public void ChangeTileType(TileButton button)
    {
        tileType = button.MyTileType;
    }

    private void ChangeTile(Vector3Int clickPos)
    {
        tileMap.SetTile(clickPos, tiles[(int)tileType]);
    }
}
