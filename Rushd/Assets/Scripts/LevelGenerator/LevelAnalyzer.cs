using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.LevelGenerator
{
    public class LevelAnalyzer : MonoBehaviour
    {
        public LevelData dates;

        private void Start ()
        {
            if (StateController.menuMode)
            {
                FileInfo fileLevel = null;
                fileLevel = StateController.currentLevel.FileLevel;

                if (fileLevel != null && fileLevel.Exists)
                {
                    AnalysisOfFileLevel(fileLevel);
                }
                else
                {
                    Debug.LogError("EL_001: Ошибка загрузки файла уровня");
                }
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
                foreach (XmlNode xmlPlatform in xmlRoot) //Get childs root.
                {
                    Platform platform = new Platform();

                    if (xmlPlatform.Name == "platform")
                    {
                        if (xmlPlatform.Attributes.Count > 0)
                        {
                            platform.NamePlatform = GetValueOfAttribute(xmlPlatform, "Name", true).Value;

                            {
                                int indexType = Convert.ToInt32(GetValueOfAttribute(xmlPlatform, "Type", true).Value);
                                platform.TypePlatform = (TypesPlatform) indexType;
                            }

                            
                        }
                        else
                        {
                            Debug.LogError("EL_003: некорректные атрибуты платформы");
                        }

                        if (xmlPlatform.HasChildNodes)
                        {
                            foreach (XmlNode xmlChild in xmlPlatform)
                            {
                                if (xmlChild.Name == "item")
                                {
                                    Item item = new Item();
                                    XmlNode xmlItem = xmlChild; // Get child xmlPlatform.

                                    if (xmlItem.Attributes != null && xmlItem.Attributes.Count > 0)
                                    {
                                        item.NameItem = GetValueOfAttribute(xmlItem, "Name", true).Value;

                                        {
                                            int indexType = Convert.ToInt32(GetValueOfAttribute(xmlItem, "Type", true).Value);
                                            item.TypeItem = (TypesItem) indexType;
                                        }

                                        platform.ItemOnPlatform = item;
                                    }
                                }

                                if (xmlChild.Name == "tank")
                                {
                                    Tank tank = new Tank();
                                    XmlNode xmlTank = xmlChild;  // Get child xmlPlatform.

                                    if (xmlTank.Attributes != null && xmlTank.Attributes.Count > 0)
                                    {
                                        tank.NameTank = GetValueOfAttribute(xmlTank, "Name", true).Value;

                                        {
                                            int indexType = Convert.ToInt32(GetValueOfAttribute(xmlTank, "Type", true).Value);
                                            tank.TypeTank = (TypesTank)indexType;
                                        }

                                        if (GetValueOfAttribute(xmlTank, "Rotate", false) != null)
                                            tank.RotateTank = Convert.ToInt32(GetValueOfAttribute(xmlTank, "Rotate", false).Value);

                                        if (GetValueOfAttribute(xmlTank, "TargetPoint", false) != null)
                                            tank.TargetPoint = GetValueOfAttribute(xmlTank, "TargetPoint", false).Value;

                                        platform.TankOnPlatform = tank;
                                    }
                                }
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
                dates.NameLevel = GetValueOfAttribute(xmlRoot, "Name").Value;

                dates.Description = GetValueOfAttribute(xmlRoot, "Description").Value;

                {
                    int indexDifficult = Convert.ToInt32(GetValueOfAttribute(xmlRoot, "Difficult").Value);
                    dates.DifficultLevel = (Difficult) indexDifficult;
                }

                dates.Height = Convert.ToInt32(GetValueOfAttribute(xmlRoot, "Height").Value);

                dates.Weight = Convert.ToInt32(GetValueOfAttribute(xmlRoot, "Weight").Value);

                dates.LightColor = GetValueOfAttribute(xmlRoot, "LightColor").Value;

            }
            else
            {
                Debug.LogError("EL_002: Некорректные атрибуты уровня");

                
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

        private XmlNode GetValueOfAttribute(XmlNode xmlNode, string nameAttribute, bool must)
        {
            XmlNode attribute = xmlNode.Attributes.GetNamedItem(nameAttribute);

            if (attribute != null)
            {
                return attribute;
            }
            else
            {
                if (must)
                    Debug.LogError("EL_002: Некорректные атрибуты уровня");
                return null;
            }
        }
    }
}
