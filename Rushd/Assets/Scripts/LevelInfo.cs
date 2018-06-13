using System.IO;
using Assets.Scripts.LevelGenerator;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LevelInfo : MonoBehaviour
    {
        [Tooltip("Текст кнопки в который будет записываться название уровня")]
        [Header("Текст с названием")]
        public Text textButtonLevel;

        [SerializeField]
        private string nameLevel;
        private string descriptionLevel;
        private Difficult difficultLevel;
        private int heightLevel;
        private int weightLevel;
        private FileInfo fileLevel;

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

        public FileInfo FileLevel
        {
            get
            {
                return fileLevel;
            }

            set
            {
                fileLevel = value;
            }
        }
    }
}
