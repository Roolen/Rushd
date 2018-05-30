using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using System.Xml;
using System.IO;
using Assets.Scripts.LevelGenerator;

public class EditorManager : MonoBehaviour
{
    public StateController stateController;
    public LevelData dates;

    private void Start()
    {
        stateController.ChangeState(StateController.States.Stop);
    }

    public void SaveFile()
    {
        SaveChangesInFile();
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

}
