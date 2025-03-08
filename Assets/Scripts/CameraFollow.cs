using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset1 = new Vector3(15, 5, -12);
    public Vector3 targetRotation1 = new Vector3(18, 45, 0f); // Editable in Inspector

    public Vector3 offset2 = new Vector3(-16, 5, -16);
    public Vector3 targetRotation2 = new Vector3(18f, 40, 0f); // Editable in Inspector
    public float rotationSpeed = 5f; // Smooth rotation speed

    private Vector3 offset;
    private Vector3 targetRotation; 

    void Update()
    {
        if (!GlobalVariables.isTriggered){
            offset = offset1;
            targetRotation = targetRotation1;
        }
        else if (GlobalVariables.isTriggered){
            offset = offset2;
            targetRotation = targetRotation2;

        }

        if (target != null)
        {
            // Smoothly move the camera position
            transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothSpeed * Time.deltaTime);

            // Smoothly interpolate rotation
            Quaternion targetQuat = Quaternion.Euler(targetRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuat, Time.deltaTime * rotationSpeed);
        }
    }
}
