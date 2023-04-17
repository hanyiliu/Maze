using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 90.0f;
    public float rotationTime = 0.5f;

    private bool isRotating = false;


    void Update()
    {
      if (!isRotating)
      {
          if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.touchCount > 0 && Input.GetTouch(0).deltaPosition.x < -50))
          {
              StartCoroutine(RotateCamera(Vector3.forward, -rotationSpeed));
          }
          else if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.touchCount > 0 && Input.GetTouch(0).deltaPosition.x > 50))
          {
              StartCoroutine(RotateCamera(Vector3.forward, rotationSpeed));
          }
      }

    }


    IEnumerator RotateCamera(Vector3 axis, float angle)
    {
        isRotating = true;

        float elapsedTime = 0.0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles + axis * angle);

        while (elapsedTime < rotationTime)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, (elapsedTime / rotationTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        isRotating = false;
    }
}
