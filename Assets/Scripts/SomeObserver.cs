using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeObserver : MonoBehaviour
{
    CameraRaycaster cameraRaycaster;

    void Awake()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.onLayerChange += SomeHandlingFunction;
    }

    void SomeHandlingFunction(Layer layer)
    {
        print("SomeHandlingFunction() called the layerChangeObserver delegate!");
    }
}
