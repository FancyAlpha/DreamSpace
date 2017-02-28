using UnityEngine;
using Dynagon;
using System.Collections.Generic;

public class FigureEditor : MonoBehaviour {
    public enum GenerateMode { Dynagon, Polyhedron }
    public GenerateMode generateMode;

    [Header("Universal Values")]
    public Vector3 scale = Vector3.one;
    public Vector3 position = Vector3.zero;

    [Range(0 , 6)]
    public int lod = 0;

    public int vertexes;

    //In-editor method
    public void FigureInEditor () {
        switch ( generateMode ) {
            case GenerateMode.Dynagon:
                GetComponent<MeshFilter>().mesh = CreatePolygon(vertexes).CreateMesh(true);
            break;
            case GenerateMode.Polyhedron:
                GetComponent<MeshFilter>().mesh = PolyhedronGenerator.Create(lod , scale , position).CreateMesh(false);
            break;
        }
    }

    MeshData CreatePolygon (int numVertices) {
        List<Vector3> vertices = new List<Vector3>();
        for ( int i = 0; numVertices > i; i++ )
            vertices.Add(Vector3.Scale(Random.onUnitSphere + position, scale));

        return new Polygon3D(Triangulator3D.Triangulate(vertices)).Build();
    }

    //quick flight
    //Vector3 fly = Vector3.zero;
    //void Update () {
    //    fly.z += 0.2f;
    //    figure.transform.Translate(Vector3.forward);
    //}
}
