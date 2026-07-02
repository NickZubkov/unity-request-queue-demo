using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RequestQueueDemo.App.Features.Breeds
{
    public sealed class BreedListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Button _button;

        private Pool _pool;
        private string _id;
        private Action<string> _onClick;

        private void Awake() => _button.onClick.AddListener(() => _onClick?.Invoke(_id));

        public void Bind(int index, string id, string name, Action<string> onClick)
        {
            _id = id;
            _onClick = onClick;
            _label.text = $"{index} - {name}";
        }

        public void Despawn() => _pool.Despawn(this);

        public sealed class Pool : MonoMemoryPool<BreedListItem>
        {
            protected override void OnCreated(BreedListItem item)
            {
                base.OnCreated(item); // деактивирует префарм-инстансы, иначе висят активными на старте
                item._pool = this;
            }
        }
    }
}
