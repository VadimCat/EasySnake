using Ji2.Utils;
using UnityEngine;

namespace Views
{
    [CreateAssetMenu(menuName = "FoodViewConfig", fileName = "Configs/Views/FoodViewConfig", order = 0)]
    public class FoodViewConfig : ScriptableObject
    {
        const string IndexSaveKey = "FoodSprites";
        [SerializeField] private Sprite[] sprites;

        private CircularSavableArray<Sprite> _savableArray;

        public Sprite GetNextSprite()
        {
            if (_savableArray == null)
            {
                _savableArray = new CircularSavableArray<Sprite>(sprites, IndexSaveKey);
            }
            
            return _savableArray.GetNext();
        }
    }
}