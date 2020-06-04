using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Camera arCamera;
    public GameObject starField;

    // Call this function to disable FPS camera,
    // and enable overhead camera.
    public void ShowArView()
    {
        mainCamera.enabled = false;
        arCamera.enabled = true;
        starField.SetActive(false);
    }

    // Call this function to enable FPS camera,
    // and disable overhead camera.
    public void ShowMainView()
    {
        mainCamera.enabled = true;
        arCamera.enabled = false;
        starField.SetActive(true);
    }
}