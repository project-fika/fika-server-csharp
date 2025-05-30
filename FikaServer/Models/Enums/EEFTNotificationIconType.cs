﻿using SPTarkov.Server.Core.Utils.Json.Converters;

namespace FikaServer.Models.Enums
{
    [EftEnumConverter]
    public enum EEFTNotificationIconType
    {
        Default = 0,
        Alert = 1,
        Friend = 2,
        Mail = 3,
        Note = 4,
        Quest = 5,
        /// <summary>
        /// This one is broken
        /// </summary>
        Achievement = 6,
        EntryPoint = 7,
        RagFair = 8,
        Hideout = 9,
        WishlistQuest = 10,
        WishlistHideout = 11,
        WishlistTrading = 12,
        WishlistEquipment = 13,
        WishlistOther = 14,
    }
}
