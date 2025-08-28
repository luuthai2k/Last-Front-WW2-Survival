using InviGiant.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourceHelper : Singleton<ResourceHelper>
{
    public GameObject[] enemyModel, enemyModels, enemyWeapons, bullets,effects,weatherEffects;
    public GameObject grenadePrefab;
    private void Start()
    {
        Preload();
    }
    public void Preload()
    {
        SmartPool.Instance.Despawn(GetEffect(EffectType.SMGMuzzleFlash, Vector3.zero, Quaternion.identity));
        SmartPool.Instance.Despawn(GetEffect(EffectType.ShotGunMuzzleFlash, Vector3.zero, Quaternion.identity));
        SmartPool.Instance.Despawn(GetEffect(EffectType.MachinegunMuzzleFlash, Vector3.zero, Quaternion.identity));
        GetEffect(EffectType.MetalImpact, Vector3.up * 1000, Quaternion.identity);
        GetEffect(EffectType.WoodImpact, Vector3.up * 1000, Quaternion.identity);
        GetEffect(EffectType.SoilImpact, Vector3.up * 1000, Quaternion.identity);
        GetEffect(EffectType.StoneImpact, Vector3.up * 1000, Quaternion.identity);
    }
    public GameObject GetBullet(WeaponType weaponType)
    {
        return SmartPool.Instance.Spawn(bullets[(int)weaponType],Vector3.zero,Quaternion.identity);
    }
    public GameObject GetBullet(int index)
    {
        return SmartPool.Instance.Spawn(bullets[index], Vector3.zero, Quaternion.identity);
    }
    public GameObject GetBullet(WeaponType weaponType,Vector3 pos,Quaternion rot)
    {
        return SmartPool.Instance.Spawn(bullets[(int)weaponType], pos, rot);
    }
    public GameObject GetEnemyModel(EnemyType enemyType)
    {
        return Instantiate(enemyModel[(int)enemyType], Vector3.zero, Quaternion.identity);
    }
    public GameObject GetEnemyWeapon(WeaponType weaponType,Transform parent)
    {
        return Instantiate(enemyWeapons[(int)weaponType], parent);
    }
    public GameObject GetEffect(EffectType effectType,Vector3 position,Quaternion rotation)
    {
        return SmartPool.Instance.Spawn(effects[(int)effectType], position, rotation);
    }
    public GameObject GetEffect(EffectType effectType, Transform parent, Vector3 position, Quaternion rotation)
    {
        GameObject goEff = SmartPool.Instance.Spawn(effects[(int)effectType], position, rotation);
        goEff.transform.parent = parent;
        return goEff;
    }
    public GameObject GetWeatherEffect(WeatherType weatherType)
    {
        Debug.LogError(weatherEffects[(int)weatherType]);
        return SmartPool.Instance.Spawn(weatherEffects[(int)weatherType], CameraManager.Instance.transform);
    }
    public GameObject GetGrenade(Vector3 position, Quaternion rotation)
    {
        return SmartPool.Instance.Spawn(grenadePrefab, position, rotation);
    }
    public GameObject GetGrenade(Transform parent)
    {
        return SmartPool.Instance.Spawn(grenadePrefab, parent);
    }
}
