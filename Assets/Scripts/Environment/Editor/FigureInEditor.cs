using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpaceControl))]
public class FigureInEditor : Editor {

    public override void OnInspectorGUI () {
        SpaceControl SpaceControl = (SpaceControl) target;

        DrawDefaultInspector();
        if ( GUILayout.Button("Generate") ) {
            SpaceControl.FigureInEditor();
        }
    }

}
