using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class StateController : MonoBehaviour
    {

        public LevelInfo nextLevel;
        public static LevelInfo currentLevel;

        public bool stopGame;
        public static bool menuMode;

        public enum States
        {
            Playing = 0,
            Stoping = 1,
            Pausing = 2,
            Exiting = 3
        }

        void Start ()
        {

        }

        /// <summary>
        /// Изменяет состояние игры.
        /// </summary>
        /// <param name="stateNew">Новое состояние</param>
        public void ChangeState(States stateNew)
        {
            if (stateNew == States.Playing) Play();

            else if (stateNew == States.Stoping) Stop();

            else if (stateNew == States.Pausing) Pause();

            else if (stateNew == States.Exiting) Exit();
        }


        public void PlayLevel()
        {
            currentLevel = nextLevel;
            menuMode = false;

            SceneManager.LoadScene("GameLevel");
        }

        public void NextLevel()
        {
            currentLevel = nextLevel;
            menuMode = false;

            SceneManager.LoadScene(SceneManager.sceneCount - 1);
        }

        public void EditorNextLevel()
        {
            currentLevel = nextLevel;
            menuMode = false;

            SceneManager.LoadScene("EditorForLevels");
        }

        public void ChangeState(int i)
        {
            States newState = (States)i; 

            ChangeState(newState);
        }

        private bool Play()
        {
            try
            {
                Time.timeScale = 1.0f;
                stopGame = false;

                Debug.Log("Time play");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        private bool Stop()
        {
            try
            {
                Time.timeScale = 1.0f;
                stopGame = true;

                Debug.Log("Time stop");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        private bool Pause()
        {
            try
            {
                Time.timeScale = 0.0f;
                Debug.Log("Time pause");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        private bool Exit()
        {
            try
            {
                Application.Quit();
                Debug.Log("Application quit");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }
    }
}
