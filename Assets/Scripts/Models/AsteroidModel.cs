using UniRx;
using UnityEngine;

public class AsteroidModel : SpaceObjectModel, ISpaceHp
{
    private AsteroidType type;
    public ReactiveProperty<int> hp { get; private set; }

    public AsteroidModel(AsteroidType type, Vector3 pos, int speed = 5) : base(pos, speed)
    {
        hp = new ReactiveProperty<int>((int)type * 2);
        speed = 8 - (int)type;
        this.type = type;
    }

    public void LoseHp()
    {
        hp.Value--;
        if (hp.Value<=0)
        {
            SendDeathNotice();
            MessageBroker.Default
                .Publish<ScoringNotice>(ScoringNotice.Create());
        }
    }

    public void SendDeathNotice()
    {
        MessageBroker.Default
            .Publish<DeathNotice>(DeathNotice.Create(
                this,
                SpaceObjectType.asteroid
            ));
    }

    public override void OutOfBounds(float newX, float newY)
    {
        if (newY < -9-(float)type)
        {
            SendDeathNotice();
        }
        else
        {
            position.Value = new Vector3(newX, newY, -1);
        }
    }

}
