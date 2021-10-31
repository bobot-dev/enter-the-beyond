using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BotsMod
{
    class AtlasTesting
    {
        public static dfAtlas CreateAtlasFromSelection(string[] paths, string name)
        {

            try
            {
                var textures = new List<Texture2D>();
                foreach (var path in paths)
                {
                    textures.Add(ItemAPI.ResourceExtractor.GetTextureFromResource(path));
                }
                var selection = textures.ToArray();
                if (selection.Length == 0)
                {
                    ETGModConsole.Log("Either no textures selected or none of the selected textures has Read/Write enabled");
                    return null;
                }

                

                var saveFolder = Path.Combine(ETGMod.ResourcesDirectory, "SpriteDump/");//Path.GetDirectoryName(AssetDatabase.GetAssetPath(selection[0]));
                /*var prefabPath = EditorUtility.SaveFilePanel("Create Texture Atlas", saveFolder, "Texture Atlas", "prefab");
                if (string.IsNullOrEmpty(prefabPath))
                    return;

                prefabPath = prefabPath.MakeRelativePath();*/

                var padding = 3;

                var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                var rects = texture.PackTextures2(selection, padding, 4096, false);

                //var texturePath = Path.ChangeExtension(prefabPath, "png");
                byte[] bytes = texture.EncodeToPNG();


                File.WriteAllBytes(Path.Combine(saveFolder, name + ".png"), bytes);

                //System.IO.File.WriteAllBytes(texturePath, bytes);
                bytes = null;
                UnityEngine.Object.DestroyImmediate(texture);


                /*setAtlasTextureSettings(texturePath, true);

                texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D;
                if (texture == null)
                    Debug.LogError("Failed to find texture at " + texturePath); */

                var sprites = new List<dfAtlas.ItemInfo>();
                for (int i = 0; i < rects.Length; i++)
                {

                    var pixelCoords = rects[i];
                    var size = new Vector2(selection[i].width, selection[i].height);

                    //var spritePath = AssetDatabase.GetAssetPath(selection[i]);
                    var guid = Guid.NewGuid().ToString();

                    var item = new dfAtlas.ItemInfo()
                    {
                        name = selection[i].name,
                        region = pixelCoords,
                        rotated = false,
                        textureGUID = paths[i],
                        sizeInPixels = size
                    };

                    sprites.Add(item);

                }

                sprites.Sort();

                var shader = Shader.Find("Daikon Forge/Default UI Shader");
                var atlasMaterial = new Material(shader);
                atlasMaterial.mainTexture = texture;
                //AssetDatabase.CreateAsset(atlasMaterial, Path.ChangeExtension(texturePath, "mat"));

                var go = new GameObject() { name = name };
                var atlas = go.AddComponent<dfAtlas>();
                atlas.Material = atlasMaterial;
                atlas.AddItems(sprites);
                return atlas;
                /*(var prefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
                prefab.name = atlas.name;
                PrefabUtility.ReplacePrefab(go, prefab);

                DestroyImmediate(go);
                AssetDatabase.Refresh();

                #region Delay execution of object selection to work around a Unity issue

                // Declared with null value to eliminate "uninitialized variable" 
                // compiler error in lambda below.
                EditorApplication.CallbackFunction callback = null;

                callback = () =>
                {
                    EditorUtility.FocusProjectWindow();
                    go = AssetDatabase.LoadMainAssetAtPath(prefabPath) as GameObject;
                    Selection.objects = new Object[] { go };
                    EditorGUIUtility.PingObject(go);
                    Debug.Log("Texture Atlas prefab created at " + prefabPath, prefab);
                    EditorApplication.delayCall -= callback;
                };

                EditorApplication.delayCall += callback;

                #endregion*/

            }
            catch (Exception err)
            {
                ETGModConsole.Log(err.ToString());
                return null;
            }

        }
        public static bool AddTexture(dfAtlas atlas, string[] paths)
        {

            try
            {
                var textures = new List<Texture2D>();
                foreach (var path in paths)
                {
                    textures.Add(ItemAPI.ResourceExtractor.GetTextureFromResource(path));
                }

                Texture2D[] newTextures = textures.ToArray();
                //selectedTextures.Clear();

                var addedItems = new List<dfAtlas.ItemInfo>();

                for (int i = 0; i < newTextures.Length; i++)
                {

                    // Grab reference to existing item, if it exists, to preserve border information
                    var existingItem = atlas[newTextures[i].name];

                    // Remove the existing item if it already exists
                    atlas.Remove(newTextures[i].name);

                    // Keep the texture size available
                    var size = new Vector2(newTextures[i].width, newTextures[i].height);

                    // Determine the guid for the texture
                    //var spritePath = AssetDatabase.GetAssetPath(newTextures[i]);
                    var guid = Guid.NewGuid().ToString();

                    // Add the new texture to the Items collection
                    var newItem = new dfAtlas.ItemInfo()
                    {
                        textureGUID = paths[i],
                        name = newTextures[i].name,
                        border = (existingItem != null) ? existingItem.border : new RectOffset(),
                        sizeInPixels = size
                    };
                    addedItems.Add(newItem);
                    atlas.AddItem(newItem);

                }

                if (!rebuildAtlas(atlas))
                {
                    atlas.Items.RemoveAll(i => addedItems.Contains(i));
                    return false;
                }

                return true;

            }
            catch (Exception err)
            {
                Debug.LogError(err.ToString(), atlas);
                
            }

            return false;

        }
    

        public static Texture2D getTexture(string guid)
        {
            //var path = AssetDatabase.GUIDToAssetPath(guid);
            return new Texture2D(0, 0);//AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
        }

        public static Texture2D GetTextureFromPathOrGuid(string pathOrId)
        {
            ETGModConsole.Log(pathOrId);
            if (pathOrId.Contains("/") || pathOrId.Contains("\\"))
            {
                return ItemAPI.ResourceExtractor.GetTextureFromResource(pathOrId);
            } 
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
                ETGModConsole.Log("Streaming Assets Path: " + Application.streamingAssetsPath);
                FileInfo[] allFiles = directoryInfo.GetFiles("*.*");


                return null;//AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(pathOrId), typeof(Texture2D)) as Texture2D;
            }
            
        }

        public static bool rebuildAtlas(dfAtlas atlas)
        {

            try
            {

           
                var sprites = atlas.Items
                    .Where(i => i != null && !i.deleted)
                    .Select(i => new { source = i, texture = GetTextureFromPathOrGuid(i.textureGUID) })
                    .Where(i => i.texture != null)
                    .OrderByDescending(i => i.texture.width * i.texture.height)
                    .ToList();

                var textures = sprites.Select(i => i.texture).ToList();

                var oldAtlasTexture = atlas.Material.mainTexture;
                //var texturePath = AssetDatabase.GetAssetPath(oldAtlasTexture);

                var padding = 3;

                var newAtlasTexture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
                var newRects = newAtlasTexture.PackTextures2(textures.ToArray(), padding, 4096, false);

                byte[] bytes = newAtlasTexture.EncodeToPNG();
                File.WriteAllBytes(Path.Combine(Path.Combine(ETGMod.ResourcesDirectory, "SpriteDump/"), atlas.gameObject.name + ".png"), bytes);
                bytes = null;
                UnityEngine.Object.DestroyImmediate(newAtlasTexture);


            

                // Fix up the new sprite locations
                for (int i = 0; i < sprites.Count; i++)
                {
                    sprites[i].source.region = newRects[i];
                    sprites[i].source.sizeInPixels = new Vector2(textures[i].width, textures[i].height);
                    sprites[i].source.texture = null;
                }

                // Remove any deleted sprites
                atlas.Items.RemoveAll(i => i.deleted);

                // Re-sort the Items collection
                atlas.Items.Sort();
                atlas.RebuildIndexes();


                dfGUIManager.RefreshAll(true);

                return true;

            }
            catch (Exception err)
            {

                Debug.LogError(err.ToString(), atlas);
           
                return false;

            }
        }

    }
}
