using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {
    [SerializeField] private float dragSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float currentZoom;
    private GameObject followingObject;
    
    private new Camera camera;

    public void Start() {
        this.camera = this.GetComponent<Camera>();
        this.minZoom = 2.6f;
        this.maxZoom = this.camera.orthographicSize; // We use the value set by World.cs
        this.currentZoom = this.camera.orthographicSize;
    }

    public void Update () {
        // Dragging right mouse button
        if(Input.GetMouseButton(1)) {
            this.transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * this.dragSpeed * -1, Input.GetAxisRaw("Mouse Y") * Time.deltaTime * this.dragSpeed * -1, 0f);
        }

        // Scroll forward
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && this.currentZoom >= this.minZoom) {
            ZoomToPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), this.zoomSpeed);
        }

        // Scoll back
        if(Input.GetAxis("Mouse ScrollWheel") < 0 && this.currentZoom <= this.maxZoom) {
            ZoomToPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), this.zoomSpeed * -1f);
        }

        if(followingObject)
            this.transform.position = followingObject.transform.position;

        this.dragSpeed = this.currentZoom + 10f;
        this.zoomSpeed = this.currentZoom * 0.1f;

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10f); // Lock z-axis
    }

    public void ZoomToPosition(Vector3 zoomTowards, float amount = 0f) {
        // Calculate how much we will have to move towards the zoomTowards position
        float multiplier = (1.0f / this.camera.orthographicSize * amount);

        // Move camera
        this.transform.position += (zoomTowards - this.transform.position) * multiplier;

        float newZoom = this.camera.orthographicSize - amount;
        if(amount == 0f) // Zoom all the way in
            newZoom = minZoom;

        // Zoom camera
        this.currentZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
        this.camera.orthographicSize = this.currentZoom;

        Debug.Log("Zoomed!");
    }

    public void FollowObject(GameObject _object) {
        this.followingObject = _object;
        this.ZoomToPosition(this.followingObject.transform.position);
    }

    public void StopFollowing(GameObject _object) {
        if(this.followingObject == _object)
            this.followingObject = null;
    }
}