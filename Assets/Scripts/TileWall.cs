using UnityEngine;

public class TileWall : Tile {
    
    public void Awake() {
        this.canSpawnAi = false;
    }
}