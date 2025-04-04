using System.Net;

namespace toastifyRise {
    internal class Manager {
        static void Main(string[] args) {
            Toaster();
        }

        public static void Toaster() {
            Listener listener = new Listener();

            // S'abonner à l'événement RequestReceived
            listener.RequestReceived += (results) => {
                string message = "", type = "info";
                string? title = null;
                bool persist = false;

                if (results.ContainsKey("message") && !String.IsNullOrEmpty(results["message"]))
                    message = results["message"];

                if (results.ContainsKey("title") && !String.IsNullOrEmpty(results["title"]))
                    title = results["title"];

                if (results.ContainsKey("type") && !String.IsNullOrEmpty(results["type"]))
                    type = results["type"];

                if (results.ContainsKey("persist") && !String.IsNullOrEmpty(results["persist"]))
                    persist = bool.Parse(results["persist"]);

                Toast.Toasting(message, title, type, persist);
            };

            // Démarrer l'écoute
            listener.StartListening();

            // Garder le programme en cours d'exécution
            Console.WriteLine("Listening for requests. Press Enter to stop.");
            Console.ReadLine();

            // Arrêter l'écoute
            listener.StopListening();
        }
    }
}
