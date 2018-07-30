using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAIInfo : UI {
    private Text text;

    public new void Start () {
        this.text = this.GetComponent<Text>();
        this.text.rectTransform.anchoredPosition = new Vector2(0, -88);
    }
    
    public void Update () {
        text.text = "";

        AI ai = World.GetSelectedAI();

        if(!ai)
            return;

        List<string> textLines = new List<string>();
        textLines.Add("AI info");
        textLines.Add("rotation multiplier: " + ai.rotationMultiplier);
        textLines.Add("movement multiplier: " + ai.movementMultiplier);
        textLines.Add("next rotation: " + ai.nextRotation);
        textLines.Add("next velocity: " + ai.nextVelocity);
        textLines.Add("max see distance: " + ai.maxSeeDistance);
        textLines.Add("vision: " + ai.vision);
        textLines.Add("age: " + ai.age);
        
        textLines.ForEach(x => {
            text.text += x;
            text.text += this.newLine;
        });
    }
}