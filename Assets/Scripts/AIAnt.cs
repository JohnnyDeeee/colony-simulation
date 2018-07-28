using UnityEngine;

public class AIAnt : AI {
    
    public new void Start() {
        base.Start();

        this.GetComponent<Rigidbody2D>().rotation = Random.Range(-180, 180+1);
    }
}