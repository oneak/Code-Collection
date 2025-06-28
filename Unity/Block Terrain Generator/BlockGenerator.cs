using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class BlockGenerator : MonoBehaviour
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

    private bool[,,] blockData;
    private float noiseOffsetX;
    private float noiseOffsetZ;
    private int blockCount = 0;

    // Texture atlas settings (6 faces = 3x2 grid)
    private const int atlasCols = 3;
    private const int atlasRows = 2;

    // Face order in CubeMeshData: Right, Left, Top, Bottom, Front, Back
    // Desired atlas order: 0=Top, 1=Right, 2=Left, 3=Front, 4=Back, 5=Bottom
    private static readonly int[] faceAtlasIndex = {
        1, // Right  → atlas 1
        2, // Left   → atlas 2
        0, // Top    → atlas 0
        5, // Bottom → atlas 5
        3, // Front  → atlas 3
        4  // Back   → atlas 4
    };

    void Start()
    {
        RandomizeOffsets();
        GenerateBlockData();
        BuildMesh();

        if (blockMaterial)
            GetComponent<MeshRenderer>().material = blockMaterial;

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

                    // Decoration placement
                    if (rand < 0.05f && treePrefab)
                        InstantiateDecoration(treePrefab, topCenter);
                    else if (rand < 0.1f && rockPrefab)
                        InstantiateDecoration(rockPrefab, topCenter);
                }
            }
        }

        Debug.Log("Chunk generation complete. Blocks created: " + blockCount);
    }

    // Ensures decoration is anchored at its base to the ground
    void InstantiateDecoration(GameObject prefab, Vector3 surfacePosition)
    {
        // Try to get the Renderer bounds of the prefab for base alignment
        float baseOffsetY = 0f;
        Renderer rend = prefab.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            // The pivot is at prefab.transform.position, so
            // The bottom of the mesh is at (center.y - extents.y) relative to the prefab's pivot
            // To place bottom of mesh flush with ground, shift up by (extents.y - center.y)
            // But for most prefabs, center is relative to prefab pivot, so just use extents
            // We'll assume pivot in center for most assets, so offset by extents.y
            baseOffsetY = rend.bounds.extents.y;
        }
        // Place so that the bottom of the mesh sits on the surface
        Vector3 spawnPos = surfacePosition + Vector3.up * baseOffsetY;
        Instantiate(prefab, spawnPos, Quaternion.identity, transform);
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

        // Assign UVs so this face uses its region in the atlas
        int atlasIdx = faceAtlasIndex[face];
        int col = atlasIdx % atlasCols;
        int row = atlasIdx / atlasCols;

        float uMin = (float)col / atlasCols;
        float vMin = 1.0f - ((float)(row + 1) / atlasRows);
        float uMax = uMin + (1.0f / atlasCols);
        float vMax = vMin + (1.0f / atlasRows);

        // Bottom-left, Top-left, Top-right, Bottom-right (match verts order)
        uvs.Add(new Vector2(uMin, vMin));
        uvs.Add(new Vector2(uMin, vMax));
        uvs.Add(new Vector2(uMax, vMax));
        uvs.Add(new Vector2(uMax, vMin));
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
        new Vector3Int(1, 0, 0),    // Right
        new Vector3Int(-1, 0, 0),   // Left
        new Vector3Int(0, 1, 0),    // Top
        new Vector3Int(0, -1, 0),   // Bottom
        new Vector3Int(0, 0, 1),    // Front
        new Vector3Int(0, 0, -1)    // Back
    };
}