/* =============================================================
   PNU_SearchIndex - custom search index for PNU SharePoint 2019
   Run ONCE on the SQL Server instance. No Full-Text required.
   ============================================================= */

IF DB_ID('PNU_SearchIndex') IS NULL
    CREATE DATABASE PNU_SearchIndex;
GO

USE PNU_SearchIndex;
GO

IF OBJECT_ID('dbo.SearchItems', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SearchItems
    (
        ItemId           BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        SourceType       NVARCHAR(20)   NOT NULL,
        SourceKey        NVARCHAR(450)  NOT NULL,
        SiteId           UNIQUEIDENTIFIER NOT NULL,
        WebId            UNIQUEIDENTIFIER NOT NULL,
        ListId           UNIQUEIDENTIFIER NULL,
        ListItemId       INT             NULL,
        TitleAr          NVARCHAR(500)   NULL,
        TitleEn          NVARCHAR(500)   NULL,
        ContentAr        NVARCHAR(MAX)   NULL,
        ContentEn        NVARCHAR(MAX)   NULL,
        Url              NVARCHAR(1000)  NOT NULL,
        Category         NVARCHAR(200)   NULL,
        DisplayDate      DATETIME        NULL,
        ModifiedDate     DATETIME        NOT NULL DEFAULT(GETDATE()),
        IsActive         BIT             NOT NULL DEFAULT(1)
    );

    CREATE UNIQUE INDEX UX_SearchItems_SourceKey ON dbo.SearchItems (SourceKey);
    CREATE INDEX IX_SearchItems_Category     ON dbo.SearchItems (Category, IsActive);
    CREATE INDEX IX_SearchItems_ModifiedDate ON dbo.SearchItems (ModifiedDate DESC);
END
GO

IF OBJECT_ID('dbo.SearchCrawlLog', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SearchCrawlLog
    (
        LogId        BIGINT IDENTITY(1,1) PRIMARY KEY,
        StartedAt    DATETIME NOT NULL DEFAULT(GETDATE()),
        FinishedAt   DATETIME NULL,
        ItemsCount   INT      NULL,
        Errors       NVARCHAR(MAX) NULL,
        CrawlType    NVARCHAR(50) NOT NULL
    );
END
GO

IF OBJECT_ID('dbo.usp_UpsertSearchItem', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_UpsertSearchItem;
GO
CREATE PROCEDURE dbo.usp_UpsertSearchItem
    @SourceType NVARCHAR(20), @SourceKey NVARCHAR(450),
    @SiteId UNIQUEIDENTIFIER, @WebId UNIQUEIDENTIFIER,
    @ListId UNIQUEIDENTIFIER = NULL, @ListItemId INT = NULL,
    @TitleAr NVARCHAR(500) = NULL, @TitleEn NVARCHAR(500) = NULL,
    @ContentAr NVARCHAR(MAX) = NULL, @ContentEn NVARCHAR(MAX) = NULL,
    @Url NVARCHAR(1000), @Category NVARCHAR(200) = NULL,
    @DisplayDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    MERGE dbo.SearchItems AS T
    USING (SELECT @SourceKey AS SourceKey) AS S ON T.SourceKey = S.SourceKey
    WHEN MATCHED THEN
        UPDATE SET TitleAr=@TitleAr, TitleEn=@TitleEn,
                   ContentAr=@ContentAr, ContentEn=@ContentEn,
                   Url=@Url, Category=@Category, DisplayDate=@DisplayDate,
                   ModifiedDate=GETDATE(), IsActive=1
    WHEN NOT MATCHED THEN
        INSERT (SourceType,SourceKey,SiteId,WebId,ListId,ListItemId,
                TitleAr,TitleEn,ContentAr,ContentEn,
                Url,Category,DisplayDate,ModifiedDate,IsActive)
        VALUES (@SourceType,@SourceKey,@SiteId,@WebId,@ListId,@ListItemId,
                @TitleAr,@TitleEn,@ContentAr,@ContentEn,
                @Url,@Category,@DisplayDate,GETDATE(),1);
END
GO

IF OBJECT_ID('dbo.usp_DeactivateSearchItem', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_DeactivateSearchItem;
GO
CREATE PROCEDURE dbo.usp_DeactivateSearchItem @SourceKey NVARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.SearchItems SET IsActive=0, ModifiedDate=GETDATE()
     WHERE SourceKey=@SourceKey;
END
GO

IF OBJECT_ID('dbo.usp_SearchItems', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_SearchItems;
GO
CREATE PROCEDURE dbo.usp_SearchItems
    @Keyword NVARCHAR(400), @IsArabic BIT = 1,
    @Categories NVARCHAR(MAX) = NULL, @SortBy NVARCHAR(20) = 'rel',
    @PageIndex INT = 1, @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageIndex - 1) * @PageSize;
    DECLARE @Cats TABLE (Cat NVARCHAR(200));
    IF @Categories IS NOT NULL AND LEN(@Categories) > 0
    BEGIN
        INSERT INTO @Cats(Cat)
        SELECT LTRIM(RTRIM(value)) FROM STRING_SPLIT(@Categories, ',')
         WHERE LEN(LTRIM(RTRIM(value))) > 0;
    END

    ;WITH base AS (
        SELECT s.*,
               CASE
                  WHEN @IsArabic=1 AND s.TitleAr LIKE N'%'+@Keyword+N'%' THEN 100
                  WHEN @IsArabic=0 AND s.TitleEn LIKE N'%'+@Keyword+N'%' THEN 100
                  WHEN @IsArabic=1 AND s.ContentAr LIKE N'%'+@Keyword+N'%' THEN 50
                  WHEN @IsArabic=0 AND s.ContentEn LIKE N'%'+@Keyword+N'%' THEN 50
                  ELSE 10 END AS Relevance
          FROM dbo.SearchItems s
         WHERE s.IsActive=1
           AND (@Keyword IS NULL OR LEN(@Keyword)=0
                OR (@IsArabic=1 AND (s.TitleAr LIKE N'%'+@Keyword+N'%' OR s.ContentAr LIKE N'%'+@Keyword+N'%'))
                OR (@IsArabic=0 AND (s.TitleEn LIKE N'%'+@Keyword+N'%' OR s.ContentEn LIKE N'%'+@Keyword+N'%')))
           AND (NOT EXISTS (SELECT 1 FROM @Cats) OR s.Category IN (SELECT Cat FROM @Cats))
    )
    SELECT @TotalCount = COUNT(*) FROM base;

    ;WITH base AS (
        SELECT s.*,
               CASE
                  WHEN @IsArabic=1 AND s.TitleAr LIKE N'%'+@Keyword+N'%' THEN 100
                  WHEN @IsArabic=0 AND s.TitleEn LIKE N'%'+@Keyword+N'%' THEN 100
                  WHEN @IsArabic=1 AND s.ContentAr LIKE N'%'+@Keyword+N'%' THEN 50
                  WHEN @IsArabic=0 AND s.ContentEn LIKE N'%'+@Keyword+N'%' THEN 50
                  ELSE 10 END AS Relevance
          FROM dbo.SearchItems s
         WHERE s.IsActive=1
           AND (@Keyword IS NULL OR LEN(@Keyword)=0
                OR (@IsArabic=1 AND (s.TitleAr LIKE N'%'+@Keyword+N'%' OR s.ContentAr LIKE N'%'+@Keyword+N'%'))
                OR (@IsArabic=0 AND (s.TitleEn LIKE N'%'+@Keyword+N'%' OR s.ContentEn LIKE N'%'+@Keyword+N'%')))
           AND (NOT EXISTS (SELECT 1 FROM @Cats) OR s.Category IN (SELECT Cat FROM @Cats))
    )
    SELECT * FROM base
     ORDER BY
        CASE WHEN @SortBy='rel'    THEN Relevance   END DESC,
        CASE WHEN @SortBy='newest' THEN DisplayDate END DESC,
        CASE WHEN @SortBy='oldest' THEN DisplayDate END ASC,
        ModifiedDate DESC
     OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

IF OBJECT_ID('dbo.usp_GetSearchCategories', 'P') IS NOT NULL DROP PROCEDURE dbo.usp_GetSearchCategories;
GO
CREATE PROCEDURE dbo.usp_GetSearchCategories
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Category, COUNT(*) AS Cnt FROM dbo.SearchItems
     WHERE IsActive=1 AND Category IS NOT NULL AND LEN(Category)>0
     GROUP BY Category ORDER BY Cnt DESC;
END
GO
