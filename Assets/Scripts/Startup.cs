using UnityEngine;

public class Startup {
	// Main startup method. Called on startup due to [InitializeOnLoad]
	[RuntimeInitializeOnLoadMethod]
	static void Start() {
		WorldGenerator worldGen = new WorldGenerator();
		worldGen.Start();
	}
}
