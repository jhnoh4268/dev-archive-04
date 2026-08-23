using UnityEngine;

public class FreezeStatus : IStatusEffect
{
    public Color originalColor;

    private float freezeStack = 0.0f;
    private float MaxFreezeStack = 1.0f;
    private float slowRate = 0.8f; // 최대감속 60%
    private float slowDuration = 2.5f;
    private float frozenDuration = 2.0f;
    private float slowTimer;

    private float elapsed = 0.0f;
    private bool isFrozen = false;
    private StatusEffect ownerEffect;
    private Monster owner;
    public bool isFinished => isFrozen ? elapsed >= frozenDuration : freezeStack <= 0;

    //public FreezeStatus(float InitaliStack)
    //{
    //    freezeStack = InitaliStack;
    //}

    public void AddStack(float amount)
    {
        freezeStack = Mathf.Clamp01(freezeStack + amount);
    }
    public void Apply(Monster target, StatusEffect effect)
    {
        if(owner == null)
        {
            owner = target;
            ownerEffect = effect;

            originalColor = effect.renderer.material.GetColor("_BaseColor");
            effect.renderer.material.SetColor("_BaseColor", new Color(0.5f, 0.8f, 1.0f, 1.0f));
        }
        slowTimer = 0.0f;

        if(freezeStack >= MaxFreezeStack && !isFrozen)
        {
            isFrozen = true;
            ownerEffect.FreezeEffet.gameObject.SetActive(true);
            elapsed = 0.0f;
            target.SetSpeedMultiplier(0.0f);
        }
        else if(!isFrozen)
        {
            float slowMultiplier = 1.0f - Mathf.Lerp(0.0f, slowRate, freezeStack);
            target.SetSpeedMultiplier(slowMultiplier);
        }
    }

    public void End(Monster target, StatusEffect effect)
    {
        effect.renderer.material.SetColor("_BaseColor", originalColor);
        target.SetSpeedMultiplier(1.0f);
        ownerEffect.FreezeEffet.gameObject.SetActive(false);
    }

    public void Tick(Monster target)
    {
        if(isFrozen)
        {
            elapsed += Time.deltaTime;
        }
        else
        {
            slowTimer += Time.deltaTime;

            if(slowTimer > slowDuration)
            {
                freezeStack = 0.0f;
                target.SetSpeedMultiplier(1.0f);
            }
            else
            {
                float slowMultiplier = 1.0f - Mathf.Lerp(0.0f, slowRate, freezeStack);
                target.SetSpeedMultiplier(slowMultiplier);
            }
        }
    }
}
