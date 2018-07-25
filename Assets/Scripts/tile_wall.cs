using UnityEngine;

public class TileWall : Tile {
    public readonly static new GameObject prefab = TileDefinitions.TILE_WALL.prefab;

    public TileWall(string name, Vector2 scale) : base(name, prefab, scale) {

    }
}