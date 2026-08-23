using UnityEngine;

public interface IFactory<T> where T : Component
{
    void Build(T entity, string name);
}
