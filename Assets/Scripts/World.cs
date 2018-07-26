using UnityEngine;

public static class World {
    private static AI currentSelectedAI;
    private static AI previousSelectedAI;

    public static void SetSelectedAI(AI newAI) {
        if(currentSelectedAI != newAI) {
            previousSelectedAI = currentSelectedAI;
            currentSelectedAI = newAI;
        }
    }

    public static AI GetSelectedAI() {
        return currentSelectedAI;
    }

    public static AI GetPreviousSelectedAI() {
        return previousSelectedAI;
    }

    public static void Init(char[,] level) {
        GameObject gridInstance = GameObject.Instantiate(ResourcesList.GRID, new Vector3(0, 0, 0), Quaternion.identity);
        gridInstance.name = "grid";
        Grid grid = gridInstance.GetComponent<Grid>();

        grid.Init(level);

        SetupCamera(grid.GetSizeInUnits(), grid.GetTileSizeInPixels().x);
    }

    private static void SetupCamera(Vector2 sizeInUnits, int tileHeight) {
        Camera cam = Camera.main;
        
        Debug.Log(string.Format("screen width: {0}, screen height: {1}, tile height: {2}", Screen.width, Screen.height, tileHeight));

        // Set size
        float offset = 0f;
        cam.orthographicSize = (sizeInUnits.y / 2) + (sizeInUnits.x / 5) + offset;

        // Set Position
        cam.transform.SetPositionAndRotation(new Vector3((sizeInUnits.x / 2) - 0.5f, (sizeInUnits.y / 2) -0.5f, cam.transform.position.z), cam.transform.rotation);
    }
}