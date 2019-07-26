using UniRx; 
using UnityEngine;

public class AsteroidView : SpaceObjectView
{
    public new void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollided.Value == false && collision.gameObject.layer == 10)
        { 
            isCollided.Value = true;
        }
    }
}
