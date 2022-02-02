﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dungeonator;
using ItemAPI;
using UnityEngine;
using GungeonAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using BotsMod;

namespace BreakAbleAPI
{


    /// <summary>
    /// This class is important if youre adding shadows to your breakables.
    /// </summary>
    public class ShadowHandler : MonoBehaviour
    {
        public ShadowHandler()
        {
            this.shadowObject = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("DefaultShadowSprite"));
            this.Offset = new Vector2(0, 0);
        }

        public void Start()
        {
            GameObject shadowObj = (GameObject)UnityEngine.Object.Instantiate(shadowObject);
            shadowObj.transform.parent = base.gameObject.transform;
            tk2dSprite shadowSprite = shadowObj.GetComponent<tk2dSprite>();
            shadowSprite.renderer.enabled = true;
            shadowSprite.HeightOffGround = base.gameObject.GetComponent<tk2dSprite>().HeightOffGround - 0.1f;
            shadowObj.transform.position.WithZ(base.gameObject.transform.position.z + 99999f);
            shadowObj.transform.position = base.gameObject.transform.position + Offset;
            DepthLookupManager.ProcessRenderer(shadowObj.GetComponent<Renderer>(), DepthLookupManager.GungeonSortingLayer.BACKGROUND);
            shadowSprite.usesOverrideMaterial = true;
            shadowSprite.renderer.material.shader = Shader.Find("Brave/Internal/SimpleAlphaFadeUnlit");
            shadowSprite.renderer.material.SetFloat("_Fade", 0.66f);
        }


        public Vector3 Offset;
        public GameObject shadowObject;
    }
  



    public static class BreakableAPIToolbox
    {
        /// <summary>
        /// Generates, and returns a KickableObject. This is for generating a basic one, it returns it so you can additionally modify it without cluttering up the setup method too much. Reminder, KickableObjects have a MinorBreakable component that you could modify as well!
        /// </summary>
        /// <param name="name">The name of your kickable. Keep it simple, its used in generating your animations, so no special characters.</param>
        /// <param name="idleSpritePaths">Your sprite paths. Only insert one path if you don't want it to be animated.</param>
        /// <param name="rollNorthPaths">The sprite paths for the animation for when it rolls NORTH.</param>
        /// <param name="rollSouthPaths">The sprite paths for the animation for when it rolls SOUTH.</param>
        /// <param name="rollEastPaths">The sprite paths for the animation for when it rolls EAST.</param>
        /// <param name="rollWestPaths">The sprite paths for the animation for when it rolls WEST.</param>
        /// <param name="impactNorthPaths">The sprite paths for the animation for when it is broken during a rolling animation state. This one is for when its facing NORTH.</param>
        /// <param name="impactSouthPaths">The sprite paths for the animation for when it is broken during a rolling animation state. This one is for when its facing SOUTH.</param>
        /// <param name="impactEastPaths">The sprite paths for the animation for when it is broken during a rolling animation state. This one is for when its facing EAST.</param>
        /// <param name="impactWestPaths">The sprite paths for the animation for when it is broken during a rolling animation state. This one is for when its facing WEST.</param>
        /// <param name="impactNotRollingPaths">The sprite paths for the animation for when it is broken before it has been kicked.</param>
        /// <param name="RolledIntoBreakPaths">The sprite paths for the animation for when it is broken WHEN the player DODGEROLLS into the kickable.</param>
        /// <param name="idleAnimFPS">The FPS of your idle animation.</param>
        /// <param name="rollAnimFPS">The FPS of your all your rolling animations. No, I will not add support for each direction having its own FPS, fuck off and fuck you.</param>
        /// <param name="breakAnimFPS">The FPS of your all your breaking animations. No, I will not add support for each direction having its own FPS, again, fuck off and fuck you.</param>
        /// <param name="breakNotRollingFPS">The FPS of your broken-before-kicked animation.</param>
        /// <param name="breakRolledIntoFPS">The FPS of your broken-when-dodgerolled-into animation.</param>
        /// <param name="UsesCustomColliderValues">Setting this to true will let you use custom collider sizes and offsets. Keeping it false will use no offsets and generate a size based on the sprites size.</param>
        /// <param name="ColliderSizeX">The X Value of your collider. Only used if UsesCustomColliderValues is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="ColliderSizeY">The Y Value of your collider. Only used if UsesCustomColliderValues is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="ColliderOffsetX">The X Value of your colliders offset. Only used if UsesCustomColliderValues is true.</param>
        /// <param name="ColliderOffsetY">The Y Value of your colliders offset. Only used if UsesCustomColliderValues is true.</param>
        /// <param name="HasAdditionalCollisions">If true, adds a BulletBlocker, EnemyBlocker and PlayerBlocker CollisionLayer to your kickable .</param>
        /// <param name="AdditionalCollisionsUseColliderSizes">If true, uses the collider sizes and offsets that you give later, else it will use the same sizes as given previously.</param>
        /// <param name="AdiitionalColliderSizeX">The X Value of your additional colliders. Only used if AdditionalCollisionsUseColliderSizes is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="AdiitionalColliderSizeY">The Y Value of your additional colliders. Only used if AdditionalCollisionsUseColliderSizes is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="AdiitionalColliderOffsetX">The X offset of your additional colliders. Only used if AdditionalCollisionsUseColliderSizes is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="AdiitionalColliderSizeY">The Y offset of your additional colliders. Only used if AdditionalCollisionsUseColliderSizes is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="breakAudioEvent">The name of the sound that plays when your kickable is broken.</param>
        /// <param name="rollingSpeed">The speed at which your kickable moves.</param>
        public static KickableObject GenerateKickableObject(string name, string[] idleSpritePaths, string[] rollNorthPaths, string[] rollSouthPaths, string[] rollEastPaths, string[] rollWestPaths, string[] impactNorthPaths, string[] impactSouthPaths, string[] impactEastPaths, string[] impactWestPaths,string[] impactNotRollingPaths, string[]RolledIntoBreakPaths, int idleAnimFPS = 4,int rollAnimFPS = 5, int breakAnimFPS = 4, int breakNotRollingFPS = 4, int breakRolledIntoFPS = 4,bool UsesCustomColliderValues = false, int ColliderSizeX = 16, int ColliderSizeY = 8, int ColliderOffsetX = 0, int ColliderOffsetY = 8, bool HasAdditionalCollisions = true, bool AdditionalCollisionsUseColliderSizes = true, int AdiitionalColliderSizeX = 8, int AdiitionalColliderSizeY = 8, int AdiitionalColliderOffsetX = 0, int AdiitionalColliderOffsetY = 0, string breakAudioEvent = "Play_OBJ_barrel_break_01", float rollingSpeed = 3)
        {
            Texture2D textureFromResource = GungeonAPI.ResourceExtractor.GetTextureFromResource(idleSpritePaths[0]);
            GameObject gameObject = SpriteBuilder.SpriteFromResource(idleSpritePaths[0], null);
            FakePrefab.MarkAsFakePrefab(gameObject);
            gameObject.name = name;
            KickableObject kickable = gameObject.AddComponent<KickableObject>();
            MinorBreakable breakable = gameObject.AddComponent<MinorBreakable>();

            tk2dSpriteCollectionData KickableSpriteCollection = SpriteBuilder.ConstructCollection(gameObject, (name + "_Collection"));
            int spriteID = SpriteBuilder.AddSpriteToCollection(idleSpritePaths[0], KickableSpriteCollection);
            tk2dSprite sprite = gameObject.GetOrAddComponent<tk2dSprite>();
            sprite.SetSprite(KickableSpriteCollection, spriteID);

            IntVector2 intVector = new IntVector2(ColliderSizeX, ColliderSizeY);
            IntVector2 colliderOffset = new IntVector2(ColliderOffsetX, ColliderOffsetY);
            IntVector2 colliderSize = new IntVector2(intVector.x, intVector.y);

            IntVector2 intVectorAdditional = new IntVector2(AdiitionalColliderSizeX, AdiitionalColliderSizeY);
            IntVector2 AdditionalcolliderSize = new IntVector2(intVectorAdditional.x, intVectorAdditional.y);
            IntVector2 AdditionalcolliderOffset = new IntVector2(AdiitionalColliderOffsetX, AdiitionalColliderOffsetY);



            if (UsesCustomColliderValues == false)
            {
                IntVector2 nonCustomintVector = new IntVector2(textureFromResource.width, textureFromResource.height);
                colliderSize = new IntVector2(nonCustomintVector.x, nonCustomintVector.y);
            }
            if (AdditionalCollisionsUseColliderSizes == true)
            {
                AdditionalcolliderSize = colliderSize;
                AdditionalcolliderOffset = colliderOffset;
            }



            SpeculativeRigidbody speculativeRigidbody = sprite.SetUpSpeculativeRigidbody(colliderOffset, colliderSize);
            speculativeRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.HighObstacle,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualWidth = colliderSize.x,
                ManualHeight = colliderSize.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0,
            });
            if (HasAdditionalCollisions == true)
            {
                speculativeRigidbody.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.BulletBlocker,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = AdditionalcolliderSize.x,
                    ManualOffsetY = AdditionalcolliderSize.y,
                    ManualWidth = AdditionalcolliderOffset.x,
                    ManualHeight = AdditionalcolliderOffset.y,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,
                });
                speculativeRigidbody.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.PlayerBlocker,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = AdditionalcolliderSize.x,
                    ManualOffsetY = AdditionalcolliderSize.y,
                    ManualWidth = AdditionalcolliderOffset.x,
                    ManualHeight = AdditionalcolliderOffset.y,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,
                });
                speculativeRigidbody.PixelColliders.Add(new PixelCollider
                {
                    ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                    CollisionLayer = CollisionLayer.EnemyBlocker,
                    IsTrigger = false,
                    BagleUseFirstFrameOnly = false,
                    SpecifyBagelFrame = string.Empty,
                    BagelColliderNumber = 0,
                    ManualOffsetX = AdditionalcolliderSize.x,
                    ManualOffsetY = AdditionalcolliderSize.y,
                    ManualWidth = AdditionalcolliderOffset.x,
                    ManualHeight = AdditionalcolliderOffset.y,
                    ManualDiameter = 0,
                    ManualLeftX = 0,
                    ManualLeftY = 0,
                    ManualRightX = 0,
                    ManualRightY = 0,
                });
            }
            tk2dSpriteAnimator animator = gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = gameObject.AddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            tk2dSpriteAnimationClip idleClip = new tk2dSpriteAnimationClip() { name = "idle", frames = new tk2dSpriteAnimationFrame[0], fps = idleAnimFPS };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < idleSpritePaths.Length; i++)
            {
                tk2dSpriteCollectionData collection = KickableSpriteCollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(idleSpritePaths[i], collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerLeft);
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
            }
            tk2dSpriteAnimationClip rollNorth = AddAnimation(animator, KickableSpriteCollection, rollNorthPaths, name + "_roll_north", rollAnimFPS, tk2dSpriteAnimationClip.WrapMode.Loop);
            tk2dSpriteAnimationClip rollSouth = AddAnimation(animator, KickableSpriteCollection, rollSouthPaths, name + "_roll_south", rollAnimFPS, tk2dSpriteAnimationClip.WrapMode.Loop);
            tk2dSpriteAnimationClip rollEast = AddAnimation(animator, KickableSpriteCollection, rollEastPaths, name + "_roll_east", rollAnimFPS, tk2dSpriteAnimationClip.WrapMode.Loop);
            tk2dSpriteAnimationClip rollWest = AddAnimation(animator, KickableSpriteCollection, rollWestPaths, name + "_roll_west", rollAnimFPS, tk2dSpriteAnimationClip.WrapMode.Loop);
            kickable.rollAnimations = new string[]
            {
                name + "_roll_north",
                name + "_roll_west",
                name + "_roll_south",
                name + "_roll_east",
            };
            tk2dSpriteAnimationClip impactNorth = AddAnimation(animator, KickableSpriteCollection, impactNorthPaths, name + "_impact_north", breakAnimFPS, tk2dSpriteAnimationClip.WrapMode.Once);
            tk2dSpriteAnimationClip impactSouth = AddAnimation(animator, KickableSpriteCollection, impactSouthPaths, name + "_impact_south", breakAnimFPS, tk2dSpriteAnimationClip.WrapMode.Once);
            tk2dSpriteAnimationClip impactEast = AddAnimation(animator, KickableSpriteCollection, impactEastPaths, name + "_impact_east", breakAnimFPS, tk2dSpriteAnimationClip.WrapMode.Once);
            tk2dSpriteAnimationClip impactWest = AddAnimation(animator, KickableSpriteCollection, impactWestPaths, name + "_impact_west", breakAnimFPS, tk2dSpriteAnimationClip.WrapMode.Once);
            kickable.impactAnimations = new string[]
            {
                name + "_impact_north",
                name + "_impact_west",
                name + "_impact_south",
                name + "_impact_east",
            };
            tk2dSpriteAnimationClip breakNotRolling = AddAnimation(animator, KickableSpriteCollection, impactNotRollingPaths, name + "_impact_nonroll", breakNotRollingFPS, tk2dSpriteAnimationClip.WrapMode.Once);

            tk2dSpriteAnimationClip rolledInto = AddAnimation(animator, KickableSpriteCollection, RolledIntoBreakPaths, name + "_rolled_into", breakRolledIntoFPS, tk2dSpriteAnimationClip.WrapMode.Once);
            kickable.RollingBreakAnim = name + "_rolled_into";
            breakable.breakAnimName = name + "_impact_nonroll";
            idleClip.frames = frames.ToArray();
            idleClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            animator.Library.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { idleClip, rollNorth, rollSouth, rollEast, rollWest, impactNorth, impactSouth, impactEast, impactWest, breakNotRolling, rolledInto }).ToArray();
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("idle");
            breakable.breakAudioEventName = breakAudioEvent;

            kickable.sprite = sprite;
            kickable.spriteAnimator = animator;

            kickable.AllowTopWallTraversal = true; 
            kickable.rollSpeed = rollingSpeed;
            return kickable; 
        }
        public static tk2dSpriteAnimationClip AddAnimation(tk2dSpriteAnimator animator, tk2dSpriteCollectionData Tablecollection, string[] spritePaths, string clipName, int FPS, tk2dSpriteAnimationClip.WrapMode wrapMode)
        {
            tk2dSpriteAnimation animation = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            tk2dSpriteAnimationClip idleClip = new tk2dSpriteAnimationClip() { name = clipName, frames = new tk2dSpriteAnimationFrame[0], fps = FPS };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < spritePaths.Length; i++)
            {
                tk2dSpriteCollectionData collection = Tablecollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(spritePaths[i], collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
            }
            idleClip.frames = frames.ToArray();
            idleClip.wrapMode = wrapMode;
            return idleClip;
        }

        /// <summary>
        /// Generates, and returns a FlippableCover. This is for generating a basic one, it returns it so you can additionally modify it without cluttering up the setup method too much. Reminder, FlippableCovers have a MajorBreakable component that you could modify as well!
        /// </summary>
        /// <param name="name">The name of your kickable. Keep it simple, its used in generating your animations, so no special characters.</param>
        /// <param name="idleSpritePaths">Your sprite paths. Only insert one path if you don't want it to be animated.</param>
        /// <param name="outlinePaths">Your sprite paths for the *outlines* that appear when you are nearby a table. Of note, your array of path should be in a SPECIFIC order, with North being 1st, East being 2nd, West being 3rd and South being forth.</param>

        /// <param name="northFlipPaths">Your sprite paths for the flip animations facing NORTH.</param>
        /// <param name="southFlipPaths">Your sprite paths for the flip animations facing SOUTH.</param>
        /// <param name="eastFlipPaths">Your sprite paths for the flip animations facing EAST.</param>
        /// <param name="westFlipPaths">Your sprite paths for the flip animations facing WEST.</param>

        /// <param name="northBreakPaths">Your sprite paths for the flipped break animations facing NORTH.</param>
        /// <param name="southBreakPaths">Your sprite paths for the flipped break animations facing SOUTH.</param>
        /// <param name="eastBreakPaths">Your sprite paths for the flipped break animations facing EAST.</param>
        /// <param name="westBreakPaths">Your sprite paths for the flipped break animations facing WEST.</param>

        /// <param name="unflippedBreakPaths">Your sprite paths for the break animations when the table has NOT been flipped yet.</param>

        /// <param name="IdleFPS">The FPS of your idle animation.</param>
        /// <param name="FlipFPS">The FPS of all your flip animations.</param>
        /// <param name="BreakFPS">The FPS of all your break-while-flipped animations.</param>
        /// <param name="UnflippedBreakFPS">The FPS of your break-while-unflipped animation.</param>

        /// <param name="UsesCustomColliderValues">Setting this to true will let you use custom collider sizes and offsets. Keeping it false will use no offsets and generate a size based on the sprites size.</param>
        /// <param name="ColliderSizeX">The X Value of your collider. Only used if UsesCustomColliderValues is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="ColliderSizeY">The Y Value of your collider. Only used if UsesCustomColliderValues is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="ColliderOffsetX">The X Value of your colliders offset. Only used if UsesCustomColliderValues is true.</param>
        /// <param name="ColliderOffsetY">The Y Value of your colliders offset. Only used if UsesCustomColliderValues is true.</param>

        /// <param name="FlippedColliderSizeX_Horizontal">The X Value of your collider for when the table is flipped NORTH or SOUTH. My code *should* automatically place the hit box appropriate to the edge of the table. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="FlippedColliderSizeY_Horizontal">The Y Value of your collider for when the table is flipped NORTH or SOUTH. My code *should* automatically place the hit box appropriate to the edge of the table. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="FlippedColliderSizeX_Vertical">The X Value of your collider for when the table is flipped EAST or WEST. My code *should* automatically place the hit box appropriate to the edge of the table. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="FlippedColliderSizeY_Vertical">The Y Value of your collider for when the table is flipped EAST or WEST. My code *should* automatically place the hit box appropriate to the edge of the table. Note that 1 here means 1 *pixel*, and not 1 tile!</param>

        /// <param name="flipStyle">The directions in which your table is able to be flipped.</param>

        /// <param name="TableHP">The amount of HP your table has. Breaks when its HP reaches 0.</param>

        /// <param name="ShadowSpritePath">currently does nothing. leave it as null.</param>

        /// <param name="breakAnimPercentagesAndSpritePaths">Switches the tables flipped sprite to one given when its at a certain percentage of HP. The percentage should be a value like 50 if you want it to switch at 50% HP. The string you have to give is a SPRITE PATH to the sprite you want it to switch to./param>
        /// <param name="unflippedBreakAnimPercentagesAndSpritePaths">Switches the tables idle sprite to one given when its at a certain percentage of HP. The percentage should be a value like 50 if you want it to switch at 50% HP. The string you have to give is a SPRITE PATH to the sprite you want it to switch to./param>

        public static FlippableCover GenerateTable(string name, string[] idleSpritePaths, string[] outlinePaths, string[] northFlipPaths, string[] southFlipPaths, string[] eastFlipPaths, string[] westFlipPaths, string[] northBreakPaths, string[] southBreakPaths, string[] eastBreakPaths, string[] westBreakPaths, string[] unflippedBreakPaths, int IdleFPS = 4, int FlipFPS = 6, int BreakFPS = 7, int UnflippedBreakFPS = 5, bool UsesCustomColliderValues = false, int ColliderSizeX = 16, int ColliderSizeY = 8, int ColliderOffsetX = 0, int ColliderOffsetY = 8, int FlippedColliderSizeX_Horizontal = 20, int FlippedColliderSizeY_Horizontal = 2, int FlippedColliderSizeX_Vertical= 4, int FlippedColliderSizeY_Vertical = 8, FlippableCover.FlipStyle flipStyle = FlippableCover.FlipStyle.ANY, float TableHP = 90, string ShadowSpritePath = null, Dictionary<float, string> breakAnimPercentagesAndSpritePaths = null, Dictionary<float, string> unflippedBreakAnimPercentagesAndSpritePaths = null)
        {
            Texture2D textureFromResource = GungeonAPI.ResourceExtractor.GetTextureFromResource(idleSpritePaths[0]);
            GameObject gameObject = SpriteBuilder.SpriteFromResource(idleSpritePaths[0], null);
            FakePrefab.MarkAsFakePrefab(gameObject);
            gameObject.name = name + "_Table";
            FlippableCover table = gameObject.AddComponent<FlippableCover>();
            MajorBreakable majorBreakable = gameObject.AddComponent<MajorBreakable>();

            tk2dSpriteCollectionData TableCollection = SpriteBuilder.ConstructCollection(gameObject, (name + "_Collection"));
            int spriteID = SpriteBuilder.AddSpriteToCollection(idleSpritePaths[0], TableCollection);
            tk2dSprite sprite = gameObject.GetOrAddComponent<tk2dSprite>();
            tk2dBaseSprite baseSprite = gameObject.GetOrAddComponent<tk2dBaseSprite>();
            baseSprite.SetSprite(TableCollection, spriteID);
            sprite.SetSprite(TableCollection, spriteID);

            baseSprite.HeightOffGround = 0;
            sprite.HeightOffGround = 0;
            gameObject.layer = 0;

            IntVector2 colliderOffset = new IntVector2(ColliderOffsetX, ColliderOffsetY);
            IntVector2 colliderSize = new IntVector2(ColliderSizeX, ColliderSizeY);
            if (UsesCustomColliderValues == false)
            {
                IntVector2 nonCustomintVector = new IntVector2(textureFromResource.width, textureFromResource.height);
                colliderSize = new IntVector2(nonCustomintVector.x, nonCustomintVector.y);
            }
            majorBreakable.HitPoints = TableHP;
            tk2dSpriteAnimator animator = gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = gameObject.AddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;

            List<tk2dSpriteAnimationClip> clips = new List<tk2dSpriteAnimationClip>();

            if (idleSpritePaths.Length >= 1)
            {
                tk2dSpriteAnimationClip idleClip = new tk2dSpriteAnimationClip() { name = "idle", frames = new tk2dSpriteAnimationFrame[0], fps = IdleFPS };
                List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
                for (int i = 0; i < idleSpritePaths.Length; i++)
                {
                    tk2dSpriteCollectionData collection = TableCollection;
                    int frameSpriteId = SpriteBuilder.AddSpriteToCollection(idleSpritePaths[i], collection);
                    tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                    frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerLeft);
                    tk2dSpriteDefinition frameDefMod = GenerateColliderForSpriteDefinition(frameDef, new Vector3(colliderSize.x, colliderSize.y), new Vector3(colliderOffset.x, colliderOffset.y));
                    frameDef = frameDefMod;
                    frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
                }
                idleClip.frames = frames.ToArray();
                idleClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
                clips.Add(idleClip);
            }

            table.flipStyle = flipStyle;

            table.outlineNorth = GenerateTableOutlineObject("North_" + name, outlinePaths[0], table.gameObject, TableCollection);
            table.outlineEast = GenerateTableOutlineObject("East_" + name, outlinePaths[1], table.gameObject, TableCollection);
            table.outlineWest = GenerateTableOutlineObject("West_" + name, outlinePaths[2], table.gameObject, TableCollection);
            table.outlineSouth = GenerateTableOutlineObject("South_" + name, outlinePaths[3], table.gameObject, TableCollection);

            if (ShadowSpritePath != null)
            {
                GameObject shadowObject = SpriteBuilder.SpriteFromResource(ShadowSpritePath, null);
                FakePrefab.MarkAsFakePrefab(shadowObject);
                int shadowID = SpriteBuilder.AddSpriteToCollection(ShadowSpritePath, TableCollection);
                tk2dSprite shadowSprite = shadowObject.GetComponent<tk2dSprite>();
                shadowSprite.SetSprite(TableCollection, shadowID);
                table.shadowSprite = shadowSprite;
                shadowObject.transform.parent = gameObject.transform;
            }
            bool DisableLeftAndRight = flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_UP_DOWN | flipStyle == FlippableCover.FlipStyle.NO_FLIPS;
            bool DisableUpAndDown = flipStyle == FlippableCover.FlipStyle.ONLY_FLIPS_LEFT_RIGHT | flipStyle == FlippableCover.FlipStyle.NO_FLIPS;
            if (DisableUpAndDown != true)
            {
                tk2dSpriteAnimationClip flipUp = AddTableAnimation(animator, TableCollection, northFlipPaths, name + "_flip_north", FlipFPS, new Vector3(FlippedColliderSizeX_Horizontal, FlippedColliderSizeY_Horizontal + colliderSize.y), new Vector3(colliderOffset.x, (ColliderOffsetY+colliderSize.y)- FlippedColliderSizeY_Horizontal));
                tk2dSpriteAnimationClip flipDown = AddTableAnimation(animator, TableCollection, southFlipPaths, name + "_flip_south", FlipFPS, new Vector3(FlippedColliderSizeX_Horizontal, FlippedColliderSizeY_Horizontal), new Vector3(colliderOffset.x, colliderOffset.y));
                tk2dSpriteAnimationClip breakUp = AddTableAnimation(animator, TableCollection, northBreakPaths, name + "_break_north", BreakFPS, new Vector3(colliderSize.x, colliderSize.y), new Vector3(colliderOffset.x, colliderOffset.y));
                tk2dSpriteAnimationClip breakDown = AddTableAnimation(animator, TableCollection, southBreakPaths, name + "_break_south", BreakFPS, new Vector3(colliderSize.x, colliderSize.y), new Vector3(colliderOffset.x, colliderOffset.y));
                clips.Add(flipUp);
                clips.Add(flipDown);
                clips.Add(breakUp);
                clips.Add(breakDown);
            }
            if (DisableLeftAndRight != true)
            {
                tk2dSpriteAnimationClip flipLeft = AddTableAnimation(animator, TableCollection, westFlipPaths, name + "_flip_east", FlipFPS, new Vector3(FlippedColliderSizeX_Vertical*2, FlippedColliderSizeY_Vertical), new Vector3((ColliderOffsetX+colliderSize.x), colliderOffset.y));
                tk2dSpriteAnimationClip flipRight = AddTableAnimation(animator, TableCollection, eastFlipPaths, name + "_flip_west", FlipFPS, new Vector3(FlippedColliderSizeX_Vertical, FlippedColliderSizeY_Vertical), new Vector3(colliderOffset.x, colliderOffset.y));//FlippedColliderSizeX_Vertical
                tk2dSpriteAnimationClip breakLeft = AddTableAnimation(animator, TableCollection, westBreakPaths, name + "_break_east", BreakFPS, new Vector3(colliderSize.x, colliderSize.y), new Vector3(colliderOffset.x, colliderOffset.y));
                tk2dSpriteAnimationClip breakRight = AddTableAnimation(animator, TableCollection, eastBreakPaths, name + "_break_west", BreakFPS, new Vector3(colliderSize.x, colliderSize.y), new Vector3(colliderOffset.x, colliderOffset.y));
                clips.Add(flipLeft);
                clips.Add(flipRight);
                clips.Add(breakLeft);
                clips.Add(breakRight);
            }
            tk2dSpriteAnimationClip breakUnflipped = AddTableAnimation(animator, TableCollection, unflippedBreakPaths, name + "_break_unflipped", UnflippedBreakFPS, new Vector3(colliderSize.x, colliderSize.y), new Vector3(colliderOffset.x, colliderOffset.y));
            clips.Add(breakUnflipped);

            table.flipAnimation = name + "_flip_{0}";
            table.breakAnimation = name + "_break_{0}";
            table.unflippedBreakAnimation = name + "_break_unflipped";

            animator.Library.clips = clips.ToArray();
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("idle");
        
            SpeculativeRigidbody speculativeRigidbody = sprite.SetUpEmptySpeculativeRigidbody(colliderOffset, colliderSize);
            speculativeRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Tk2dPolygon,
                CollisionLayer = CollisionLayer.LowObstacle,

                IsTrigger = false,
                Enabled = true,
                BagleUseFirstFrameOnly = true,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualDiameter = 0,
            });
            speculativeRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Tk2dPolygon,
                CollisionLayer = CollisionLayer.BulletBlocker,
                IsTrigger = false,
                Enabled = false,
                BagleUseFirstFrameOnly = true,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualDiameter = 0,
            });
            table.sprite = sprite;
            table.spriteAnimator = animator;
            table.majorBreakable.specRigidbody = speculativeRigidbody;
            animator.transform.position = table.transform.position;
            animator.transform.parent = table.transform;
            table.majorBreakable.destroyedOnBreak = false;


            if (breakAnimPercentagesAndSpritePaths != null)
            {
                List<BreakFrame> breakFrameList = new List<BreakFrame>();
                foreach (var Entry in breakAnimPercentagesAndSpritePaths)
                {
                    BreakFrame breakFrame = new BreakFrame();
                    breakFrame.healthPercentage = Entry.Key;
                    int SpriteID = SpriteBuilder.AddSpriteToCollection(Entry.Value, TableCollection);
                    breakFrame.sprite = TableCollection.spriteDefinitions[SpriteID].name;
                    breakFrameList.Add(breakFrame);
                }
                BreakFrame[] array = breakFrameList.ToArray();
                table.prebreakFrames = array;
            }
            if (unflippedBreakAnimPercentagesAndSpritePaths != null)
            {
                List<BreakFrame> breakFrameList = new List<BreakFrame>();
                foreach (var Entry in unflippedBreakAnimPercentagesAndSpritePaths)
                {
                    BreakFrame breakFrame = new BreakFrame();
                    breakFrame.healthPercentage = Entry.Key;
                    int SpriteID = SpriteBuilder.AddSpriteToCollection(Entry.Value, TableCollection);
                    breakFrame.sprite = TableCollection.spriteDefinitions[SpriteID].name;
                    breakFrameList.Add(breakFrame);
                }
                BreakFrame[] array = breakFrameList.ToArray();
                table.prebreakFramesUnflipped = array;
            }

            //Frame stuff
            /*
            table.pitfallAnimation = 
            */


            //table.flipSubElements FUCKING USELESS

            return table;
        }

        private static GameObject GenerateTableOutlineObject(string name, string outlinePath, GameObject parent, tk2dSpriteCollectionData collection)
        {
            GameObject gameObject = SpriteBuilder.SpriteFromResource(outlinePath, null);
            FakePrefab.MarkAsFakePrefab(gameObject);
            gameObject.name = name + "_Outline";
            gameObject.layer = 0;
            gameObject.GetComponent<tk2dSprite>().HeightOffGround = 0.1f;
            SpriteBuilder.AddSpriteToCollection(outlinePath, collection);

            gameObject.transform.parent = parent.transform;
            return gameObject;
        }
        public static SpeculativeRigidbody SetUpEmptySpeculativeRigidbody(this tk2dSprite sprite, IntVector2 offset, IntVector2 dimensions)
        {
            var body = sprite.gameObject.GetOrAddComponent<SpeculativeRigidbody>();
            body.PixelColliders = new List<PixelCollider>() { };
            return body;
        }
        public static tk2dSpriteAnimationClip AddTableAnimation(tk2dSpriteAnimator animator, tk2dSpriteCollectionData Tablecollection, string[] spritePaths, string clipName, int FPS, Vector3 colliderSize, Vector3 colliderOffset)
        {
            tk2dSpriteAnimation animation = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            tk2dSpriteAnimationClip idleClip = new tk2dSpriteAnimationClip() { name = clipName, frames = new tk2dSpriteAnimationFrame[0], fps = FPS };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < spritePaths.Length; i++)
            {
                tk2dSpriteCollectionData collection = Tablecollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(spritePaths[i], collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerLeft);
                
                if (clipName.Contains("break"))
                {
                    tk2dSpriteDefinition frameDefMod = GenerateNoColliderForSpriteDefinition(frameDef);
                    frameDef = frameDefMod;
                }
                else
                {
                    tk2dSpriteDefinition frameDefMod = GenerateColliderForSpriteDefinition(frameDef, colliderSize, colliderOffset);
                    frameDef = frameDefMod;
                }
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
            }
            idleClip.frames = frames.ToArray();
            idleClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            return idleClip;
        }
        private static tk2dSpriteDefinition GenerateColliderForSpriteDefinition(tk2dSpriteDefinition frameDef, Vector3 colliderSize, Vector3 colliderOffset)
        {
            frameDef.colliderVertices = new Vector3[] { new Vector3(colliderSize.x / 32, colliderSize.y / 32), new Vector3(colliderSize.x / 32, colliderSize.y / 32) };
            frameDef.collisionLayer = CollisionLayer.HighObstacle;
            frameDef.colliderConvex = false;
            frameDef.colliderType = tk2dSpriteDefinition.ColliderType.Box;
            frameDef.colliderSmoothSphereCollisions = false;
            frameDef.complexGeometry = false;
            frameDef.flipped = tk2dSpriteDefinition.FlipMode.Tk2d;            
            if (colliderOffset != null)
            {
                AddOffset(frameDef, new Vector2(colliderOffset.x/32, colliderOffset.y/32));         
            }
            
            return frameDef;
        }
        private static tk2dSpriteDefinition GenerateNoColliderForSpriteDefinition(tk2dSpriteDefinition frameDef)
        {
            frameDef.colliderVertices = new Vector3[] { new Vector3(0,0)};
            frameDef.collisionLayer = CollisionLayer.HighObstacle;
            frameDef.colliderConvex = false;
            frameDef.colliderType = tk2dSpriteDefinition.ColliderType.None;
            frameDef.colliderSmoothSphereCollisions = false;
            frameDef.complexGeometry = false;
            frameDef.flipped = tk2dSpriteDefinition.FlipMode.Tk2d;
            return frameDef;
        }


        public static void AddOffset(this tk2dSpriteDefinition def, Vector2 offset)
        {
            float xOffset = offset.x;
            float yOffset = offset.y;
            if (def.colliderVertices != null && def.colliderVertices.Length > 0)
            {
                def.colliderVertices[0] += new Vector3(xOffset, yOffset, 0);
               
            }
        }


      
        public static DirectionalAnimation AddDirectionAnimation(tk2dSpriteAnimator animator, string Prefix, string[] animationNames, DirectionalAnimation.FlipType[] flipType, DirectionalAnimation.DirectionType directionType = DirectionalAnimation.DirectionType.Single)
        {
            DirectionalAnimation newDirectionalAnimation = new DirectionalAnimation
            {
                Type = directionType,
                Prefix = Prefix,
                AnimNames = animationNames,
                Flipped = flipType
            };
            return newDirectionalAnimation;
        }
        /// <summary>
        /// Generates, and returns a MajorBreakable. This is for generating a basic one, it returns it so you can additionally modify it without cluttering up the setup method too much.
        /// </summary>
        /// <param name="name">The name of your breakable. Not very useful, but I figured it would be important to set it.</param>
        /// <param name="idleSpritePaths">Your sprite paths. Only insert one path if you don't want it to be animated.</param>
        /// <param name="idleAnimFPS">The FPS of your breakables idle animation.</param>
        /// <param name="breakSpritePaths">Your sprite paths for the break animation. You can set this to null if you dont want a break animation.</param>
        /// <param name="breakAnimFPS">The FPS of your breakables breaking animation.</param>
        /// <param name="HP">The amount of damage your MajorBreakable can sustain before breaking.</param>
        /// <param name="ShadowSpritePath">The spritepath of your shadow. Leave this null to not have a shadow.</param>
        /// <param name="ShadowOffsetX">The X value of the shadows offset. Note that 1 here means 1 *tile*, and not 1 pixel!</param>
        /// <param name="ShadowOffsetY">The Y value of the shadows offset. Note that 1 here means 1 *tile*, and not 1 pixel!</param>
        /// <param name="UsesCustomColliderValues">Setting this to true will let you use custom collider sizes and offsets. Keeping it false will use no offsets and generate a size based on the sprites size.</param>
        /// <param name="ColliderSizeX">The X Value of your collider. Only used if UsesCustomColliderValues is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="ColliderSizeY">The Y Value of your collider. Only used if UsesCustomColliderValues is true. Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="ColliderOffsetX">The X Value of your colliders offset. Only used if UsesCustomColliderValues is true.</param>
        /// <param name="ColliderOffsetY">The Y Value of your colliders offset. Only used if UsesCustomColliderValues is true.</param>
        /// <param name="DistribleShards">When shards spawn, if set to true, will spawn the shards at random positions inside the bounds of your breakables sprite.</param>
        /// <param name="breakVFX">The VFX that plays when the breakable is broken. Keep this as null to not have any VFX there.</param>
        /// <param name="damagedVFX">The VFX that plays when the breakable is damaged. Keep this as null to not have any VFX there.</param>
        /// <param name="BlocksPaths">Will act as a blocker and will not let enemies path find through it, I think.</param>
        public static MajorBreakable GenerateMajorBreakable(string name, string[] idleSpritePaths, int idleAnimFPS = 2, string[] breakSpritePaths = null, int breakAnimFPS = 5, float HP = 100, string ShadowSpritePath = null, float ShadowOffsetX = 0, float ShadowOffsetY = 0, bool UsesCustomColliderValues = false, int ColliderSizeX = 16, int ColliderSizeY = 8, int ColliderOffsetX = 0, int ColliderOffsetY = 8, bool DistribleShards = true, VFXPool breakVFX = null, VFXPool damagedVFX = null, bool BlocksPaths = false)
        {
            Texture2D textureFromResource = GungeonAPI.ResourceExtractor.GetTextureFromResource(idleSpritePaths[0]);
            GameObject gameObject = SpriteBuilder.SpriteFromResource(idleSpritePaths[0], null);
            FakePrefab.MarkAsFakePrefab(gameObject);
            gameObject.name = name;
            MajorBreakable breakable = gameObject.AddComponent<MajorBreakable>();

            tk2dSpriteCollectionData MajorBreakableSpriteCollection = SpriteBuilder.ConstructCollection(gameObject, (name + "_Collection"));
            int spriteID = SpriteBuilder.AddSpriteToCollection(idleSpritePaths[0], MajorBreakableSpriteCollection);
            tk2dSprite sprite = gameObject.GetOrAddComponent<tk2dSprite>();
            sprite.SetSprite(MajorBreakableSpriteCollection, spriteID);

            IntVector2 intVector = new IntVector2(ColliderSizeX, ColliderSizeY);
            IntVector2 colliderOffset = new IntVector2(ColliderOffsetX, ColliderOffsetY);
            IntVector2 colliderSize = new IntVector2(intVector.x, intVector.y);

            if (UsesCustomColliderValues == false)
            {
                IntVector2 nonCustomintVector = new IntVector2(textureFromResource.width, textureFromResource.height);
                colliderSize = new IntVector2(nonCustomintVector.x, nonCustomintVector.y);
            }

            SpeculativeRigidbody speculativeRigidbody = sprite.SetUpSpeculativeRigidbody(colliderOffset, colliderSize);
            speculativeRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.BulletBlocker,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualWidth = colliderSize.x,
                ManualHeight = colliderSize.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0,
            });
            speculativeRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.EnemyBulletBlocker,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualWidth = colliderSize.x,
                ManualHeight = colliderSize.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0,
            });
            speculativeRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.BeamBlocker,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualWidth = colliderSize.x,
                ManualHeight = colliderSize.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0,
            });


            tk2dSpriteAnimator animator = gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = gameObject.AddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            tk2dSpriteAnimationClip idleClip = new tk2dSpriteAnimationClip() { name = "idle", frames = new tk2dSpriteAnimationFrame[0], fps = idleAnimFPS };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < idleSpritePaths.Length; i++)
            {
                tk2dSpriteCollectionData collection = MajorBreakableSpriteCollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(idleSpritePaths[i], collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
                frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerLeft);
            }
            idleClip.frames = frames.ToArray();
            idleClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            animator.Library.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { idleClip }).ToArray();
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("idle");

            if (breakSpritePaths != null)
            {
                tk2dSpriteAnimation breakAnimation = gameObject.AddComponent<tk2dSpriteAnimation>();
                breakAnimation.clips = new tk2dSpriteAnimationClip[0];
                tk2dSpriteAnimationClip breakClip = new tk2dSpriteAnimationClip() { name = "break", frames = new tk2dSpriteAnimationFrame[0], fps = breakAnimFPS };
                List<tk2dSpriteAnimationFrame> breakFrames = new List<tk2dSpriteAnimationFrame>();
                for (int i = 0; i < breakSpritePaths.Length; i++)
                {
                    tk2dSpriteCollectionData collection = MajorBreakableSpriteCollection;
                    int frameSpriteId = SpriteBuilder.AddSpriteToCollection(breakSpritePaths[i], collection);
                    tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                    frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerLeft);
                    breakFrames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
                }
                breakClip.frames = breakFrames.ToArray();
                breakClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
                animator.Library.clips = breakAnimation.clips.Concat(new tk2dSpriteAnimationClip[] { idleClip, breakClip }).ToArray();
                animator.playAutomatically = true;
                animator.DefaultClipId = animator.GetClipIdByName("idle");
                breakable.breakAnimation = "break";
            }
            breakable.sprite = sprite;
            breakable.sprite.transform.position = breakable.sprite.transform.position;
            breakable.specRigidbody = speculativeRigidbody;
            breakable.spriteAnimator = animator;
            breakable.HitPoints = HP;
            breakable.HandlePathBlocking = BlocksPaths;
            if (ShadowSpritePath != null)
            {
                GameObject shadowObject = SpriteBuilder.SpriteFromResource(ShadowSpritePath, null);
                shadowObject.name = "Shadow_" + name;
                int newSpriteId2 = SpriteBuilder.AddSpriteToCollection(ShadowSpritePath, MajorBreakableSpriteCollection);
                tk2dSprite orAddComponent3 = shadowObject.GetOrAddComponent<tk2dSprite>();
                orAddComponent3.SetSprite(MajorBreakableSpriteCollection, newSpriteId2);
                FakePrefab.MarkAsFakePrefab(shadowObject);
                ShadowHandler orAddComponent4 = breakable.gameObject.GetOrAddComponent<ShadowHandler>();
                orAddComponent4.shadowObject = orAddComponent3.gameObject;
                orAddComponent4.Offset = new Vector3(ShadowOffsetX, ShadowOffsetY);
            }
            if (breakVFX != null) { breakable.breakVfx = breakVFX; }
            if (damagedVFX != null) { breakable.damageVfx = damagedVFX; }

            breakable.distributeShards = DistribleShards;
            return breakable;
        }
        /// <summary>
        /// Generates, and returns a MinorBreakable. This is for generating a basic one, it returns it so you can additionally modify it without cluttering up the setup method too much.
        /// </summary>
        /// <param name="name">The name of your breakable. Not very useful, but I figured it would be important to set it.</param>
        /// <param name="idleSpritePaths">Your sprite paths. Only insert one path if you don't want it to be animated.</param>
        /// <param name="idleAnimFPS">The FPS of your breakables idle animation.</param>
        /// <param name="breakSpritePaths">Your sprite paths for the break animation. You can set this to null if you dont want a break animation.</param>
        /// <param name="breakAnimFPS">The FPS of your breakables breaking animation.</param>
        /// <param name="breakAudioEvent">The sound that plays when your breakable is broken. You can set it to null for it to not play a sound.</param>
        /// <param name="ShadowSpritePath">The spritepath of your shadow. Leave this null to not have a shadow.</param>
        /// <param name="ShadowOffsetX">The X value of the shadows offset.  Note that 1 here means 1 *tile*, and not 1 pixel!</param>
        /// <param name="ShadowOffsetY">The Y value of the shadows offset.  Note that 1 here means 1 *tile*, and not 1 pixel!</param>
        /// <param name="UsesCustomColliderValues">Setting this to true will let you use custom collider sizes and offsets. Keeping it false will use no offsets and generate a size based on the sprites size.</param>
        /// <param name="ColliderSizeX">The X Value of your collider. Only used if UsesCustomColliderValues is true.  Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="ColliderSizeY">The Y Value of your collider. Only used if UsesCustomColliderValues is true.  Note that 1 here means 1 *pixel*, and not 1 tile!</param>
        /// <param name="ColliderOffsetX">The X Value of your colliders offset. Only used if UsesCustomColliderValues is true.</param>
        /// <param name="ColliderOffsetY">The Y Value of your colliders offset. Only used if UsesCustomColliderValues is true.</param>
        /// <param name="DestroyVFX">The VFX that plays when your breakable is destroyed.</param>
        public static MinorBreakable GenerateMinorBreakable(string name, string[] idleSpritePaths, int idleAnimFPS = 1, string[] breakSpritePaths = null, int breakAnimFPS = 5, string breakAudioEvent = "Play_OBJ_pot_shatter_01", string ShadowSpritePath = null, float ShadowOffsetX = 0, float ShadowOffsetY = -1, bool UsesCustomColliderValues = false, int ColliderSizeX = 16, int ColliderSizeY = 8, int ColliderOffsetX = 0, int ColliderOffsetY = 8, GameObject DestroyVFX = null)
        {
            Texture2D textureFromResource = GungeonAPI.ResourceExtractor.GetTextureFromResource(idleSpritePaths[0]);
            GameObject gameObject = SpriteBuilder.SpriteFromResource(idleSpritePaths[0], null);
            FakePrefab.MarkAsFakePrefab(gameObject);
            gameObject.name = name;
            MinorBreakable breakable = gameObject.AddComponent<MinorBreakable>();

            tk2dSpriteCollectionData MinorBreakableSpriteCollection = SpriteBuilder.ConstructCollection(gameObject, (name + "_Collection"));
            int spriteID = SpriteBuilder.AddSpriteToCollection(idleSpritePaths[0], MinorBreakableSpriteCollection);
            tk2dSprite sprite = gameObject.GetOrAddComponent<tk2dSprite>();
            sprite.SetSprite(MinorBreakableSpriteCollection, spriteID);

            IntVector2 intVector = new IntVector2(ColliderSizeX, ColliderSizeY);
            IntVector2 colliderOffset = new IntVector2(ColliderOffsetX, ColliderOffsetY);
            IntVector2 colliderSize = new IntVector2(intVector.x, intVector.y);

            if (UsesCustomColliderValues == false)
            {
                IntVector2 nonCustomintVector = new IntVector2(textureFromResource.width, textureFromResource.height);
                colliderSize = new IntVector2(nonCustomintVector.x, nonCustomintVector.y);
            }

            SpeculativeRigidbody speculativeRigidbody = sprite.SetUpSpeculativeRigidbody(colliderOffset, colliderSize);
            speculativeRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.BulletBlocker,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualWidth = colliderSize.x,
                ManualHeight = colliderSize.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0,
            });
            speculativeRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.EnemyBulletBlocker,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualWidth = colliderSize.x,
                ManualHeight = colliderSize.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0,
            });
            speculativeRigidbody.PixelColliders.Add(new PixelCollider
            {
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.BeamBlocker,
                IsTrigger = false,
                BagleUseFirstFrameOnly = false,
                SpecifyBagelFrame = string.Empty,
                BagelColliderNumber = 0,
                ManualOffsetX = colliderOffset.x,
                ManualOffsetY = colliderOffset.y,
                ManualWidth = colliderSize.x,
                ManualHeight = colliderSize.y,
                ManualDiameter = 0,
                ManualLeftX = 0,
                ManualLeftY = 0,
                ManualRightX = 0,
                ManualRightY = 0,
            });

            tk2dSpriteAnimator animator = gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = gameObject.AddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            tk2dSpriteAnimationClip idleClip = new tk2dSpriteAnimationClip() { name = "idle", frames = new tk2dSpriteAnimationFrame[0], fps = idleAnimFPS };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < idleSpritePaths.Length; i++)
            {
                tk2dSpriteCollectionData collection = MinorBreakableSpriteCollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(idleSpritePaths[i], collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerLeft);
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
            }
            idleClip.frames = frames.ToArray();
            idleClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
            animator.Library.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { idleClip }).ToArray();
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("idle");

            if (breakSpritePaths != null)
            {
                tk2dSpriteAnimation breakAnimation = gameObject.AddComponent<tk2dSpriteAnimation>();
                breakAnimation.clips = new tk2dSpriteAnimationClip[0];
                tk2dSpriteAnimationClip breakClip = new tk2dSpriteAnimationClip() { name = "break", frames = new tk2dSpriteAnimationFrame[0], fps = breakAnimFPS };
                List<tk2dSpriteAnimationFrame> breakFrames = new List<tk2dSpriteAnimationFrame>();
                for (int i = 0; i < breakSpritePaths.Length; i++)
                {
                    tk2dSpriteCollectionData collection = MinorBreakableSpriteCollection;
                    int frameSpriteId = SpriteBuilder.AddSpriteToCollection(breakSpritePaths[i], collection);
                    tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                    frameDef.ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerLeft);
                    breakFrames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
                }
                breakClip.frames = breakFrames.ToArray();
                breakClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
                animator.Library.clips = breakAnimation.clips.Concat(new tk2dSpriteAnimationClip[] { idleClip, breakClip }).ToArray();
                animator.playAutomatically = true;
                animator.DefaultClipId = animator.GetClipIdByName("idle");
                breakable.breakAnimName = "break";
            }
            breakable.sprite = sprite;
            breakable.specRigidbody = speculativeRigidbody;
            breakable.spriteAnimator = animator;
            breakable.breakAudioEventName = breakAudioEvent;

            if (ShadowSpritePath != null)
            {
                GameObject ShadowObject = SpriteBuilder.SpriteFromResource(ShadowSpritePath, null);
                FakePrefab.MarkAsFakePrefab(ShadowObject);
                ShadowObject.name = "Shadow_" + name;
                int shadowSpriteID = SpriteBuilder.AddSpriteToCollection(ShadowSpritePath, MinorBreakableSpriteCollection);
                tk2dSprite orAddComponent3 = ShadowObject.GetOrAddComponent<tk2dSprite>();
                orAddComponent3.SetSprite(MinorBreakableSpriteCollection, shadowSpriteID);
                ShadowHandler orAddComponent4 = breakable.gameObject.GetOrAddComponent<ShadowHandler>();
                orAddComponent4.shadowObject = orAddComponent3.gameObject;
                orAddComponent4.Offset = new Vector3(ShadowOffsetX, ShadowOffsetY);
            }

            if (DestroyVFX != null) { breakable.AdditionalVFXObject = DestroyVFX; }
            return breakable;
        }
        public static void OnBroken()
        {

        }
        /// <summary>
        /// Generates, and returns a DebrisObject that you can add to a ShardCluster, which in turn can be used by your breakable  
        /// </summary>
        /// <param name="debrisObjectsCanRotate">Enables/Disables whether your shards can rotate in-flight.</param>
        /// <param name="LifeSpanMin">The minimum flight-time of your shards.</param>
        /// <param name="LifeSpanMax">The maximum flight-time of your shards.</param>
        /// <param name="AngularVelocity">How much your shards will rotate in-flight.</param>
        /// <param name="AngularVelocityVariance">Adds/removes some angular velocity to your shards when created. For example, having 40 AngularVelocity and an AngularVelocityVariance of 12 will set the AngularVelocity of your shards from anywhere between 28 and 52.</param>
        /// <param name="shadowSprite">The sprite of the shadow your DebrisObject will use. Leave this as null to not have a shadow.</param>
        /// <param name="Mass">Default of 1. The amount of additional weight applied to your DebrisObject</param>
        /// <param name="AudioEventName">The sound that will play when the shards contact the ground.</param>
        /// <param name="BounceVFX">The VFX that plays when your shards bounce.</param>
        /// <param name="DebrisBounceCount">The amount of times your shards will bounce.</param>
        /// <param name="DoesGoopOnRest">If true, will spawn goop on itself when it is in a resting state.</param>
        /// <param name="GoopType">The goop it will spawn if DoesGoopOnRest is true.</param>
        /// <param name="GoopRadius">The radius of the spawned goop.</param>

        public static DebrisObject GenerateDebrisObject(string shardSpritePath, bool debrisObjectsCanRotate = true, float LifeSpanMin = 0.33f, float LifeSpanMax = 2f, float AngularVelocity = 540, float AngularVelocityVariance = 180f, tk2dSprite shadowSprite = null, float Mass = 1, string AudioEventName = null, GameObject BounceVFX = null, int DebrisBounceCount = 0, bool DoesGoopOnRest = false, GoopDefinition GoopType = null, float GoopRadius = 1f)
        {
            GameObject debrisObject = SpriteBuilder.SpriteFromResource(shardSpritePath, null);
            FakePrefab.MarkAsFakePrefab(debrisObject);
            tk2dSprite tk2dsprite = debrisObject.GetComponent<tk2dSprite>();
            DebrisObject DebrisObj = debrisObject.AddComponent<DebrisObject>();
            DebrisObj.canRotate = debrisObjectsCanRotate;
            DebrisObj.lifespanMin = LifeSpanMin;
            DebrisObj.lifespanMax = LifeSpanMax;
            DebrisObj.bounceCount = DebrisBounceCount;
            DebrisObj.angularVelocity = AngularVelocity;
            DebrisObj.angularVelocityVariance = AngularVelocityVariance;
            if (AudioEventName != null) { DebrisObj.audioEventName = AudioEventName; }
            if (BounceVFX != null) { DebrisObj.optionalBounceVFX = BounceVFX; }
            DebrisObj.sprite = tk2dsprite;
            DebrisObj.DoesGoopOnRest = DoesGoopOnRest;
            if (GoopType != null) { DebrisObj.AssignedGoop = GoopType; }
            DebrisObj.GoopRadius = GoopRadius;
            if (shadowSprite != null) { DebrisObj.shadowSprite = shadowSprite; }
            DebrisObj.inertialMass = Mass;
            return DebrisObj;
        }
        /// <summary>
        /// Generates, and returns an animated DebrisObject that you can add to a ShardCluster, which in turn can be used by your breakable  
        /// </summary>
        /// <param name="FPS">The FPS of your DebrisObject.</param>
        /// <param name="wrapMode">The wrap mode of the animated DebrisObject.</param>
        /// <param name="debrisObjectsCanRotate">Enables/Disables whether your shards can rotate in-flight.</param>
        /// <param name="LifeSpanMin">The minimum flight-time of your shards.</param>
        /// <param name="LifeSpanMax">The maximum flight-time of your shards.</param>
        /// <param name="AngularVelocity">How much your shards will rotate in-flight.</param>
        /// <param name="AngularVelocityVariance">Adds/removes some angular velocity to your shards when created. For example, having 40 AngularVelocity and an AngularVelocityVariance of 12 will set the AngularVelocity of your shards from anywhere between 28 and 52.</param>
        /// <param name="shadowSprite">The sprite of the shadow your DebrisObject will use. Leave this as null to not have a shadow.</param>
        /// <param name="Mass">Default of 1. The amount of additional weight applied to your DebrisObject</param>
        /// <param name="AudioEventName">The sound that will play when the shards hit the ground.</param>
        /// <param name="BounceVFX">The VFX that plays when your shards bounce.</param>
        /// <param name="DebrisBounceCount">The amount of times your shards will bounce.</param>
        /// <param name="DoesGoopOnRest">If true, will spawn goop on itself when it is in a resting state.</param>
        /// <param name="GoopType">The goop it will spawn if DoesGoopOnRest is true.</param>
        /// <param name="GoopRadius">The radius of the spawned goop.</param>
        public static DebrisObject GenerateAnimatedDebrisObject(string[] shardSpritePaths, int FPS = 12, tk2dSpriteAnimationClip.WrapMode wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop, bool debrisObjectsCanRotate = true, float LifeSpanMin = 0.33f, float LifeSpanMax = 2f, float AngularVelocity = 540, float AngularVelocityVariance = 180f, tk2dSprite shadowSprite = null, float Mass = 1, string AudioEventName = null, GameObject BounceVFX = null, int DebrisBounceCount = 0, bool DoesGoopOnRest = false, GoopDefinition GoopType = null, float GoopRadius = 1f)
        {
            GameObject debrisObject = SpriteBuilder.SpriteFromResource(shardSpritePaths[0], null);
            FakePrefab.MarkAsFakePrefab(debrisObject);
            DebrisObject DebrisObj = debrisObject.AddComponent<DebrisObject>();

            tk2dSpriteCollectionData VFXSpriteCollection = SpriteBuilder.ConstructCollection(debrisObject, (shardSpritePaths[0] + "_Collection"));
            int spriteID = SpriteBuilder.AddSpriteToCollection(shardSpritePaths[0], VFXSpriteCollection);
            tk2dSprite sprite = debrisObject.GetOrAddComponent<tk2dSprite>();
            sprite.SetSprite(VFXSpriteCollection, spriteID);

            tk2dSpriteAnimator animator = debrisObject.GetOrAddComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimation animation = debrisObject.AddComponent<tk2dSpriteAnimation>();
            animation.clips = new tk2dSpriteAnimationClip[0];
            animator.Library = animation;
            tk2dSpriteAnimationClip idleClip = new tk2dSpriteAnimationClip() { name = "idle", frames = new tk2dSpriteAnimationFrame[0], fps = FPS };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < shardSpritePaths.Length; i++)
            {
                tk2dSpriteCollectionData collection = VFXSpriteCollection;
                int frameSpriteId = SpriteBuilder.AddSpriteToCollection(shardSpritePaths[i], collection);
                tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
            }
            idleClip.frames = frames.ToArray();
            idleClip.wrapMode = wrapMode;
            animator.Library.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { idleClip }).ToArray();
            animator.playAutomatically = true;
            animator.DefaultClipId = animator.GetClipIdByName("idle");
            DebrisObj.canRotate = debrisObjectsCanRotate;
            DebrisObj.lifespanMin = LifeSpanMin;
            DebrisObj.lifespanMax = LifeSpanMax;
            DebrisObj.bounceCount = DebrisBounceCount;
            DebrisObj.angularVelocity = AngularVelocity;
            DebrisObj.angularVelocityVariance = AngularVelocityVariance;
            if (AudioEventName != null) { DebrisObj.audioEventName = AudioEventName; }
            if (BounceVFX != null) { DebrisObj.optionalBounceVFX = BounceVFX; }
            DebrisObj.sprite = sprite;
            DebrisObj.DoesGoopOnRest = DoesGoopOnRest;
            if (GoopType != null) { DebrisObj.AssignedGoop = GoopType; }
            DebrisObj.GoopRadius = GoopRadius;
            if (shadowSprite != null) { DebrisObj.shadowSprite = shadowSprite; }
            DebrisObj.inertialMass = Mass;

            return DebrisObj;
        }
        /// <summary>
        /// Generates, and returns an array of DebrisObjects that you can add to a ShardCluster, which in turn can be used by your breakable. note that each Debris Object generated here will all use the same values you gave it
        /// </summary>
        /// <param name="debrisObjectsCanRotate">Enables/Disables whether your shards can rotate in-flight.</param>
        /// <param name="LifeSpanMin">The minimum flight-time of your shards.</param>
        /// <param name="LifeSpanMax">The maximum flight-time of your shards.</param>
        /// <param name="AngularVelocity">How much your shards will rotate in-flight.</param>
        /// <param name="AngularVelocityVariance">Adds/removes some angular velocity to your shards when created. For example, having 40 AngularVelocity and an AngularVelocityVariance of 12 will set the AngularVelocity of your shards from anywhere between 28 and 52.</param>
        /// <param name="shadowSprite">The sprite of the shadow your DebrisObject will use. Leave this as null to not have a shadow.</param>
        /// <param name="Mass">Default of 1. The amount of additional weight applied to your DebrisObject</param>
        /// <param name="AudioEventName">The sound that will play when the shards hit the ground.</param>
        /// <param name="BounceVFX">The VFX that plays when your shards bounce.</param>
        /// <param name="DebrisBounceCount">The amount of times your shards will bounce.</param>
        /// <param name="DoesGoopOnRest">If true, will spawn goop on itself when it is in a resting state.</param>
        /// <param name="GoopType">The goop it will spawn if DoesGoopOnRest is true.</param>
        /// <param name="GoopRadius">The radius of the spawned goop.</param>
        public static DebrisObject[] GenerateDebrisObjects(string[] shardSpritePaths, bool debrisObjectsCanRotate = true, float LifeSpanMin = 0.33f, float LifeSpanMax = 2f, float AngularVelocity = 540, float AngularVelocityVariance = 180f, tk2dSprite shadowSprite = null, float Mass = 1, string AudioEventName = null, GameObject BounceVFX = null, int DebrisBounceCount = 0, bool DoesGoopOnRest = false, GoopDefinition GoopType = null, float GoopRadius = 1f)
        {
            List<DebrisObject> DebrisObjectList = new List<DebrisObject>();
            for (int i = 0; i < shardSpritePaths.Length; i++)
            {
                GameObject debrisObject = SpriteBuilder.SpriteFromResource(shardSpritePaths[i], null);
                FakePrefab.MarkAsFakePrefab(debrisObject);
                tk2dSprite tk2dsprite = debrisObject.GetComponent<tk2dSprite>();
                DebrisObject DebrisObj = debrisObject.AddComponent<DebrisObject>();
                DebrisObj.canRotate = debrisObjectsCanRotate;
                DebrisObj.lifespanMin = LifeSpanMin;
                DebrisObj.lifespanMax = LifeSpanMax;
                DebrisObj.bounceCount = DebrisBounceCount;
                DebrisObj.angularVelocity = AngularVelocity;
                DebrisObj.angularVelocityVariance = AngularVelocityVariance;
                if (AudioEventName != null) { DebrisObj.audioEventName = AudioEventName; }
                if (BounceVFX != null) { DebrisObj.optionalBounceVFX = BounceVFX; }
                DebrisObj.sprite = tk2dsprite;
                DebrisObj.DoesGoopOnRest = DoesGoopOnRest;
                if (GoopType != null) { DebrisObj.AssignedGoop = GoopType; }
                DebrisObj.GoopRadius = GoopRadius;
                if (shadowSprite != null) { DebrisObj.shadowSprite = shadowSprite; }
                DebrisObj.inertialMass = Mass;
                DebrisObjectList.Add(DebrisObj);
            }
            DebrisObject[] DebrisArray = DebrisObjectList.ToArray();
            return DebrisArray;
        }

        /// <summary>
        /// Generates, and returns an array of animated DebrisObjects that you can add to a ShardCluster, which in turn can be used by your breakable. note that each Debris Object generated here will all use the same values you gave it
        /// </summary>
        /// <param name="FPS">The FPS of your DebrisObject.</param>
        /// <param name="wrapMode">The wrap mode of the animated DebrisObject.</param>
        /// <param name="debrisObjectsCanRotate">Enables/Disables whether your shards can rotate in-flight.</param>
        /// <param name="LifeSpanMin">The minimum flight-time of your shards.</param>
        /// <param name="LifeSpanMax">The maximum flight-time of your shards.</param>
        /// <param name="AngularVelocity">How much your shards will rotate in-flight.</param>
        /// <param name="AngularVelocityVariance">Adds/removes some angular velocity to your shards when created. For example, having 40 AngularVelocity and an AngularVelocityVariance of 12 will set the AngularVelocity of your shards from anywhere between 28 and 52.</param>
        /// <param name="shadowSprite">The sprite of the shadow your DebrisObject will use. Leave this as null to not have a shadow.</param>
        /// <param name="Mass">Default of 1. The amount of additional weight applied to your DebrisObject</param>
        /// <param name="AudioEventName">The sound that will play when the shards hit the ground.</param>
        /// <param name="BounceVFX">The VFX that plays when your shards bounce.</param>
        /// <param name="DebrisBounceCount">The amount of times your shards will bounce.</param>
        /// <param name="DoesGoopOnRest">If true, will spawn goop on itself when it is in a resting state.</param>
        /// <param name="GoopType">The goop it will spawn if DoesGoopOnRest is true.</param>
        /// <param name="GoopRadius">The radius of the spawned goop.</param>

        public static DebrisObject[] GenerateAnimatedDebrisObjects(List<string[]> shardSpritePathsList, int FPS = 12, tk2dSpriteAnimationClip.WrapMode wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop, bool debrisObjectsCanRotate = true, tk2dSprite shadowSprite = null, float Mass = 1, float LifeSpanMin = 0.33f, float LifeSpanMax = 2f, float AngularVelocity = 540, float AngularVelocityVariance = 180f, string AudioEventName = null, GameObject BounceVFX = null, int DebrisBounceCount = 0, bool DoesGoopOnRest = false, GoopDefinition GoopType = null, float GoopRadius = 1f)
        {
            List<DebrisObject> DebrisObjectList = new List<DebrisObject>();
            for (int i = 0; i < shardSpritePathsList.Count; i++)
            {
                string[] paths = shardSpritePathsList[i];
                for (int e = 0; e < paths.Length; e++)
                {
                    GameObject debrisObject = SpriteBuilder.SpriteFromResource(paths[0], null);
                    FakePrefab.MarkAsFakePrefab(debrisObject);
                    DebrisObject DebrisObj = debrisObject.AddComponent<DebrisObject>();
                    tk2dSpriteCollectionData VFXSpriteCollection = SpriteBuilder.ConstructCollection(debrisObject, (paths[0] + "_Collection"));
                    int spriteID = SpriteBuilder.AddSpriteToCollection(paths[0], VFXSpriteCollection);
                    tk2dSprite sprite = debrisObject.GetOrAddComponent<tk2dSprite>();
                    sprite.SetSprite(VFXSpriteCollection, spriteID);

                    tk2dSpriteAnimator animator = debrisObject.GetOrAddComponent<tk2dSpriteAnimator>();
                    tk2dSpriteAnimation animation = debrisObject.AddComponent<tk2dSpriteAnimation>();
                    animation.clips = new tk2dSpriteAnimationClip[0];
                    animator.Library = animation;
                    tk2dSpriteAnimationClip idleClip = new tk2dSpriteAnimationClip() { name = "idle", frames = new tk2dSpriteAnimationFrame[0], fps = FPS };
                    List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
                    for (int q = 0; q < paths.Length; q++)
                    {
                        tk2dSpriteCollectionData collection = VFXSpriteCollection;
                        int frameSpriteId = SpriteBuilder.AddSpriteToCollection(paths[q], collection);
                        tk2dSpriteDefinition frameDef = collection.spriteDefinitions[frameSpriteId];
                        frames.Add(new tk2dSpriteAnimationFrame { spriteId = frameSpriteId, spriteCollection = collection });
                    }
                    idleClip.frames = frames.ToArray();
                    idleClip.wrapMode = wrapMode;
                    animator.Library.clips = animation.clips.Concat(new tk2dSpriteAnimationClip[] { idleClip }).ToArray();
                    animator.playAutomatically = true;
                    animator.DefaultClipId = animator.GetClipIdByName("idle");
                    DebrisObj.canRotate = debrisObjectsCanRotate;
                    DebrisObj.lifespanMin = LifeSpanMin;
                    DebrisObj.lifespanMax = LifeSpanMax;
                    DebrisObj.bounceCount = DebrisBounceCount;
                    DebrisObj.angularVelocity = AngularVelocity;
                    DebrisObj.angularVelocityVariance = AngularVelocityVariance;
                    if (AudioEventName != null) { DebrisObj.audioEventName = AudioEventName; }
                    if (BounceVFX != null) { DebrisObj.optionalBounceVFX = BounceVFX; }
                    DebrisObj.sprite = sprite;
                    DebrisObj.DoesGoopOnRest = DoesGoopOnRest;
                    if (GoopType != null) { DebrisObj.AssignedGoop = GoopType; }
                    DebrisObj.GoopRadius = GoopRadius;
                    if (shadowSprite != null) { DebrisObj.shadowSprite = shadowSprite; }
                    DebrisObj.inertialMass = Mass;
                    DebrisObjectList.Add(DebrisObj);
                }
            }
            DebrisObject[] DebrisArray = DebrisObjectList.ToArray();
            return DebrisArray;
        }
        /// <summary>
        /// Generates, and returns a ShardCluster that you can add to your breakable to have it create shards. 
        /// </summary>
        /// <param name="clusterForceMultiplier">The force applied multiplicatively onto the shards when they're created.</param>
        /// <param name="ClusterLaunchStrength">The amount of force applied multiplicatively to your shards when created.</param>
        /// <param name="MinClusterAmount">The minimum amount of shards that the shard cluster will spawn.</param>
        /// <param name="MaxClusterAmount">The maximum amount of shards that the shard cluster will spawn.</param>
        /// <param name="clusterRotationMultiplier">The amount of rotation applied multiplicatively.</param>
        public static ShardCluster GenerateShardCluster(DebrisObject[] debrisObjects, float ClusterLaunchStrength = 0.5f, float clusterForceMultiplier = 2, int MinClusterAmount = 1, int MaxClusterAmount = 5, float clusterRotationMultiplier = 1)
        {
            ShardCluster cluster = new ShardCluster();
            DebrisObject[] DebrisObjectArray = debrisObjects;
            cluster.forceMultiplier = clusterForceMultiplier;
            cluster.rotationMultiplier = clusterRotationMultiplier;
            cluster.minFromCluster = MinClusterAmount;
            cluster.maxFromCluster = MaxClusterAmount;
            cluster.forceAxialMultiplier = new Vector3(ClusterLaunchStrength, ClusterLaunchStrength, ClusterLaunchStrength);
            cluster.clusterObjects = DebrisObjectArray;
            return cluster;
        }
        /// <summary>
        /// Generates, and returns an array of ShardClusters that you can add to your breakable to have it create shards. note that each ShardCluster generated here will all use the same values you gave it
        /// </summary>
        /// <param name="clusterForceMultiplier">The force applied multiplicatively onto the shards when they're created.</param>
        /// <param name="ClusterLaunchStrength">The amount of force applied multiplicatively to your shards when created.</param>
        /// <param name="MinClusterAmount">The minimum amount of shards that the shard cluster will spawn.</param>
        /// <param name="MaxClusterAmount">The maximum amount of shards that the shard cluster will spawn.</param>
        /// <param name="clusterRotationMultiplier">The amount of rotation applied multiplicatively.</param>
        public static ShardCluster[] GenerateShardClustersFromArray(List<DebrisObject[]> debrisObjectList, float ClusterLaunchStrength = 0.5f, float clusterForceMultiplier = 2, int MinClusterAmount = 1, int MaxClusterAmount = 5, float clusterRotationMultiplier = 1)
        {
            List<ShardCluster> ShardClusters = new List<ShardCluster>();
            for (int i = 0; i < debrisObjectList.Count; i++)
            {
                ShardCluster cluster = new ShardCluster();
                DebrisObject[] DebrisObjectArray = debrisObjectList[i];
                cluster.forceMultiplier = clusterForceMultiplier;
                cluster.rotationMultiplier = clusterRotationMultiplier;
                cluster.minFromCluster = MinClusterAmount;
                cluster.maxFromCluster = MaxClusterAmount;
                cluster.forceAxialMultiplier = new Vector3(ClusterLaunchStrength, ClusterLaunchStrength, ClusterLaunchStrength);
                cluster.clusterObjects = DebrisObjectArray;
                ShardClusters.Add(cluster);
            }
            ShardCluster[] clusterArray = ShardClusters.ToArray();
            return clusterArray;
        }

        //Everything below here is an example of how you would generate your own KickableObject
        public static void ExampleKickableObjectSetup()
        {
            string defaultPath = "Planetside/Resources/DungeonObjects/EmberPot/";
            string defaultTablePath = "Planetside/Resources/DungeonObjects/testTable/";

            string[] idlePaths = new string[]
            {
                defaultPath+"Pot/emberpot_idle_001.png",
                defaultPath+"Pot/emberpot_idle_002.png",
            };
            string[] flipLeftPaths = new string[]
            {
                defaultTablePath+"derptable_flip_002.png",
            };
            string[] flipRightPaths = new string[]
            {
                defaultTablePath+"derptable_flip_right_002.png",
            };
            string[] flipUpPaths = new string[]
            {
                defaultTablePath+"derptable_flip_up_002.png",
            };
            string[] flipDownPaths = new string[]
            {
                defaultTablePath+"derptable_flip_down_002.png",
            };

            string[] breakLeftPaths = new string[]
            {
                defaultTablePath+"derptable_break_left_001.png",
                defaultTablePath+"derptable_break_left_002.png",
            };
            string[] breakRightPaths = new string[]
            {
                defaultTablePath+"derptable_break_right_001.png",
                defaultTablePath+"derptable_break_right_002.png",
            };
            string[] breakUpPaths = new string[]
            {
                defaultTablePath+"derptable_break_up_001.png",
                defaultTablePath+"derptable_break_up_002.png",
            };
            string[] breakDownPaths = new string[]
            {
                defaultTablePath+"derptable_break_down_001.png",
                defaultTablePath+"derptable_break_down_002.png",
            };
            string[] breakUnflippedPaths = new string[]
            {
                defaultTablePath+"derptable_breakunflipped_001.png",
                defaultTablePath+"derptable_breakunflipped_002.png",
                defaultTablePath+"derptable_breakunflipped_003.png",
            };
            string[] breakPathsassad = new string[]
            {
                defaultPath+"Pot/emberpot_break_001.png",
            };
            KickableObject kickable = GenerateKickableObject("test", idlePaths, flipUpPaths, flipDownPaths, flipLeftPaths, flipRightPaths, breakUpPaths, breakDownPaths, breakLeftPaths, breakRightPaths, breakUnflippedPaths, breakPathsassad, 2, 1, 1, 2, 2, false, 16, 16, 0, 0, true);// "Play_OBJ_barrel_break_01", 5);
            kickable.triggersBreakTimer = false; //If true, your kickable will break after being kicked after a certain amount of time has passed.
            kickable.breakTimerLength = 10; //The amount of time that your kickable will live for before going bye-bye.
            kickable.RollingDestroysSafely = true; //if true, allows the player to dodge-roll into it safely to break it regardless of collision? idfk.
            kickable.timerVFX = null; //if NOT null, enables a gameobject that adds as a VFX for when it is rolling but ONLY if triggersBreakTimer is true.     
            kickable.leavesGoopTrail = false; //if true, makes your kickable leave a goop trail when it rolls.
            kickable.goopFrequency = 0.05f; //the time between spawning goop when it is rolling.
            kickable.goopRadius = 1;//the radius of the goop that spawns.
            kickable.goopType = null;//the goop that spawns.

            //StaticReferences.StoredRoomObjects.Add("test", kickable.gameObject);
        }

        public static void EmberPotSetup()
        {
            string defaultPath = "Planetside/Resources/DungeonObjects/EmberPot/";
            string[] idlePaths = new string[]
            {
                defaultPath+"Pot/emberpot_idle_001.png",
                defaultPath+"Pot/emberpot_idle_002.png",
                defaultPath+"Pot/emberpot_idle_003.png",
                defaultPath+"Pot/emberpot_idle_004.png"
            };
            string[] breakPaths = new string[]
            {
                defaultPath+"Pot/emberpot_break_001.png",
            };
            string shadowPath = "Planetside/Resources/DungeonObjects/EmberPot/emberpotshadow.png";
            MinorBreakable breakable = GenerateMinorBreakable("Ember_Pot", idlePaths, 2, breakPaths, 4, "Play_OBJ_pot_shatter_01", shadowPath, 0, -0.125f, true, 13, 14, 1, 1);
            breakable.stopsBullets = true;
            breakable.OnlyPlayerProjectilesCanBreak = false;
            breakable.OnlyBreaksOnScreen = false;
            breakable.resistsExplosions = false;
            breakable.canSpawnFairy = false;
            breakable.chanceToRain = 1;
            breakable.dropCoins = false;
            breakable.goopsOnBreak = true; 
            breakable.goopRadius = 1.33f;
            //breakable.goopType = EasyGoopDefinitions.FireDef;
            breakable.breakStyle = MinorBreakable.BreakStyle.BURST; 
            string shardDefaultPath = "Planetside/Resources/DungeonObjects/EmberPot/";
            string[] shardPaths = new string[]
            {
                shardDefaultPath+"Shards/emberpot_shard1.png",
                shardDefaultPath+"Shards/emberpot_shard2.png",
                shardDefaultPath+"Shards/emberpot_shard3.png",
                shardDefaultPath+"Shards/emberpot_shard4.png",
                shardDefaultPath+"Shards/emberpot_shard5.png",
                shardDefaultPath+"Shards/emberpot_shard6.png"
            };
            DebrisObject[] shardObjects = GenerateDebrisObjects(shardPaths, true, 1, 5, 720, 540, null, 1.5f, null, null, 0, false);
            string emberDefPath = "Planetside/Resources/DungeonObjects/EmberPot/Embers/";
            string[] emberSmallPaths = new string[]
            {
                emberDefPath+"embersmall_001.png",
                emberDefPath+"embersmall_002.png",
                emberDefPath+"embersmall_003.png",
            };
            string[] emberTinyPaths = new string[]
            {
                emberDefPath+"embertiny_001.png",
                emberDefPath+"embertiny_002.png",
                emberDefPath+"embertiny_003.png",
            };
            string[] emberMediumPaths = new string[]
            {
                emberDefPath+"embermedium_001.png",
                emberDefPath+"embermedium_002.png",
                emberDefPath+"embermedium_003.png",
            };
            //GameObject poofVFX = (PickupObjectDatabase.GetById(336) as Gun).DefaultModule.projectiles[0].hitEffects.overrideMidairDeathVFX;
            //DebrisObject smallEmber = GenerateAnimatedDebrisObject(emberSmallPaths, 6, tk2dSpriteAnimationClip.WrapMode.Loop, true, 0.5f, 3, 1080, 540, null, 0.7f, null, poofVFX, 0, true, EasyGoopDefinitions.FireDef, 0.7f);
            //DebrisObject tinyEmber = GenerateAnimatedDebrisObject(emberTinyPaths, 7, tk2dSpriteAnimationClip.WrapMode.Loop, true, 0.4f, 2, 1600, 800, null, 0.5f, null, poofVFX, 0, true, EasyGoopDefinitions.FireDef, 0.5f);
           // DebrisObject mediumEmber = GenerateAnimatedDebrisObject(emberMediumPaths, 5, tk2dSpriteAnimationClip.WrapMode.Loop, true, 0.5f, 3, 1080, 540, null, 0.7f, null, poofVFX, 0, true, EasyGoopDefinitions.FireDef, 0.9f);
            //DebrisObject smallEmberTwo = GenerateAnimatedDebrisObject(emberSmallPaths, 7, tk2dSpriteAnimationClip.WrapMode.Loop, true, 0.35f, 2.5f, 900, 450, null, 0.8f, null, poofVFX, 0, true, EasyGoopDefinitions.FireDef, 0.9f);
            //DebrisObject tinyEmberTwo = GenerateAnimatedDebrisObject(emberTinyPaths, 6, tk2dSpriteAnimationClip.WrapMode.Loop, true, 0.4f, 2, 1300, 650, null, 0.5f, null, poofVFX, 0, true, EasyGoopDefinitions.FireDef, 0.5f);
            //DebrisObject mediumEmberTwo = GenerateAnimatedDebrisObject(emberMediumPaths, 4, tk2dSpriteAnimationClip.WrapMode.Loop, true, 0.7f, 4, 1500, 600, null, 0.8f, null, poofVFX, 0, true, EasyGoopDefinitions.FireDef, 1f);
            //ShardCluster potShardCluster = GenerateShardCluster(shardObjects, 0.35f, 1.2f, 6, 9, 0.8f);
            //ShardCluster emberClusterOne = GenerateShardCluster(new DebrisObject[] { smallEmber, tinyEmber, mediumEmber, smallEmberTwo, tinyEmberTwo, mediumEmberTwo }, 1f, 1.5f, 4, 7, 0.8f);
            //ShardCluster emberClusterTwo = GenerateShardCluster(new DebrisObject[] { smallEmber, tinyEmber, mediumEmber, smallEmberTwo, tinyEmberTwo, mediumEmberTwo }, 1f, 2f, 4, 7, 1f);
            //ShardCluster emberClusterThree = GenerateShardCluster(new DebrisObject[] { smallEmber, tinyEmber, mediumEmber, smallEmberTwo, tinyEmberTwo, mediumEmberTwo }, 1f, 2.5f, 4, 7, 1.2f);
            //ShardCluster[] array = new ShardCluster[] { potShardCluster, emberClusterOne, emberClusterTwo, emberClusterThree };
            //breakable.shardClusters = array;
            breakable.OnBreak += OnBroken; //Code that runs when the breakable is broken. If doesnt have any arguments so im not sure how useful it can be
            breakable.amountToRain = 30;
            breakable.EmitStyle = GlobalSparksDoer.EmitRegionStyle.RANDOM;
            breakable.ParticleColor = Color.red;
            breakable.ParticleLifespan = 2;
            //Attach ParticleSystem component to set particles
            breakable.ParticleMagnitude = 1;
            breakable.ParticleMagnitudeVariance = 1;
            breakable.ParticleSize = 0.1f;
            breakable.ParticleType = GlobalSparksDoer.SparksType.EMBERS_SWIRLING;
            breakable.hasParticulates = true;
            breakable.MaxParticlesOnBurst = 10;
            breakable.MinParticlesOnBurst = 4;
            //StaticReferences.StoredRoomObjects.Add("test", breakable.gameObject);

            //new Hook(typeof(KickableObject).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic), typeof(BreakableAPIToolbox).GetMethod("StartHookBox"));

        }

        //Everything below here is an example of how you would generate your own Minorbreakable
        public static void ExampleMinorBreakableSetup()
        {
            MinorBreakable breakable = GenerateMinorBreakable("testBreakable", new string[] { "Planetside/Resources/brokenchamberfixed.png", "Planetside/Resources/gunslingersring.png" }, 4, new string[] { "Planetside/Resources/gunslingersring.png", "Planetside/Resources/gunwarrant.png", "Planetside/Resources/plaetunstableteslacoil.png" }, 10, "Play_OBJ_pot_shatter_01", "Planetside/Resources/plaetunstableteslacoil.png", 0, -0.25f, true);
            //The reason it returns a minorbreakable is so you can set more values here, without adding to the setup method to prevent clutter. this is why i am leaving examples here

            breakable.stopsBullets = true; // makes projectiles break when colliding with the breakable
            breakable.OnlyPlayerProjectilesCanBreak = true;
            breakable.OnlyBreaksOnScreen = true;
            breakable.resistsExplosions = true;
            breakable.canSpawnFairy = false;

            //Coin Drops
            breakable.chanceToRain = 1; //The chance for your breakable to drop a casing
            breakable.dropCoins = false;

            //Explosives
            breakable.explodesOnBreak = false; //makes it explode when it breaks!
            breakable.explosionData = null; //the explosion that it uses when it does break and has the above bool set to true
            //Goops
            breakable.goopsOnBreak = false; //same as above, but for goops
            breakable.goopRadius = 1;
            breakable.goopType = null; //the goop that it uses when it breaks

            //Misc
            breakable.IgnoredForPotShotsModifier = false; //self explanatory
            breakable.stainObject = null; //The gameobject that is placed on the ground to act as a stain when the breakable is broken.
            breakable.CastleReplacedWithWaterDrum = false; //I assume its to prevent it from being replaced with a water barrel in the keep

            breakable.breakStyle = MinorBreakable.BreakStyle.BURST; // the breakstyle that the shards will use. certain ones will only burst shards in certain directions

            //An example for adding shard clusters using the GenerateDebrisObject/GenerateAnimatedDebrisObject and GenerateShardCluster method
            DebrisObject obj = GenerateDebrisObject("Planetside/Resources/gunslingersring.png");
            DebrisObject obj2 = GenerateAnimatedDebrisObject(new string[] { "Planetside/Resources/gunslingersring.png", "Planetside/Resources/gunwarrant.png", "Planetside/Resources/plaetunstableteslacoil.png" });
            //obj2.inertialMass = 1; 
            //obj2.shadowSprite

            //breakable.ShardSpawnOffset = new Vector2(0, 0); The offset in which shards will spawn

            ShardCluster cluster = GenerateShardCluster(new DebrisObject[] { obj, obj2 });
            ShardCluster[] array = new ShardCluster[] { cluster };
            breakable.shardClusters = array;


            //An example for adding shard clusters using the GenerateDebrisObjects/GenerateAnimatedDebrisObjects and GenerateShardClustersFromArray method
            /*
            DebrisObject[] objs = GenerateDebrisObjects(new string[] { "Planetside/Resources/heresyhammer.png", "Planetside/Resources/injectorrounds.png" });
            DebrisObject[] objs2 = GenerateAnimatedDebrisObjects(new List<string[]> { new string[] { "Planetside/Resources/bluecasing.png", "Planetside/Resources/bloodidol.png" }, new string[] { "Planetside/Resources/oscillatingbullets.png", "Planetside/Resources/planetsidemedal.png" } });
            ShardCluster[] clusters = GenerateShardClustersFromArray(new List<DebrisObject[]>() { objs, objs2 } );
            ShardCluster[] arrays = clusters;
            breakable.shardClusters = arrays;
            */


            breakable.OnBreak += OnBroken; //Code that runs when the breakable is broken. If doesnt have any arguments so im not sure how useful it can be


            // everything beloew here is for adding particle effects to your breakable when it breaks, tinker with your own caution!
            //note that you dont necessarily need a custom particle system for it to work
            /*
            breakable.breakStyle = BreakStyle.BURST;
            breakable.amountToRain = 30;
            breakable.EmitStyle = GlobalSparksDoer.EmitRegionStyle.RANDOM;
            breakable.ParticleColor = Color.red;
            breakable.ParticleLifespan = 2;
            //Attach ParticleSystem component to set particles
            breakable.ParticleMagnitude = 1;
            breakable.ParticleMagnitudeVariance = 1;
            breakable.ParticleSize = 0.1f;
            breakable.ParticleType = GlobalSparksDoer.SparksType.EMBERS_SWIRLING;
            breakable.hasParticulates = true;
            breakable.MaxParticlesOnBurst = 20;
            breakable.MinParticlesOnBurst = 2;
            */
            //StaticReferences.StoredRoomObjects.Add("test", breakable.gameObject);
        }


      

        //Everything below here is an example of how you would generate your own MajorBreakable
        public static void ExampleMajorBreakableSetup()
        {
            MajorBreakable breakable = GenerateMajorBreakable("testMajorBreakable", new string[] { "Planetside/Resources/gunslingersring.png", "Planetside/Resources/blashshower.png" }, 5, new string[] { "Planetside/Resources/gunslingersring.png", "Planetside/Resources/gunwarrant.png", "Planetside/Resources/plaetunstableteslacoil.png" }, 10, 30, "Planetside/Resources/brokenchamberfixedtier2.png", 0, -0.25f, true);

            breakable.GameActorMotionBreaks = false; //Breaks the MajorBreakable if anything move over it
            breakable.damageVfxMinTimeBetween = 0.1f; //the minimum time that the breakable has to wait before playing its damageVFX again
            breakable.distributeShards = true; // controls the distribultion of the shards? not sure
            breakable.EnemyDamageOverride = 2; // a value that overrides how much damage the breakable takes *IF* the source of the damage is an enemy
            breakable.MinHits = 1; //the minimum amount of hits it can take before *actually* taking damage
            breakable.ScaleWithEnemyHealth = false; //lets the breakables HP scale with the floor HP multiplier


            breakable.IgnoreExplosions = true; //not sure, it isnt used at all in the MajorBreakable class
            breakable.ImmuneToBeastMode = false;//not sure, it isnt used at all in the MajorBreakable class

            breakable.InvulnerableToEnemyBullets = false; //prevents damage from enemy bullets
            breakable.OnlyExplosions = false;//prevents all damage from alll sources except for explosions

            breakable.SpawnItemOnBreak = true; //enambles/disables whether an item spawns from it
            breakable.ItemIdToSpawnOnBreak = 73; //the ID of the item it spawns when it is broken. this one is set to spawn half red hearts

            breakable.shardBreakStyle = MinorBreakable.BreakStyle.BURST; // the breakstyle that the shards will use. certain ones will only burst shards in certain directions

            breakable.TemporarilyInvulnerable = false; //seems to prevent all damage, doesnt seem to run on any timer
            breakable.usesTemporaryZeroHitPointsState = false; // becomes invulnerable when reaches 0 HP, probably for use with spriteNameToUseAtZeroHP
            //breakable.spriteNameToUseAtZeroHP = null;

            breakable.PlayerRollingBreaks = true; //breaks it instantly if the player dodge rolls into it

            breakable.minShardPercentSpeed = 0.5f; //The minimum multiplier for the shards speed
            breakable.maxShardPercentSpeed = 3;  //The maximum multiplier for the shards speed

            breakable.destroyedOnBreak = true; //Destroys the object *completely* when its HP reaches 0. having this false will keep the object but remove its collision when its HP reaches 0
            breakable.handlesOwnBreakAnimation = true;

            //An example for adding shard clusters using the GenerateDebrisObject/GenerateAnimatedDebrisObject and GenerateShardCluster method
            DebrisObject obj = GenerateDebrisObject("Planetside/Resources/gunslingersring.png");
            DebrisObject obj2 = GenerateAnimatedDebrisObject(new string[] { "Planetside/Resources/gunslingersring.png", "Planetside/Resources/gunwarrant.png", "Planetside/Resources/plaetunstableteslacoil.png" });
            ShardCluster cluster = GenerateShardCluster(new DebrisObject[] { obj, obj2 });
            ShardCluster[] array = new ShardCluster[] { cluster };
            breakable.shardClusters = array;


            //An example for adding shard clusters using the GenerateDebrisObjects/GenerateAnimatedDebrisObjects and GenerateShardClustersFromArray method
            /*
            DebrisObject[] objs = GenerateDebrisObjects(new string[] { "Planetside/Resources/heresyhammer.png", "Planetside/Resources/injectorrounds.png" });
            DebrisObject[] objs2 = GenerateAnimatedDebrisObjects(new List<string[]> { new string[] { "Planetside/Resources/bluecasing.png", "Planetside/Resources/bloodidol.png" }, new string[] { "Planetside/Resources/oscillatingbullets.png", "Planetside/Resources/planetsidemedal.png" } });
            ShardCluster[] clusters = GenerateShardClustersFromArray(new List<DebrisObject[]>() { objs, objs2 } );
            ShardCluster[] arrays = clusters;
            breakable.shardClusters = arrays;
            */
            //StaticReferences.StoredRoomObjects.Add("test", breakable.gameObject);

        }

        public static void ExampleTableSetup()
        {
            string defaultPath = "Planetside/Resources/DungeonObjects/testTable/";
            string[] idlePaths = new string[]
            {
                defaultPath+"derptable_001.png",
                defaultPath+"derptable_002.png",
                defaultPath+"derptable_003.png",
            };
            string[] flipLeftPaths = new string[]
            {
                defaultPath+"derptable_flip_001.png",
                defaultPath+"derptable_flip_002.png",
            };
            string[] flipRightPaths = new string[]
            {
                defaultPath+"derptable_flip_right_001.png",
                defaultPath+"derptable_flip_right_002.png",
            };
            string[] flipUpPaths = new string[]
            {
                defaultPath+"derptable_flip_up_001.png",
                defaultPath+"derptable_flip_up_002.png",
            };
            string[] flipDownPaths = new string[]
            {
                defaultPath+"derptable_flip_down_001.png",
                defaultPath+"derptable_flip_down_002.png",
            };
            string[] breakLeftPaths = new string[]
{
                defaultPath+"derptable_break_left_001.png",
                defaultPath+"derptable_break_left_002.png",
};
            string[] breakRightPaths = new string[]
            {
                defaultPath+"derptable_break_right_001.png",
                defaultPath+"derptable_break_right_002.png",
            };
            string[] breakUpPaths = new string[]
            {
                defaultPath+"derptable_break_up_001.png",
                defaultPath+"derptable_break_up_002.png",
            };
            string[] breakDownPaths = new string[]
            {
                defaultPath+"derptable_break_down_001.png",
                defaultPath+"derptable_break_down_002.png",
            };
            string[] breakUnflippedPaths = new string[]
            {
                defaultPath+"derptable_breakunflipped_001.png",
                defaultPath+"derptable_breakunflipped_002.png",
                defaultPath+"derptable_breakunflipped_003.png",
            };

            FlippableCover flippableCover = GenerateTable("testTable", idlePaths, new string[] { "Planetside/Resources/outlineNorth.png", "Planetside/Resources/outlineEast.png", "Planetside/Resources/outlineWest.png", "Planetside/Resources/outlineSouth.png" }, flipUpPaths, flipDownPaths, flipLeftPaths, flipRightPaths, breakUpPaths, breakDownPaths, breakLeftPaths, breakRightPaths, breakUnflippedPaths, 3, 3, 3, 3, true, 20, 11, 0, 5, 20, 2, 4, 8, FlippableCover.FlipStyle.ANY , 90, "Planetside/Resources/cointosser.png", null, new Dictionary<float, string>() { {50, "Planetside/Resources/cointosser.png" } });

            //StaticReferences.StoredRoomObjects.Add("test", flippableCover.gameObject);
            //new Hook(typeof(FlippableCover).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic), typeof(BreakableAPIToolbox).GetMethod("StartHook"));

            

        }

      
    }
}
