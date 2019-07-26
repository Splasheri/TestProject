using UniRx;
using UnityEngine;

public class BulletModel : SpaceObjectModel, ISpaceDestroyable
{
    public BulletModel(Vector3 pos, int speed = 7) : base(pos, speed)
    {
    }
    public override void OutOfBounds(float newX, float newY)
    {
        if (newY >9 )
        {
            SendDeathNotice();
        }
        else
        {
            position.Value = new Vector3(newX, newY, -1);
        }
    }
    public void SendDeathNotice()
    {
        MessageBroker.Default
            .Publish<DeathNotice>(DeathNotice.Create(
                this,
                SpaceObjectType.bullet
            ));
    }
}
