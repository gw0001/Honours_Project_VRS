/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * SCENE SYSTEM
 * 
 * SceneSystem.cs
 * =====================================
 * Script to handle the experiment
 * scene and UI display for the 
 * user.
 * 
 * At the start, the script sets the
 * text for the player beginning the 
 * experiment and sets the users watch
 * to display the time remaining. The 
 * user controls are disabled to prevent
 * movement and allow the user to see the
 * virtual environment.
 * 
 * A 5 second count down begins, and is
 * displayed in front of the users view.
 * Once the countdown ends, the user is 
 * given the instruction to go, the 
 * experimetn timer starts counting down 
 * and the controls are enabled to allow
 * the user the freedom to exlpore the
 * environment.
 * 
 * Once the countdown timer reaches 0
 * (or less) the controls are disabled and
 * a message is displayed to let the user
 * know the test has ended. A countdown
 * begins before the scene changes back
 * to the main menu.
 */

// Namespaces
using UnityEngine;

public class SceneSystem : MonoBehaviour
{
    // The Game Manager
    [SerializeField] private GameManager _theGM;

    // Text mesh
    [SerializeField] private TextMesh _userText;

    // Wait timer
    [SerializeField] private float _waitTimer = 8.0f;

    // Waiting enabled boolean
    private bool _waitingEnabled = true;

    // Text mesh renderer
    private MeshRenderer _textMeshRenderer;

    // Go message enabled boolean
    private bool _goMessageEnabled = false;

    // "Go" message timer
    [SerializeField] private float _goMessageTimer = 2.0f;

    // Test enabled boolean
    private bool _testEnabled = false;

    // Test timer float set to 600 seconds (10 mins) as default
    [SerializeField] private float _testTimer = 600.0f;

    // Experiment boolean over
    private bool _experimentOver = false;

    // Experiment over timer
    [SerializeField] private float _experimentOverTimer = 5.0f;

    // Player watch to display remaining experiment time
    [SerializeField] private TextMesh _playerWatch;

    // Left Controller
    private LeftController _leftController;

    /*
     * START FUNCTION
     * 
     * Standard Unity function invoked
     * before the first frame update.
     * 
     * Method first sets the text of 
     * the user text, then obtains the
     * mesh renderer of the text
     * object. Method then displays 
     * the remaining time to the user
     * and disables control from the 
     * left VR controller.
     */
    void Start()
    {
        // Set the display text to the wait timer
        _userText.text = ((int)_waitTimer).ToString();

        // Obtain the text mesh renderer
        _textMeshRenderer = _userText.GetComponent<MeshRenderer>();

        // Display the remaining time on the player's watch
        DisplayRemainingTime();

        // Disable the left controller
        _leftController.ControllerEnabled = false;
    }

    /*
     * AWAKE METHOD
     * 
     * Standard Unity method that is invoked
     * when the script is awoken.
     * 
     * Method obtains the game controller and
     * the left controller
     */
    private void Awake()
    {
        // Find the game manager
        _theGM = FindObjectOfType<GameManager>();

        // Find the left controller script
        _leftController = FindObjectOfType<LeftController>();
    }

    /*
     * UPDATE METHOD
     * 
     * Standard Unity functions that carries out
     * updates for each frame of the application.
     * 
     * Method first invokes a wait timer before
     * the experiment begins and displays the
     * timer to the user.
     * 
     * Once counter reaches zero, the 
     * text alerts the user to move and 
     * control is given to the player. Begins
     * an experiment timer which is displayed
     * to the left controller game object.
     * 
     * Once experiment ends, user is locked out
     * of locomotion and an message is displayed 
     * to the user. Another timer then begins.
     * 
     * Once the timer meets the condition,
     * the user is returned ot the main menu scene.
     */
    void Update()
    {
        // Check if waiting is enabled
        if(_waitingEnabled == true)
        {
            // Decrement the wait timer
            _waitTimer -= Time.deltaTime;

            // Set message to the wait timer
            _userText.text = ((int)_waitTimer + 1).ToString();
        }

        // Check if wait time is less than 0 and the waiting timer has been enabled
        if(_waitTimer <= 0.0f && _waitingEnabled == true)
        {
            // Set text to "GO!"
            _userText.text = "GO!";

            // Set go message enabled boolean to true
            _goMessageEnabled = true;

            // Disable waiting
            _waitingEnabled = false;

            // Set test enabled boolean to true
            _testEnabled = true;

            // Enable the left controller
            _leftController.ControllerEnabled = true;
        }

        // Check if go message is enabled
        if(_goMessageEnabled == true)
        {
            // Decrement timer by delta time
            _goMessageTimer -= Time.deltaTime;
        }

        // Check if the go message timer is less that 1 second
        if(_goMessageTimer <= 0.0f && _goMessageEnabled == true)
        {
            // Disable the text mesh renderer
            _textMeshRenderer.enabled = false;

            // Disable the go message
            _goMessageEnabled = false;
        }
        
        // Check if test is enabled
        if(_testEnabled == true)
        {
            // Decrement the test time by delta time
            _testTimer -= Time.deltaTime;
        }

        // Check if test is enabled
        if (_testEnabled == true)
        {
            // Set message to the wait timer
            DisplayRemainingTime();
        }

        // Check if test time is less than 0
        if (_testTimer <= 0.0f && _testEnabled == true)
        {
            // Set display text to "Test Complete" message
            _userText.text = "Test\n Complete";

            // Enable the display text mesh renderer
            _textMeshRenderer.enabled = true;

            // Set the player watch to display 00:00 (no more time left)
            _playerWatch.text = "00:00";

            // Lock player movement
            _leftController.ControllerEnabled = false;

            // Set experiment over boolean to true
            _testEnabled = false;

            // Set experiment over boolean to true
            _experimentOver = true;
        }

        // Check if experiment over boolean is true
        if(_experimentOver == true)
        {
            // Decrement the experiment over timer by delta time
            _experimentOverTimer -= Time.deltaTime;
        }

        // Check if the experiment over timer is equal to or less than 0
        if(_experimentOverTimer <= 0.0f && _experimentOver == true)
        {
            // Instruct the game manager to load the menu scene
            _theGM.sceneSelection("MainScene");
        }
    }

    /*
     * DISPLAY REMAINING TIME METHOD
     * 
     * Method is used to display the value held
     * by the test timer in a minutes:seconds 
     * format.
     */
    private void DisplayRemainingTime()
    {
        // Determine the minutes from the test timer
        float minutes = Mathf.FloorToInt(_testTimer / 60);

        // Determine the seconds from the test timer
        float seconds = Mathf.FloorToInt(_testTimer % 60);

        // Change the text on the users display watch to show the number
        // of minutes and seconds remaining for the test
        _playerWatch.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
