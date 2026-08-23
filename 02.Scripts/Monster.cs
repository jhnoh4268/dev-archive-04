using UnityEngine;

public class Monster : MonoBehaviour
{
    public Animator animator;

    public float HP;
    public float MaxHP;
    protected float skillCooldown = 5.0f;
    protected float skillTimer;

    public string monsterID;

    public bool isDead;
    public bool isSpawned = false;
    public bool isStunned = false;

    public Transform target;
    private IFactory<Monster> factory;

    protected float speedMultiplier = 1.0f;
    protected float shockAmp = 0.0f;
    protected Coroutine SkillCoroutine;
    public MonsterSkill monsterSkill;

    StatusEffect effect;

    public virtual void Initalize(Transform player, string monsterID)
    {
        if(effect == null)
            effect = GetComponent<StatusEffect>();

        effect.Initalize();
        MANAGER.Session.AddMonsterCount();

        MaxHP = MANAGER.DB.levelDesign.GetHP(MANAGER.Session.gameTime, GetIsBoss(monsterID));
        HP = MaxHP;
        
        
        isDead = false;
        isSpawned = false;
        target = player;
         
        this.monsterID = monsterID;
        factory = new GenericPartFactory<Monster>(MANAGER.DB.Monster);
        factory.Build(this, monsterID);
    }

    public void GetDamage(float dmg)
    {
        GetComponent<StatusEffect>().GetHitEffect();

        bool isCritical = MANAGER.Session.GetCritical();
        float criticalDmg = isCritical ? dmg + dmg * MANAGER.Session.CriticalDamage / 100 : dmg;
        
        float finalDmg = criticalDmg * (1 + shockAmp);
        HP -= finalDmg;

        var _damagefont = MANAGER.poolManager.PoolingObj("DamageFont").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initalize(
            BaseCanvas.instance.HOLDERLAYER,
            transform.position,
            ((int)finalDmg).ToString(),
            Color.white,
            isCritical);
        });

        if(HP <= 0)
        {
            //effect.Initalize();
            isSpawned = false;
            isDead = true;

            MANAGER.Session.RemoveMonsterCount();
            var deadEffect = MANAGER.poolManager.PoolingObj("DeadEffect").Get((value) =>
            {
                value.transform.position = transform.position;
            });

            MANAGER.instance.Run(UtilsCoroutine.Delay(1.0f,
                () => MANAGER.poolManager.m_Pool_Dictionary["DeadEffect"].Return(deadEffect)));

            MANAGER.poolManager.m_Pool_Dictionary["Monster"].Return(gameObject);

            int exp = Mathf.FloorToInt(Random.Range(MANAGER.DB.levelDesign.MinExp, MANAGER.DB.levelDesign.MaxExp + 1));
            DropExp(transform.position,
                GetIsBoss(monsterID) ? exp * 10 : exp);
        }
    }

    protected bool GetIsBoss(string monsterID)
    {
        return monsterID.Split("_")[1] == "Boss";
    }

    private void DropExp(Vector3 deathPoint, float exp = 1.0f)
    {
        float[] units = { 9f, 2f, 1f };

        foreach(float unit in units)
        {
            while(exp >= unit)
            {
                exp -= unit;
                OrbMake(deathPoint, unit);
            }
        }

        if(exp > 0.01f)
        {
            OrbMake(deathPoint, exp);
        }
    }

    private void OrbMake(Vector3 deathPoint, float exp)
    {
        Vector3 spawnPos = deathPoint + UtilsWorld.GetRandomCircleOffset(1.5f);
        spawnPos.y += 0.5f;

        var orb = MANAGER.poolManager.PoolingObj("Orb").Get((value) =>
        {
            value.transform.position = transform.position;
            value.GetComponent<Orb>().Initalize(exp, spawnPos);
        });
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = value;
        animator.speed = value;
    }

    public void SetShockAmp(float value)
    {
        shockAmp = value;
    }

    public void SetStunned(bool isStun)
    {
        isStunned = isStun;
        animator.speed = isStun ? 0.0f : 1.0f;
    }
}
