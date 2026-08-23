using UnityEngine;

public class ShockStatus : IStatusEffect
{
    private float duration = 3.0f;
    private float elasped = 0.0f;
    private float damageAmp = 0.2f;

    public bool isFinished => elasped >= duration;

    public void Apply(Monster target, StatusEffect effect)
    {
        target.SetShockAmp(damageAmp);
        effect.ShockEffect.gameObject.SetActive(true);
    }

    public void End(Monster target, StatusEffect effect)
    {
        target.SetShockAmp(0.0f);
        effect.ShockEffect.gameObject.SetActive(false);
    }

    public void Tick(Monster target)
    {
        elasped += Time.deltaTime;

    }
}
