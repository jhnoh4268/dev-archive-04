using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesign", menuName = "DB/LevelDesign")]
public class LevelDesign : ScriptableObject
{
    [Header("---몬스터 레벨디자인---")]
    [TextArea(0, 3)]
    public string ExplaneMonsterLevelDesign;

    [Range(0, 20)]
    public float baseHP, bossHPMultiplier = 10.0f;
    [Range(0, 1)]
    public float grawRate = 0.25f;
    [Range(0, 5)]
    public float exponent = 2.0f;
    [Range(0, 5)]
    public float monsterSpawnRate = 1.0f;

    [Header("---보스 몬스터 생성주기---")]
    public float BossSpawnRate = 600.0f;

    [Header("---플레이어 경험치 획득량---")]
    public float PlayerExpExponent = 1.5f;

    [Header("---몬스터 드롭 경험치량---")]
    [Range(0, 10)]
    public float MinExp;
    [Range(5, 15)]
    public float MaxExp;

    public float GetHP(float gameTime, bool isBoss)
    {
        float minutes = gameTime / 60.0f;
        float finalHP = baseHP * 1f + grawRate * Mathf.Pow(minutes, exponent);
      
        return isBoss ? finalHP * bossHPMultiplier : finalHP;

    }
}
