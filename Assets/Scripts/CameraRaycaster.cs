using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{

    public Layer[] layerPriorities = {
        Layer.Walkable,
        Layer.Enemy,
        Layer.Explorable,
        Layer.Ignored
        //Layer.RaycastEndStop
    };


    float distanceToBackground = 1000f;
    Camera viewCamera;


    RaycastHit raycastHit;
    public RaycastHit hit
    {
        get { return raycastHit; }
    }

    Layer layerHit;
    public Layer currentLayerHit
    {
        get { return layerHit; }
    }


    public delegate void OnLayerChange();  // declare new delegate
    public OnLayerChange layerChangeObservers;  // instantiate an observer set


    void Start() // TODO Awake?
    {
        viewCamera = Camera.main;

    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            //print(layerPriorities.Length);
            //print(layer);
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                raycastHit = hit.Value;
                if (layerHit != layer) // layer changed
                {
                    layerHit = layer;
                    layerChangeObservers(); // Call Delegates
                }
                layerHit = layer;
                return;
            }
        }
        // Otherwise return background hit
        raycastHit.distance = distanceToBackground;
        layerHit = Layer.RaycastEndStop;
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }

}
