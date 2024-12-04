using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageToMiniMap : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager arTrackedImageManager;
    [SerializeField] private GameObject miniMapUI;

    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // 새로 인식된 이미지
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateMiniMap(trackedImage);
        }

        // 상태가 변경된 이미지 (인식이 유지되거나 사라지는 경우)
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateMiniMap(trackedImage);
        }

        // 사라진 이미지
        foreach (var trackedImage in eventArgs.removed)
        {
            DisableMiniMap();
        }
    }

    private void UpdateMiniMap(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
        {
            miniMapUI.SetActive(true); // 미니맵 활성화
        }
        else
        {
            miniMapUI.SetActive(false); // 미니맵 비활성화
        }
    }

    private void DisableMiniMap()
    {
        miniMapUI.SetActive(false); // 미니맵 비활성화
    }
}
