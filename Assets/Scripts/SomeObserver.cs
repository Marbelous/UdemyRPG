using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeObserver : MonoBehaviour
{
    CameraRaycaster cameraRaycaster;

    void Awake()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += SomeHandlingFunction;
    }

    void SomeHandlingFunction()
    {
        print("SomeHandlingFunction() called the layerChangeObserver delegate!");
    }
}
