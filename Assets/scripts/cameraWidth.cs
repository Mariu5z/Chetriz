using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraWidth : MonoBehaviour
{
    float desiredHorizontalSize;
    float aspectRatio;
    public static Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        float desiredHorizontalSize = 28.0f; // Set your desired horizontal size
        float aspectRatio = mainCamera.aspect;

        mainCamera.orthographicSize = desiredHorizontalSize / aspectRatio;
    }

}
