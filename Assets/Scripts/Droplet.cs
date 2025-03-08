using UnityEngine;

public class Droplet : MonoBehaviour
{
    public WaterScale waterScale; // Assigned when the droplet is created

    private void OnTriggerEnter(Collider other)
    {
        if (waterScale != null && other.gameObject == waterScale.currentWaterBlock.gameObject)
        {
            //Debug.Log("Droplet hit water - destroying.");
            Destroy(gameObject, 0.05f);
        }
    }
}
