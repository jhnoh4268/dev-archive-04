using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectCard
{
    public CardDB db;
    public int level;
}

[System.Serializable]
public class StatusEffectClass
{
    public Effect_Status status;
    public float value;
}

public enum CardState { Active, Passive, None }

[CreateAssetMenu(fileName = "Scriptable", menuName = "DB/Card", order = int.MaxValue)]
public class CardDB : ScriptableObject
{
    public string id;
    public string className;

    [Space(20)]
    [TextArea(1,10)]
    public string description;
    public CardState state;

    [Space(20)]
    [Header("스킬데이터")]
    public float baseDamage;
    public float baseCooldown;
    public float damagePerLevel;
    public float cooldownPerLevel;

    public List<StatusEffectClass> effects = new();

}
