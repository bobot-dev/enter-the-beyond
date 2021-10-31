﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using GungeonAPI;
using BotsMod;
using System;

namespace CustomCharacters
{
    /*
     * Handles adding sprites to the appropriate collections,
     * creating animations, and loading them when necessary
     */

    public static class SpriteHandler
    {
        private static FieldInfo spriteNameLookup =
            typeof(tk2dSpriteCollectionData).GetField("spriteNameLookupDict",
            BindingFlags.Instance | BindingFlags.NonPublic);

        private static FieldInfo m_playerMarkers = typeof(Minimap).GetField("m_playerMarkers", BindingFlags.NonPublic | BindingFlags.Instance);
        public static dfAtlas uiAtlas;
        public static List<dfAtlas.ItemInfo> uiFaceCards = new List<dfAtlas.ItemInfo>();
        public static List<dfAtlas.ItemInfo> punchoutFaceCards = new List<dfAtlas.ItemInfo>();

        private static readonly Rect uiFacecardBounds = new Rect(0, 1235, 2048, 813);
        private static readonly Rect punchoutFacecardBounds = new Rect(128, 71, 128, 186);
        private static readonly Vector2 faceCardSizeInPixels = new Vector2(34, 34);

        private static Dictionary<string, Material[]> usedMaterialDictionary = new Dictionary<string, Material[]>();

        public static void HandleSprites(PlayerController player, CustomCharacterData data)
        {
            if (data.minimapIcon != null)
                HandleMinimapIcons(player, data);

            if (data.bossCard != null)
                HandleBossCards(player, data);

            if (data.altSprites != null || data.altPlayerSheet != null)
                HandleAltAnimations(player, data);


            if (data.sprites != null || data.playerSheet != null)
                HandleAnimations(player, data);




            //face card stuff
            uiAtlas = data.atlas;//GameUIRoot.Instance.ConversationBar.portraitSprite.Atlas;
            if (data.faceCard != null)
                HandleFacecards(player, data);

            if (data.punchoutFaceCards != null && data.punchoutFaceCards.Count > 0)
                HandlePunchoutFaceCards(data);


            if (data.loadoutSprites != null)
                HandleLoudoutSprites(player, data);
            else
                ToolsGAPI.Print("        loadout sprites is null.", "FFBB00");
        }
        private static tk2dSpriteCollectionData itemCollection = PickupObjectDatabase.GetByEncounterName("singularity").sprite.Collection;
        /// <summary>
        /// Returns an object with a tk2dSprite component with the texture provided
        /// </summary>
        public static GameObject SpriteFromTexture(Texture2D texture, GameObject obj = null)
        {
            if (obj == null)
            {
                obj = new GameObject();
            }
            tk2dSprite sprite;
            sprite = obj.AddComponent<tk2dSprite>();

            int id = AddSpriteToCollection(texture, itemCollection);
            sprite.SetSprite(itemCollection, id);
            sprite.SortingOrder = 0;
            sprite.IsPerpendicular = true;

            obj.GetComponent<BraveBehaviour>().sprite = sprite;

            return obj;
        }

        public static void HandleLoudoutSprites(PlayerController player, CustomCharacterData data)
        {
            //var UIRootPrefab = AmmonomiconAPI.Tools.LoadAssetFromAnywhere<GameObject>("UI Root").GetComponent<GameUIRoot>();

           

            for (int i = 0; i < data.loadoutSprites.Count; i++)
            {
                var sprite = uiAtlas.AddNewItemToAtlas(AddOutlineToTexture(data.loadoutSprites[i], Color.white), data.loadoutSprites[i].name.Replace(" ","_"));
                
                
                data.loadoutSpriteNames.Add(data.loadoutSprites[i].name.Replace(" ", "_"));
            }
            ToolsGAPI.ExportTexture(uiAtlas.Texture, "SpriteDump/" + "atlasthingo");
        }


        /// <summary>
        /// Adds a sprite (from a resource) to a collection
        /// </summary>
        /// <returns>The spriteID of the defintion in the collection</returns>
        public static int AddSpriteToCollection(Texture2D texture, tk2dSpriteCollectionData collection, string name = "")
        {

            var definition = ConstructDefinition(texture); //Generate definition
            if (string.IsNullOrEmpty(name))
            {
                definition.name = texture.name; //naming the definition is actually extremely important 
            }
            else
            {
                definition.name = name; //naming the definition is actually extremely important 
            }


            return AddSpriteToCollection(definition, collection);
        }

        /// <summary>
        /// Constructs a new tk2dSpriteDefinition with the given texture
        /// </summary>
        /// <returns>A new sprite definition with the given texture</returns>
        public static tk2dSpriteDefinition ConstructDefinition(Texture2D texture)
        {
            RuntimeAtlasSegment ras = ETGMod.Assets.Packer.Pack(texture); //pack your resources beforehand or the outlines will turn out weird

            Material material = new Material(ShaderCache.Acquire(PlayerController.DefaultShaderName));
            material.mainTexture = ras.texture;
            //material.mainTexture = texture;

            var width = texture.width;
            var height = texture.height;

            var x = 0f;
            var y = 0f;

            var w = width / 16f;
            var h = height / 16f;

            var def = new tk2dSpriteDefinition
            {
                normals = new Vector3[] {
                new Vector3(0.0f, 0.0f, -1.0f),
                new Vector3(0.0f, 0.0f, -1.0f),
                new Vector3(0.0f, 0.0f, -1.0f),
                new Vector3(0.0f, 0.0f, -1.0f),
            },
                tangents = new Vector4[] {
                new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
            },
                texelSize = new Vector2(1 / 16f, 1 / 16f),
                extractRegion = false,
                regionX = 0,
                regionY = 0,
                regionW = 0,
                regionH = 0,
                flipped = tk2dSpriteDefinition.FlipMode.None,
                complexGeometry = false,
                physicsEngine = tk2dSpriteDefinition.PhysicsEngine.Physics3D,
                colliderType = tk2dSpriteDefinition.ColliderType.None,
                collisionLayer = CollisionLayer.HighObstacle,
                position0 = new Vector3(x, y, 0f),
                position1 = new Vector3(x + w, y, 0f),
                position2 = new Vector3(x, y + h, 0f),
                position3 = new Vector3(x + w, y + h, 0f),
                material = material,
                materialInst = material,
                materialId = 0,
                //uvs = ETGMod.Assets.GenerateUVs(texture, 0, 0, width, height), //uv machine broke
                uvs = ras.uvs,
                boundsDataCenter = new Vector3(w / 2f, h / 2f, 0f),
                boundsDataExtents = new Vector3(w, h, 0f),
                untrimmedBoundsDataCenter = new Vector3(w / 2f, h / 2f, 0f),
                untrimmedBoundsDataExtents = new Vector3(w, h, 0f),
            };

            def.name = texture.name;
            return def;
        }

        public static Texture2D AddOutlineToTexture(Texture2D sprite, Color color)
        {

            int y = Mathf.FloorToInt(0);
            int width = Mathf.FloorToInt(sprite.width);
            int height = Mathf.FloorToInt(sprite.height);

            var posList = new List<Vector2Int> { new Vector2Int { x = 0, y = 1 }, new Vector2Int { x = 0, y = -1 }, new Vector2Int { x = 1, y = 0 }, new Vector2Int { x = -1, y = 0 }, };

            sprite.AddBorder(1);

            for (int x = 0; y < sprite.height + 1; x++)
            {
                if (x >= sprite.width)
                {
                    x = 0;
                    y++;
                    
                }
                //BotsModule.Log($"{sprite.name} ({x}, {y})");
                var pixel = sprite.GetPixel(x, y);
                if (pixel.a > 0 && pixel != Color.white)
                {
                    for(int i = 0; i < 4; i++)
                    {
                        var pixel1 = sprite.GetPixel(x + posList[i].x, y + posList[i].y);
                        if (pixel1.a == 0)
                        {
                            //BotsModule.Log($"{sprite.name}: white pixel added at ({x + posList[i].x}, {y + posList[i].y})");
                            sprite.SetPixel(x + posList[i].x, y + posList[i].y, color);
                            sprite.Apply();
                        }
                    }
                }
            }

            ToolsGAPI.ExportTexture(sprite, "SpriteDump/" + "OutlineTest");

            // Color[] pix = sprite.GetPixels(x, y, width, height);
            //Texture2D destTex = new Texture2D(width, height);
            //destTex.SetPixels(pix);
            //destTex.Apply();


            return sprite;
        }

        public static bool AddBorder(this Texture2D tex, int borderSize = 1)
        {
            int width = 0;
            int height = 0;
            if (tex.IsReadable())
            {

                width = tex.width + borderSize * 2;
                height = tex.height + borderSize * 2;

                Color[][] pixels = new Color[Math.Min(tex.width, width)][];


                for (int x = 0; x < Math.Min(tex.width, width); x++)
                {
                    for (int y = 0; y < Math.Min(tex.height, height); y++)
                    {
                        if (pixels[x] == null)
                        {
                            pixels[x] = new Color[Math.Min(tex.height, height)];
                        }
                        pixels[x][y] = tex.GetPixel(x, y);
                    }
                }


                bool result = tex.Resize(width, height);
                for (int x = borderSize; x < tex.width - borderSize; x++)
                {
                    for (int y = borderSize; y < tex.height - borderSize; y++)
                    {
                        bool isInOrigTex = false;
                        if (x - borderSize < pixels.Length)
                        {
                            if (y - borderSize < pixels[x - borderSize].Length)
                            {
                                isInOrigTex = true;
                                tex.SetPixel(x, y, pixels[x - borderSize][y - borderSize]);
                            }
                        }
                        if (!isInOrigTex)
                        {
                            tex.SetPixel(x, y, Color.clear);
                        }
                    }
                }

                for (int x = 0; x < tex.width; x++)
                {
                    for (int y = 0; y < tex.height; y++)
                    {

                        if (tex.GetPixel(x, y) == new Color32(205, 205, 205, 205))
                        {
                            tex.SetPixel(x, y, Color.clear);
                        }

                    }
                }

                tex.Apply();
                return result;
            }
            return tex.Resize(width, height);
        }

        public static void HandleAnimations(PlayerController player, CustomCharacterData data)
        {
            var orig = player.sprite.Collection;
            var copyCollection = GameObject.Instantiate(orig);
            GameObject.DontDestroyOnLoad(copyCollection);

            tk2dSpriteDefinition[] copyDefinitions = new tk2dSpriteDefinition[orig.spriteDefinitions.Length];
            for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
            {
                copyDefinitions[i] = orig.spriteDefinitions[i].Copy();
            }
            copyCollection.spriteDefinitions = copyDefinitions;
            //ToolsGAPI.ExportTexture(TextureStitcher.GetReadable(copyCollection.textures[0]), "SpriteDump/balls", TextureStitcher.GetReadable(copyCollection.textures[0]).name + UnityEngine.Random.Range(1, 10000));
            
            if (data.playerSheet != null)
            {
                ToolsGAPI.Print("        Using sprite sheet replacement.", "FFBB00");
                var materialsToCopy = orig.materials;
                copyCollection.materials = new Material[orig.materials.Length];
                for (int i = 0; i < copyCollection.materials.Length; i++)
                {
                    if (materialsToCopy[i] == null) continue;
                    var mat = new Material(materialsToCopy[i]);
                    GameObject.DontDestroyOnLoad(mat);
                    mat.mainTexture = data.playerSheet;
                    mat.name = materialsToCopy[i].name;
                    copyCollection.materials[i] = mat;
                }

                for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
                {
                    foreach (var mat in copyCollection.materials)
                    {
                        if (mat != null && copyDefinitions[i].material.name.Equals(mat.name))
                        {
                            copyDefinitions[i].material = mat;
                            copyDefinitions[i].materialInst = new Material(mat);
                        }
                    }
                }
            }
            else if (data.sprites != null)
            {

                ToolsGAPI.Print("        Using individual sprite replacement.", "FFBB00");
                bool notSlinger = data.baseCharacter != PlayableCharacters.Gunslinger;

                RuntimeAtlasPage page = new RuntimeAtlasPage();
                for (int i = 0; i < data.sprites.Count; i++)
                {
                    var tex = data.sprites[i];

                    float nw = (tex.width) / 16f;
                    float nh = (tex.height) / 16f;

                    var def = copyCollection.GetSpriteDefinition(tex.name);
                    if (def != null)
                    {

                        if (notSlinger && def.boundsDataCenter != Vector3.zero)
                        {
                            var ras = page.Pack(tex);
                            def.materialInst.mainTexture = ras.texture;
                            def.uvs = ras.uvs;
                            def.extractRegion = true;
                            def.position0 = new Vector3(0, 0, 0);
                            def.position1 = new Vector3(nw, 0, 0);
                            def.position2 = new Vector3(0, nh, 0);
                            def.position3 = new Vector3(nw, nh, 0);

                            //BotsModule.Log(def.name + ": " + def.material.name + ", " + def.material.shader.name);

                            def.boundsDataCenter = new Vector2(nw / 2, nh / 2);
                            def.untrimmedBoundsDataCenter = def.boundsDataCenter;

                            def.boundsDataExtents = new Vector2(nw, nh);
                            def.untrimmedBoundsDataExtents = def.boundsDataExtents;
                        }
                        else
                        {
                            def.ReplaceTexture(tex);
                        }
                    }
                }
                page.Apply();
                ToolsGAPI.ExportTexture(TextureStitcher.GetReadable(page.Texture), "SpriteDump/balls", TextureStitcher.GetReadable(copyCollection.textures[0]).name + UnityEngine.Random.Range(1, 10000));
            }
            else
            {
                ToolsGAPI.Print("        Not replacing sprites.", "FFFF00");
            }
            
            player.spriteAnimator.Library = GameObject.Instantiate(player.spriteAnimator.Library);
            GameObject.DontDestroyOnLoad(player.spriteAnimator.Library);

            foreach (var clip in player.spriteAnimator.Library.clips)
            {
                for (int i = 0; i < clip.frames.Length; i++)
                {
                    clip.frames[i].spriteCollection = copyCollection;
                }
            }



            copyCollection.name = player.OverrideDisplayName;

            player.primaryHand.sprite.Collection = copyCollection;
            player.secondaryHand.sprite.Collection = copyCollection;
            player.sprite.Collection = copyCollection;
        }

        public static void HandleAltAnimations(PlayerController player, CustomCharacterData data)
        {

            var sharedAssets1 = ResourceManager.LoadAssetBundle("shared_auto_001");
            var guide_Swap = sharedAssets1.LoadAsset<GameObject>("Guide_Swap");
            

            tk2dSpriteCollectionData orig = guide_Swap.GetComponent<tk2dSpriteCollectionData>();

            var copyCollection = GameObject.Instantiate(orig);
            GameObject.DontDestroyOnLoad(copyCollection);



            tk2dSpriteDefinition[] copyDefinitions = new tk2dSpriteDefinition[orig.spriteDefinitions.Length];
            for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
            {
                copyDefinitions[i] = orig.spriteDefinitions[i].Copy();
            }
            copyCollection.spriteDefinitions = copyDefinitions;

            if (data.altPlayerSheet != null)
            {
                ToolsGAPI.Print("        Using sprite sheet replacement.", "FFBB00");
                var materialsToCopy = orig.materials;
                copyCollection.materials = new Material[orig.materials.Length];
                for (int i = 0; i < copyCollection.materials.Length; i++)
                {
                    if (materialsToCopy[i] == null) continue;
                    var mat = new Material(materialsToCopy[i]);
                    GameObject.DontDestroyOnLoad(mat);
                    mat.mainTexture = data.altPlayerSheet;
                    mat.name = materialsToCopy[i].name;
                    copyCollection.materials[i] = mat;
                }

                for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
                {
                    foreach (var mat in copyCollection.materials)
                    {
                        if (mat != null && copyDefinitions[i].material.name.Equals(mat.name))
                        {
                            copyDefinitions[i].material = mat;
                            copyDefinitions[i].materialInst = new Material(mat);
                        }
                    }
                }
            }
            
            else if (data.altSprites != null)
            {
                BotsModule.Log("altSprites arent null thank god!");
                ToolsGAPI.Print("        Using individual sprite replacement.", "FFBB00");
                bool notSlinger = data.baseCharacter != PlayableCharacters.Gunslinger;

                RuntimeAtlasPage page = new RuntimeAtlasPage();
                for (int i = 0; i < data.altSprites.Count; i++)
                {
                    var tex = data.altSprites[i];

                    float nw = (tex.width) / 16f;
                    float nh = (tex.height) / 16f;

                    var def = copyCollection.GetSpriteDefinition(tex.name);
                    if (def != null)
                    {
                        //BotsModule.Log("def isnt null thank god!");
                        if (notSlinger && def.boundsDataCenter != Vector3.zero)
                        {
                            var ras = page.Pack(tex);
                            def.materialInst.mainTexture = ras.texture;
                            def.uvs = ras.uvs;
                            def.extractRegion = true;
                            def.position0 = new Vector3(0, 0, 0);
                            def.position1 = new Vector3(nw, 0, 0);
                            def.position2 = new Vector3(0, nh, 0);
                            def.position3 = new Vector3(nw, nh, 0);

                            //BotsModule.Log("(alt) " + def.name + ": " + def.material.name + ", " + def.material.shader.name);

                            def.boundsDataCenter = new Vector2(nw / 2, nh / 2);
                            def.untrimmedBoundsDataCenter = def.boundsDataCenter;

                            def.boundsDataExtents = new Vector2(nw, nh);
                            def.untrimmedBoundsDataExtents = def.boundsDataExtents;
                        }
                        else
                        {
                            def.ReplaceTexture(tex);
                        }
                        //BotsModule.Log("def copy shit done thank god!");
                    }
                }
                BotsModule.Log("pre applying def shit idfk! thank god!");
                page.Apply();
            }
            else
            {
                ToolsGAPI.Print("        Not replacing sprites.", "FFFF00");
            }
            BotsModule.Log("balls wide... thank god!");
            player.AlternateCostumeLibrary = GameObject.Instantiate(player.AlternateCostumeLibrary);
            GameObject.DontDestroyOnLoad(player.AlternateCostumeLibrary);

            foreach (var clip in player.AlternateCostumeLibrary.clips)
            {
                for (int i = 0; i < clip.frames.Length; i++)
                {
                    clip.frames[i].spriteCollection = copyCollection;
                }
            }


            data.AlternateCostumeLibrary = player.AlternateCostumeLibrary;
            BotsModule.LostAltSkinAnimator = player.AlternateCostumeLibrary;

            copyCollection.name = player.OverrideDisplayName + "_alt";

            //player.primaryHand.sprite.Collection = copyCollection;
            //player.secondaryHand.sprite.Collection = copyCollection;
            //player.sprite.Collection = copyCollection;
        }

        public static Vector2[] GetMarginUVS(Texture2D orig, Texture2D margined)
        {
            int padding = TextureStitcher.padding;

            //float xOff = 0;
            //float yOff = 0;
            float xOff = (float)(padding) / (margined.width);
            float yOff = (float)(padding) / (margined.height);

            float w = (float)(orig.width) / (margined.width);
            float h = (float)(orig.height) / (margined.height);

            return new Vector2[]
            {
                new Vector2(xOff, yOff),
                new Vector2(xOff + w, yOff),
                new Vector2(xOff, yOff + h),
                new Vector2(xOff + w, yOff + h)
            };
        }

        public static Vector2[] defaultUVS = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
        };


        public static void HandleMinimapIcons(PlayerController player, CustomCharacterData data)
        {
            player.minimapIconPrefab = GameObject.Instantiate(player.minimapIconPrefab);
            var minimapSprite = player.minimapIconPrefab.GetComponent<tk2dSprite>();
            GameObject.DontDestroyOnLoad(minimapSprite);

            string iconName = "Player_" + player.name + "_001";
            int id = minimapSprite.Collection.GetSpriteIdByName(iconName, -1); //return -1 if not found

            if (id < 0)
            {
                var spriteDef = minimapSprite.GetCurrentSpriteDef();
                var copy = spriteDef.Copy();
                copy.ReplaceTexture(data.minimapIcon);
                copy.name = iconName;
                id = AddSpriteToCollection(copy, minimapSprite.Collection);
            }
            else
            {
                ToolsGAPI.Print("Minimap icon for " + iconName + " already found, not generating a new one");
            }

            //SetMinimapIconSpriteID(minimapSprite.spriteId, id);
            minimapSprite.SetSprite(id);
        }

        public static void HandleBossCards(PlayerController player, CustomCharacterData data)
        {
            int count = player.BosscardSprites.Count;
            player.BosscardSprites = new List<Texture2D>();
            for (int i = 0; i < count; i++)
            {
                player.BosscardSprites.Add(data.bossCard);
            }
        }

        public static void HandleFacecards(PlayerController player, CustomCharacterData data)
        {


            

            /*var atlas = uiAtlas;
            var atlasTex = atlas.Texture;

            dfAtlas.ItemInfo info = new dfAtlas.ItemInfo();
            info.name = player.name + "_facecard";
            info.region = TextureStitcher.AddFaceCardToAtlas(data.faceCard, atlasTex, uiFaceCards.Count, uiFacecardBounds);
            info.sizeInPixels = faceCardSizeInPixels;

            atlas.AddItem(info);

            if (atlas.Replacement)
            {
                atlas.Replacement.Material.mainTexture = atlasTex;
            }*/
            var sprite = uiAtlas.AddNewItemToAtlas(data.faceCard, player.name + "_facecard");
            uiFaceCards.Add(sprite);
        }

        public static void HandlePunchoutSprites(PunchoutPlayerController player, CustomCharacterData data)
        {
            var primaryPlayer = GameManager.Instance.PrimaryPlayer;
            player.PlayerUiSprite.Atlas = uiAtlas;

            if (data != null)
            {
                if (data.punchoutSprites != null && player.sprite.Collection.name != (data.nameShort + " Punchout Collection"))
                    HandlePunchoutAnimations(player, data);

                if (data.faceCard != null)
                {
                    player.PlayerUiSprite.SpriteName = data.nameInternal + "_punchout_facecard1";
                }

            }
        }

        public static void HandlePunchoutFaceCards(CustomCharacterData data)
        {
            var atlas = uiAtlas;
            var atlasTex = atlas.Texture;
            if (data.punchoutFaceCards != null)
            {
                ToolsGAPI.Print("Adding punchout facecards");
                int count = Mathf.Min(data.punchoutFaceCards.Count, 3);
                for (int i = 0; i < count; i++)
                {
                    var sprite = uiAtlas.AddNewItemToAtlas(data.punchoutFaceCards[i], data.nameInternal + "_punchout_facecard" + (i + 1));
                    //dfAtlas.ItemInfo info = new dfAtlas.ItemInfo();
                    //info.name = data.nameInternal + "_punchout_facecard" + (i + 1);
                    //info.region = TextureStitcher.AddFaceCardToAtlas(data.punchoutFaceCards[i], atlasTex, uiFaceCards.Count, uiFacecardBounds);
                    //info.sizeInPixels = faceCardSizeInPixels;

                    //atlas.AddItem(info);
                    uiFaceCards.Add(sprite);
                }
            }
        }

        public static void HandlePunchoutAnimations(PunchoutPlayerController player, CustomCharacterData data)
        {
            ToolsGAPI.Print("Replacing punchout sprites...");

            var orig = player.sprite.Collection;
            var copyCollection = GameObject.Instantiate(orig);

            GameObject.DontDestroyOnLoad(copyCollection);

            tk2dSpriteDefinition[] copyDefinitions = new tk2dSpriteDefinition[orig.spriteDefinitions.Length];
            for (int i = 0; i < copyCollection.spriteDefinitions.Length; i++)
            {
                copyDefinitions[i] = orig.spriteDefinitions[i].Copy();
            }
            copyCollection.spriteDefinitions = copyDefinitions;

            foreach (var tex in data.punchoutSprites)
            {
                var def = copyCollection.GetSpriteDefinition(tex.name);
                if (def != null)
                {
                    def.ReplaceTexture(tex.CropWhiteSpace());
                }
            }

            player.spriteAnimator.Library = GameObject.Instantiate(player.spriteAnimator.Library);
            GameObject.DontDestroyOnLoad(player.spriteAnimator.Library);

            foreach (var clip in player.spriteAnimator.Library.clips)
            {
                for (int i = 0; i < clip.frames.Length; i++)
                {
                    clip.frames[i].spriteCollection = copyCollection;
                }
            }

            copyCollection.name = data.nameShort + " Punchout Collection";
            //CharacterBuilder.storedCollections.Add(data.nameInternal, copyCollection);
            player.sprite.Collection = copyCollection;
            ToolsGAPI.Print("Punchout sprites successfully replaced");
        }

        public static void SetMinimapIconSpriteID(int key, int value)
        {
            if (Minimap.HasInstance)
            {
                var playerMarkers = (List<Tuple<Transform, Renderer>>)m_playerMarkers.GetValue(Minimap.Instance);
                foreach (var marker in playerMarkers)
                {
                    var sprite = marker.First.gameObject.GetComponent<tk2dSprite>();
                    if (sprite != null && sprite.spriteId == key)
                    {
                        sprite.SetSprite(value);
                    }
                }
            }
        }

        public static tk2dSpriteDefinition Copy(this tk2dSpriteDefinition orig)
        {



            tk2dSpriteDefinition copy = new tk2dSpriteDefinition()
            {
                boundsDataCenter = orig.boundsDataCenter,
                boundsDataExtents = orig.boundsDataExtents,
                colliderConvex = orig.colliderConvex,
                colliderSmoothSphereCollisions = orig.colliderSmoothSphereCollisions,
                colliderType = orig.colliderType,
                colliderVertices = orig.colliderVertices,
                collisionLayer = orig.collisionLayer,
                complexGeometry = orig.complexGeometry,
                extractRegion = orig.extractRegion,
                flipped = orig.flipped,
                indices = orig.indices,
                //material = new Material(orig.material),
                materialId = orig.materialId,
                //materialInst = new Material(orig.materialInst),
                metadata = orig.metadata,
                name = orig.name,
                normals = orig.normals,
                physicsEngine = orig.physicsEngine,
                position0 = orig.position0,
                position1 = orig.position1,
                position2 = orig.position2,
                position3 = orig.position3,
                regionH = orig.regionH,
                regionW = orig.regionW,
                regionX = orig.regionX,
                regionY = orig.regionY,
                tangents = orig.tangents,
                texelSize = orig.texelSize,
                untrimmedBoundsDataCenter = orig.untrimmedBoundsDataCenter,
                untrimmedBoundsDataExtents = orig.untrimmedBoundsDataExtents,
                uvs = orig.uvs
            };

            if (orig.material != null)
            {                 
                copy.material = new Material(orig.material);
            }
            else
            {
                BotsModule.Log("material is null thanknt god!");
            }
            if (orig.materialInst != null)
            {
                
                copy.materialInst = new Material(orig.materialInst);
            }
            else
            {
                //BotsModule.Log("materialInst is null thanknt god!");
            }


            return copy;
        }

        public static tk2dSpriteDefinition Copy(this tk2dSpriteDefinition orig, bool ignoreMat)
        {
            tk2dSpriteDefinition copy = new tk2dSpriteDefinition()
            {
                boundsDataCenter = orig.boundsDataCenter,
                boundsDataExtents = orig.boundsDataExtents,
                colliderConvex = orig.colliderConvex,
                colliderSmoothSphereCollisions = orig.colliderSmoothSphereCollisions,
                colliderType = orig.colliderType,
                colliderVertices = orig.colliderVertices,
                collisionLayer = orig.collisionLayer,
                complexGeometry = orig.complexGeometry,
                extractRegion = orig.extractRegion,
                flipped = orig.flipped,
                indices = orig.indices,

                metadata = orig.metadata,
                name = orig.name,
                normals = orig.normals,
                physicsEngine = orig.physicsEngine,
                position0 = orig.position0,
                position1 = orig.position1,
                position2 = orig.position2,
                position3 = orig.position3,
                regionH = orig.regionH,
                regionW = orig.regionW,
                regionX = orig.regionX,
                regionY = orig.regionY,
                tangents = orig.tangents,
                texelSize = orig.texelSize,
                untrimmedBoundsDataCenter = orig.untrimmedBoundsDataCenter,
                untrimmedBoundsDataExtents = orig.untrimmedBoundsDataExtents,
                uvs = orig.uvs
            };
            return copy;
        }

        public static tk2dSpriteAnimationClip CopyOf(tk2dSpriteAnimationClip orig)
        {
            return new tk2dSpriteAnimationClip(orig);
        }

        public static int AddSpriteToCollection(tk2dSpriteDefinition spriteDefinition, tk2dSpriteCollectionData collection)
        {
            //Add definition to collection
            var defs = collection.spriteDefinitions;
            var newDefs = defs.Concat(new tk2dSpriteDefinition[] { spriteDefinition }).ToArray();
            collection.spriteDefinitions = newDefs;

            //Reset lookup dictionary
            spriteNameLookup.SetValue(collection, null);  //Set dictionary to null
            collection.InitDictionary(); //InitDictionary only runs if the dictionary is null
            return newDefs.Length - 1;
        }


        public class ReplacedCharacterData
        {
            public PlayableCharacters baseCharacter;
            public int origMapIconID = -1;
            public int replaceMapIconID = -1;
            public tk2dSpriteCollectionData origPlayerCollection;
            public tk2dSpriteCollectionData replacePlayerCollection;
        }
    }
}
