/* 
 * GNU General Public License v2
 * This script is licensed under the GNU GPL v2
 * You are free to modify and distribute it under the same license
 * Credit: Oneak (https://realmmadness.com/oneak)
 */

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class BlockGeneratorMesh : MonoBehaviour
{
    [Header("Chunk Settings")]
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

    [Header("Visuals")]
    public Material blockMaterial;
    public Camera playerCamera;

    private bool[,,] blockData;
    private float noiseOffsetX;
    private float noiseOffsetZ;
    private int blockCount = 0;

    void Start()
    {
        RandomizeOffsets();
        GenerateBlockData();
        BuildMesh();

        if (blockMaterial)
            GetComponent<MeshRenderer>().material = blockMaterial;

        if (playerCamera == null)
            playerCamera = Camera.main;

        SpawnPlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Regenerating chunk...");
            RandomizeOffsets();
            GenerateBlockData();
            BuildMesh();
            SpawnPlayer();
        }
    }

    void RandomizeOffsets()
    {
        noiseOffsetX = Random.Range(0f, 10000f);
        noiseOffsetZ = Random.Range(0f, 10000f);
    }

    void GenerateBlockData()
    {
        // Destroy previously instantiated decorations (trees, rocks)
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        blockData = new bool[width, height, depth];

        // Reset the counter
        blockCount = 0;

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
                    blockData[x, y, z] = true;
                    blockCount++;
                }

                if (yMax > 0)
                {
                    Vector3 topCenter = new Vector3(x + 0.5f, yMax, z + 0.5f);
                    float rand = Random.value;
                    if (rand < 0.05f && treePrefab)
                        Instantiate(treePrefab, topCenter, Quaternion.identity, transform);
                    else if (rand < 0.1f && rockPrefab)
                        Instantiate(rockPrefab, topCenter, Quaternion.identity, transform);
                }
            }
        }

        Debug.Log("Chunk generation complete. Blocks created: " + blockCount);
    }

    void BuildMesh()
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        int faceIndex = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (!blockData[x, y, z]) continue;

                    for (int f = 0; f < 6; f++)
                    {
                        Vector3Int dir = faceChecks[f];
                        int nx = x + dir.x;
                        int ny = y + dir.y;
                        int nz = z + dir.z;

                        if (!IsSolid(nx, ny, nz))
                        {
                            AddFace(x, y, z, f, verts, tris, uvs, faceIndex);
                            faceIndex++;
                        }
                    }
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        MeshCollider col = GetComponent<MeshCollider>();
        col.sharedMesh = null;
        col.sharedMesh = mesh;
    }

    bool IsSolid(int x, int y, int z)
    {
        if (x < 0 || x >= width || y < 0 || y >= height || z < 0 || z >= depth)
            return false;
        return blockData[x, y, z];
    }

    void AddFace(int x, int y, int z, int face, List<Vector3> verts, List<int> tris, List<Vector2> uvs, int faceIndex)
    {
        Vector3[] faceVerts = CubeMeshData.faceVertices[face];
        foreach (Vector3 v in faceVerts)
            verts.Add(new Vector3(x, y, z) + v);

        int baseIndex = faceIndex * 4;
        tris.Add(baseIndex + 0);
        tris.Add(baseIndex + 1);
        tris.Add(baseIndex + 2);
        tris.Add(baseIndex + 0);
        tris.Add(baseIndex + 2);
        tris.Add(baseIndex + 3);

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
    }

    void SpawnPlayer()
    {
        int spawnX = width / 2;
        int spawnZ = depth / 2;
        float yMax = Mathf.FloorToInt(Mathf.PerlinNoise(
            (spawnX * noiseScale) + noiseOffsetX,
            (spawnZ * noiseScale) + noiseOffsetZ
        ) * height);

        Vector3 spawnPos = new Vector3(spawnX, yMax + 1, spawnZ);
        if (currentPlayer)
            Destroy(currentPlayer.gameObject);

        if (playerPrefab)
            currentPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    }

    private static readonly Vector3Int[] faceChecks = {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1)
    };
}