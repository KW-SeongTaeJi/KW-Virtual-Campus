protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 
IF ERRORLEVEL 1 PAUSE

START ../../../Server/PacketGenerator/bin/Debug/netcoreapp3.1/PacketGenerator.exe ./Protocol.proto
XCOPY /Y Protocol.cs "../../../Server/Server/Packet"
XCOPY /Y Protocol.cs "../../../Client/Assets/Scripts/Network/Packet"
XCOPY /Y ServerPacketManager.cs "../../../Server/Server/Packet"
XCOPY /Y ClientPacketManager.cs "../../../Client/Assets/Scripts/Network/Packet"