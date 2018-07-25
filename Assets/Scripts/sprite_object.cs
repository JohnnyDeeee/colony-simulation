using UnityEngine;

public class SpriteObject {
    protected GameObject parent;
    protected GameObject instance;

    private GameObject prefab;
    private Sprite sprite;
    private string name;
    private Vector2 position;

    public SpriteObject(string name, GameObject prefab, Vector2 position) {
        this.name = name;
        this.prefab = prefab;
        this.position = position;

        this.sprite = this.prefab.GetComponent<SpriteRenderer>().sprite;
    }

    public void Render() {
        Vector3 newPos = new Vector3(this.position.x, this.position.y * -1, 0); // * -1 to make sure pos 0,0 is top-left
        GameObject spriteObj = GameObject.Instantiate(this.prefab, newPos, Quaternion.identity);
        if(this.parent)
            spriteObj.transform.SetParent(this.parent.transform);
        spriteObj.name = this.name;
        this.instance = spriteObj;
    }

    public void SetParent(GameObject parent) {
        this.parent = parent;
    }

    public Sprite GetSprite() { return this.sprite; }
    public string GetName() { return this.name; }
    public Vector2 GetPosition() { return this.position; }
}