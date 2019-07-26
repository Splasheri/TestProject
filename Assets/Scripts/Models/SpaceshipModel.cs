using UniRx;
using UnityEngine;

public class SpaceshipModel : SpaceObjectModel, ISpaceHp
{
    public SpaceshipModel(Vector3 pos) : base(pos)
    {
        hp = new ReactiveProperty<int>(3);
    }

    public ReactiveProperty<int> hp { get; private set; }

    public void SendDeathNotice()
    {
        MessageBroker.Default
          .Publish(DeathNotice.Create(
              this,
              SpaceObjectType.spaceship
          ));
    }

    public void LoseHp()
    {
        hp.Value -= 1;
        if (hp.Value == 0)
        {
            SendDeathNotice();
        }
    }

    public override void OutOfBounds(float newX, float newY)
    {
        if (newX < -10.5f) { newX = 10.5f; }
        if (newX > 10.5f) { newX = -10.5f; }
        if (newY > 6.5f) { newY = 6.5f; }
        if (newY < -9) { newY = -9; }
        position.Value = new Vector3(newX, newY, -1);
    }
}
