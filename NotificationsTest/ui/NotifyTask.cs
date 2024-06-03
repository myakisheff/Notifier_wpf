using Microsoft.Toolkit.Uwp.Notifications;

namespace Notifier.ui
{
    class NotifyTask
    {
        internal static void SimpleNotify(domain.Task selectedTask)
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText(selectedTask.label)
                .AddText(selectedTask.text)
                .Show();
        }

        internal static void ButtonsNotify(domain.Task selectedTask)
        {
            int conversationId = 384928;
            Random random = new();
            int id = random.Next(conversationId);

            // Construct the content
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", id)
                .AddHeader(selectedTask.id.ToString(), selectedTask.label, "label")
                .AddText(selectedTask.text)

                // Buttons
                .AddButton(new ToastButton()
                    .SetContent("Ok")
                    .AddArgument("action", "ok")
                    .SetBackgroundActivation())

                .AddButton(new ToastButton()
                    .SetContent("Later")
                    .AddArgument("action", "later")
                    .SetBackgroundActivation())
                .Show();
        }



        private static void BuildToast()
        {


        }
    }
}
