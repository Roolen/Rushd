using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using Assets.Scripts.LevelGenerator;

namespace Assets.Scripts
{
    public class BuilderForPlayMenu : MonoBehaviour
    {
        private List<LevelInfo> levels = new List<LevelInfo>();

        [Header("Путь к папке с картами")]
        public string pathByMap;

        [Header("Родительский объект в котором будут создаваться кнопки уровней")]
        public RectTransform contentArea;
        [Header("Префаб содержащий кнопку уровня")]
        public Button buttonLevel;
        [Header("Кнопка запуска уровня")]
        public Button playButton;
        [Header("Кнопка удаления уровня")]
        public Button deleteButton;

        [Header("Текст в который будет записываться название уровня")]
        public Text textLevelName;
        [Header("Текст в который будет записываться описание уровня")]
        public Text textLevelDescription;
        [Header("Текст в который будет записываться сложность уровня")]
        public Text textLevelDifficult;

        void Start ()
        {

            BuildPlayMenu();

            MakeLevelButtons();
        }

        private bool BuildPlayMenu()
        {
            //DirectoryInfo dir = new DirectoryInfo("Map\\");
            DirectoryInfo dir = new DirectoryInfo(pathByMap);

            if (!dir.Exists)
            {
                Debug.LogError("EL_001: Проблема с загрузкой файла игрового уровня");
                return false;
            }

            foreach (FileInfo levelFile in dir.GetFiles())
            {
                if (!levelFile.Exists)
                {
                    Debug.LogError("EL_001: Проблема с загрузкой файла игрового уровня");
                    continue;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(levelFile.FullName);

                XmlElement xmlRoot = xmlDoc.DocumentElement;

                LevelInfo level = gameObject.AddComponent<LevelInfo>();


                if (xmlRoot.Attributes.Count > 0)
                {

                    level.NameLevel = GetValueOfAttribute(xmlRoot, "Name").Value;

                    level.DescriptionLevel = GetValueOfAttribute(xmlRoot, "Description").Value;

                    {
                        int indexDifficult = Convert.ToInt32(GetValueOfAttribute(xmlRoot, "Difficult").Value);
                        level.DifficultLevel = (Difficult)indexDifficult;
                    }

                    level.HeightLevel = Convert.ToInt32(GetValueOfAttribute(xmlRoot, "Height").Value);

                    level.WeightLevel = Convert.ToInt32(GetValueOfAttribute(xmlRoot, "Weight").Value);

                    level.FileLevel = levelFile;


                    levels.Add(level);
                }
                else
                {
                    Debug.LogError("EL_002: Некорректные атрибуты уровня");
                }

            }

            return true;
        }

        private void MakeLevelButtons()
        {
            int y = -20;

            foreach (LevelInfo level in levels)
            {
                Button instanceButton = Instantiate(buttonLevel,contentArea.transform);
                level.textButtonLevel = instanceButton.GetComponent<LevelInfo>().textButtonLevel;
                instanceButton.transform.Translate(0, y, 0);

                instanceButton.onClick.AddListener(delegate { ButtonLevel_Click(level); });

                level.textButtonLevel.text = level.NameLevel;

                y -= 35;
            }

        }

        private XmlNode GetValueOfAttribute(XmlElement xmlRoot, string nameAttribute)
        {
            XmlNode attribute = xmlRoot.Attributes.GetNamedItem(nameAttribute);

            if (attribute != null)
            {
                return attribute;
            }
            else
            {
                Debug.LogError("EL_002: Некорректные атрибуты уровня");
                return null;
            }
        }

        private void ButtonLevel_Click(LevelInfo level)
        {
            textLevelName.text = level.NameLevel;
            textLevelDescription.text = level.DescriptionLevel;
            textLevelDifficult.text = level.DifficultLevel.ToString();

            GameObject.FindGameObjectWithTag("StateController").GetComponent<StateController>().nextLevel = level;

            playButton.interactable = true;
            deleteButton.interactable = true;
        }
    }
}
