﻿using FikaServer.Services;
using HarmonyLib;
using SPTarkov.Server.Core.Context;
using SPTarkov.Server.Core.Models.Eft.Match;

namespace FikaServer.Overrides.Services
{
    [HarmonyPatch(typeof(SPTarkov.Server.Core.Services.LocationLifecycleService))]
    [HarmonyPatch(nameof(SPTarkov.Server.Core.Services.LocationLifecycleService.EndLocalRaid))]
    public class EndLocalRaidOverride
    {
        public static bool Prefix(string sessionId, EndLocalRaidRequestData request)
        {
            ServiceProvider? sp = ApplicationContext.GetInstance()?.GetLatestValue(ContextVariableType.SERVICE_PROVIDER)?.GetValue<ServiceProvider>();
            MatchService matchService = sp?.GetService<MatchService>();
            InsuranceService insuranceService = sp?.GetService<InsuranceService>();

            // Get match id from player session id
            var matchId = matchService.GetMatchIdByPlayer(sessionId);

            // Find player that exited the raid
            var player = matchService.GetPlayerInMatch(matchId, sessionId);

            if (player is not null)
            {
                insuranceService.OnEndLocalRaidRequest(sessionId, insuranceService.GetMatchId(sessionId), request);

                // If the player is not a spectator, continue running EndLocalRaid
                if (!player.IsSpectator)
                {
                    return true;
                }
            }

            // Stop running the method if the player is a spectator
            return false;
        }
    }
}
