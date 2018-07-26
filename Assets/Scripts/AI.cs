using UnityEngine;

public class AI : SpriteObject {
    private bool selected;
    private Color originalColor;
    [SerializeField] private Color selectedColor = Color.red;

    public void Start() {
        this.originalColor = this.GetComponent<SpriteRenderer>().color;
    }

    public void Update() {

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