# ExileApi
Forum Thread discussing this repository and a list of possible plugin repositories can be found here https://www.ownedcore.com/forums/mmo/path-of-exile/poe-bots-programs/902969-exileapi-fork-3-11-release.html

# Release
https://github.com/Queuete/ExileApi/releases

# Pull Requests or other forms of input are appreciated
# How To Setup a Developer Version
Please look into the base repository the needed software is described there, including some troubleshooting https://github.com/Qvin0000/ExileApi

To use this version you need .net4.8 and git. 
Git https://git-scm.com/downloads
.NET 4.8 https://dotnet.microsoft.com/download/thank-you/net48

1. Create a PoeHUD (any name) folder
2. Open a git bash inside the folder
3. Run `git clone https://github.com/Queuete/ExileApi`
4. Open the solution file in Visual Studio
5. This should compile already, there could be some reference errors, those are mostly fixed by removing the reference (solution exlporer-plugin_project->references) the ones with warnings need to be readded

-> Adding plugins (example DevTree)

6. Open git bash in "ExileApi/Plugins"
7. Run `git clone https://github.com/Queuete/DevTree`
8. Open Visual Studio, right click the "ExileApi" solution in the "Solution Explorer". -> Add -> New Solution Folder (name it "Plugins")
9. Right click created folder -> Add -> Existing Project -> "ExileApi/Plugins/Devtree/DevTree.csproj"


Tools used for reverse engineering are mostly: ReClass.NET, CheatEngine, Ghidra
When you want to learn something about it, get comfortable with at least the first tool or second tool and try to comprehend already updated offsets. (For 3.10 I advise you to start with the LifeComponent)

