using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public float boatSpeed = 2f; // Speed of the boat's horizontal movement

    private Transform boatTransform;

    private void Start()
    {
        // Get the Transform of the boat object
        boatTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        // Get the current position of the boat
        Vector3 boatPosition = boatTransform.position;

        // Move the boat left or right based on arrow key input
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            boatPosition.x -= boatSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            boatPosition.x += boatSpeed * Time.deltaTime;
        }

        // Apply the updated position back to the Transform
        boatTransform.position = boatPosition;
    }
}
