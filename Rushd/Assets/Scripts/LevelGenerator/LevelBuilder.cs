using UnityEngine;
using System;

namespace Assets.Scripts.LevelGenerator
{
    public class LevelBuilder : MonoBehaviour
    {
        public LevelData data;

        void Start ()
        {
            if (data.Platforms.Count > 0)
            {
                for (int i = 0; i < data.Platforms.Count; i++)
                {
                    Platform platform = data.Platforms[i];

                    GameObject platformInstance = new GameObject();
                    platformInstance.name = platform.NamePlatform;

                    //Instantiate(platform);
                }
            }
        }
    }
}
