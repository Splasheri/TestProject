using UnityEngine;

public class ShootNotice
{
    public Vector3 startPosition { get; private set;}

    public ShootNotice(Vector3 position)
    {
        startPosition = position;
    }

    public static ShootNotice Create(Vector3 position)
    {
        return new ShootNotice(position);
    }
}
