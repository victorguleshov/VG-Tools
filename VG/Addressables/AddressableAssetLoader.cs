using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace VG.Addressables
{
    // ReSharper disable once ClassNeverInstantiated.Global (Instantiated by Zenject);
    public static class AddressableAssetsLoader
    {
        // ReSharper disable once CollectionNeverQueried.Local;
        private static readonly Dictionary<string, Object> cachedAssets = new();

        public static async UniTaskVoid LoadAndCacheAssetAsync<T>(string key, Action<T> onReady,
            bool releaseImmediate = false) where T : Object
        {
            var asset = await LoadAndCacheAssetAsync<T>(key, releaseImmediate);
            onReady?.Invoke(asset);
        }

        public static async UniTaskVoid LoadAndCacheAssetAsync<T>(string key, Action<string, T> onReady,
            bool releaseImmediate = false) where T : Object
        {
            var asset = await LoadAndCacheAssetAsync<T>(key, releaseImmediate);
            onReady?.Invoke(key, asset);
        }

        public static async UniTaskVoid LoadAndCacheAssetAsync<TO, T1>(string key, Action<Object> onReady,
            bool releaseImmediate = false)
            where TO : Object
            where T1 : Object
        {
            var asset = await LoadAndCacheAssetOrObjectAsync<TO, T1>(key, releaseImmediate);
            onReady?.Invoke(asset);
        }

        public static async UniTaskVoid LoadAndCacheAssetAsync<TO, T1>(string key, Action<string, Object> onReady,
            bool releaseImmediate = false)
            where TO : Object
            where T1 : Object
        {
            var asset = await LoadAndCacheAssetOrObjectAsync<TO, T1>(key, releaseImmediate);
            onReady?.Invoke(key, asset);
        }

        public static async UniTaskVoid LoadAndApplySpriteAsync(string key, Image image, Action<bool> callback = null)
        {
            var spriteAsset = await LoadAndCacheAssetAsync<Sprite>(key);

            if (image && spriteAsset)
            {
                image.sprite = spriteAsset;

                callback?.Invoke(true);
                return;
            }

            callback?.Invoke(false);
        }

        public static async UniTaskVoid LoadAndApplySpriteAsync(string key, SpriteRenderer image,
            Action<bool> callback = null)
        {
            var spriteAsset = await LoadAndCacheAssetAsync<Sprite>(key);

            if (image && spriteAsset)
            {
                image.sprite = spriteAsset;

                callback?.Invoke(true);
                return;
            }

            callback?.Invoke(false);
        }

        public static async UniTask<T> LoadAndCacheAssetAsync<T>(string key, bool releaseImmediate = false)
            where T : Object
        {
            if (string.IsNullOrWhiteSpace(key) == false)
            {
                if (cachedAssets != null &&
                    cachedAssets.TryGetValue(key, out var cachedAsset))
                    if (cachedAsset &&
                        cachedAsset is T asset)
                        return asset;

                var resourcesAtPath = await UnityEngine.AddressableAssets.Addressables.LoadResourceLocationsAsync(key);
                if (resourcesAtPath.Count == 0) return default;

                var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(key);

                var result = await handle;

                if (result)
                {
                    if (releaseImmediate == false)
                        cachedAssets[key] = result;
                    else
                        UnityEngine.AddressableAssets.Addressables.Release(handle);

                    return result;
                }

                UnityEngine.AddressableAssets.Addressables.Release(handle);
            }

            return default;
        }

        public static async UniTask<Object> LoadAndCacheAssetOrObjectAsync<TO, T1>(string key,
            bool releaseImmediate = false)
            where TO : Object
            where T1 : Object
        {
            if (string.IsNullOrWhiteSpace(key) == false)
            {
                if (cachedAssets != null &&
                    cachedAssets.TryGetValue(key, out var cachedAsset))
                    if (cachedAsset &&
                        cachedAsset is TO asset)
                        return asset;

                var resourcesAtPath = await UnityEngine.AddressableAssets.Addressables.LoadResourceLocationsAsync(key);
                if (resourcesAtPath.Count == 0) return default;

                var haveSelectedType1 = false;
                var haveSelectedType2 = false;
                foreach (var VARIABLE in resourcesAtPath)
                    if (VARIABLE.ResourceType == typeof(TO))
                    {
                        haveSelectedType1 = true;
                        break;
                    }
                    else if (VARIABLE.ResourceType == typeof(T1))
                    {
                        haveSelectedType2 = true;
                        break;
                    }

                if (haveSelectedType1)
                {
                    var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<TO>(key);

                    var result = await handle;

                    if (result)
                    {
                        if (releaseImmediate == false)
                            cachedAssets[key] = result;
                        else
                            UnityEngine.AddressableAssets.Addressables.Release(handle);

                        return result;
                    }

                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                }
                else if (haveSelectedType2)
                {
                    var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T1>(key);

                    var result = await handle;

                    if (result)
                    {
                        if (releaseImmediate == false)
                            cachedAssets[key] = result;
                        else
                            UnityEngine.AddressableAssets.Addressables.Release(handle);

                        return result;
                    }

                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                }
                else
                {
                    var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Object>(key);

                    var result = await handle;

                    if (result)
                    {
                        if (releaseImmediate == false)
                            cachedAssets[key] = result;
                        else
                            UnityEngine.AddressableAssets.Addressables.Release(handle);

                        return result;
                    }

                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                }
            }

            return default;
        }
    }
}