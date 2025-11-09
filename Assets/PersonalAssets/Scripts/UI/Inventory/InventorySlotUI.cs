namespace ExoLab.UI
{
    using ExoLab.Assembly;
    using ExoLab.Service;
    using UnityEngine;
    using UnityEngine.UI;

    public class InventorySlotUI : MonoBehaviour // текстовый прогон от ии, todo автогенерация изображений
    {
        public RawImage rawImage;
        public WeaponBuild currentBuild;

        void Start()
        {
            if (currentBuild != null && rawImage != null)
            {
                RenderPreview();
            }
        }

        public void SetWeapon(WeaponBuild build)
        {
            currentBuild = build;
            RenderPreview();
        }

        void RenderPreview()
        {
            if (currentBuild == null) return;

            RenderTexture rt = WeaponPreviewManager.Instance.RenderWeapon(currentBuild);
            rawImage.texture = rt;
        }
    }
}