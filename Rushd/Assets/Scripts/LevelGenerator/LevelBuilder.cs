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
                int i = 0;
                int b = 0;

                foreach (var platform in data.Platforms)
                {

                    GameObject platformInstance = Instantiate(data.typesPlatforms[ (int)platform.TypePlatform] );

                    platformInstance.name = platform.NamePlatform;

                    platformInstance.transform.position = new Vector3(i * 15, 0, b * 15);

                    i++;

                    if (i == data.Height) {b++; i = 0;}
                }
            }
        }
    }
}
