using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.LevelGenerator;
using UnityEngine;

public class EditorElement : MonoBehaviour
{
    public string nameElement;
    public int typeElement;
    public int universalType;
    public int idForElement;
    public GameObject elementOn;
    public bool thisLandingPlatform;
    private Platform mirrorPlatform;

    private EditorManager editor;

    private Color stableColor;
    private float timer;

    private void Start()
    {
        stableColor = GetComponent<Renderer>().material.color;

        editor = GameObject.FindObjectOfType<EditorManager>();

        mirrorPlatform = FindObjectOfType<LevelData>().Platforms.Find(platform => platform.NamePlatform == nameElement);
    }

    private void FixedUpdate()
    {
        timer = 1 + timer;

        if (timer >= 5) Deselect();  // Check for selection.
    }

    /// <summary>
    /// Выделяет элемент выбраным цветом.
    /// </summary>
    /// <param name="color">Цвет выделения</param>
    public void Select(Color color)
    {
        if (typeElement == 0)
        {
            GetComponent<Renderer>().material.color = color;
            Renderer[] rends = GetComponentsInChildren<Renderer>().ToArray();

            for (int i = 0; i < rends.Length; i++)
            {
                rends[i].material.color = color;
            }
        }

        timer = 0;
    }

    /// <summary>
    /// Убирает выделение с элемента.
    /// </summary>
    public void Deselect()
    {
        if (typeElement == 0)
        {
            GetComponent<Renderer>().material.color = stableColor;

            Renderer[] rends = GetComponentsInChildren<Renderer>().ToArray();

            for (int i = 0; i < rends.Length; i++)
            {
                rends[i].material.color = stableColor;
            }
        }
    }

    public void Rename(string newName)
    {
        nameElement = newName;
        mirrorPlatform.NamePlatform = newName;
    }

    /// <summary>
    /// Изменяет элемент на новый, в зависимости от того, какой элемент выбран.
    /// </summary>
    public void ChangeElement()
    {
        if (typeElement == 0 && editor.TypeSelectElement.typeGameObject != 0 && !thisLandingPlatform)
        {
            Transform nowPosition = gameObject.transform;

            GameObject instanceNewElement = Instantiate(editor.SelectElement, nowPosition.position + new Vector3(0,5,0), Quaternion.identity);

            EditorElement edElement = instanceNewElement.AddComponent<EditorElement>();
            edElement.typeElement = editor.TypeSelectElement.typeGameObject;

            if (elementOn != null)
            {
                instanceNewElement.GetComponent<EditorElement>().nameElement = elementOn.name;
                Destroy(elementOn);
            }

            elementOn = instanceNewElement;

            

            if (elementOn.GetComponent<EditorElement>().typeElement == 1)
            {
                mirrorPlatform.ItemOnPlatform = new Item
                {
                    TypeItem = (TypesItem)editor.TypeSelectElement.universalType
                };
            }
            else if (elementOn.GetComponent<EditorElement>().typeElement == 2)
            {
                mirrorPlatform.TankOnPlatform = new Tank
                {
                    TypeTank = (TypesTank)editor.TypeSelectElement.universalType,
                    RotateTank = (int)elementOn.transform.rotation.y
                };
            }
        }

        if (editor.TypeSelectElement.typeGameObject == typeElement)
        {
            Transform nowPosition = gameObject.transform;

            GameObject instNewElement = Instantiate(editor.SelectElement, nowPosition.position, Quaternion.identity);

            EditorElement edElement = instNewElement.AddComponent<EditorElement>();
            edElement.typeElement = typeElement;
            edElement.elementOn = elementOn;
            edElement.nameElement = nameElement;

            Platform mirrorPlatform = FindObjectOfType<LevelData>().Platforms.Find(platform => platform.NamePlatform == nameElement);
            mirrorPlatform.TypePlatform = (TypesPlatform)editor.TypeSelectElement.universalType;

            Debug.Log(mirrorPlatform.NamePlatform + " " + (TypesPlatform)editor.TypeSelectElement.universalType);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Набор логики для редактирования свойств элемента.
    /// </summary>
    public void Enter()
    {
        if (typeElement == 0 && !thisLandingPlatform)
        {

            if (Input.GetMouseButtonDown(1))
            {
                Platform mirrorPlatform = FindObjectOfType<LevelData>().Platforms.Find(platform => platform.NamePlatform == nameElement);

                if (elementOn.GetComponent<EditorElement>().typeElement == 1)
                {
                    Debug.Log("Delete item " + mirrorPlatform.ItemOnPlatform.NameItem);
                    mirrorPlatform.RemoveItemOnPlatform();
                }
                else if (elementOn.GetComponent<EditorElement>().typeElement == 2)
                {
                    Debug.Log("Delete tank " + mirrorPlatform.TankOnPlatform.NameTank);
                    mirrorPlatform.RemoveTankOnPlatform();
                }

                Destroy(elementOn);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                RotateTank(90);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateTank(-90);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            EditorManager.CallAttributeEditor(this);
        }
    }

    private void RotateTank(int rotateOnY)
    {
        elementOn.gameObject.transform.Rotate(0, rotateOnY, 0);

        Platform mirrorPlatform = FindObjectOfType<LevelData>().Platforms.Find(platform => platform.NamePlatform == nameElement);

        if (elementOn.GetComponent<EditorElement>().typeElement == 2)
        {
            mirrorPlatform.TankOnPlatform.RotateTank = Convert.ToInt32(elementOn.transform.localRotation.eulerAngles.y);
            Debug.Log(mirrorPlatform.TankOnPlatform.RotateTank);
        }
    }


}
