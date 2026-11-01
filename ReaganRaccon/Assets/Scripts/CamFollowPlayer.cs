using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    //Set reference in inspector
    public GameObject player;
    public Camera myCamera;

    [Header("Camera Zoom Behavior")]
    [SerializeField] private float defaultCameraSize = 9.0f;
    [SerializeField] private float zoomedCameraSize = 14.0f;
    [SerializeField] private bool isZoomedOut = false;

    public void OnZoom()
    {
        if (isZoomedOut)
        {
            ZoomIn();
            isZoomedOut=false;
        }
        else
        {
            ZoomOut();
            isZoomedOut = true;
        }
    }

    public void ZoomOut()
    {
        myCamera.orthographicSize = zoomedCameraSize;
    }

    public void ZoomIn()
    {
        myCamera.orthographicSize = defaultCameraSize;
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            transform.position.z);

    }

    void Start()
    {
        if (myCamera == null)
        {
            myCamera = Camera.main;
        }

        //myCamera.orthographicSize = 7.0f;
    }
}




