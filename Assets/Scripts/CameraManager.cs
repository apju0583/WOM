using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;

    public GameObject Target;
    public float follow_speed = 4.0f;
    public float z = -10.0f;
    public float zoomOutSize = 7.5f;

    private Transform this_transform;
    private Transform Target_transform;
    private Camera cam;

    private Vector2 minPosition;
    private Vector2 maxPosition;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        instance = this;

        this_transform = GetComponent<Transform>();
        Target_transform = Target.GetComponent<Transform>();
        cam = GetComponent<Camera>();
        cam.orthographicSize = zoomOutSize;

        SceneManager.sceneLoaded += OnSceneLoaded;

        UpdateCameraLimits();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        Vector3 newPosition = Vector2.Lerp(this_transform.position, Target_transform.position, follow_speed * Time.deltaTime);

        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);
        newPosition.z = z;

        this_transform.position = newPosition;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCameraLimits();
    }

    private void UpdateCameraLimits()
    {
        CameraLimit cameraLimit = FindObjectOfType<CameraLimit>();
        if (cameraLimit != null)
        {
            minPosition = cameraLimit.minPosition;
            maxPosition = cameraLimit.maxPosition;
        }
    }
}
