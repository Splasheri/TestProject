using UniRx;
using UnityEngine;

public interface ISpaceHp : ISpaceDestroyable
{
    ReactiveProperty<int> hp { get; }
    void LoseHp();
}