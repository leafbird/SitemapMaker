#!/usr/bin/env dotnet-script
#r "nuget:Cs.Logging, 0.0.23"
#load "./Source/SitemapMaker.csx"

using Cs.Logging;

var maker = SitemapMaker.Create(Args);
if (maker is null)
{
    return -1;
}

maker.Run();