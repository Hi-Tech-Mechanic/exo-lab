namespace ExoLab.Items
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Utility for working with GameObject layers.
    /// </summary>
    public static class LayerHelper
    {
        /// <summary>
        /// Sets the layer to the object and all its children recursively.
        /// </summary>
        /// <param name="root">Root object</param>
        /// <param name="layer">Layer index to set</param>
        public static void SetLayerRecursively(GameObject root, int layer)
        {
            root.layer = layer;

            foreach (Transform child in root.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        /// <summary>
        /// Collects the object and all its children with their current layers.
        /// </summary>
        /// <param name="root">Root object</param>
        /// <returns>List of objects with their original layers</returns>
        public static List<LayerEntry> CollectLayers(GameObject root)
        {
            var result = new List<LayerEntry>();
            CollectLayersRecursively(root, result);
            return result;
        }

        /// <summary>
        /// Restores the layers for all collected objects.
        /// </summary>
        /// <param name="entries">Layer entries to restore</param>
        public static void RestoreLayers(List<LayerEntry> entries)
        {
            foreach (var entry in entries)
            {
                entry.obj.layer = entry.layer;
            }
        }

        /// <summary>
        /// Gets the first enabled bit index from a LayerMask.
        /// </summary>
        /// <param name="layerMask">Layer mask</param>
        /// <returns>Layer index</returns>
        public static int GetFirstEnabledLayer(LayerMask layerMask)
        {
            for (int i = 0; i < 32; i++)
            {
                if ((layerMask.value & (1 << i)) != 0)
                {
                    return i;
                }
            }

            return 0;
        }

        private static void CollectLayersRecursively(GameObject root, List<LayerEntry> collected)
        {
            collected.Add(new LayerEntry(root, root.layer));

            foreach (Transform child in root.transform)
            {
                CollectLayersRecursively(child.gameObject, collected);
            }
        }

        /// <summary>
        /// Represents a GameObject with its original layer index.
        /// </summary>
        public readonly struct LayerEntry
        {
            public readonly GameObject obj;
            public readonly int layer;

            public LayerEntry(GameObject obj, int layer)
            {
                this.obj = obj;
                this.layer = layer;
            }
        }
    }
}