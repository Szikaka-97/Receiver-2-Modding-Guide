using BepInEx;
using Receiver2;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ExamplePlugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static int gun_model = 1004;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Harmony.CreateAndPatchAll(this.GetType());
        }

        [HarmonyPatch(typeof(ReceiverCoreScript), "Awake")]
        [HarmonyPostfix]
        private static void PatchCoreAwake(ref ReceiverCoreScript __instance, ref GameObject[] ___gun_prefabs_all) {
            GunScript mod = null;

            mod = ___gun_prefabs_all.Single( gameObject => {
                return ((int) gameObject.GetComponent<GunScript>().gun_model == gun_model);
            }).GetComponent<GunScript>();

            __instance.generic_prefabs = new List<InventoryItem>( __instance.generic_prefabs ) { mod }.ToArray();
        }

    }
}

