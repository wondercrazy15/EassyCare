using System;
using EasyCare.Models.Chat;
using EasyCare.Views.Dashboard.Chat.Cells;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as ChatMessage;
            if (messageVm == null)
                return null;

            return (messageVm.user == Application.Current.Properties["Email"].ToString()) ? incomingDataTemplate :outgoingDataTemplate ;
        }

    }
}
