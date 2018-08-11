using System;
using System.Collections.Generic;
using UnityEngine;

public static class World {
    private static AI currentSelectedAI;
    private static AI previousSelectedAI;
    private static Grid grid;
    private static UI ui;
    private static int seed;
    public static List<AI> ais = new List<AI>();
    public static int age;
    public static int generation;

    // Settings
    public static int nextAgeUpdate = 1; // How many seconds one year in the world takes
    public static int maxPopulationAge = 50; // After every X ages a new population will be born
    public static readonly float mutationProbability = 1f; //0.03f;

    public static void Init(char[,] level) {
        seed = (int)DateTime.Now.Ticks;
        UnityEngine.Random.InitState(seed);

        // Create grid object
        GameObject gridInstance = GameObject.Instantiate(ResourcesList.GRID, new Vector3(0, 0, 0), Quaternion.identity);
        gridInstance.name = "grid";
        grid = gridInstance.GetComponent<Grid>();
        grid.Init(level);

        SetupCamera(grid.GetSizeInUnits(), grid.GetTileSizeInPixels().x);

        // Create UI
        GameObject uiInstance = GameObject.Instantiate(ResourcesList.UI, new Vector3(0, 0, 0), Quaternion.identity);
        uiInstance.name = "ui";
        ui = uiInstance.GetComponent<UI>();
    }

    private static void SetupCamera(Vector2 sizeInUnits, float tileHeight) {
        Camera cam = Camera.main;
        
        Debug.Log(string.Format("screen width: {0}, screen height: {1}, tile height: {2}", Screen.width, Screen.height, tileHeight));

        // Set size
        float offset = 0f;
        cam.orthographicSize = (sizeInUnits.y / 2) + (sizeInUnits.x / 5) + offset;

        // Set Position
        cam.transform.SetPositionAndRotation(new Vector3((sizeInUnits.x / 2) - 0.5f, (sizeInUnits.y / 2) -0.5f, cam.transform.position.z), cam.transform.rotation);
    }

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

    public static Grid GetGrid() {
        return grid;
    }

    public static UI GetUI() {
        return ui;
    }

    public static int GetSeed() {
        return seed;
    }
}