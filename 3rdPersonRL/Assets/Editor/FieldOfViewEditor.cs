using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
     void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngle = fow.dirFromAngle(-fow.viewAngle / 2.0f, false);
        Vector3 viewAngleB = fow.dirFromAngle(fow.viewAngle / 2.0f, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngle * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        if(fow.visibleTarget != null)
            Handles.DrawLine(fow.transform.position, fow.visibleTarget.transform.position);
    }
}
