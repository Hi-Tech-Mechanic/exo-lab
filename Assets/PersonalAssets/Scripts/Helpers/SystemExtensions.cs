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

        /// <summary>
        /// Ищет среди всех детей объекта, даже не активных
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject? TryGetChildWithName(this GameObject gameObject, string name)
        {
            var childs = gameObject.transform.GetComponentsInChildren<Transform>(true);

            foreach (var child in childs)
            {
                if (child.name == name)
                {
                    return child.gameObject;
                }
            }

            return null;
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

        public static void AddRange<T>(this List<T> list, List<T> additionalObjects)
        {
            list.AddRange(additionalObjects.ToArray());
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

        /// <summary>
        /// Вернуть первый найденный или <see cref="T"/>> по умолчанию
        /// компонент. Ищет через <see cref="GameObject.GetComponents"/>>, может быть долгим иногда
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T FirstOrDefaultComponent<T>(ICollection<GameObject> collection)
        {
            foreach (var item in collection)
            {
                var typedItem = item.GetComponents<T>();
                if (typedItem.Length != 0)
                {
                    return typedItem[0];
                }
            }

            return default;
        }
    }
}
