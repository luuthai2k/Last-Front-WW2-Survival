using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstain : MonoBehaviour
{
  
    // Scene
    public const string MainMenu = "MainMenu";
    public const string GamePlay = "GamePlay";

    // Anim
    public const string IDLE = "idle";
    public const string SPEED = "speed";
    public const string IS_MOVE = "is_move";
    public const string IS_STOPPING = "is_stopping";
    public const string IS_COVER = "is_cover";
    public const string COVER_DIRECTION = "cover_direction";
    public const string COVER_HEIGHT = "cover_height";
    public const string AIM = "aim";
    public const string RELOAD = "reload";
    public const string RELOAD_TYPE = "reload_type";
    public const string GRENADE = "grenade";   
    public const string GRENADE_CANCER = "grenade_cancer"; 
    public const string IS_THROW = "is_throw";
    public const string WEAPON_TYPE = "weapon_type";
    public const string RUN_HEIGHT = "run_height";
    public const string ALARM = "alarm";
    public const string GET_HIT = "get_hit";
    public const string HIT_TYPE = "hit_type";
    public const string STAND = "stand";
    public const string IS_RECOIL = "is_recoil";
     public const string IS_HOLSTER = "is_holster";
    public const string IS_EQUIP = "is_equip";


    // SFX

    public const string WEAPON_SELECT = "weapon_select";
    public const string PLAYERRELOADSMG = "PlayerReloadSMG";
    public const string PLAYERRELOADMACHINEGUN = "PlayerReloadMachineGun";
    public const string PLAYERRELOADSNIPER = "PlayerReloadSniper";
    public const string PLAYERHURT = "PlayerHurt";
    public const string BULLET_TIME = "bullet_time";
    public const string CHEST_DROP = "ChestDrop";
    public const string CHEST_OPEN = "ChestOpen";
    public const string HEAL = "heal_1";
    public const string ENEMY_DEAD = "EnemyDead";
    public const string HEADSHOT = "HeadShot_1";
    public const string CHESTPROGRESS = "ChestProgress";
    public const string WEAPON_UNLOCK_COUNTER = "weapon_unlock_counter";
    public const string MAINREWARD = "MainReward";
    public const string REWARD_POP = "reward_pop";
    public const string RANK_PICKUP = "rank_pickup";
    public const string BADGE_SHOW = "badge_show";
    public const string KEY_COLLECT = "KeyCollect";
    public const string MISSION_START = "MissionStart";

    // Music

    public const string BGM_MAINMENU = "BGM_mainmenu";

   
    // Int
    public const int MAXLEVEL = 30;
    public const int MAX_HEALTH = 500;
    public const int MAX_HAND_STABILITY = 200;
    public const int MAX_RELOAD_SPEED = 300; 
    public const int MAX_DAMAGE = 600;
    public const int MAX_MAGAZINE = 100;
    public const int MAX_FIRE_RATE = 900;
    public const int MAXBUILD= 4;
    public const int SOLDIER_MAX_LEVEL = 20;
    // PlayerPrefs
    public const string AUTO_SHOW_RATE_POPUP = "Auto_Show_Rate_Popup";
    public const string ENDLESS_OFFER_PROCESS = "Endless_Offer_Process";
    public const string AUTO_SHOW_ENDLESS_OFFER_POPUP = "Auto_Show_Endless_Offer_Popup";
    public const string CLAIM_DAILY_REWARD = "Claim_Daily_Reward";
    // Tutorial
    public const string UNLOCK_SHOTGUN_TUT = "Unlock_Shotgun_tut";
    public const string UPDATE_WEAPON_TUT = "Update_weapon_tut";
    public const string UPDATE_SOLDIER_TUT = "Update_soldier_tut";
    public const string STORE_TUT = "Store_tut";
    public const string BATTLEPASS_TUT = "BattlePass_Tut";
    public const string SNIPER_CONTROL_TUT = "Sniper_Control_Tut";
    public const string MACHINEGUN_CONTROL_TUT = "MachineGun_Control_Tut";
    public const string HEAL_TUT = "Heal_Tut";
}
