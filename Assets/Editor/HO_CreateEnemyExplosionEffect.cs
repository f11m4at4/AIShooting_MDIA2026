using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class HO_CreateEnemyExplosionEffect
{
    private const string EffectsFolder = "Assets/Effects";
    private const string PrefabPath = EffectsFolder + "/EnemyExplosion.prefab";
    private const string SmokeTexturePath = EffectsFolder + "/EnemyExplosionSmoke.png";
    private const string BurstTexturePath = EffectsFolder + "/EnemyExplosionBurst.png";
    private const string SmokeMaterialPath = EffectsFolder + "/EnemyExplosionSmoke.mat";
    private const string BurstMaterialPath = EffectsFolder + "/EnemyExplosionBurst.mat";

    [MenuItem("Tools/HO/Create Enemy Explosion Effect")]
    public static void CreateAssets()
    {
        EnsureFolder("Assets", "Effects");

        var smokeTexture = CreateSmokeTexture(SmokeTexturePath);
        var burstTexture = CreateBurstTexture(BurstTexturePath);

        var smokeMaterial = CreateParticleMaterial(
            SmokeMaterialPath,
            smokeTexture,
            new Color(0.82f, 0.82f, 0.82f, 1f),
            false);

        var burstMaterial = CreateParticleMaterial(
            BurstMaterialPath,
            burstTexture,
            Color.white,
            true);

        CreateExplosionPrefab(smokeMaterial, burstMaterial);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Created enemy explosion effect at {PrefabPath}");
    }

    private static void EnsureFolder(string parentFolder, string childFolder)
    {
        var combinedPath = $"{parentFolder}/{childFolder}";
        if (!AssetDatabase.IsValidFolder(combinedPath))
        {
            AssetDatabase.CreateFolder(parentFolder, childFolder);
        }
    }

    private static Texture2D CreateSmokeTexture(string assetPath)
    {
        var texture = new Texture2D(128, 128, TextureFormat.RGBA32, false)
        {
            name = "EnemyExplosionSmoke"
        };

        var center = new Vector2(63.5f, 63.5f);
        var radius = 52f;

        for (var y = 0; y < texture.height; y++)
        {
            for (var x = 0; x < texture.width; x++)
            {
                var uv = new Vector2(x, y);
                var direction = uv - center;
                var distance01 = direction.magnitude / radius;
                var angle = Mathf.Atan2(direction.y, direction.x);

                var puffWobble = Mathf.Sin(angle * 5f) * 0.08f + Mathf.Cos(angle * 9f) * 0.04f;
                var ring = Mathf.Clamp01(1f - Mathf.Pow(Mathf.Clamp01(distance01 + puffWobble), 1.8f));
                var innerCut = Mathf.SmoothStep(0.12f, 0f, distance01);
                var alpha = Mathf.Clamp01(ring - innerCut * 0.35f);

                texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }

        texture.Apply();
        SaveTexture(texture, assetPath);
        return AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
    }

    private static Texture2D CreateBurstTexture(string assetPath)
    {
        var texture = new Texture2D(128, 128, TextureFormat.RGBA32, false)
        {
            name = "EnemyExplosionBurst"
        };

        var center = new Vector2(63.5f, 63.5f);
        var radius = 56f;

        for (var y = 0; y < texture.height; y++)
        {
            for (var x = 0; x < texture.width; x++)
            {
                var uv = new Vector2(x, y);
                var direction = uv - center;
                var distance = direction.magnitude;
                var distance01 = distance / radius;
                var angle = Mathf.Atan2(direction.y, direction.x);

                var spikePattern = Mathf.Abs(Mathf.Sin(angle * 4f)) * 0.22f;
                spikePattern += Mathf.Abs(Mathf.Sin(angle * 8f + 0.3f)) * 0.12f;
                var effectiveRadius = 0.5f + spikePattern;

                var core = Mathf.Clamp01(1f - Mathf.Pow(distance01 / 0.42f, 2.2f));
                var spikes = Mathf.Clamp01(1f - Mathf.Pow(distance01 / effectiveRadius, 3.6f));
                var cut = Mathf.SmoothStep(1f, 0.8f, distance01);
                var alpha = Mathf.Clamp01(Mathf.Max(core, spikes) * cut);

                texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }

        texture.Apply();
        SaveTexture(texture, assetPath);
        return AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
    }

    private static void SaveTexture(Texture2D texture, string assetPath)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
        File.WriteAllBytes(fullPath, texture.EncodeToPNG());
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

        if (AssetImporter.GetAtPath(assetPath) is TextureImporter importer)
        {
            importer.textureType = TextureImporterType.Default;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Bilinear;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }
    }

    private static Material CreateParticleMaterial(string assetPath, Texture2D texture, Color color, bool additive)
    {
        var material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
        if (material == null)
        {
            material = new Material(FindParticleShader());
            AssetDatabase.CreateAsset(material, assetPath);
        }

        material.name = Path.GetFileNameWithoutExtension(assetPath);
        ApplyMaterialSettings(material, texture, color, additive);
        EditorUtility.SetDirty(material);

        return material;
    }

    private static Shader FindParticleShader()
    {
        var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (shader != null)
        {
            return shader;
        }

        shader = Shader.Find("Particles/Standard Unlit");
        if (shader != null)
        {
            return shader;
        }

        shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader != null)
        {
            return shader;
        }

        return Shader.Find("Sprites/Default");
    }

    private static void ApplyMaterialSettings(Material material, Texture2D texture, Color color, bool additive)
    {
        if (material.HasProperty("_BaseMap"))
        {
            material.SetTexture("_BaseMap", texture);
        }
        else if (material.HasProperty("_MainTex"))
        {
            material.SetTexture("_MainTex", texture);
        }

        if (material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }
        else if (material.HasProperty("_Color"))
        {
            material.SetColor("_Color", color);
        }

        if (material.HasProperty("_Surface"))
        {
            material.SetFloat("_Surface", 1f);
        }

        if (material.HasProperty("_Blend"))
        {
            material.SetFloat("_Blend", additive ? 2f : 0f);
        }

        if (material.HasProperty("_BlendMode"))
        {
            material.SetFloat("_BlendMode", additive ? 2f : 0f);
        }

        if (material.HasProperty("_SrcBlend"))
        {
            material.SetFloat("_SrcBlend", additive ? (float)BlendMode.SrcAlpha : (float)BlendMode.SrcAlpha);
        }

        if (material.HasProperty("_DstBlend"))
        {
            material.SetFloat("_DstBlend", additive ? (float)BlendMode.One : (float)BlendMode.OneMinusSrcAlpha);
        }

        if (material.HasProperty("_ZWrite"))
        {
            material.SetFloat("_ZWrite", 0f);
        }

        material.renderQueue = (int)RenderQueue.Transparent;

        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        if (additive)
        {
            material.EnableKeyword("_ADDITIVE");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }
        else
        {
            material.DisableKeyword("_ADDITIVE");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }
    }

    private static void CreateExplosionPrefab(Material smokeMaterial, Material burstMaterial)
    {
        var root = new GameObject("EnemyExplosion");
        try
        {
            root.transform.position = Vector3.zero;

            var rootParticleSystem = root.AddComponent<ParticleSystem>();
            ConfigureRoot(rootParticleSystem);
            ConfigureRenderer(rootParticleSystem.GetComponent<ParticleSystemRenderer>(), null, 0);

            CreateFlash(root.transform, burstMaterial);
            CreateBurst(root.transform, burstMaterial);
            CreateSmoke(root.transform, smokeMaterial);

            PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
        }
        finally
        {
            Object.DestroyImmediate(root);
        }
    }

    private static void ConfigureRoot(ParticleSystem particleSystem)
    {
        var main = particleSystem.main;
        main.duration = 1.2f;
        main.loop = false;
        main.playOnAwake = true;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.stopAction = ParticleSystemStopAction.Destroy;
        main.maxParticles = 1;

        var emission = particleSystem.emission;
        emission.enabled = false;

        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.enabled = false;
    }

    private static void CreateFlash(Transform parent, Material burstMaterial)
    {
        var flash = new GameObject("Flash");
        flash.transform.SetParent(parent, false);

        var particleSystem = flash.AddComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.duration = 0.18f;
        main.loop = false;
        main.startLifetime = 0.15f;
        main.startSpeed = 0f;
        main.startSize = 1.25f;
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0.97f, 0.8f, 0.95f));
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.maxParticles = 2;

        var emission = particleSystem.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[]
        {
            new ParticleSystem.Burst(0f, 1)
        });

        var shape = particleSystem.shape;
        shape.enabled = false;

        var sizeOverLifetime = particleSystem.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(
            1f,
            new AnimationCurve(
                new Keyframe(0f, 0.2f),
                new Keyframe(0.35f, 1f),
                new Keyframe(1f, 1.65f)));

        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(
            BuildGradient(
                new[]
                {
                    new GradientColorKey(new Color(1f, 1f, 0.92f), 0f),
                    new GradientColorKey(new Color(1f, 0.8f, 0.35f), 0.55f),
                    new GradientColorKey(new Color(0.95f, 0.45f, 0.1f), 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0.9f, 0f),
                    new GradientAlphaKey(0.5f, 0.5f),
                    new GradientAlphaKey(0f, 1f)
                }));

        ConfigureRenderer(particleSystem.GetComponent<ParticleSystemRenderer>(), burstMaterial, 10);
    }

    private static void CreateBurst(Transform parent, Material burstMaterial)
    {
        var burst = new GameObject("Burst");
        burst.transform.SetParent(parent, false);

        var particleSystem = burst.AddComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.duration = 0.32f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.18f, 0.34f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(2.1f, 4.2f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.26f, 0.58f);
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
        main.startColor = Color.white;
        main.gravityModifier = 0f;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.maxParticles = 24;

        var emission = particleSystem.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[]
        {
            new ParticleSystem.Burst(0f, 14)
        });

        var shape = particleSystem.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.08f;
        shape.arcMode = ParticleSystemShapeMultiModeValue.Random;

        var limitVelocity = particleSystem.limitVelocityOverLifetime;
        limitVelocity.enabled = true;
        limitVelocity.dampen = 0.35f;

        var sizeOverLifetime = particleSystem.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(
            1f,
            new AnimationCurve(
                new Keyframe(0f, 0.75f),
                new Keyframe(0.55f, 1f),
                new Keyframe(1f, 0.15f)));

        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(
            BuildGradient(
                new[]
                {
                    new GradientColorKey(new Color(1f, 0.97f, 0.65f), 0f),
                    new GradientColorKey(new Color(1f, 0.67f, 0.15f), 0.45f),
                    new GradientColorKey(new Color(0.92f, 0.25f, 0.04f), 1f)
                },
                new[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0.7f, 0.65f),
                    new GradientAlphaKey(0f, 1f)
                }));

        var rotationOverLifetime = particleSystem.rotationOverLifetime;
        rotationOverLifetime.enabled = true;
        rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-180f, 180f);

        ConfigureRenderer(particleSystem.GetComponent<ParticleSystemRenderer>(), burstMaterial, 11);
    }

    private static void CreateSmoke(Transform parent, Material smokeMaterial)
    {
        var smoke = new GameObject("Smoke");
        smoke.transform.SetParent(parent, false);

        var particleSystem = smoke.AddComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.duration = 0.95f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.55f, 0.95f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.45f, 1.15f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.65f, 1.15f);
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
        main.startColor = Color.white;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.maxParticles = 18;

        var emission = particleSystem.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[]
        {
            new ParticleSystem.Burst(0.03f, 8)
        });

        var shape = particleSystem.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.12f;
        shape.arcMode = ParticleSystemShapeMultiModeValue.Random;

        var velocityOverLifetime = particleSystem.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(-0.12f, 0.12f);
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(-0.12f, 0.12f);
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f, 0f);

        var limitVelocity = particleSystem.limitVelocityOverLifetime;
        limitVelocity.enabled = true;
        limitVelocity.dampen = 0.65f;

        var sizeOverLifetime = particleSystem.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(
            1f,
            new AnimationCurve(
                new Keyframe(0f, 0.45f),
                new Keyframe(0.4f, 0.95f),
                new Keyframe(1f, 1.65f)));

        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(
            BuildGradient(
                new[]
                {
                    new GradientColorKey(new Color(0.42f, 0.42f, 0.42f), 0f),
                    new GradientColorKey(new Color(0.28f, 0.28f, 0.28f), 0.6f),
                    new GradientColorKey(new Color(0.18f, 0.18f, 0.18f), 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0.72f, 0f),
                    new GradientAlphaKey(0.42f, 0.4f),
                    new GradientAlphaKey(0f, 1f)
                }));

        var rotationOverLifetime = particleSystem.rotationOverLifetime;
        rotationOverLifetime.enabled = true;
        rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-45f, 45f);

        ConfigureRenderer(particleSystem.GetComponent<ParticleSystemRenderer>(), smokeMaterial, 9);
    }

    private static void ConfigureRenderer(ParticleSystemRenderer renderer, Material material, int sortingOrder)
    {
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.alignment = ParticleSystemRenderSpace.View;
        renderer.sortMode = ParticleSystemSortMode.Distance;
        renderer.minParticleSize = 0f;
        renderer.maxParticleSize = 5f;
        renderer.maskInteraction = SpriteMaskInteraction.None;
        renderer.sortingOrder = sortingOrder;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        if (material != null)
        {
            renderer.sharedMaterial = material;
        }
    }

    private static Gradient BuildGradient(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys)
    {
        var gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
        return gradient;
    }
}
