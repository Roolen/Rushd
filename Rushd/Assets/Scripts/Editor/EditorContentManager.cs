using System;
using System.Collections.Generic;
using Assets.Scripts;
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

            UpdatePlatforms();
            EditorGUILayout.Space();

            UpdateTanks();
            EditorGUILayout.Space();

            UpdateItems();

            this.Repaint();
        }

        private void UpdatePlatforms()
        {
            EditorGUILayout.LabelField("Platforms: ", target.TypesPlatforms.Count.ToString());

            string[] platforms = AssetDatabase.FindAssets("l:Platform");
            List<GameObject> objectsPlatforms = new List<GameObject>();

            foreach (var platform in platforms)
            {
                objectsPlatforms.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(platform)));
            }

            target.TypesPlatforms.Clear();
            foreach (var objectPlatform in objectsPlatforms)
            {
                
                target.TypesPlatforms.Add(objectPlatform);
            }

            foreach (var objectPlatform in objectsPlatforms)
            {
                EditorGUILayout.ObjectField(objectPlatform, typeof(Object), true);
            }
        }

        private void UpdateTanks()
        {
            EditorGUILayout.LabelField("Tanks: ", target.TanksTypes.Count.ToString());

            string[] tanks = AssetDatabase.FindAssets("l:Tank");
            List<GameObject> objectsTanks = new List<GameObject>();

            foreach (var tank in tanks)
            {
                objectsTanks.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(tank)));
            }

            target.TanksTypes.Clear();
            foreach (var objectTank in objectsTanks)
            {

                target.TanksTypes.Add(objectTank);
            }

            foreach (var objectTank in objectsTanks)
            {
                EditorGUILayout.ObjectField(objectTank, typeof(Object), true);
            }
        }

        private void UpdateItems()
        {
            EditorGUILayout.LabelField("Items: ", target.TypesItems.Count.ToString());

            string[] items = AssetDatabase.FindAssets("l:Item");
            List<GameObject> objectsItems = new List<GameObject>();

            foreach (var item in items)
            {
                objectsItems.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(item)));
            }

            target.TypesItems.Clear();
            foreach (var objectItem in objectsItems)
            {

                target.TypesItems.Add(objectItem);
            }

            foreach (var objectItem in objectsItems)
            {
                EditorGUILayout.ObjectField(objectItem, typeof(Object), true);
            }
        }
    }
}
