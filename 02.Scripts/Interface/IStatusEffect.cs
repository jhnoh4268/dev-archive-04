using UnityEngine;

public interface IStatusEffect
{
    void Apply(Monster target, StatusEffect effect);
    void Tick(Monster target);
    void End(Monster target, StatusEffect effect);
    bool isFinished { get; }
}
