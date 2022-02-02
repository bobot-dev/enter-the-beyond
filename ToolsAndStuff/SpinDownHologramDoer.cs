using System;
using System.Collections;
using UnityEngine;

namespace BotsMod
{
	public class SpinDownHologramDoer : BraveBehaviour
	{
		private void Start()
		{
			this.ArcRenderer.enabled = false;
			this.m_arcMaterial = this.ArcRenderer.material;
			if (this.Automatic)
			{
				base.StartCoroutine(this.HandleAutomaticTrigger());
			}
		}
		private IEnumerator HandleAutomaticTrigger()
		{
			yield return new WaitForSeconds(0.25f);
			while (this.spriteAnimator.IsPlaying("hbux_base_intro"))
			{
				yield return null;
			}
			this.spriteAnimator.Play("hbux_base_on");
			this.ChangeToSprite(this.gameObject, null, -1);
			yield break;
		}
		private void Update()
		{
		}

		private IEnumerator ToggleAdditionalLight(bool lightEnabled)
		{
			float elapsed = 0f;
			float duration = 0.25f;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				float t = elapsed / duration;
				if (!lightEnabled)
				{
					t = 1f - t;
				}
				this.AttachedBraveLight.LightIntensity = Mathf.Lerp(0f, 2.09f, t);
				yield return null;
			}
			yield break;
		}

		public void HideSprite(GameObject source, bool instant = false)
		{
			if (this.m_lastSource == source || instant)
			{
				if (this.m_hologramSprite)
				{
					if (this.NotAHologram)
					{
						SpriteOutlineManager.ToggleOutlineRenderers(this.m_hologramSprite, false);
					}
					this.m_hologramSprite.renderer.enabled = false;
				}
				if (this.AttachedBraveLight)
				{
					base.StartCoroutine(this.ToggleAdditionalLight(false));
				}
				if (instant)
				{
					this.ArcRenderer.enabled = false;
				}
				else
				{
					base.StartCoroutine(this.HandleArcLerp(true));
				}
				if (this.Glower)
				{
					this.Glower.renderer.material.SetFloat("_EmissivePower", 0f);
				}
				this.m_lastSource = null;
			}
		}
		private IEnumerator HandleArcLerp(bool invert)
		{
			float ela = 0f;
			if (this.Automatic)
			{
				this.m_arcMaterial.SetColor("_OverrideColor", new Color(0f, 0.4f, 0f));
			}
			while (ela < 0.2f)
			{
				ela += BraveTime.DeltaTime;
				float t = ela / 0.2f;
				if (invert)
				{
					t = Mathf.Clamp01(1f - t);
				}
				float smoothT = Mathf.SmoothStep(0f, 1f, t);
				this.m_arcMaterial.SetFloat("_RevealAmount", smoothT);
				this.ArcRenderer.enabled = !this.NotAHologram;
				yield return null;
			}
			if (!invert)
			{
				while (this.m_lastSource != null)
				{
					this.m_arcMaterial.SetFloat("_BrightnessWarble", Mathf.PingPong(Time.realtimeSinceStartup, 1f) / 10f + 1f);
					yield return null;
				}
			}
			yield break;
		}
		public void ChangeToSprite(GameObject source, tk2dSpriteCollectionData collection, int spriteId)
		{
			this.m_lastSource = source;
			if (this.Automatic)
			{
				this.m_hologramSprite = this.TargetAutomaticSprite;
			}
			else if (this.m_hologramSprite == null)
			{
				GameObject go = new GameObject("hologram");
				this.m_hologramSprite = tk2dSprite.AddComponent(go, collection, spriteId);
				if (this.parentHologram)
				{
					this.m_hologramSprite.transform.parent = base.transform;
				}
				if (this.NotAHologram)
				{
					SpriteOutlineManager.AddOutlineToSprite(this.m_hologramSprite, Color.white);
				}
				if (this.Glower && !this.NotAHologram)
				{
					this.Glower.usesOverrideMaterial = true;
					this.Glower.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
				}
			}
			else
			{
				this.m_hologramSprite.SetSprite(collection, spriteId);
				this.m_hologramSprite.ForceUpdateMaterial();
			}
			if (this.NotAHologram)
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.m_hologramSprite, true);
			}
			this.m_hologramSprite.renderer.enabled = true;
			this.m_hologramSprite.usesOverrideMaterial = true;
			if (!this.NotAHologram)
			{
				this.m_hologramSprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
			}
			if (this.Automatic)
			{
				this.m_hologramSprite.renderer.material.SetFloat("_IsGreen", 1f);
				this.m_hologramSprite.spriteAnimator.PlayForDuration(this.m_hologramSprite.spriteAnimator.DefaultClip.name, this.m_hologramSprite.spriteAnimator.DefaultClip.BaseClipLength, "hbux_symbol_idle", false);
			}
			else
			{
				this.m_hologramSprite.PlaceAtPositionByAnchor(this.Holopoint.position, tk2dBaseSprite.Anchor.LowerCenter);
				this.m_hologramSprite.transform.localPosition = this.m_hologramSprite.transform.localPosition.Quantize(0.0625f);
			}
			if (this.Glower && !this.NotAHologram)
			{
				this.Glower.renderer.material.SetFloat("_EmissivePower", 20f);
				this.Glower.renderer.material.SetFloat("_EmissiveColorPower", 3f);
			}
			if (this.AttachedBraveLight)
			{
				base.StartCoroutine(this.ToggleAdditionalLight(true));
			}
			base.StartCoroutine(this.HandleArcLerp(false));
		}
		public Transform Holopoint;
		public tk2dSprite Glower;
		public MeshRenderer ArcRenderer;
		private Material m_arcMaterial;
		public bool Automatic;
		public tk2dSprite TargetAutomaticSprite;
		public AdditionalBraveLight AttachedBraveLight;
		public bool parentHologram;
		public bool NotAHologram;
		private GameObject m_lastSource;
		private tk2dSprite m_hologramSprite;
	}
}
