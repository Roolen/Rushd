﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.LevelGenerator
{
    public class EditorManager : MonoBehaviour
    {
        public StateController stateController;
        public LevelData dates;
        public Button buttonEditor;
        public Transform panelPlatforms;
        public Transform panelItems;
        public Transform panelTanks;
        public GameObject panelEditAttributesElement;

        [Header("Цвет выделения для элементов уровня")]
        public Color colorForSelectElement;

        private TypeElement typeSelectElement;
        private GameObject selectElement;

        public TypeElement TypeSelectElement
        {
            get
            {
                return typeSelectElement;
            }

            set
            {
                typeSelectElement = value;
            }
        }

        public GameObject SelectElement
        {
            get
            {
                return selectElement;
            }

            set
            {
                selectElement = value;
            }
        }

        private void Start()
        {
            stateController.ChangeState(StateController.States.Stoping);

            ShowElementsPanels();
        }

        private void FixedUpdate()
        {
            Ray rayFromCamera = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(rayFromCamera, out hit, 100))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                EditorElement element = hit.transform.gameObject.GetComponent<EditorElement>();

                element.Select(colorForSelectElement);
                element.Enter();

                if (Input.GetMouseButtonDown(0)) element.Click();

            }
        }

        public void SaveFile()
        {
            if (SaveChangesInFile(dates, StateController.currentLevel.FileLevel.FullName)) { }
            else
            {
                Debug.Log("EL_006: Не удалось сохранить файл уровня.");
            }
        }

        public void UpdateAttributeLevel()
        {
            GameObject.Find("InputFieldAttributeNameLevel").GetComponent<InputField>().text = dates.NameLevel;
            GameObject.Find("InputFieldAttributeDescription").GetComponent<InputField>().text = dates.Description;
            GameObject.Find("DropdownDifficultAttributeLevel").GetComponent<Dropdown>().value = Convert.ToInt32(dates.DifficultLevel);
            GameObject.Find("InputFieldAttributeSizeX").GetComponent<InputField>().text = dates.Height.ToString();
            GameObject.Find("InputFieldAttributeSizeY").GetComponent<InputField>().text = dates.Weight.ToString();
        }

        public void ChangeAttributeLevel()
        {
            int y = dates.Height;
            int x = dates.Weight;
            if (GameObject.Find("InputFieldAttributeNameLevel").GetComponent<InputField>().text != "")
            {
                dates.NameLevel = GameObject.Find("InputFieldAttributeNameLevel").GetComponent<InputField>().text;
            }

            if (GameObject.Find("InputFieldAttributeDescription").GetComponent<InputField>().text != "")
            {
                dates.Description = GameObject.Find("InputFieldAttributeDescription").GetComponent<InputField>().text;
            }

            dates.DifficultLevel = (Difficult)GameObject.Find("DropdownDifficultAttributeLevel").GetComponent<Dropdown>().value;

            //todo: Need will develop changes a sizes of level.

            Debug.Log("Edit attributes file: " + StateController.currentLevel.FileLevel.FullName);
        }

        public void CreateNewLevel()
        {
            LevelInfo newLevel = gameObject.AddComponent<LevelInfo>();

            newLevel.NameLevel = GameObject.Find("TextNameNewLevel").GetComponent<Text>().text;
            newLevel.HeightLevel = Convert.ToInt32(GameObject.Find("TextSizeX").GetComponent<Text>().text);
            newLevel.WeightLevel = Convert.ToInt32(GameObject.Find("TextSizeY").GetComponent<Text>().text);
            newLevel.DifficultLevel = (Difficult)GameObject.Find("DropdownDifficult").GetComponent<Dropdown>().value;
            newLevel.DescriptionLevel = GameObject.Find("TextDescriptionNewLevel").GetComponent<Text>().text;

            string pathByNewLevel = "C:\\\\Map\\" + newLevel.NameLevel + ".lvl";
            using (StreamWriter text = File.CreateText(pathByNewLevel))
            {
                text.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            }


            newLevel.FileLevel = new FileInfo(pathByNewLevel);

            SaveNewLevel(pathByNewLevel, newLevel);

            stateController.nextLevel = newLevel;
            stateController.EditorNextLevel();

        }

        public void ShowElementsPanels()
        {
            int i = -30;

            foreach (GameObject platform in dates.typesPlatforms)
            {
                Button buttonInstance = Instantiate(buttonEditor, panelPlatforms.transform);
                buttonInstance.GetComponentInChildren<Text>().text = platform.name;
                buttonInstance.transform.Translate(75, i, 0);

                buttonInstance.onClick.AddListener(delegate { ButtonEditor_Click(platform.GetComponent<TypeElement>(), platform.gameObject); });

                i -= 30;
            }

            int j = -30;

            foreach (GameObject item in dates.typesItems)
            {
                Button buttonInstance = Instantiate(buttonEditor, panelItems.transform);
                buttonInstance.GetComponentInChildren<Text>().text = item.name;
                buttonInstance.transform.Translate(75, j, 0);

                buttonInstance.onClick.AddListener(delegate { ButtonEditor_Click(item.GetComponent<TypeElement>(), item.gameObject); });

                j -= 30;
            }

            int c = -30;

            foreach (GameObject tank in dates.tanksTypes)
            {
                Button buttonInstance = Instantiate(buttonEditor, panelTanks.transform);
                buttonInstance.GetComponentInChildren<Text>().text = tank.name;
                buttonInstance.transform.Translate(75, c, 0);

                buttonInstance.onClick.AddListener(delegate { ButtonEditor_Click(tank.GetComponent<TypeElement>(), tank.gameObject); });

                c -= 30;
            }
        }

        public static void CallAttributeEditor(EditorElement element)
        {
            GameObject panel = GameObject.FindObjectOfType<EditorManager>().panelEditAttributesElement;

            panel.SetActive(true);
            GameObject.Find("InputFieldNamePlatform").GetComponent<InputField>().text = element.nameElement;

            Button saveButton = GameObject.Find("ButtonEditorSaveAttributesPlatform").GetComponent<Button>();
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(delegate { ButtonAttributeSave_Click(element); });

        }

        private bool SaveNewLevel(string pathNewLevel, LevelInfo level)
        {
            List<Platform> platforms = new List<Platform>();
            for (int i = 0; i < level.HeightLevel * level.WeightLevel - 1; i++)
            {
                platforms.Add(new Platform("Platform" + i, TypesPlatform.PlatformOpenSpace, null));
            }

            LevelData dataNewLevel = new LevelData(level.NameLevel, level.HeightLevel, level.WeightLevel)
            {
                DifficultLevel = level.DifficultLevel,
                Description = level.DescriptionLevel,
                Platforms = platforms
            };

            SaveChangesInFile(dataNewLevel, pathNewLevel);

            return true;
        }

        private bool SaveChangesInFile(LevelData data, string pathByFileLevel)
        {
            FileInfo fileLevel = null;
            fileLevel = new FileInfo(pathByFileLevel);
            

            if (!fileLevel.Exists)
            {
                Debug.LogError("EL_001: Ошибка загрузки файла уровня");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileLevel.FullName);

            xmlDoc.RemoveAll(); // Remove all elements in xml document

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlNode xmlRoot = xmlDoc.AppendChild(xmlDoc.CreateElement("level"));

            SaveAttributesRoot(xmlRoot, xmlDoc, data);

            SavePlatforms(xmlRoot, xmlDoc, data.Platforms);

            xmlDoc.Save(fileLevel.FullName);
            Debug.Log("Save file: " + fileLevel.FullName);
            return true;
        }

        private void SavePlatforms(XmlNode xmlRoot, XmlDocument xmlDoc, List<Platform> platforms)
        {
            foreach (Platform platform in platforms)
            {
                XmlNode xmlPlatform = xmlRoot.AppendChild(xmlDoc.CreateElement("platform"));

                SaveAttribute(xmlPlatform, xmlDoc, "Name", platform.NamePlatform);
                SaveAttribute(xmlPlatform, xmlDoc, "Type", Convert.ToString((int)platform.TypePlatform));

                if (platform.ItemOnPlatform != null)
                {
                    XmlNode xmlItem = xmlPlatform.AppendChild(xmlDoc.CreateElement("item"));

                    SaveAttribute(xmlItem, xmlDoc, "Name", platform.ItemOnPlatform.NameItem);
                    SaveAttribute(xmlItem, xmlDoc, "Type", Convert.ToString((int)platform.ItemOnPlatform.TypeItem));
                }

                if (platform.TankOnPlatform != null)
                {
                    XmlNode xmlTank = xmlPlatform.AppendChild(xmlDoc.CreateElement("tank"));

                    SaveAttribute(xmlTank, xmlDoc, "Name", platform.TankOnPlatform.NameTank);
                    SaveAttribute(xmlTank, xmlDoc, "Type", Convert.ToString((int)platform.TankOnPlatform.TypeTank));
                    SaveAttribute(xmlTank, xmlDoc, "Rotate", platform.TankOnPlatform.RotateTank.ToString());
                    SaveAttribute(xmlTank, xmlDoc, "TargetPoint", platform.TankOnPlatform.TargetPoint);
                }
            }
        }


        private void SaveAttributesRoot(XmlNode xmlRoot, XmlDocument xmlDoc, LevelData data)
        {

            SaveAttribute(xmlRoot, xmlDoc, "Name", data.NameLevel);

            SaveAttribute(xmlRoot, xmlDoc, "Description", data.Description);

            SaveAttribute(xmlRoot, xmlDoc, "Difficult", Convert.ToString((int)data.DifficultLevel));

            SaveAttribute(xmlRoot, xmlDoc, "Height", data.Height.ToString());

            SaveAttribute(xmlRoot, xmlDoc, "Weight", data.Weight.ToString());

            SaveAttribute(xmlRoot, xmlDoc, "LightColor", data.LightColor);

        }

        private void SaveAttribute(XmlNode xmlNode, XmlDocument xmlDoc, string nameOfAttribute, string valueOfAttribute)
        {
            XmlAttribute attributeName = xmlNode.Attributes.Append(xmlDoc.CreateAttribute(nameOfAttribute));
            attributeName.AppendChild(xmlDoc.CreateTextNode(valueOfAttribute));
        }


        private void ButtonEditor_Click(TypeElement type, GameObject objectOnButton)
        {
            SelectElement = objectOnButton;
            TypeSelectElement = type;

        }

        private static void ButtonAttributeSave_Click(EditorElement element)
        {
            element.Rename(GameObject.Find("InputFieldNamePlatform").GetComponent<InputField>().text);
        }

    }
}
