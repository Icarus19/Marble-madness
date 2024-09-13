using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnLevel : MonoBehaviour
{
    ARTrackedImageManager trackedImageManager;
    [SerializeField]
    GameObject[] levels;
    bool tracking = false;

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImage;
    }
    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImage;
    }

    void OnTrackedImage(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (!tracking)
            return;
        foreach(var trackedImage in eventArgs.added)
        {
            foreach(var level in levels)
            {
                if(trackedImage.referenceImage.name == level.name)
                {
                    Instantiate(level, trackedImage.transform.position, Quaternion.identity);
                    ChangeTracking();
                }
            }
        }
    }
    public void ChangeTracking()
    {
        tracking = !tracking;
    }
}
