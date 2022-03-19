using System;
using System.IO;

namespace PacketGenerator
{
    class Program
    {
        static string clientRegisterText;
        static string serverRegisterText;

        static void Main(string[] args)
        {
            // Set proto file
            string protoFile = "../../../Common/protoc-3.19.4-win64/bin/Protocol.proto";
            if (args.Length >= 1)
                protoFile = args[0];

            bool startParsing = false;
            foreach (string line in File.ReadAllLines(protoFile))
            {
                if (!startParsing && line.Contains("enum MsgId"))
                {
                    startParsing = true;
                    continue;
                }

                if (!startParsing)
                    continue;

                if (line.Contains("}"))
                    break;

                string name = line.Trim().Split(" =")[0];
                if (name.StartsWith("S_"))
                {
                    string[] words = name.Split("_");

                    string msgName = "";
                    foreach (string word in words)
                        msgName += FirstCharToUpper(word);

                    string packetName = $"S_{msgName.Substring(1)}";
                    clientRegisterText += string.Format(PacketFormat.managerRegisterFormat, msgName, packetName);
                }
                else if (name.StartsWith("C_"))
                {
                    string[] words = name.Split("_");

                    string msgName = "";
                    foreach (string word in words)
                        msgName += FirstCharToUpper(word);

                    string packetName = $"C_{msgName.Substring(1)}";
                    serverRegisterText += string.Format(PacketFormat.managerRegisterFormat, msgName, packetName);
                }
            }

            string clientManagerText = string.Format(PacketFormat.managerFormat, clientRegisterText);
            string serverManagerText = string.Format(PacketFormat.managerFormat, serverRegisterText);
            File.WriteAllText("ClientPacketManager.cs", clientManagerText);
            File.WriteAllText("ServerPacketManager.cs", serverManagerText);
        }

        public static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return input[0].ToString().ToUpper() + input.Substring(1).ToLower();
        }
    }
}
