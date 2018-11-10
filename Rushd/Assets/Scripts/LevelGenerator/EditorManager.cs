using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ReSharper disable IdentifierTypo
#pragma warning disable 649

namespace Assets.Scripts.LevelGenerator
{
    public class EditorManager : MonoBehaviour
    {
        [Header("Экземпляр StateController")]
        [SerializeField] private StateController stateController;

        [Header("Экземпляр LevelData")]
        [SerializeField] private LevelData dates;

        [Header("Экземпляр ContentManager")]
        [SerializeField] private ContentManager content;

        [Header("Префаб с кнопкой для редактора")]
        [SerializeField] private Button buttonEditor;

        [Header("Панель с типами платформ")]
        [SerializeField] private Transform panelPlatforms;

        [Header("Панель с типами предметов")]
        [SerializeField] private Transform panelItems;

        [Header("Панель с типами танков")]
        [SerializeField] private Transform panelTanks;

        [Header("Панель для редактирования атрибутов элемента")]
        [SerializeField] private GameObject panelEditAttributesElement;

        [Header("Цвет выделения для элементов уровня")]
        [SerializeField] private Color colorForSelectElement;

        public TypeElement TypeSelectElement { get; set; }

        public GameObject SelectElement { get; set; }

        private ICommandController[] commansEditor;

        #region Properties

        public StateController StateController
        {
            get
            {
                return stateController;
            }
        }

        public LevelData Dates
        {
            get
            {
                return dates;
            }
        }

        public Button ButtonEditor
        {
            get
            {
                return buttonEditor;
            }
        }

        public Transform PanelPlatforms
        {
            get
            {
                return panelPlatforms;
            }
        }

        public Transform PanelItems
        {
            get
            {
                return panelItems;
            }
        }

        public Transform PanelTanks
        {
            get
            {
                return panelTanks;
            }
        }

        public GameObject PanelEditAttributesElement
        {
            get
            {
                return panelEditAttributesElement;
            }
        }

        public Color ColorForSelectElement
        {
            get
            {
                return colorForSelectElement;
            }
        }

        #endregion


        private void Start()
        {
            StateController.ChangeState(StateController.States.Stoping);

            ShowElementsPanels();
        }

        private void FixedUpdate()
        {
            // Задержка выделителя объектов.
            if (Time.timeSinceLevelLoad > 1) 
            {
                RaycastForElement();
            }
        }

        /// <summary>
        /// Пускает луч для игрового элемента.
        /// </summary>
        private void RaycastForElement()
        {
            Ray rayFromCamera = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(rayFromCamera, out hit, 300))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                EditorElement element = hit.transform.gameObject.GetComponentInParent<EditorElement>();

                element.Select(ColorForSelectElement);
                element.Enter();

                if (Input.GetMouseButtonDown(0)) element.ChangeElement();

            }
        }

        /// <summary>
        /// Сохраняет уровень в файл уровня.
        /// </summary>
        public void SaveFile()
        {
            if (SaveChangesInFile(Dates, StateController.currentLevel.FileLevel.FullName)) { }
            else
            {
                Debug.Log("EL_006: Не удалось сохранить файл уровня.");
            }
        }

        /// <summary>
        /// Обновляет отображение атрибутов уровня в специальном окне.
        /// </summary>
        public void UpdateAttributeLevel()
        {
            GameObject.Find("InputFieldAttributeNameLevel").GetComponent<InputField>().text = Dates.NameLevel;
            GameObject.Find("InputFieldAttributeDescription").GetComponent<InputField>().text = Dates.Description;
            GameObject.Find("DropdownDifficultAttributeLevel").GetComponent<Dropdown>().value = Convert.ToInt32(Dates.DifficultLevel);
            GameObject.Find("InputFieldAttributeSizeX").GetComponent<InputField>().text = Dates.Height.ToString();
            GameObject.Find("InputFieldAttributeSizeY").GetComponent<InputField>().text = Dates.Weight.ToString();
        }

        /// <summary>
        /// Изменяет атрибуты уровня на новые.
        /// </summary>
        public void ChangeAttributeLevel()
        {
            //int y = Dates.Height;
            //int x = Dates.Weight;
            if (GameObject.Find("InputFieldAttributeNameLevel").GetComponent<InputField>().text != "")
            {
                Dates.NameLevel = GameObject.Find("InputFieldAttributeNameLevel").GetComponent<InputField>().text;
            }

            if (GameObject.Find("InputFieldAttributeDescription").GetComponent<InputField>().text != "")
            {
                Dates.Description = GameObject.Find("InputFieldAttributeDescription").GetComponent<InputField>().text;
            }

            Dates.DifficultLevel = (Difficult)GameObject.Find("DropdownDifficultAttributeLevel").GetComponent<Dropdown>().value;

            //todo: Need will develop changes a sizes of level.

            Debug.Log("Edit attributes file: " + StateController.currentLevel.FileLevel.FullName);
        }

        /// <summary>
        /// Создает новый уровень.
        /// </summary>
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
                //Запись минимального значения, для того чтобы файл распозновался как xml.
                text.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                text.WriteLine("<level> </level>");
            }


            newLevel.FileLevel = new FileInfo(pathByNewLevel);

            SaveNewLevel(pathByNewLevel, newLevel);

            StateController.nextLevel = newLevel;
            StateController.EditorNextLevel();

        }

        /// <summary>
        /// Выводит элементы на панели типов, для разных элементов.
        /// </summary>
        private void ShowElementsPanels()
        {
            int i = -30;

            foreach (GameObject platform in content.TypesPlatforms)
            {
                Button buttonInstance = Instantiate(ButtonEditor, PanelPlatforms.transform);
                buttonInstance.GetComponentInChildren<Text>().text = platform.name;
                buttonInstance.transform.Translate(75, i, 0);

                var platformLocal = platform;
                buttonInstance.onClick.AddListener(delegate { ButtonEditor_Click(platformLocal.GetComponent<TypeElement>(), platformLocal.gameObject); });

                i -= 30;
            }

            int j = -30;

            foreach (GameObject item in content.TypesItems)
            {
                Button buttonInstance = Instantiate(ButtonEditor, PanelItems.transform);
                buttonInstance.GetComponentInChildren<Text>().text = item.name;
                buttonInstance.transform.Translate(75, j, 0);

                var itemLocale = item;
                buttonInstance.onClick.AddListener(delegate { ButtonEditor_Click(itemLocale.GetComponent<TypeElement>(), itemLocale.gameObject); });

                j -= 30;
            }

            int c = -30;

            foreach (GameObject tank in content.TanksTypes)
            {
                Button buttonInstance = Instantiate(ButtonEditor, PanelTanks.transform);
                buttonInstance.GetComponentInChildren<Text>().text = tank.name;
                buttonInstance.transform.Translate(75, c, 0);

                var tankLocal = tank;
                buttonInstance.onClick.AddListener(delegate { ButtonEditor_Click(tankLocal.GetComponent<TypeElement>(), tankLocal.gameObject); });

                c -= 30;
            }
        }

        /// <summary>
        /// Вызывает окно редактирования атрибутов игрового элемента.
        /// </summary>
        /// <param name="element">Игровой элемент</param>
        public static void CallAttributeEditor(EditorElement element)
        {
            if (element.typeElement == 0)
            {
                GameObject panel = FindObjectOfType<EditorManager>().PanelEditAttributesElement;

                panel.SetActive(true);
                GameObject.Find("InputFieldNamePlatform").GetComponent<InputField>().text = element.nameElement;

                Button saveButton = GameObject.Find("ButtonEditorSaveAttributesPlatform").GetComponent<Button>();
                saveButton.onClick.RemoveAllListeners();
                saveButton.onClick.AddListener(delegate { ButtonAttributeSave_Click(element); });
            }
            

        }

        /// <summary>
        /// Сохраняет созданый уровень в файл.
        /// </summary>
        /// <param name="pathNewLevel">Путь к файлу нового уровня</param>
        /// <param name="level">Игровой уровень</param>
        /// <returns>Успешность сохранения</returns>
        private bool SaveNewLevel(string pathNewLevel, LevelInfo level)
        {
            List<Platform> platforms = new List<Platform>();
            for (int i = 0; i < level.HeightLevel * level.WeightLevel; i++)
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

        /// <summary>
        /// Сохраняет изменения сделаные в редакторе, в файл.
        /// </summary>
        /// <param name="data">Данные об уровне</param>
        /// <param name="pathByFileLevel">Путь к файлу уровня</param>
        /// <returns>Успешность сохранения</returns>
        private bool SaveChangesInFile(LevelData data, string pathByFileLevel)
        {
            var fileLevel = new FileInfo(pathByFileLevel);
            

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

        /// <summary>
        /// Сохранение атрибута в файл.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="nameOfAttribute"></param>
        /// <param name="valueOfAttribute"></param>
        private void SaveAttribute(XmlNode xmlNode, XmlDocument xmlDoc, string nameOfAttribute, string valueOfAttribute)
        {
            if (xmlNode.Attributes != null)
            {
                XmlAttribute attributeName = xmlNode.Attributes.Append(xmlDoc.CreateAttribute(nameOfAttribute));
                attributeName.AppendChild(xmlDoc.CreateTextNode(valueOfAttribute));
            }
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

    public class EditorCommand : ICommandController
    {
        

        void ICommandController.Execute()
        {
            throw new NotImplementedException();
        }

        void ICommandController.Undo()
        {
            throw new NotImplementedException();
        }
    }
}
