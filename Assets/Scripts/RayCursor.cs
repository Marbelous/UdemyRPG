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

    CameraRaycaster cameraRaycaster;

    void Awake()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        // TODO: Setup mechanism to deregister OnLayerChanged delegate after change in game scene?
        cameraRaycaster.onLayerChange += OnLayerChanged; // Register the delegate
    }

    void OnLayerChanged(Layer newLayer)
    {
        switch (newLayer)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Explorable:
                Cursor.SetCursor(explorableCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Ignored:
                break;
            default:
                Debug.LogError("No Icon for this hit location!");
                return;
        }
    }
}
