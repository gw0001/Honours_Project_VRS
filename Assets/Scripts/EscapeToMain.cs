/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * RIGHT CONTROLLER
 * 
 * EscapeToMain.cs
 * =====================================
 * Script for allowing the user to 
 * return to the main menu.
 * 
 * Script first obtains the game 
 * manager, sets the escape timer, and
 * hides the messages to the user from
 * view.
 * 
 * Update method first checks if both
 * grip buttons are pressed. If both
 * are pressed, the method begins 
 * decrementing the timer and displays
 * an escape message to the player.
 * 
 * If one or neither grip buttons are 
 * not pressed, then the method 
 * resets the timer and both messages
 * are hidden from the players view.
 * 
 * If the timer reaches, or goes below,
 * 0 seconds, the update function tells
 * the game manager to return to the 
 * main scene.
 * 
 */

// Name space
using UnityEngine;

public class EscapeToMain : MonoBehaviour
{
    // The Game Manager
    private GameManager _theGM;

    // Left Grip button
    private bool _leftGrip = false;

    // Right Grip Button
    private bool _rightGrip = false;

    // Escape wait time
    [SerializeField] private float _escapeWaitTime = 3.0f;

    // Escape timer
    [SerializeField] private float _escapeTimer;

    // Message text mesh
    [SerializeField] private TextMesh _userMessageText;

    // Escape counter text mesh
    [SerializeField] private TextMesh _userEscapeCounter;

    // Message mesh renderer
    private MeshRenderer _userMessageTextMesh;

    // Escape counter mesh renderer
    private MeshRenderer _userEscapeCounterMesh;

    /*
     * START METHOD
     * 
     * Standard Unity method that is invoked
     * before the first frame update
     */
    void Start()
    {
        // Find the game manager
        _theGM = FindObjectOfType<GameManager>();

        // Set the escape timer
        _escapeTimer = _escapeWaitTime;

        // Obtain the message text mesh renderer
        _userMessageTextMesh = _userMessageText.GetComponent<MeshRenderer>();

        // Obtain the escape counter mesh renderer
        _userEscapeCounterMesh = _userEscapeCounter.GetComponent<MeshRenderer>();

        // Hide the message text
        _userMessageTextMesh.enabled = false;

        // Hide the escape counter
        _userEscapeCounterMesh.enabled = false;
    }

    /*
     * UPDATE METHOD
     * 
     * Standard Unity method that is invoked
     * once per frame.
     * 
     * Method checks if both grip buttons
     * have been pressed in order to escape
     * to the main menu scene.
     */
    void Update()
    {
        // Check if the left and right grip buttons are pressed
        if ((_leftGrip == true) && (_rightGrip == true))
        {
            // Decrement the escape timer
            _escapeTimer -= Time.deltaTime;

            // Display the value of the counter
            _userEscapeCounter.text = ((int)_escapeTimer + 1).ToString();

            // Make the message text visible to the user
            _userMessageTextMesh.enabled = true;

            // Make the counter visible to the user
            _userEscapeCounterMesh.enabled = true;
        }
        else
        {
            // Set the escape timer to the wait time
            _escapeTimer = _escapeWaitTime;

            // Hide the message text from the user
            _userMessageTextMesh.enabled = false;

            // Hide the escape counter from the user
            _userEscapeCounterMesh.enabled = false;
        }
        
        // Check if the escape timer is less than or equal to 0 seconds
        if (_escapeTimer <= 0.0f)
        {
            // Return to the main scene (the menu)
            _theGM.sceneSelection("MainScene");
        }
    }

    /*
     * ENABLE LEFT GRIP METHOD
     * 
     * Method is used to set the
     * left grip boolean to true
     */
    public void EnableLeftGrip()
    {
        // Set the left grip bool to true
        _leftGrip = true;
    }

    /*
     * DISABLE LEFT GRIP FUNCTION
     * 
     * Method is used to set the
     * left grip boolean to false
     */
    public void DisableLeftGrip()
    {
        // Set the left grip bool to false
        _leftGrip = false;
    }

    /*
     * ENABLE RIGHT GRIP METHOD
     * 
     * Method is used to set the
     * right grip boolean to true
     */
    public void EnableRightGrip()
    {
        // Set the right grip bool to true
        _rightGrip = true;
    }

    /*
     * DISABLE LEFT GRIP FUNCTION
     * 
     * Method is used to set the
     * right grip boolean to false
     */
    public void DisableRightGrip()
    {
        // Set the right grip bool to false
        _rightGrip = false;
    }
}
