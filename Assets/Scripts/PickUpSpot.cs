/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * PICK UP SPOT
 * 
 * PickUpSpot.cs
 * =====================================
 * Script for collectable points in the
 * world.
 * 
 * When activated, the script hides the
 * mesh of the collectable point.
 */

// Namespace
using UnityEngine;

public class PickUpSpot : MonoBehaviour
{
    /*
     * START METHOD
     * 
     * Standard Unity method invoked before
     * the first frame update.
     * 
     * Hides the mesh of the game object
     * it is attached to.
     */
    void Start()
    {
        // Hide the mesh of the pick up spot
        GetComponent<MeshRenderer>().enabled = false;
    }
}
