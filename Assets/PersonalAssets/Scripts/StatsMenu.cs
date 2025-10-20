using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace SameGame.CharacterSystem
{
    public class StatsMenu : MonoBehaviour
    {
        #region Serialized Feilds
        [Header("Color for all stats value")]
        [SerializeField] private Color statColor;
        [Space(5)]

        [Header("Stats text components")]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI armorText;
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private TextMeshProUGUI critChanceText;
        [SerializeField] private TextMeshProUGUI critDamageText;
        #endregion

        #region Private Fields
        private string cachedHexColor;

        private const byte normalScale = 1;
        [SerializeField] private float expandedScaleText = 1.15f;
        [SerializeField] private float scaleDuration = 0.15f;
        #endregion

        #region Public Methods

        private void Awake()
        {
            cachedHexColor = statColor.ToHexString();
        }

        public void DisplayHealth(float value)
        {
            healthText.text = $"Health: <color=#{cachedHexColor}>{value}</color>";

            StatChangeScaleAnimation(healthText.rectTransform);
        }

        public void DisplayArmor(float value)
        {
            armorText.text = $"Armor: <color=#{cachedHexColor}>{value}</color>";

            StatChangeScaleAnimation(armorText.rectTransform);
        }

        public void DisplayDamage(float value)
        {
            damageText.text = $"Damage: <color=#{cachedHexColor}>{value}</color>";

            StatChangeScaleAnimation(damageText.rectTransform);
        }

        public void DisplayCritChance(float value)
        {
            critChanceText.text = $"CritChance: <color=#{cachedHexColor}>{value}</color>";

            StatChangeScaleAnimation(critChanceText.rectTransform);
        }

        public void DisplayCritDamage(float value)
        {
            critDamageText.text = $"CritDamage: <color=#{cachedHexColor}>{value}</color>";

            StatChangeScaleAnimation(critDamageText.rectTransform);
        }

        private void StatChangeScaleAnimation(RectTransform component)
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.Append(component.DOScale(expandedScaleText, scaleDuration));
            sequence.Append(component.DOScale(normalScale, scaleDuration));
        }

        #endregion
    }
}