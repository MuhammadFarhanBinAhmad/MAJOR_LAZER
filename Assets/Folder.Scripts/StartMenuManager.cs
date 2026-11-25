using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class NatureKitSceneBuilder
{
    // ---------- Settings you can tweak ----------
    const string SearchFolder   = "Assets";                       // narrow to e.g. "Assets/Kenney" if you like
    const string ScenePath      = "Assets/Scenes/NatureKit_Scene.unity";
    const int    Seed           = 12345;
    const int    GroundTiles    = 30;      // ground is GroundTiles x GroundTiles
    const float  ClearingRadius = 4f;      // empty campfire area in the middle
    const bool   AddToBuildSettings = true;

    static readonly string[] ModelExtensions = { ".fbx", ".prefab", ".obj", ".gltf", ".glb" };

    static Dictionary<string, GameObject> library;
    static List<Vector3> placed = new List<Vector3>();

    [MenuItem("Tools/Nature Kit/Build Scene")]
    public static void Build()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        BuildLibrary();
        if (library.Count == 0)
        {
            EditorUtility.DisplayDialog("Nature Kit", "No models found under '" + SearchFolder + "'. Import the Kenney Nature Kit first.", "OK");
            return;
        }

        Random.InitState(Seed);
        placed.Clear();

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        scene.name = "NatureKit_Scene";

        var root       = new GameObject("NatureKit").transform;
        var groundRoot = Child(root, "Ground");
        var forestRoot = Child(root, "Trees");
        var rockRoot   = Child(root, "Rocks");
        var plantRoot  = Child(root, "Plants");
        var campRoot   = Child(root, "Camp");

        float size = BuildGround(groundRoot);
        float half = size * 0.5f - 1.5f;

        BuildCamp(campRoot);

        // --- Scatter ---
        var trees     = Pick("tree_", "stump");
        var rocks     = Pick("rock_");
        var stones    = Pick("stone_");
        var bushes    = Pick("plant_bush", "");
        var flowers   = Pick("flower_");
        var mushrooms = Pick("mushroom_");
        var grass     = Pick("grass");
        var logs      = Pick("log");
        var stumps    = Pick("stump_");

        Scatter(trees,     180, 1.4f, ClearingRadius + 1f, half, 0.9f, 1.4f, forestRoot);
        Scatter(rocks,      35, 1.2f, ClearingRadius + 1f, half, 0.8f, 1.6f, rockRoot);
        Scatter(stones,     40, 0.6f, ClearingRadius,      half, 0.8f, 1.3f, rockRoot);
        Scatter(bushes,     45, 0.8f, ClearingRadius,      half, 0.9f, 1.3f, plantRoot);
        Scatter(flowers,    60, 0.5f, ClearingRadius - 1f, half, 0.9f, 1.2f, plantRoot);
        Scatter(mushrooms,  25, 0.5f, ClearingRadius,      half, 0.9f, 1.2f, plantRoot);
        Scatter(grass,      90, 0.4f, ClearingRadius - 1f, half, 0.9f, 1.3f, plantRoot);
        Scatter(logs,       10, 1.0f, ClearingRadius + 1f, half, 0.9f, 1.2f, forestRoot);
        Scatter(stumps,     10, 1.0f, ClearingRadius + 1f, half, 0.9f, 1.2f, forestRoot);

        // --- Spawn point + camera ---
        var spawn = new GameObject("PlayerSpawn");
        spawn.transform.position = new Vector3(0f, 0f, -ClearingRadius + 0.5f);

        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(0f, 6f, -12f);
            cam.transform.LookAt(Vector3.zero);
        }

        // --- Save ---
        Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
        EditorSceneManager.SaveScene(scene, ScenePath);

        if (AddToBuildSettings) AddSceneToBuildSettings(ScenePath);

        AssetDatabase.Refresh();
        Debug.Log("Nature Kit scene created at " + ScenePath);
    }

    // ---------- Asset lookup ----------
    static void BuildLibrary()
    {
        library = new Dictionary<string, GameObject>();
        foreach (var guid in AssetDatabase.FindAssets("t:GameObject", new[] { SearchFolder }))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (!ModelExtensions.Contains(ext)) continue;

            var name = Path.GetFileNameWithoutExtension(path);
            if (library.ContainsKey(name)) continue;           // avoid duplicates if several formats imported

            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go != null) library[name] = go;
        }
    }

    static List<GameObject> Pick(string prefix, string excludeContains = "")
    {
        var result = library
            .Where(kv => kv.Key.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase)
                      && (string.IsNullOrEmpty(excludeContains) || !kv.Key.ToLowerInvariant().Contains(excludeContains)))
            .Select(kv => kv.Value)
            .ToList();
        if (result.Count == 0) Debug.LogWarning("Nature Kit: no models found for prefix '" + prefix + "' (skipped).");
        return result;
    }

    static GameObject Spawn(GameObject prefab, Transform parent, Vector3 pos, float yRot, float scale)
    {
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        go.transform.SetParent(parent, false);
        go.transform.position = pos;
        go.transform.rotation = Quaternion.Euler(0f, yRot, 0f);
        go.transform.localScale = Vector3.one * scale;
        GameObjectUtility.SetStaticEditorFlags(go, StaticEditorFlags.BatchingStatic | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OccludeeStatic);
        return go;
    }

    static Transform Child(Transform parent, string name)
    {
        var t = new GameObject(name).transform;
        t.SetParent(parent, false);
        return t;
    }

    // ---------- Ground ----------
    static float BuildGround(Transform parent)
    {
        var tiles = Pick("ground_grass");
        GameObject tile = tiles.FirstOrDefault(t => t.name == "ground_grass") ?? tiles.FirstOrDefault();

        if (tile == null)
        {
            // Fallback: simple plane so the scene is still usable
            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = "GroundPlane";
            plane.transform.SetParent(parent, false);
            plane.transform.localScale = new Vector3(GroundTiles / 10f, 1f, GroundTiles / 10f);
            return GroundTiles;
        }

        // Measure the tile so we can grid them without gaps
        var temp = (GameObject)PrefabUtility.InstantiatePrefab(tile);
        var rends = temp.GetComponentsInChildren<Renderer>();
        float tileSize = 1f;
        if (rends.Length > 0)
        {
            var b = rends[0].bounds;
            foreach (var r in rends) b.Encapsulate(r.bounds);
            tileSize = Mathf.Max(b.size.x, b.size.z);
            if (tileSize < 0.01f) tileSize = 1f;
        }
        Object.DestroyImmediate(temp);

        float offset = (GroundTiles - 1) * tileSize * 0.5f;
        for (int x = 0; x < GroundTiles; x++)
            for (int z = 0; z < GroundTiles; z++)
                Spawn(tile, parent, new Vector3(x * tileSize - offset, 0f, z * tileSize - offset), 0f, 1f);

        return GroundTiles * tileSize;
    }

    // ---------- Camp ----------
    static void BuildCamp(Transform parent)
    {
        var fire = Pick("campfire").FirstOrDefault();
        if (fire != null) Spawn(fire, parent, Vector3.zero, 0f, 1f);

        var tent = Pick("tent_detailedClosed").FirstOrDefault() ?? Pick("tent").FirstOrDefault();
        if (tent != null) Spawn(tent, parent, new Vector3(2.2f, 0f, 1.2f), -35f, 1f);

        var logs = Pick("log");
        if (logs.Count > 0)
        {
            Spawn(logs[0], parent, new Vector3(-1.4f, 0f, 0.4f), 80f, 1f);
            Spawn(logs[Mathf.Min(1, logs.Count - 1)], parent, new Vector3(0.6f, 0f, -1.5f), 10f, 1f);
        }
    }

    // ---------- Scatter ----------
    static void Scatter(List<GameObject> prefabs, int count, float minDist,
                        float minRadius, float maxRadius, float minScale, float maxScale, Transform parent)
    {
        if (prefabs == null || prefabs.Count == 0) return;

        int attempts = 0, spawned = 0;
        while (spawned < count && attempts < count * 20)
        {
            attempts++;
            var p = new Vector3(Random.Range(-maxRadius, maxRadius), 0f, Random.Range(-maxRadius, maxRadius));
            if (p.magnitude < minRadius) continue;
            if (placed.Any(o => (o - p).sqrMagnitude < minDist * minDist)) continue;

            var prefab = prefabs[Random.Range(0, prefabs.Count)];
            Spawn(prefab, parent, p, Random.Range(0f, 360f), Random.Range(minScale, maxScale));
            placed.Add(p);
            spawned++;
        }
    }

    // ---------- Build settings ----------
    static void AddSceneToBuildSettings(string path)
    {
        var scenes = EditorBuildSettings.scenes.ToList();
        if (scenes.Any(s => s.path == path)) return;
        scenes.Add(new EditorBuildSettingsScene(path, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }
}