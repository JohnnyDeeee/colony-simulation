using UnityEngine;

public class TileFood : Tile {
    public float foodAmount = 1.0f; // 100%

    public void Awake() {
        this.canSpawnAi = true;
    }

    public void Update() {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        // renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, foodAmount);
        
        // renderer.color = Color.Lerp(new Color(134, 102, 1), renderer.color, foodAmount);
        renderer.color = Color.Lerp(TileDefinitions.TILE_GROUND.color, TileDefinitions.TILE_FOOD.color, foodAmount);
    }
}