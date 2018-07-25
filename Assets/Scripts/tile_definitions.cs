using System.Reflection;
using UnityEngine;

public struct TileDefinition {
    public char _char;
    public GameObject prefab;

    public TileDefinition(char _char, GameObject prefab) {
        this._char = _char;
        this.prefab = prefab;
    }
}

public static class TileDefinitions {
    public static readonly TileDefinition TILE_GROUND = new TileDefinition('-', ResourcesList.TILE_GROUND);
    public static readonly TileDefinition TILE_WALL = new TileDefinition('x', ResourcesList.TILE_WALL);

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