USE RentACar;
GO

-- Mevcut max customer ID
DECLARE @offset INT = (SELECT ISNULL(MAX(Id), 0) FROM Customers);

-- 1) Her User icin base Customer olustur
-- Gecici mapping tablosu ile
SELECT Id AS UserId, 
       @offset + ROW_NUMBER() OVER (ORDER BY Id) AS NewCustomerId,
       FirstName, LastName, IdentityNumber, PhoneNumber,
       Email, PasswordHash, PasswordSalt, Status
INTO #UserMapping
FROM Users_backup;

-- Customers base kayitlari ekle
SET IDENTITY_INSERT Customers ON;
INSERT INTO Customers (Id, PhoneNumber, CreatedDate)
SELECT NewCustomerId, PhoneNumber, GETDATE()
FROM #UserMapping;
SET IDENTITY_INSERT Customers OFF;

-- IndividualCustomers derived kayitlari ekle
INSERT INTO IndividualCustomers (Id, FirstName, LastName, IdentityNumber)
SELECT NewCustomerId, ISNULL(FirstName, 'N/A'), ISNULL(LastName, 'N/A'), IdentityNumber
FROM #UserMapping;

-- 2) Users tablosuna ekle
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (Id, Email, PasswordHash, PasswordSalt, Status, CustomerId)
SELECT UserId, Email, PasswordHash, PasswordSalt, Status, NewCustomerId
FROM #UserMapping;
SET IDENTITY_INSERT Users OFF;

PRINT 'Users inserted: ' + CAST((SELECT COUNT(*) FROM Users) AS VARCHAR(10));

-- 3) UserOperationClaims
SET IDENTITY_INSERT UserOperationClaims ON;
INSERT INTO UserOperationClaims (Id, UserId, OperationClaimId)
SELECT Id, UserId, OperationClaimId FROM UserOperationClaims_backup;
SET IDENTITY_INSERT UserOperationClaims OFF;

PRINT 'UserOperationClaims inserted: ' + CAST((SELECT COUNT(*) FROM UserOperationClaims) AS VARCHAR(10));

DROP TABLE #UserMapping;
GO

-- Sonuc
SELECT 'Customers' AS T, COUNT(*) AS Cnt FROM Customers
UNION ALL SELECT 'IndividualCustomers', COUNT(*) FROM IndividualCustomers
UNION ALL SELECT 'CorporateCustomers', COUNT(*) FROM CorporateCustomers
UNION ALL SELECT 'Users', COUNT(*) FROM Users
UNION ALL SELECT 'UserOperationClaims', COUNT(*) FROM UserOperationClaims;
GO
