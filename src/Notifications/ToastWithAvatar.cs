using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;

namespace ToastifyRise.Notifications {
    internal class ToastWithAvatar {
        public const int ScenarioId = 1;
        public const string ScenarioName = "Toast with Avatar";

        public static bool SendToast(string title, string message, string type, bool persist) {
            Dictionary<string, string> image = new Dictionary<string, string>() {
                    { "info"    , "img/info.png" },
                    { "success" , "img/success.png" },
                    { "warning" , "img/warn.png" },
                    { "failure" , "img/error.png" }
                };

            if (!image.ContainsKey(type))
                type = "info";

            string icon = Path.GetFullPath(image[type]);

            var appNotification = new AppNotificationBuilder()
                .SetAppLogoOverride(new Uri("file://" + icon), AppNotificationImageCrop.Circle)
                .AddText(title)
                .AddText(message);

            if (type == "failure" || persist) {
                appNotification.SetScenario(AppNotificationScenario.Reminder);
                appNotification.AddButton(new AppNotificationButton("OK"));
                appNotification.SetDuration(AppNotificationDuration.Long);
            }

            /*
             * TODO: Custom Button from Apprise
             */
            //appNotification.AddButton(new AppNotificationButton("OK")
            //        .AddArgument("url", "https://hotline.sterimed.fr"));

            AppNotification notification = appNotification.BuildNotification();
            //Console.WriteLine(notification.Payload);

            AppNotificationManager.Default.Show(notification);

            return notification.Id != 0;
        }
    }
}
