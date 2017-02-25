using UnityEngine;
using System.Collections;

public class SpaceControl : MonoBehaviour {
    public enum GenerateMode { Delauney, Polyhedron }
    public GenerateMode generateMode;

    [Header("Universal Values")]
    public Vector3 scale = Vector3.one;
    public Vector3 position = Vector3.zero;

    [Space(10)]
    [Range(0 , 6)]
    public int lod = 0;

    [Space(5)]
    public int vertexCount;
    public int irregularity;

    public Camera camera;

    public void FigureInEditor () {
        switch ( generateMode ) {
            case GenerateMode.Delauney: {
                MeshData meshData = PolygonGenerator.polyfy(position, scale, vertexCount, irregularity);
                GetComponent<MeshFilter>().mesh = meshData.CreateMesh(true);
            }
            break;
            case GenerateMode.Polyhedron: {
                MeshData meshData = PolyhedronGenerator.Create(lod, scale, position);
                GetComponent<MeshFilter>().mesh = meshData.CreateMesh(false);
            }
            break;
        }
    }

    Vector3 fly = Vector3.zero;
    void Update () {
        fly.z += 0.2f;
        camera.transform.Translate(Vector3.forward);
    }
}
