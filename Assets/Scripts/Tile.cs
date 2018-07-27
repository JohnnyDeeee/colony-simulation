using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : SpriteObject {
    [SerializeField] protected bool canSpawnAi = true;
    [SerializeField] protected List<AI> aiList = new List<AI>(); // ai's that are currently on this tile

    public void AddAI(AI ai) {
        this.aiList.Add(ai);
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        AI colliderAI = collider.gameObject.GetComponent<AI>();
        if(colliderAI)
            this.AddAI(colliderAI);
    }

    public void OnTriggerExit2D(Collider2D collider) {
        AI colliderAI = collider.gameObject.GetComponent<AI>();
        if(colliderAI)
            this.aiList.Remove(colliderAI);
    }

    public bool CanSpawnAi() { return this.canSpawnAi; }
}