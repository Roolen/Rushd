using System;
using System.Collections.Generic;
using System.Globalization;
using Assets.Scripts;
using Assets.Scripts.LevelGenerator;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets
{
    [CustomEditor(typeof(ContentManager))]
    public class EditorContentManager : Editor
    {
        public new ContentManager target;

        public int counterPlatforms;
        public int counterTanks;
        public int counterItems;

        public void OnEnable()
        {
            target = (ContentManager)base.target;
        }

        public override void OnInspectorGUI()
        {
            
            serializedObject.Update();

            if (GUI.Button(new Rect(180, 158, 80, 30), "Update"))
            {
                AssetDatabase.ForceReserializeAssets();

                Repaint();

                target.LastChange = DateTime.Now;
            }

            UpdatePlatforms();
            UpdateTanks();
            UpdateItems();

            EditorGUILayout.ObjectField(target.content, typeof(Content));

            EditorGUILayout.PrefixLabel(target.LastChange.ToString(CultureInfo.InvariantCulture));

            ShowPlatforms();
            EditorGUILayout.Space();

            ShowTanks();
            EditorGUILayout.Space();

            ShowItems();
        }

        private void ShowPlatforms()
        {
            EditorGUILayout.LabelField("Platforms: ", target.TypesPlatforms.Count.ToString());

            foreach (var platform in target.TypesPlatforms)
            {
                EditorGUILayout.ObjectField(platform.gameObject, typeof(Object), true);
            }
        }

        private void ShowTanks()
        {
            EditorGUILayout.LabelField("Tanks: ", target.TanksTypes.Count.ToString());

            foreach (var tank in target.TanksTypes)
            {
                EditorGUILayout.ObjectField(tank, typeof(Object), true);
            }
        }

        private void ShowItems()
        {
            EditorGUILayout.LabelField("Items: ", target.TypesItems.Count.ToString());

            foreach (var item in target.TypesItems)
            {
                EditorGUILayout.ObjectField(item, typeof(Object), true);
            }
        }

        private void UpdatePlatforms()
        {

            string[] platformS = AssetDatabase.FindAssets("l:Platform");
            List<GameObject> objectsPlatforms = new List<GameObject>();

            foreach (var platform in platformS)
            {
                objectsPlatforms.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(platform)));
            }

            foreach (var platform in objectsPlatforms)
            {
                if (platform.GetComponent<TypeElement>() == null)
                    platform.AddComponent<TypeElement>();
            }

            target.TypesPlatforms.Clear();
            foreach (var objectPlatform in objectsPlatforms)
            {
                target.TypesPlatforms.Add(objectPlatform);
            }
        }

        private void UpdateTanks()
        {
            string[] tankS = AssetDatabase.FindAssets("l:Tank");
            List<GameObject> objectsTanks = new List<GameObject>();

            foreach (var tank in tankS)
            {
                objectsTanks.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(tank)));
            }

            foreach (var tank in objectsTanks)
            {
                if (tank.GetComponent<TypeElement>() == null)
                    tank.AddComponent<TypeElement>();
            }

            target.TanksTypes.Clear();
            foreach (var objectTank in objectsTanks)
            {

                target.TanksTypes.Add(objectTank);
            }
        }

        private void UpdateItems()
        {
            string[] items = AssetDatabase.FindAssets("l:Item");
            List<GameObject> objectsItems = new List<GameObject>();

            foreach (var item in items)
            {
                objectsItems.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(item)));
            }

            foreach (var item in objectsItems)
            {
                if (item.GetComponent<TypeElement>() == null)
                    item.AddComponent<TypeElement>();
            }

            target.TypesItems.Clear();
            foreach (var objectItem in objectsItems)
            {

                target.TypesItems.Add(objectItem);
            }
        }
    }
}
