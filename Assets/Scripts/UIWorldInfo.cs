using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldInfo : UI {
    [SerializeField] private Text text;

    public new void Start () {
        
    }
    
    public void Update () {
        text.text = "";

        List<string> textLines = new List<string>();
        textLines.Add("World info");
        textLines.Add("seed: " + World.GetSeed());
        textLines.Add("age: " + World.age);
        
        textLines.ForEach(x => {
            text.text += x;
            text.text += this.newLine;
        });
    }
}