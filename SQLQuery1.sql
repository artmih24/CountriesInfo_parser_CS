CREATE DATABASE Countries_DB;
GO

USE Countries_DB;
GO

CREATE TABLE dbo.Regions
(
	RegionID int IDENTITY(1, 1) NOT NULL,
	PRIMARY KEY (RegionID),
	RegionName varchar(max)
);
GO

CREATE TABLE dbo.Cities
(
	CityID int IDENTITY(1, 1) NOT NULL,
	PRIMARY KEY (CityID),
	CityName varchar(max)
);
GO

CREATE TABLE dbo.Countries
(
	CountryID int IDENTITY(1, 1) NOT NULL,
	PRIMARY KEY (CountryID),
	CountryName varchar(max),
	CountryCode varchar(3),
	CountryCapital int,
	FOREIGN KEY (CountryCapital) REFERENCES dbo.Cities(CityID),
	CountryArea float,
	CountryPopulation int,
	CountryRegion int,
	FOREIGN KEY (CountryRegion) REFERENCES dbo.Regions(RegionID)
);
GO