using Sirenix.OdinInspector;
using UnityEngine;

public class testfill : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    
    
    [Button]
    private void Fill()
    {
        float size = 115.71f;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Debug.LogError(new Vector3(size * i, size * j));
            }
        }
    }
}
