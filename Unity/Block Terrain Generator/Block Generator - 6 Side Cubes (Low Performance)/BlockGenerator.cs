/* 
 * GNU General Public License v2
 * This script is licensed under the GNU GPL v2
 * You are free to modify and distribute it under the same license
 * Credit: Oneak (https://realmmadness.com/oneak)
 */

using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [Header("Block Settings")]
    public GameObject blockPrefab;
    public int width = 16;
    public int height = 16;
    public int depth = 16;
    public float noiseScale = 0.1f;

    [Header("Decoration Prefabs")]
    public GameObject treePrefab;
    public GameObject rockPrefab;

    [Header("Player")]
    public Transform playerPrefab;
    private Transform currentPlayer;

    // Noise offsets for random terrain
    private float noiseOffsetX = 0f;
    private float noiseOffsetZ = 0f;

    // Block counter
    private int blockCount = 0;

    void Start()
    {
        RandomizeOffsets();
        GenerateChunk();
        SpawnPlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key pressed. Regenerating chunk...");
            RegenerateChunk();
        }
    }

    void RegenerateChunk()
    {
        Debug.Log("RegenerateChunk() called. Destroying old blocks...");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        RandomizeOffsets();
        Debug.Log($"Noise offset set to ({noiseOffsetX:F2}, {noiseOffsetZ:F2})");
        GenerateChunk();
        SpawnPlayer();
        Debug.Log("Chunk regenerated and player respawned.");
    }

    void RandomizeOffsets()
    {
        noiseOffsetX = Random.Range(0f, 10000f);
        noiseOffsetZ = Random.Range(0f, 10000f);
    }

    void GenerateChunk()
    {
        blockCount = 0; // Reset counter
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float yMax = Mathf.FloorToInt(Mathf.PerlinNoise(
                    (x * noiseScale) + noiseOffsetX,
                    (z * noiseScale) + noiseOffsetZ
                ) * height);

                for (int y = 0; y < yMax; y++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    Instantiate(blockPrefab, position, Quaternion.identity, this.transform);
                    blockCount++;
                }

                // Place trees and rocks on the topmost block
                if (yMax > 0)
                {
                    Vector3 topBlockPos = new Vector3(x, yMax - 1, z);
                    float rand = Random.value;
                    if (rand < 0.05f && treePrefab != null)
                    {
                        Instantiate(treePrefab, topBlockPos + Vector3.up, Quaternion.identity, this.transform);
                    }
                    else if (rand < 0.10f && rockPrefab != null)
                    {
                        Instantiate(rockPrefab, topBlockPos + Vector3.up * 0.5f, Quaternion.identity, this.transform);
                    }
                }
            }
        }
        Debug.Log("Chunk generation complete. Blocks created: " + blockCount);
    }

    void SpawnPlayer()
    {
        // Place player at center, above the terrain
        int spawnX = width / 2;
        int spawnZ = depth / 2;
        float yMax = Mathf.FloorToInt(Mathf.PerlinNoise(
            (spawnX * noiseScale) + noiseOffsetX,
            (spawnZ * noiseScale) + noiseOffsetZ
        ) * height);

        Vector3 spawnPosition = new Vector3(spawnX, yMax + 1, spawnZ);

        if (currentPlayer)
            Destroy(currentPlayer.gameObject);

        if (playerPrefab)
            currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("Player spawned at " + spawnPosition);
    }
}