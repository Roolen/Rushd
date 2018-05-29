using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class StateController : MonoBehaviour
    {

        public LevelInfo nextLevel;
        public static LevelInfo currentLevel;

        public enum States
        {
            Play = 0,
            Stop = 1,
            Exit = 2
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
            if (stateNew == States.Play) Play();

            else if (stateNew == States.Stop) Stop();

            else if (stateNew == States.Exit) Exit();
        }

        public void PlayLevel()
        {
            currentLevel = nextLevel;

            SceneManager.LoadScene("GameLevel");
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
                Time.timeScale = 0.0f;
                Debug.Log("Time stop");

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
