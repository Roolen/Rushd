using UnityEngine;
using System;

namespace Assets.Scripts.LevelGenerator
{
    public class LevelBuilder : MonoBehaviour
    {
        public LevelData data;

        public bool editorMode;

        void Start ()
        {
            if (data.Platforms != null)
            {
                MakePlatforms();


                if (!editorMode) MakeTank();
            }

        }

        private void MakeTank()
        {
            GameObject landingPlatform = GameObject.FindWithTag("LandingPlatform");

            GameObject tankInstance = Instantiate(data.tanksTypes[1]);

            tankInstance.name = "PlayerTank";

            {
                tankInstance.AddComponent<TankController>();
                tankInstance.AddComponent<PlayerController>();
            }

            tankInstance.transform.position = new Vector3(landingPlatform.transform.position.x, 5, landingPlatform.transform.position.z);
        }

        private void MakePlatforms()
        {
            if (data.Platforms.Count > 0)
            {
                int x = 0;
                int z = 0;

                foreach (var platform in data.Platforms)
                {

                    GameObject platformInstance = Instantiate(data.typesPlatforms[(int)platform.TypePlatform]);

                    if (editorMode)
                    {
                        EditorElement edElement = platformInstance.AddComponent<EditorElement>();
                        edElement.typeElement = 0;
                        edElement.nameElement = platform.NamePlatform;
                        edElement.universalType = (int)platform.TypePlatform;

                        if (platform.TypePlatform == TypesPlatform.LandingPlatform) { edElement.thisLandingPlatform = true; }
                    }

                    platformInstance.name = platform.NamePlatform;

                    platformInstance.tag = platform.TypePlatform.ToString();

                    platformInstance.transform.position = new Vector3(x * 15, 0, z * 15);


                    if (platform.ItemOnPlatform != null)
                    {
                        GameObject itemInstance = Instantiate(data.typesItems[(int) platform.ItemOnPlatform.TypeItem]);

                        if (editorMode)
                        {
                            EditorElement edElement = itemInstance.AddComponent<EditorElement>();
                            edElement.typeElement = 1;
                            platformInstance.GetComponent<EditorElement>().elementOn = itemInstance;
                        }

                        itemInstance.name = platform.ItemOnPlatform.NameItem;

                        itemInstance.transform.position = new Vector3(x * 15, 5, z * 15);
                    }

                    if (platform.TankOnPlatform != null)
                    {
                        GameObject tankInstance = Instantiate(data.tanksTypes[(int) platform.TankOnPlatform.TypeTank], new Vector3(x*15, 5, z*15), new Quaternion());

                        if (editorMode)
                        {
                            EditorElement edElement = tankInstance.AddComponent<EditorElement>();
                            edElement.typeElement = 2;
                            platformInstance.GetComponent<EditorElement>().elementOn = tankInstance;
                        }

                        tankInstance.name = platform.TankOnPlatform.NameTank;
                        tankInstance.transform.Rotate(0, platform.TankOnPlatform.RotateTank, 0);

                        {
                            tankInstance.AddComponent<TankController>();
                            tankInstance.AddComponent<TankBot>();
                            tankInstance.GetComponent<TankBot>().targetPoint = GameObject.Find(platform.TankOnPlatform.TargetPoint);
                        }
                    }

                    x++;

                    if (x == data.Height) { z++; x = 0; }
                }
            }
            else
            {
                Debug.LogWarning("Не удалось загрузить платформы на сцену");
            }
        }
    }
}
