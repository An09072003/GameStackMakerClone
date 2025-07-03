using UnityEngine;
using UnityEditor;

public class BatchSetLayerByTag : EditorWindow
{
    string tagToFind = "Ground"; // Đổi thành tag bạn dùng
    int layerToSet = 8; // Số layer. "Ground" thường là Layer 8 nếu bạn tạo mới

    [MenuItem("Tools/Batch Set Layer By Tag")]
    public static void ShowWindow()
    {
        GetWindow<BatchSetLayerByTag>("Batch Set Layer");
    }

    void OnGUI()
    {
        tagToFind = EditorGUILayout.TagField("Tag", tagToFind);
        layerToSet = EditorGUILayout.LayerField("Layer to Set", layerToSet);

        if (GUILayout.Button("Apply Layer"))
        {
            ApplyLayer();
        }
    }

    void ApplyLayer()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(tagToFind);
        foreach (GameObject obj in allObjects)
        {
            obj.layer = layerToSet;
        }

        Debug.Log($"Đã gán {allObjects.Length} object từ tag '{tagToFind}' sang layer '{LayerMask.LayerToName(layerToSet)}'");
    }
}
