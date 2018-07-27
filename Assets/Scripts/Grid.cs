using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Grid : MonoBehaviour {
    [SerializeField] private Vector2 levelSize;
    private char[,] level;
    private Tile[,] tiles;

    public void Update() {
        if(World.GetSelectedAI())
            World.GetSelectedAI().MarkSelected();
        if(World.GetPreviousSelectedAI())
            World.GetPreviousSelectedAI().MarkUnselected();
    }

    public void Init(char[,] level) {
        this.level = level;

        // GetLength 0-1 because we only deal with 2 dimensions in this 2D simulation
        this.levelSize = new Vector2(this.level.GetLength(0), this.level.GetLength(1));

        // Create the grid in memory
        this.tiles = new Tile[(int)this.levelSize.x, (int)this.levelSize.y];
        for(int x = 0; x < this.levelSize.x; x++) {
            for(int y = 0; y < this.levelSize.y; y++) {
                Tile tile = null;

                if(this.level[x, y] == TileDefinitions.TILE_WALL._char) {
                    GameObject tileInstance = GameObject.Instantiate(ResourcesList.TILE_WALL, new Vector3(x, y, 0), Quaternion.identity);
                    tileInstance.name = "tile_wall";
                    tile = tileInstance.GetComponent<Tile>();
                } else if(this.level[x, y] == TileDefinitions.TILE_GROUND._char) {
                    GameObject tileInstance = GameObject.Instantiate(ResourcesList.TILE_GROUND, new Vector3(x, y, 0), Quaternion.identity);
                    tileInstance.name = "tile_ground";
                    tile = tileInstance.GetComponent<Tile>();
                } else
                    throw new Exception(String.Format("Tile type '{0}' not found", this.level[x, y]));

                this.tiles[x, y] = tile;
                tile.SetParent(this.gameObject);
            }
        }

        // Create AI
        int spawnSize = 100;
        for(int i = 0; i < spawnSize; i++) {
            float randX = UnityEngine.Random.Range(0f, this.levelSize.x);
            float randY = UnityEngine.Random.Range(0f, this.levelSize.y);
            Tile randTile = this.GetTile((int)randX, (int)randY);

            if(!randTile.CanSpawnAi()){
                i--;
                continue;
            }

            GameObject aiInstance = GameObject.Instantiate(ResourcesList.AI_ANT, randTile.transform.position, Quaternion.identity);
            aiInstance.name = "ai_ant";
        }
    }

    public Tile GetTile(int posX, int posY) { return this.tiles[posX, posY]; }

    public Vector2 GetTilePosition(Tile tile) {
        for (int x = this.tiles.GetLowerBound(0); x <= this.tiles.GetUpperBound(0); x++)
            for (int y = this.tiles.GetLowerBound(1); y <= this.tiles.GetUpperBound(1); y++)
                if(this.tiles[x, y] == tile)
                    return new Vector2(x, y);
        return Vector2.negativeInfinity;
    }

    public Tile[,] GetTiles() { return this.tiles; }

    public Vector2 GetSizeInTiles() { return this.levelSize; }

    public Vector2 GetSizeInPixels() {
        Vector2 sizeInTiles = this.GetSizeInTiles();
        Vector2 sizeInPixels = new Vector2(sizeInTiles.x * (this.GetTileSizeInPixels().x), sizeInTiles.y * (this.GetTileSizeInPixels().y));
        return sizeInPixels;
    }

    public Vector2 GetSizeInUnits() {
        Vector2 sizeInPixels = this.GetSizeInPixels();
        int pixelsPerUnit = (int)this.tiles[0,0].GetSprite().pixelsPerUnit;
        Vector2 sizeInUnits = new Vector2(sizeInPixels.x / pixelsPerUnit, sizeInPixels.y / pixelsPerUnit);
        return sizeInUnits;
    }

    public Vector2 GetTileSizeInPixels() {
        Sprite sprite = this.tiles[0,0].GetSprite();
        return new Vector2(sprite.rect.width, sprite.rect.height);
    }
}