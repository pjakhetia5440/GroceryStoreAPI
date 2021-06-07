Tools 
    Visual Studio 2019 
    SQL Server Management Studio

.Net 5.0 SDK must be installed on your machine and SSMS is needed for the database.

Created Web API project in .Net 5 and EntityFramework Core. I used Code first approach to set up my DB components.

I have included migrations that will set up the database automatically the first time when you will run the project. 
Please update appsettings.Development.json if you run the project locally with some other Database Server name. 
By default I have used "(localdb)\\MSSQLLOCALDB" so it will create a Database under that Server.

For Logging purposes, I have Serilog and used SEQ to post all my logs.

For UnitTesting I have used XUnit testing and SQLLite to generate an in-memory database for my unit test cases.

Assumptions 
	1. Customer table created with extra info like address, city etc. which can be updated later on.
	2. Validations of fields can be included later on