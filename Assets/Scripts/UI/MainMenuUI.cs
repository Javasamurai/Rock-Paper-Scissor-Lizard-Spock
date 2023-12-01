using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPS
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] public TextMeshProUGUI scoreText;

        private void Awake()
        {
            startButton.onClick.AddListener(StartGame);
            GameManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GAME_STATE obj)
        {
            var visible = obj == GAME_STATE.MENU;
            if (visible)
            {
                scoreText.text = $"Max score: {PlayerPrefs.GetInt("MaxScore")}";
            }
            gameObject.SetActive(visible);
        }

        private void StartGame()
        {
            GameManager.Instance.ChangeGameState(GAME_STATE.AI_TURN);
        }
    }
}