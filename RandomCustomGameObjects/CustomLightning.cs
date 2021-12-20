using FerryMansOar;
using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class CustomLightning
    {
        public static GameObject lightningVFX;

        public static void Init()
        {
            var lightningObj = PrefabAPI.PrefabBuilder.BuildObject("VFX_PurpleChainLightning");

            List<string> animationPaths = new List<string>()
            {
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_001",
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_002",
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_003",
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_004",
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_005",
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_006",
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_007",
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_008",
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_009",
                "BotsMod/sprites/Lightning/purple_lightning_beam_middle_010",
            };

            int spriteID = SpriteBuilder.AddSpriteToCollection(animationPaths[0], ETGMod.Databases.Items.ProjectileCollection, animationPaths[0].Split('/').Last());
            tk2dTiledSprite tiledSprite = lightningObj.GetOrAddComponent<tk2dTiledSprite>();
            //TrailController trailController = lightningObj.GetOrAddComponent<TrailController>();

            tk2dSpriteAnimator animator = lightningObj.GetOrAddComponent<tk2dSpriteAnimator>();

            tk2dSpriteAnimation animation = lightningObj.GetOrAddComponent<tk2dSpriteAnimation>();


            

            tiledSprite.SetSprite(ETGMod.Databases.Items.ProjectileCollection, spriteID);
            tiledSprite.automaticallyManagesDepth = true;

            tiledSprite.dimensions = new Vector2(0, 14);

            //tk2dSpriteDefinition def = tiledSprite.GetCurrentSpriteDef();


            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip() { name = "purple_lightning_beam_middle", frames = new tk2dSpriteAnimationFrame[0], fps = 12, wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop };
            clip.CopyFrom(Game.Items["shock_rounds"].GetComponent<ComplexProjectileModifier>().ChainLightningVFX.GetComponent<tk2dSpriteAnimator>().Library.GetClipById(Game.Items["shock_rounds"].GetComponent<ComplexProjectileModifier>().ChainLightningVFX.GetComponent<tk2dSpriteAnimator>().DefaultClipId));
            int i = 0;
            clip.name = "purple_lightning_beam_middle";
            foreach (var frame in clip.frames)
            {
                frame.spriteCollection = ETGMod.Databases.Items.ProjectileCollection;
                frame.spriteId = SpriteBuilder.AddSpriteToCollection(animationPaths[i], frame.spriteCollection, animationPaths[i].Split('/').Last());

                i++;
            }

           


            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            animation.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
            /*if (animationPaths != null)
            {
                
                List<string> spritePaths = animationPaths;

                List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
                foreach (string path in spritePaths)
                {
                    tk2dSpriteCollectionData collection = ETGMod.Databases.Items.ProjectileCollection;
                    int frameSpriteId = SpriteBuilder.AddSpriteToCollection(path, collection);
                    tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                    frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.MiddleLeft);
                    frameDef.colliderVertices = def.colliderVertices;
                    frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
                }

                clip.frames = frames.ToArray();
               
            }*/
            //animator.DefaultClipId = 0;
            animator.playAutomatically = true;
            lightningVFX = lightningObj;
        }
    }
}
