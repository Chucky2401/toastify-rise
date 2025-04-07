using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;

namespace toastifyRise {
    internal class Toast {
        public static void Toasting(string message, string? title = null, string type = "info", bool persist = false, double expiration = 86400) {
            ToastContentBuilder toast = new ToastContentBuilder();

            Dictionary<string, string> image = new Dictionary<string, string>() {
                    { "info"    , "img/info.png" },
                    { "success" , "img/success.png" },
                    { "warning" , "img/warn.png" },
                    { "failure" , "img/error.png" }
                };

            if (!image.ContainsKey(type))
                type = "info";

            string icon = Path.GetFullPath(image[type]);

            toast.AddText(title);
            toast.AddText(message);
            toast.AddAppLogoOverride(new Uri(icon));

            if (persist) {
                toast.SetToastScenario(ToastScenario.Reminder);
                toast.AddButton(new ToastButton().SetContent("OK").SetDismissActivation());
            }

            toast.Show(toast => {
                toast.ExpirationTime = DateTime.Now.AddSeconds(expiration);
            });
        }
    }
}
