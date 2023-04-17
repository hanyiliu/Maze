using UnityEngine;
using UnityEngine.UI;

public class EndPointScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI winText;
    public Button restartButton;
    public UserInput userInput;
    public MoveCamera moveCamera;

    private bool hasWon = false;

    public void Start()
    {
        Debug.Log("Started");
        winText.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        if (other.CompareTag("Player") && !hasWon)
        {
            hasWon = true;
            winText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            userInput.canMove = false;
            moveCamera.won = true;
        }
    }

    public void RestartGame()
    {
        // Implement your game reset logic here
        // This method is called when the user clicks the "Restart" button on the winText object
    }
}
