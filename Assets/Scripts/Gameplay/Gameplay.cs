using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace RPS
{
    [Serializable]
    public struct GestureMapping
    {
        public GestureConfig gestureConfig;
        public Button button;
    }

    public class Gameplay : MonoBehaviour
    {
        [SerializeField] public SpriteRenderer AIgestureSprite;
        [SerializeField] public SpriteRenderer playerGestureSprite;
    
        private int score;
        private GestureConfig currentAIgesture;
        private bool gameStarted;

        private void Awake()
        {
            GameManager.OnGameStateChanged += OnGameStateChanged;
            GamePlayUI.OnGestureGenerated += OnOnGestureGenerated;
            GamePlayUI.OnGestureSelected += OnGestureSelected;
        }

        private void OnGestureSelected(GestureConfig.GestureType gestureType)
        {
            Debug.Log($"Selected {gestureType}");
            var playerGesture = GameManager.Instance.GestureConfigs.Find(x => x.gestureType == gestureType);
            playerGestureSprite.sprite = playerGesture.gestureSprite;
            playerGestureSprite.transform.DOMoveY(-3, .5f);
            var winner = CheckWinner(playerGesture, currentAIgesture);
            GameManager.Instance.ChangeGameState(winner ? GAME_STATE.PLAYER_WON : GAME_STATE.PLAYER_LOST);
        }

        private void OnOnGestureGenerated(GestureConfig.GestureType gestureType)
        {
            currentAIgesture = GameManager.Instance.GestureConfigs.Find(x => x.gestureType == gestureType);
            AIgestureSprite.sprite = currentAIgesture.gestureSprite;
            AIgestureSprite.transform.DOMoveY(3, .5f);
        }

        private void OnGameStateChanged(GAME_STATE state)
        {
            if (state == GAME_STATE.MENU)
            {
                ResetGame();
            }
        }

        private void ResetGame()
        {
            AIgestureSprite.sprite = null;
            playerGestureSprite.sprite = null;
            AIgestureSprite.transform.position = Vector3.zero;
            playerGestureSprite.transform.position = Vector3.zero;
            
            AIgestureSprite.transform.position = Vector3.zero;
            playerGestureSprite.transform.position = Vector3.zero;
        }

        private bool CheckWinner(GestureConfig playerGesture, GestureConfig aiGesture)
        {
            var gameManager = GameManager.Instance;
            
            if (playerGesture.beats.Contains(aiGesture.gestureType))
            {
                score++;
                gameManager.ChangeGameState(GAME_STATE.PLAYER_WON);
                StartCoroutine(gameManager.ChangeStateAfterDelay(1f, GAME_STATE.PLAYER_TURN));
                return true;
            }
            if (aiGesture.beats.Contains(playerGesture.gestureType))
            {
                PlayerPrefs.SetInt("MaxScore", score);
                score = 0;
                gameManager.ChangeGameState(GAME_STATE.PLAYER_LOST);
                StartCoroutine(gameManager.ChangeStateAfterDelay(1f, GAME_STATE.MENU));
                return false;
            }
            gameManager.ChangeGameState(GAME_STATE.DRAW);
            StartCoroutine(gameManager.ChangeStateAfterDelay(1f, GAME_STATE.PLAYER_TURN));
            return true;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= OnGameStateChanged;
            GamePlayUI.OnGestureGenerated -= OnOnGestureGenerated;
            GamePlayUI.OnGestureSelected -= OnGestureSelected;
        }
    }
}