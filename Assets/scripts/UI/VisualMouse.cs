using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualMouse : MonoBehaviour
{
    public static VisualMouse Instance { get; private set; }
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    //void OnMouseEnter()
    //{
    //    Debug.Log("entrou");
    //    Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    //}
    //
    //void OnMouseExit()
    //{
    //    Debug.Log("saiu");
    //    Cursor.SetCursor(null, Vector2.zero, cursorMode);
    //}
}
