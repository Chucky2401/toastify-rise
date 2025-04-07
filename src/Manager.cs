using ToastifyRise.Notifications;

namespace toastifyRise {
    public class Manager {

        static void Main(string[] args) {
            Toaster();
        }

        static void Toaster() {
            Listener listener = new Listener();
            NotificationManager notificationManager = new NotificationManager();
            notificationManager.Init();

            // S'abonner à l'événement RequestReceived
            listener.RequestReceived += (results) => {
                string message = "", title = "", type = "info";
                bool persist = false;

                if (results.ContainsKey("message") && !String.IsNullOrEmpty(results["message"]))
                    message = results["message"];

                if (results.ContainsKey("title") && !String.IsNullOrEmpty(results["title"]))
                    title = results["title"];

                if (results.ContainsKey("type") && !String.IsNullOrEmpty(results["type"]))
                    type = results["type"];

                if (results.ContainsKey("persist") && !String.IsNullOrEmpty(results["persist"]))
                    persist = bool.Parse(results["persist"]);

                ToastWithAvatar.SendToast(title, message, type, persist);
            };


            // Démarrer l'écoute
            listener.StartListening();

            // Garder le programme en cours d'exécution
            Console.WriteLine("Listening for requests. Press Enter to stop.");
            Console.ReadLine();

            // Arrêter l'écoute
            listener.StopListening();
            notificationManager.Unregister();
        }
    }
}
