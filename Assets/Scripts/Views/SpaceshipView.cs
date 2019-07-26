using UniRx;
using UnityEngine;

public class SpaceshipView : SpaceObjectView, ISpaceShooter
{
    private GameObject hpHandler;

    private void Start()
    {
        hpHandler = GameObject.Find("HPHandler");
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown("space"))
            .Subscribe(x => {
                SendShootNotice();
            }).AddTo(this);
    }
    public void UpdateHP(int hpLeft)
    {
        hpHandler.transform.GetChild(2 - hpLeft).GetComponent<UnityEngine.UI.RawImage>().color = Color.black;
    }
    public void SendShootNotice()
    {
        MessageBroker.Default
          .Publish(ShootNotice.Create(
              this.transform.position
          ));
    }
    public new void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollided.Value == false && collision.gameObject.layer == 9)
        {
            isCollided.Value = true;
        }
    }
}
