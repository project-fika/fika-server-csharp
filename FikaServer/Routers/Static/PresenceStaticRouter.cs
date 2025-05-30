﻿using FikaServer.Callbacks;
using FikaServer.Models.Fika.Presence;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Utils;

namespace FikaServer.Routers.Static
{
    [Injectable]
    public class PresenceStaticRouter(PresenceCallbacks fikaPresenceCallbacks, JsonUtil jsonUtil) : StaticRouter(jsonUtil, [
            new RouteAction(
                "/fika/presence/get",
                (
                    url,
                    info,
                    sessionId,
                    output
                ) => fikaPresenceCallbacks.HandleGetPresence(url, info, sessionId)),
            new RouteAction(
                "/fika/presence/set",
                (
                    url,
                    info,
                    sessionId,
                    output
                ) => fikaPresenceCallbacks.HandleSetPresence(url, info as FikaSetPresence, sessionId),
                typeof(FikaSetPresence)
                ),
            new RouteAction(
                "/fika/presence/setget",
                (
                    url,
                    info,
                    sessionId,
                    output
                ) => fikaPresenceCallbacks.HandleSetGetPresence(url, info as FikaSetPresence, sessionId),
                typeof(FikaSetPresence)
                ),
        ])
    {
    }
}
