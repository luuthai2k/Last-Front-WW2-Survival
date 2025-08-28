using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReloadBar : MonoBehaviour
{
    public Image cirBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void UpdateCirBar(float _value)
    {
        cirBar.fillAmount = _value;
    }
}
