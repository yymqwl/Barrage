@echo off

for /f "delims=" %%i in ('dir /b proto "Proto/*.proto"') do protoc.exe  --csharp_out="./Cs" --proto_path="./Proto/" %%i
echo GenFinish...
xcopy /s/e/f/y "Cs/*.cs"  "../../../BarrageClient/Assets/Src/GameMain/Message"
echo CopyFinish..
pause