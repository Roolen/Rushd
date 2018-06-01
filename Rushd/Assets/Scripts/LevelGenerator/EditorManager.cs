using System;
using System.IO;
using System.Xml;
using UnityEngine;
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

        public Collider myCollider;
        private int typeSelectElement;
        private GameObject selectElement;

        public int TypeSelectElement
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

        public void SaveFile()
        {
            SaveChangesInFile();
        }

        public void ShowElementsPanels()
        {
            int i = -30;

            foreach (GameObject platform in dates.typesPlatforms)
            {
                Button buttonInstance = Instantiate(buttonEditor, panelPlatforms.transform);
                buttonInstance.GetComponentInChildren<Text>().text = platform.name;
                buttonInstance.transform.Translate(65, i, 0);

                buttonInstance.onClick.AddListener(delegate { ButtonEditor_Click(0, platform.gameObject); });

                i -= 30;
            }

            int j = -30;

            foreach (GameObject item in dates.typesItems)
            {
                Button buttonInstance = Instantiate(buttonEditor, panelItems.transform);
                buttonInstance.GetComponentInChildren<Text>().text = item.name;
                buttonInstance.transform.Translate(65, j, 0);

                buttonInstance.onClick.AddListener(delegate { ButtonEditor_Click(1, item.gameObject); });

                j -= 30;
            }

            int c = -30;

            foreach (GameObject tank in dates.tanksTypes)
            {
                Button buttonInstance = Instantiate(buttonEditor, panelTanks.transform);
                buttonInstance.GetComponentInChildren<Text>().text = tank.name;
                buttonInstance.transform.Translate(65, c, 0);

                buttonInstance.onClick.AddListener(delegate { ButtonEditor_Click(2, tank.gameObject); });

                c -= 30;
            }
        }

        private void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit;
            //    if (myCollider.Raycast(ray, out hit, 100.0f))
            //        myCollider.transform.position = ray.GetPoint(100.0f);
            //}
        }

        private bool SaveChangesInFile()
        {
            FileInfo fileLevel = StateController.currentLevel.FileLevel;

            if (!fileLevel.Exists)
            {
                Debug.LogError("EL_001: Ошибка загрузки файла уровня");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileLevel.FullName);

            xmlDoc.RemoveAll(); // Remove all elements in xml document

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlNode xmlRoot = xmlDoc.AppendChild(xmlDoc.CreateElement("level"));

            SaveAttributesRoot(xmlRoot, xmlDoc);

            SavePlatforms(xmlRoot, xmlDoc);

            xmlDoc.Save(fileLevel.FullName);
            Debug.Log("Save file: " + fileLevel.FullName);
            return true;
        }

        private void SavePlatforms(XmlNode xmlRoot, XmlDocument xmlDoc)
        {
            foreach (Platform platform in dates.Platforms)
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


        private void SaveAttributesRoot(XmlNode xmlRoot, XmlDocument xmlDoc)
        {

            SaveAttribute(xmlRoot, xmlDoc, "Name", dates.NameLevel);

            SaveAttribute(xmlRoot, xmlDoc, "Description", dates.Description);

            SaveAttribute(xmlRoot, xmlDoc, "Difficult", Convert.ToString((int)dates.DifficultLevel));

            SaveAttribute(xmlRoot, xmlDoc, "Height", dates.Height.ToString());

            SaveAttribute(xmlRoot, xmlDoc, "Weight", dates.Weight.ToString());

            SaveAttribute(xmlRoot, xmlDoc, "LightColor", dates.LightColor);

        }

        private void SaveAttribute(XmlNode xmlNode, XmlDocument xmlDoc, string nameOfAttribute, string valueOfAttribute)
        {
            XmlAttribute attributeName = xmlNode.Attributes.Append(xmlDoc.CreateAttribute(nameOfAttribute));
            attributeName.AppendChild(xmlDoc.CreateTextNode(valueOfAttribute));
        }


        private void ButtonEditor_Click(int typeElement, GameObject objectOnButton)
        {
            SelectElement = objectOnButton;
            TypeSelectElement = typeElement;
        }

    }
}
