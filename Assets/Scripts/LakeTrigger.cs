using UnityEngine;

public class LakeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat")) // Change "Player" to whatever tag should trigger it
        {
            GlobalVariables.isTriggered = true;
            Debug.Log("Trigger Activated! Global Variable changed.");
        }
    }
}
