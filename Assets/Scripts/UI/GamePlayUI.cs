using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPS
{
    public class GamePlayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI result;
        [SerializeField] public Transform buttonContainer;
        [SerializeField] public Image timerBarSprite;
        [SerializeField] public Button buttonPrefab;

        public static event Action<GestureConfig.GestureType> OnGestureGenerated;
        public static event Action<GestureConfig.GestureType> OnGestureSelected;
        private List<GestureConfig> GestureConfigs => GameManager.Instance.GestureConfigs;
        private bool buttonsCreated;

        private void Awake()
        {
            GameManager.OnGameStateChanged += OnGameStateChanged;
        }
        
        private void SelectGesture(GestureConfig config)
        {
            OnGestureSelected?.Invoke(config.gestureType);
        }

        private GestureConfig RandomGesture()
        {
            var randomIndex = UnityEngine.Random.Range(0, GestureConfigs.Count);
            return GestureConfigs[randomIndex];
        }

        private void Update()
        {
            if (GameManager.CurrentGameState == GAME_STATE.PLAYER_TURN)
            {
                timerBarSprite.fillAmount = GameManager.Instance.percentageRemaining;
            }
        }

        private void OnGameStateChanged(GAME_STATE state)
        {
            var visible = state != GAME_STATE.MENU;
            gameObject.SetActive(visible);
            
            if (!buttonsCreated && visible)
            {
                foreach (var gestureConfig in GestureConfigs)
                {
                    var button = Instantiate(buttonPrefab, buttonContainer);
                    button.GetComponent<Image>().sprite = gestureConfig.gestureSprite;
                    var config = gestureConfig;
                    button.onClick.AddListener(() => SelectGesture(config));
                }
                buttonsCreated = true;
            }

            switch (state)
            {
                case GAME_STATE.AI_TURN:
                    timerBarSprite.fillAmount = 0;
                    var aiGesture = RandomGesture();
                    GameManager.Instance.ChangeGameState(GAME_STATE.PLAYER_TURN);
                    OnGestureGenerated?.Invoke(aiGesture.gestureType);
                    break;
                case GAME_STATE.PLAYER_WON:
                    result.text = "You won!";
                    break;
                case GAME_STATE.PLAYER_LOST:
                    result.text = "You lost!";
                    break;
                case GAME_STATE.DRAW:
                    result.text = "Draw!";
                    break;
                default:
                    result.text = string.Empty;
                    break;
            }
            buttonContainer.gameObject.SetActive(state != GAME_STATE.PLAYER_LOST && state != GAME_STATE.PLAYER_WON &&
                state != GAME_STATE.DRAW);
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}