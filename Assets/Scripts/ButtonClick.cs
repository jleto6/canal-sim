using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public void PrintMessage(){
        //Debug.Log("Button Clicked!");
        GlobalVariables.isStarted = true;
    }

    public void updateSlider(float value){
        //Debug.Log("Slider Moved To: " + value);
        GlobalVariables.waterSpeed = value*(3f);
        GlobalVariables.boatSpeed = value*(0.7f);
        //Debug.Log(GlobalVariables.speed);
    }

}
