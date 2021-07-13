using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod.ToolsAndStuff
{
	class LinkControllerComp : MonoBehaviour
	{

		private void Start()
		{
			self = this.GetComponent<DebrisObject>();

			var comp = otherItem.gameObject.AddComponent<LinkControllerComp2>();
			comp.otherItem = self;
			comp.extantLink = extantLink;

		}

		private void Update()
		{


			PlayerController player = GameManager.Instance.PrimaryPlayer;
			if (player && this.extantLink == null)
			{
				tk2dTiledSprite component = SpawnManager.SpawnVFX(this.LinkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
				this.extantLink = component;
			}
			else if (player && this.extantLink != null)
			{
				if (this.extantLink.GetComponent<Renderer>().material.shader != ShaderCache.Acquire("Brave/Internal/HologramShader"))
				{
					this.extantLink.GetComponent<Renderer>().material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
				}

				if (otherItem != null && self != null)
				{
					Tools.UpdateLink(otherItem, this.extantLink, self);
				}

			}
			else if (extantLink != null)
			{
				SpawnManager.Despawn(extantLink.gameObject);
				extantLink = null;
			}

			if (extantLink != null && otherItem == null)
			{
				SpawnManager.Despawn(extantLink.gameObject);

				UnityEngine.Object.DestroyImmediate(self.gameObject);

				extantLink = null;
			}
		}

		public GameObject LinkVFXPrefab;
		public tk2dTiledSprite extantLink;
		public DebrisObject otherItem;
		public DebrisObject self;
    }

    class LinkControllerComp2 : MonoBehaviour
    {

		private void Update()
		{

			if (extantLink != null && otherItem == null)
			{
				SpawnManager.Despawn(extantLink.gameObject);
				UnityEngine.Object.DestroyImmediate(this.gameObject);
				extantLink = null;
			}
		}
		public tk2dTiledSprite extantLink;
		public DebrisObject otherItem;
	}
}
