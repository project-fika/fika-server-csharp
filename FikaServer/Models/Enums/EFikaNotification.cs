﻿using SPTarkov.Server.Core.Utils.Json.Converters;

namespace FikaServer.Models.Enums
{
    [EftEnumConverter]
    public enum EFikaNotification
    {
        KeepAlive = 0,
        StartedRaid = 1,
        SentItem = 2,
        PushNotification = 3,
        OpenAdminSettings = 4,
        ShutdownClient = 5
    }
}
