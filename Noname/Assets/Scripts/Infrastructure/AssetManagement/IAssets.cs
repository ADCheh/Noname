using System.Threading.Tasks;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 at);
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        void CleanUp();
        Task<T> Load<T>(string address) where T : class;
        void Initialize();
    }
}