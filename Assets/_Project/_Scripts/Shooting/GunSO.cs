using Praxi.Combat;
using UnityEngine;

namespace Praxi.Shooting
{

    [CreateAssetMenu(fileName = "GunSO", menuName = "Praxi/GunSO", order = 0)]
    public class GunSO : ScriptableObject
    {
        public Projectile BulletPrefab;
        public ParticleSystem MuzzleEffect;

        public int Damage;
        public int AmmoNumb;
        public float FireRate;
        public float ReloadTime;
        public bool IsAutomatic;
    }
}
