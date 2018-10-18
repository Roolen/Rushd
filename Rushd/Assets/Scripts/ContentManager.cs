using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class ContentManager : MonoBehaviour
    {
        public Content content;
        private string pathContent;

        public List<GameObject> TanksTypes
        {
            get
            {
                return content.TanksTypes;
            }

            set
            {
                content.TanksTypes = value;
            }
        }

        public List<GameObject> TypesPlatforms
        {
            get
            {
                return content.TypesPlatforms;
            }

            set
            {
                content.TypesPlatforms = value;
            }
        }

        public List<GameObject> TypesItems
        {
            get
            {
                return content.TypesItems;
            }

            set
            {
                content.TypesItems = value;
            }
        }

        public DateTime LastChange
        {
            get { return content.lastChange; }
            set
            {
                if (content != null) content.lastChange = value;
            }
        }

        public void UpdateContent()
        {
            pathContent = "Assets/GameObjects/SystemObjects/Content.prefab";

            content = AssetDatabase.LoadAssetAtPath<Content>(pathContent).GetComponent<Content>();
        }

        public void SaveContent()
        {
            AssetDatabase.ForceReserializeAssets();

            UpdateContent();
        }
    }
}
