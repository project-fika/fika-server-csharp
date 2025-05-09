﻿using FikaServer.Models.Enums;
using FikaServer.Models.Fika.Presence;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Eft.Profile;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Utils;
using System.Collections.Concurrent;

namespace FikaServer.Services
{
    [Injectable(InjectionType.Singleton)]
    public class PresenceService(SaveServer saveServer, TimeUtil timeUtil, ISptLogger<PresenceService> logger)
    {
        private ConcurrentDictionary<string, FikaPlayerPresence> onlinePlayers = [];

        public void AddPlayerPresence(string sessionID)
        {
            SptProfile profile = saveServer.GetProfile(sessionID);

            if (profile == null)
            {
                return;
            }

            FikaPlayerPresence data = new()
            {
                Nickname = profile.CharacterData.PmcData.Info.Nickname,
                Level = profile.CharacterData.PmcData.Info.Level ?? 0,
                Activity = EFikaPlayerPresences.IN_MENU,
                ActivityStartedTimestamp = timeUtil.GetTimeStamp()
            };

            logger.Debug($"[Fika Presence] Adding player: {data.Nickname}");

            onlinePlayers.TryAdd(sessionID, data);
        }

        public List<FikaPlayerPresence> GetAllPlayersPresence()
        {
            return onlinePlayers.Values.ToList();
        }

        public void UpdatePlayerPresence(string sessionID, FikaSetPresence NewPresence)
        {
            if (!onlinePlayers.TryGetValue(sessionID, out FikaPlayerPresence currentPresence))
            {
                return;
            }

            SptProfile profile = saveServer.GetProfile(sessionID);

            onlinePlayers.TryUpdate(sessionID, new FikaPlayerPresence
            {
                Nickname = profile.CharacterData.PmcData.Info.Nickname,
                Level = profile.CharacterData.PmcData.Info.Level ?? 0,
                Activity = NewPresence.Activity,
                ActivityStartedTimestamp = timeUtil.GetTimeStamp(),
                RaidInformation = NewPresence.RaidInformation
            }, currentPresence);
        }

        public void RemovePlayerPresence(string sessionID)
        {
            if (!onlinePlayers.ContainsKey(sessionID))
            {
                return;
            }

            onlinePlayers.TryRemove(sessionID, out _);
        }
    }
}
