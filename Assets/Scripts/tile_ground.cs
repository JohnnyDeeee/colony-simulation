using UnityEngine;

public class TileGround : Tile {
    private readonly static new GameObject prefab = TileDefinitions.TILE_GROUND.prefab;

    public TileGround(string name, Vector2 position) : base(name, prefab, position) {
    }
}