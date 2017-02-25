using UnityEngine;

public class MeshData {
    public Vector3 [] vertices;
    public int [] triangles;
    public Vector3 [] normals;
    public Vector2 [] uvs;

    int triangleCount;

    Mesh mesh = new Mesh();
    public Mesh CreateMesh (bool recalculateNormals) {
        mesh.name = "Polygon";
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        if ( recalculateNormals ) {
            mesh.RecalculateNormals();
        } else {
            mesh.normals = normals;
        }
        
        mesh.uv = uvs;

        mesh.Optimize();

        return mesh;
    }

    public void addTriangle(int a, int b, int c) {
        triangles [triangleCount] = a;
        triangles [triangleCount + 1] = b;
        triangles [triangleCount + 2] = c;
        triangleCount += 3;
    }
}
