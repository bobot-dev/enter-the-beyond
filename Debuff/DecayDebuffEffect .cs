using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class GameActorDecayEffect : GameActorEffect
	{
		static Shader BackupShader;
		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
		{
			if (actor is AIActor)
			{


				base.OnEffectApplied(actor, effectData, partialAmount);

				AIActor aiactor = actor as AIActor;


				//actor.ClearOverrideShader();

				//BackupShader = aiactor.sprite.renderer.material.shader;


				var texture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\paradox_test.png");

				if (aiactor != null && aiactor.sprite != null && aiactor.sprite.renderer != null && aiactor.sprite.renderer.material != null)
				{

					//aiactor.sprite.usesOverrideMaterial = true;

					aiactor.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/PlayerShaderEevee");
					aiactor.sprite.renderer.material.SetTexture("_EeveeTex", texture);

					aiactor.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
					aiactor.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");



				}



				//





				//actor.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");

				if (this.SpeedMultiplier != 1f)
				{
					SpeculativeRigidbody specRigidbody = actor.specRigidbody;
					specRigidbody.OnPreMovement = (Action<SpeculativeRigidbody>)Delegate.Combine(specRigidbody.OnPreMovement, new Action<SpeculativeRigidbody>(this.ModifyVelocity));
				}
				if (this.CooldownMultiplier != 1f && actor.behaviorSpeculator)
				{
					actor.behaviorSpeculator.CooldownScale /= this.CooldownMultiplier;
				}
			}
		}

		public float DamagePerSecondToEnemies;

		public bool ignitesGoops;

		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{

			if (BackupShader != null)				
			{
				//actor.sprite.renderer.material.shader = BackupShader;
			}
			//ETGModConsole.Log("L");
			
			if (this.SpeedMultiplier != 1f)
			{
				SpeculativeRigidbody specRigidbody = actor.specRigidbody;
				specRigidbody.OnPreMovement += this.ModifyVelocity;
			}
			if (this.CooldownMultiplier != 1f && actor.behaviorSpeculator)
			{
				actor.behaviorSpeculator.CooldownScale *= this.CooldownMultiplier;
			} 
		}

		public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			if (this.AffectsEnemies && actor is AIActor)
			{
				actor.healthHaver.ApplyDamage(this.DamagePerSecondToEnemies * BraveTime.DeltaTime, Vector2.zero, this.effectIdentifier, CoreDamageTypes.None, DamageCategory.DamageOverTime, false, null, false);
			}
			if (this.ignitesGoops)
			{
				DeadlyDeadlyGoopManager.IgniteGoopsCircle(actor.CenterPosition, 0.5f);
			}
		}

		public void ModifyVelocity(SpeculativeRigidbody myRigidbody)
		{
			myRigidbody.Velocity *= this.SpeedMultiplier;
		}

		public float initalAmount;

		public float SpeedMultiplier;

		public float CooldownMultiplier;

	}
}
