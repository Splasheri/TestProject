using UniRx;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public class SpaceObjectModel 
{
    public Vector3ReactiveProperty position { get; private set; }
    public int speed { get; private set; }
    public SpaceObjectModel(Vector3 pos, int speed = 5)
    {
        this.speed = speed;
        this.position = new Vector3ReactiveProperty(pos);
    }
    public void UpdatePosition(float modY, float modX = 0)
    {
        float newX = position.Value.x + modX*speed;
        float newY = position.Value.y + modY*speed;
        if (newX>10.5f | newX<-10.5f | newY>6.5f | newY<-9)
        {
            OutOfBounds(newX, newY);
        }
        else
        {
            position.Value = new Vector3(newX, newY, -1);
        }
    }
    public virtual void OutOfBounds(float newX, float newY)
    {
        position.Value = new Vector3(newX, newY, -1);
    }
    public virtual string Save()
    {
        BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        MemoryStream memStr = new MemoryStream();
        try
        {
            bf.Serialize(memStr, this);
            memStr.Position = 0;

            return Convert.ToBase64String(memStr.ToArray());
        }
        finally
        {
            memStr.Close();
        }
    }
}
