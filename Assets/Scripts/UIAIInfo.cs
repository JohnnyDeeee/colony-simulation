using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAIInfo : UI {
    [SerializeField] private Text text;
    [SerializeField] private RawImage visonImage;
    [SerializeField] private RawImage antColorImage;
    [SerializeField] private Text deceased;

    public new void Start () {
        
    }
    
    public void Update () {
        this.text.text = "";
        this.visonImage.gameObject.SetActive(false);
        this.antColorImage.gameObject.SetActive(false);
        this.deceased.gameObject.SetActive(false);

        AI ai = World.GetSelectedAI();

        if(!ai)
            return;

        List<string> textLines = new List<string>();
        textLines.Add("AI info");
        // AI info
        textLines.Add("rotation multiplier: " + ai.rotationMultiplier);
        textLines.Add("movement multiplier: " + ai.movementMultiplier);
        textLines.Add("next rotation: " + ai.nextRotation);
        textLines.Add("next velocity: " + ai.nextVelocity);
        textLines.Add("max see distance: " + ai.maxSeeDistance);
        textLines.Add("vision: ");
        this.visonImage.gameObject.SetActive(true);
        this.visonImage.color = new Color((float)ai.vision[0], (float)ai.vision[1], (float)ai.vision[2]);
        textLines.Add("age: " + ai.age);
        // AIAnt info
        AIAnt ant = ai as AIAnt;
        if(ant) {
            textLines.Add("food amount: " + ant.foodAmount);
            textLines.Add("max food amount: " + ant.maxFoodAmount);
            textLines.Add("eat probability: " + ant.nextEatProbabillity);
            textLines.Add("food depletion multiplier: " + ant.foodDepletionMultiplier);
            textLines.Add("distance travelled: " + ant.distanceTravelled);
            textLines.Add("color: ");
            this.antColorImage.gameObject.SetActive(true);
            this.antColorImage.color = ant.color;
            if(ant.dead)
                this.deceased.gameObject.SetActive(true);
        }
        
        textLines.ForEach(x => {
            this.text.text += x;
            this.text.text += this.newLine;
        });
    }
}