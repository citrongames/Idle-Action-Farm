using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 2)]
public class GameData : ScriptableObject
{
    [SerializeField] private int _coins;
    public int Coins 
    {
        get => _coins;
        set => _coins = value;
    }
    private int _levelNum;
}
