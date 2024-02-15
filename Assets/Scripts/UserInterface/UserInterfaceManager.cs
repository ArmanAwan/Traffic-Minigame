using System;
using System.Globalization;
using Jam.Mechanics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Jam.Mechanics.Score;

namespace Jam.UserInterface
{
    public class UserInterfaceManager : MonoBehaviour
    {

        [SerializeField]
        private GameManager _gameManager;
        private GameManager GameManager => _gameManager;

        [SerializeField]
        private TMP_Text _mainText;
        private TMP_Text MainText => _mainText;
        [SerializeField]
        private TMP_Text _scoreText;
        private TMP_Text ScoreText => _scoreText;

        private enum CanvasName { Start, InLevel, End}
        [SerializeField]
        private Button _playButton;
        private Button PlayButton => _playButton;
        
        private void Start() =>
            CriticalLoad();
        private void CriticalLoad()
        {
            PlayButton.onClick.AddListener(GameManager.LevelStart);
            PlayButton.onClick.AddListener(LevelStart);
            GameManager.LevelEndEvent += LevelEnd;
            MoneyManager.UpdateScoreEvent += ScoreUpdated;
            SwitchCanvas(CanvasName.Start);
        }

        private void Update()
        {
            if(!GameManager.GameIsRunning) return;
            MainText.text = MathF.Ceiling(GameManager.LevelTimer).ToString();
        }

        private void LevelStart()
        {
            SwitchCanvas(CanvasName.InLevel);
        }

        private void ScoreUpdated(int newScore)
        {
            ScoreText.text = "Money: " + newScore.ToString("D" + 5);
        }
        
        private async void LevelEnd()
        {
            SwitchCanvas(CanvasName.End);
            await Task.Delay(4000);
            SwitchCanvas(CanvasName.Start);
        }

        //TODO: Consider actually switching canvases rather than elements
        private void SwitchCanvas(CanvasName canvasName)
        {
            switch (canvasName)
            {
                case CanvasName.Start:
                    MainText.text = "Traffic Jam!";
                    ScoreText.text = "High Score: " + MoneyManager.HighScore.ToString("D" + 5);
                    PlayButton.gameObject.SetActive(true);
                    break;
                case CanvasName.InLevel:
                    PlayButton.gameObject.SetActive(false);
                    break;
                case CanvasName.End:
                    MainText.text = "GAME OVER";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(canvasName), canvasName, null);
            }
        }

        public void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}