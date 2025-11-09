namespace ExoLab.Service
{
    using ExoLab.Assembly;
    using UnityEngine;

    public class WeaponPreviewManager : MonoBehaviour
    {
        public static WeaponPreviewManager Instance;

        [Header("Настройки рендера")]
        public Camera previewCamera;
        public Transform renderRoot; // место, куда инстанцируем оружие
        public int textureSize = 256;
        public LayerMask previewLayer; // чтобы основная камера не видела превью

        private RenderTexture workingRT;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            // Настройка камеры
            previewCamera.targetTexture = null;
            previewCamera.cullingMask = previewLayer;
            previewCamera.clearFlags = CameraClearFlags.SolidColor;
            previewCamera.backgroundColor = Color.clear;
            previewCamera.orthographic = true;
            previewCamera.orthographicSize = 1.0f;
        }

        public RenderTexture RenderWeapon(WeaponBuild build)
        {
            string hash = build.GetHash();

            // Проверяем кэш
            if (PreviewCache.TryGet(hash, out var cached))
                return cached;

            // Удаляем старый инстанс
            foreach (Transform child in renderRoot)
                Destroy(child.gameObject);

            // Создаём базовое оружие
            GameObject baseWeapon = Instantiate(build.GetBasePrefab(), renderRoot);
            baseWeapon.layer = LayerMask.NameToLayer("Preview"); // убедись, что слой существует

            // === Здесь твоя логика установки обвесов ===
            // Например:
            // foreach (var attId in build.attachments)
            //     AttachPart(baseWeapon, attId);

            // Просто поворачиваем для вида
            baseWeapon.transform.localRotation = Quaternion.Euler(15, -30, 0);

            // Готовим RenderTexture
            if (workingRT == null || workingRT.width != textureSize)
            {
                if (workingRT != null)
                {
                    workingRT.Release();
                    DestroyImmediate(workingRT);
                }
                workingRT = new RenderTexture(textureSize, textureSize, 24, RenderTextureFormat.Default);
            }

            previewCamera.targetTexture = workingRT;
            previewCamera.Render();

            // Копируем результат в новую текстуру (чтобы не зависеть от workingRT)
            RenderTexture finalRT = new RenderTexture(textureSize, textureSize, 0);
            Graphics.Blit(workingRT, finalRT);

            // Сохраняем в кэш
            PreviewCache.Set(hash, finalRT);

            // Очищаем временную модель
            foreach (Transform child in renderRoot)
                Destroy(child.gameObject);

            return finalRT;
        }

        void OnDestroy()
        {
            if (workingRT != null)
            {
                workingRT.Release();
                DestroyImmediate(workingRT);
            }
            PreviewCache.ClearAll();
        }
    }
}
