using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GestureConfig", menuName = "ScriptableObjects/GestureConfig", order = 1)]
public class GestureConfig : ScriptableObject
{
    public string gestureName;
    public enum GestureType
    {
        Rock,
        Paper,
        Scissors,
        Lizard,
        Spock
    }

    public GestureType gestureType;
    public Sprite gestureSprite;
    public List<GestureType> beats;
}
