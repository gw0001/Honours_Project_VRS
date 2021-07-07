/* =====================================
 * HONOURS PROJECT
 * GRAEME B. WHITE - 40415739
 * =====================================
 * VIRTUAL NOSE
 * 
 * VirtualNose.cs
 * =====================================
 * Script for the virtual nose 
 * mitigation
 * 
 * When disabled, the virtual nose is
 * hidden.
 * 
 * When enabled, the virtual nose is 
 * visible on the appropriate areas
 * of the left and right eyes of the
 * VR device.
 * 
 * Various variables help to fine tune
 * the nose.
 */

// Directives
using UnityEngine;

public class VirtualNose : MonoBehaviour
{
    // Virtual nose mesh renderer
    private MeshRenderer _virtualNoseMesh;

    // Enable mitigation boolean (initialised to false)
    [SerializeField] private bool _enableMitigation = false;

    // Nose height fine tuning variable
    [SerializeField] private float _fineTuneNoseHeight = -0.0003f;

    // Nose depth fine tuning variable
    [SerializeField] private float _fineTuneNoseDepth = -0.0075f;

    // Nose Width fine tuning variable
    [SerializeField] private float _fineTuneNoseWidth = 0.0202f;

    // Nose Length fine tuning variable
    [SerializeField] private float _fineTuneNoseLength = 0.0211f;

    /*
     * START METHOD
     * 
     * Standard Unity method that is invoked
     * before the first frame.
     * 
     * Obtains the mesh renderer of the virtual
     * nose.
     */
    void Start()
    {
        // Obtain mesh from model in child
        _virtualNoseMesh = gameObject.GetComponentInChildren<MeshRenderer>();
    }


    /*
     * UPDATE METHOD
     * 
     * Standard Unity method that is 
     * invoked at every frame.
     * 
     * First checks if mitigation is
     * enabled. If true, then the mesh
     * renderer is enabled revealing the
     * nose. If false, the nose is
     * hidden from view.
     * 
     * Method then transforms the position
     * and scale of the nose to match the 
     * movement and rotation of the camera
     * alongside the fine tuned values
     */
    void Update()
    {
        // Check if mitigation is enabled
        if(_enableMitigation == true)
        {
            // Mitigation is enabled, mesh renderer enabled
            _virtualNoseMesh.enabled = true;
        }
        else
        {
            // Mitigation disabled, mesh renderer disabled
            _virtualNoseMesh.enabled = false;
        }

        // Transform local position based on the nose height and nose depth
        transform.localPosition = new Vector3(transform.localPosition.x, _fineTuneNoseHeight, _fineTuneNoseDepth);

        // Transform the local scale based on the nose width and the nose length
        transform.localScale = new Vector3(_fineTuneNoseWidth, transform.localScale.y, _fineTuneNoseLength);
    }
}
