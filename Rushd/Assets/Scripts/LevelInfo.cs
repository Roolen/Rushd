using Assets.Scripts.LevelGenerator;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LevelInfo : MonoBehaviour
    {
        public Text textButtonLevel;

        public string nameLevel;
        private string descriptionLevel;
        private Difficult difficultLevel;
        private int heightLevel;
        private int weightLevel;

        public string NameLevel
        {
            get
            {
                return nameLevel;
            }

            set
            {
                nameLevel = value;
            }
        }

        public string DescriptionLevel
        {
            get
            {
                return descriptionLevel;
            }

            set
            {
                descriptionLevel = value;
            }
        }

        public Difficult DifficultLevel
        {
            get
            {
                return difficultLevel;
            }

            set
            {
                difficultLevel = value;
            }
        }

        public int HeightLevel
        {
            get
            {
                return heightLevel;
            }

            set
            {
                heightLevel = value;
            }
        }

        public int WeightLevel
        {
            get
            {
                return weightLevel;
            }

            set
            {
                weightLevel = value;
            }
        }
    }
}
