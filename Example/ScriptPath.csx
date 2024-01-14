#!/usr/bin/env dotnet-script
#r "nuget:Cs.Logging, 0.0.23"

using System.Runtime.CompilerServices; // for CallerFilePathAtribute
using Cs.Logging;

static string GetScriptPath([CallerFilePath] string path = null) => path;
var scriptPath = GetScriptPath();
Log.Debug($"scriptPath:{scriptPath} currentPath:{Environment.CurrentDirectory}");