using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Startup {
	// Main startup method. Called on startup due to [InitializeOnLoad]
	static Startup() {
		WorldGenerator worldGen = new WorldGenerator("worldGen");
		worldGen.Start();
	}
}
