using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    Monster monster;
    List<IStatusEffect> activeEffects = new();
    [HideInInspector]public Renderer renderer;

    public GameObject BurnEffect;
    public GameObject FreezeEffet;
    public GameObject ShockEffect;
    public GameObject StunEffect;

    Coroutine flashCoroutine;

    private bool clearNextFrame;
    public void Initalize()
    {
        StopAllCoroutines();
        clearNextFrame = true;
    }

    private void Start()
    {
        monster = GetComponent<Monster>();
    }
    public void ApplayBurn()
    {
        activeEffects.RemoveAll(e => e is BurnStatus);

        BurnStatus burn = new BurnStatus();
        burn.Apply(monster, this);
        activeEffects.Add(burn);
    }

    public void ApplyFreeze(float stackAmount)
    {
        var freeze = activeEffects.FirstOrDefault(e => e is FreezeStatus) as FreezeStatus;

        if(freeze != null)
        {
            freeze.AddStack(stackAmount);
            freeze.Apply(monster, this);
        } 
        else
        {
            var newFreeze = new FreezeStatus();
            newFreeze.AddStack(stackAmount);
            newFreeze.Apply(monster, this);
            activeEffects.Add(newFreeze);
        }
    }

    public void ApplyShock()
    {
        activeEffects.RemoveAll(e => e is ShockStatus);
        
        ShockStatus shock = new ShockStatus();
        shock.Apply(monster, this);
        activeEffects.Add(shock);
    }

    public void ApplyStune()
    {
        activeEffects.RemoveAll(e => e is StunStatus);

        StunStatus stun = new StunStatus();
        stun.Apply(monster, this);
        activeEffects.Add(stun);
    }

    public void ApplyKnockback(float distance, float duration)
    {
        if(monster.isDead) return;
        Vector3 dir = (transform.position - Player.instance.transform.position).normalized;
        StartCoroutine(KnockbackCoroutine(dir, distance, duration));
    }

    IEnumerator KnockbackCoroutine(Vector3 dir, float distance, float time)
    {
        float elapsed = 0.0f;
        float speed = distance / time;

        Vector3 start = transform.position;
        Vector3 end = start + dir.normalized * distance;

        while(elapsed < time)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, elapsed / time);
            yield return null;
        }
    }

    public void GetHitEffect()
    {
        if(flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashEmission(0.5f));
    }

    IEnumerator FlashEmission(float fadeTime)
    {
        Color flashColor = Color.white * 4;
        renderer.material.SetColor("_EmissionColor", flashColor);

        float timer = 0.0f;

        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            Color current = Color.Lerp(flashColor, Color.black, timer / fadeTime);
            renderer.material.SetColor("_EmissionColor", current);

            yield return null;
        }

        // 원래값으로 초기화
        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    private void Update()
    {
        if(clearNextFrame)
        {
            foreach(var effect in activeEffects)
            {
                effect.End(monster, this);
            }
            activeEffects.Clear();
            renderer.material.SetColor("_EmissionColor", Color.black);
            clearNextFrame = false;
            return;
        }

        for(int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Tick(monster);
            if(activeEffects[i].isFinished || monster.isDead)
            {
                activeEffects[i].End(monster, this);
                activeEffects.RemoveAt(i);
            }
        }
    }
}
