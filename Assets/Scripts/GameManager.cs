using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tron
{
    public class GameManager : MonoBehaviour
    {

        #region Singleton
        public static GameManager Instance { get; private set; }

        public void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Multiple instances of " + name + " detected!");
            }
            Instance = this;
        }
        #endregion

        public Color[] colors;
        public int maxPlayers;
        public SpawnPoint[] spawnPoints;
        public PlayerManager playerPrefab;
        public List<PlayerManager> players;

        public void AddPlayer()
        {
            if (playerPrefab == null) throw new MissingReferenceException("The playerPrefab field of GameManager is null. Please assign in the editor.");
            var id = players.Count;
            var spawnPoint = spawnPoints[id % spawnPoints.Length];
            PlayerManager player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            players.Add(player);

            player.id = id;
            var color = colors[id % colors.Length];
            player.trail.trailMat.color = color;

            player.gameObject.layer = 10 + id;
            player.virtualCamera.gameObject.layer = 10 + id;
            player.cameraBrain.cullingMask = player.cameraBrain.cullingMask | (1 << player.gameObject.layer);

            player.gameObject.SetActive(true);
        }

        // Start is called before the first frame update
        private void Start()
        {
            spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
            AddPlayer();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}