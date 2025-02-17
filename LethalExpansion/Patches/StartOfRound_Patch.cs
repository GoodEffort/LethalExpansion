﻿using HarmonyLib;
using LethalExpansion.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LethalExpansion.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRound_Patch
    {
        [HarmonyPatch(nameof(StartOfRound.StartGame))]
        [HarmonyPostfix]
        public static void Awake_Postfix(StartOfRound __instance)
        {
            if (__instance.currentLevel.name.StartsWith("Assets/Mods/"))
            {
                SceneManager.LoadScene(__instance.currentLevel.name, LoadSceneMode.Additive);
            }
            LethalExpansion.Log.LogInfo("Game started.");
        }
        [HarmonyPatch("OnPlayerConnectedClientRpc")]
        [HarmonyPostfix]
        static void OnPlayerConnectedClientRpc_Postfix(StartOfRound __instance, ulong clientId)
        {
            if (!LethalExpansion.ishost)
            {
                LethalExpansion.ishost = false;
                LethalExpansion.sessionWaiting = false;
                LethalExpansion.Log.LogInfo("LethalExpansion Client Started." + __instance.NetworkManager.LocalClientId);
            }
            else
            {
                NetworkPacketManager.Instance.sendPacket(NetworkPacketManager.packetType.request, "clientinfo", string.Empty, (long)clientId);
            }
        }
        [HarmonyPatch(nameof(StartOfRound.SetMapScreenInfoToCurrentLevel))]
        [HarmonyPostfix]
        static void SetMapScreenInfoToCurrentLevel_Postfix(StartOfRound __instance)
        {
            AutoScrollText obj = __instance.screenLevelDescription.GetComponent<AutoScrollText>();
            if (obj != null)
            {
                obj.ResetScrolling();
            }
        }
    }
}
