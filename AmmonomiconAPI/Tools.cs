﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Reflection;
using System.IO;

namespace AmmonomiconAPI
{
    public static class Tools
    {
        private static string[] BundlePrereqs;

        public static void Init()
        {
            BundlePrereqs = new string[]
            {
                "brave_resources_001",
                "dungeon_scene_001",
                "encounters_base_001",
                "enemies_base_001",
                "flows_base_001",
                "foyer_001",
                "foyer_002",
                "foyer_003",
                "shared_auto_001",
                "shared_auto_002",
                "shared_base_001",
                "dungeons/base_bullethell",
                "dungeons/base_castle",
                "dungeons/base_catacombs",
                "dungeons/base_cathedral",
                "dungeons/base_forge",
                "dungeons/base_foyer",
                "dungeons/base_gungeon",
                "dungeons/base_mines",
                "dungeons/base_nakatomi",
                "dungeons/base_resourcefulrat",
                "dungeons/base_sewer",
                "dungeons/base_tutorial",
                "dungeons/finalscenario_bullet",
                "dungeons/finalscenario_convict",
                "dungeons/finalscenario_coop",
                "dungeons/finalscenario_guide",
                "dungeons/finalscenario_pilot",
                "dungeons/finalscenario_robot",
                "dungeons/finalscenario_soldier"
            };
            AmmonomiconHooks.Init();
        }

        public static void Log(string text, string color = "#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        /// <summary>
		/// Builds and adds a new <see cref="dfAtlas.ItemInfo"/> to <paramref name="atlas"/> with the texture of <paramref name="tex"/> and the name of <paramref name="name"/>.
		/// </summary>
		/// <param name="atlas">The <see cref="dfAtlas"/> to add the new <see cref="dfAtlas.ItemInfo"/> to.</param>
		/// <param name="tex">The texture of the new <see cref="dfAtlas.ItemInfo"/>.</param>
		/// <param name="name">The name of the new <see cref="dfAtlas.ItemInfo"/>. If <see langword="null"/>, it will default to <paramref name="tex"/>'s name.</param>
		/// <returns>The built <see cref="dfAtlas.ItemInfo"/>.</returns>
		public static dfAtlas.ItemInfo AddNewItemToAtlas(this dfAtlas atlas, Texture2D tex, string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = tex.name;
            }
            if (atlas[name] != null)
            {
                return atlas[name];
            }
            dfAtlas.ItemInfo item = new dfAtlas.ItemInfo
            {
                border = new RectOffset(),
                deleted = false,
                name = name,
                region = atlas.FindFirstValidEmptySpace(new IntVector2(tex.width, tex.height)),
                rotated = false,
                sizeInPixels = new Vector2(tex.width, tex.height),
                texture = tex,
                textureGUID = name
            };
            int startPointX = Mathf.RoundToInt(item.region.x * atlas.Texture.width);
            int startPointY = Mathf.RoundToInt(item.region.y * atlas.Texture.height);
            for (int x = startPointX; x < Mathf.RoundToInt(item.region.xMax * atlas.Texture.width); x++)
            {
                for (int y = startPointY; y < Mathf.RoundToInt(item.region.yMax * atlas.Texture.height); y++)
                {
                    atlas.Texture.SetPixel(x, y, tex.GetPixel(x - startPointX, y - startPointY));
                }
            }
            atlas.Texture.Apply();
            atlas.AddItem(item);
            return item;
        }


        /// <summary>
        /// Gets the pixel regions of <paramref name="atlas"/>.
        /// </summary>
        /// <param name="atlas">The <see cref="dfAtlas"/> to get the pixel regions from.</param>
        /// <returns>A list with all pixel regions in <paramref name="atlas"/></returns>
        public static List<RectInt> GetPixelRegions(this dfAtlas atlas)
        {
            return atlas.Items.Convert(delegate (dfAtlas.ItemInfo item)
            {
                return new RectInt(Mathf.RoundToInt(item.region.x * atlas.Texture.width), Mathf.RoundToInt(item.region.y * atlas.Texture.height), Mathf.RoundToInt(item.region.width * atlas.Texture.width),
                    Mathf.RoundToInt(item.region.height * atlas.Texture.height));
            });
        }

        /// <summary>
        /// Converts a list of the type <typeparamref name="T"/> to a list of the type <typeparamref name="T2"/> using <paramref name="convertor"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="self"/> list.</typeparam>
        /// <typeparam name="T2">The type to convert the <paramref name="self"/> list to.</typeparam>
        /// <param name="self">The original list.</param>
        /// <param name="convertor">A delegate that converts an element of type <typeparamref name="T"/> to an element of a type <typeparamref name="T2"/>.</param>
        /// <returns>The converted list of type <typeparamref name="T2"/></returns>
        public static List<T2> Convert<T, T2>(this List<T> self, Func<T, T2> convertor)
        {
            List<T2> result = new List<T2>();
            foreach (T element in self)
            {
                result.Add(convertor(element));
            }
            return result;
        }


        /// <summary>
		/// Gets the first empty space in <paramref name="atlas"/> that has at least the size of <paramref name="pixelScale"/>.
		/// </summary>
		/// <param name="atlas">The <see cref="dfAtlas"/> to find the empty space in.</param>
		/// <param name="pixelScale">The required size of the empty space.</param>
		/// <returns>The rect of the empty space divided by the atlas texture's size.</returns>
		public static Rect FindFirstValidEmptySpace(this dfAtlas atlas, IntVector2 pixelScale)
        {
            if (atlas == null || atlas.Texture == null || !atlas.Texture.IsReadable())
            {
                return new Rect(0f, 0f, 0f, 0f);
            }
            Vector2Int point = new Vector2Int(0, 0);
            int pointIndex = -1;
            List<RectInt> rects = atlas.GetPixelRegions();
            while (true)
            {
                bool shouldContinue = false;
                foreach (RectInt rint in rects)
                {
                    if (rint.Contains(point))
                    {
                        shouldContinue = true;
                        pointIndex++;
                        if (pointIndex >= rects.Count)
                        {
                            return new Rect(0f, 0f, 0f, 0f);
                        }
                        point = rects[pointIndex].max + Vector2Int.one;
                        if (point.x > atlas.Texture.width || point.y > atlas.Texture.height)
                        {
                            atlas.ResizeAtlas(new IntVector2(atlas.Texture.width * 2, atlas.Texture.height * 2));
                        }
                        break;
                    }
                    bool shouldBreak = false;
                    foreach (RectInt rint2 in rects)
                    {
                        RectInt currentRect = new RectInt(point, pixelScale.ToVector2Int());
                        if (rint2.x < currentRect.x || rint2.y < currentRect.y)
                        {
                            continue;
                        }
                        else
                        {
                            if (currentRect.Contains(rint2.position))
                            {
                                shouldContinue = true;
                                shouldBreak = true;
                                pointIndex++;
                                if (pointIndex >= rects.Count)
                                {
                                    return new Rect(0f, 0f, 0f, 0f);
                                }
                                point = rects[pointIndex].max + Vector2Int.one;
                                if (point.x > atlas.Texture.width || point.y > atlas.Texture.height)
                                {
                                    atlas.ResizeAtlas(new IntVector2(atlas.Texture.width * 2, atlas.Texture.height * 2));
                                }
                                break;
                            }
                        }
                    }
                    if (shouldBreak)
                    {
                        break;
                    }
                }
                if (shouldContinue)
                {
                    continue;
                }
                RectInt currentRect2 = new RectInt(point, pixelScale.ToVector2Int());
                if (currentRect2.xMax > atlas.Texture.width || currentRect2.yMax > atlas.Texture.height)
                {
                    atlas.ResizeAtlas(new IntVector2(atlas.Texture.width * 2, atlas.Texture.height * 2));
                }
                break;
            }
            RectInt currentRect3 = new RectInt(point, pixelScale.ToVector2Int());
            Rect rect = new Rect((float)currentRect3.x / atlas.Texture.width, (float)currentRect3.y / atlas.Texture.height, (float)currentRect3.width / atlas.Texture.width, (float)currentRect3.height / atlas.Texture.height);
            return rect;
        }

        /// <summary>
		/// Resizes <paramref name="atlas"/> and all of it's <see cref="dfAtlas.ItemInfo"/>s.
		/// </summary>
		/// <param name="atlas">The <see cref="dfAtlas"/> to resize/</param>
		/// <param name="newDimensions"><paramref name="atlas"/>'s new size.</param>
		public static void ResizeAtlas(this dfAtlas atlas, IntVector2 newDimensions)
        {
            Texture2D tex = atlas.Texture;
            if (!tex.IsReadable())
            {
                return;
            }
            if (tex.width == newDimensions.x && tex.height == newDimensions.y)
            {
                return;
            }
            foreach (dfAtlas.ItemInfo item in atlas.Items)
            {
                if (item.region != null)
                {
                    item.region.x = (item.region.x * tex.width) / newDimensions.x;
                    item.region.y = (item.region.y * tex.height) / newDimensions.y;
                    item.region.width = (item.region.width * tex.width) / newDimensions.x;
                    item.region.height = (item.region.height * tex.height) / newDimensions.y;
                }
            }
            tex.ResizeBetter(newDimensions.x, newDimensions.y);
            atlas.Material.SetTexture("_MainTex", tex);
        }


        /// <summary>
		/// Resizes <paramref name="tex"/> without it losing it's pixel information.
		/// </summary>
		/// <param name="tex">The <see cref="Texture2D"/> to resize.</param>
		/// <param name="width">The <paramref name="tex"/>'s new width.</param>
		/// <param name="height">The <paramref name="tex"/>'s new height.</param>
		/// <returns></returns>
		public static bool ResizeBetter(this Texture2D tex, int width, int height)
        {
            if (tex.IsReadable())
            {
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
                for (int x = 0; x < tex.width; x++)
                {
                    for (int y = 0; y < tex.height; y++)
                    {
                        bool isInOrigTex = false;
                        if (x < pixels.Length)
                        {
                            if (y < pixels[x].Length)
                            {
                                isInOrigTex = true;
                                tex.SetPixel(x, y, pixels[x][y]);
                            }
                        }
                        if (!isInOrigTex)
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

        /// <summary>
		/// Converts <paramref name="vector"/> to a <see cref="Vector2Int"/>.
		/// </summary>
		/// <param name="vector">The <see cref="IntVector2"/> to convert.</param>
		/// <returns><paramref name="vector"/> converted to <see cref="Vector2Int"/>.</returns>
		public static Vector2Int ToVector2Int(this IntVector2 vector)
        {
            return new Vector2Int(vector.x, vector.y);
        }


        /// <summary>
        /// Converts an embedded resource to a Texture2D object
        /// </summary>
        public static Texture2D GetTextureFromResource(string resourceName)
        {
            string file = resourceName;
            byte[] bytes = ExtractEmbeddedResource(file);
            if (bytes == null)
            {
                Tools.PrintError("No bytes found in " + file);
                return null;
            }
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            ImageConversion.LoadImage(texture, bytes);
            texture.filterMode = FilterMode.Point;

            string name = file.Substring(0, file.LastIndexOf('.'));
            if (name.LastIndexOf('.') >= 0)
            {
                name = name.Substring(name.LastIndexOf('.') + 1);
            }
            texture.name = name;

            return texture;
        }

        public static byte[] ExtractEmbeddedResource(String filePath)
        {
            filePath = filePath.Replace("/", ".");
            filePath = filePath.Replace("\\", ".");
            var baseAssembly = Assembly.GetCallingAssembly();
            using (Stream resFilestream = baseAssembly.GetManifestResourceStream(filePath))
            {
                if (resFilestream == null)
                {
                    return null;
                }
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                return ba;
            }
        }

        public static void PrintError<T>(T obj, string color = "FF0000")
        {
            ETGModConsole.Log($"<color=#{color}>[AmmonomiconAPI]: {obj.ToString()}</color>");
        }

        /// <summary>
        /// Loads an asset with the type of <typeparamref name="T"/> from any asset bundle.
        /// </summary>
        /// <typeparam name="T">The type of the asset that will be loaded.</typeparam>
        /// <param name="path">The asset's filepath in a bundle.</param>
        /// <returns>The loaded asset.</returns>
        public static T LoadAssetFromAnywhere<T>(string path) where T : UnityEngine.Object
        {
            if (BundlePrereqs == null)
            {
                Init();
            }
            T obj = null;
            foreach (string name in BundlePrereqs)
            {
                try
                {
                    obj = ResourceManager.LoadAssetBundle(name).LoadAsset<T>(path);
                }
                catch
                {
                }
                if (obj != null)
                {
                    break;
                }
            }
            return obj;
        }




    }
}
