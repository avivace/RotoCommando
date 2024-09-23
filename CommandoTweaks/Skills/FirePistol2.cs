using RoR2.Projectile;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.Skills;
using System.Collections;
using R2API.Utils;
using System.ComponentModel;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace HIFUCommandoTweaks.Skills
{
    internal class FirePistol2 : TweakBase
    {
        public override string SkillToken => "special_alt1";
        public override string Name => ": Special :: Frag Grenade";

        public override string DescText => "Fire a <style=cIsDamage>piercing</style> bullet for <style=cIsDamage> 2 damage</style>. Deals <style=cIsDamage>40%</style> more total damage for each enemy pierced.";

        public override void Init()
        {
            base.Init();
        }

        public override void Hooks()
        {
            Changes();
            // On.EntityStates.Commando.CommandoWeapon.FirePistol2.OnEnter += FirePistol2_OnEnter;
            IL.EntityStates.Commando.CommandoWeapon.FirePistol2.FireBullet += FirePistol2_FireBullet;
        }

        private void FirePistol2_OnEnter(On.EntityStates.Commando.CommandoWeapon.FirePistol2.orig_OnEnter orig, EntityStates.Commando.CommandoWeapon.FirePistol2 self)
        {
            EntityStates.Commando.CommandoWeapon.FirePistol2.damageCoefficient = 100f;
            orig(self);
        }

        private void FirePistol2_FireBullet(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchNewobj<BulletAttack>()))
            {
                c.Emit(OpCodes.Dup);
                c.Emit(OpCodes.Ldc_R4, 1.25f);

                c.Emit<BulletAttack>(OpCodes.Stfld, nameof(BulletAttack.procCoefficient));

            }
        }

        public static void Changes()
        {

        }
    }
}
