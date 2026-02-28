namespace ExoLab
{
    using UnityEngine;
    using LiteDB;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class StructurePersistence : MonoBehaviour
    {
        private const string DB_FILENAME = "ExoLab.db";

        private string dbPath => Path.Combine(Application.persistentDataPath, DB_FILENAME);

        private LiteDatabase db;
        private ILiteCollection<AssembledStructure> savedStructures;

        public static StructurePersistence Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeDatabase();
        }

        private void OnDestroy()
        {
            db?.Dispose();
        }

        private void InitializeDatabase()
        {
            try
            {
                db = new LiteDatabase(dbPath);
                savedStructures = db.GetCollection<AssembledStructure>("structures");
                savedStructures.EnsureIndex(x => x.structureId, true); // уникальный индекс
                Debug.Log($"LiteDB initialized at: {dbPath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize LiteDB: {ex.Message}");
            }
        }

        // Сохранить или обновить конструкцию
        public void SaveStructure(AssembledStructure structure)
        {
            if (string.IsNullOrEmpty(structure.structureId))
            {
                Debug.LogError("Cannot save structure: missing ID");
                return;
            }

            try
            {
                savedStructures.Upsert(structure);
                Debug.Log($"Saved structure: {structure.structureId}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save structure: {ex.Message}");
            }
        }

        // Загрузить конструкцию по ID
        public AssembledStructure LoadStructure(string structureId)
        {
            try
            {
                return savedStructures.FindById(structureId);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load structure {structureId}: {ex.Message}");
                return null;
            }
        }

        // Загрузить все конструкции (опционально)
        public List<AssembledStructure> LoadAllStructures()
        {
            return savedStructures.FindAll().ToList();
        }

        // Удалить конструкцию
        public void DeleteStructure(string structureId)
        {
            savedStructures.Delete(structureId);
        }
    }
}
