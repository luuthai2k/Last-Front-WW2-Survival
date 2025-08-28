using UnityEngine;
using UnityEngine.AddressableAssets;
namespace InviGiant.Tools
{
    public class InstantiteSpawPrefabs : MonoBehaviour
    {
        public AssetReference item;
        void Start()
        {
            item.LoadAssetAsync<GameObject>().Completed += (UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj) =>
            {
                SmartPool.Instance.Preload(obj.Result, 1);
            };
        }
    }
}
