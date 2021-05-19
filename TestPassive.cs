using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using BotsMod;
using System.Reflection;

namespace BotsMod
{
    public class TestPassive : BulletStatusEffectItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {

            string itemName = "happyiness";
            string resourceName = "BotsMod/sprites/wip";
            GameObject obj = new GameObject();
            //var item = BotsModule.WarCrime2;//obj.AddComponent<PirmalShotgrub>().GetComponent<PickupObject>();
            var item = obj.AddComponent<TestPassive>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "testing item";
            string longDesc = "this item is purly for testing";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "");
            item.quality = ItemQuality.EXCLUDED;

            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RangeMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);

        }
        private ImprovedAfterImage zoomy;
        public override void Pickup(PlayerController player)
        {

            WindowRect = new Rect(500f, 0f, 450f, 900f);

           



            zoomy = player.gameObject.AddComponent<ImprovedAfterImage>();
            zoomy.dashColor = new Color(180, 32, 42);
            zoomy.spawnShadows = false;
            zoomy.shadowTimeDelay = 0.01f;
            zoomy.shadowLifetime = 0.3f;
            zoomy.minTranslation = 0.05f;
            zoomy.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
            player.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
            zoomy.spawnShadows = true;


            player.healthHaver.ApplyDamage(10000, Vector2.zero, "happiness");

            base.Pickup(player);
        }

        private static Rect WindowRect;

        private static Vector2 ScrollPos;

        private Shader m_glintShader;
        protected override void Update()
        {

        }




        public static void DrawProperty(string text)
        {
            GUILayout.Label(text, new GUILayoutOption[0]);
            GUILayout.Space(8f);
            
        }

        public void WindowFunction(int windowID)
        {
            ScrollPos = GUILayout.BeginScrollView(ScrollPos, new GUILayoutOption[0]);
            GUILayout.EndScrollView();
            UnityEngine.GUI.DragWindow();
            UnityEngine.GUI.color = Color.green;
        }


        private void ProcessGunShader(Gun g)
        {
            MeshRenderer component = g.GetComponent<MeshRenderer>();
            if (!component)
            {
                return;
            }
            Material[] sharedMaterials = component.sharedMaterials;
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                if (sharedMaterials[i].shader == this.m_glintShader)
                {
                    return;
                }
            }
            Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
            Material material = new Material(this.m_glintShader);
            material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
            sharedMaterials[sharedMaterials.Length - 1] = material;
            component.sharedMaterials = sharedMaterials;
        }
    }

        
}

/*
protected override void Update()
{
    base.Update();
    if (!this.m_pickedUp && base.gameObject.layer != LayerMask.NameToLayer("Unpixelated"))
    {
        base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
    }
}

// Token: 0x06007082 RID: 28802 RVA: 0x002CA328 File Offset: 0x002C8528
public override void Pickup(PlayerController player)
{
    if (this.m_pickedUp)
    {
        return;
    }
    SimpleSpriteRotator[] componentsInChildren = base.GetComponentsInChildren<SimpleSpriteRotator>();
    if (componentsInChildren != null)
    {
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
        }
    }
    GameManager.Instance.PrimaryPlayer.PastAccessible = true;
    Shader.SetGlobalFloat("_MapActive", 1f);

    CreateCompanion(player);
    CreateCompanion2(player);

    base.Pickup(player);
}
private GameObject m_extantCompanion;

string CompanionGuid = "ebf2314289ff4a4ead7ea7ef363a0a2e";
string CompanionGuid2 = "ab4a779d6e8f429baafa4bf9e5dca3a9";

private void CreateCompanion(PlayerController owner)
{
    CompanionSynergyProcessor companionSynergy = new CompanionSynergyProcessor();

    AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
    Vector3 position = owner.transform.position;
    GameObject extantCompanion = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, position, Quaternion.identity);
    m_extantCompanion = extantCompanion;
    CompanionController orAddComponent = m_extantCompanion.GetOrAddComponent<CompanionController>();
    orAddComponent.Initialize(owner);
    if (orAddComponent.specRigidbody)
    {
        PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
    }
}

private void CreateCompanion2(PlayerController owner)
{
    CompanionSynergyProcessor companionSynergy = new CompanionSynergyProcessor();

    AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid2);
    Vector3 position = owner.transform.position;
    GameObject extantCompanion = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, position, Quaternion.identity);
    m_extantCompanion = extantCompanion;
    CompanionController orAddComponent = m_extantCompanion.GetOrAddComponent<CompanionController>();
    orAddComponent.Initialize(owner);
    if (orAddComponent.specRigidbody)
    {
        PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
    }
}

// Token: 0x06007083 RID: 28803 RVA: 0x002CA398 File Offset: 0x002C8598
public override DebrisObject Drop(PlayerController player)
{
    DebrisObject debrisObject = base.Drop(player);
    debrisObject.GetComponent<TestPassive>().m_pickedUpThisRun = true;
    debrisObject.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
    return debrisObject;
}
/*
public override void Pickup(PlayerController player)
{
    PlayerItem currentItem = player.CurrentItem;
    float num = -1;
    currentItem.Use(player, out num);
    player.PostProcessProjectile += this.PostProcessProjectile;
    // this.proj = base.GetComponent<Projectile>();
    //Projectile projectile = this.proj;
    // projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
    /*int i = 0;

     foreach (PlayerItem item in player.activeItems)
     {
         PlayerItem playerItem = player.activeItems[i];
         //PlayerItem playerItem = new PlayerItem();
         float number = 0;
         playerItem.Use(player, out number);
         //playerItem.DoEffect(player) as PlayerItem;
         i++;
         ETGModConsole.Log(i + "", false);
     }

    base.Pickup(player);


}
private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
{
    obj.OnHitEnemy = this.HandleHitEnemy;

}
private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
{
    //if (hitRigidbody = null) return;
    StartCoroutine(BulletReturn(sourceProjectile));
}

private IEnumerator BulletReturn(Projectile sourceProjectile)
{
    yield return new WaitForSeconds(0.2f);
    sourceProjectile.Direction = sourceProjectile.Direction * -1;

    yield break;
}

private Projectile proj;*/
//}
//}
