-- ============================================================
-- RentACar: Adim 2 - Veri Tasima (Backup -> Yeni Sema)
-- Eski tablolardan yeni sema tablolarina veri aktarimi
-- ============================================================
USE RentACar;
GO

SET NOCOUNT ON;
PRINT '>>> Veri tasima basliyor...';
GO

-- ============================================================
-- 1. Lookup tablolari (FK bagimliligi yok)
-- ============================================================
PRINT '  [1/10] Brands...';
SET IDENTITY_INSERT Brands ON;
INSERT INTO Brands (BrandId, BrandName)
SELECT BrandId, BrandName FROM Brands_backup;
SET IDENTITY_INSERT Brands OFF;
GO

PRINT '  [2/10] Colors...';
SET IDENTITY_INSERT Colors ON;
INSERT INTO Colors (ColorId, ColorName)
SELECT ColorId, ColorName FROM Colors_backup;
SET IDENTITY_INSERT Colors OFF;
GO

PRINT '  [3/10] Fuels...';
SET IDENTITY_INSERT Fuels ON;
INSERT INTO Fuels (FuelId, FuelName)
SELECT FuelId, FuelName FROM Fuels_backup;
SET IDENTITY_INSERT Fuels OFF;
GO

PRINT '  [4/10] Gears...';
SET IDENTITY_INSERT Gears ON;
INSERT INTO Gears (GearId, GearName)
SELECT GearId, GearName FROM Gears_backup;
SET IDENTITY_INSERT Gears OFF;
GO

PRINT '  [5/10] Segments...';
SET IDENTITY_INSERT Segments ON;
INSERT INTO Segments (SegmentId, SegmentName)
SELECT SegmentId, SegmentName FROM Segments_backup;
SET IDENTITY_INSERT Segments OFF;
GO

-- ============================================================
-- 2. LocationCities (yeni tablo - eski LocationCity string'lerinden olustur)
-- ============================================================
PRINT '  [6/10] LocationCities + Locations...';
-- Eski Location tablosundaki benzersiz sehirleri LocationCities'e ekle
INSERT INTO LocationCities (Name)
SELECT DISTINCT LocationCity
FROM Locations_backup
WHERE LocationCity IS NOT NULL AND LocationCity <> ''
  AND LocationCity NOT IN (SELECT Name FROM LocationCities);
GO

-- Locations: Eski LocationId -> Yeni Id mapping
SET IDENTITY_INSERT Locations ON;
INSERT INTO Locations (Id, LocationName, Address, Email, PhoneNumber, Latitude, Longitude, LocationCityId)
SELECT
    L.LocationId,
    L.LocationName,
    L.Address,
    L.Email,
    L.PhoneNumber,
    ISNULL(L.Latitude, 0),
    ISNULL(L.Longitude, 0),
    LC.Id
FROM Locations_backup L
INNER JOIN LocationCities LC ON LC.Name = L.LocationCity;
SET IDENTITY_INSERT Locations OFF;
GO

-- ============================================================
-- 3. OperationClaims
-- ============================================================
PRINT '  [7/10] OperationClaims + UserOperationClaims...';
SET IDENTITY_INSERT OperationClaims ON;
INSERT INTO OperationClaims (Id, Name)
SELECT Id, Name FROM OperationClaims_backup;
SET IDENTITY_INSERT OperationClaims OFF;
GO

-- ============================================================
-- 4. Customers (TPT: base Customers -> IndividualCustomers / CorporateCustomers)
--    Eski semada: IndividualCustomers ve CorporateCustomers ayri tablolardi
--    Yeni semada: Customers (base) + IndividualCustomers (derived) + CorporateCustomers (derived)
-- ============================================================
PRINT '  [8/10] Customers (TPT hierarchy)...';

-- Individual customers -> Customers base + IndividualCustomers derived
SET IDENTITY_INSERT Customers ON;
INSERT INTO Customers (Id, PhoneNumber, CreatedDate)
SELECT
    CustomerId,
    PhoneNumber,
    GETDATE()
FROM IndividualCustomers_backup;
SET IDENTITY_INSERT Customers OFF;
GO

SET IDENTITY_INSERT IndividualCustomers ON;
INSERT INTO IndividualCustomers (Id, FirstName, LastName, IdentityNumber)
SELECT
    CustomerId,
    FirstName,
    LastName,
    IdentityNumber
FROM IndividualCustomers_backup;
SET IDENTITY_INSERT IndividualCustomers OFF;
GO

-- Corporate customers -> Customers base + CorporateCustomers derived
-- ID'leri individual'larla cakmamasi icin offset ekle
DECLARE @maxIndId INT = (SELECT ISNULL(MAX(Id), 0) FROM Customers);

SET IDENTITY_INSERT Customers ON;
INSERT INTO Customers (Id, PhoneNumber, CreatedDate)
SELECT
    CustomerId + @maxIndId,
    PhoneNumber,
    GETDATE()
FROM CorporateCustomers_backup;
SET IDENTITY_INSERT Customers OFF;

SET IDENTITY_INSERT CorporateCustomers ON;
INSERT INTO CorporateCustomers (Id, CompanyName, TaxNumber)
SELECT
    CustomerId + @maxIndId,
    CompanyName,
    TaxNumber
FROM CorporateCustomers_backup;
SET IDENTITY_INSERT CorporateCustomers OFF;
GO

-- ============================================================
-- 5. Users (yeni semada CustomerId FK var)
-- ============================================================
PRINT '  [9/10] Users...';
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (Id, Email, PasswordHash, PasswordSalt, Status, CustomerId)
SELECT
    U.Id,
    U.Email,
    U.PasswordHash,
    U.PasswordSalt,
    U.Status,
    -- Eski semada Users dogrudan customer'a bagliydi. 
    -- CustomerId=0 olarak baslat, sonra gerekirse guncellenebilir.
    0
FROM Users_backup U;
SET IDENTITY_INSERT Users OFF;
GO

-- UserOperationClaims
SET IDENTITY_INSERT UserOperationClaims ON;
INSERT INTO UserOperationClaims (Id, UserId, OperationClaimId)
SELECT Id, UserId, OperationClaimId FROM UserOperationClaims_backup;
SET IDENTITY_INSERT UserOperationClaims OFF;
GO

-- ============================================================
-- 6. Cars (eski sema: LocationId, IsRented, int DailyPrice
--          yeni sema: CurrentLocationId, Status string, decimal DailyPrice, PlateNumber, KM, RowVersion)
-- ============================================================
PRINT '  [10/10] Cars + CarImages...';
SET IDENTITY_INSERT Cars ON;
INSERT INTO Cars (Id, BrandId, ColorId, CurrentLocationId, ModelYear, DailyPrice, Deposit, 
                  PlateNumber, KM, Status, FuelId, GearId, SegmentId, Description)
SELECT
    Id,
    BrandId,
    ColorId,
    LocationId,          -- -> CurrentLocationId
    ModelYear,
    CAST(DailyPrice AS decimal(18,2)),
    CAST(ISNULL(Deposit, 0) AS decimal(18,2)),
    'PLATE-' + CAST(Id AS VARCHAR(10)),  -- PlateNumber (eski semada yoktu, dummy uret)
    0,                                    -- KM (eski semada yoktu)
    CASE WHEN IsRented = 1 THEN 'Rented' ELSE 'Available' END,  -- Status enum string
    FuelId,
    GearId,
    SegmentId,
    RTRIM(ISNULL(Description, ''))
FROM Cars_backup;
SET IDENTITY_INSERT Cars OFF;
GO

-- CarImages
SET IDENTITY_INSERT CarImages ON;
INSERT INTO CarImages (CarImageId, BrandId, ColorId, ImagePath)
SELECT CarImageId, BrandId, ColorId, ImagePath FROM CarImages_backup;
SET IDENTITY_INSERT CarImages OFF;
GO

-- ============================================================
-- Sonuc raporu
-- ============================================================
PRINT '';
PRINT '==========================================';
PRINT '>>> VERI TASIMA TAMAMLANDI';
PRINT '==========================================';

SELECT 'Brands' AS Tablo, COUNT(*) AS Kayit FROM Brands
UNION ALL SELECT 'Colors', COUNT(*) FROM Colors
UNION ALL SELECT 'Fuels', COUNT(*) FROM Fuels
UNION ALL SELECT 'Gears', COUNT(*) FROM Gears
UNION ALL SELECT 'Segments', COUNT(*) FROM Segments
UNION ALL SELECT 'LocationCities', COUNT(*) FROM LocationCities
UNION ALL SELECT 'Locations', COUNT(*) FROM Locations
UNION ALL SELECT 'OperationClaims', COUNT(*) FROM OperationClaims
UNION ALL SELECT 'Customers', COUNT(*) FROM Customers
UNION ALL SELECT 'IndividualCustomers', COUNT(*) FROM IndividualCustomers
UNION ALL SELECT 'CorporateCustomers', COUNT(*) FROM CorporateCustomers
UNION ALL SELECT 'Users', COUNT(*) FROM Users
UNION ALL SELECT 'UserOperationClaims', COUNT(*) FROM UserOperationClaims
UNION ALL SELECT 'Cars', COUNT(*) FROM Cars
UNION ALL SELECT 'CarImages', COUNT(*) FROM CarImages;
GO
