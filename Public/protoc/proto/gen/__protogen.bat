@echo off

cd ..\
protobuf-net\ProtoGen\protogen.exe ^
-i:common.proto ^
-i:person.proto ^
-o:PBMessage\PBMessage.cs -ns:PBMessage
cd gen
