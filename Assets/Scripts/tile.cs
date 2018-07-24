using UnityEngine;

public class Tile : Base {
    private GameObject prefab;
    private Sprite sprite;

    public GameObject GetPrefab() { return this.prefab; }
    public Sprite GetSprite() { return this.sprite; }

    public Tile(string name, GameObject prefab, Vector2 scale) : base(name) {
        this.prefab = prefab;

        this.prefab.transform.localScale = new Vector3(scale.x, scale.y, 0);
        this.sprite = this.prefab.GetComponent<SpriteRenderer>().sprite;
    }
}