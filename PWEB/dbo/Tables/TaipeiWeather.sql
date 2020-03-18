CREATE TABLE [dbo].[TaipeiWeather] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [Location]           NVARCHAR (50)  NOT NULL,
    [StartTime]          DATETIME       NOT NULL,
    [EndTime]            DATETIME       NOT NULL,
    [RainChance]         TINYINT        CONSTRAINT [DF_TaipeiWeather_RainChance] DEFAULT ((0)) NOT NULL,
    [Temperature]        TINYINT        NOT NULL,
    [RelativeHumidity]   TINYINT        NOT NULL,
    [MinCI]              TINYINT        NOT NULL,
    [MaxCI]              TINYINT        NOT NULL,
    [WindSpeed]          TINYINT        NOT NULL,
    [WindDirection]      NVARCHAR (10)  NOT NULL,
    [MinAT]              TINYINT        NOT NULL,
    [MaxAT]              TINYINT        NOT NULL,
    [MinTemperature]     TINYINT        NOT NULL,
    [MaxTemperature]     TINYINT        NOT NULL,
    [UVI]                TINYINT        NOT NULL,
    [Td]                 TINYINT        NOT NULL,
    [Weather]            NVARCHAR (50)  NOT NULL,
    [WeatherDescription] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_TaipeiWeather] PRIMARY KEY CLUSTERED ([ID] ASC)
);

