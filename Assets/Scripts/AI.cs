using UnityEngine;

public class AI : SpriteObject {
    private bool selected;
    private Color originalColor;
    private NeuralNetwork network;
    [SerializeField] private Color selectedColor = Color.red;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationMultiplier;
    [SerializeField] private float movementMultiplier;
    [SerializeField] private float maxSeeDistance;
    [SerializeField] private double[] vision;

    [SerializeField] private Color visionColor;

    public void Start() {
        this.originalColor = this.GetComponent<SpriteRenderer>().color;
        this.rotationMultiplier = 10f;
        this.movementMultiplier = 5f;
        this.maxSeeDistance = 10f;

        this.movementSpeed = 1.0f; // Initial speed, otherwise if the ai doesn't see anything, it wont move..

        int inputLayerSize = 3; // 1 eye can see RGB
        this.network = new NeuralNetwork(inputLayerSize, 3, 2); // output: rotationSpeed, velocity
    }

    public void Update() {
        this.vision = null; // Reset the vision

        // Cast rays (the "eyes" part of the AI)
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, this.transform.up, this.maxSeeDistance);
        Debug.DrawRay(this.transform.position, this.transform.up * this.maxSeeDistance, Color.red, 0);
        if(hits.Length > 0) {
            foreach(RaycastHit2D hit in hits) {
                Collider2D collider = hit.collider;

                // Ignore ground tiles and (if AI) itself
                if(collider.GetComponent<TileGround>() || (collider.GetComponent<AI>() && collider.GetComponent<AI>() == this))
                    continue;
                
                // Get RGB from colliding tile sprite
                SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
                this.vision = new double[3] {sr.color.r, sr.color.g, sr.color.b};

                // DEBUG
                this.visionColor = new Color((float)this.vision[0], (float)this.vision[1], (float)this.vision[2]);

                // Draw a ray towards what we are seeing
                Debug.DrawRay(this.transform.position, collider.transform.position - this.transform.position, Color.blue, 0);

                break; // Break after first contact
            }
        }

        // Let the "brain" do its thing, if we see something
        if(this.vision != null) {
            double[] output = this.network.FeedForward(vision);
            this.rotationSpeed = (float)output[0] * this.rotationMultiplier;
            this.movementSpeed = (float)output[1] * this.movementMultiplier;
        }

        // Move according to the output of the "brain"
        Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();
        float rotation = (rigidBody.rotation + (this.rotationSpeed));
        rigidBody.MoveRotation(rotation);
        rigidBody.velocity = (this.transform.rotation * Vector2.up) * this.movementSpeed;
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
}