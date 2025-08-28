using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    public bool canTakeDame;
    private int maxHealth;
    public float safeTime;
    public void InitHP(int maxHP,float safeTime)
    {
        maxHealth = maxHP;
        this.safeTime = safeTime;
        health = maxHealth;
        GamePlayUIManager.Instance.gamePlayMenu.UpdateHealthText(health);
        isDead = false;
    }
    public override void TakeDamageBullet( int damage)
    {
        //if (!canTakeDame) return;
        if (isDead) return;
        health = Mathf.Max(health-damage,0);
        PlayerController.Instance.GetHit();
        DelayCanTakeDame();
        GamePlayUIManager.Instance.gamePlayMenu.UpdateHealthText(health);
        if (health <= 0)
        {
            PlayerController.Instance.OnDead();
            isDead = true;
        }
        PlayerUIControl.Instance.TakeDameEffect();
        AudioController.Instance.PlaySfx(GameConstain.PLAYERHURT + Random.Range(1, 3).ToString());
        FirebaseServiceController.Instance.LogEvent($"PLAY_DAMAGED_{DataController.Instance.Level}_{GameManager.Instance.levelControl.currentWave}");
     
    }
    public override void TakeDamageFlameThrower(int damage)
    {
        if (isDead) return;
        health = Mathf.Max(health - damage, 0);
        GamePlayUIManager.Instance.gamePlayMenu.UpdateHealthText(health);
        if (health <= 0)
        {
            PlayerController.Instance.OnDead();
            isDead = true;
        }
        PlayerUIControl.Instance.TakeDameEffect();
        AudioController.Instance.PlaySfx(GameConstain.PLAYERHURT + Random.Range(1, 3).ToString());

    }
    public void TakeDamageTank(int damage)
    {
        if (isDead) return;
        health = Mathf.Max(health - damage, 0);
        GamePlayUIManager.Instance.gamePlayMenu.UpdateHealthText(health);
        if (health <= 0)
        {
            PlayerController.Instance.OnDead();
            isDead = true;
        }
        PlayerUIControl.Instance.TakeDameEffect();
        AudioController.Instance.PlaySfx(GameConstain.PLAYERHURT + Random.Range(1, 3).ToString());

    }
    public void Heal()
    {
        health = Mathf.Min(health + 100, maxHealth);
        GamePlayUIManager.Instance.gamePlayMenu.UpdateHealthText(health);
    }
    public void Revive()
    {
        health = 100;
        isDead = false;
        GamePlayUIManager.Instance.gamePlayMenu.UpdateHealthText(health);
    }
    public void DelayCanTakeDame()
    {
        canTakeDame = false;
        GamePlayUIManager.Instance.gamePlayMenu.SetAlarm(true, safeTime, CanTakeDame);
    }
    public void DelayCanTakeDame(float percentage)
    {
        canTakeDame = false;
        GamePlayUIManager.Instance.gamePlayMenu.SetAlarm(true, safeTime * percentage + safeTime, CanTakeDame);
    }
    public void CanTakeDame()
    {
        if (PlayerController.Instance.currentState != PlayerState.Shoot && PlayerController.Instance.currentweaponType != WeaponType.Machinegun) return;
        canTakeDame = true;
    }
    public void CanTakeDame(bool canTakeDame)
    {
        this.canTakeDame = canTakeDame;
        GamePlayUIManager.Instance.gamePlayMenu.SetAlarm(false);
    }
    public string GetProcessHealth()
    {
        return $"{(int)(health/maxHealth)}%";
    }
    public float GetRateHealth()
    {
        return (float)health/maxHealth;
    }
   
}
