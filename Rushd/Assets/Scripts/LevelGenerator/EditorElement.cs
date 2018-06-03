using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private EditorManager editor;

    private Color stableColor;

    private void Start()
    {
        stableColor = GetComponent<Renderer>().material.color;

        editor = GameObject.FindObjectOfType<EditorManager>();
    }

    private void OnMouseEnter()
    {
        if (typeElement == 0)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
            Renderer[] rends = GetComponentsInChildren<Renderer>().ToArray();

            for (int i = 0; i < rends.Length; i++)
            {
                rends[i].material.color = Color.cyan;
            }
        }
    }

    private void OnMouseExit()
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

    private void OnMouseDown()
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

            Platform mirrorPlatform = FindObjectOfType<LevelData>().Platforms.Find(platform => platform.NamePlatform == nameElement);

            if (elementOn.GetComponent<EditorElement>().typeElement == 1)
            {
                mirrorPlatform.ItemOnPlatform.TypeItem = (TypesItem) editor.TypeSelectElement.universalType;
            }
            else if (elementOn.GetComponent<EditorElement>().typeElement == 2)
            {
                mirrorPlatform.TankOnPlatform.TypeTank = (TypesTank) editor.TypeSelectElement.universalType;
                mirrorPlatform.TankOnPlatform.RotateTank = (int)elementOn.transform.rotation.y;
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

    private void OnMouseOver()
    {
        if (typeElement == 0 && !thisLandingPlatform)
        {

            if (Input.GetMouseButtonDown(1))
            {
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
