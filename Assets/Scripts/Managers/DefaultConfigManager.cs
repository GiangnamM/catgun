using System.Collections.Generic;

using UnityEngine;

namespace App {
    [CreateAssetMenu(fileName = "ConfigManager", menuName = "Config Manager", order = 1)]
    public class DefaultConfigManager : ScriptableObject, IConfigManager {
        
        [SerializeField]
        private List<UpgradeGun> _upgradeGuns;
        
        public List<UpgradeGun> UpgradeGuns => _upgradeGuns;

        public IDictionary<GunSkin, (List<float>, List<int>)> AllUpgradeGunTuples {
            get {
                var result = new Dictionary<GunSkin, (List<float>, List<int>)>();
                foreach (var gun in _upgradeGuns) {
                    result[gun.gunSkin] = (gun.damage, gun.bullet);
                }
                return result;
            }
        }
    }
}