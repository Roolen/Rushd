using System;
using System.IO;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.LevelGenerator
{
    public class LevelAnalyzer : MonoBehaviour
    {
        public LevelData dates;

        private void Start ()
        {
            FileInfo fileLevel = null;
            fileLevel = new FileInfo("D:\\WorkingDocument\\Rushd\\Level_1.lvl");

            if (fileLevel.Exists)
            {
                AnalysisOfFileLevel(fileLevel);
            }
            else
            {
                Debug.LogError("EL_0001: Ошибка загрузки файла уровня");
                SceneManager.LoadScene(0);
            }
        }

        private void AnalysisOfFileLevel(FileInfo fileLevel)
        {
            
        }

        private void AnalysisOfDataLevel(FileInfo fileLevel)
        {

        }
    }
}
