using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHealth : EnemyHealth
{
    public override void TakeDamageBullet(int damage)
    {
        if (isDead) return;
        health = Mathf.Max(0, health - damage);
        if (!healthBar.gameObject.activeSelf)
        {
            healthBar.gameObject.SetActive(true);
        }
        healthBar.UpdateHealth(damage, (float)health / totalHealth, false);
        if (health <= 0)
        {
            isDead = true;
            enemyController.Dead(Vector3.zero);
            GamePlayUIManager.Instance.gamePlayMenu.ShowBadge(BadgeType.Eliminated);
            GameManager.Instance.headshot++;
        }


    }
    public override void TakeDamageExplosion(Vector3 direction, int damage, float force)
    {
        if (isDead) return;
        health = Mathf.Max(0, health - damage);
        if (!healthBar.gameObject.activeSelf)
        {
            healthBar.gameObject.SetActive(true);
        }
        healthBar.UpdateHealth(damage, (float)health / totalHealth, false);
        if (health <= 0)
        {
            isDead = true;
            enemyController.Dead(direction.normalized * force);
            GamePlayUIManager.Instance.gamePlayMenu.ShowBadge(BadgeType.Eliminated);
        }

    }
}
    
   
