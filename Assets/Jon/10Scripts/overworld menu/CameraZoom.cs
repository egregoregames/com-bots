using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraZoomOnPause : MonoBehaviour
{
    [SerializeField] UISo uiSo;
    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] float normalFOV = 45f;
    [SerializeField] float zoomedFOV = 25f;
    [SerializeField] float zoomDuration = 0.3f;

    Coroutine _currentZoomRoutine;

    void OnEnable()
    {
        uiSo.OnPauseStateChanged += HandlePauseChanged;
    }

    void OnDisable()
    {
        uiSo.OnPauseStateChanged -= HandlePauseChanged;
    }

    void HandlePauseChanged(bool menuOpen)
    {
        float targetFOV = menuOpen ? normalFOV : zoomedFOV;

        if (_currentZoomRoutine != null)
            StopCoroutine(_currentZoomRoutine);

        _currentZoomRoutine = StartCoroutine(SmoothZoom(targetFOV));
    }

    IEnumerator SmoothZoom(float targetFOV)
    {
        float startFOV = cinemachineCamera.Lens.FieldOfView;
        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / zoomDuration);
            yield return null;
        }

        cinemachineCamera.Lens.FieldOfView = targetFOV;
    }
}