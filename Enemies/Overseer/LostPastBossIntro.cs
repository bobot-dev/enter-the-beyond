using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace BotsMod
{

    [RequireComponent(typeof(GenericIntroDoer))]
    public class LostPastBossIntro : SpecificIntroDoer
    {

        public bool m_finished;

        // private bool m_initialized;        
        public AIActor m_AIActor;

        //public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
        //{
        //    GameManager.Instance.StartCoroutine(PlaySound());
        //}

        public void ModifyCamera(bool value)
        {
            if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
            {
                return;
            }
            CameraController mainCameraController = GameManager.Instance.MainCameraController;
            if (!mainCameraController)
            {
                return;
            }
            if (value)
            {
                mainCameraController.OverrideZoomScale = 0.75f;                
            }
            else
            {
                mainCameraController.OverrideZoomScale = 1f;
            }
        }

        protected override void OnDestroy()
        {
            this.ModifyCamera(false);
            base.OnDestroy();
        }

        public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
        {
            GameManager.Instance.StartCoroutine(PlaySound());
        }

        private IEnumerator PlaySound()
        {
            ModifyCamera(true);
            yield return StartCoroutine(WaitForSecondsInvariant(3.2f));
            AkSoundEngine.PostEvent("Play_WPN_bsg_shot_01", base.aiActor.gameObject);
            yield break;
        }

        private IEnumerator WaitForSecondsInvariant(float time)
        {
            for (float elapsed = 0f; elapsed < time; elapsed += GameManager.INVARIANT_DELTA_TIME) { yield return null; }
            yield break;
        }
    }
}