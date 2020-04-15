echo off
echo "Start Translate......"
.\protoc.exe -I="." --csharp_out=..\Assets\Scripts\DataStore\ .\YiQuJiangHu.proto
.\protoc -I="." --cpp_out="." .\YiQuJiangHu.proto
echo "Translate Complete......"
pause