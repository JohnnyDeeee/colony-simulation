using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class WorldGenerator {

	public WorldGenerator() {

	}

	// Use this for initialization
	public void Start() {
		Debug.Log("started!");

		World.Init(this.StringToCharArray(ResourcesList.LEVEL_1.text));
	}

	// Converts a level string to a mutli dimensional char array
	private char[,] StringToCharArray(string _string) {
		_string = Regex.Replace(_string, @"\t|\r", ""); // Remove unwanted characters

		// Calculate dimensions
		string[] split = _string.Split('\n');
		int dimensionX = split[0].Length;
		int dimensionY = split.Length;
		Vector2 dimensions = new Vector2(dimensionX, dimensionY);
		
		// Create charArray
		char[,] charArray = new char[dimensions.x, dimensions.y];
		int x = 0;
		int y = 0;
		foreach(char _char in _string) {
			// Sanitize the string to only allow "valid" defintions
			if(!TileDefinitions.IsDefintionValid(_char))
				continue;

			charArray[x, y] = _char;

			if(x < dimensions.x -1)
				x += 1;
			else {
				x = 0;
				if(y < dimensions.y -1)
					y += 1;
				else
					break;
			}			
		}

		return charArray;
	}
}
