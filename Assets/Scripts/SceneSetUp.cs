/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * SCENE SET UP
 * 
 * SceneSetUp.cs
 * =====================================
 * Script for the setting up a scene
 * with collectables and handling
 * collectable positions.
 * 
 * Script first looks for all 
 * collectable spawn points in the 
 * scene. Spawn points are then stored
 * in a list.
 * 
 * An initial collectable spawn point
 * is initialised for the collectable
 * to first spawn.
 * 
 * Script also contains method to 
 * change the point where the 
 * collectable can spawn.
 */

// Namespaces
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneSetUp : MonoBehaviour
{
    // First pick up point
    private Vector3 _initialPickUpPoint;

    // List of pickup points
    private List<Vector3> _pickUpPoints = new List<Vector3>();

    // Current point
    private Vector3 _currentPoint;

    // Previous Point
    private Vector3 _previousPoint;

    // Collectable game object
    private GameObject _collectable;

    // Player character controller
    [SerializeField] private CharacterController _player;

    // Collectable counter
    private int _collectableCounter = 0;

    // Collectable counter text mesh
    [SerializeField] private TextMesh _counterTextMesh;

    // Enable debug boolean
    [SerializeField] private bool _debugEnabled = false;

    /*
     * START METHOD
     * 
     * Standard Unity method that is
     * invoked before the first frame updated.
     * 
     * Method first obtains the position
     * of the first collectable point, then
     * obtains the position of all the 
     * other collectable points.
     * 
     * Method then initialises the collectable
     * to the initial position.
     * 
     */
    void Start()
    {
        // Obtain the first pickup point location
        _initialPickUpPoint = GameObject.FindGameObjectWithTag("firstPickUpPoint").transform.position;

        // Game Object array of pick up objects
        GameObject[] pickUpObjects;

        // Obtain all pick up objects based on tag
        pickUpObjects = GameObject.FindGameObjectsWithTag("pickUpPoint");

        // Check if debug mode is enabled
        if(_debugEnabled == true)
        {
            // Output the total number of pick up object spots to Unitys debug log
            Debug.Log("Number of pick up object spots: " + pickUpObjects.Length);
        }

        // Itterate over all pick up objects
        foreach(GameObject ob in pickUpObjects)
        {
            // Obtain the script attached to the pick up game object
            PickUpSpot point = ob.GetComponent<PickUpSpot>();

            // Obtain the origin point and add to list
            _pickUpPoints.Add(point.transform.position);
        }

        // Check if debug mode is enabled
        if (_debugEnabled == true)
        {
            // Output the info of all collectables to the Unity debug log
            for (int i = 0; i < _pickUpPoints.Count; i++)
            {
                // Output debug message with point information
                Debug.Log("Point " + (i + 1) + ". X: " + _pickUpPoints[i].x + " Y: " + _pickUpPoints[i].y + " Z: " + _pickUpPoints[i].z);
            }
        }

        // Initialise first point for collectable
        InitialPoint();

        // Obtain the collectable game object
        _collectable = GameObject.FindGameObjectWithTag("collectable");

        // Invoke change collecatble position method
        ChangeCollectablePosition();

        // Display the collectable counter
        _counterTextMesh.text = _collectableCounter.ToString();
    }

    /*
     * UPDATE METHOD
     * 
     * Standard Unity method that is invoked
     * once per frame.
     * 
     * Method checks if debug mode is enabled.
     * If true, then allows the collectable
     * position to be changed by pressing the
     * space bar.
     * 
     * Method also carries out a collision check
     * between the player and the collectable.
     * If collision has occured, the collectable
     * position is changed.
     */
    void Update()
    {
        // Check if debug mode is enabled
        if(_debugEnabled == true)
        {
            // Debug mode enabled, check if the space bar has been pressed
            if (Keyboard.current.spaceKey.wasPressedThisFrame == true)
            {
                // Invoke Select Random Point method to choose a new random point
                SelectRandomPoint();

                // Change the position of the collectable
                ChangeCollectablePosition();
            }
        }

        // Check the status of collision check
        if(CollectableCollisionCheck() == true)
        {
            // Invoke Select Random Point method to choose a new random point
            SelectRandomPoint();

            // Change the position of the collectable
            ChangeCollectablePosition();

            // Increment the collectable counter
            _collectableCounter++;

            // Display the updated collectable counter
            _counterTextMesh.text = _collectableCounter.ToString();
        }
    }

    /*
     * INITIAL POINT METHOD
     * 
     * Method is used to set the collectable
     * to the first initial point
     */
    private void InitialPoint()
    {
        // Set current point to the first pick up point
        _currentPoint = _initialPickUpPoint;

        // Set previous point to empty vector
        _previousPoint = Vector3.zero;

        // Check if debug mode is enabled
        if (_debugEnabled == true)
        {
            // Output info to the console
            Debug.Log("First pick up point X: " + _currentPoint.x + " Y: " + _currentPoint.y + " Z: " + _currentPoint.z);
        }
    }

    /*
     * SELECT RANDOM POINT
     * 
     * Method is used to select a random
     * point from the pick up points list.
     * 
     * Will only select a point that isn't
     * the current point the object is at,
     * or at the las point previous to the
     * current point.
     */
    private void SelectRandomPoint()
    {
        // Index value
        int idx;

        // Point changed boolean initialised to false
        bool pointChanged = false;

        // Loop whilst point changed is false
        while(pointChanged == false)
        {
            // Obtain a random index
            idx = Random.Range(0, _pickUpPoints.Count);

            // Check if randomly selected point doesn't equal the previous point
            if (_pickUpPoints[idx] != _currentPoint && _pickUpPoints[idx] != _previousPoint)
            {
                // Set previous point to the current point
                _previousPoint = _currentPoint;

                // Random point is new, set the current point to the new random point
                _currentPoint = _pickUpPoints[idx];

                // Set point changed to true
                pointChanged = true;
            }
        }

        // Check if debug mode is enabled
        if(_debugEnabled == true)
        {
            // Output info to the console
            Debug.Log("Point X: " + _currentPoint.x + " Y: " + _currentPoint.y + " Z: " + _currentPoint.z);
        }
    }

    /*
     * CHANGE COLLECTABLE POSITION METHOD
     * 
     * Method is used to change the position
     * of the collectable object
     */
    public void ChangeCollectablePosition()
    {
        // Change position of the collectable
        _collectable.GetComponent<PickUp>().ChangePosition(_currentPoint);
    }

    /*
     * COLLECTABLE COLLISION CHECK METHOD
     * 
     * Method is used to check if there is a collision
     * between the player and the collectable. Based
     * on simple sphere-sphere collision checking.
     * If collision occurs, returns true. If no collision,
     * return false.
     */
    private bool CollectableCollisionCheck()
    {
        // Determine the centre of the player
        Vector3 playerCentre = _player.transform.position + _player.center;

        // Obtain the player radius
        float playerRadius = _player.radius;

        // Obtain the collectable position
        Vector3 collectablePos = _collectable.GetComponent<PickUp>().CollectablePosition();

        // Obtain the radius of the collectable
        float collectableRadius = _collectable.GetComponent<PickUp>().CollectableRadius();

        // Determine the distance between the player and the collectable
        float distance = Vector3.Distance(playerCentre, collectablePos);

        // Check if the distance between the collectable and player is smaller than the 
        // sum of the collectable and player radii
        if (distance < (collectableRadius + playerRadius) == true)
        {
            // Return true
            return true;
        }

        // Return false
        return false;
    }
}
