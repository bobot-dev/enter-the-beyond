using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	public class EmpActorEffect : GameActorEffect
	{
		static Shader BackupShader;
		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
		{
			if (actor is AIActor)
			{


				base.OnEffectApplied(actor, effectData, partialAmount);

				AIActor aiactor = actor as AIActor;


				//actor.ClearOverrideShader();

				BackupShader = aiactor.sprite.renderer.material.shader;


				var texture = ItemAPI.ResourceExtractor.GetTextureFromResource("BotsMod\\sprites\\paradox_test.png");

				if (aiactor != null && aiactor.sprite != null && aiactor.sprite.renderer != null && aiactor.sprite.renderer.material != null)
				{

					/*aiactor.sprite.usesOverrideMaterial = true;

					aiactor.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/PlayerShaderEevee");
					aiactor.sprite.renderer.material.SetTexture("_EeveeTex", texture);

					aiactor.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
					aiactor.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
					*/
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

				if (this.DamageMultiplier != 1f)
				{

					actor.healthHaver.AllDamageMultiplier *= DamageMultiplier;
					
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
				specRigidbody.OnPreMovement = (Action<SpeculativeRigidbody>)Delegate.Remove(specRigidbody.OnPreMovement, new Action<SpeculativeRigidbody>(this.ModifyVelocity));
			}
			if (this.CooldownMultiplier != 1f && actor.behaviorSpeculator)
			{
				actor.behaviorSpeculator.CooldownScale *= this.CooldownMultiplier;
			}

			if (this.DamageMultiplier != 1f && actor.healthHaver)
			{
				actor.healthHaver.AllDamageMultiplier /= this.DamageMultiplier;
			}
		}

		public void ModifyVelocity(SpeculativeRigidbody myRigidbody)
		{
			myRigidbody.Velocity *= this.SpeedMultiplier;
		}

		public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			if (this.AffectsEnemies && actor is AIActor)
			{
				actor.healthHaver.ApplyDamage(this.DamagePerSecondToEnemies * BraveTime.DeltaTime, Vector2.zero, this.effectIdentifier, CoreDamageTypes.None, DamageCategory.DamageOverTime, false, null, false);


				Vector2 unitDimensions = actor.specRigidbody.HitboxPixelCollider.UnitDimensions;
				Vector2 a = unitDimensions / 2f;
				int num2 = Mathf.RoundToInt((float)this.flameNumPerSquareUnit * 0.5f * Mathf.Min(30f, Mathf.Min(new float[]
				{
					unitDimensions.x * unitDimensions.y
				})));
				this.m_particleTimer += BraveTime.DeltaTime * (float)num2;
				if (this.m_particleTimer > 1f)
				{
					int num3 = Mathf.FloorToInt(this.m_particleTimer);
					Vector2 vector = actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
					Vector2 vector2 = actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
					PixelCollider pixelCollider = actor.specRigidbody.GetPixelCollider(ColliderType.Ground);
					if (pixelCollider != null && pixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.Manual)
					{
						vector = Vector2.Min(vector, pixelCollider.UnitBottomLeft);
						vector2 = Vector2.Max(vector2, pixelCollider.UnitTopRight);
					}
					vector += Vector2.Min(a * 0.15f, new Vector2(0.25f, 0.25f));
					vector2 -= Vector2.Min(a * 0.15f, new Vector2(0.25f, 0.25f));
					vector2.y -= Mathf.Min(a.y * 0.1f, 0.1f);
					GlobalSparksDoer.DoRandomParticleBurst(num3, vector, vector2, Vector3.zero, 0f, 0f, null, null, new Color?(new Color(0.25f, 0.25f, 1f, 1f)), GlobalSparksDoer.SparksType.FLOATY_CHAFF);
					this.m_particleTimer -= Mathf.Floor(this.m_particleTimer);
				}


			}
		}
		public int flameNumPerSquareUnit;
		public float SpeedMultiplier;
		public float CooldownMultiplier;
		public float initalAmount;
		public float DamageMultiplier;

		private float m_particleTimer;
	}
}
