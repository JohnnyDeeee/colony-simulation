using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldInfo : UI {
    private Text text;

    public new void Start () {
        this.text = this.GetComponent<Text>();
        this.text.rectTransform.anchoredPosition = new Vector2(0, 0);
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