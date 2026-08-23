using UnityEngine;

public class BurnStatus : IStatusEffect
{
    private float totalDamage => MANAGER.Session.damage * 0.5f;
    private float duration = 4.0f;
    private float elapsed = 0.0f;
    private float tickInterval = 1.0f;
    private float tickTimer = 0.0f;
    public bool isFinished => elapsed >= duration;

    public void Apply(Monster target, StatusEffect effect)
    {
        // ÀÌÆåÆ®
        effect.BurnEffect.gameObject.SetActive(true);
    }

    public void End(Monster target, StatusEffect effect)
    {
        effect.BurnEffect.gameObject.SetActive(false);
    }

    public void Tick(Monster target)
    {
        elapsed += Time.deltaTime;
        tickTimer += Time.deltaTime;

        if(tickTimer >= tickInterval)
        {
            tickTimer = 0.0f;
            target.GetDamage(totalDamage);
        }

    }
}
