using UnityEngine;

namespace RTS.World
{
    /// <summary>
    /// Defines terrain types and their properties
    /// </summary>
    public enum TerrainType
    {
        Ocean,
        River,
        Plains,
        Desert,
        Forest_Birch,
        Forest_Spruce,
        Forest_Oak,
        Hills,
        Rocky_Mountains,
        Snowy_Peaks
    }

    /// <summary>
    /// Configuration for a terrain type
    /// </summary>
    [System.Serializable]
    public class TerrainConfig
    {
        public TerrainType type;
        public Color color;
        public Material material;
        public float heightMin = 0f;
        public float heightMax = 0.5f;
        public float moisture = 0.5f;
        public float temperature = 0.5f;
        public bool isWalkable = true;
        public bool hasResources = false;
        public string displayName;

        public TerrainConfig(TerrainType terrainType, string name, Color terrainColor)
        {
            type = terrainType;
            displayName = name;
            color = terrainColor;
        }
    }

    /// <summary>
    /// Manages terrain configuration and lookup
    /// </summary>
    public static class TerrainManager
    {
        private static TerrainConfig[] terrainConfigs;

        public static void Initialize()
        {
            terrainConfigs = new TerrainConfig[]
            {
                new TerrainConfig(TerrainType.Ocean, "Ocean", new Color(0.2f, 0.4f, 0.8f))
                {
                    heightMin = -1f,
                    heightMax = 0f,
                    isWalkable = false,
                    moisture = 1f,
                    temperature = 0.3f
                },

                new TerrainConfig(TerrainType.River, "River", new Color(0.3f, 0.6f, 0.9f))
                {
                    heightMin = -0.1f,
                    heightMax = 0.1f,
                    isWalkable = false,
                    moisture = 0.95f,
                    temperature = 0.4f
                },

                new TerrainConfig(TerrainType.Plains, "Plains", new Color(0.5f, 0.8f, 0.3f))
                {
                    heightMin = 0f,
                    heightMax = 0.2f,
                    isWalkable = true,
                    moisture = 0.4f,
                    temperature = 0.6f
                },

                new TerrainConfig(TerrainType.Desert, "Desert", new Color(0.9f, 0.8f, 0.3f))
                {
                    heightMin = 0f,
                    heightMax = 0.35f,
                    isWalkable = true,
                    moisture = 0.1f,
                    temperature = 0.9f
                },

                new TerrainConfig(TerrainType.Forest_Birch, "Birch Forest", new Color(0.4f, 0.7f, 0.3f))
                {
                    heightMin = 0.1f,
                    heightMax = 0.4f,
                    isWalkable = true,
                    moisture = 0.6f,
                    temperature = 0.5f,
                    hasResources = true
                },

                new TerrainConfig(TerrainType.Forest_Spruce, "Spruce Forest", new Color(0.2f, 0.5f, 0.2f))
                {
                    heightMin = 0.1f,
                    heightMax = 0.4f,
                    isWalkable = true,
                    moisture = 0.7f,
                    temperature = 0.3f,
                    hasResources = true
                },

                new TerrainConfig(TerrainType.Forest_Oak, "Oak Forest", new Color(0.35f, 0.6f, 0.25f))
                {
                    heightMin = 0.1f,
                    heightMax = 0.4f,
                    isWalkable = true,
                    moisture = 0.5f,
                    temperature = 0.6f,
                    hasResources = true
                },

                new TerrainConfig(TerrainType.Hills, "Hills", new Color(0.6f, 0.65f, 0.4f))
                {
                    heightMin = 0.3f,
                    heightMax = 0.6f,
                    isWalkable = true,
                    moisture = 0.3f,
                    temperature = 0.5f
                },

                new TerrainConfig(TerrainType.Rocky_Mountains, "Rocky Mountains", new Color(0.5f, 0.5f, 0.5f))
                {
                    heightMin = 0.5f,
                    heightMax = 0.8f,
                    isWalkable = false,
                    moisture = 0.2f,
                    temperature = 0.4f
                },

                new TerrainConfig(TerrainType.Snowy_Peaks, "Snowy Peaks", new Color(0.85f, 0.85f, 0.9f))
                {
                    heightMin = 0.7f,
                    heightMax = 1f,
                    isWalkable = false,
                    moisture = 0.3f,
                    temperature = 0.1f
                }
            };
        }

        public static TerrainConfig GetTerrainConfig(TerrainType type)
        {
            if (terrainConfigs == null)
                Initialize();

            foreach (TerrainConfig config in terrainConfigs)
            {
                if (config.type == type)
                    return config;
            }

            return terrainConfigs[0]; // Fallback to first config
        }

        public static TerrainType GetTerrainTypeByHeight(float height, float moisture, float temperature)
        {
            // Ocean
            if (height < 0f)
                return TerrainType.Ocean;

            // Rivers - narrow bands near water level
            if (height < 0.1f && moisture > 0.8f)
                return TerrainType.River;

            // Snowy Peaks
            if (height > 0.7f)
                return TerrainType.Snowy_Peaks;

            // Rocky Mountains
            if (height > 0.5f)
                return TerrainType.Rocky_Mountains;

            // Hills
            if (height > 0.3f)
                return TerrainType.Hills;

            // Forest selection based on temperature and moisture
            if (height > 0.1f && moisture > 0.4f)
            {
                if (temperature < 0.35f)
                    return TerrainType.Forest_Spruce;
                else if (temperature > 0.65f)
                    return TerrainType.Forest_Oak;
                else
                    return TerrainType.Forest_Birch;
            }

            // Desert
            if (temperature > 0.8f && moisture < 0.3f)
                return TerrainType.Desert;

            // Plains (default)
            return TerrainType.Plains;
        }
    }
}
