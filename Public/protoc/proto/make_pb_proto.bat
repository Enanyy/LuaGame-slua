for %%i in (*.proto) do (  
	
	..\protoc --descriptor_set_out=pb.txt %%i
) 
pause
