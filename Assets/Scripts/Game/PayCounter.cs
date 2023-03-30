using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class PayCounter : MonoBehaviour
    {
        public event Action OnCountingFinished;

        public TextMeshPro text;
        public uint price = 10;
        public float timePerGoldUnit = 0.2f;

        public float retrieveGold = 0.05f;

        private int filledSlots = 0;
        Coroutine SlotsCounter;

        public void StartCounting()
        {
            StopCountingCoroutine();
            SlotsCounter = StartCoroutine(AddGold());
        }

        IEnumerator AddGold()
        {
            filledSlots = 0;
            text.text = $"{filledSlots}/{price}";
            for (int i = 0; i < price; i++)
            {
                yield return new WaitForSeconds(timePerGoldUnit);
                filledSlots++;
                text.text = $"{filledSlots}/{price} gold";
            }

            OnCountingFinished?.Invoke();
        }

        public void StopCounting()
        {
            StopCountingCoroutine();
            SlotsCounter = StartCoroutine(RemoveGold());
        }

        IEnumerator RemoveGold()
        {
            int slots = filledSlots;
            for (int i = slots; i > 0; i--)
            {
                yield return new WaitForSeconds(retrieveGold);
                filledSlots--;
                text.text = $"{filledSlots}/{price} gold";
            }
            gameObject.SetActive(false);
        }

        private void StopCountingCoroutine()
        {
            if (SlotsCounter != null)
                StopCoroutine(SlotsCounter);
        }

        private void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}

