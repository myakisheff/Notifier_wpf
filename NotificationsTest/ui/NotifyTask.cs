using Microsoft.Toolkit.Uwp.Notifications;

namespace Notifier.ui
{
    class NotifyTask
    {
        internal static void SimpleNotify(domain.model.Task selectedTask)
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText(selectedTask.TaskTitle)
                .AddText(selectedTask.TaskDescription)
                .Show();
        }

        internal static void ButtonsNotify(domain.model.Task? selectedTask)
        {
            if (selectedTask == null)
            {
                return;
            }

            int conversationId = 384928;
            Random random = new();
            int id = random.Next(conversationId);

            // Construct the content
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", id)
                .AddHeader(selectedTask.ID.ToString(), selectedTask.TaskTitle, "label")
                .AddText(selectedTask.TaskDescription)

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
    }
}
