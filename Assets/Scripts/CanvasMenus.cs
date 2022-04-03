using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Class for handling UI menus. </summary>
public class CanvasMenus : MonoBehaviour
{
    /// <summary> The menu gameObjects. </summary>
    /// <remarks> All gameObjects must have unique names for OpenMenu() to work properly.
    /// The first gameObject in the list will be the default menu which is open when the game
    /// starts. </remarks>
    [SerializeField]
    private GameObject[] menus = new GameObject[0];
    /// <summary> Menu gameObject dictionary mapping names to gameObjects. </summary>
    private Dictionary<string, GameObject> menuObjects = new Dictionary<string, GameObject>();
    void Start()
    {
        foreach(GameObject menu in menus) menuObjects[menu.name] = menu;
        OpenMenu(menus[0].name);
    }
    /// <summary> Open a menu. </summary>
    /// <param name="menuName"> The name of the menu to open. </param>
    public void OpenMenu(string menuName)
    {
        foreach(GameObject menu in menuObjects.Values) menu.SetActive(false);
        menuObjects[menuName].SetActive(true);
    }
}
