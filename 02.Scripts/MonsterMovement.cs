using System.Collections;
using UnityEngine;

public class MonsterMovement : Monster
{
    private Rigidbody rb;

    public float moveSpeed = 3.0f;

    private Vector3 direction;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void Initalize(Transform player, string monsterID)
    {
        base.Initalize(player, monsterID);
        GetDirection();
        Rotate(direction, false);
        isSpawned = false;

        bool isBoss = GetIsBoss(monsterID);
        float scale = isBoss ? 25.0f : 15.0f;
        moveSpeed = isBoss ? 5.0f : 3.0f;

        if(isBoss)
        {
            monsterSkill = GetComponentInChildren<MonsterSkill>();
            monsterSkill.monster = this;
        }

        StopAllCoroutines();
        StartCoroutine(SpawnStartCouroutine(new Vector3(scale, scale, scale)));
    }

    IEnumerator SpawnStartCouroutine(Vector3 finalScale)
    {
        Rotate(direction);

        Vector3 scaleStart = Vector3.zero;
        Vector3 scaleEnd = finalScale;

        float duration = 0.5f;
        float timer = 0.0f;

        while(timer < duration)
        {
            float t = timer / duration;
            transform.localScale = Vector3.Lerp(scaleStart, scaleEnd, t);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = scaleEnd;

        animator.SetTrigger("MOVE");
        isSpawned = true;
    }

    private void FixedUpdate()
    {
        if(target == null || !isSpawned || isDead || isStunned)
        {
            return;
        }

        if(GetIsBoss(monsterID) && monsterSkill != null)
        {
            skillTimer += Time.fixedDeltaTime;
            if(skillTimer > skillCooldown && SkillCoroutine == null)
            {
                SkillCoroutine = StartCoroutine(nameof(CaseBossSkill));
                skillTimer = 0f;
            }
        }

        GetDirection();
        if(SkillCoroutine == null) 
            MoveAndRotate();
    }

    IEnumerator CaseBossSkill()
    {
        animator.SetTrigger("MAGIC");

        yield return monsterSkill.CastSkill();

        skillTimer = 0.0f;
        SkillCoroutine = null;
    }

    void MoveAndRotate()
    {
        Rotate(direction);
        rb.MovePosition(rb.position + direction * moveSpeed * speedMultiplier * Time.fixedDeltaTime);
        
    }
    
    void GetDirection()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;
        direction = dir;
    }

    void Rotate(Vector3 direction, bool useLerp = true)
    {
        if(direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            if(useLerp)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 10.0f * Time.deltaTime);
            }
            else
            {
                transform.rotation = targetRot;
            }
        }
    }
}
