using System.Collections.Generic;
using UnityEngine;

public delegate void OnExpChanged(float exp);
public delegate void OnMonsterCountChanged(int value);
public delegate void OnSelectedCard();
public delegate void OnHPChanged(float hp);
public delegate void OnBossTime();

public class SessionManager : MonoBehaviour
{
    public OnExpChanged onExpChanged;
    public OnMonsterCountChanged onMonsterCountChanged;
    public OnSelectedCard onSelectedCard;
    public OnHPChanged onHPChanged;
    public OnBossTime onBossTime;

    public List<Orb> Orbs = new List<Orb>();
    public Dictionary<string, SelectCard> selectedCard = new Dictionary<string, SelectCard> ();

    public int currentWave;
    public int playerLevel;
    public int monsterCount;

    public float enemySpawnRate;
    public float gameTime;
    public float Exp;
    private float BossTime = 10.0f;

    public bool isGameOver = false;

    private float hp = 50;
    public float baseMaxHP;
    public float baseDamage;

    [Space(20)]
    [Header("##Player Data##")]
    public float magnetRadius;
    public float HP
    { 
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHP);
            onHPChanged?.Invoke(hp);
        }
    }
    public float MaxHP => baseMaxHP * (1.0f + HPPercentage / 100.0f);
    public float damage => baseDamage * (1.0f + damagePercentage / 100.0f);

    [Space(20)]
    [Header("##Player PlusData##")]
    public float damagePercentage;
    public float HPPercentage;
    public float magnetRadiusPercentage;
    public float expPlusPercentage;
    public float CriticalPercentage;
    public float CriticalDamage;

    private void Start()
    {
        baseMaxHP = HP;
        BossTime = MANAGER.DB.levelDesign.BossSpawnRate;
        BaseCanvas.instance.HPChanged(HP);
    }
    private void Update()
    {
        gameTime += Time.deltaTime;
        if(gameTime >= BossTime)
        {
            BossTime += MANAGER.DB.levelDesign.BossSpawnRate;
            onBossTime?.Invoke();
        }
    }

    public void SelectedCard(CardDB db)
    {
        if(selectedCard.ContainsKey(db.id))
        {
            var data = selectedCard[db.id];
            data.level++;
        } else
        {
            var selected = new SelectCard();
            selected.db = db;
            selected.level = 1;
            selectedCard.Add(db.id, selected);
        }
        //Debug.Log(db.id + "카드가 선택되었습니다.\n" + selectedCard[db.id].level + "레벨");

        RegisterSkill(db);
    }

    public void RegisterSkill(CardDB db)
    {
        MANAGER.SkillManager.RegisterSkill(db, selectedCard[db.id].level);
        onSelectedCard?.Invoke();
    }

    public void AddMonsterCount()
    {
        monsterCount++;
        onMonsterCountChanged?.Invoke(monsterCount);
    }
    public void RemoveMonsterCount()
    {
        monsterCount--;
        onMonsterCountChanged?.Invoke(monsterCount);
    }

    public void GetDamage(float dmg)
    {
        HP -= dmg;
    }

    public void AddExp(float exp)
    {
        // exp획득증가량 적용
        exp = exp + (exp * MANAGER.Session.expPlusPercentage / 100);

        Exp += exp;
        if(Exp >= GetRequiredExp())
        {
            Exp = 0;
            playerLevel++;

            BaseCanvas.instance.SelectCard();
        }

        onExpChanged?.Invoke(Exp);
    }

    public int GetRequiredExp()
    {

        int level = playerLevel + 1;

        return Mathf.FloorToInt(5 * Mathf.Pow(level, MANAGER.DB.levelDesign.PlayerExpExponent));
        // 1 = 5
        // 5 = 56
        // 50 = 1767

        //if(level < 20)
        //    return (level * 10) - 5;
        //else if(level == 20)
        //    return (level * 10) - 5 + 600;
        //else if(level < 40)
        //    return (level * 13) - 6;
        //else if(level == 40)
        //    return (level * 13) - 6 + 2400;
        //else return (level * 16) - 8;
    }

    public bool GetCritical()
    {
        float RandomValue = Random.value * 100.0f;
        if(RandomValue <= CriticalPercentage) return true;
        return false;
    }

    public void RefreshHPbyPercentage(float oldMaxHP)
    {
        float ratio = MaxHP / oldMaxHP;
        HP += ratio;
    }
}
