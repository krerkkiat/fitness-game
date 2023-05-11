// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System.Text;

Console.WriteLine("Begin");

var message = "Hello World";
var message_bytes = Encoding.UTF8.GetBytes(message);

uint payload_length = (uint) message_bytes.Length;
Console.WriteLine($"Payload Length: {payload_length}");
// BitConverter returns little endian.
byte[] length_bytes = BitConverter.GetBytes(payload_length).Reverse().ToArray();

IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.85"), 5051);
Socket socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(ipEndPoint);

Console.WriteLine("Connected");

socket.Send(length_bytes, SocketFlags.None);
socket.Send(message_bytes, SocketFlags.None);

Console.WriteLine("Sent!");

// Reading back the reply.
byte[] length_buffer = new byte[4];
var received = socket.Receive(length_buffer, SocketFlags.None);
uint data_length = BitConverter.ToUInt32(length_buffer.Reverse().ToArray(), 0);
Console.WriteLine($"Received data length: {data_length}");
Console.WriteLine($"Raw data length:{BitConverter.ToString(length_buffer)}");


byte[] payload_buffer = new byte[data_length];
var received2 = socket.Receive(payload_buffer);
Console.WriteLine(BitConverter.ToString(payload_buffer));

var response = Encoding.UTF8.GetString(payload_buffer, 0, received2);
Console.WriteLine($"Got: {response}");


socket.Shutdown(SocketShutdown.Both);
socket.Disconnect(false);
socket.Dispose();

Console.WriteLine("Bye");