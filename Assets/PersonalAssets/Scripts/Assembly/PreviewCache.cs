// Assets/Scripts/PreviewCache.cs
using UnityEngine;
using System.Collections.Generic;

public static class PreviewCache
{
    private static Dictionary<string, RenderTexture> cache = new();

    public static bool TryGet(string key, out RenderTexture rt)
    {
        return cache.TryGetValue(key, out rt) && rt != null;
    }

    public static void Set(string key, RenderTexture rt)
    {
        if (cache.TryGetValue(key, out var old) && old != null)
        {
            old.Release();
            Object.DestroyImmediate(old);
        }

        cache[key] = rt;
    }

    public static void ClearAll()
    {
        foreach (var kvp in cache)
        {
            if (kvp.Value != null)
            {
                kvp.Value.Release();
                Object.DestroyImmediate(kvp.Value);
            }
        }
        cache.Clear();
    }
}