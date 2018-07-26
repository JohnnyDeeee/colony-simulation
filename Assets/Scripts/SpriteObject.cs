using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteObject : MonoBehaviour {
    
    public void SetParent(GameObject parent) {
        this.transform.parent = parent.transform;
    }

    public Sprite GetSprite() { return this.GetComponent<SpriteRenderer>().sprite; }
}