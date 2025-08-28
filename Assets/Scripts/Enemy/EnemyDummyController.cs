using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDummyController : EnemyBase
{
    public GameObject goModel;
    public HealthBar healthBar;
    public int health;
    private int totalHealth;
    private bool isDead;
    public UnityEvent OnDead;
    public Collider[] colliders;
    public override void Init(int id, MoveType height, bool canChangePatrolPoint, int maxHP, WeaponType weaponType, int damage, float fireRate)
    {
        this.id = id;
        totalHealth = maxHP;
        health = maxHP;
    }

    public override void StartWave(EnemyPoint targetPoint)
    {
        goModel.transform.DOLocalRotate(new Vector3(90, 0, 0), 0.5f).OnComplete(() =>
        {
        
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }
        });
    }
    public void TakeDamageBullet(BoneType boneType,int damage)
    {
        if (isDead) return;
        health = Mathf.Max(0, health - damage);
        int i = Random.Range(0, sfxHitBodys.Length);
        AudioController.Instance.PlaySfx(sfxHitBodys[i]);
        //if (!healthBar.gameObject.activeSelf)
        //{
        //    healthBar.gameObject.SetActive(true);
        //}
        //healthBar.UpdateHealth(damage, (float)health / totalHealth, boneType == BoneType.Head);
        if (health <= 0)
        {
            isDead = true;
            Dead();
            StartCoroutine(DelayActive());
           
        }
    }
    public void TakeDamage( int damage)
    {
        if (isDead) return;
        health = Mathf.Max(0, health - damage);
        int i = Random.Range(0, sfxHitBodys.Length);
        AudioController.Instance.PlaySfx(sfxHitBodys[i]);
        //if (!healthBar.gameObject.activeSelf)
        //{
        //    healthBar.gameObject.SetActive(true);
        //}
        //healthBar.UpdateHealth(damage, (float)health / totalHealth, false);
        if (health <= 0)
        {
            isDead = true;
            Dead();
            StartCoroutine(DelayActive());

        }
    }
    public void Dead()
    {
        for(int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        goModel.transform.DOLocalRotate(Vector3.zero, 0.35f);
        OnDead?.Invoke();
        GameManager.Instance.levelControl.RemoveEnemy(this);
        GameManager.Instance.levelControl.ChangePlayerCoverPoint();
        if (GameManager.Instance.levelControl.GetEnemyCount() == 0)
        {
            StartCoroutine(DelayCheckNextWave());
        }
           
    }
    IEnumerator DelayCheckNextWave()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.levelControl.ActiveNextEnemiesWave();
    }
    IEnumerator DelayActive()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
