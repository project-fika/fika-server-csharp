﻿using SPTarkov.Server.Core.Utils.Json.Converters;

namespace FikaServer.Models.Enums
{
    [EftEnumConverter]
    public enum EFikaPlayerPresences
    {
        IN_MENU = 0,
        IN_RAID = 1,
        IN_STASH = 2,
        IN_HIDEOUT = 3,
        IN_FLEA = 4,
    }
}
