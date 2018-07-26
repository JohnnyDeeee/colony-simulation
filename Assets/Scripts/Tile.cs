using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : SpriteObject {
    [SerializeField] protected bool canSpawnAi = true;
    [SerializeField] protected List<AI> aiList = new List<AI>(); // ai's that are currently on this tile

    public void AddAI(AI ai) {
        if(this.canSpawnAi)
            this.aiList.Add(ai);
        else
            throw new Exception("This tile can not spawn ai(s)");
    }

    public bool CanSpawnAi() { return this.canSpawnAi; }
}