/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * PICK UP
 * 
 * PickUp.cs
 * =====================================
 * Script for handling the collectable
 * game object.
 * 
 * When activated, the script first
 * stores the position of the collectable
 * in the virtual space.
 * 
 * The update function updates the 
 * position and rotation of the 
 * collectable item, to give it a floating
 * animation.
 * 
 * Script also contains methods to change
 * the position of the collectable, method 
 * to return the collectable's position 
 * and a method to return the radius of 
 * the collectable item.
 */

// Namespace
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Collectable GameObject
    [SerializeField] private GameObject collectable;

    // Degrees per second
    [SerializeField] private float _degrees = 30.0f;

    // Amplitude
    [SerializeField] private float _amplitude = 0.20f;

    // Frequency
    [SerializeField] private float _frequency = 1.0f;

    // Position vector
    private Vector3 _position;

    // Temporary position vector
    private Vector3 _tempPos = new Vector3();

    // Collision Radius
    [SerializeField] private float _collectableRadius = 1.0f;

    /*
     * START METHOD
     * 
     * Method is invoked before the 
     * first frame update.
     * 
     * Obtains the position of the 
     * collectable.
     */
    void Start()
    {
        // Obtain the initial position of the collectable
        _position = transform.position;
    }

    /*
     * UPDATE METHOD
     * 
     * Method is invoked once every 
     * frame and is used to update
     * the scene.
     * 
     * Method is used to animate the
     * collectable with a floating
     * animation.
     */
    void Update()
    {
        // Rotate the item
        transform.Rotate(new Vector3(0.0f, Time.deltaTime * _degrees, 0.0f), Space.Self);

        // Set the temporary position
        _tempPos = _position;

        // Float object in y axis with a sine wave
        _tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * _frequency) * _amplitude;

        // Set the transform position to the temporary position
        transform.position = _tempPos;
    }

    /*
     * CHANGE POSITION METHOD
     * 
     * Method is used to change the position
     * of the object
     */
    public void ChangePosition(Vector3 newPos)
    {
        // Set the position
        _position = newPos;

        // Set the transform of the object the script is attached to
        transform.position = _position;
    }

    /*
     * COLLECTABLE POSITION METHOD
     * 
     * Method returns the position of the 
     * collectable
     */
    public Vector3 CollectablePosition()
    {
        // Return the position of the collectable
        return transform.position;
    }

    /*
     * COLLECTABLE RADIUS METHOD
     * 
     * Method returns the radius of the 
     * collectable
     */
    public float CollectableRadius()
    {
        // Return the radius of the collectable
        return _collectableRadius; 
    }
}
