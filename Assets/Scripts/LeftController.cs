/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * LEFT CONTROLLER
 * 
 * LeftController.cs
 * =====================================
 * Script for handling actions performed
 * by the left VR controller.
 * 
 * Script designed to allow for player 
 * movement within VR.
 * 
 * A method obtains the value of the
 * trigger button (pressed or not).
 * If the button is pressed, it invokes
 * the trigger pressed event. When the
 * trigger is released, it invokes the 
 * trigger released event.
 * 
 * Continuous movement is mapped to the 
 * left analogue input (trackpad on HTC
 * Vive/ Analogue stick on Oculus 
 * Touch) when enabled. By holding the 
 * analogue stick forward/touching the 
 * top of the track pad, the user moves
 * in the direction they are facing.
 */

// Namespaces
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class LeftController : MonoBehaviour
{
    // Controller Node
    [SerializeField] private XRNode _controllerNode;

    // Player Camera
    [SerializeField] private Camera _playerCamera;

    // Player body
    [SerializeField] private CharacterController _playerBody;

    // Empty list device
    private List<InputDevice> _devices = new List<InputDevice>();

    // Controller
    private InputDevice _controller;

    // Analogue input (analogue stick or trackpad)
    private Vector2 _analogueInput = Vector2.zero;

    // Public analog input value
    public Vector2 analogueInput = Vector2.zero;

    // Analogue Input Y value
    [Range(-1.0f, 1.0f)] public float analogueInputY;

    // Movement enabled boolean
    [SerializeField] private bool _controllerEnabled = true;

    // Teleport enabled
    [SerializeField] private bool _teleportEnabled = true;

    // Continuous enabled
    [SerializeField] private bool _continuousEnabled = false;

    // Debug enabled boolean
    [SerializeField] private bool _debugEnabled = false;

    // Player speed
    [SerializeField, Range(1.1f, 3.5f)] private float _playerSpeed = 3.0f;

    // Minimum axis value
    [SerializeField] private float _minAxisValue = 0.5f;

    // Private trigger button boolean
    private bool _triggerButton = false;

    // Public trigger button boolean
    public bool triggerButton = false;

    // Private grip button boolean
    private bool _gripButton = false;

    // Public grip button boolean
    public bool gripButton = false;

    // On trigger press event
    public UnityEvent OnTriggerPress;

    // On trigger release event
    public UnityEvent OnTriggerRelease;

    // Grip button press event
    public UnityEvent OnGripButtonPress;

    // Grip button release event
    public UnityEvent OnGripButtonRelease;

    // Gravity
    private float _gravity = -9.81f;

    // Body velocity (used for Y component)
    private Vector3 _bodyVelocity;

    /*
     * AWAKE METHOD
     * 
     * Method is invoked when the script
     * is woken up. Sets the controller
     * node to the left hand controller
     * and checks if the controller is
     * valid.
     * 
     * If not, invokes a method to find
     * the controller
     */
    void Awake()
    {
        // Set the left controller to the left hand XRNode
        _controllerNode = XRNode.LeftHand;

        // Check if the device is valid
        if (_controller.isValid == false)
        {
            // Device not valid, invoke the Find Left Controller Function
            FindController();
        }
    }

    /*
     * FIND CONTROLLER METHOD
     * 
     * Method is used to find the controller
     * based on the value of the XR controller
     * node
     */
    void FindController()
    {
        // Get devices based on controller node and populate the devices list
        InputDevices.GetDevicesAtXRNode(_controllerNode, _devices);

        // Set the controller, based on the first/default device in list
        _controller = _devices.FirstOrDefault();
    }

    /*
     * TRIGGER ACTION METHOD
     * 
     * Method determines which event should 
     * be invoked when trigger is pressed or
     * released
     */
    private void TriggerAction(bool value)
    {
        // Check that the value is not the same as the trigger button
        if(value != triggerButton)
        {
            // Set the value of trigger button to the value
            triggerButton = value;

            // Check if value is true
            if (value == true)
            {
                // Invoke function on trigger press
                OnTriggerPress.Invoke();
            }
            else
            {
                // Invoke function on trigger release
                OnTriggerRelease.Invoke();
            }

            // Check if debug mode is enabled
            if (_debugEnabled == true)
            {
                // Log button status to Unity debug log
                Debug.Log("Trigger Pressed?: " + triggerButton + " with " + _controllerNode);
            }
        }
    }

    /*
     * GRIP ACTION METHOD
     * 
     * Method invokes events based
     * on the state of the grip
     * button.
     */
    private void GripAction(bool value)
    {
        // Check that the value is not the same as the trigger button
        if (value != gripButton)
        {
            // Set the value of trigger button to the value
            gripButton = value;

            // Check if value is true
            if (value == true)
            {
                // Invoke function on trigger press
                OnGripButtonPress.Invoke();
            }
            else
            {
                // Invoke function on trigger release
                OnGripButtonRelease.Invoke();
            }

            // Check if debug mode is enabled
            if (_debugEnabled == true)
            {
                // Log button status to Unity debug log
                Debug.Log("Grip Button Pressed?: " + gripButton + " with " + _controllerNode);
            }
        }
    }

    /*
     * UPDATE METHOD
     * 
     * Standard Unity method that is 
     * invoked once per frame.
     * 
     * Method checks if controller is 
     * valid. If not, invokes method to
     * find the controller.
     * 
     * If true, allows the user to
     * move with either continuous or 
     * teleportation. Also allows the user
     * to hold down grip button in order
     * to escape to the main menu.
     */
    void Update()
    {
        // Check if the device is valid
        if (_controller.isValid == false)
        {
            // Device not valid, invoke the Find Left Controller Function
            FindController();
        }

        // Check if movement is enabled
        if (_controllerEnabled == true)
        {
            // Check if continuous locomotion is enabled
            if (_continuousEnabled == true)
            {
                // Check if the player is grounded and the Y component of velocity is less than 0
                if (_playerBody.isGrounded && _bodyVelocity.y < 0.0f)
                {
                    // Player is grounded ensure Y component of velocity is 0
                    _bodyVelocity.y = 0.0f;
                }

                // Add to the Y component of the velocity vector 
                _bodyVelocity.y += _gravity * Time.deltaTime * _playerSpeed;

                // Continuous locomotion enabled, read the value of the analogue stick
                if (_controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out _analogueInput))
                {
                    // Check the absolute value of X value from the input and check if it is greater than
                    // the minimum axis value
                    if (Mathf.Abs(_analogueInput.y) > _minAxisValue)
                    {
                        // Check that the value does not equal the value held by the public analogue input
                        if (_analogueInput != analogueInput)
                        {
                            // Set Y value from analogue input
                            analogueInputY = _analogueInput.y;

                            // Initialise forward vector based on the cameras X and Z values. Y component kept to 0 to prevent user from flying
                            Vector3 forwardVec = new Vector3(_playerCamera.transform.forward.x, 0, _playerCamera.transform.forward.z);

                            // Check if the analogue input is negative (backwards)
                            if (analogueInputY < 0)
                            {
                                // Determoine forward vector from the camera's forward vector
                                forwardVec = -forwardVec;
                            }

                            // Move the character controller
                            _playerBody.Move(forwardVec * Time.deltaTime * _playerSpeed);
                        }
                    }
                    else
                    {
                        // Check that the value does not equal the value held by the public analogue input
                        if (_analogueInput != analogueInput)
                        {
                            // Set Y value from analogue input
                            analogueInputY = _analogueInput.y;

                            // Make the player body stationary
                            _playerBody.Move(Vector3.zero);
                        }
                    }
                }

                // Player is in the air, move them down
                _playerBody.Move(_bodyVelocity * Time.deltaTime);
            }

            // Continuous motion not enabled, check if teleportation movement enabled
            if (_teleportEnabled == true)
            {
                // Capture the value of the left controller trigger button    
                if (_controller.TryGetFeatureValue(CommonUsages.triggerButton, out _triggerButton))
                {
                    // Check the value of trigger button
                    if (_triggerButton == true)
                    {
                        // Pass true to value to Trigger Action method
                        TriggerAction(true);
                    }
                    else
                    {
                        // Pass false to value to Trigger Action method
                        TriggerAction(false);
                    }
                }
            }

            // Capture the value of the left controller trigger button    
            if (_controller.TryGetFeatureValue(CommonUsages.gripButton, out _gripButton))
            {
                // Check the value of trigger button
                if (_gripButton == true)
                {
                    // Pass true to value to Trigger Action method
                    GripAction(true);
                }
                else
                {
                    // Pass false to value to Trigger Action method
                    GripAction(false);
                }
            }
        }
    }

    /*
     * MOVEMENT ENABLED METHOD
     * 
     * Method is used to retrieve or set
     * the value of the movement 
     * enabled boolean
     */
    public bool ControllerEnabled
    {
        // Get function
        get
        {
            // Return the value of movement enabled
            return _controllerEnabled;
        }
        // Set function
        set
        {
            // Set the value of movement enabled
            _controllerEnabled = value;
        }
    }
}
