using UnityEngine;
using RTS.World;

namespace RTS.Resources
{
    /// <summary>
    /// Manages resource placement and availability on the map
    /// </summary>
    public class ResourceManager : MonoBehaviour
    {
        [System.Serializable]
        public class ResourceNode
        {
            public Vector3 position;
            public ResourceType resourceType;
            public float amount;
            public float maxAmount;
        }

        public enum ResourceType
        {
            Minerals,
            Gas
        }

        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private float mineralsPerNode = 500f;
        [SerializeField] private float gasPerNode = 300f;
        [SerializeField] private float resourceDensity = 0.15f; // Percentage of map that has resources

        private ResourceNode[] resourceNodes;
        private float totalMinerals = 0f;
        private float totalGas = 0f;

        private void Start()
        {
            if (mapGenerator == null)
                mapGenerator = GetComponent<MapGenerator>();

            GenerateResources();
        }

        /// <summary>
        /// Generate resource nodes on the map
        /// </summary>
        public void GenerateResources()
        {
            if (mapGenerator == null)
                return;

            int mapWidth = mapGenerator.MapWidth;
            int mapHeight = mapGenerator.MapHeight;
            float tileSize = mapGenerator.TileSize;
            TerrainType[,] terrainMap = mapGenerator.TerrainMap;

            // Count resource tiles
            int resourceTileCount = 0;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    TerrainConfig config = TerrainManager.GetTerrainConfig(terrainMap[x, y]);
                    if (config.hasResources && Random.value < resourceDensity)
                    {
                        resourceTileCount++;
                    }
                }
            }

            // Create resource nodes
            resourceNodes = new ResourceNode[resourceTileCount];
            int nodeIndex = 0;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    TerrainConfig config = TerrainManager.GetTerrainConfig(terrainMap[x, y]);
                    
                    if (config.hasResources && Random.value < resourceDensity)
                    {
                        ResourceNode node = new ResourceNode();
                        node.position = new Vector3(
                            (x + 0.5f) * tileSize,
                            0.1f,
                            (y + 0.5f) * tileSize
                        );

                        // Randomly assign mineral or gas
                        if (Random.value > 0.5f)
                        {
                            node.resourceType = ResourceType.Minerals;
                            node.maxAmount = mineralsPerNode;
                            totalMinerals += mineralsPerNode;
                        }
                        else
                        {
                            node.resourceType = ResourceType.Gas;
                            node.maxAmount = gasPerNode;
                            totalGas += gasPerNode;
                        }

                        node.amount = node.maxAmount;
                        resourceNodes[nodeIndex] = node;
                        nodeIndex++;
                    }
                }
            }

            Debug.Log($"Generated {resourceNodes.Length} resource nodes. Minerals: {totalMinerals}, Gas: {totalGas}");
        }

        /// <summary>
        /// Get resource nodes in an area
        /// </summary>
        public ResourceNode[] GetResourcesInArea(Vector3 center, float radius)
        {
            System.Collections.Generic.List<ResourceNode> nodesInArea = new System.Collections.Generic.List<ResourceNode>();

            foreach (ResourceNode node in resourceNodes)
            {
                float distance = Vector3.Distance(center, node.position);
                if (distance <= radius && node.amount > 0)
                {
                    nodesInArea.Add(node);
                }
            }

            return nodesInArea.ToArray();
        }

        /// <summary>
        /// Harvest resources from a node
        /// </summary>
        public float HarvestResource(ResourceNode node, float amount)
        {
            if (node == null)
                return 0f;

            float harvested = Mathf.Min(amount, node.amount);
            node.amount -= harvested;

            if (node.resourceType == ResourceType.Minerals)
                totalMinerals -= harvested;
            else
                totalGas -= harvested;

            return harvested;
        }

        /// <summary>
        /// Visualize resource nodes
        /// </summary>
        public void VisualizeResources()
        {
            if (resourceNodes == null)
                return;

            foreach (ResourceNode node in resourceNodes)
            {
                Color color = node.resourceType == ResourceType.Minerals ? Color.cyan : Color.yellow;
                Debug.DrawLine(node.position, node.position + Vector3.up * 2f, color, 100f);
            }
        }

        public ResourceNode[] ResourceNodes => resourceNodes;
        public float TotalMinerals => totalMinerals;
        public float TotalGas => totalGas;
    }
}
