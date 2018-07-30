using UnityEngine;

public class TileFood : Tile {
    public float foodAmount = 1.0f; // 100%

    public void Awake() {
        this.canSpawnAi = true;
    }

    public void Update() {
        if(this.foodAmount <= 0f) {
            TileDefinitions.TILE_GROUND.Instantiate(this.transform.position, World.GetGrid().gameObject);
            GameObject.Destroy(this.gameObject);
        }

        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        renderer.color = Color.Lerp(TileDefinitions.TILE_GROUND.color, TileDefinitions.TILE_FOOD.color, foodAmount);
    }
}