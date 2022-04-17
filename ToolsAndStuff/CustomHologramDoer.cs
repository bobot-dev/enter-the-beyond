using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    public class CustomHologramDoer : BraveBehaviour
    {
        public CustomHologramDoer()
        {
            hologramIsGreen = false;
        }


        public bool hologramIsGreen;
        public GameObject extantSprite;
        private tk2dSprite m_ItemSprite;

        public void ShowSprite(tk2dSpriteCollectionData encounterIconCollection, int spriteID)
        {
            if (!encounterIconCollection)
            {
                return;
            }
            if (base.gameActor)
            {
                if (extantSprite) { Destroy(extantSprite); }
                extantSprite = new GameObject("Item Hologram", new Type[] { typeof(tk2dSprite) }) { layer = 0 };
                extantSprite.transform.position = (transform.position + new Vector3(0.5f, 2));
                m_ItemSprite = extantSprite.AddComponent<tk2dSprite>();
                // CloningAndDuplication.DuplicateSprite(m_ItemSprite, sprite as tk2dSprite);
                m_ItemSprite.SetSprite(encounterIconCollection, spriteID);
                m_ItemSprite.PlaceAtPositionByAnchor(extantSprite.transform.position, tk2dBaseSprite.Anchor.LowerCenter);
                m_ItemSprite.transform.localPosition = m_ItemSprite.transform.localPosition.Quantize(0.0625f);
                if (base.gameActor != null) { extantSprite.transform.parent = base.gameActor.transform; }

                if (m_ItemSprite)
                {
                    sprite.AttachRenderer(m_ItemSprite);
                    m_ItemSprite.depthUsesTrimmedBounds = true;
                    m_ItemSprite.UpdateZDepth();
                }
                sprite.UpdateZDepth();

                ApplyHologramShader(m_ItemSprite, hologramIsGreen);

            }
        }
        public void ShowSpinDownHologram(int itemId, GameObject obj)
        {
            tk2dSpriteCollectionData collection = AmmonomiconController.ForceInstance.EncounterIconCollection;
            itemId = SpinDownDice.SpinDownID(itemId);
            var pickupObject = PickupObjectDatabase.GetById(itemId);
            var spriteName = pickupObject?.encounterTrackable?.journalData?.AmmonomiconSprite;
            if (collection && pickupObject && !string.IsNullOrEmpty(spriteName))
            {



                
                var spriteId = collection.GetSpriteIdByName(spriteName);


                var m_hologramSprite = obj.transform.Find("spindown hologram")?.gameObject.GetComponent<tk2dSprite>();
                if (m_hologramSprite == null)
                {
                    GameObject go = new GameObject("spindown hologram");
                    m_hologramSprite = tk2dSprite.AddComponent(go, collection, spriteId);
                    m_hologramSprite.transform.parent = obj.transform;

                    //this.Glower.usesOverrideMaterial = true;
                    //this.Glower.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
                }
                else
                {
                    m_hologramSprite.SetSprite(collection, spriteId);
                    m_hologramSprite.ForceUpdateMaterial();
                }
                m_hologramSprite.renderer.enabled = true;
                m_hologramSprite.usesOverrideMaterial = true;
                m_hologramSprite.renderer.material.shader = BeyondPrefabs.fucktilesets.LoadAsset<Shader>("GayHologramShader");
                m_hologramSprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
                m_hologramSprite.renderer.material.SetFloat("_IsGreen", 1f);
                //m_hologramSprite.renderer.material.SetColor("_HoloColor", new Color32(255, 0, 179, 255));
                m_hologramSprite.PlaceAtPositionByAnchor(obj.GetComponent<tk2dSprite>() != null ? obj.GetComponent<tk2dSprite>().WorldTopCenter + new Vector2(0f, 0.25f) : (Vector2)obj.transform.position + new Vector2(0f, 0.5f), tk2dBaseSprite.Anchor.LowerCenter);
                m_hologramSprite.transform.localPosition = m_hologramSprite.transform.localPosition.Quantize(0.0625f);

            }
        }


        public void ApplyHologramShader(tk2dBaseSprite targetSprite, bool isGreen = false, bool usesOverrideMaterial = true)
        {

            

            Shader m_cachedShader = BeyondPrefabs.fucktilesets.LoadAsset<Shader>("GayHologramShader");
            Material m_cachedMaterial = new Material(m_cachedShader);
            m_cachedMaterial.name = "HologramMaterial";
            Material m_cachedSharedMaterial = m_cachedMaterial;

            m_cachedMaterial.SetTexture("_MainTex", targetSprite.renderer.material.GetTexture("_MainTex"));
            m_cachedSharedMaterial.SetTexture("_MainTex", targetSprite.renderer.sharedMaterial.GetTexture("_MainTex"));
            if (true)
            {
                m_cachedMaterial.SetFloat("_IsGreen", 1f);
                m_cachedSharedMaterial.SetFloat("_IsGreen", 1f);

                m_cachedMaterial.SetColor("_HoloColor", new Color32(255, 0, 179, 255));
                m_cachedSharedMaterial.SetColor("_HoloColor", new Color32(255, 0, 179, 255));
            }
            targetSprite.renderer.material.shader = m_cachedShader;
            targetSprite.renderer.material = m_cachedMaterial;
            targetSprite.renderer.sharedMaterial = m_cachedSharedMaterial;
            targetSprite.usesOverrideMaterial = usesOverrideMaterial;
        }

        public void HideSprite()
        {
            if (base.gameActor && extantSprite)
            {
                Destroy(extantSprite);
            }
        }
    }
}
