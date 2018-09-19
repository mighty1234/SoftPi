Create TABLE [IP](
Id int primary key identity, 
[Ip] varbinary(16),
Country nvarchar(256),
Company nvarchar(256)
)

Create table [File](
Id int primary key identity,
[Path] nvarchar(max),
[Name] nvarchar (256),
Size int 
)

Create table [Log](
Id int primary key identity,
Ip_id int ,
File_path_id int,
result int ,
requestType nvarchar(256),
requestTime nvarchar(256),
CONSTRAINT FK_Log_Ip FOREIGN KEY (Ip_id)     
    REFERENCES [IP](Id),
	CONSTRAINT FK_Log_File FOREIGN KEY (File_path_id)     
    REFERENCES [File](Id),

)