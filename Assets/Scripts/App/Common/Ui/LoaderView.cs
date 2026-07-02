using UnityEngine;

namespace RequestQueueDemo.App.Common.Ui
{
    public sealed class LoaderView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        public void Show() => _root.SetActive(true);
        public void Hide() => _root.SetActive(false);
    }
}
