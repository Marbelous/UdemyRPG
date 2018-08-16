using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeObserver : MonoBehaviour
{
    CameraRaycaster cameraRaycaster;

    void Awake()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += SomeHandlingFunction;
    }

    void SomeHandlingFunction(int layer)
    {
        print("SomeHandlingFunction() called the layerChangeObserver delegate!");
    }
}
