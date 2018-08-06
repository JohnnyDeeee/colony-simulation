using System.Collections.Generic;
using System.Linq;
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
        textLines.Add("next population: " + (World.maxPopulationAge - (World.age % World.maxPopulationAge)));
        textLines.Add("generation: " + World.generation);
        textLines.Add("average ant age: " + Mathf.Floor((float)World.ais.Select(x => x.age).Average()));
        textLines.Add("highest ant age: " + World.ais.Select(x => x.age).Max());
        textLines.Add("lowest ant age: " + World.ais.Select(x => x.age).Min());
        textLines.Add("ant population: " + World.ais.Where(x => !(x as AIAnt).dead).Count());
        
        textLines.ForEach(x => {
            text.text += x;
            text.text += this.newLine;
        });
    }
}