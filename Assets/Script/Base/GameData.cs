using UnityEngine;

[System.Serializable]
public class GameData
{
    public float money;
    public bool firstTime;
    public GameData(float money, bool firstTime)
    {
        this.money = money;
        this.firstTime = firstTime;
    }
}
