using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyReloadBar : MonoBehaviour
{
    private float timerMax;

    private float timer;

    public Image redCir;

    private void OnEnable()
    {
       
    }

    public void Init(float _time)
    {
        timer = 0.0f;
        timerMax = _time;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, CameraManager.Instance.transform.rotation.y, 0);
        timer += Time.deltaTime;
        redCir.fillAmount = timer / timerMax;
        if(timer >= timerMax)
        {
            timer = 0.0f;
            gameObject.SetActive(false);
        }
    }

}
