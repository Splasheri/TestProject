using UniRx;
using UnityEngine;

public class BulletView : SpaceObjectView
{
    public new void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollided.Value == false && collision.gameObject.layer == 9)
        {
            isCollided.Value = true;
        }
    }
}
