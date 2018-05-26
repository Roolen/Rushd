using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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
            fileLevel = new FileInfo("D:\\Level_1.lvl");

            if (fileLevel.Exists)
            {
                AnalysisOfFileLevel(fileLevel);
            }
            else
            {
                Debug.LogError("EL_001: Ошибка загрузки файла уровня");
                SceneManager.LoadScene(0);
            }
        }

        private void AnalysisOfFileLevel(FileInfo fileLevel)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileLevel.FullName);

            XmlElement xmlRoot = xmlDoc.DocumentElement;  //Get root element.

            AnalisisAttributesRoot(xmlRoot);

            List<Platform> platforms = new List<Platform>();

            if (xmlRoot != null)
                foreach (XmlNode xmlChild in xmlRoot) //Get childs root.
                {
                    Platform platform = new Platform();

                    if (xmlChild.Name == "platform")
                    {
                        if (xmlChild.Attributes.Count > 0)
                        {
                            XmlNode attributeName = xmlChild.Attributes.GetNamedItem("Name");

                            if (attributeName != null)
                            {
                                platform.NamePlatform = attributeName.Value;
                            }
                            else
                            {
                                Debug.LogError("EL_003: некорректные атрибуты платформы");
                            }

                            XmlNode attributeType = xmlChild.Attributes.GetNamedItem("Type");

                            if (attributeType != null)
                            {
                                int indexType = Convert.ToInt32(attributeType.Value);
                                platform.TypePlatform = (TypesPlatform) indexType;
                            }
                            else
                            {
                                Debug.LogError("EL_003: некорректные атрибуты платформы");
                            }
                        }
                        else
                        {
                            Debug.LogError("EL_003: некорректные атрибуты платформы");
                        }

                        if (xmlChild.HasChildNodes)
                        {
                            Item item = new Item();
                            XmlNode xmlItem = xmlChild.FirstChild; // Get childs xmlChild.

                            if (xmlItem.Attributes != null && xmlItem.Attributes.Count > 0)
                            {
                                XmlNode attributeName = xmlItem.Attributes.GetNamedItem("Name");
                                if (attributeName != null)
                                {
                                    item.NameItem = attributeName.Value;
                                }
                                else
                                {
                                    Debug.LogError("EL_004: некорректные атрибуты предмета");
                                }

                                XmlNode attributeType = xmlItem.Attributes.GetNamedItem("Type");
                                if (attributeType != null)
                                {
                                    int indexType = Convert.ToInt32(attributeType.Value);
                                    item.TypeItem = (TypesItem) indexType;
                                }
                                else
                                {
                                    Debug.LogError("EL_004: некорректные атрибуты предмета");
                                }

                                platform.ItemOnPlatform = item;
                            }
                            else
                            {
                                Debug.LogError("EL_004: некорректные атрибуты предмета");
                            }
                        }
                    }

                    platforms.Add(platform);
                }

            dates.Platforms = platforms;

        }

        private void AnalisisAttributesRoot(XmlElement xmlRoot)
        {
            if (xmlRoot.Attributes.Count > 0)
            {
                XmlNode attributeName = xmlRoot.Attributes.GetNamedItem("Name");

                if (attributeName != null)
                {
                    dates.NameLevel = attributeName.Value;
                }
                else
                {
                    Debug.LogError("EL_002: Некорректные атрибуты уровня");
                }

                XmlNode attributeDifficult = xmlRoot.Attributes.GetNamedItem("Difficult");

                if (attributeDifficult != null)
                {
                    int indexDifficult = Convert.ToInt32(attributeDifficult.Value);
                    dates.DifficultLevel = (Difficult)indexDifficult;
                }
                else
                {
                    Debug.LogError("EL_002: Некорректные атрибуты уровня");
                }

                XmlNode attributeHeight = xmlRoot.Attributes.GetNamedItem("Height");

                if (attributeHeight != null)
                {
                    dates.Height = Convert.ToInt32(attributeHeight.Value);
                }
                else
                {
                    Debug.LogError("EL_002: Некорректные атрибуты уровня");
                }

                XmlNode attributeWeight = xmlRoot.Attributes.GetNamedItem("Weight");

                if (attributeWeight != null)
                {
                    dates.Weight = Convert.ToInt32(attributeWeight.Value);
                }
                else
                {
                    Debug.LogError("EL_002: Некорректные атрибуты уровня");
                }

                XmlNode attributeLightColor = xmlRoot.Attributes.GetNamedItem("LightColor");

                if (attributeLightColor != null)
                {
                    dates.LightColor = attributeLightColor.Value;
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
        }
    }
}
