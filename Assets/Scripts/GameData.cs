using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 2)]
public class GameData : ScriptableObject
{
    [SerializeField] private int _coins;
    [SerializeField] private int _wheatValue;
    [SerializeField] private int _maxStacked;
    public int Coins 
    {
        get => _coins;
        set => _coins = value;
    }

    public int MaxStacked
    {
        get => _maxStacked;
    }

    private int _levelNum;

    private int _wheat = 0;
    public int Wheat
    {
        get => _wheat;
        set => _wheat = value;
    }

    public int WheatValue
    {
        get => _wheatValue;
    }
}
