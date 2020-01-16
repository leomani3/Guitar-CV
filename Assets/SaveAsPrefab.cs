using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveAsPrefab : Editor
{

    static public void saveAsPrefab(GameObject go, string fileName)
    {
        PrefabUtility.SaveAsPrefabAsset(go, "Assets/" + fileName + ".prefab");
    }
}
