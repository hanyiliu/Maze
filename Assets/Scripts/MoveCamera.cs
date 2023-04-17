using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
  public UserInput userInput;
  public CameraController cameraController;
  public float height = 3f;
  public float rotationAmount = 30f;
  public float maxRotationAngle = 90f;
  public bool won = false;

  public float swipeSpeed = 1.0f;
  public float minRotation = 90.0f;
  public float maxRotation = 180.0f;

  private bool up = false;


  public void Move()
  {
    if(won) {
      return;
    }

    if(!up) {
      transform.position += new Vector3(0f, height, 0f);
      userInput.canMove = false;
      cameraController.rotationSpeed = 45f;
      up = true;
    } else {
      transform.position -= new Vector3(0f, height, 0f);
      transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);



      // Round the angle to the nearest 90 degrees
      float angle = transform.rotation.eulerAngles.y;
      angle = Mathf.Round(angle / 90f) * 90f;
      // Set the rotation
      transform.rotation = Quaternion.Euler(0f, angle, 0f);

      userInput.canMove = true;
      cameraController.rotationSpeed = 90f;
      up = false;
    }
  }

  #if UNITY_EDITOR
  public void Update() {
    if(up) {
      if (Input.GetKeyDown(KeyCode.DownArrow))
      {
        // Rotate the camera downward, but only if it's not already at the maximum angle
        if (transform.rotation.eulerAngles.x < maxRotationAngle || transform.rotation.eulerAngles.x > 180)
        {
          transform.Rotate(Vector3.right, rotationAmount, Space.Self);
        }
      }
      else if (Input.GetKeyDown(KeyCode.UpArrow))
      {
        // Rotate the camera upward, but only if it's not already at the minimum angle
        if (transform.rotation.eulerAngles.x > 0)
        {
          transform.Rotate(Vector3.right, -rotationAmount, Space.Self);
        }
      }

    }
  }
  #endif

  #if !UNITY_EDITOR


  private float currentRotation = 0.0f;

  void Update()
  {
    if(up) {
      if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
      {
        float ySwipe = Input.GetTouch(0).deltaPosition.y * swipeSpeed * Time.deltaTime;

        currentRotation = Mathf.Clamp(currentRotation - ySwipe, minRotation, maxRotation);
        Debug.Log(currentRotation);
        transform.localRotation = Quaternion.Euler(currentRotation, 0, 0);
      }
    }
  }

  #endif

}
