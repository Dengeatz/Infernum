using System;
using Infernum.FPS.Core;
using TMPro;
using UnityEngine;

namespace Player.View
{
    public class PlayerHealthView : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private TMP_Text _healthText;

        private void Update()
        {
            _healthText.text = $"HP: {_health.Current:0.0}";
        }
    }
}
