using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class RayCursor : MonoBehaviour
{
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D enemyCursor = null;
    [SerializeField] Texture2D explorableCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(96, 96);

    // TODO: Create proper customized editor for layers.
    [SerializeField] const int walkableLayerNumber = 10;
    [SerializeField] const int enemyLayerNumber = 11;
    CameraRaycaster cameraRaycaster;

    void Awake()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        // TODO: Setup mechanism to deregister OnLayerChanged delegate after change in game scene?
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged; // Register the delegate
    }

    void OnLayerChanged(int newLayer)
    {
        switch (newLayer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(explorableCursor, cursorHotspot, CursorMode.Auto);
                return;
        }
    }
}
