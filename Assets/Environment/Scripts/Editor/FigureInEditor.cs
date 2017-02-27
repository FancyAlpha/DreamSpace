using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FigureEditor))]
public class FigureInEditor : Editor {

    public override void OnInspectorGUI () {
        FigureEditor SpaceControl = (FigureEditor) target;

        DrawDefaultInspector();
        if ( GUILayout.Button("Generate") )
            SpaceControl.FigureInEditor();
    }
}
