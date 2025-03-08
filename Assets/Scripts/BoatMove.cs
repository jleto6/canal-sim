using UnityEngine;

public class BoatMove : MonoBehaviour
{
    private float baseSpeed = 0.0041f; // real-world speed
    private Vector3 targetPosition; // the target position the boat will move towards
    private float dampingFactor = 4f; //reduce momentum when stopping

    private float accelerationFactor = 3f; //  smoother start
    private float currentSpeed = 0f; // Gradually increase this
    private float moveSpeed;

    private void Start()
    {
        targetPosition = transform.position; // Start with the boat's current position 
    }

    private float lastSpeed = 0;

    private void Update()
    {
        if (lastSpeed != GlobalVariables.boatSpeed)
        {
            moveSpeed = baseSpeed * GlobalVariables.boatSpeed;
            lastSpeed = GlobalVariables.boatSpeed;
        }

        // If ready and simulation is active
        if (GlobalVariables.stop)
        {
            GlobalVariables.isMoving = false;
        }
        else if (GlobalVariables.ready && GlobalVariables.isStarted && !GlobalVariables.stop)
        {
            GlobalVariables.isMoving = true;
            targetPosition = transform.position - (Vector3.right * 8 * (GlobalVariables.boatSpeed * 5));
        }

        // Smooth acceleration when movement starts
        if (GlobalVariables.isMoving)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, accelerationFactor * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, dampingFactor * Time.deltaTime);
        }

        // Move the boat
        transform.position = Vector3.Lerp(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }
}
