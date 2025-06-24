using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.src.HUDUI
{
    public class TopHUDController : MonoBehaviour
    {
        [SerializeField] PlayerData playerData;
        [SerializeField] TextMeshProUGUI clockText;
        [SerializeField] TextMeshProUGUI creditsText;
        [SerializeField] TextMeshProUGUI currencyText;
        [SerializeField] TextMeshProUGUI locationText;
        [SerializeField] TextMeshProUGUI rankText;
        [SerializeField] Slider rankExperienceSlider;

        void OnEnable()
        {
            StartCoroutine(StartTime());
            UpdateHUD();
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }
        
        IEnumerator StartTime()
        {
            while (true)
            {
                string timeText = System.DateTime.Now.ToString("ddd h:mm tt");
                clockText.text = $"{timeText} ({playerData.termName})" ;
                yield return new WaitForSeconds(1f); // update once per second
            }
        }

        void UpdateHUD()
        {
            UpdateCredits();
            UpdateCurrency();
            UpdateLocation();
            UpdateRank();
            UpdateRankExperience();
        }

        void UpdateCredits()
        {
            creditsText.text = playerData.termCredits.ToString(CultureInfo.InvariantCulture);
        }

        void UpdateCurrency()
        {
            currencyText.text = playerData.currency.ToString("N0", CultureInfo.InvariantCulture);;
        }

        void UpdateLocation()
        {
            locationText.text = playerData.currentLocation;
        }

        void UpdateRank()
        {
            rankText.text = $"Rank {playerData.rank.ToString()}";
        }

        void UpdateRankExperience()
        {
            rankExperienceSlider.maxValue = playerData.requiredRankExperience;
            rankExperienceSlider.value = playerData.currentRankExperience;
        }
    }
}
