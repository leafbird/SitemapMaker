![publish nuget](https://github.com/leafbird/SitemapMaker/actions/workflows/nuget.yml/badge.svg)

# Sitemap Maker

![](https://raw.githubusercontent.com/leafbird/SitemapMaker/main/Assets/logo.png)

a sitemap creation script for GitHub's document recording depot.

If you cannot read Korean, use Google Chrome Translate. Google's capabilities are better than mine.

github의 문서 기록용 depot을 위한 sitemap 생성 스크립트 입니다.
적용된 예시는 다음의 링크에서 확인할 수 있습니다 : https://github.com/leafbird/devwiki

## Getting Started

### Prerequisites

#### 1. install dotnet-script tool

csx script로 작성되었으므로 적절한 runner가 필요합니다. https://github.com/dotnet-script/dotnet-script 

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

* 첫 번째 인자 : sitemap을 표시할 대상 파일. 예: ./README.md
* 두 번째 이후 인자들 : 히스토리성 데이터를 담는 폴더 목록. 예: TIL

```shell
./packages/SitemapMaker.0.0.x/contentFiles/csx/any/SitemapMaker.csx ./README.md TIL
```

nuget을 다운받아서 스크립트의 실행까지 한 번에 처리하는 간단한 [파워쉘 예제](https://github.com/leafbird/devwiki/blob/main/UpdateReadme.ps1)가 devwiki depot에 있습니다. 버전업 등의 예외사항에 모두 대응하고 있진 않지만 일반적으로 충분히 사용할 만 합니다. 

### 히스토리성 데이터란

TIL이나 개발일지처럼, 날짜별로 남기는 기록의 성격을 갖는 데이터를 말합니다. 다른 정보들과는 별개로 취급되며, 다음과 같은 추가 규칙으로 정리됩니다.

* 폴더별 카테고리를 표시할 때 : 일반 데이터 폴더를 모두 표시한 후 가장 아래쪽에 표시합니다.
* 폴더 내 파일목록을 표시할 때 : 일반 데이터는 알파벳 오름차순 정렬하지만, 히스토리 데이터는 내림차순 정렬합니다. 

![](https://raw.githubusercontent.com/leafbird/SitemapMaker/main/Assets/historicalData.webp)