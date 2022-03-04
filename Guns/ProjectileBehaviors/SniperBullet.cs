using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class SniperBullet : Projectile
    {
        public void CopyFromNormalProjectile(Projectile otherProjectile)
        {
            var freezeEffectFields = typeof(Projectile).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var field in freezeEffectFields)
            {
                field.SetValue(this, field.GetValue(otherProjectile));
            }
        }


        protected override void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {

            var headBottom = (otherRigidbody.sprite.WorldTopCenter - otherRigidbody.sprite.WorldCenter) / 1.5f;
            if (myRigidbody.UnitCenter.y > (otherRigidbody.sprite.WorldCenter + headBottom).y && myRigidbody.projectile)
            {

                myRigidbody.projectile.baseData.damage *= 1.5f;
            }

            base.OnPreCollision(myRigidbody, myCollider, otherRigidbody, otherCollider);
        }
    }
}
