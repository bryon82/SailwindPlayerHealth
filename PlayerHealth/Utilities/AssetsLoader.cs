using System.IO;
using UnityEngine;

namespace PlayerHealth
{
    internal class AssetsLoader
    {
        public static Texture2D healthIconTexture;
        public static Material healthIconMaterial;

        public static void LoadAssets()
        {
            var assetsPath = Path.Combine(Path.GetDirectoryName(Plugin.instance.Info.Location), "Assets");
            healthIconTexture = LoadTexture(Path.Combine(assetsPath, "Heart.png"));
            healthIconMaterial = CreateMaterial(healthIconTexture);
            Plugin.logger.LogInfo("Health icon loaded.");
        }

        private static Texture2D LoadTexture(string path)
        {
            byte[] array = File.Exists(path) ? File.ReadAllBytes(path) : null;
            Texture2D texture2D = new Texture2D(1, 1);
            if (array == null)
            {
                Plugin.logger.LogError("Failed to load " + path);
                return texture2D;
            }
            ImageConversion.LoadImage(texture2D, array);
            return texture2D;
        }

        private static Material CreateMaterial(Texture2D tex)
        {
            Material material = new Material(Shader.Find("Standard"))
            {
                renderQueue = 2001,
                mainTexture = tex
            };
            material.EnableKeyword("_ALPHATEST_ON");
            material.SetShaderPassEnabled("ShadowCaster", false);
            return material;
        }        
    }
}

