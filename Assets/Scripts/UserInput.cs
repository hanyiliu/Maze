using UnityEngine;

public class UserInput : MonoBehaviour
{
    public float speed = 5f; // Movement speed of the sprite
    public bool canMove = true;
    public Vector3 targetPosition; // The position to move the sprite towards

    void Update()
    {


        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && canMove && (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)))
        {
            Debug.Log("Test0");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits any of the maze tiles
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("FloorTile")))
            {
                Debug.Log("Test1");
                // Check if there is a clear path to the clicked tile
                if (IsPathClear(transform.position, hit.transform.position))
                {
                    Debug.Log("Test2");
                    // Set the target position to the clicked tile's position
                    targetPosition = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);

                }
            }
        }

        // Move the sprite towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Make the sprite face the direction of movement
        // if (transform.position != targetPosition)
        // {
        //     transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);
        // }
    }

    bool IsPathClear(Vector3 startPos, Vector3 endPos)
    {
        // Check if there is a clear path from startPos to endPos
        RaycastHit hit;
        if (Physics.Linecast(startPos, endPos, out hit))
        {
            if (hit.transform.tag == "FloorTile")
            {
                // The line cast hit a floor tile, indicating a clear path
                return true;
            }
            else
            {
                // The line cast hit something else, indicating a blocked path
                return false;
            }
        }
        // The line cast did not hit anything, indicating a clear path
        return true;
    }
}
