using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected CardDB cardData;

    public string Skillid;
    public float cooldown;
    public float timer;
    public int level;
    protected List<Transform> GetTargetsList(float radius)
    {
        return Player.instance.GetColliderHitMonster(radius);
    }
    public void Initalize(CardDB data, int level)
    {
        cardData = data;
        Skillid = data.id;
        this.level = level;

        cooldown = cardData.baseCooldown - cardData.cooldownPerLevel * (level - 1);
        timer = 0.0f;

        OnInitalize();
}

    public void LevelUp(int newLevel)
    {
        level = newLevel;

        cooldown = cardData.baseCooldown - cardData.cooldownPerLevel * (level - 1);
        OnLevelUp();
    }

    public void Tick()
    {
        timer += Time.deltaTime;

        if(timer > cooldown)
        {
            timer = 0.0f;
            Fire();
        }
    }

    protected float Damage(StatusEffect effect = null)
    {
        float percent = cardData.baseDamage + cardData.damagePerLevel * (level - 1);
        float finalDamage = MANAGER.Session.damage * (percent / 100f);

        if(effect != null)
            SetApplyStatus(effect);
        
        return finalDamage;
    }

    protected void MakeBullet(Vector3 dir)
    {
        var bullet = MANAGER.poolManager.PoolingObj(cardData.name).Get((value) =>
        {
            value.transform.position = Player.instance.transform.position + new Vector3(0f, 0.5f, 0f);
            value.transform.rotation = Quaternion.LookRotation(dir);
            value.GetComponent<Bullet>().Initalize(dir, Damage(), cardData.name, cardData.effects);
        });
    }

    protected Vector3 RandomPos(float radius)
    {
        Vector2 rand = Random.insideUnitCircle * radius;
        Vector3 center = Player.instance.transform.position;

        return new Vector3(center.x + rand.x, center.y, center.z + rand.y);
    }

    protected List<Monster> RangeMonsterList(Vector3 randomPos, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(
            randomPos,
            radius,
            1 << LayerMask.NameToLayer("MONSTER"));

        List<Monster> monsters = new();

        foreach(Collider col in hits)
        {
            if(col.TryGetComponent(out Monster m) && m.isSpawned)
            {
                monsters.Add(m);
            }
        }

        return monsters;
    }

    protected void SetApplyStatus(StatusEffect effect)
    {
        for(int i = 0; i < cardData.effects.Count; i++)
        {
            MANAGER.SkillManager.ApplyStatus(
                cardData.effects[i].status,
                effect,
                cardData.effects[i].value);
        }
    }
    protected abstract void OnInitalize();
    protected abstract void OnLevelUp();
    protected abstract void Fire();
}
