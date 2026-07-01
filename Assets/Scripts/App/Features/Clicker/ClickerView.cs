using System;
using DG.Tweening;
using RequestQueueDemo.App.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RequestQueueDemo.App.Features.Clicker
{
    public sealed class ClickerView : MonoBehaviour, IClickerView
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _tapButton;
        [SerializeField] private RectTransform _buttonRect;
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private TMP_Text _energyText;
        [SerializeField] private AudioSource _audioSource;

        private FloatingText.Pool _floatingPool;
        private TapParticle.Pool _particlePool;
        private AudioClip _clickSfx;
        private Vector3 _defaultScale;
        private Vector2 _defaultAnchoredPos;

        public event Action TapClicked;

        [Inject]
        public void Construct(FloatingText.Pool floatingPool, TapParticle.Pool particlePool, ClickerConfig config)
        {
            _floatingPool = floatingPool;
            _particlePool = particlePool;
            _clickSfx = config.ClickSfx;
        }

        private void Awake()
        {
            _tapButton.onClick.AddListener(() => TapClicked?.Invoke());
            _defaultScale = _buttonRect.localScale;
            _defaultAnchoredPos = _buttonRect.anchoredPosition;
        }

        // Сбрасывает кнопку к дефолту и убивает активные твины — иначе частые тапы
        // складывают punch/shake и кнопка «распухает»/уползает.
        private void ResetButtonTransform()
        {
            _buttonRect.DOKill();
            _buttonRect.localScale = _defaultScale;
            _buttonRect.anchoredPosition = _defaultAnchoredPos;
        }

        public void Show() => _panel.SetActive(true);
        public void Hide() => _panel.SetActive(false);
        public void SetCurrency(long amount) => _currencyText.text = amount.ToString();
        public void SetEnergy(int energy, int max) => _energyText.text = $"{energy}/{max}";

        public void PlayTapVfx()
        {
            var pos = _buttonRect.position;
            _particlePool.Spawn().Play(pos);
            _floatingPool.Spawn().Play(pos, "+1");
            if (_clickSfx != null) _audioSource.PlayOneShot(_clickSfx);
        }

        public void ClearVfx()
        {
            // Возвращаем все активные «+1» и партиклы в пул — при уходе с вкладки кликера.
            _floatingPool.DespawnAllActive();
            _particlePool.DespawnAllActive();
        }

        public void PlayButtonPunch()
        {
            ResetButtonTransform();
            _buttonRect.DOPunchScale(Vector3.one * 0.15f, 0.2f);
        }

        public void PlayNoEnergyFeedback()
        {
            ResetButtonTransform();
            _buttonRect.DOShakeAnchorPos(0.3f, 10f);
        }
    }
}
