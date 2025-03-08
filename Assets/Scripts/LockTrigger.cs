using UnityEngine;

public class LockTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat")) // Change "Player" to whatever tag should trigger it
        {
            GlobalVariables.stop = true;
            //Debug.Log("Trigger Activated Global Variable changed.");
        }
    }
}