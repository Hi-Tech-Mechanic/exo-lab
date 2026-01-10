namespace ExoLab.Helpers
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class SystemExtensions
    {
        public static Dictionary<T1, T2> AddRange<T1, T2>(this Dictionary<T1, T2> dictionary, Dictionary<T1, T2> addedDictionary)
        {
            foreach (var keyValuePair in addedDictionary)
            {
                dictionary.TryAdd(keyValuePair.Key, keyValuePair.Value);
            }

            return dictionary;
        }

        public static GameObject? TryGetChildWithTag(this GameObject gameObject, string tag)
        {
            var result = gameObject.TryGetComponentWithTag<Transform>(tag);
            if (result != null)
            {
                return gameObject.TryGetComponentWithTag<Transform>(tag).gameObject;
            }

            return null;
        }

        public static T TryGetComponentWithTag<T>(this GameObject gameObject, string tag)
        {
            var childs = gameObject.GetChilds();

            foreach (var child in childs)
            {
                if (child.tag.Equals(tag))
                {
                    return child.GetComponent<T>();
                }
            }

            return default;
        }

        public static GameObject[] GetChilds(this GameObject gameObject)
        {
            var childs = new List<GameObject>();
            var transforms = gameObject.GetComponentsInChildren<Transform>();

            foreach (var transform in transforms)
            {
                childs.Add(transform.gameObject);
            }

            return childs.ToArray();
        }

        public static void RemoveRange<T>(this List<T> list, List<T> removableObjects)
        {
            list.RemoveRange(removableObjects.ToArray());
        }

        public static void RemoveRange<T>(this List<T> list, T[] removableObjects)
        {
            foreach (var targetObject in removableObjects)
            {
                list.Remove(targetObject);
            }
        }
    }
}
