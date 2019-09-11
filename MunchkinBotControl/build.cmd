@echo OFF
cd MunchkinBot
git pull
nuget restore
devenv /Build Release "MunchkinBot.sln"