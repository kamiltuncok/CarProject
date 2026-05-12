-- ============================================================
-- RentACar: Eski Sema -> Yeni Sema Veri Tasima Scripti
-- Backup'tan restore edilen veritabanini EF Core migration
-- semasina cevirerek eski verileri korur.
-- ============================================================
USE RentACar;
GO

-- ============================================================
-- ADIM 1: Eski tablolari _backup olarak yedekle
-- ============================================================
PRINT '>>> Eski tablolar yedekleniyor...';

SELECT * INTO Brands_backup FROM Brands;
SELECT * INTO Colors_backup FROM Colors;
SELECT * INTO Fuels_backup FROM Fuels;
SELECT * INTO Gears_backup FROM Gears;
SELECT * INTO Segments_backup FROM Segments;
SELECT * INTO Locations_backup FROM Locations;
SELECT * INTO Cars_backup FROM Cars;
SELECT * INTO CarImages_backup FROM CarImages;
SELECT * INTO Users_backup FROM Users;
SELECT * INTO OperationClaims_backup FROM OperationClaims;
SELECT * INTO UserOperationClaims_backup FROM UserOperationClaims;
SELECT * INTO IndividualCustomers_backup FROM IndividualCustomers;
SELECT * INTO CorporateCustomers_backup FROM CorporateCustomers;
SELECT * INTO Rentals_backup FROM Rentals;

-- Ek tablolar (varsa)
IF OBJECT_ID('CorporateUsers','U') IS NOT NULL SELECT * INTO CorporateUsers_backup FROM CorporateUsers;
IF OBJECT_ID('LocationOperationClaims','U') IS NOT NULL SELECT * INTO LocationOperationClaims_backup FROM LocationOperationClaims;
IF OBJECT_ID('PricingLogs','U') IS NOT NULL SELECT * INTO PricingLogs_backup FROM PricingLogs;
IF OBJECT_ID('sysdiagrams','U') IS NOT NULL SELECT * INTO sysdiagrams_backup FROM sysdiagrams;
GO

PRINT '>>> Yedekleme tamamlandi.';
GO

-- ============================================================
-- ADIM 2: Tum eski tablolari DROP et (FK sirasina gore)
-- ============================================================
PRINT '>>> Eski tablolar siliniyor...';

-- Disable FK checks by dropping constraints first
DECLARE @sql NVARCHAR(MAX) = N'';
SELECT @sql += 'ALTER TABLE [' + OBJECT_SCHEMA_NAME(parent_object_id) + '].[' + OBJECT_NAME(parent_object_id) + '] DROP CONSTRAINT [' + name + '];' + CHAR(13)
FROM sys.foreign_keys
WHERE OBJECT_NAME(parent_object_id) NOT LIKE '%_backup';
EXEC sp_executesql @sql;
GO

-- Drop all non-backup base tables
IF OBJECT_ID('Rentals','U') IS NOT NULL DROP TABLE Rentals;
IF OBJECT_ID('Cars','U') IS NOT NULL DROP TABLE Cars;
IF OBJECT_ID('CarImages','U') IS NOT NULL DROP TABLE CarImages;
IF OBJECT_ID('IndividualCustomers','U') IS NOT NULL DROP TABLE IndividualCustomers;
IF OBJECT_ID('CorporateCustomers','U') IS NOT NULL DROP TABLE CorporateCustomers;
IF OBJECT_ID('CorporateUsers','U') IS NOT NULL DROP TABLE CorporateUsers;
IF OBJECT_ID('UserOperationClaims','U') IS NOT NULL DROP TABLE UserOperationClaims;
IF OBJECT_ID('LocationOperationClaims','U') IS NOT NULL DROP TABLE LocationOperationClaims;
IF OBJECT_ID('PricingLogs','U') IS NOT NULL DROP TABLE PricingLogs;
IF OBJECT_ID('sysdiagrams','U') IS NOT NULL DROP TABLE sysdiagrams;
IF OBJECT_ID('Users','U') IS NOT NULL DROP TABLE Users;
IF OBJECT_ID('Locations','U') IS NOT NULL DROP TABLE Locations;
IF OBJECT_ID('Brands','U') IS NOT NULL DROP TABLE Brands;
IF OBJECT_ID('Colors','U') IS NOT NULL DROP TABLE Colors;
IF OBJECT_ID('Fuels','U') IS NOT NULL DROP TABLE Fuels;
IF OBJECT_ID('Gears','U') IS NOT NULL DROP TABLE Gears;
IF OBJECT_ID('Segments','U') IS NOT NULL DROP TABLE Segments;
IF OBJECT_ID('OperationClaims','U') IS NOT NULL DROP TABLE OperationClaims;
IF OBJECT_ID('Customers','U') IS NOT NULL DROP TABLE Customers;
IF OBJECT_ID('LocationCities','U') IS NOT NULL DROP TABLE LocationCities;
IF OBJECT_ID('LocationUserRoles','U') IS NOT NULL DROP TABLE LocationUserRoles;
IF OBJECT_ID('__EFMigrationsHistory','U') IS NOT NULL DROP TABLE __EFMigrationsHistory;
GO

PRINT '>>> Eski tablolar silindi. EF Core migration uygulanabilir.';
GO
