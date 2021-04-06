using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
	public class BossFinalLostDeathController : BraveBehaviour
	{
		// Token: 0x0600587E RID: 22654 RVA: 0x00212A91 File Offset: 0x00210C91
		public void Start()
		{
			base.healthHaver.OnPreDeath += this.OnBossDeath;
		}

		// Token: 0x0600587F RID: 22655 RVA: 0x0003AB5B File Offset: 0x00038D5B
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06005880 RID: 22656 RVA: 0x00212AAC File Offset: 0x00210CAC
		private void OnBossDeath(Vector2 dir)
		{
			LostPastController lostPastController = UnityEngine.Object.FindObjectOfType<LostPastController>();
			lostPastController.OnBossKilled(base.transform);
			BotsModule.Log("Overseer got hit to much like a fucking idiot", BotsModule.LOST_COLOR);
		}
	}
}
