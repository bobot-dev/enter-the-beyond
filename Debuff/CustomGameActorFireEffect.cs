using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E24 RID: 3620
[Serializable]
public class CustomGameActorFireEffect : GameActorHealthEffect
{
	// Token: 0x06004C9A RID: 19610 RVA: 0x000323F4 File Offset: 0x000305F4
	public CustomGameActorFireEffect()
	{
		this.flameNumPerSquareUnit = 10;
		this.flameBuffer = new Vector2(0.0625f, 0.0625f);
		this.flameFpsVariation = 0.5f;
		this.flameMoveChance = 0.2f;
	}

	// Token: 0x17000AC8 RID: 2760
	// (get) Token: 0x06004C9B RID: 19611 RVA: 0x0000492E File Offset: 0x00002B2E
	public override bool ResistanceAffectsDuration
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004C9C RID: 19612 RVA: 0x001C2C64 File Offset: 0x001C0E64
	public static RuntimeGameActorEffectData ApplyFlamesToTarget(GameActor actor, CustomGameActorFireEffect sourceEffect)
	{
		return new RuntimeGameActorEffectData
		{
			actor = actor
		};
	}

	// Token: 0x06004C9D RID: 19613 RVA: 0x001C2C80 File Offset: 0x001C0E80
	public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
	{
		base.OnEffectApplied(actor, effectData, partialAmount);
		effectData.OnActorPreDeath = delegate (Vector2 dir)
		{
			CustomGameActorFireEffect.DestroyFlames(effectData);
		};
		actor.healthHaver.OnPreDeath += effectData.OnActorPreDeath;
		//if (this.FlameVfx != null && this.FlameVfx.Count > 0)
		//{
			if (effectData.vfxObjects == null)
			{
				effectData.vfxObjects = new List<Tuple<GameObject, float>>();
			}
			effectData.OnFlameAnimationCompleted = delegate (tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip)
			{
				if (effectData.destroyVfx || !actor)
				{
					spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, effectData.OnFlameAnimationCompleted);
					UnityEngine.Object.Destroy(spriteAnimator.gameObject);
					return;
				}
				if (UnityEngine.Random.value < this.flameMoveChance)
				{
					Vector2 a = actor.specRigidbody.HitboxPixelCollider.UnitDimensions / 2f;
					Vector2 b = BraveUtility.RandomVector2(-a + this.flameBuffer, a - this.flameBuffer);
					Vector2 v = actor.specRigidbody.HitboxPixelCollider.UnitCenter + b;
					spriteAnimator.transform.position = v;
				}
				spriteAnimator.Play(clip, 0f, clip.fps * UnityEngine.Random.Range(1f - this.flameFpsVariation, 1f + this.flameFpsVariation), false);
			};
		//}
	}

	// Token: 0x06004C9E RID: 19614 RVA: 0x001C2D40 File Offset: 0x001C0F40
	public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		base.EffectTick(actor, effectData);
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH && effectData.actor && effectData.actor.specRigidbody.HitboxPixelCollider != null)
		{
			Vector2 unitBottomLeft = effectData.actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
			Vector2 unitTopRight = effectData.actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
			this.m_emberCounter += 30f * BraveTime.DeltaTime;
			if (this.m_emberCounter > 1f)
			{
				int num = Mathf.FloorToInt(this.m_emberCounter);
				this.m_emberCounter -= (float)num;
				//GlobalSparksDoer.DoRandomParticleBurst(num, unitBottomLeft, unitTopRight, new Vector3(1f, 1f, 0f), 120f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
			}
		}
		if (actor && actor.specRigidbody)
		{
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
				CustomSparkDoer.DoRandomParticleBurst(num3, vector, vector2, Vector3.zero, 0f, 0f, null, null, null, CustomSparkDoer.SparksType.PINK_FIRE);
				this.m_particleTimer -= Mathf.Floor(this.m_particleTimer);
			}
		}
		if (actor.IsGone)
		{
			effectData.elapsed = 10000f;
		}
		if ((actor.IsFalling || actor.IsGone) && effectData.vfxObjects != null && effectData.vfxObjects.Count > 0)
		{
			CustomGameActorFireEffect.DestroyFlames(effectData);
		}
	}

	// Token: 0x06004C9F RID: 19615 RVA: 0x0003242F File Offset: 0x0003062F
	public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		base.OnEffectRemoved(actor, effectData);
		actor.healthHaver.OnPreDeath -= effectData.OnActorPreDeath;
		CustomGameActorFireEffect.DestroyFlames(effectData);
	}

	// Token: 0x06004CA0 RID: 19616 RVA: 0x001C3084 File Offset: 0x001C1284
	public static void DestroyFlames(RuntimeGameActorEffectData effectData)
	{
		if (effectData.vfxObjects == null)
		{
			return;
		}
		if (!effectData.actor.IsFrozen)
		{
			for (int i = 0; i < effectData.vfxObjects.Count; i++)
			{
				GameObject first = effectData.vfxObjects[i].First;
				if (first)
				{
					first.transform.parent = SpawnManager.Instance.VFX;
				}
			}
		}
		effectData.vfxObjects.Clear();
		effectData.destroyVfx = true;
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH && effectData.actor && effectData.actor.healthHaver && effectData.actor.healthHaver.GetCurrentHealth() <= 0f && effectData.actor.specRigidbody.HitboxPixelCollider != null)
		{
			Vector2 unitBottomLeft = effectData.actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
			Vector2 unitTopRight = effectData.actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
			float num = (unitTopRight.x - unitBottomLeft.x) * (unitTopRight.y - unitBottomLeft.y);
			//GlobalSparksDoer.DoRandomParticleBurst(Mathf.Max(1, (int)(75f * num)), unitBottomLeft, unitTopRight, new Vector3(1f, 1f, 0f), 120f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
		}
	}

	// Token: 0x040042A0 RID: 17056
	public const float BossMinResistance = 0.25f;

	// Token: 0x040042A1 RID: 17057
	public const float BossMaxResistance = 0.75f;

	// Token: 0x040042A2 RID: 17058
	public const float BossResistanceDelta = 0.025f;

	// Token: 0x040042A3 RID: 17059
	public List<GameObject> FlameVfx;

	// Token: 0x040042A4 RID: 17060
	public int flameNumPerSquareUnit;

	// Token: 0x040042A5 RID: 17061
	public Vector2 flameBuffer;

	// Token: 0x040042A6 RID: 17062
	public float flameFpsVariation;

	// Token: 0x040042A7 RID: 17063
	public float flameMoveChance;

	// Token: 0x040042A9 RID: 17065
	private float m_particleTimer;

	// Token: 0x040042AA RID: 17066
	private float m_emberCounter;
}
