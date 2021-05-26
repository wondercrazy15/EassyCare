using System;
using System.Collections.Generic;
using EasyCare.Models.Chat;
using EasyCare.ViewModels.Dashboard.Calendar;

namespace EasyCare.Interface
{
    public interface ITwilioChatHelper
    {
        void GetToken();
        void CreateClient(string chatToken, string email);
        void CreateChannel(string friendlyName);
        void SendMessage(string Msg, string id, string Groupid);
        void SendMediaMessage(string url, bool IsMedia, string id, string Groupid);
        void SendCalenderEvent(CalandarAgendaModel Msg, bool IsMedia, string id, string Groupid);
        void RemoveMember(string friendlyName, string Useridentity);
    }
}
