using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat
{
    public partial class ChatInputBarView : ContentView
    {
        public ChatInputBarView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.SetBinding(HeightRequestProperty, new Binding("Height", BindingMode.OneWay, null, null, null, chatTextInput));
            }
        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            chatTextInput.Text = "";
        }
    }
}
