using UnityEngine;

namespace Assets.Scripts.LevelGenerator
{
    public class TypeElement : MonoBehaviour
    {
        public int typeGameObject;
        public int universalType;

        public TypeElement(int typeGameObject, int universalType)
        {
            this.typeGameObject = typeGameObject;
            this.universalType = universalType;
        }
    }
}