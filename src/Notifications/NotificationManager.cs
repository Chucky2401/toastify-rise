using System.Diagnostics;

using Microsoft.Windows.AppNotifications;

namespace ToastifyRise.Notifications {
    internal class NotificationManager {
        private bool m_isRegistered;

        public NotificationManager() {
            m_isRegistered = false;
        }

        ~NotificationManager() {
            Unregister();
        }

        public void Init() {
            AppNotificationManager notificationManager = AppNotificationManager.Default;
            notificationManager.NotificationInvoked += NotificationManager_NotificationInvoked;
            notificationManager.Register();
            m_isRegistered = true;
        }

        public void Unregister() {
            if (m_isRegistered) {
                AppNotificationManager.Default.Unregister();
                m_isRegistered = false;
            }
        }

        private void NotificationManager_NotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args) {
            HandleNotification(args);
        }

        private void HandleNotification(AppNotificationActivatedEventArgs args) {
            IDictionary<string, string> arguments = args.Arguments;
            
            //foreach (var arg in arguments) {
            //    Console.WriteLine(arg.ToString());
            //}

            if (arguments.ContainsKey("print")) {
                Console.WriteLine(arguments["print"]);
            }

            if (arguments.ContainsKey("url"))
                System.Diagnostics.Process.Start(new ProcessStartInfo() { FileName = arguments["url"], UseShellExecute = true });
        }
    }
}
