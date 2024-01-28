## Prepair to Run
## EF migration
0. install global tool to make migration(do only 1 time & your machine is good to go for the next)
```
dotnet tool install --global dotnet-ef
```
1. create migrations & the dbcontext snapshot will rendered.
Open CLI at apis folder & run command
-s is startup project(create dbcontext instance at design time)
-p is migrations assembly project 
```
dotnet ef migrations add NewMigration
```

2. apply the change
```
dotnet ef database update
```