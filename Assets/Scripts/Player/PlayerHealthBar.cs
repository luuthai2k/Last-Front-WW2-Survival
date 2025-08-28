using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image greenBar;

    public Image redBar;

    public Image hatIcon;

  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHat(float _value)
    {
        greenBar.fillAmount = _value;
    }

    public void UpdateShied(float _value)
    {
        redBar.fillAmount = _value;
    }

}
