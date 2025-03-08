using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBouyancy : MonoBehaviour
{
    // How strongly the object pushes up in water
    [Range(1f, 20f)]
    public float buoyancyStrength = 6f;
    
    // How much of the object should stay above water (0-1 percentage)
    [Range(0f, 1f)]
    public float floatPercent = 0.5f;
    
    // How quickly the object stabilizes
    [Range(0.1f, 10f)]
    public float stabilizationStrength = 2f;
    
    // Water resistance
    [Range(0.1f, 5f)]
    public float waterDrag = 1f;
    
    // Tag for water objects
    public string waterTag = "Water";
    
    // Internal variables
    private Rigidbody rb;
    private float objectHeight;
    
    // Track all water triggers we're currently in
    private List<Transform> waterBlocks = new List<Transform>();
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Calculate object height based on collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            objectHeight = col.bounds.size.y;
        }
        else
        {
            objectHeight = transform.localScale.y;
            Debug.LogWarning("No collider found, using transform scale for height calculation");
        }
    }
    
    void FixedUpdate()
    {
        // Reset drag if we're not in any water
        if (waterBlocks.Count == 0)
        {
            rb.linearDamping  = 0.05f;
            rb.angularDamping  = 0.05f;
            return;
        }
        
        // Find the highest water surface among all water blocks we're touching
        float highestWaterSurface = float.MinValue;
        Transform currentHighestWater = null;
        
        foreach (Transform water in waterBlocks)
        {
            if (water == null)
                continue;
                
            float surfaceY = CalculateWaterSurfaceY(water);
            if (surfaceY > highestWaterSurface)
            {
                highestWaterSurface = surfaceY;
                currentHighestWater = water;
            }
        }
        
        // Skip if we somehow don't have valid water
        if (currentHighestWater == null)
            return;
            
        // Calculate where we want the object to float
        float desiredPositionY = highestWaterSurface - (objectHeight * (1f - floatPercent));
        
        // How far is our object from the desired position
        float heightDifference = desiredPositionY - transform.position.y;
        
        // Base force depends on how far we are from ideal position
        float forceAmount = heightDifference * buoyancyStrength;
        
        // Apply vertical buoyancy force
        rb.AddForce(Vector3.up * forceAmount, ForceMode.Acceleration);
        
        // Apply increased drag in water
        rb.linearDamping  = waterDrag;
        rb.angularDamping  = waterDrag;
        
        // Add stabilizing torque to level the object with water surface
        Vector3 rotation = transform.rotation.eulerAngles;
        Vector3 targetRotation = new Vector3(0, rotation.y, 0);
        
        Quaternion targetQuaternion = Quaternion.Euler(targetRotation);
        Quaternion rotationDifference = targetQuaternion * Quaternion.Inverse(transform.rotation);
        
        Vector3 torque = new Vector3(
            rotationDifference.x, 
            rotationDifference.y, 
            rotationDifference.z
        ) * stabilizationStrength;
        
        rb.AddTorque(torque, ForceMode.Acceleration);
    }
    
    // Calculate the top surface Y position of the water block
    private float CalculateWaterSurfaceY(Transform waterBlock)
    {
        if (waterBlock == null)
            return 0f;
            
        // Get the center of the water block
        Vector3 waterCenter = waterBlock.position;
        
        // Get the scale of the water block
        Vector3 waterScale = waterBlock.localScale;
        
        // Calculate the top surface (center Y + half height)
        return waterCenter.y + (waterScale.y / 2f);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(waterTag))
        {
            // Add to our list if not already there
            if (!waterBlocks.Contains(other.transform))
            {
                waterBlocks.Add(other.transform);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(waterTag))
        {
            // Remove from our list
            waterBlocks.Remove(other.transform);
        }
    }
    
    // Optional: clean up null references if water blocks get destroyed
    void LateUpdate()
    {
        for (int i = waterBlocks.Count - 1; i >= 0; i--)
        {
            if (waterBlocks[i] == null)
            {
                waterBlocks.RemoveAt(i);
            }
        }
    }
}