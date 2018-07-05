using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ContentManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> tanksTypes = new List<GameObject>();

        [SerializeField] private List<GameObject> typesPlatforms = new List<GameObject>();

        [SerializeField] private List<GameObject> typesItems = new List<GameObject>();

        public List<GameObject> TanksTypes
        {
            get
            {
                return tanksTypes;
            }

            set
            {
                tanksTypes = value;
            }
        }

        public List<GameObject> TypesPlatforms
        {
            get
            {
                return typesPlatforms;
            }

            set
            {
                typesPlatforms = value;
            }
        }

        public List<GameObject> TypesItems
        {
            get
            {
                return typesItems;
            }

            set
            {
                typesItems = value;
            }
        }
    }
}
