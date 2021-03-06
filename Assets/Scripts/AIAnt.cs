using System.Linq;
using UnityEngine;

public class AIAnt : AI {
    public float foodAmount {get; private set;}
    public float maxFoodAmount {get; private set;}
    public bool dead {get; private set;}
    public float nextEatProbabillity {get; private set;}
    public float foodDepletionMultiplier {get; private set;}
    public float distanceTravelled {get; private set;}
    public Color color {get; private set;}
    public int generation {get; private set;}

    public new void Awake() {
        base.Awake();

        this.network = new NeuralNetwork(
            this.network.GetInputLayerSize() + 1,
            this.network.GetHiddenLayerSize(),
            this.network.GetOutputLayerSize() + 1); // Add foodAmount as an input, add extra output for nextEatProbabillity
        this.maxFoodAmount = 1.0f; // 100%
        this.foodAmount = this.maxFoodAmount;
        this.foodDepletionMultiplier = 1.0f;
        this.generation = World.generation;

        this.GetComponent<Rigidbody2D>().rotation = Random.Range(-180, 180+1);
    }

    public new void Update() {
        if(this.dead)
            return;

        if(this.foodAmount <= 0) {
            this.Die();
            return;
        }

        // Add foodAmount as an input
        this.networkInput.Add(this.foodAmount);

        base.Update();

        if(this.networkOutput != null)
            this.nextEatProbabillity = (float)this.networkOutput[2]; // First 2 are used in base class

        // Translate the network weights to a color
        // idea here is to be able to see if ants have a brain that looks alike
        // TODO: Check what happens to kids when we implement mating
        double[] inputWeights = this.network.GetInputWeights();
        float[] colorValues = new float[inputWeights.Length];
        for(int i = 0; i < inputWeights.Length; i++) {
            float colorValue = (10f * (float)inputWeights[i]);
            colorValue = Mathf.Clamp01(colorValue);
            
            // Merge this color with the previous one
            if(i > 0)
                colorValue = (colorValues[i-1] * 0.5f) + (colorValue * 0.5f);
            
            colorValues[i] = Mathf.Abs(colorValue);
        }
        this.color = Color.HSVToRGB(colorValues[colorValues.Length-1], 1f, 1f);

        this.GetComponent<SpriteRenderer>().color = this.color;
    }

    // Important to do this in fixedUpdate, because this needs to be called after every movement (base.fixedUpdate)
    public new void FixedUpdate() {
        if(this.dead)
            return;

        base.FixedUpdate();

        // Lose food depending on how much you have travelled in the last frame (and the multiplier)
        Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();
        float distanceTravelledInLastFrame = Vector2.Distance(rigidBody.position, rigidBody.position + rigidBody.velocity);
        this.foodAmount -= (this.distanceTravelled - distanceTravelledInLastFrame) * (this.foodDepletionMultiplier * 0.000001f);
        this.distanceTravelled += distanceTravelledInLastFrame;
    }

    public void OnTriggerStay2D(Collider2D collider) {
        if(this.dead)
            return;

        TileFood foodTile = collider.GetComponent<TileFood>();
        if(foodTile) { // Eat food from food tile when colliding with it
            if(Random.value > this.nextEatProbabillity)
                this.EatFoodFromTile(foodTile);
        }
    }

    private void EatFoodFromTile(TileFood tile) {
        // Take food from food tile
        if(tile.foodAmount > 0 && this.foodAmount < this.maxFoodAmount) {
            // Eat 0.1f or the amount to get to maxFoodAmount (if the second is less than 0.1f ofcourse)
            float amountToEat = this.maxFoodAmount - this.foodAmount >= 0.1f ? 0.1f : this.maxFoodAmount - this.foodAmount;
            tile.foodAmount -= amountToEat;
            this.foodAmount += amountToEat;
        }
    }

    private void Die() {
        this.foodAmount = 0;
        this.dead = true;
        Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();
        rigidBody.velocity = Vector2.zero;
        rigidBody.freezeRotation = true;
        rigidBody.simulated = false;

        // TODO: Let bodies decintegrate over time and provide food for carnivores
    }
}