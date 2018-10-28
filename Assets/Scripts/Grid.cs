using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Grid : MonoBehaviour {
    [SerializeField] private Vector2 levelSize;
    [SerializeField] private float xOrg;
    [SerializeField] private float yOrg;
    [SerializeField] private float noiseScale;
    [SerializeField] private float noiseSmoothness;

    private char[,] level;
    private Tile[,] tiles;
    private float foodThreshold;
    private GameObject aiParent;
    private bool spawnLock;
    private List<float> fitnessPerPopulation = new List<float>();

    public void Update() {
        // Update world age every second
        if(Time.time >= World.nextAgeUpdate) {
            World.age += 1;
            World.nextAgeUpdate += 1;
            spawnLock = false;
        }

        // Start creating new population
        if(World.age > 0 && (World.age % World.maxPopulationAge == 0) && !spawnLock) {
            spawnLock = true;

            // Cleanup dead ants
            List<AI> copyAis = new List<AI>(World.ais);
            foreach (AIAnt ant in copyAis){ // Loop over copy
                if(!ant.dead)
                    continue;

                World.ais.Remove(ant); // Remove from original
                GameObject.Destroy(ant.gameObject);
            }

            // Create new population
            List<AI> candidates = World.ais;
            AI[] parents = GeneticAlgorithm.GetParents(candidates);
            this.CreatePopulation(World.nextPopulationAmount, parents);

            // Save fitness from winners (parents)
            foreach (AI parent in parents) {
                this.fitnessPerPopulation.Add(parent.fitness);
            }
            string logString1 = "Parents(1): ";
            string logString2 = "Parents(2): ";
            for (int i = 0; i < this.fitnessPerPopulation.Count; i++){
                if(i % 2 == 0)
                    logString2 += this.fitnessPerPopulation[i] + ",";
                else
                    logString1 += this.fitnessPerPopulation[i] + ",";
            }
            Debug.Log("Fitness winners (every 2 parents per population): ");
            Debug.Log(logString1);
            Debug.Log(logString2);
            
            // DEBUG
            // winner.selectedColor = Color.green;
            // winner.MarkSelected(); // Select the winner
            // World.SetSelectedAI(winner);
            // Debug.Log(string.Format("f:{0}, nf:{1}, af:{2}", winner.fitness, winner.normFitness, winner.accNormFitness));
            // Debug.Log(String.Join(",", winner.GetGenome().OfType<bool>().Select(x => x?"1":"0").ToArray()));
            // BitArray newGenome = GeneticAlgorithm.Crossover(winner.GetGenome());
            // Debug.Log(String.Join(",", newGenome.OfType<bool>().Select(x => x?"1":"0").ToArray()));
            // newGenome = GeneticAlgorithm.Mutation(newGenome);
            // Debug.Log(String.Join(",", newGenome.OfType<bool>().Select(x => x?"1":"0").ToArray()));
            // UnityEditor.EditorApplication.isPaused = true;
        }

        // Mark ai
        if(World.GetSelectedAI())
            World.GetSelectedAI().MarkSelected();
        if(World.GetPreviousSelectedAI())
            World.GetPreviousSelectedAI().MarkUnselected();
    }

    public void Init(char[,] level) {
        this.level = level;

        this.noiseScale = 10f; // Number of perlinNoise cycles (Lower = less patches, Higher = more patches)
        this.foodThreshold = 0.65f; // Lower = bigger food patches, Higher = smaller food patches (0f - 1f)
        this.noiseSmoothness = 8f; // Lower = less smooth food transitions, Higher = smoother transitions (0f - ~100f)

        // GetLength 0-1 because we only deal with 2 dimensions in this 2D simulation
        this.levelSize = new Vector2(this.level.GetLength(0), this.level.GetLength(1));

        // Create noise map for spawning food patches
        Color[] noisePixels = new Color[(int)this.levelSize.x * (int)this.levelSize.y];
        float _y = 0.0f;
        while(_y < this.levelSize.y) {
            float _x = 0.0f;
            while(_x < this.levelSize.x) {
                float xCoord = this.xOrg + _x / this.levelSize.y * this.noiseScale;
                float yCoord = this.yOrg + _y / this.levelSize.x * this.noiseScale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                sample = Mathf.Round(sample * this.noiseSmoothness) / this.noiseSmoothness; // Round of the sample to make it more/less smoother
                noisePixels[(int)_y * (int)this.levelSize.y + (int)_x] = new Color(sample, sample, sample);
                _x++;
            }
            _y++;
        }

        // Create the tiles
        this.tiles = new Tile[(int)this.levelSize.x, (int)this.levelSize.y];
        for(int x = 0; x < this.levelSize.x; x++) {
            for(int y = 0; y < this.levelSize.y; y++) {
                TileDefinition tileDefinition;

                if(this.level[x, y] == TileDefinitions.TILE_WALL._char) { // Wall tile
                    tileDefinition = TileDefinitions.TILE_WALL;
                } else if(this.level[x, y] == TileDefinitions.TILE_GROUND._char) { // Food / Ground
                    Color color = noisePixels[x + (y * (int)this.levelSize.x)];
                    if(color.grayscale >= this.foodThreshold) {
                        tileDefinition = TileDefinitions.TILE_FOOD;
                        tileDefinition.prefab.GetComponent<TileFood>().foodAmount = color.grayscale;
                    } else
                        tileDefinition = TileDefinitions.TILE_GROUND;
                } else // Unknown tile type
                    throw new Exception(String.Format("Tile type '{0}' not found", this.level[x, y]));

                GameObject tileInstance = tileDefinition.Instantiate(new Vector2(x, y), this.gameObject);

                Tile tile = tileInstance.GetComponent<Tile>();
                this.tiles[x, y] = tile;
            }
        }
        
        // Create AI
        this.aiParent = new GameObject("ai's"); 
        this.CreatePopulation(World.nextPopulationAmount);
    }

    private void CreatePopulation(int amount, AI[] parents = null) {
        World.generation += 1;

        for(int i = 0; i < amount; i++) {
            float randX = UnityEngine.Random.Range(0f, this.levelSize.x);
            float randY = UnityEngine.Random.Range(0f, this.levelSize.y);
            Tile randTile = this.GetTile((int)randX, (int)randY);

            if(!randTile.CanSpawnAi()){
                i--;
                continue;
            }

            GameObject aiInstance = GameObject.Instantiate(ResourcesList.AI_ANT, randTile.transform.position, Quaternion.identity);
            aiInstance.name = "ai_ant";
            aiInstance.transform.parent = this.aiParent.transform;

            AI ai = aiInstance.GetComponent<AI>();

            // If parent, do crossover/mutation
            if(parents != null && parents.Length == 2) {
                AI parent1 = parents[0];
                AI parent2 = parents[1];
                BitArray newGenome = GeneticAlgorithm.Crossover(parent1.GetGenome(), parent2.GetGenome());
                newGenome = GeneticAlgorithm.Mutation(newGenome);
                ai.SetGenome(newGenome);
            }

            World.ais.Add(ai);
        }
    }

    public Tile GetTile(int posX, int posY) { return this.tiles[posX, posY]; }

    public Vector2 GetTilePosition(Tile tile) {
        for (int x = this.tiles.GetLowerBound(0); x <= this.tiles.GetUpperBound(0); x++)
            for (int y = this.tiles.GetLowerBound(1); y <= this.tiles.GetUpperBound(1); y++)
                if(this.tiles[x, y] == tile)
                    return new Vector2(x, y);
        return Vector2.negativeInfinity;
    }

    public Tile[,] GetTiles() { return this.tiles; }

    public Vector2 GetSizeInTiles() { return this.levelSize; }

    public Vector2 GetSizeInPixels() {
        Vector2 sizeInTiles = this.GetSizeInTiles();
        Vector2 sizeInPixels = new Vector2(sizeInTiles.x * (this.GetTileSizeInPixels().x), sizeInTiles.y * (this.GetTileSizeInPixels().y));
        return sizeInPixels;
    }

    public Vector2 GetSizeInUnits() {
        Vector2 sizeInPixels = this.GetSizeInPixels();
        int pixelsPerUnit = (int)this.tiles[0,0].GetSprite().pixelsPerUnit;
        Vector2 sizeInUnits = new Vector2(sizeInPixels.x / pixelsPerUnit, sizeInPixels.y / pixelsPerUnit);
        return sizeInUnits;
    }

    public Vector2 GetTileSizeInPixels() {
        Sprite sprite = this.tiles[0,0].GetSprite();
        return new Vector2(sprite.rect.width, sprite.rect.height);
    }
}