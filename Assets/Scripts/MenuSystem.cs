/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * MENU SYSTEM
 * 
 * MenuSystem.cs
 * =====================================
 * Script for allowing the player to 
 * interact with the items of the main
 * menu of the VR application with the
 * right controller.
 */

// Namespace
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    // Right controller
    [SerializeField] private RightController _rightController;

    // Controller model
    [SerializeField] private MeshRenderer _controllerModel;

    // Menu target ring
    [SerializeField] private GameObject _menuTargetRing;

    // List of pickup points
    private MenuInteractable[] _interactables;

    // Menu target
    private MeshRenderer _menuTargetMesh;

    // The Game Manager
    [SerializeField] private GameManager _theGM;

    // Default line colour
    private Color _defaultColour = new Color(0.0f, 0.6901f, 1.0f);

    // Scene to load string
    [SerializeField] private string _sceneToLoad = null;

    // Selection line
    private LineRenderer _selectionLine;

    // Selection line length - set to 15.0f
    private float _selectionLineLength = 15.0f;

    /*
     * START METHOD
     * 
     * Standard Unity method that is
     * invoked before the first frame update.
     * 
     * Method sets up the guide line for 
     * menu interaction by setting width,
     * colour, and the menu target mesh.
     */
    void Start()
    {
        // Add a line render game object
        _selectionLine = gameObject.AddComponent<LineRenderer>();

        // Set material of the teleport guide line
        _selectionLine.material = new Material(Shader.Find("Sprites/Default"));

        // Set the start width of the line
        _selectionLine.startWidth = 0.02f;

        // Set the end width of the line
        _selectionLine.endWidth = 0.02f;

        // Set line colour
        SetLineColour(_defaultColour);

        // Obtain the mesh renderer from the target point
        _menuTargetMesh = _menuTargetRing.GetComponent<MeshRenderer>();

        // Find objects of Menu Interactable type and store them in the interactables array
        _interactables = FindObjectsOfType<MenuInteractable>();
    }

    /*
     * AWAKE METHOD
     * 
     * Standard Unity function that invokes when the
     * script is woken up
     */
    private void Awake()
    {
        // Find the game manager
        _theGM = FindObjectOfType<GameManager>();

        // Find the right controller script
        _rightController = FindObjectOfType<RightController>();
    }

    /*
     * FIXED UPDATE METHOD
     * 
     * Standard Unity method. Unlike the 
     * Update method, this method is invoked
     * at a set time interval
     */
    void FixedUpdate()
    {
        // Cast Selection Ray
        CastSelectionRay();
    }

    /*
     * CAST SELECTION RAY METHOD
     * 
     * Method for casting the ray from the 
     * right controller to allow the 
     * user to select a menu item from the 
     * main menu.
     * 
     * Method first casts a ray from the right
     * controller, based on the transform position
     * of the controller. The ray is directed towards 
     * the forward vector of the controller.
     * 
     * Method then checks for a ray collision. 
     * If a collision occurs, the selection line
     * is drawn from the controller to the 
     * point of collision. A selection target model
     * is also set to the position of the collision
     * point and the rotation is adjusted based on
     * the normal from the collision point.
     * 
     * If the collision has occured with a menu item,
     * the menu item is highlighted and the name of the
     * scene to load is obtained from the menu interactable.
     * 
     * If the collision has occured with anything other
     * than the menu interactable, then the scene to load
     * is set to null.
     * 
     * Script also contains a method used to change scene,
     * based on the value held by the "scene to load" string.
     */
    private void CastSelectionRay()
    {
        // Create a ray from user's perspective
        Ray ray = new Ray(_controllerModel.transform.position, _controllerModel.transform.forward);

        // Raycast hit object
        RaycastHit aCollision;

        // Check a collision exists
        if (Physics.Raycast(ray, out aCollision, _selectionLineLength) == true)
        {
            // Set line to start from right controller to the collision point
            SetLine(aCollision.point);

            // Move the target menu object point to the collision point
            _menuTargetRing.transform.position = aCollision.point;

            // Change the rotation of the menu target object based on the normal the collision hits
            _menuTargetRing.transform.rotation = Quaternion.FromToRotation(Vector3.up, aCollision.normal);

            // Set the menu target mesh to the highlight colour
            _menuTargetMesh.material.color = _defaultColour;

            // Check if the collision has happened with a menu interactable
            if (aCollision.collider.GetComponent<MenuInteractable>())
            {
                // Obtain Interactive Item based on collision and store in Temporary Interactive Item object
                MenuInteractable menuItem = aCollision.collider.GetComponent<MenuInteractable>();

                // Enable the highlight colour for the highlighted menu interabtable
                menuItem.ItemHighlightedColour();

                // Set the scene to load string, based on the string from the menu interactable
                _sceneToLoad = menuItem.SceneToLoad;
            }
            // No collision with menu interactable
            else
            {
                // Set scene to load as null
                _sceneToLoad = null;

                // Deactivate all menu items
                DeactivateAllMenuItems();
            }
        }
    }

    /*
     * SET LINE COLOUR METHOD
     * 
     * Method is used to set the colour 
     * of the menu selection line
     */
    private void SetLineColour(Color aColour)
    {
        // Set the line colour at the start
        _selectionLine.startColor = aColour;

        // Set the line colour at the end
        _selectionLine.endColor = aColour;
    }

    /*
     * LOAD SCENE METHOD
     * 
     * Method used to load the scene
     * based on the item stored in the 
     * string
     */
    public void LoadScene()
    {
        // Check if the scene to load string is not null
        if(_sceneToLoad != null)
        {
            // Check if the scene to load string is set to "QUIT"
            if(_sceneToLoad == "QUIT")
            {
                // Quit the application
                Application.Quit();
            }
            else
            {
                // Have the game manager load the scene based on the value of the scene to load string
                _theGM.sceneSelection(_sceneToLoad);
            }
        }
    }

    /*
     * SET LINE METHOD
     * 
     * Method is used to set the line
     * position from the transform position 
     * of the controller, to a point passed 
     * through from the argument.
     */
    private void SetLine(Vector3 lineEnd)
    {
        // Set first point of the guide line based on the position of the controller
        _selectionLine.SetPosition(0, _controllerModel.transform.position);

        // Set the end point of the guide line based on the value of the argument
        _selectionLine.SetPosition(1, lineEnd);
    }

    /*
     * DEACTIVATE ALL MENU ITEMS METHOD
     * 
     * Method is used to set the colour
     * of all menu items to the default
     * colour (black).
     */
    private void DeactivateAllMenuItems()
    {
        // Iterate over all interactables in the interactable array
        foreach(MenuInteractable interactable in _interactables)
        {
            // Set the interactable item to the default colour
            interactable.ItemDefaultColour();
        }
    }
}
