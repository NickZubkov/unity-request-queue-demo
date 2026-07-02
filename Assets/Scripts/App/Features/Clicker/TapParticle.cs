using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace RequestQueueDemo.App.Features.Clicker
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class TapParticle : MonoBehaviour
    {
        [SerializeField] private float _yOffset;

        private ParticleSystem _ps;
        private Pool _pool;
        private int _generation;

        private void Awake() => _ps = GetComponent<ParticleSystem>();

        public void Play(Vector3 screenPos)
        {
            transform.position = new Vector3(screenPos.x, screenPos.y, _yOffset);
            _ps.Play();
            WaitAndDespawn(++_generation).Forget();
        }

        private async UniTaskVoid WaitAndDespawn(int generation)
        {
            await UniTask.WaitWhile(() => _ps.IsAlive(true),
                                    cancellationToken: this.GetCancellationTokenOnDestroy());
            if (generation == _generation)
                _pool.Despawn(this);
        }

        private void Stop()
        {
            _generation++;
            _ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public sealed class Pool : MonoMemoryPool<TapParticle>
        {
            private readonly HashSet<TapParticle> _active = new();

            protected override void OnCreated(TapParticle item)
            {
                base.OnCreated(item);
                item._pool = this;
            }

            protected override void OnSpawned(TapParticle item)
            {
                base.OnSpawned(item);
                _active.Add(item);
            }

            protected override void OnDespawned(TapParticle item)
            {
                _active.Remove(item);
                item.Stop();
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
