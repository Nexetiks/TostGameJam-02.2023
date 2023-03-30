using General;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private EndGameScreen _endGameScreen;
        [SerializeField]
        private TextMeshProUGUI _coinsText;

        private void Awake()
        {
            GameManager.Instance.OnGoldChange += UpdateCoins;
        }

        public void ShowEndgameScreen()
        {
            _endGameScreen.gameObject.SetActive(true);
        }

        private void UpdateCoins(int gold)
        {
            _coinsText.text = $"{gold} gold";
        }
    }

}
