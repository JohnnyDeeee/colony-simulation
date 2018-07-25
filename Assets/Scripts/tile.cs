using UnityEngine;

public class Tile {
    protected GameObject prefab;
    private Sprite sprite;
    private string name;

    public GameObject GetPrefab() { return this.prefab; }
    public Sprite GetSprite() { return this.sprite; }
    public string GetName() { return this.name; }

    public Tile(string name, GameObject prefab, Vector2 scale) {
        this.name = name;
        this.prefab = prefab;

        this.prefab.transform.localScale = new Vector3(scale.x, scale.y, 0);
        this.sprite = this.prefab.GetComponent<SpriteRenderer>().sprite;
    }
}