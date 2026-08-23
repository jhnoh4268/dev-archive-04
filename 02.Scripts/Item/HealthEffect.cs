using UnityEngine;

public class HealthEffect : MonoBehaviour, IItemEffect
{
    private bool isPickUp = false;
    public void Initlize()
    {
        isPickUp = false;
    }

    public void OnPickUp(GameObject owner)
    {
        if (isPickUp) return;
        isPickUp = true;

        float healValue = MANAGER.Session.MaxHP * 0.3f;
        MANAGER.Session.HP += healValue;

        var _damagefont = MANAGER.poolManager.PoolingObj("DamageFont").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initalize(
            BaseCanvas.instance.HOLDERLAYER,
            transform.position,
            ((int)healValue).ToString(),
            Color.greenYellow
            );
        });
    }
}
