using BotsMod;
using ItemAPI;
using System;
using System.Collections;
using UnityEngine;

public static class CustomSparkDoer
{
	public static void DoSingleParticle(Vector3 position, Vector3 direction, float? startSize = null, float? startLifetime = null, Color? startColor = null, CustomSparkDoer.SparksType systemType = CustomSparkDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		ParticleSystem particleSystem = CustomSparkDoer.m_particles;
		switch (systemType)
		{
			case CustomSparkDoer.SparksType.SPARKS_ADDITIVE_DEFAULT:
				particleSystem = CustomSparkDoer.m_particles;
				break;
			case CustomSparkDoer.SparksType.PINK_FIRE:
				particleSystem = CustomSparkDoer.m_pinkFireParticles;
				break;

			case CustomSparkDoer.SparksType.ELECTRIC_FIRE:
				particleSystem = CustomSparkDoer.m_blueFireParticles;
				break;
		}
		if (particleSystem == null)
		{
			particleSystem = CustomSparkDoer.InitializeParticles(systemType);
		}
		if (!particleSystem.gameObject.activeSelf)
		{
			particleSystem.gameObject.SetActive(true);
		}
		ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
		{
			position = position,
			velocity = direction,
			startSize = ((startSize == null) ? particleSystem.startSize : startSize.Value),
			startLifetime = ((startLifetime == null) ? particleSystem.startLifetime : startLifetime.Value),
			startColor = ((startColor == null) ? particleSystem.startColor : startColor.Value),
			randomSeed = (uint)UnityEngine.Random.Range(1, 1000)
		};
		particleSystem.Emit(emitParams, 1);
	}

	public static void DoRandomParticleBurst(int num, Vector3 minPosition, Vector3 maxPosition, Vector3 direction, float angleVariance, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, CustomSparkDoer.SparksType systemType = CustomSparkDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		for (int i = 0; i < num; i++)
		{
			Vector3 position = new Vector3(UnityEngine.Random.Range(minPosition.x, maxPosition.x), UnityEngine.Random.Range(minPosition.y, maxPosition.y), UnityEngine.Random.Range(minPosition.z, maxPosition.z));
			Vector3 direction2 = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-angleVariance, angleVariance)) * (direction.normalized * UnityEngine.Random.Range(direction.magnitude - magnitudeVariance, direction.magnitude + magnitudeVariance));
			CustomSparkDoer.DoSingleParticle(position, direction2, startSize, startLifetime, startColor, systemType);
		}
	}

	public static void DoLinearParticleBurst(int num, Vector3 minPosition, Vector3 maxPosition, float angleVariance, float baseMagnitude, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, CustomSparkDoer.SparksType systemType = CustomSparkDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		for (int i = 0; i < num; i++)
		{
			Vector3 position = Vector3.Lerp(minPosition, maxPosition, ((float)i + 1f) / (float)num);
			Vector3 vector = UnityEngine.Random.insideUnitCircle.normalized.ToVector3ZUp(0f);
			Vector3 direction = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-angleVariance, angleVariance)) * (vector.normalized * UnityEngine.Random.Range(baseMagnitude - magnitudeVariance, vector.magnitude + magnitudeVariance));
			CustomSparkDoer.DoSingleParticle(position, direction, startSize, startLifetime, startColor, systemType);
		}
	}

	public static void DoRadialParticleBurst(int num, Vector3 minPosition, Vector3 maxPosition, float angleVariance, float baseMagnitude, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, CustomSparkDoer.SparksType systemType = CustomSparkDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(minPosition.x, maxPosition.x), UnityEngine.Random.Range(minPosition.y, maxPosition.y), UnityEngine.Random.Range(minPosition.z, maxPosition.z));
			Vector3 vector2 = vector - (maxPosition + minPosition) / 2f;
			Vector3 direction = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-angleVariance, angleVariance)) * (vector2.normalized * UnityEngine.Random.Range(baseMagnitude - magnitudeVariance, vector2.magnitude + magnitudeVariance));
			CustomSparkDoer.DoSingleParticle(vector, direction, startSize, startLifetime, startColor, systemType);
		}
	}

	public static void EmitFromRegion(CustomSparkDoer.EmitRegionStyle emitStyle, float numPerSecond, float duration, Vector3 minPosition, Vector3 maxPosition, Vector3 direction, float angleVariance, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, CustomSparkDoer.SparksType systemType = CustomSparkDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		GameUIRoot.Instance.StartCoroutine(CustomSparkDoer.HandleEmitFromRegion(emitStyle, numPerSecond, duration, minPosition, maxPosition, direction, angleVariance, magnitudeVariance, startSize, startLifetime, startColor, systemType));
	}

	private static IEnumerator HandleEmitFromRegion(CustomSparkDoer.EmitRegionStyle emitStyle, float numPerSecond, float duration, Vector3 minPosition, Vector3 maxPosition, Vector3 direction, float angleVariance, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, CustomSparkDoer.SparksType systemType = CustomSparkDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		float elapsed = 0f;
		float numReqToSpawn = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			numReqToSpawn += numPerSecond * BraveTime.DeltaTime;
			if (numReqToSpawn > 1f)
			{
				int num = Mathf.FloorToInt(numReqToSpawn);
				if (emitStyle != CustomSparkDoer.EmitRegionStyle.RANDOM)
				{
					if (emitStyle == CustomSparkDoer.EmitRegionStyle.RADIAL)
					{
						CustomSparkDoer.DoRadialParticleBurst(num, minPosition, maxPosition, angleVariance, direction.magnitude, magnitudeVariance, startSize, startLifetime, startColor, systemType);
					}
				}
				else
				{
					CustomSparkDoer.DoRandomParticleBurst(num, minPosition, maxPosition, direction, angleVariance, magnitudeVariance, startSize, startLifetime, startColor, systemType);
				}
			}
			numReqToSpawn %= 1f;
			yield return null;
		}
		yield break;
	}

	private static ParticleSystem InitializeParticles(CustomSparkDoer.SparksType targetType)
	{
		switch (targetType)
		{
			case CustomSparkDoer.SparksType.SPARKS_ADDITIVE_DEFAULT:
				CustomSparkDoer.m_particles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/SparkSystem"), Vector3.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
				return CustomSparkDoer.m_particles;

			case CustomSparkDoer.SparksType.PINK_FIRE:
				CustomSparkDoer.m_pinkFireParticles = FakePrefab.Clone(CustomFire.pinkFire).GetComponent<ParticleSystem>();
				return CustomSparkDoer.m_pinkFireParticles;

			case CustomSparkDoer.SparksType.ELECTRIC_FIRE:
				CustomSparkDoer.m_blueFireParticles = FakePrefab.Clone(CustomFire.blueFire).GetComponent<ParticleSystem>();
				return CustomSparkDoer.m_blueFireParticles;

			default:
				return CustomSparkDoer.m_particles;
		}
	}

	public static void CleanupOnSceneTransition()
	{
		CustomSparkDoer.m_particles = null;
		CustomSparkDoer.m_pinkFireParticles = null;
		CustomSparkDoer.m_blueFireParticles = null;
	}

	private static ParticleSystem m_particles;

	private static ParticleSystem m_pinkFireParticles;
	private static ParticleSystem m_blueFireParticles;
	public enum EmitRegionStyle
	{
		RANDOM,
		RADIAL
	}

	public enum SparksType
	{
		SPARKS_ADDITIVE_DEFAULT,

		PINK_FIRE,
		ELECTRIC_FIRE
	}
}
