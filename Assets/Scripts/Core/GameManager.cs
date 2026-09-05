using UnityEngine;

namespace RTS.Core
{
    /// <summary>
    /// Main game manager - handles game state, initialization, and overall game flow
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private float gameSpeed = 1f;
        private bool isPaused = false;

        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeGame();
        }

        private void InitializeGame()
        {
            Debug.Log("Initializing RTS Game...");
            // Initialize game systems here
            // - Load level
            // - Spawn units
            // - Setup UI
            // - Initialize AI
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : gameSpeed;
            Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
        }

        public void SetGameSpeed(float speed)
        {
            gameSpeed = Mathf.Clamp(speed, 0.1f, 3f);
            if (!isPaused)
            {
                Time.timeScale = gameSpeed;
            }
        }

        public bool IsPaused => isPaused;
        public float GameSpeed => gameSpeed;

        public void GameOver(bool playerWon)
        {
            Time.timeScale = 0f;
            Debug.Log(playerWon ? "Victory!" : "Defeat!");
            // Show game over UI
        }
    }
}
