using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public static Player instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [NonSerialized]public PlayerMovement movement;
    [NonSerialized]public PlayerAttacker attacker;

    public float detectionRadius = 10.0f;
    public LayerMask monsterLayer;
    public List<Transform> targets = new List<Transform>();
    public Renderer[] renderers;

    bool isHit = false;

    [SerializeField] Volume volume;
    private Vignette vignette;
    [SerializeField] Color vignetteColor;

    public Transform target
    {
        get { return GetNearestMonster(); }
    }

    public Vector3 GetDirectionToMonster()
    {
        Vector3 dirToMonster = (target.position - transform.position).normalized;
        return dirToMonster;
    }

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        attacker = GetComponent<PlayerAttacker>();

        volume.profile.TryGet(out vignette);
    }

    public Transform GetNearestMonster()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayer);
        Transform nearset = null;
        float minDistance = Mathf.Infinity;

        targets.Clear();

        foreach(Collider col in hits)
        {
            if(!col.GetComponent<Monster>().isSpawned)
                continue;
            targets.Add(col.transform);

            float distance = Vector3.Distance(transform.position, col.transform.position);
            if(minDistance > distance)
            {
                nearset = col.transform;
                minDistance = distance;
            }
        }
        return nearset;
    }
    public List<Transform> GetColliderHitMonster(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, monsterLayer);

        List<Transform> targetsList = new();

        foreach(Collider col in hits)
        {
            if(!col.GetComponent<Monster>().isSpawned) continue;
            targetsList.Add(col.transform);
        }
        return targetsList;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("MONSTER"))
        {
            if(!isHit)
                GetDamage(10);
        }
    }

    public void GetDamage(float dmg)
    {
        isHit = true;
        StartCoroutine(FlashEmission(0.5f));
        StartCoroutine(CameraShake(Camera.main.transform, 0.1f, 0.05f));

        var _damagefont = MANAGER.poolManager.PoolingObj("DamageFont").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initalize(
            BaseCanvas.instance.HOLDERLAYER,
            transform.position,
            ((int)dmg).ToString(),
            Color.red
            );
        });

        MANAGER.Session.GetDamage(dmg);
    }

    IEnumerator FlashEmission(float fadeTime)
    {

        Color flashColor = Color.white * 4;
        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.SetColor("_EmissionColor", flashColor);
        }

        float timer = 0.0f;
        
        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            Color current = Color.Lerp(flashColor, Color.black, timer / fadeTime);
            for(int i = 0; i < renderers.Length; i++)
                renderers[i].material.SetColor("_EmissionColor", current);
            
            yield return null;
        }

        // 원래값으로 초기화
        for(int i = 0; i < renderers.Length; i++)
            renderers[i].material.SetColor("_EmissionColor", Color.black);

        isHit = false;
    }

    IEnumerator VignettePulse()
    {
        float t = 0.0f;
        float duration = 0.15f;
        float maxIntencity = 0.4f;

        vignette.color.value = vignetteColor;

        while(t < duration)
        {
            t += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0, maxIntencity, t / duration);

            yield return null;
        }

        t = 0.0f;

        while(t < duration)
        {
            t += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(maxIntencity, 0, t / duration);

            yield return null;
        }

        vignette.intensity.value = 0.0f;
    }

    IEnumerator CameraShake(Transform camTransform, float duration, float strength)
    {
        Vector3 originalPos = camTransform.localPosition;
        float timer = 0.0f;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            float offSetX = Random.Range(-1.0f, 1.0f) * strength;
            float offSetY = Random.Range(-1.0f, 1.0f) * strength;

            camTransform.localPosition = originalPos + new Vector3(offSetX, offSetY, 0);

            yield return null;
        }

        camTransform.localPosition = originalPos;
    }
}
