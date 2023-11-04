/*
Name: D.H. Shane Kavinda Amabhashana
Date: 02.11.2023
Purpose: Create a database to be used by a .NET Web API
*/


CREATE DATABASE Inventory;

-- Create Customer Table
CREATE TABLE Customer (
    UserId uniqueidentifier PRIMARY KEY,
    Username nvarchar(30),
    Email nvarchar(20),
    FirstName nvarchar(20),
    LastName nvarchar(20),
    CreatedOn datetime,
    IsActive bit
);


-- Create Supplier Table
CREATE TABLE Supplier (
    SupplierId uniqueidentifier PRIMARY KEY,
    SupplierName nvarchar(50),
    CreatedOn datetime,
    IsActive bit
);


-- Create Product Table
CREATE TABLE Product (
    ProductId uniqueidentifier PRIMARY KEY,
    ProductName nvarchar(50),
    UnitPrice decimal(18, 2),
    SupplierId uniqueidentifier,
    CreatedOn datetime,
    IsActive bit,
    FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);


-- Create Order Table
CREATE TABLE Orders(
    OrderId uniqueidentifier PRIMARY KEY,
    ProductId uniqueidentifier,
    OrderStatus int, 
    OrderType int,   
    OrderBy uniqueidentifier,
    OrderedOn datetime,
    ShippedOn datetime,
    IsActive bit,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    FOREIGN KEY (OrderBy) REFERENCES Customer(UserId)
);


/* Get the connection string */
select
    'data source=' + @@servername +
    ';initial catalog=' + db_name() +
    case type_desc
        when 'WINDOWS_LOGIN' 
            then ';trusted_connection=true'
        else
            ';user id=' + suser_name() + ';password=<<YourPassword>>'
    end
    as ConnectionString
from sys.server_principals
where name = suser_name()


-------------------------------------------------------------------------------------------------------------
/* 
POPULATION SCRIPT

*/

-- Populate the Customer table with random Guid values
INSERT INTO Customer (UserId, Username, Email, FirstName, LastName, CreatedOn, IsActive)
VALUES
    (NEWID(), 'user1', 'user1@example.com', 'John', 'Doe', GETDATE(), 1),
    (NEWID(), 'user2', 'user2@example.com', 'Jane', 'Smith', GETDATE(), 1),
    (NEWID(), 'user3', 'user3@example.com', 'Bob', 'Johnson', GETDATE(), 0),
	(NEWID(), 'user4', 'user4@example.com', 'Steve', 'Smith', GETDATE(), 1);



-- Populate the Supplier table with random Guid values
INSERT INTO Supplier (SupplierId, SupplierName, CreatedOn, IsActive)
VALUES
    (NEWID(), 'Supplier1', GETDATE(), 1),
    (NEWID(), 'Supplier2', GETDATE(), 1),
	(NEWID(), 'Supplier3', GETDATE(), 1);

SELECT * FROM Supplier;

-- Populate the Product table 
INSERT INTO Product (ProductId, ProductName, UnitPrice, SupplierId, CreatedOn, IsActive)
VALUES
    (NEWID(), 'Product1', 10.00, '902071CD-C206-4ED7-87DB-886AA15D1129', GETDATE(), 1),
    (NEWID(), 'Product2', 20.00, '2497258A-9B13-49AE-ACD1-B0AA5825CB64', GETDATE(), 1),
	(NEWID(), 'Product3', 15.00, 'CA88FA88-034C-4B0D-82A0-D486D4D5392C', GETDATE(), 1);

SELECT * FROM Product;
SELECT * FROM Customer;
SELECT * FROM Orders;
-- Populate the Order table with random Guid values
INSERT INTO Orders (OrderId, ProductId, OrderStatus, OrderType, OrderBy, OrderedOn, ShippedOn, IsActive)
VALUES
    (NEWID(), '7D40D107-A282-47C5-B8E8-0888B3793E7B', 1, 1, '9DC4913A-3356-4319-A359-CEF9D4C10F87', '2023-11-01', NULL, 1),
    (NEWID(), '5DFD7F15-7725-4157-B305-70087984CB9B', 2, 2, 'E9B160BF-4DB7-45A3-AAB9-066833D0F18C', GETDATE(), GETDATE(), 1),
	(NEWID(), '8ABB34F4-E06A-4CB2-A66A-D37245F6787C', 1, 1, '9DC4913A-3356-4319-A359-CEF9D4C10F87', '2023-11-01', '2023-11-02', 1);



-------------------------------------------------------------------------------------------------------------------------------------------------
/* Stored Procedures

*/

-- Create a Procedure to get all Orders placed by a Customer
CREATE PROCEDURE spGetActiveOrdersByCustomer
    @CustomerId uniqueidentifier
AS
BEGIN
    SELECT
        o.OrderId,
        o.ProductId,
        o.OrderStatus,
        o.OrderType,
        o.OrderedOn,
        o.ShippedOn,
        o.IsActive,
        p.ProductName,
        p.UnitPrice,
        s.SupplierName
    FROM
        Orders o
    INNER JOIN
        Product p ON o.ProductId = p.ProductId
    INNER JOIN
        Supplier s ON p.SupplierId = s.SupplierId
    WHERE
        o.OrderBy = @CustomerId
        AND o.IsActive = 1; -- Assuming 1 represents active orders
END;


-- Test the above procedure
-- Execute the stored procedure to get active orders for a specific customer
DECLARE @Id uniqueidentifier = '9DC4913A-3356-4319-A359-CEF9D4C10F87';

EXEC spGetActiveOrdersByCustomer @Id;