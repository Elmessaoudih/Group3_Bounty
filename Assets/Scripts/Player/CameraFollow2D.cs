using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Camera Settings")]
    // What the camera follows
    public Transform target;
    // How fast the camera follows target,
    // in other words, the fraction of the target and camera's difference the camera will move at a given FixedUpdate()
    public float smoothSpeed = 0.125f;
    // A Vector that holds the height (in its z direction) the camera stays at regardless of where it follows
    public Vector3 offset;
    // A BoxCollider2D that confines the camera's view
    public BoxCollider2D boundary;

    // The Camera component from this gameObject
    private Camera cam;
    // The highest positive x position the camera can see
    private float xBound;
    // The highest positive y position the camera can see
    private float yBound;


    // Initialize cam and calculate bounds when game starts
    private void Start()
    {
        // Get Camera component
        cam = GetComponent<Camera>();

        // Calculate the highest positive x position the camera can see
        xBound = ((boundary.size.x) / 2) - (cam.orthographicSize * cam.aspect);
        // Calculate the highest positive y position the camera can see
        yBound = ((boundary.size.y) / 2) - cam.orthographicSize;
    }

    // Follow the Player (your typical target) every FixedUpdate()
    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        // Guard for when player isnt in scene, if player isn't in Scene, don't bother moving camera
        if (target == null)
            return;

        // Calulate where camera should move
        Vector3 desiredPosition = target.position + offset;

        // Confine desiredPosition to a rectangle (assumes bound in negative directions is symmetrical along origin with positive)
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, -xBound, xBound);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, -yBound, yBound);

        // Smoothly move camera to desiredPosition, smoothedPosition being the next step of the camera's journey for this FixedUpdate()
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}