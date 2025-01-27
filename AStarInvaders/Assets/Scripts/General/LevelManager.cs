using UnityEngine.SceneManagement;
using AStar.Behaviours;
using AStar.Utilities;
using UnityEngine;


namespace AStar.General
{
    public class LevelManager : SingleBehaviour<LevelManager>
    {
        [SerializeField] private Player _player;
        [Header("UI")] 
        [SerializeField] private GameObject _winMenu;
        [SerializeField] private GameObject _looseMenu;

        public bool InputAllowed
        {
            get => (_player != null);
        }
        public bool PlayerAlive
        {
            get => (_player != null);
        }


        public void EndGame()
        {
            _player = null;
            _winMenu.SetActive(true);
        }

        public void ResetGame()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        
        public void KillPlayer()
        {
            _player = null;
            _looseMenu.SetActive(true);
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OpenMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}