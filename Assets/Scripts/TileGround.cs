using UnityEngine;

public class TileGround : Tile {
    
    public void Awake() {
        this.canSpawnAi = true;
    }
}