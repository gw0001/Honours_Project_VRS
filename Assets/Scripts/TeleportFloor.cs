/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * TELEPORT FLOOR
 * 
 * TeleportFloor.cs
 * =====================================
 * Simple script to handle the 
 * visibility of the teleportation 
 * floor. 
 */

// Namespace
using UnityEngine;

public class TeleportFloor : MonoBehaviour
{
    // Target mesh renderer
    [SerializeField] private MeshRenderer _teleportFloorMeshRenderer;

    /*
     * ON ENABLE METHOD
     * 
     * Method is invoked when the script is 
     * enabled.
     * 
     * Begins by obtaining the mesh renderer
     * of the teleportable floor, then disables
     * the mesh renderer to hide the floor from
     * view.
     */
    private void Start()
    {
        // Obtain teleport floor mesh renderer
        _teleportFloorMeshRenderer = gameObject.GetComponent<MeshRenderer>();

        // Disable the mesh renderer for the teleport floor
        _teleportFloorMeshRenderer.enabled = false;
    }

    /*
     * VIEW FLOOR METHOD
     * 
     * Method is used to make the
     * floor visible
     */
    public void ViewFloor()
    {
        // View the floor
        _teleportFloorMeshRenderer.enabled = true;
    }

    /*
     * HIDE FLOOR METHOD
     * 
     * Method is used to make the
     * floor invisible
     */
    public void HideFloor()
    {
        // Hide the floor
        _teleportFloorMeshRenderer.enabled = false;
    }
}
