/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * MENU INTERACTABLE
 * 
 * MenuInteractable.cs
 * =====================================
 * Script for handling the interactable
 * menu objects.
 * 
 * When activated, the mesh renderer
 * of the game object the script is 
 * attatched to is stored and the 
 * item colour is set to the 
 * default unselected menu colour.
 * 
 * Script contains methods to change
 * the material colour to default,
 * change the material colour to the
 * highlighted colour, and to return
 * the value held by the "scene to 
 * load" string.
 * 
 * Value of the "scene to load" string
 * is set in the editor.
 */

// Namespace
using UnityEngine;

public class MenuInteractable : MonoBehaviour
{
    // Scene to load string
    [SerializeField] private string _sceneToLoad;

    // Frame mesh renderer
    [SerializeField] private MeshRenderer _selectableMesh;

    // Default Colour
    private Color _defaultColour = new Color(0.0f, 0.0f, 0.0f);

    // Highlight Colour
    private Color _highlightColour = new Color(1.0f, 0.6901f, 1.0f);


    /*
     * START METHOD
     * 
     * Standard Unity method that is invoked
     * before the first frame update.
     * 
     * Method first finds the mesh the script is
     * attached to and sets the material colour
     * to a default black colour.
     */
    void Start()
    {
        // Obtain the mesh renderer of the component script is attached to
        _selectableMesh = GetComponent<MeshRenderer>();

        // Change the colour of the highlighted item to the default colour
        ItemDefaultColour();
    }

    /*
     * CHANGE COLOUR METHOD
     * 
     * Method changes the colour of the 
     * frames mesh renderer
     */
    public void ChangeColour(Color aColour)
    {
        // Set the material colour of the mesh to the colour passed through the argument
        _selectableMesh.material.color = aColour;
    }

    /*
     * CHANGE COLOUR METHOD
     * 
     * Method changes the frame mesh 
     * colour to the highlighted colour
     */
    public void ItemHighlightedColour()
    {
        _selectableMesh.material.color = _highlightColour;
    }

    /*
     * CHANGE COLOUR METHOD
     * 
     * Method changes the frame mesh 
     * colour to the default colour
     */
    public void ItemDefaultColour()
    {
        // Set the material colour to the 
        _selectableMesh.material.color = _defaultColour;
    }

    /*
     * SCENE TO LOAD METHOD
     * 
     * Returns the value held by
     * the scene to load string
     */
    public string SceneToLoad
    {
        // Get function
        get
        {
            // Return the value held by the scene to load strin
            return _sceneToLoad;
        }
    }
}
