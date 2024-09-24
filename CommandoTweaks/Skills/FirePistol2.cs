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
        public static float CustomProcCoefficient;
        public override string SkillToken => "FirePistol2";
        public override string Name => "M1";
        public override string DescText => "A description";

        public override void Init()
        {
            CustomProcCoefficient = ConfigOption(1f, "M1 Proc", "Decimal. Vanilla is 1");
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
                c.Emit(OpCodes.Ldc_R4, CustomProcCoefficient);
                c.Emit<BulletAttack>(OpCodes.Stfld, nameof(BulletAttack.procCoefficient));

            }
        }

        public static void Changes()
        {

        }
    }
}
