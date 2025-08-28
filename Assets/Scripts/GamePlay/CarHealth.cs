using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHealth : BaseHealth
{
    public Car car;
    public HealthBar healthBar;
    public GameObject fireEffect;
    private int health;
    public int totalHealth;
    private void Start()
    {
        health = totalHealth;
    }
    public override void TakeDamageBullet(int damage)
    {
        if (isDead)
            return;
        health -= damage;
        if (health<=totalHealth*0.5f&&!fireEffect.activeSelf)
        {
            fireEffect.SetActive(true);
            StartCoroutine(DelayFire());
        }
        if (!healthBar.gameObject.activeSelf)
        {
            healthBar.gameObject.SetActive(true);
        }
        healthBar.UpdateHealth(damage,(float)health / totalHealth,false);
        if (health <= 0)
        {
            isDead = true;
            fireEffect.SetActive(false);
            car.Destruction();
        }
    }
   
    IEnumerator DelayFire()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1);
            TakeDamageBullet(20);
        }
    }
}
