using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : SpriteObject {
    protected bool spawnAi = true;
    protected List<AI> aiList = new List<AI>(); // ai's that are currently on this tile

    public Tile(string name, GameObject prefab, Vector2 position) : base(name, prefab, position) {
    }

    public new void Render() {
        base.Render();

        // Render AI
        foreach(AI ai in this.aiList){
            ai.SetParent(this.instance);
            ai.Render();
        }
    }

    public void AddAI(AI ai) {
        if(this.spawnAi)
            this.aiList.Add(ai);
        else
            throw new Exception("This tile can not spawn ai(s)");
    }

    public bool CanSpawnAi() { return this.spawnAi; }
}