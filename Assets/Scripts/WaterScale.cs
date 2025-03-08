using UnityEngine;
using UnityEngine.Rendering;

public class WaterScale : MonoBehaviour
{
    private float baseBlockRiseSpeed = 0.017f; // Real-world accurate speed
    private float blockRiseSpeed;
    public bool enteringLake = false; // Set if we are about to enter the lake 
    public bool leavingLake = false; // Set if we are about to leave the lake
    public bool enteringOcean = false; // Set if we are about to enter the ocean


    private bool isBoatColliding = false;

    public Transform currentWaterBlock;
    public Transform currentSpawner;

    public Transform nextWaterBlock;

    private float lastSpeed = 0;

    private void Update()
    {

        


            
        if (lastSpeed != (GlobalVariables.waterSpeed)){
            blockRiseSpeed = baseBlockRiseSpeed * (GlobalVariables.waterSpeed);
            lastSpeed = (GlobalVariables.waterSpeed);
        }

        if (isBoatColliding && !(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {

            Debug.Log(AreSurfacesAtSameLevel());

            if (AreSurfacesAtSameLevel()){
                Debug.Log("at same surface");
                GlobalVariables.stop = false;
            }

            if (!AreSurfacesAtSameLevel() && (!GlobalVariables.isMoving))
            {

                

                if (GlobalVariables.isTriggered == false){

                    if (Input.GetKey(KeyCode.UpArrow) || GlobalVariables.isStarted)
                    {
                        if (currentSpawner != null){
                            // Move the spawner with it
                            float nextWaterSurfaceY = nextWaterBlock.position.y + (nextWaterBlock.localScale.y / 2f);
                                    
                            // Update object's Y position to match the water surface
                            currentSpawner.position = new Vector3(
                            currentSpawner.position.x,  // Keep X position the same
                            nextWaterSurfaceY - 0.5f,     // Set Y position to water surface
                            currentSpawner.position.z   // Keep Z position the same
                        );                        }


                        //CURRENT WATER
                        // Scale and move the block upwards
                        currentWaterBlock.localScale += Vector3.up * blockRiseSpeed * Time.deltaTime;
                        currentWaterBlock.position += Vector3.up * (blockRiseSpeed / 2f) * Time.deltaTime;

                        // NEXT WATER BLOCK - Only move if NOT a lake
                        if (!enteringLake)
                        {
                            nextWaterBlock.localScale -= Vector3.up * blockRiseSpeed * Time.deltaTime;
                            nextWaterBlock.position -= Vector3.up * (blockRiseSpeed / 2f) * Time.deltaTime;
                        }
            
                    }
                // if is triggered
                }else {

                    if (Input.GetKey(KeyCode.DownArrow) || GlobalVariables.isStarted)
                    {
                        // Prevent shrinking below 0
                        if (currentWaterBlock.localScale.y > 0.1f)
                        {

                          if (currentSpawner != null && !leavingLake){
                            // Move the spawner with it
                            float currentWaterSurfaceY = currentWaterBlock.position.y + (currentWaterBlock.localScale.y / 2f);
                                    
                            // Update object's Y position to match the water surface
                            currentSpawner.position = new Vector3(
                            currentSpawner.position.x,  // Keep X position the same
                            currentWaterSurfaceY - 0.55f   ,     // Set Y position to water surface
                            currentSpawner.position.z   // Keep Z position the same
                            );   
                          }
                            

                            //NEXT WATER - Only shrink if not entering an ocean

                            if (!enteringOcean){
                                // Scale and move the block upwards
                                nextWaterBlock.localScale += Vector3.up * blockRiseSpeed * Time.deltaTime;
                                nextWaterBlock.position += Vector3.up * (blockRiseSpeed / 2f) * Time.deltaTime;
                        
                            }
    
                            // CURRENT WATER BLOCK - Only shrink if NOT leaving lake
                            if (!leavingLake )
                            {
                                currentWaterBlock.localScale -= Vector3.up * blockRiseSpeed * Time.deltaTime;
                                currentWaterBlock.position -= Vector3.up * (blockRiseSpeed / 2f) * Time.deltaTime;
                            }
                        
                            
                        }
                    }
                }
                
            } else {
                //Debug.Log("water at same surfcae");
                GlobalVariables.ready = true;
            }
        }
    }

    // Public method to check if surfaces are at the same level
    public bool AreSurfacesAtSameLevel()
    {
        if (currentWaterBlock == null || nextWaterBlock == null)
            return false;
            
        // Calculate top surface of each water block
        float currentWaterSurfaceY = currentWaterBlock.position.y + (currentWaterBlock.localScale.y / 2f);
        float nextWaterSurfaceY = nextWaterBlock.position.y + (nextWaterBlock.localScale.y / 2f);



        // Check if they're within a small threshold of each other
        return Mathf.Abs(currentWaterSurfaceY - nextWaterSurfaceY) < 0.01f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boat")){
            isBoatColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Boat")){
            isBoatColliding = false;
        }
    }
}
