for %%i in (*.proto) do (  
	
	..\protoc --descriptor_set_out=person.txt %%i
) 
pause
