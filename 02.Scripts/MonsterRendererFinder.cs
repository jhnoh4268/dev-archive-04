using System.Threading;
using UnityEngine;

public class MonsterRendererFinder : MonoBehaviour
{
    public Renderer renderer;

    public void Initalize()
    {
        Animator animator = GetComponent<Animator>();
        transform.parent.GetComponent<Monster>().animator = animator;
        StatusEffect effect = transform.parent.GetComponent<StatusEffect>();
        effect.renderer = this.renderer;
    }
}
