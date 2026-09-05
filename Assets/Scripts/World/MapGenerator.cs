using UnityEngine;

namespace RTS.World
{
    /// <summary>
    /// Perlin noise-based terrain generator for procedural map generation
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        [Header("Map Settings")]
        [SerializeField] private int mapWidth = 100;
        [SerializeField] private int mapHeight = 100;
        [SerializeField] private float tileSize = 1f;

        [Header("Noise Settings")]
        [SerializeField] private float heightOffset = 0.5f;
        [SerializeField] private float heightFrequency = 0.1f;
        [SerializeField] private float moistureFrequency = 0.05f;
        [SerializeField] private float temperatureFrequency = 0.08f;

        [Header("Noise Layers")]
        [SerializeField] private int heightOctaves = 4;
        [SerializeField] private float heightPersistence = 0.5f;
        [SerializeField] private float heightLacunarity = 2f;

        [SerializeField] private int moistureOctaves = 3;
        [SerializeField] private float moisturePersistence = 0.6f;
        [SerializeField] private float moistureLacunarity = 2f;

        [Header("Map Generation")]
        [SerializeField] private long randomSeed = 12345;
        [SerializeField] private bool useRandomSeed = false;
        [SerializeField] private bool generateOnStart = true;

        private float[,] heightMap;
        private float[,] moistureMap;
        private float[,] temperatureMap;
        private TerrainType[,] terrainMap;

        private void Start()
        {
            if (generateOnStart)
            {
                GenerateMap();
            }
        }

        /// <summary>
        /// Generate a complete procedural map
        /// </summary>
        public void GenerateMap()
        {
            TerrainManager.Initialize();

            // Set seed
            if (useRandomSeed)
                randomSeed = System.DateTime.Now.Millisecond;
            Random.InitState((int)randomSeed);

            // Generate noise maps
            heightMap = GenerateNoiseMap(mapWidth, mapHeight, heightFrequency, heightOctaves, heightPersistence, heightLacunarity);
            moistureMap = GenerateNoiseMap(mapWidth, mapHeight, moistureFrequency, moistureOctaves, moisturePersistence, moistureLacunarity);
            temperatureMap = GenerateNoiseMap(mapWidth, mapHeight, temperatureFrequency, moistureOctaves, moisturePersistence, moistureLacunarity);

            // Generate terrain map based on noise
            GenerateTerrainMap();

            // Visualize the map
            VisualizeMap();

            Debug.Log($"Map generated: {mapWidth}x{mapHeight} with seed {randomSeed}");
        }

        /// <summary>
        /// Generate a Perlin noise map with multiple octaves
        /// </summary>
        private float[,] GenerateNoiseMap(int width, int height, float frequency, int octaves, float persistence, float lacunarity)
        {
            float[,] noiseMap = new float[width, height];
            float amplitude = 1f;
            float maxValue = 0f;

            for (int oct = 0; oct < octaves; oct++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        float sampleX = (x * frequency) / width;
                        float sampleY = (y * frequency) / height;
                        noiseMap[x, y] += Mathf.PerlinNoise(sampleX + (oct * 100), sampleY + (oct * 100)) * amplitude;
                    }
                }

                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= lacunarity;
            }

            // Normalize the map to 0-1
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    noiseMap[x, y] /= maxValue;
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y]);
                }
            }

            return noiseMap;
        }

        /// <summary>
        /// Generate terrain types based on noise maps
        /// </summary>
        private void GenerateTerrainMap()
        {
            terrainMap = new TerrainType[mapWidth, mapHeight];

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float height = Mathf.Lerp(-heightOffset, 1 - heightOffset, heightMap[x, y]);
                    float moisture = moistureMap[x, y];
                    float temperature = temperatureMap[x, y];

                    terrainMap[x, y] = TerrainManager.GetTerrainTypeByHeight(height, moisture, temperature);
                }
            }
        }

        /// <summary>
        /// Visualize the generated map with colored quads
        /// </summary>
        private void VisualizeMap()
        {
            // Clear existing terrain objects
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Create terrain tiles
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    TerrainType terrainType = terrainMap[x, y];
                    TerrainConfig config = TerrainManager.GetTerrainConfig(terrainType);

                    // Create a quad for this tile
                    GameObject tile = CreateTerrainTile(x, y, config);
                    tile.transform.parent = transform;
                }
            }

            // Add collider to entire map for raycasting
            BoxCollider mapCollider = gameObject.AddComponent<BoxCollider>();
            mapCollider.size = new Vector3(mapWidth * tileSize, 0.1f, mapHeight * tileSize);
            mapCollider.center = new Vector3((mapWidth * tileSize) / 2, 0, (mapHeight * tileSize) / 2);
        }

        /// <summary>
        /// Create a visual terrain tile
        /// </summary>
        private GameObject CreateTerrainTile(int x, int y, TerrainConfig config)
        {
            GameObject tile = new GameObject($"Tile_{x}_{y}");
            tile.transform.position = new Vector3(x * tileSize, 0, y * tileSize);

            // Add sprite/mesh renderer for visualization
            MeshFilter meshFilter = tile.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = tile.AddComponent<MeshRenderer>();

            // Create simple quad mesh
            Mesh quadMesh = CreateQuadMesh(tileSize);
            meshFilter.mesh = quadMesh;

            // Create and assign material
            Material tileMaterial = new Material(Shader.Find("Standard"));
            tileMaterial.color = config.color;
            meshRenderer.material = tileMaterial;

            // Add collider
            BoxCollider collider = tile.AddComponent<BoxCollider>();
            collider.size = new Vector3(tileSize, 0.1f, tileSize);

            // Tag terrain
            tile.tag = "Terrain";
            tile.layer = LayerMask.NameToLayer("Default");

            return tile;
        }

        /// <summary>
        /// Create a simple quad mesh for a tile
        /// </summary>
        private Mesh CreateQuadMesh(float size)
        {
            Mesh mesh = new Mesh();
            float half = size / 2f;

            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-half, 0, -half),
                new Vector3(half, 0, -half),
                new Vector3(half, 0, half),
                new Vector3(-half, 0, half)
            };

            int[] triangles = new int[]
            {
                0, 2, 1,
                0, 3, 2
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }

        /// <summary>
        /// Get the terrain type at world position
        /// </summary>
        public TerrainType GetTerrainAtPosition(Vector3 worldPos)
        {
            int x = Mathf.FloorToInt(worldPos.x / tileSize);
            int z = Mathf.FloorToInt(worldPos.z / tileSize);

            if (x >= 0 && x < mapWidth && z >= 0 && z < mapHeight)
            {
                return terrainMap[x, z];
            }

            return TerrainType.Ocean;
        }

        /// <summary>
        /// Check if a position is walkable
        /// </summary>
        public bool IsWalkable(Vector3 worldPos)
        {
            TerrainType terrain = GetTerrainAtPosition(worldPos);
            TerrainConfig config = TerrainManager.GetTerrainConfig(terrain);
            return config.isWalkable;
        }

        // Getters
        public int MapWidth => mapWidth;
        public int MapHeight => mapHeight;
        public float TileSize => tileSize;
        public Vector3 MapSize => new Vector3(mapWidth * tileSize, 0, mapHeight * tileSize);
        public TerrainType[,] TerrainMap => terrainMap;
        public float[,] HeightMap => heightMap;
        public float[,] MoistureMap => moistureMap;
        public float[,] TemperatureMap => temperatureMap;

#if UNITY_EDITOR
        /// <summary>
        /// Visualize the noise maps in the editor
        /// </summary>
        public Texture2D GetHeightMapTexture()
        {
            if (heightMap == null) return null;

            Texture2D texture = new Texture2D(mapWidth, mapHeight, TextureFormat.RGB24, false);
            Color[] pixels = new Color[mapWidth * mapHeight];

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float height = heightMap[x, y];
                    pixels[y * mapWidth + x] = new Color(height, height, height);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        public Texture2D GetTerrainMapTexture()
        {
            if (terrainMap == null) return null;

            Texture2D texture = new Texture2D(mapWidth, mapHeight, TextureFormat.RGB24, false);
            Color[] pixels = new Color[mapWidth * mapHeight];

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    TerrainConfig config = TerrainManager.GetTerrainConfig(terrainMap[x, y]);
                    pixels[y * mapWidth + x] = config.color;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
#endif
    }
}
