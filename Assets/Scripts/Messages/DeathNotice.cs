public class DeathNotice
{
    public ISpaceDestroyable sender { get; private set; }
    public SpaceObjectType objectType { get; private set; }

    public DeathNotice(ISpaceDestroyable sender, SpaceObjectType objectType)
    {
        this.sender = sender;
        this.objectType = objectType;
    }

    public static DeathNotice Create(ISpaceDestroyable sender, SpaceObjectType objectType)
    {
        return new DeathNotice(sender, objectType);
    }
}
