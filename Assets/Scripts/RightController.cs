/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * RIGHT CONTROLLER
 * 
 * RightController.cs
 * =====================================
 * Script for handling actions performed
 * by the right VR controller.
 * 
 * Script designed to allow for right
 * controller to allow for menu 
 * interaction.
 * 
 * Contains methods for trigger 
 * interaction and grip button
 * interaction.
 */

// Namespaces
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class RightController : MonoBehaviour
{
    // Controller Node
    [SerializeField] private XRNode _controllerNode;

    // Player Camera
    [SerializeField] private Camera _playerCamera;

    // Empty list device
    private List<InputDevice> _devices = new List<InputDevice>();

    // Controller
    private InputDevice _controller;

    // Teleport enabled
    [SerializeField] private bool _triggerEnabled = true;

    // Debug enabled boolean
    [SerializeField] private bool _debugEnabled = false;

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

    // On Grip button press event
    public UnityEvent OnGripButtonPress;

    // On Grip button release event
    public UnityEvent OnGripButtonRelease;

    /*
     * ON ENABLE METHOD
     * 
     * Method is invoked when the script
     * is activated.
     * 
     * Sets the controller node to the 
     * right hand controller and checks
     * if it is valid.
     * 
     * If not valid, it finds the controller.
     */
    void OnEnable()
    {
        // Set the right controller to the right hand XRNode
        _controllerNode = XRNode.RightHand;

        // Check if the device is valid
        if (_controller.isValid == false)
        {
            // Device not valid, invoke the Find Controller Function
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
        if (value != triggerButton)
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
     * Method determines the grip
     * action event to take, based on 
     * the value passed through the 
     * argument
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
     * Method updates the state of the application
     * and is invoked once per frame.
     * 
     * Method is used to check if the controller
     * is valid. If not, it invokes a method
     * to find the controller.
     * 
     * Method then checks if the triggeer
     * and grip buttons are enabled.
     * 
     * Trigger allows the user to select 
     * an item from the menu, and the 
     * grip button allows the user to 
     * escape to the main scene.
     */
    void Update()
    {
        // Check if the device is valid
        if (_controller.isValid == false)
        {
            // Device not valid, invoke the Find Controller Function
            FindController();
        }

        // Check if trigger enabled
        if (_triggerEnabled == true)
        {
            // Capture the value of the right controller trigger button    
            if (_controller.TryGetFeatureValue(CommonUsages.triggerButton, out _triggerButton))
            {
                // Check the value of trigger button
                if (_triggerButton == true)
                {
                    // Find Menu System object
                    if(FindObjectOfType<MenuSystem>() == true)
                    {
                        // Invoke the Load Scene function held by the menu scene object
                        FindObjectOfType<MenuSystem>().LoadScene();
                    }
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
