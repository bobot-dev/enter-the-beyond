using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using UnityEngine;


namespace ChallengeAPI
{
    /// <summary>
    /// A more advanced example challenge made by Some Bunny
    /// </summary>
    public class ChambersCurseModifier : ChallengeModifier
    {
        public void Start()
        {
            //If you want something to happen when the challenge starts, this is the place.
        }

        public void Update()
        {
            //If you want something to happen every frame while it's active, this is the place.
            PlayerController player = GameManager.Instance.PrimaryPlayer;
            this.elapsed += BraveTime.DeltaTime;
            bool flag3 = this.elapsed > 4f;
            if (flag3)
            {
                AkSoundEngine.PostEvent("Play_BOSS_wall_slam_01", base.gameObject);
                this.FireRocket(dragunBoulder, player.sprite.WorldCenter, player);
                this.elapsed = 0f;
            }
        }
        private void FireRocket(GameObject skyRocket, Vector2 target, PlayerController player1)
        {
            PlayerController player = player1;
            SkyRocket component = SpawnManager.SpawnProjectile(skyRocket, target, Quaternion.identity, true).GetComponent<SkyRocket>();
            component.TargetVector2 = target;
            tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
            component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
            component.ExplosionData.ignoreList.Add(player.specRigidbody);
        }

        public void OnDestroy()
        {
            //If you want something to happen when the challenge ends, this is the place.
        }

        public override bool IsValid(RoomHandler room)
        {
            //This method checks if the room is valid for this challenge. If you return true it means it's valid, if you return false it means it's not valid.
            return true;
        }

        private float elapsed;
        public GameObject dragunBoulder;
    }
}
