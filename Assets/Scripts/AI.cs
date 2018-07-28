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

                // Draw a ray towards what we are seeing
                Debug.DrawRay(this.transform.position, collider.transform.position - this.transform.position, Color.blue, 0);

                break; // Break after first contact
            }
        }

        // Let the "brain" do its thing, if we see something
        if(this.vision != null)
            this.rotationSpeed = (float)this.network.FeedForward(vision)[0] * 10;

        // Move according to the output of the "brain"
        Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();
        this.rotation = (rigidBody.rotation + (this.rotationSpeed));
        rigidBody.MoveRotation(rotation);

        // Constant movement forward
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