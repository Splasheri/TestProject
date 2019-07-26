using UniRx;
using UnityEngine;

public class SpaceObjectView : MonoBehaviour
{
    public ReactiveProperty<bool> isCollided { get; protected set; }
    public SpaceObjectView()
    {
        isCollided = new ReactiveProperty<bool>(false);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollided.Value == false)
        {
            isCollided.Value = true;
        }
    }
    public void CloseFlag()
    {
        isCollided.Value = false;
    }
    public void UpdatePosition(Vector3 pos)
    {
        this.transform.position = pos;
    }
    public void SelfDestruct()
    {
        Object.Destroy(this.gameObject);
    }
}
