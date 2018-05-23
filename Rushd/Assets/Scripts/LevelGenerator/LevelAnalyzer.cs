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
                Debug.LogError("EL_0001: Ошибка загрузки файла уровня");
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

            foreach (XmlNode xmlNode in xmlRoot)
            {
                Platform platform = new Platform();

                if (xmlNode.Name == "platform")
                {
                    if (xmlNode.Attributes.Count > 0)
                    {
                        XmlNode attributeName = xmlNode.Attributes.GetNamedItem("Name");

                        if (attributeName != null)
                        {
                            platform.NamePlatform = attributeName.Value;
                        }

                        XmlNode attributeType = xmlNode.Attributes.GetNamedItem("Type");

                        if (attributeType != null)
                        {
                            int indexType = Convert.ToInt32(attributeType.Value);
                            platform.TypePlatform = (TypesPlatform)indexType;
                        }
                    }

                    if (xmlNode.HasChildNodes)
                    {
                        Item item = new Item();
                        XmlNode xmlItem = xmlNode.FirstChild;

                        if (xmlItem.Attributes.Count > 0)
                        {
                            XmlNode attributeName = xmlItem.Attributes.GetNamedItem("Name");

                            if (attributeName != null)
                            {
                                item.NameItem = attributeName.Value;
                            }

                            XmlNode attributeType = xmlItem.Attributes.GetNamedItem("Type");

                            if (attributeType != null)
                            {
                                int indexType = Convert.ToInt32(attributeType.Value);
                                item.TypeItem = (TypesItem)indexType;
                            }

                            platform.ItemOnPlatform = item;
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
                    Debug.LogError("EL_002: Отсутствуют необходимые данные уровня");
                }

                XmlNode attributeDifficult = xmlRoot.Attributes.GetNamedItem("Difficult");

                if (attributeDifficult != null)
                {
                    int indexDifficult = Convert.ToInt32(attributeDifficult.Value);
                    dates.DifficultLevel = (Difficult)indexDifficult;
                }
                else
                {
                    Debug.LogError("EL_002: Отсутствуют необходимые данные уровня");
                }

                XmlNode attributeHeight = xmlRoot.Attributes.GetNamedItem("Height");

                if (attributeHeight != null)
                {
                    dates.Height = Convert.ToInt32(attributeHeight.Value);
                }
                else
                {
                    Debug.LogError("EL_002: Отсутствуют необходимые данные уровня");
                }

                XmlNode attributeWeight = xmlRoot.Attributes.GetNamedItem("Weight");

                if (attributeWeight != null)
                {
                    dates.Weight = Convert.ToInt32(attributeWeight.Value);
                }
                else
                {
                    Debug.LogError("EL_002: Отсутствуют необходимые данные уровня");
                }

                XmlNode attributeLightColor = xmlRoot.Attributes.GetNamedItem("LightColor");

                if (attributeLightColor != null)
                {
                    dates.LightColor = attributeLightColor.Value;
                }
                else
                {
                    Debug.LogError("EL_002: Отсутствуют необходимые данные уровня");
                }
            }
        }
    }
}
