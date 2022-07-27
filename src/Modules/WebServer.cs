using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomBot.Modules;

public class WebServer
{
    readonly StringBuilder _optimizer = new();
    readonly string _httpHeaders =
        "HTTP/1.1 200 OK\r\nContent-Type: application/json; charset=utf-8\r\nCache-Control: no-store\r\nX-Powered-By: Basic HTTP C# Server\r\nPragma: no-cache";
    readonly string _httpContent =
        "{\"server_status\": \"200 OK\"}";
    readonly TcpListener _requestListener;
    /// <summary>
    /// Starts the HTTP Server.
    /// </summary>
    /// <returns>The <see cref="Thread"/> running the server</returns>
    public Thread StartServer()
    {
        _optimizer
            .Append(_httpHeaders)
            .Append("\r\n\r\n")
            .Append(_httpContent);

        byte[] cachedResponse = Encoding.ASCII.GetBytes(_optimizer.ToString());

        Thread serverThread = new(
        async () =>
        {
            while (true)
            {
                try
                {
                    while (true)
                    {
                        _requestListener.Start();
                        Socket response = await _requestListener.AcceptSocketAsync();
                        response.Send(cachedResponse);
                        await response.DisconnectAsync(false);
                        response.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Http Server Error Encountered! -> \r\n\r\n{0}\r\n\r\nResuming Http Server Thread Execution...", ex));
                }
            }
        })
        {
            Name = "Http Server"
        };
        serverThread.Start();
        return serverThread;
    }
    public WebServer(WebServerConfig configurations, string? HttpHeaders, string? HttpContent)
    {
        _requestListener = new(configurations.ListenTo, configurations.Port);

        // Optional Parameters.
        if (HttpHeaders is not null)
        {
            _httpHeaders = HttpHeaders;
        }
        if (HttpContent is not null)
        {
            _httpContent = HttpContent;
        }
    }
    public WebServer(WebServerConfig configurations)
    {
        _requestListener = new(configurations.ListenTo, configurations.Port);
    }
}

public ref struct WebServerConfig
{
    public ushort Port { get; } = 80;
    public IPAddress ListenTo { get; } = IPAddress.Any;

    public WebServerConfig(ushort Port, IPAddress targetListener)
    {
        if (Port is not 80)
            this.Port = Port;

        if (targetListener != IPAddress.Any)
            this.ListenTo = targetListener;
    }
    public WebServerConfig()
    {
        Port = 80;
        ListenTo = IPAddress.Any;
    }
}