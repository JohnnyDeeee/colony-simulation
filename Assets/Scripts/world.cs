using UnityEngine;

public class World {
    private Grid grid;

    public World(char[,] level) {
        this.grid = new Grid(level);
        this.grid.Render();

        this.SetupCamera(this.grid.GetSizeInUnits(), this.grid.GetTileSizeInPixels().x);
    }

    private void SetupCamera(Vector2 sizeInUnits, int tileHeight) {
        Camera cam = Camera.main;
        
        Debug.Log(string.Format("screen width: {0}, screen height: {1}, tile height: {2}", Screen.width, Screen.height, tileHeight));

        // Set size
        float offset = 0f;
        cam.orthographicSize = (sizeInUnits.y / 2) + (sizeInUnits.x / 5) + offset;

        // Set Position
        cam.transform.SetPositionAndRotation(new Vector3((sizeInUnits.x / 2) - 0.5f, ((sizeInUnits.y / 2) -0.5f) * -1, cam.transform.position.z), cam.transform.rotation);
    }
}