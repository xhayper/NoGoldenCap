using System.Reflection;
using HarmonyLib;
using BepInEx;

namespace NoGoldenCap;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
[HarmonyPatch]
public class Plugin: BaseUnityPlugin
{
    private const string PLUGIN_GUID = "io.github.xhayper.NoGoldenCap";
    private const string PLUGIN_NAME = "NoGoldenCap";
    private const string PLUGIN_VERSION = "1.0.0";

    private static readonly Harmony Harmony = new(PLUGIN_GUID);

    private void OnEnable()
    {
        Harmony.PatchAll(Assembly.GetExecutingAssembly());
        Logger.LogInfo("Have fun one shotting the boss :)");
    }

    private void OnDisable()
    {
        Harmony.UnpatchSelf();
    }
    
    [HarmonyPatch(typeof(PlayerFleeceManager), nameof(PlayerFleeceManager.IncrementDamageModifier))]
    [HarmonyPrefix]
    public static bool PlayerFleeceManager_IncrementDamageModifierPrefix()
    {
        if (DataManager.Instance.PlayerFleece != 1) return false;
        PlayerFleeceManager.damageMultiplier += 0.1f;
        PlayerFleeceManager.OnDamageMultiplierModified?.Invoke(PlayerFleeceManager.damageMultiplier);
        return false;
    }
}