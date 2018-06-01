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
        GetComponent<Renderer>().material.color = Color.cyan;
        Renderer[] rends = GetComponentsInChildren<Renderer>().ToArray();

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = Color.cyan;
        }
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = stableColor;

        Renderer[] rends = GetComponentsInChildren<Renderer>().ToArray();

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = stableColor;
        }
    }

    private void OnMouseDown()
    {
        if (typeElement == 0 && editor.TypeSelectElement.typeGameObject != 0 && !thisLandingPlatform)
        {
            Transform nowPosition = gameObject.transform;

            GameObject instanceNewElement = Instantiate(editor.SelectElement, nowPosition.position + new Vector3(0,5,0), Quaternion.identity);

            if (elementOn != null) { Destroy(elementOn); }
            elementOn = instanceNewElement;

            EditorElement edElement = instanceNewElement.AddComponent<EditorElement>();
            edElement.typeElement = editor.TypeSelectElement.typeGameObject;

            
        }

        if (editor.TypeSelectElement.typeGameObject == typeElement)
        {
            Transform nowPosition = gameObject.transform;

            GameObject instNewElement = Instantiate(editor.SelectElement, nowPosition.position, Quaternion.identity);

            EditorElement edElement = instNewElement.AddComponent<EditorElement>();
            edElement.typeElement = typeElement;
            edElement.elementOn = elementOn;

            //Platform mirrorPlatform = FindObjectOfType<LevelData>().Platforms.Find(platform => platform.NamePlatform == nameElement);
            //mirrorPlatform.TypePlatform = (TypesPlatform)editor.TypeSelectElement.universalType;
            //Debug.Log(mirrorPlatform.NamePlatform + " " + (TypesPlatform)universalType);

            Destroy(gameObject);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && typeElement != 0)
        {
            Destroy(gameObject);
        }
    }


}
