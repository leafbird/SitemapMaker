#!/usr/bin/env dotnet-script
#load "./SitemapMaker.csx"

var maker = SitemapMaker.Create(Args);
if (maker is null)
{
    return -1;
}

maker.Run();