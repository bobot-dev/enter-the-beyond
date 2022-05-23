using CustomCharacters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class LostPastController : MonoBehaviour
    {

		public void OnBossKilled(Transform bossTransform)
		{
			base.StartCoroutine(this.HandleBossKilled());
		}

		private IEnumerator HandleBossKilled()
		{
			GameStatsManager.Instance.SetCharacterSpecificFlag((PlayableCharacters)CustomPlayableCharacters.Lost, CharacterSpecificGungeonFlags.KILLED_PAST, true);
			SaveAPI.SaveAPIManager.SetFlag("bot", SaveFlags.BOT_BOSSKILLED_LOST_PAST, true);
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_KILLED_PAST, 1f);


			PlayerController m_lost = GameManager.Instance.PrimaryPlayer;

			GameManager.Instance.MainCameraController.OverridePosition = m_lost.CenterPosition;
			//while (this.VictoryTalkDoer.IsTalking)
			//{
			//	yield return null;
			//}
			yield return new WaitForSeconds(0.5f);
			Pixelator.Instance.FreezeFrame();
			BraveTime.RegisterTimeScaleMultiplier(0f, this.gameObject);
			float ela = 0f;
			while (ela < ConvictPastController.FREEZE_FRAME_DURATION)
			{
				ela += GameManager.INVARIANT_DELTA_TIME;
				yield return null;
			}
			BraveTime.ClearMultiplier(this.gameObject);
			TimeTubeCreditsController ttcc = new TimeTubeCreditsController();
			ttcc.ClearDebris();
			yield return this.StartCoroutine(ttcc.HandleTimeTubeCredits(m_lost.sprite.WorldCenter, false, null, -1, false));
			AmmonomiconController.Instance.OpenAmmonomicon(true, true);
			yield break;
		}
	}
}
