using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField]
        private Button _restartGameButton;

        private void Awake()
        {
            _restartGameButton.onClick.AddListener(RestartGameButton_OnClick);
        }

        private void RestartGameButton_OnClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
