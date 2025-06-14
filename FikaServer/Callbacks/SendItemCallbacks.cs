﻿using FikaServer.Controllers;
using FikaServer.Models.Fika.SendItem;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.ItemEvent;
using SPTarkov.Server.Core.Utils;

namespace FikaServer.Callbacks
{
    [Injectable]
    public class SendItemCallbacks(HttpResponseUtil httpResponseUtil, SendItemController sendItemController)
    {
        public async ValueTask<ItemEventRouterResponse> HandleSendItem(PmcData pmcData, SendItemRequestData body, string sessionID)
        {
            return await sendItemController.SendItem(pmcData, body, sessionID);
        }

        /// <summary>
        /// Handle /fika/senditem/availablereceivers
        /// </summary>
        public ValueTask<string> HandleAvailableReceivers(string sessionID)
        {
            return new ValueTask<string>(httpResponseUtil.NoBody(sendItemController.HandleAvailableReceivers(sessionID)));
        }
    }
}
