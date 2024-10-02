using UnityEngine;

public static class GameConstants
{
    // 遊戲設置
    public const int STARTING_LEVEL = 1;
    public const int MAX_LEVELS = 10;
    public const float PLAYER_START_HEALTH = 100f;

    // 殭屍設置
    public const float NORMAL_ZOMBIE_HEALTH = 50f;
    public const float FAST_ZOMBIE_HEALTH = 30f;
    public const float BOSS_ZOMBIE_HEALTH = 200f;

    // 武器設置
    public const float PISTOL_DAMAGE = 10f;
    public const float RIFLE_DAMAGE = 25f;
    public const float SHOTGUN_DAMAGE = 50f;


    // 你可以根據需要添加更多常量...

    public static readonly Vector3 PLAYER_SPAWN_POS = new(710, 15, 650);
    public static readonly Vector3 CENTER = new(760, 15, 650);

    public static readonly Vector3 NORMAL_SPAWN_POS_1 = new(780, 20, 505);
    public static readonly Vector3 NORMAL_SPAWN_POS_2 = new(685, 40, 420);
    public static readonly Vector3 NORMAL_SPAWN_POS_3 = new(560, 5, 450);
    public static readonly Vector3 NORMAL_SPAWN_POS_4 = new(840, 40, 515);
    public static readonly Vector3 NORMAL_SPAWN_POS_5 = new(640, 45, 745);
    public static readonly Vector3 EVOLVED_SPAWN_POS_1 = new(750, 4, 870);
    public static readonly Vector3 EVOLVED_SPAWN_POS_2 = new(740, 3, 470);
    public static readonly Vector3 BOMB_SPAWN_POS_1 = new(800, 3, 700);
    public static readonly Vector3 BOMB_SPAWN_POS_2 = new(800, 3, 600);
    public static readonly Vector3 BOMB_SPAWN_POS_3 = new(600, 3, 650);

    public const float BOUNDARY_MIN_X = 630;
    public const float BOUNDARY_MAX_X = 780;
    public const float BOUNDARY_MIN_Z = 510;
    public const float BOUNDARY_MAX_Z = 790;

    // public static readonly Vector3 SPAWN_POS_5 = new(1000, 25, 555);

    public const float START_TIME = 5f;
    public const float READY_TIME = 21f;
    public const float BATTLE_TIME = 180f;
    public const float BUFFER_TIME = 5f;

    public const int INITIAL_MONEY = 2000;



}
