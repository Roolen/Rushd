using UnityEngine;
using System;
using Assets.Scripts.Controllers;
using UnityEngine.AI;
using UnityStandardAssets.Cameras;

namespace Assets.Scripts.LevelGenerator
{
    public class LevelBuilder : MonoBehaviour
    {
        public LevelData data;

        [SerializeField] [Range(0, 2)] private int typePlayerTank;

        public ContentManager content;

        public bool editorMode;

        void Start ()
        {
            if (data.Platforms != null)
            {
                MakePlatforms();

                if (!editorMode) MakeTank();
            }

            MakeNavMesh();
        }

        private void MakeTank()
        {
            GameObject landingPlatform = GameObject.FindWithTag("LandingPlatform");

            GameObject tankInstance = Instantiate(content.TanksTypes[typePlayerTank]);

            tankInstance.name = "PlayerTank";

            {
                tankInstance.AddComponent<PlayerController>();
                GameObject.FindObjectOfType<FreeLookCam>().GetComponent<FreeLookCam>().SetTarget(tankInstance.transform);
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

                    GameObject platformInstance = Instantiate(content.TypesPlatforms[(int) platform.TypePlatform]);

                    if (editorMode)
                    {
                        EditorElement edElement = platformInstance.AddComponent<EditorElement>();
                        edElement.typeElement = 0;
                        edElement.nameElement = platform.NamePlatform;
                        edElement.universalType = (int)platform.TypePlatform;

                        if (platform.TypePlatform == TypesPlatform.LandingPlatform) { edElement.thisLandingPlatform = true; }
                    }

                    platformInstance.name = platform.NamePlatform;

                    if (platform.TypePlatform == TypesPlatform.LandingPlatform)
                    {
                        platformInstance.tag = platform.TypePlatform.ToString();
                    }

                    platformInstance.transform.position = new Vector3(x * 15, 0, z * 15);


                    if (platform.ItemOnPlatform != null)
                    {
                        GameObject itemInstance = Instantiate(content.TypesItems[(int)platform.ItemOnPlatform.TypeItem]);

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
                        GameObject tankInstance = Instantiate(content.TanksTypes[(int)platform.TankOnPlatform.TypeTank], new Vector3(x*15, 5, z*15), new Quaternion());

                        if (editorMode)
                        {
                            EditorElement edElement = tankInstance.AddComponent<EditorElement>();
                            edElement.typeElement = 2;
                            platformInstance.GetComponent<EditorElement>().elementOn = tankInstance;
                        }

                        if (!editorMode)
                        {
                            tankInstance.AddComponent<BotController>();
                            //tankInstance.GetComponent<BotController>().defenseTarget = GameObject.Find(platform.TankOnPlatform.TargetPoint);
                        }

                        tankInstance.name = platform.TankOnPlatform.NameTank;
                        tankInstance.transform.Rotate(0, platform.TankOnPlatform.RotateTank, 0);

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

        private void MakeNavMesh()
        {

            NavMeshSurface navMesh = gameObject.AddComponent<NavMeshSurface>();

            navMesh.tileSize = 4;

            navMesh.BuildNavMesh();
        }
    }
}
