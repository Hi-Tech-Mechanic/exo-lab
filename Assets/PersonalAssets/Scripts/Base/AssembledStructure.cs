namespace ExoLab
{
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;
    using LiteDB;
    using UnityEngine;

    public abstract class AssembledStructure : MonoBehaviour
    {
        [BsonId]
        public string structureId;

        public List<AssemblyComponentBase> components = new();

        public AssembledStructure() { }

        public AssembledStructure(string id)
        {
            structureId = id;
        }

        protected abstract void Save();

        protected abstract void Load();
    }
}
