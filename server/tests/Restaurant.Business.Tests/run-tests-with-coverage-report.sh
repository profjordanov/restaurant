#!/bin/bash
dotnet test Restaurant.Business.Tests.csproj -p:Exclude="[xunit*]*%2c[Restaurant.Persistence.Migrations]%2c[Restaurant.Api.Program]%2c[Restaurant.Api.Startup]" -p:CollectCoverage=true -p:CoverletOutputFormat=opencover --logger:"console;verbosity=normal"
