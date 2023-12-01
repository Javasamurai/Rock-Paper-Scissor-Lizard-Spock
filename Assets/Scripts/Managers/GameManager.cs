using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPS
{
    public enum GAME_STATE
    {
        MENU,
        AI_TURN,
        PLAYER_TURN,
        DRAW,
        PLAYER_WON,
        PLAYER_LOST
    }

    [DefaultExecutionOrder(100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static event Action<GAME_STATE> OnGameStateChanged;
        public static GAME_STATE CurrentGameState { get; private set; }
        
        [SerializeField] public List<GestureConfig> GestureConfigs;
        [HideInInspector] public float timeRemaining;
        
        // TODO: Move to config
        private const float INPUT_TIME = 1f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            GamePlayUI.OnGestureSelected += GestureSelected;
            ChangeGameState(GAME_STATE.MENU);
        }

        private void GestureSelected(GestureConfig.GestureType obj)
        {
            StopCoroutine(StartTimer());
        }

        public void ChangeGameState(GAME_STATE state)
        {
            OnGameStateChanged?.Invoke(state);
            CurrentGameState = state;
            
            switch (state)
            {
                case GAME_STATE.MENU:
                    timeRemaining = 0;
                    break;
                case GAME_STATE.PLAYER_TURN:
                    StartCoroutine(StartTimer());
                    break;
            }
            Debug.Log($"Changed game state to {state}");
        }

        public IEnumerator ChangeStateAfterDelay(float delay, GAME_STATE state)
        {
            yield return new WaitForSeconds(delay);
            ChangeGameState(state);
        }

        private IEnumerator StartTimer()
        {
            var spentTime = 0f;
            while (spentTime <= INPUT_TIME)
            {
                spentTime += Time.deltaTime;
                timeRemaining = spentTime - INPUT_TIME;
                yield return null;
            }
            ChangeGameState(GAME_STATE.MENU);
        }
    }
}