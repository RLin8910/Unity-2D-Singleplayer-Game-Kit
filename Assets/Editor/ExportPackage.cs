using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary> Editor script to export UnityPackages with project settings. </summary>
public static class ExportPackage
{
    /// <summary> Export the project as a Unity Package. </summary>

    [MenuItem("Assets/Export Package with Project Settings")]
    public static void Export()
    {
        string[] projectContent = new string[] { "Assets", "ProjectSettings/TagManager.asset", "ProjectSettings/InputManager.asset", "ProjectSettings/ProjectSettings.asset" };
        AssetDatabase.ExportPackage(projectContent, "Done.unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Project Exported");
    }
}