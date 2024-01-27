![publish nuget](https://github.com/leafbird/SitemapMaker/actions/workflows/nuget.yml/badge.svg)

# Sitemap Maker

![](https://raw.githubusercontent.com/leafbird/SitemapMaker/main/Assets/logo.png)

github의 문서 기록용 depot을 위한 sitemap 생성 스크립트 입니다.

a sitemap creation script for GitHub's document recording depot.

적용된 예시는 다음의 링크에서 확인할 수 있습니다. 

An applied example can be found in: https://github.com/leafbird/devwiki

## Getting Started

### Prerequisites

#### 1. install dotnet-script tool

This is made csx script. https://github.com/dotnet-script/dotnet-script 

```shell
dotnet tool install -g dotnet-script
```

#### 2. install SitemapMaker from nuget.org

```shell
nuget install SitemapMaker -OutputDirectory packages
or
nuget install SitemapMaker -Source https://api.nuget.org/v3/index.json -OutputDirectory packages
```

### Usage

* argument 0 : target file path. ex: ./README.md
* argument 1.. : historical data folders. ex: TIL

```shell
./packages/SitemapMaker.0.0.x/contentFiles/csx/any/SitemapMaker.csx ./README.md TIL
```