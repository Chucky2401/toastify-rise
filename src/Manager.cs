using System.Net;

namespace toastifyRise {
    internal class Manager {
        static void Main(string[] args) {
            //Thread thread = new(new ThreadStart(Toaster));
            //thread.Start();

            //while (true) {
            //    Thread.Sleep(60000);

            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //}

            Toaster();
        }

        //static void Toaster() {
        //    Listener listener = new Listener();
        //    Dictionary<string, string>? results = new Dictionary<string, string>();

        //    while (true) {
        //        string message = "", type = "info";
        //        string? title = null;

        //        Console.WriteLine("Listening...");
        //        results = listener.Listen();

        //        Console.WriteLine("Data received:");
        //        for (int i = 0; i < results.Count; i++) {
        //            Console.WriteLine(results.Keys.ElementAt(i) + ": " + results.Values.ElementAt(i));
        //        }

        //        if (results.ContainsKey("message") && !String.IsNullOrEmpty(results["message"]))
        //            message = results["message"];

        //        if (results.ContainsKey("title") && !String.IsNullOrEmpty(results["title"]))
        //            title = results["title"];

        //        if (results.ContainsKey("type") && !String.IsNullOrEmpty(results["type"]))
        //            type = results["type"];

        //        Toast.Toasting(message, title, type);

        //        Console.WriteLine("");

        //        results = null;

        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //}

        public static void Toaster() {
            Listener listener = new Listener();

            // S'abonner à l'événement RequestReceived
            listener.RequestReceived += (results) => {
                string message = "", type = "info";
                string? title = null;
                bool persist = false;

                // Console.WriteLine("Data received:");
                // foreach (var kvp in results) {
                //     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                // }

                if (results.ContainsKey("message") && !String.IsNullOrEmpty(results["message"]))
                    message = results["message"];

                if (results.ContainsKey("title") && !String.IsNullOrEmpty(results["title"]))
                    title = results["title"];

                if (results.ContainsKey("type") && !String.IsNullOrEmpty(results["type"]))
                    type = results["type"];

                if (results.ContainsKey("persist") && !String.IsNullOrEmpty(results["persist"]))
                    persist = bool.Parse(results["persist"]);

                Toast.Toasting(message, title, type, persist);

                // Console.WriteLine("");
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
