using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    { 
        slider.maxValue= health;
        slider.value= health;
    }

    public void setHealth(int health)
    { 
        slider.value = health;
    }
}
