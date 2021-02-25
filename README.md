# VAT

Project created using .NET 5 

----
#### Installation
- To get database ready, run commands below in your console
> `C:\fakePath\User>sqlcmd`  
> `1> CREATE DATABASE VATDB`  
> `2> GO`  
> `3> USE VATDB`  
> `4> GO`  
> `5> CREATE LOGIN VATDBUser WITH PASSWORD = 'VATDBUserPassword';`  
> `6> CREATE USER VATDBUser FOR LOGIN VATDBUser`  
> `7> GO`  
> `8> EXEC sp_addrolemember N'db_datareader', N'VATDBUser'`  
> `9> EXEC sp_addrolemember N'db_datawriter', N'VATDBUser'`  
> `10> EXEC sp_addrolemember N'db_ddladmin', N'VATDBUser'`  
> `11> GO`  
  
   
- To prepare backend environment you have to write in console:
> `update-database`