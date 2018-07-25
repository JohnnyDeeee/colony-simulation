using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Grid {
    private Vector2 levelSize;
    private Vector2 tileScaleInUnits;
    private char[,] level;
    private Tile[,] tiles;
    private GameObject gameObject;

    public Vector2 GetSizeInTiles() { return this.levelSize; }
    public Vector2 GetSizeInPixels() {
        Vector2 sizeInTiles = this.GetSizeInTiles();
        Vector2 sizeInPixels = new Vector2(sizeInTiles.x * (this.GetTileSizeInPixels().x * tileScaleInUnits.x), sizeInTiles.y * (this.GetTileSizeInPixels().y * tileScaleInUnits.y));
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
    public Vector2 GetTileScaleInUnits() { return this.tileScaleInUnits; }

    public Grid(char[,] level, Vector2 tileScale) {
        this.level = level;
        this.tileScaleInUnits = tileScale;

        // GetLength 0-1 because we only deal with 2 dimensions in this 2D simulation
        this.levelSize = new Vector2(this.level.GetLength(0), this.level.GetLength(1));

        // Create the grid in memory
        this.tiles = new Tile[this.levelSize.x, this.levelSize.y];
        for(int x = 0; x < this.levelSize.x; x++) {
            for(int y = 0; y < this.levelSize.y; y++) {
                Tile tile = null;

                if(this.level[x, y] == TileDefinitions.TILE_WALL._char)
                    tile = new TileWall(string.Format("tile_wall_{0}_{1}", x, y), this.tileScaleInUnits);
                else if(this.level[x, y] == TileDefinitions.TILE_GROUND._char)
                    tile = new TileGround(string.Format("tile_ground_{0}_{1}", x, y), this.tileScaleInUnits);
                else
                    throw new Exception(String.Format("Tile type '{0}' not found", this.level[x, y]));

                this.tiles[x, y] = tile;
            }
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
                Vector2 offset = this.tileScaleInUnits;
                Vector3 newPos = new Vector3(x * offset.x, y * offset.y * -1, 0); // * -1 to make sure pos 0,0 is top-left
                GameObject tilePrefab = tile.GetPrefab();
                GameObject tileObj = GameObject.Instantiate(tilePrefab, newPos, Quaternion.identity, this.gameObject.transform);
                tileObj.name = tile.GetName();
            }
        }
    }

    public void CleanUp() {
        GameObject.Destroy(this.gameObject);
    }

    public Tile GetTile(int posX, int posY) {
        return this.tiles[posX, posY];
    }
}