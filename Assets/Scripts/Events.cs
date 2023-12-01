using System;

namespace RPS
{
    public class Events
    {
        // public const string GAME_START = "GAME_START";
        // public const string ROUND_OVER = "ROUND_OVER";
        // public const string GAME_OVER = "GAME_OVER";
        // public const string GESTURE_SELECTED = "GESTURE_SELECTED";
        public static event Action<GestureConfig.GestureType> GestureSelected;
    }
}