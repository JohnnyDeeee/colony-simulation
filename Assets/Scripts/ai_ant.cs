using UnityEngine;

public class AIAnt : AI {
    public readonly static new GameObject prefab = ResourcesList.AI_ANT;

    public AIAnt(string name, Vector2 position) : base(name, prefab, position) {

    }
}