﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace toastifyRise {
    public class Listener {

        private string schema;
        private string listeningAddress;
        private Int32 listeningPort;
        private HttpListener server;
        private Thread listenerThread;
        private bool isListening;

        public event Action<Dictionary<string, string>> RequestReceived;

        public Listener() {
            schema = "http";
            listeningAddress = "*";
            listeningPort = 13000;
            server = new HttpListener();
            isListening = false;
        }

        public void StartListening() {
            if (isListening) return;

            server.Prefixes.Add($"{schema}://{listeningAddress}:{listeningPort}/");
            server.Start();
            isListening = true;

            listenerThread = new Thread(ListenForRequests);
            listenerThread.Start();
        }

        private void ListenForRequests() {
            while (isListening) {
                try {
                    HttpListenerContext context = server.GetContext();
                    Task.Run(() => HandleRequest(context));
                } catch (HttpListenerException) {
                    // Handle exceptions, possibly log them
                }
            }
        }

        private void HandleRequest(HttpListenerContext context) {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            Dictionary<string, string> postParams = new Dictionary<string, string>();

            using (Stream body = request.InputStream) {
                using (StreamReader reader = new StreamReader(body, request.ContentEncoding)) {
                    string rawData = reader.ReadToEnd();

                    string[] rawParams = rawData.Split('&');

                    foreach (string param in rawParams) {
                        string[] kvPair = param.Split('=');
                        if (kvPair.Length == 2) {
                            string key = kvPair[0];
                            string value = HttpUtility.UrlDecode(kvPair[1]);
                            postParams.Add(key, value);
                        }
                    }
                }
            }

            string responseString = "OK";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            using (Stream output = response.OutputStream) {
                output.Write(buffer, 0, buffer.Length);
            }

            RequestReceived?.Invoke(postParams);
        }

        public void StopListening() {
            isListening = false;
            server.Stop();
            listenerThread.Join();
        }
    }
}
