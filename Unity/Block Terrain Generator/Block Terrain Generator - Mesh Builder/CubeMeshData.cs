using UnityEngine;

// Static data for default cube faces
// Each face has 4 vertices, in order: bottom-left, top-left, top-right, bottom-right (counterclockwise)
public static class CubeMeshData
{
    // 6 faces, 4 verts per face
    public static readonly Vector3[][] faceVertices = {
        // Right (+X)
        new Vector3[] { new Vector3(1,0,0), new Vector3(1,1,0), new Vector3(1,1,1), new Vector3(1,0,1) },
        // Left (-X)
        new Vector3[] { new Vector3(0,0,1), new Vector3(0,1,1), new Vector3(0,1,0), new Vector3(0,0,0) },
        // Top (+Y)
        new Vector3[] { new Vector3(0,1,0), new Vector3(0,1,1), new Vector3(1,1,1), new Vector3(1,1,0) },
        // Bottom (-Y)
        new Vector3[] { new Vector3(0,0,1), new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(1,0,1) },
        // Front (+Z)
        new Vector3[] { new Vector3(1,0,1), new Vector3(1,1,1), new Vector3(0,1,1), new Vector3(0,0,1) },
        // Back (-Z)
        new Vector3[] { new Vector3(0,0,0), new Vector3(0,1,0), new Vector3(1,1,0), new Vector3(1,0,0) }
    };
}