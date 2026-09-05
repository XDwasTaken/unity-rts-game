using UnityEngine;
using RTS.Core;
using RTS.Camera;
using RTS.UI;
using RTS.World;

namespace RTS.Setup
{
    /// <summary>
    /// Automatically creates and configures the main game scene
    /// </summary>
    public class SceneSetup : MonoBehaviour
    {
        [MenuItem("RTS/Setup Game Scene")]
        public static void SetupGameScene()
        {
            Debug.Log("Setting up game scene...");

            // Create World
            GameObject worldObj = new GameObject("World");
            MapGenerator mapGen = worldObj.AddComponent<MapGenerator>();
            ResourceManager resManager = worldObj.AddComponent<ResourceManager>();
            WorldSpawner spawner = worldObj.AddComponent<WorldSpawner>();
            
            Debug.Log("✓ World setup complete");

            // Create Camera
            GameObject cameraObj = new GameObject("Main Camera");
            Camera camera = cameraObj.AddComponent<Camera>();
            cameraObj.AddComponent<AudioListener>();
            RTSCamera rtsCamera = cameraObj.AddComponent<RTSCamera>();
            cameraObj.tag = "MainCamera";
            cameraObj.transform.position = new Vector3(50, 50, 50);
            cameraObj.transform.rotation = Quaternion.Euler(45, 45, 0);
            
            Debug.Log("✓ Camera setup complete");

            // Create GameManager
            GameObject gameManagerObj = new GameObject("GameManager");
            gameManagerObj.AddComponent<GameManager>();
            
            Debug.Log("✓ GameManager setup complete");

            // Create SelectionManager
            GameObject selectionObj = new GameObject("SelectionManager");
            selectionObj.AddComponent<SelectionManager>();
            
            Debug.Log("✓ SelectionManager setup complete");

            Debug.Log("═══════════════════════════════════════");
            Debug.Log("Game scene setup complete!");
            Debug.Log("═══════════════════════════════════════");
            Debug.Log("Press Play to generate the map!");
        }
    }
}
