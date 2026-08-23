using UnityEngine;

public class StunStatus : IStatusEffect
{
    private float duration = 2.0f;
    private float elasped = 0.0f;
    public bool isFinished => elasped >= duration;

    public void Apply(Monster target, StatusEffect effect)
    {
        effect.StunEffect.gameObject.SetActive(true);
        target.SetStunned(true);
    }

    public void End(Monster target, StatusEffect effect)
    {
        effect.StunEffect.gameObject.SetActive(false);
        target.SetStunned(false);
    }

    public void Tick(Monster target)
    {
        elasped += Time.deltaTime;
    }
}
