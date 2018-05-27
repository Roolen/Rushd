using UnityEngine;
using System;

namespace Assets.Scripts.LevelGenerator
{
    public class LevelBuilder : MonoBehaviour
    {
        public LevelData data;

        void Start ()
        {
            MakePlatforms();

            MakeTank();
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
                int i = 0;
                int b = 0;

                foreach (var platform in data.Platforms)
                {

                    GameObject platformInstance = Instantiate(data.typesPlatforms[(int)platform.TypePlatform]);

                    platformInstance.name = platform.NamePlatform;

                    platformInstance.tag = platform.TypePlatform.ToString();

                    platformInstance.transform.position = new Vector3(i * 15, 0, b * 15);


                    if (platform.ItemOnPlatform != null)
                    {
                        GameObject itemInstance = Instantiate(data.typesItems[(int) platform.ItemOnPlatform.TypeItem]);

                        itemInstance.name = platform.ItemOnPlatform.NameItem;

                        itemInstance.transform.position = new Vector3(i * 15, 5, b * 15);
                    }

                    if (platform.TankOnPlatform != null)
                    {
                        GameObject tankInstance = Instantiate(data.tanksTypes[(int) platform.TankOnPlatform.TypeTank]);

                        tankInstance.name = platform.TankOnPlatform.NameTank;

                        {
                            tankInstance.AddComponent<TankController>();
                            tankInstance.AddComponent<TankBot>();
                            tankInstance.GetComponent<TankBot>().targetPoint = GameObject.Find(platform.TankOnPlatform.TargetPoint);
                        }

                        tankInstance.transform.position = new Vector3(i * 15, 5, b * 15);
                        tankInstance.transform.Rotate(0, platform.TankOnPlatform.RotateTank, 0);
                    }

                    i++;

                    if (i == data.Height) { b++; i = 0; }
                }
            }
            else
            {
                Debug.LogWarning("Не удалось загрузить платформы на сцену");
            }
        }
    }
}
