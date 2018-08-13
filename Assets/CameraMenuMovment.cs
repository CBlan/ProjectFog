using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuMovment : MonoBehaviour {

    public Transform[] cameraPositions;
    public float speed = 1;
    private float startTime;
    private float length;
    private int movePoint;

    private int currentPoint;


    private void Start()
    {
        transform.position = cameraPositions[0].position;
        transform.rotation = cameraPositions[0].rotation;
    }

    public void MovePoint(int _movePoint)
    {
        movePoint = _movePoint;
        if (transform.position != cameraPositions[movePoint].position)
        {
            startTime = Time.time;
            length = Vector3.Distance(cameraPositions[currentPoint].position, cameraPositions[movePoint].position);
            StartCoroutine(LerpCamera());
        }
    }

    IEnumerator LerpCamera()
    {
        while (transform.position != cameraPositions[movePoint].position)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / length;

            transform.position = Vector3.Lerp(transform.position, cameraPositions[movePoint].position, fracJourney);

            float _length = Vector3.Distance(transform.position, cameraPositions[movePoint].position);
            if (_length < 0.01)
            {
                transform.position = cameraPositions[movePoint].position;
                currentPoint = movePoint;
            }
            yield return null;
        }
        yield break;
    }
}
