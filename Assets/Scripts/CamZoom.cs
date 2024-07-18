using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoom : MonoBehaviour
{
    private Camera cam;
    private float defaultSize;
    private Vector3 defaultPos;

    public float zoomSize;
    public float zoomSpeed;
    public Transform playerTarget;

    public bool zoomIn = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
        defaultPos = cam.transform.position;
    }

    // Zoom the camera in.
    public void ZoomIn()
    {
        Vector3 targetPos = new Vector3 (transform.position.x, playerTarget.position.y/5, -10);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.5f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, zoomSpeed * Time.unscaledDeltaTime);
    }

    // Zoom the camera out.
    public void ZoomOut()
    {
        Vector3 targetPos = defaultPos;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.5f);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultSize, zoomSpeed * Time.unscaledDeltaTime);
    }
    private void FixedUpdate()
    {
        if(zoomIn)
        {
            ZoomIn();
        }
        else if(!zoomIn && cam.orthographicSize != defaultSize)
        {
            ZoomOut();
        }
    }
}
