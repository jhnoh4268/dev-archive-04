using System.Collections;
using UnityEngine;

public abstract class MonsterSkill : MonoBehaviour
{
    public Monster monster;

    public abstract IEnumerator CastSkill();
}
