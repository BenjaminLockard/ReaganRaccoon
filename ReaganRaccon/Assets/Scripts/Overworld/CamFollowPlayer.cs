using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CamFollowPlayer : MonoBehaviour
{
    //Set reference in inspector
    public GameObject player;
    public Camera myCamera;

    [Header("Camera Zoom Behavior")]
    [SerializeField] private float defaultCameraSize = 9.0f;
    [SerializeField] private float zoomedCameraSize = 18.0f;
    [SerializeField] private bool isZoomedOut = false;

    private Coroutine zoomCoroutine;

    public AudioClip zoomSound;
    public AudioSource audioSource;
    public void OnZoom()
    {
        audioSource.PlayOneShot(zoomSound, 2.0f);
        if (isZoomedOut)
        {
            zoomCoroutine = StartCoroutine(ZoomIn());
        }
        else
        {
            zoomCoroutine = StartCoroutine(ZoomOut());
        }
    }

    public IEnumerator ZoomOut()
    {
        if (!isZoomedOut)
        {
            isZoomedOut = true;

            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            for (float i = 0.125f; myCamera.orthographicSize <= zoomedCameraSize; i *= 1.5f)
            {
                myCamera.orthographicSize += i;
                yield return new WaitForSeconds(0.035f);
            }

            myCamera.orthographicSize = zoomedCameraSize;
        }
    }

    public IEnumerator ZoomIn()
    {
        if (isZoomedOut)
        {
            isZoomedOut = false;

            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            for (float i = myCamera.orthographicSize / 6; myCamera.orthographicSize >= defaultCameraSize; i /= 1.5f)
            {
                myCamera.orthographicSize -= i;
                yield return new WaitForSeconds(0.035f);
            }

            myCamera.orthographicSize = defaultCameraSize;
        }
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
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (myCamera == null)
        {
            myCamera = Camera.main;
        }

        //myCamera.orthographicSize = 7.0f;
    }
}




