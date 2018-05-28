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
        public GameObject contentArea;
        public Button buttonLevel;

        void Start ()
        {
		    BuildPlayMenu();
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

                Button instanceButton = Instantiate(buttonLevel);
                instanceButton.transform.parent = contentArea.transform;

                LevelInfo level = instanceButton.GetComponent<LevelInfo>();

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
                }
                else
                {
                    Debug.LogError("EL_002: Некорректные атрибуты уровня");
                }


                {
                    level.textButtonLevel.text = level.nameLevel;
                }
            }

            return true;
        }
    }
}
