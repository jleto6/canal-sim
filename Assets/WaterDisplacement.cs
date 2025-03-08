using UnityEngine;

public class WaterDisplacement : MonoBehaviour
{
    public Collider waterObject; // Assign the water object's collider in the Inspector
    public MeshCollider  boatSizeCollider; // Collider of the child object (e.g., Boat Size)

    private float boatWeight; // Weight of the boat in kg
    private float surfaceArea; // Surface area of the water
    private const float waterDensity = 1010f; // Density of water in kg/m³
    private const float gravity = 9.81f; // Gravity in m/s²
    
    private float lastSubmergedPercentage = -1f; // Initialize to a value that won't match initially
    private float lastWaterRise = -1f; // Initialize to a value that won't match initially


    private void Start()
    {
        // Get surface area of water
        if (waterObject != null)
        {
            // Get the bounds of the water object's collider
            float width = waterObject.bounds.size.x;
            float length = waterObject.bounds.size.z;

            // Calculate and store the surface area
            surfaceArea = width * length;
            Debug.Log($"Surface Area: {surfaceArea} square meters");
        }
        else
        {
            Debug.LogWarning("Water object is not assigned!");
        }



        
    }

    private void Update()
    {
        if (waterObject != null && boatSizeCollider != null)
        {
            // Calculate the percentage of the boat submerged
            float submergedPercentage = CalculateSubmergedPercentage();

            // Print if value has changed
            if (Mathf.Abs(submergedPercentage - lastSubmergedPercentage) > 0.01f) // Check if the value has changed
            //if (submergedPercentage != lastSubmergedPercentage)
            {
                Debug.Log($"Submerged Percentage: {submergedPercentage * 100f}%");
                
                // Calculate the weight of the submerged portion (in kg)
                float weightSubmerged = boatWeight * submergedPercentage;

                // Calculate displaced water volume
                float volumeDisplaced = weightSubmerged / waterDensity;

                // Calculate water level rise
                float waterLevelRise = volumeDisplaced / surfaceArea;

                if (waterLevelRise != lastWaterRise)
                {
                    Debug.Log($"Water Level Rise: {waterLevelRise} meters");
                    lastWaterRise = waterLevelRise;

                    // Check if sumberge increased 
                    if (submergedPercentage > lastSubmergedPercentage){
                        // Scale and move the water object
                        Vector3 waterScale = waterObject.transform.localScale;
                        Vector3 waterPosition = waterObject.transform.position;

                        // Adjust scale and position to simulate water level rise
                        waterScale.y += waterLevelRise;
                        waterPosition.y += waterLevelRise / 2f;

                        waterObject.transform.localScale = waterScale;
                        waterObject.transform.position = waterPosition;
                    }

                    // Check if submerge decreased
                    if (submergedPercentage < lastSubmergedPercentage){
                        // Scale and move the water object
                        Vector3 waterScale = waterObject.transform.localScale;
                        Vector3 waterPosition = waterObject.transform.position;

                        // Adjust scale and position to simulate water level lowering
                        waterScale.y -= waterLevelRise;
                        waterPosition.y -= waterLevelRise / 2f;

                        waterObject.transform.localScale = waterScale;
                        waterObject.transform.position = waterPosition;
                    }
                }

                lastSubmergedPercentage = submergedPercentage; // Update the last value
            }

            
        }
    }

    private float CalculateSubmergedPercentage()
    {
        // Get bounds of the boat size collider and the water collider
        Bounds boatBounds = boatSizeCollider.bounds;
        Bounds waterBounds = waterObject.bounds;

        // Calculate the overlapping bounds (intersection)
        float overlapMinY = Mathf.Max(boatBounds.min.y, waterBounds.min.y);
        float overlapMaxY = Mathf.Min(boatBounds.max.y, waterBounds.max.y);

        if (overlapMaxY > overlapMinY) // Ensure there is overlap
        {
            float overlapHeight = overlapMaxY - overlapMinY;
            float boatHeight = boatBounds.size.y;

            // Calculate the percentage of the boat's height that is submerged
            return overlapHeight / boatHeight;
        }
        return 0f; // No overlap means no submersion
    }
}
