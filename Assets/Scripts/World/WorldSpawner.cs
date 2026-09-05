using UnityEngine;

namespace RTS.World
{
    /// <summary>
    /// World spawner that initializes map generation and core systems
    /// </summary>
    public class WorldSpawner : MonoBehaviour
    {
        [Header("World Settings")]
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private ResourceManager resourceManager;

        private void Start()
        {
            InitializeWorld();
        }

        /// <summary>
        /// Initialize the entire game world
        /// </summary>
        public void InitializeWorld()
        {
            Debug.Log("Initializing World...");

            // Generate terrain
            if (mapGenerator == null)
                mapGenerator = GetComponent<MapGenerator>();

            if (mapGenerator != null)
            {
                mapGenerator.GenerateMap();
                Debug.Log("Map generated successfully");
            }

            // Generate resources
            if (resourceManager == null)
                resourceManager = GetComponent<ResourceManager>();

            if (resourceManager != null)
            {
                resourceManager.GenerateResources();
                resourceManager.VisualizeResources();
                Debug.Log($"Resources generated - Minerals: {resourceManager.TotalMinerals}, Gas: {resourceManager.TotalGas}");
            }

            // Setup camera bounds
            if (mapGenerator != null)
            {
                RTSCamera camera = FindObjectOfType<RTSCamera>();
                if (camera != null)
                {
                    Vector3 mapSize = mapGenerator.MapSize;
                    camera.SetMapBounds(Vector2.zero, new Vector2(mapSize.x, mapSize.z));
                }
            }

            Debug.Log("World initialized successfully!");
        }

        /// <summary>
        /// Regenerate the world with new settings
        /// </summary>
        public void RegenerateWorld()
        {
            Debug.Log("Regenerating World...");
            InitializeWorld();
        }
    }
}
