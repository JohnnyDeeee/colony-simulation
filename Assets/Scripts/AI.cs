using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : SpriteObject {
    private bool selected;
    private Color originalColor;
    private int nextAgeUpdate = 1;
    private int ageInterval = 1; // Seconds
    protected NeuralNetwork network;
    protected List<double> networkInput = new List<double>();
    protected double[] networkOutput;
    public Color selectedColor = Color.red; // TODO: Change into a border or something?
    public float rotationMultiplier { get; private set;}
    public float movementMultiplier { get; private set;}
    public float nextRotation { get; private set;}
    public Vector2 nextVelocity { get; private set;}
    public float maxSeeDistance { get; private set;}
    public double[][] vision { get; private set;}
    public int eyeAmount { get; private set;}
    public int age { get; private set;}

    public float fitness { get { return this.age; } private set { this.fitness = value; } }
    public float normFitness { get; set; } // Normalized fitness
    public float accNormFitness { get; set; } // Accumulated Normalized fitness

    public void Awake() {
        this.originalColor = this.GetComponent<SpriteRenderer>().color;
        this.rotationMultiplier = 10f;
        this.movementMultiplier = 5f;
        this.maxSeeDistance = 10f;

        this.eyeAmount = 2;

        int inputLayerSize = 3 * this.eyeAmount; // Vision in RGB (3)
        int outputLayerSize = 2; // Rotation, velocity
        this.network = new NeuralNetwork(inputLayerSize, 3, outputLayerSize);

        this.nextVelocity = Vector2.zero;
        this.nextRotation = 0f;

        this.vision = new double[this.eyeAmount][];
        for (int i = 0; i < this.vision.Length; i++){
            this.vision[i] = new double[3];
        }

        this.nextAgeUpdate = Mathf.CeilToInt(Time.time);
    }

    public void Update() {
        for (int i = 0; i < this.vision.Length; i++){
            this.vision[i] = new double[3]{0, 0, 0}; // Reset the vision (done at beginning so that UIAIInfo can access it)
        }

        // TODO: Make raycast for each eye ?
        
        // Cast rays (the "eyes" part of the AI)
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.localPosition, this.transform.up, this.maxSeeDistance);
        hits = hits.Where(x => { // Exclude Ground Tile and myself form hits
            return !x.collider.GetComponent<TileGround>() && x.collider.GetComponent<AI>() != this;
        }).ToArray();
        Debug.DrawRay(this.transform.localPosition, this.transform.up * this.maxSeeDistance, Color.red, 0);

        // TODO: Choose hits randomly? Now we just pick the first X ones (where X = this.eyeAmount)
        // So when we see 2 food_tiles we cannot see the AI that might be in front of us..

        for(int i = 0; i < hits.Length; i++) {
            if(i >= this.eyeAmount)
                break;

            RaycastHit2D hit = hits[i];
            Collider2D collider = hit.collider;
            
            // Get RGB from colliding tile sprite
            SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
            this.vision[i] = new double[3] {sr.color.r, sr.color.g, sr.color.b};

            // Draw a ray towards what we are seeing
            Debug.DrawRay(this.transform.localPosition, collider.transform.GetComponent<Renderer>().bounds.center - this.transform.localPosition, Color.blue, 0);

            // break; // Break after first contact
        }

        Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();

        // Let the "brain" do its thing with the given inputs
        for (int i = 0; i < this.vision.Length; i++){
            this.vision[i].ToList().ForEach(x => { this.networkInput.Add(x); }); // Add vision to the input array
        }
        
        this.networkOutput = this.network.FeedForward(this.networkInput.ToArray()); // Brain calculating
        float rotation = (float)this.networkOutput[0] * this.rotationMultiplier;
        float velocity = (float)this.networkOutput[1] * this.movementMultiplier;
        
        this.nextRotation = (rotation + rigidBody.rotation);        
        this.nextVelocity = new Vector2(velocity, velocity);

        // Update age
        if(Time.time >= this.nextAgeUpdate) {
            this.age += 1;
            this.nextAgeUpdate += this.ageInterval;
        }  

        this.networkInput = new List<double>(); // Reset network input
    }

    public void FixedUpdate() {
        Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();

        // Move according to the output of the "brain"
        rigidBody.rotation = Mathf.LerpAngle(rigidBody.rotation, this.nextRotation, 10f * Time.fixedDeltaTime);
        rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, (this.transform.rotation * Vector2.up) * this.nextVelocity, 10f * Time.fixedDeltaTime);
    }

    public void OnMouseDown() {
        if(World.GetSelectedAI() == this) {
            this.MarkUnselected(); 
            World.SetSelectedAI(null);  
        } else {
            this.MarkUnselected();
            World.SetSelectedAI(this);
        }
    }

    public void MarkSelected() {
        if(!this.selected) {
            this.selected = true;
            this.GetComponent<SpriteRenderer>().color = this.selectedColor;
            this.GetComponent<SpriteRenderer>().sortingOrder = 1;
            Camera.main.GetComponent<Cam>().FollowObject(this.gameObject);
        }
    }

    public void MarkUnselected() {
        if(this.selected) {
            this.selected = false;
            this.GetComponent<SpriteRenderer>().color = this.originalColor;
            Camera.main.GetComponent<Cam>().StopFollowing(this.gameObject);
        }
    }

    public BitArray GetGenome() {
        byte[] bytes = this.network.GetInputWeights().SelectMany(x => BitConverter.GetBytes(x)).ToArray();
        return new BitArray(bytes);
    }

    public void SetGenome(BitArray genome) {
        byte[] bytes = new byte[genome.Length / 8];
        for (int i = 0; i < genome.Length; i+=8) { // 8 bits in a byte
            // Convert bits to a byte
            List<bool> bits = genome.OfType<bool>().ToList().GetRange(i, 8).ToList();
            byte @byte = 0;
            bits.ForEach(x => { @byte <<= 1; if(x) @byte |= 1; });
            bytes[i / 8] = @byte;
        }
        double[] weights = new double[bytes.Length / 8]; // 8 bytes in a double
        for (int i = 0; i < weights.Length; i++) {
            weights[i] = BitConverter.ToDouble(bytes, i);
        }

        this.network = new NeuralNetwork(this.network.GetInputLayerSize(), this.network.GetHiddenLayerSize(), this.network.GetOutputLayerSize(), weights);
    }
}