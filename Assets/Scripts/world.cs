using UnityEngine;

public class World : Base {
    private Grid grid;

    public World(string name, char[,] level, Vector2 tileScale = null) : base(name) {
        // Default tileScale to Vector2(1,1)
        tileScale = tileScale == null ? new Vector2(1,1) : tileScale;

        this.grid = new Grid("grid1", level, tileScale);
        this.grid.Render();

        this.SetupCamera(this.grid.GetSizeInUnits(), this.grid.GetTileSizeInPixels().x);
    }

    private void SetupCamera(Vector2 sizeInUnits, int tileHeight) {
        Camera cam = Camera.main;
        
        this.Log(string.Format("screen width: {0}, screen height: {1}, tile height: {2}", Screen.width, Screen.height, tileHeight));

        // Set size
        // cam.orthographicSize = Screen.width / (((Screen.width / Screen.height) * 2) * (Screen.width / tileHeight));
        float offset = 0f;
        cam.orthographicSize = (sizeInUnits.y / 2) + (sizeInUnits.x / 5) + offset;

        // Set Position
        cam.transform.SetPositionAndRotation(new Vector3((sizeInUnits.x / 2) - 0.5f, ((sizeInUnits.y / 2) -0.5f) * -1, cam.transform.position.z), cam.transform.rotation);
    }
}