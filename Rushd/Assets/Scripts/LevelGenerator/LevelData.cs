using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelGenerator
{
    /// <summary>
    /// Уровень сложности.
    /// </summary>
    public enum Difficult
    {
        Easy,
        Medium,
        Hard
    }

    /// <summary>
    /// Типы игровой платформы.
    /// </summary>
    public enum TypesPlatform
    {
        LandingPlatform,
        EvacuationPlatform,
        PlatformOpenSpace,
        PlatformWall
    }

    /// <summary>
    /// Тип предмета.
    /// </summary>
    public enum TypesItem
    {
        Flag,
        RepairKit,
        WeaponCharges
    }

    public class LevelData : MonoBehaviour
    {
        [Header("Платформы")]
        public GameObject platformOpenSpace;

        public GameObject tankPlayer;

        public GameObject[] typesPlatforms;

        public GameObject[] typesItems;

        private string nameLevel;
        private Difficult difficultLevel;

        private int heightLevel;
        private int weightLevel;

        private string lightColor;

        private List<Platform> platforms = new List<Platform>();

        /// <summary>
        /// Массив платформ из которых состоит игровой уровень.
        /// </summary>
        public List<Platform> Platforms
        {
            get
            {
                return platforms;
            }

            set
            {
                if (value != null)
                {
                    platforms = value;
                }
            }
        }

        /// <summary>
        /// Название игрового уровня.
        /// </summary>
        public string NameLevel
        {
            get
            {
                return nameLevel;
            }

            set
            {
                if (value != null)
                {
                    nameLevel = value;
                }
            }
        }

        /// <summary>
        /// Высота уровня в клетках.
        /// </summary>
        public int Height
        {
            get
            {
                return heightLevel;
            }

            set
            {
                if (value <= 32)
                {
                    heightLevel = value;
                }
            }
        }

        /// <summary>
        /// Ширина уровня в клетках.
        /// </summary>
        public int Weight
        {
            get
            {
                return weightLevel;
            }

            set
            {
                if (value <= 32)
                {
                    weightLevel = value;
                }
            }
        }

        /// <summary>
        /// Сложность игрового уровня.
        /// </summary>
        internal Difficult DifficultLevel
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

        /// <summary>
        /// Цвет солнца на уровне.
        /// </summary>
        public string LightColor
        {
            get
            {
                return lightColor;
            }

            set
            {
                lightColor = value;
            }
        }

        /// <summary>
        /// Создает экземпляр класса LevelData.
        /// </summary>
        /// <param name="levelName">Название уровня</param>
        /// <param name="heightLevel">Высота уровня</param>
        /// <param name="weightLevel">Ширина уровня</param>
        public LevelData(string levelName, int heightLevel, int weightLevel)
        {
            NameLevel = levelName;
            Height = heightLevel;
            Weight = weightLevel;
        }
    }

    /// <summary>
    /// Экземпляр игровой платформы.
    /// </summary>
    public class Platform
    {
        private string namePlatform;
        private TypesPlatform typePlatform;
        private Item itemOnPlatform;

        /// <summary>
        /// Предмет находящийся на игровой платформе.
        /// </summary>
        public Item ItemOnPlatform
        {
            get
            {
                return itemOnPlatform;
            }

            set
            {
                if (value != null)
                {
                    itemOnPlatform = value;
                }
            }
        }

        /// <summary>
        /// Название игровой платформы.
        /// </summary>
        public string NamePlatform
        {
            get
            {
                return namePlatform;
            }

            set
            {
                if (value != null)
                {
                    namePlatform = value;
                }
            }
        }

        /// <summary>
        /// Тип игровой платформы.
        /// </summary>
        internal TypesPlatform TypePlatform
        {
            get
            {
                return typePlatform;
            }

            set
            {
                typePlatform = value;
            }
        }

        /// <summary>
        /// Создает экземпляр игровой платформы.
        /// </summary>
        /// <param name="namePlatform">Название платформы</param>
        /// <param name="typePlatform">Тип платформы</param>
        /// <param name="itemPlatform">Предмет содержащийся на платформе.</param>
        public Platform(string namePlatform, TypesPlatform typePlatform, Item itemPlatform)
        {
            NamePlatform = namePlatform;
            TypePlatform = typePlatform;
            ItemOnPlatform = itemPlatform;
        }

        public Platform()
        {
            NamePlatform = null;
            TypePlatform = TypesPlatform.PlatformOpenSpace;
            ItemOnPlatform = null;

        }
    }

    public class Item
    {
        private string nameItem;
        private TypesItem typeItem;

        /// <summary>
        /// Название платформы.
        /// </summary>
        public string NameItem
        {
            get
            {
                return nameItem;
            }

            set
            {
                if (value != null)
                {
                    nameItem = value;
                }
            }
        }

        /// <summary>
        /// Тип платформы.
        /// </summary>
        public TypesItem TypeItem
        {
            get
            {
                return typeItem;
            }

            set
            {
                typeItem = value;
            }
        }

        /// <summary>
        /// Создает экземпляр предмета.
        /// </summary>
        /// <param name="nameItem">Название предмета</param>
        /// <param name="typeItem">Тип предмета</param>
        public Item(string nameItem, TypesItem typeItem)
        {
            NameItem = nameItem;
            TypeItem = typeItem;
        }

        public  Item() { }
    }
}