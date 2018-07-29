using System.Reflection;
using UnityEngine;

public struct TileDefinition {
    public char _char;
    public GameObject prefab;
    public Color color;

    public TileDefinition(char _char, GameObject prefab, Color color) {
        this._char = _char;
        this.prefab = prefab;
        this.color = color;
    }

    public GameObject Instantiate(Vector2 position) {
        GameObject tileInstance = GameObject.Instantiate(this.prefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        tileInstance.name = this.prefab.name;
        tileInstance.GetComponent<SpriteRenderer>().color = this.color;

        return tileInstance;
    }
}

public static class TileDefinitions {
    public static readonly TileDefinition TILE_GROUND = new TileDefinition('-', ResourcesList.TILE_GROUND, new Color(0.525f, 0.4f, 0.03f));
    public static readonly TileDefinition TILE_WALL = new TileDefinition('x', ResourcesList.TILE_WALL, Color.black);
    public static readonly TileDefinition TILE_FOOD = new TileDefinition('f', ResourcesList.TILE_FOOD, new Color(0.03f, 0.827f, 0f)); // Can be placed down with its char, but will be randomly placed aswell

    // Checks if a char is defined in our Tile definitions
    public static bool IsDefintionValid(char definitionToTest) {
        FieldInfo[] fields = typeof(TileDefinitions).GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach(FieldInfo field in fields) {
            if(field.FieldType == typeof(TileDefinition)) { // Definitions are only of type TileDefinition
                TileDefinition definition = (TileDefinition)field.GetValue(null);
                if(definition._char == definitionToTest)
                    return true;
            }
        }

        return false;
    }
}