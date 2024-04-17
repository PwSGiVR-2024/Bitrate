using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class InfiniteTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public List<TileBase> tiles; // List to hold different types of tiles
    public Transform player;

    private Vector3Int previousPlayerPos;
    private int generationRadius = 50;  // The radius around the player that tiles will be generated in
    private int bufferZone = 5;  // Additional buffer zone for tile generation

    private void Start()
    {
        previousPlayerPos = tilemap.WorldToCell(player.position);
        GenerateInitialTiles(generationRadius + bufferZone); // Generate an initial large area around the player
    }

    private void Update()
    {
        Vector3Int currentPlayerPos = tilemap.WorldToCell(player.position);
        if (!currentPlayerPos.Equals(previousPlayerPos))
        {
            UpdateTilesAroundPlayer(currentPlayerPos);
            previousPlayerPos = currentPlayerPos;
        }
    }

    void GenerateInitialTiles(int radius)
    {
        Vector3Int startCell = tilemap.WorldToCell(player.position);
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector3Int pos = new Vector3Int(startCell.x + x, startCell.y + y, startCell.z);
                tilemap.SetTile(pos, GetRandomTile());
            }
        }
    }

    void UpdateTilesAroundPlayer(Vector3Int playerCell)
    {
        for (int x = -generationRadius; x <= generationRadius; x++)
        {
            for (int y = -generationRadius; y <= generationRadius; y++)
            {
                Vector3Int tilePos = new Vector3Int(playerCell.x + x, playerCell.y + y, playerCell.z);
                if (tilemap.GetTile(tilePos) == null)
                {
                    tilemap.SetTile(tilePos, GetRandomTile());
                }
            }
        }
    }

    // Method to randomly select a tile
    TileBase GetRandomTile()
    {
        if (tiles.Count == 0) return null; // Guard clause to prevent errors
        int index = Random.Range(0, tiles.Count);
        return tiles[index];
    }
}
