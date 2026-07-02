using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace RequestQueueDemo.App.Features.Clicker
{
    public sealed class FloatingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private float _floatingDisatnce;
        [SerializeField] private float _floatingDuration;

        private Pool _pool;
        private Tween _tween;

        public void Play(Vector3 screenPos, string label)
        {
            transform.position = screenPos;
            _text.text = label;
            _group.alpha = 1f;
            _tween = DOTween.Sequence()
                .Append(transform.DOMoveY(screenPos.y + _floatingDisatnce, _floatingDuration))
                .Join(_group.DOFade(0f, 0.8f))
                .OnComplete(() => _pool.Despawn(this))
                .SetLink(gameObject);
        }

        private void KillTween()
        {
            _tween?.Kill();
            _tween = null;
        }

        public sealed class Pool : MonoMemoryPool<FloatingText>
        {
            private readonly HashSet<FloatingText> _active = new();

            protected override void OnCreated(FloatingText item)
            {
                base.OnCreated(item);
                item._pool = this;
            }

            protected override void OnSpawned(FloatingText item)
            {
                base.OnSpawned(item);
                _active.Add(item);
            }

            protected override void OnDespawned(FloatingText item)
            {
                _active.Remove(item);
                item.KillTween();
                base.OnDespawned(item);
            }

            public void DespawnAllActive()
            {
                foreach (var item in _active.ToArray())
                    Despawn(item);
            }
        }
    }
}
