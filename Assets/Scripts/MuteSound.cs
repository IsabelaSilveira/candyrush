using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteSound : MonoBehaviour
{

    bool toggle;
    public void ToggleSound()
     {
         toggle = !toggle;
 
         if (toggle){
             AudioListener.volume = 1f;
         }
         else{
             AudioListener.volume = 0f;
         }
     }
     
     
    public void OnSoundClick() { 
      toggle = false; 
      ToggleSound(); 
    }
    
    public void OffSoundClick() { 
      toggle = true; 
      ToggleSound(); 
    }
}
