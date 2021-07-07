/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * TELEPORTATION
 * 
 * Teleportation.cs
 * =====================================
 * Script for teleport locomotion in
 * Virtual Reality.
 * 
 * Upon starting the script, the 
 * guide line for teleportation is set
 * up, the teleport target mesh is 
 * hidden from view.
 * 
 * During the scene update the end point
 * for the teleport guide line is updated
 * with every frame. A check is carried out
 * to see if the cooldown timer is greater
 * than 0. If the timer is greater than 0,
 * the cooldown timer is decremented by the 
 * time slice (delta time). A check is 
 * also carried out to determine if the left
 * controller is enabled. If the left 
 * controller is disabled, the teleport
 * guide line and target ring are hidden from
 * view.
 * 
 * The script also contains a fixed update
 * method, to use the Unity physics system
 * to carry out ray collision function coded
 * into the CastRay() function, provided 
 * teleportation is enabled.
 * 
 * Script also contains a method for
 * casting a ray from the left controller
 * to the teleportable ground or an object
 * it has collided with. Depending on the 
 * collision, the method stores the point 
 * of collision that will be used to 
 * teleport the player. 
 */

// Namespaces
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Teleportation : MonoBehaviour
{
    // Controller game object
    [SerializeField] private GameObject _controllerModel;

    // Left Controller
    private LeftController _leftController;

    // Player Rig
    [SerializeField] private XRRig _playerRig;

    // Teleporter guide line
    private LineRenderer _guideLine;

    // Teleport System Enabled boolean
    private bool _teleportSystemEnabled = false;

    // Length of ray - set to 3.0f by default
    [SerializeField] private float _rayLength = 3.0f;

    // Default line colour
    private Color _defaultLineColour = Color.black;

    // Good teleport line colour
    private Color _goodTeleport = Color.green;

    // Bad teleport line colour
    private Color _badTeleport = Color.red;

    // Can teleport boolean
    private bool _canTeleport = false;

    // Teleport cool down amount
    [SerializeField] private float _teleportCooldownAmount = 0.5f;

    // Teleport cool down timer
    private float _teleportCooldownTimer = 0.0f;

    // Target game object
    [SerializeField] private GameObject _targetObject;

    // Target mesh renderer
    private MeshRenderer _targetMeshRenderer;

    // End point of the line
    private Vector3 _endPoint;

    // Teleport point for the player
    private Vector3 _teleportPoint;

    /*
     * START METHOD
     * 
     * Standard Unity method that is invoked
     * before the first frame update
     */
    private void Start()
    {
        // Add a line render game object
        _guideLine = gameObject.AddComponent<LineRenderer>();

        // Set material of the teleport guide line
        _guideLine.material = new Material(Shader.Find("Sprites/Default"));

        // Set the start width of the line
        _guideLine.startWidth = 0.02f;

        // Set the end width of the line
        _guideLine.endWidth = 0.02f;

        // Set line colour
        SetLineColour(_defaultLineColour);

        // Hide the line from view
        _guideLine.enabled = false;

        // Obtain the mesh renderer from the target point
        _targetMeshRenderer = _targetObject.GetComponent<MeshRenderer>();

        // Hide the teleport target
        _targetMeshRenderer.enabled = false;

        // Initialise the teleport point the the player rig position
        _teleportPoint = _playerRig.transform.position;

        // The left controller
        _leftController = FindObjectOfType<LeftController>();
    }

    /*
     * UPDATE METHOD
     * 
     * Standard Unity method that is invoked
     * with every frame.
     * 
     * Method first calculates the end point
     * for a guide line with no collision.
     * 
     * Then checks if the cool down timer is
     * greater than 0. If true, then it 
     * decrements the timer.
     * 
     * Method then checks if the controller
     * has been disabled by the application.
     * If so, the ability to draw lines is
     * removed and the teleport system is
     * deactived from used.
     */
    private void Update()
    {
        // Update the end point based on the forward direction 
        _endPoint = _controllerModel.transform.position + _controllerModel.transform.forward * _rayLength;

        // Check if the cooldown timer is greater than 0
        if (_teleportCooldownTimer > 0.0f)
        {
            // Decrement cooldown timer by delta time
            _teleportCooldownTimer -= Time.deltaTime;
        }

        // Check if the left controller is enabled
        if (_leftController.ControllerEnabled == false)
        {
            // Disable the teleport guide line
            _guideLine.enabled = false;

            // Disable the target mesh
            _targetMeshRenderer.enabled = false;

            // Hide the floor from view
            FindObjectOfType<TeleportFloor>().HideFloor();
        }
    }

    /*
     * FIXED UPDATE METHOD
     * 
     * Standard Unity method. Differs from the
     * update method as it is invoked at a set time
     * interval, rather than every frame.
     * 
     * Used to cast a ray into the scene
     * for teleportation.
     */
    private void FixedUpdate()
    {
        // Check if teleport check is enabled
        if (_teleportSystemEnabled == true)
        {
            // Cast ray from the controller
            CastRay();
        }
        else
        {
            // Hide line
            _guideLine.enabled = false;
        }
    }

    /*
     * CAST RAY METHOD
     * 
     * Method for casting a ray from
     * the left controller.
     * 
     * Method casts a ray from the
     * controllers transform position
     * in the forward direction.
     * 
     * Method then checks for a collision,
     * based on the ray and the defined
     * ray length.
     * 
     * If no collision is found, the guide
     * line from the controller is black,
     * indicating that no collision has 
     * occured.
     * 
     * If a collision has occured, the 
     * line from the controller is drawn 
     * from the controller to the point 
     * of collision and the target object
     * point is moved to the collision 
     * point. The "can teleport" boolean
     * is set to true.
     * 
     * If the collision has occured with
     * the teleportable floor mesh, the
     * line and target object are coloured
     * green and the collision point 
     * coordinates are stored as a point
     * where the player can teleport to.
     * The "can teleport" boolean is set
     * to false.
     * 
     * If the collision has occured with
     * something other than the teleportable
     * floor, returns a red line and guide 
     * ring.
     * 
     * If no collision has occured, a black
     * line to show where the user is aiming
     * is returned.
     */
    public void CastRay()
    {
        // Create a ray from the controller, in the direction the controller is pointing
        Ray ray = new Ray(_controllerModel.transform.position, _controllerModel.transform.forward);

        // Raycast hit object
        RaycastHit aCollision;

        // Check if ray collides with interactive item
        if (Physics.Raycast(ray, out aCollision, _rayLength) == true)
        {
            // Change the line length based on the collision point
            SetLine(aCollision.point);

            // Move the target object to the point of collision
            _targetObject.transform.position = aCollision.point;

            // Rotate the teleport target based on the rotation of the collision point
            _targetObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, aCollision.normal);

            // Enable the teleport guide line
            _guideLine.enabled = true;

            // Enable the target mesh renderer
            _targetMeshRenderer.enabled = true;

            // Check if line first hits the teleportable floor
            if (aCollision.collider.GetComponent<TeleportFloor>())
            {
                // Change the colour to the good teleport colour
                SetLineColour(_goodTeleport);

                // Set the target mesh renderer colour to the good teleport colour
                _targetMeshRenderer.material.color = _goodTeleport;

                // Set can teleport to true
                _canTeleport = true;

                // Set the teleport point to the collision point
                _teleportPoint = aCollision.point;
            }

            // Non-teleport floor hit by ray
            else
            {
                // Change the colour to the bad teleport colour
                SetLineColour(_badTeleport);

                // Set the target mesh renderer colour to the bad teleport colour
                _targetMeshRenderer.material.color = _badTeleport;

                // Set can teleport to false
                _canTeleport = false;

                // Set the teleport point to the player rig position - might be able to get rid of this
                _teleportPoint = _playerRig.transform.position;
            }
        }
        // No hit detected
        else
        {
            // Set the line based on the end point
            SetLine(_endPoint);

            // Set the line colour to default
            SetLineColour(_defaultLineColour);

            // Enable the guide line
            _guideLine.enabled = true;

            // Enable the target mesh renderer
            _targetMeshRenderer.enabled = false;

            // Set can teleport to false
            _canTeleport = false;

            // Set the teleport point to the player rig position - might be able to get rid of this
            _teleportPoint = _playerRig.transform.position;
        }
    }

    /*
     * SET LINE COLOUR METHOD
     * 
     * Method takes in a colour as an argument
     * and is used to set the start and end colours
     * of the teleport guide line
     */
    private void SetLineColour(Color aColour)
    {
        // Set the line colour at the start
        _guideLine.startColor =aColour;

        // Set the line colour at the end
        _guideLine.endColor = aColour;
    }

    /*
     * SET LINE METHOD
     * 
     * Method takes in a vector 3 and is used to
     * set the teleport guide line start point 
     * and end point.
     */
    private void SetLine(Vector3 lineEnd)
    {
        // Set first point of the guide line based on the position of the controller
        _guideLine.SetPosition(0, _controllerModel.transform.position);

        // Set the end point of the guide line based on the value of the argument
        _guideLine.SetPosition(1, lineEnd);
    }

    /*
     * TELEPORT CHECK
     * 
     * Method is used to enable the 
     * teleport check, which in turn
     * begins the script to cast the
     * ray from the controller.
     * 
     */
    public void EnableTeleportSystem()
    {
        // Enable teleport syste
        _teleportSystemEnabled = true;
    }

    /*
     * TELEPORT METHOD
     * 
     * Method is used to move the player from
     * their current position to the value
     * held by the teleport point variable.
     * 
     */
    public void Teleport()
    {
        // Disable the mesh renderer of the teleport target
        _targetMeshRenderer.enabled = false;

        // Check if the player can teleport and if the cool down timer is less than 0
        if (_canTeleport == true && _teleportCooldownTimer <= 0.0f)
        {
            // Move the player rig to the teleport point
            _playerRig.transform.position = _teleportPoint;

            // Set the teleport cooldown timer
            _teleportCooldownTimer = _teleportCooldownAmount;

            // Set can teleport to false
            _canTeleport = false;
        }

        // Disable teleport system
        _teleportSystemEnabled = false;
    }
}
