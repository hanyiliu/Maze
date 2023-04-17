using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public UserInput userInput;
    public MoveCamera moveCamera;
    public Transform playerStart;

    public void OnRestartButtonClick()
    {
        // Destroy all walls and rebuild the maze
        mazeGenerator.ResetMaze();

        // Reset the player's position to the start of the maze
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = playerStart.position;

        // Hide the congratulation screen and reset the hasWon flag
        gameObject.SetActive(false);
        userInput.canMove = true;
        userInput.targetPosition = playerStart.position;
        moveCamera.won = false;

    }
}
