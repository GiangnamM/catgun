using System.Collections.Generic;
using UnityEngine;

namespace App
{
    [CreateAssetMenu(fileName = "ConfigManager", menuName = "Config Manager", order = 1)]
    public class DefaultConfigManager : ScriptableObject, IConfigManager
    {
        [SerializeField] private List<UpgradeGun> _upgradeGuns;

        [SerializeField] private List<BaseGun> _baseGuns;


        public IDictionary<GunSkin, (float, float)> GunBaseInfo
        {
            get
            {
                var result = new Dictionary<GunSkin, (float, float)>();
                foreach (var gun in _baseGuns)
                {
                    result[gun.gunSkin] = (gun.damage, gun.fireRate);
                }

                return result;
            }
        }

        public IDictionary<GunSkin, (List<float>, List<float>)> AllUpgradeGunTuples
        {
            get
            {
                var result = new Dictionary<GunSkin, (List<float>, List<float>)>();
                foreach (var gun in _upgradeGuns)
                {
                    result[gun.gunSkin] = (gun.damage, gun.fireRate);
                }

                return result;
            }
        }
    }
}