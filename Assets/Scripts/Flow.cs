using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting; // Required for List<T>
using TMPro;
using System;

public class Flow : MonoBehaviour
{
    public WaterScale waterScale;


    public GameObject waterPrefab;       // Assign water droplet object
    public int spawnAmount = 5;          // Number of droplets per spawn
    public float spawnInterval = 0.05f;  // **Decreased spawn interval for smoother flow**
    public float spawnRadius = 0.2f;     // Random spawn offset range
    public float objectLifetime = 1.5f;  // Lifetime before deletion
    public float forceStrength = 2f;     // Initial throw force
    public float destroyDelay = 1f;      // Time before deletion on collision with water

    public List<Transform> spawnPoints;  // List of spawn points (spawns at all)

    private bool isBoatColliding = false;


    private float spawnTimer = 0f;
    private Transform flowParent;        // Reference to "Flow" GameObject
    private bool sameLevel = false; 
    public TextMeshProUGUI waterUsageText; // Set UI text

    public string formatNumber(float n){

        n = (float)Math.Round(n);

        if (n >= 1_000_000_000_000)
            return $"{n / 1_000_000_000_000:F2} trillion";
        else if (n >= 1_000_000_000)
            return $"{n / 1_000_000_000:F2} billion";
        else if (n >= 1_000_000)
            return $"{n / 1_000_000:F2} million";     
        else
            return n.ToString("N0"); // Format with commas
    }
    
    private void Start()
    {

        //Debug.Log("Assigned waterScale: " + waterScale.gameObject.name);

        // Find or create the "Flow" GameObject
        GameObject existingFlow = GameObject.Find("Flow");

        if (existingFlow != null)
        {
            flowParent = existingFlow.transform;
        }
        else
        {
            GameObject newFlow = new GameObject("Flow");
            flowParent = newFlow.transform;
        }
    }

    private void Update()
    {

        GameObject boat = GameObject.FindGameObjectWithTag("Boat");


        BoxCollider waterCollider = null;
        if (!GlobalVariables.isTriggered){
            waterCollider = waterScale.currentWaterBlock.GetComponent<BoxCollider>();
        } else if (GlobalVariables.isTriggered){
            waterCollider = waterScale.currentWaterBlock.GetComponent<BoxCollider>();

        }

        if (waterCollider != null)
        {
            isBoatColliding = waterCollider.bounds.Contains(boat.transform.position);
            if (isBoatColliding)
            {
                //Debug.Log("Boat detected inside: " + waterScale.currentWaterBlock.name);
            }
        }



        if (waterScale.AreSurfacesAtSameLevel())
        {
            sameLevel = true;
            //Debug.Log("Surfaces are at the same level (called from Flow)");
        }

        if (isBoatColliding && !(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))){


            // **Spawn continuously while UpArrow is held and we aren't on the same level 
            if ((GlobalVariables.isStarted || (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))) && !sameLevel && !GlobalVariables.isMoving)
            {

                float waterIncrement = (55586.19f * GlobalVariables.waterSpeed * 2.5f) * Time.deltaTime; 
                GlobalVariables.waterUse += waterIncrement;
                Debug.Log(GlobalVariables.waterUse);

                // Update UI text
                waterUsageText.text = "Water Usage: " + formatNumber(GlobalVariables.waterUse) + " gallons";


                spawnTimer += Time.deltaTime;
                if (spawnTimer >= spawnInterval)
                {
                    SpawnWaterDroplets();
                    spawnTimer = 0f; // Reset timer only AFTER spawning
                }
            }
        }
    }

    private void SpawnWaterDroplets()
    {
        if (spawnPoints == null || spawnPoints.Count == 0) return; // Ensure there are spawn points 

        foreach (Transform spawnPoint in spawnPoints) // Loop through ALL spawn points
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                    UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                    UnityEngine.Random.Range(-spawnRadius, spawnRadius)
                );

                // Instantiate droplet at each spawn point inside "Flow"
                GameObject droplet = Instantiate(waterPrefab, spawnPoint.position + randomOffset, Quaternion.identity, flowParent);

                // Apply force to simulate pressure
                Rigidbody rb = droplet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 forceDirection = spawnPoint.forward + new Vector3(
                        UnityEngine.Random.Range(-0.2f, 0.2f),
                        UnityEngine.Random.Range(0f, 0.3f),
                        0
                    );
                    rb.AddForce(forceDirection * forceStrength, ForceMode.Impulse);
                }

                // ✅ Attach collision handler and pass waterScale
                DropletCollisionHandler handler = droplet.AddComponent<DropletCollisionHandler>();
                handler.waterScale = this.waterScale; // Pass waterScale reference

                Destroy(droplet, 1);
            }
        }

    }

    

    // ✅ Internal class to handle droplet collision with water
    private class DropletCollisionHandler : MonoBehaviour
    {
        public WaterScale waterScale;

        private void OnTriggerEnter(Collider other)
        {
            if (!GlobalVariables.isTriggered){
                
                if (other.gameObject == waterScale.currentWaterBlock.gameObject)
                    {
                        //Debug.Log("Droplet hit water - destroying.");
                        Destroy(gameObject, 0.0f);
                    }
            }
            else{

                if (other.gameObject == waterScale.nextWaterBlock.gameObject)
                    {
                        //Debug.Log("Droplet hit water - destroying.");
                        Destroy(gameObject, 0.0f);
                    }

            }
            
        }
    }

}
