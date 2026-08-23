using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    SessionManager session;
    private void Start()
    {
        session = MANAGER.Session;
    }

    public void SetPassiveCard(CardDB db, int level)
    {
        switch(db.className)
        {
            case "Magnet": PASSIVE01(db, level); break;
            case "ATK": PASSIVE02(db, level); break;
            case "EXP": PASSIVE03(db, level); break;
            case "CP": PASSIVE04(db, level); break;
            case "CD": PASSIVE05(db, level); break;
            case "HP": PASSIVE06(db, level); break;
        }
    }

    public void PASSIVE01(CardDB db, int level)
    {
        session.magnetRadiusPercentage = GetIncrementValue(db, level);
    }

    public void PASSIVE02(CardDB db, int level)
    {
        session.damagePercentage = GetIncrementValue(db, level);
    }
    public void PASSIVE03(CardDB db, int level)
    {
        session.expPlusPercentage = GetIncrementValue(db, level);
    }
    public void PASSIVE04(CardDB db, int level)
    {
        session.CriticalPercentage = GetIncrementValue(db, level);
    }

    public void PASSIVE05(CardDB db, int level)
    {
        session.CriticalDamage = GetIncrementValue(db, level);
    }

    public void PASSIVE06(CardDB db, int level)
    {
        float prevMax = session.MaxHP;
        session.HPPercentage = GetIncrementValue(db, level);
        session.RefreshHPbyPercentage(prevMax);
    }

    public float GetIncrementValue(CardDB db, int level)
    {
        return db.baseDamage + db.damagePerLevel * (level - 1);
    }
}
