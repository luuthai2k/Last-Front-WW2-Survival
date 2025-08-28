using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : BaseHealth
{
    public EnemyBase enemyController;
    public HealthBar healthBar;
    protected int totalHealth;
    public void InitHP(int maxHP)
    {
        totalHealth = maxHP;
        health = totalHealth;
        isDead = false;
    }
    public override void TakeDamageBullet(BodyPartControl bodyPart, Vector3 direction, int damage, bool isGetHit = true)
    {
        if (isDead) return;
        health = Mathf.Max(0,health-damage);
        if(isGetHit) enemyController.GetHit(bodyPart.boneType == BoneType.Head?1:0);
        if (!healthBar.gameObject.activeSelf)
        {
            healthBar.gameObject.SetActive(true);
        }
        healthBar.UpdateHealth(damage, (float)health / totalHealth, bodyPart.boneType == BoneType.Head);
        if (health <= 0)
        {
            isDead = true;
            enemyController.Dead(direction.normalized*10);
            GamePlayUIManager.Instance.gamePlayMenu.ShowBadge(BadgeType.Eliminated);
            if (bodyPart.boneType == BoneType.Head)
            {
                GameManager.Instance.headshot++;
                AudioController.Instance.PlaySfx(GameConstain.HEADSHOT);
                GamePlayUIManager.Instance.gamePlayMenu.ShowBadge(BadgeType.HeadShoot);
                MessageManager.Instance.SendMessage(new Message(TeeMessageType.OnHeadShoot, new object[] { enemyController.currentType, PlayerController.Instance.currentweaponType }));
            }
            //else
            //{
            //    GamePlayUIManager.Instance.gamePlayMenu.ShowBadge(BadgeType.Eliminated);
            //}
            StartCoroutine(DelayActive());


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
            StartCoroutine(DelayActive());
            GamePlayUIManager.Instance.gamePlayMenu.ShowBadge(BadgeType.Eliminated);
        }

    } 
    public override void TakeDamagePoison( int damage)
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
            StartCoroutine(DelayActive());
            GamePlayUIManager.Instance.gamePlayMenu.ShowBadge(BadgeType.Eliminated);
        }
    }
    IEnumerator DelayActive()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
