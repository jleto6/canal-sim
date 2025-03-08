using UnityEngine;

public class Rendering : MonoBehaviour
{
    public Material targetMaterial; // Assign your particle or water material here
    public int renderQueue = 3001;  // Set the desired Render Queue value

    void Start()
    {
        if (targetMaterial != null)
        {
            targetMaterial.renderQueue = renderQueue;
        }
    }
}
