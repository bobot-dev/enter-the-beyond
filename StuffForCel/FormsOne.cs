using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using ItemAPI;
using Dungeonator;
namespace Items
{
    public class UnstableStrafeForm : MonoBehaviour
    {
        
        public void Awake()
        {
            this.item = base.GetComponent<PassiveItem>();
            this.player = item.Owner;
            
        }
        public void Update()
        {
            
        }
        public void OnDestroy()
        {
            this.item = null;
            this.player = null;
        }

        private PlayerController player;
        private PassiveItem item;
    }
    public class NevernamedForm : MonoBehaviour
    {
        public void Awake()
        {
            this.item = base.GetComponent<PassiveItem>();
            this.player = item.Owner;
            HandleRadialIndicator(7, player.sprite);
            CelsItems.Log("NN effect has awoken");
        }
        public void Update()
        {
            RoomHandler r = player.sprite.transform.position.GetAbsoluteRoom();
            Vector3 tableCenter = player.sprite.WorldCenter.ToVector3ZisY(0f);
            Action<AIActor, float> AuraAction = delegate (AIActor actor, float dist)
            {
                actor.healthHaver.ApplyDamage(.45f, Vector2.zero, "bow unto his power");
            };
            r.ApplyActionToNearbyEnemies(tableCenter.XY(), 7, AuraAction);
            CelsItems.Log("NN effect should've triggered");
        }
        private void HandleRadialIndicator(float Radius, tk2dBaseSprite Psprite)
        {
            Psprite = player.sprite;
            if (!this.indicator)
            {
                Vector3 position = Psprite.WorldCenter.ToVector3ZisY(0f);
                indicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), position, Quaternion.identity, player.sprite.transform)).GetComponent<HeatIndicatorController>();
                indicator.CurrentRadius = 7;
                //indicator.CurrentColor = new Color(20, 19, 20);
            }
        }
        private void UnhandleRadialIndicator()
        {
            if (indicator)
            {
                indicator.EndEffect();
                indicator = null;
            }
        }
        public void OnDestroy()
        {
            this.item = null;
            this.player = null;
            UnhandleRadialIndicator();
        }
        private PlayerController player;
        private PassiveItem item;
        private HeatIndicatorController indicator;
    }
    public class TurtleForm : MonoBehaviour
    {
        public void Awake()
        {
            this.item = base.GetComponent<PassiveItem>();
            this.player = item.Owner;
        }
        public void Update()
        {

        }
        public void OnDestroy()
        {
            this.item = null;
            this.player = null;
        }
        private PlayerController player;
        private PassiveItem item;
    }
    public class BlazeyForm : MonoBehaviour
    {
        public void Awake()
        {
            this.item = base.GetComponent<PassiveItem>();
            this.player = item.Owner;
        }
        public void Update()
        {

        }
        public void OnDestroy()
        {
            this.item = null;
            this.player = null;
        }
        private PlayerController player;
        private PassiveItem item;
    }
}
