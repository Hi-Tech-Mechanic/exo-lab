using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace SameGame.CharacterSystem
{
    public class InfoWindow : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Color for all stats value")]
        [SerializeField] private Color statColor;
        [SerializeField] private Color negativeStatColor;
        [Space(5)]

        [Header("Stats text components")]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI armorText;
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private TextMeshProUGUI critChanceText;
        [SerializeField] private TextMeshProUGUI critDamageText;

        [SerializeField] private TextMeshProUGUI commentText;

        #endregion

        private string cachedHexColor;
        private string cachedHexNegativeColor;
        private float animationDuration = 0.3f;

        private void OnEnable()
        {
            SuitElement.OnOpenItemInfoWindow += DisplayAllStats;
        }

        private void OnDisable()
        {
            SuitElement.OnOpenItemInfoWindow -= DisplayAllStats;
        }

        private void Awake()
        {
            cachedHexColor = statColor.ToHexString();
            cachedHexNegativeColor = negativeStatColor.ToHexString();
        }

        public void CloseInfoWindow()
        {
            GameObject window = transform.GetChild(0).gameObject;

            IEnumerator Wait()
            {
                window.transform.DOScale(0, animationDuration);
                yield return new WaitForSeconds(animationDuration);
                window.SetActive(false);
            }

            StartCoroutine(Wait());
        }

        private void DisplayAllStats(float[] statValues, string comment)
        {
            OpenInfoWindow();

            string statColor = cachedHexColor;
            string value;

            //UpdateOutputValues(statValues[(byte)Stats.Health]);
            //healthText.text = $"Health: <color=#{statColor}>{value}</color>";

            //UpdateOutputValues(statValues[(byte)Stats.Armor]);
            //armorText.text = $"Armor: <color=#{statColor}>{value}</color>";

            //UpdateOutputValues(statValues[(byte)Stats.Damage]);
            //damageText.text = $"Damage: <color=#{statColor}>{value}</color>";

            //UpdateOutputValues(statValues[(byte)Stats.CritChance]);
            //critChanceText.text = $"CritChance: <color=#{statColor}>{value}</color>";

            //UpdateOutputValues(statValues[(byte)Stats.CritDamage]);
            //critDamageText.text = $"CritDamage: <color=#{statColor}>{value}</color>";

            if (comment == null)
                commentText.text = "Sample comment";
            else commentText.text = comment;

            void UpdateOutputValues(float itemValue)
            {
                if (itemValue == 0)
                {
                    statColor = cachedHexNegativeColor;
                    value = "NONE";
                }
                else
                {
                    statColor = cachedHexColor;
                    value = itemValue.ToString();
                }
            }
        }

        private void OpenInfoWindow()
        {
            GameObject window = transform.GetChild(0).gameObject;

            if(window.activeInHierarchy == false)
            {
                window.SetActive(true);

                window.transform.localScale = Vector3.zero;
                window.transform.DOScale(1, 0.3f);
            }
        }
    }
}

