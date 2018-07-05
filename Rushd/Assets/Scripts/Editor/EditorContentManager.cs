using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;

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
            counterPlatforms = EditorGUILayout.IntField("Count of platforms: ", counterPlatforms);

            if (GUILayout.Button("Add platforms"))
            {
                for (int i = 0; i < counterPlatforms; i++)
                {
                    if (i > target.TypesPlatforms.Count - 1)
                    {
                        target.TypesPlatforms.Add(null);
                    }
                }
            }

            EditorGUILayout.LabelField("Current platforms: ", target.TypesPlatforms.Count.ToString());

            for (int i = 0; i < target.TypesPlatforms.Count; i++)
            {
                target.TypesPlatforms[i] = (GameObject)EditorGUILayout.ObjectField(target.TypesPlatforms[i], typeof(Object), true);
            }
        }

        private void UpdateTanks()
        {
            counterTanks = EditorGUILayout.IntField("Count of tanks: ", counterTanks);

            if (GUILayout.Button("Add tanks"))
            {
                for (int i = 0; i < counterTanks; i++)
                {
                    if (i > target.TanksTypes.Count - 1)
                    {
                        target.TanksTypes.Add(null);
                    }
                }
            }

            EditorGUILayout.LabelField("Current tanks: ", target.TanksTypes.Count.ToString());

            for (int i = 0; i < target.TanksTypes.Count; i++)
            {
                target.TanksTypes[i] = (GameObject)EditorGUILayout.ObjectField(target.TanksTypes[i], typeof(Object), true);
            }
        }

        private void UpdateItems()
        {
            counterItems = EditorGUILayout.IntField("Count of items: ", counterItems);

            if (GUILayout.Button("Add items"))
            {
                for (int i = 0; i < counterItems; i++)
                {
                    if (i > target.TypesItems.Count - 1)
                    {
                        target.TypesItems.Add(null);
                    }
                }
            }

            EditorGUILayout.LabelField("Current items: ", target.TypesItems.Count.ToString());

            for (int i = 0; i < target.TypesItems.Count; i++)
            {
                target.TypesItems[i] = (GameObject)EditorGUILayout.ObjectField(target.TypesItems[i], typeof(Object), true);
            }
        }
    }
}
