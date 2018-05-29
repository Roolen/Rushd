using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using Assets.Scripts.LevelGenerator;
using UnityEditor.IMGUI.Controls;

namespace Assets.Scripts
{
    public class BuilderForPlayMenu : MonoBehaviour
    {
        private List<LevelInfo> levels = new List<LevelInfo>();

        public RectTransform contentArea;
        public Button buttonLevel;

        public Text textLevelName;
        public Text textLevelDescription;
        public Text textLevelDifficult;

        void Start ()
        {

            BuildPlayMenu();

            MakeLevelButtons();
        }

        private bool BuildPlayMenu()
        {
            //DirectoryInfo dir = new DirectoryInfo("Map\\");
            DirectoryInfo dir = new DirectoryInfo("D:\\Map");

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

                LevelInfo level = new LevelInfo();


                if (xmlRoot.Attributes.Count > 0)
                {

                    XmlNode attributeName = xmlRoot.Attributes.GetNamedItem("Name");

                    if (attributeName != null)
                    {
                        level.NameLevel = attributeName.Value;
                    }
                    else
                    {
                        Debug.LogError("EL_002: Некорректные атрибуты уровня");
                    }

                    XmlNode attributeDescriprion = xmlRoot.Attributes.GetNamedItem("Description");

                    if (attributeDescriprion != null)
                    {
                        level.DescriptionLevel = attributeDescriprion.Value;
                    }
                    else
                    {
                        Debug.LogError("EL_002: Некорректные атрибуты уровня");
                    }

                    XmlNode attributeDifficult = xmlRoot.Attributes.GetNamedItem("Difficult");

                    if (attributeDifficult != null)
                    {
                        int indexDifficult = Convert.ToInt32(attributeDifficult.Value);
                        level.DifficultLevel = (Difficult)indexDifficult;
                    }
                    else
                    {
                        Debug.LogError("EL_002: Некорректные атрибуты уровня");
                    }

                    XmlNode attributeHeight = xmlRoot.Attributes.GetNamedItem("Height");

                    if (attributeHeight != null)
                    {
                        level.HeightLevel = Convert.ToInt32(attributeHeight.Value);
                    }
                    else
                    {
                        Debug.LogError("EL_002: Некорректные атрибуты уровня");
                    }

                    XmlNode attributeWeight = xmlRoot.Attributes.GetNamedItem("Weight");

                    if (attributeWeight != null)
                    {
                        level.WeightLevel = Convert.ToInt32(attributeWeight.Value);
                    }
                    else
                    {
                        Debug.LogError("EL_002: Некорректные атрибуты уровня");
                    }

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
            int y = 250;

            foreach (LevelInfo level in levels)
            {
                Button instanceButton = Instantiate(buttonLevel);
                level.textButtonLevel = instanceButton.GetComponent<LevelInfo>().textButtonLevel;
                instanceButton.transform.SetParent(contentArea.transform);
                instanceButton.transform.position = new Vector3(0, y, 0);

                instanceButton.onClick.AddListener(delegate { ButtonLevel_Click(level); });

                level.textButtonLevel.text = level.nameLevel;

                y -= 40;
            }

        }

        private void ButtonLevel_Click(LevelInfo level)
        {
            textLevelName.text = level.NameLevel;
            textLevelDescription.text = level.DescriptionLevel;
            textLevelDifficult.text = level.DifficultLevel.ToString();
        }
    }
}
