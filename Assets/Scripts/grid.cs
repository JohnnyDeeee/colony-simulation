using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Grid {
    private Vector2 levelSize;
    private char[,] level;
    private Tile[,] tiles;
    private GameObject gameObject;

    public Grid(char[,] level) {
        this.level = level;

        // GetLength 0-1 because we only deal with 2 dimensions in this 2D simulation
        this.levelSize = new Vector2(this.level.GetLength(0), this.level.GetLength(1));

        // Create the grid in memory
        this.tiles = new Tile[this.levelSize.x, this.levelSize.y];
        for(int x = 0; x < this.levelSize.x; x++) {
            for(int y = 0; y < this.levelSize.y; y++) {
                Tile tile = null;

                if(this.level[x, y] == TileDefinitions.TILE_WALL._char)
                    tile = new TileWall("tile_wall", new Vector2(x, y));
                else if(this.level[x, y] == TileDefinitions.TILE_GROUND._char)
                    tile = new TileGround("tile_ground", new Vector2(x, y));
                else
                    throw new Exception(String.Format("Tile type '{0}' not found", this.level[x, y]));

                this.tiles[x, y] = tile;
            }
        }

        // Create AI
        int spawnSize = 10;
        for(int i = 0; i < spawnSize; i++) {
            int randX = UnityEngine.Random.Range(0, this.levelSize.x);
            int randY = UnityEngine.Random.Range(0, this.levelSize.y);
            Tile randTile = this.GetTile(randX, randY);

            if(!randTile.CanSpawnAi()){
                i--;
                continue;
            }

            randTile.AddAI(new AIAnt("ai_ant", randTile.GetPosition()));
        }
    }

    public void Render() {
        this.CleanUp();

        // Create the parent object
        this.gameObject = new GameObject("grid");

        // Instantiate all tiles from memory to viewport
        for(int x = 0; x < this.levelSize.x; x++) {
            for(int y = 0; y < this.levelSize.y; y++) {
                Tile tile = this.tiles[x, y];
                tile.SetParent(this.gameObject);
                tile.Render();
            }
        }
    }

    public void CleanUp() {
        GameObject.Destroy(this.gameObject);
    }

    public Tile GetTile(int posX, int posY) { return this.tiles[posX, posY]; }

    public Vector2 GetTilePosition(Tile tile) {
        for (int x = this.tiles.GetLowerBound(0); x <= this.tiles.GetUpperBound(0); x++)
            for (int y = this.tiles.GetLowerBound(1); y <= this.tiles.GetUpperBound(1); y++)
                if(this.tiles[x, y] == tile)
                    return new Vector2(x, y);
        return null;
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

    public GameObject GetGameObject() { return this.gameObject; }
}