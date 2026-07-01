using UnityEngine;

namespace RequestQueueDemo.App.Config
{
    [CreateAssetMenu(menuName = "RequestQueueDemo/ClickerConfig", fileName = "ClickerConfig")]
    public sealed class ClickerConfig : ScriptableObject
    {
        [SerializeField] private long _tapReward = 1;
        [SerializeField] private int _tapEnergyCost = 1;
        [SerializeField] private float _autoCollectIntervalSeconds = 3f;
        [SerializeField] private long _autoCollectReward = 1;
        [SerializeField] private int _autoCollectEnergyCost = 1;
        [SerializeField] private int _energyMax = 1000;
        [SerializeField] private int _energyStart = 1000;
        [SerializeField] private float _energyRegenIntervalSeconds = 10f;
        [SerializeField] private int _energyRegenAmount = 10;
        [SerializeField] private AudioClip _clickSfx;

        public long TapReward => _tapReward;
        public int TapEnergyCost => _tapEnergyCost;
        public float AutoCollectIntervalSeconds => _autoCollectIntervalSeconds;
        public long AutoCollectReward => _autoCollectReward;
        public int AutoCollectEnergyCost => _autoCollectEnergyCost;
        public int EnergyMax => _energyMax;
        public int EnergyStart => _energyStart;
        public float EnergyRegenIntervalSeconds => _energyRegenIntervalSeconds;
        public int EnergyRegenAmount => _energyRegenAmount;
        public AudioClip ClickSfx => _clickSfx;
    }
}
