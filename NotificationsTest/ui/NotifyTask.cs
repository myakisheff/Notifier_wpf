using Microsoft.Toolkit.Uwp.Notifications;

namespace Notifier.ui
{
    class NotifyTask
    {
        internal static int Notify(domain.model.Task? selectedTask)
        {
            if (selectedTask == null)
            {
                return -1;
            }

            int id = selectedTask.ID;

            // Construct the content
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", id)
                .AddHeader(selectedTask.ID.ToString(), selectedTask.TaskTitle, "label")
                .AddText(selectedTask.TaskDescription)
                .AddText(selectedTask.TaskTargetDate)
                .Show();

            return id;
        }
    }
}
