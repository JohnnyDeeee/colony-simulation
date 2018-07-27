using UnityEngine;

public class AI : SpriteObject {
    private bool selected;
    private Color originalColor;
    private NeuralNetwork network;
    [SerializeField] private Color selectedColor = Color.red;
    [SerializeField] private float speed;
    [SerializeField] private float rotation;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxSeeDistance;
    [SerializeField] private double[] vision;

    public void Start() {
        this.originalColor = this.GetComponent<SpriteRenderer>().color;
        this.speed = 3.5f;
        this.maxSeeDistance = 10f;

        int inputLayerSize = 3; // 1 eye can see RGB
        this.network = new NeuralNetwork(inputLayerSize, 3, 1);
    }

    public void Update() {
        // Cast rays (the "eyes" part of the AI)
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.up, this.maxSeeDistance);
        Debug.DrawRay(this.transform.position, this.transform.up * this.maxSeeDistance, Color.red, 0);
        if(hit.collider) {
            // TODO: Get RGB from tile sprite
        }

        // TODO: Get these from actual vision (rays)
        // Ant example inputs
        this.vision = new double[6];
        // Left eye
        vision[0] = Random.Range(0, 255); // R
        vision[1] = Random.Range(0, 255); // G
        vision[2] = Random.Range(0, 255); // B
        // Right eye
        vision[3] = Random.Range(0, 255); // R
        vision[4] = Random.Range(0, 255); // G
        vision[5] = Random.Range(0, 255); // B

        this.rotationSpeed = (float)this.network.FeedForward(vision)[0];
        
        Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();
        this.rotation = (rigidBody.rotation + (this.rotationSpeed));

        rigidBody.MoveRotation(rotation);
        rigidBody.velocity = (this.transform.rotation * Vector2.up) * this.speed;
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