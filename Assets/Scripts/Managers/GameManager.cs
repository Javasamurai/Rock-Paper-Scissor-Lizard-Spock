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
        private float timeRemaining;
        // TODO: Move to config
        private const float INPUT_TIME = 1.5f; // 0.5 seconds extra for animation
        
        private Coroutine _timerCoroutine;
        public float percentageRemaining => timeRemaining / INPUT_TIME;

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
            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);
        }

        public void ChangeGameState(GAME_STATE state)
        {
            CurrentGameState = state;
            OnGameStateChanged?.Invoke(state);
            
            switch (state)
            {
                case GAME_STATE.MENU:
                    timeRemaining = 0;
                    break;
                case GAME_STATE.PLAYER_TURN:
                    _timerCoroutine = StartCoroutine(StartTimer());
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
                timeRemaining = INPUT_TIME - spentTime;
                yield return null;
            }
            ChangeGameState(GAME_STATE.PLAYER_LOST);
            StartCoroutine(ChangeStateAfterDelay(1, GAME_STATE.MENU));
        }
    }
}