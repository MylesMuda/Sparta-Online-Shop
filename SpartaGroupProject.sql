use master 
go

drop database if exists SpartaShop
go

create database SpartaShop
go

use SpartaShop
go


DROP TABLE IF EXISTS Users
GO
DROP TABLE IF EXISTS UserType
GO 
DROP TABLE IF EXISTS OrderStatus
GO
DROP TABLE IF EXISTS Orders
GO
DROP TABLE IF EXISTS Products
GO
DROP TABLE IF EXISTS OrderDetails
GO
DROP TABLE IF EXISTS Reviews
GO
DROP TABLE IF EXISTS Baskets
GO
DROP TABLE IF EXISTS BasketItems
GO
DROP TABLE IF EXISTS Creators
GO

CREATE TABLE UserType(
	UserTypeID INT NOT NULL IDENTITY PRIMARY KEY,
	TypeName NVARCHAR(100) NULL,
	TypeDescription VARCHAR(MAX) NULL, 
);



CREATE TABLE Users(
	UserID INT NOT NULL IDENTITY PRIMARY KEY,
	UserTypeID INT NULL FOREIGN KEY REFERENCES UserType(UserTypeID),
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	UserPassword NVARCHAR(50) NOT NULL, 
	UserEmail NVARCHAR(320) NOT NULL, 
	--UserAddress //needs to be looked into more before adding
	IsVerified BIT NOT NULL, 
	ActivationCode NVARCHAR(MAX) NULL, 
	LastLogin DATETIME NULL,
	Locked BIT NULL
);



CREATE TABLE OrderStatus(
	OrderStatusID INT NOT NULL IDENTITY PRIMARY KEY,
	StatusName NVARCHAR(100) NULL,
	StatusDescription VARCHAR(MAX) NULL
);



CREATE TABLE Orders(
	OrderID INT NOT NULL IDENTITY PRIMARY KEY, 
	UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
	OrderStatusID INT NULL FOREIGN KEY REFERENCES OrderStatus(OrderStatusID), 
	TotalCost DECIMAL(10, 2) NULL, 
	OrderDate DATETIME NULL,
	ShipDate DATETIME NULL
);



CREATE TABLE Products(
	ProductID INT NOT NULL IDENTITY PRIMARY KEY,
	SKU NVARCHAR(100), 
	ProductName NVARCHAR(100),
	ProductDescription VARCHAR(MAX) NULL, 
	Stock INT NULL, 
	Price DECIMAL(10, 2) NOT NULL
);


CREATE TABLE OrderDetails(
	OrderDetailID INT NOT NULL IDENTITY PRIMARY KEY,
	OrderID INT NOT NULL FOREIGN KEY REFERENCES Orders(OrderID),
	ProductID INT NOT NULL FOREIGN KEY REFERENCES Products(ProductID),
	ProductPrice DECIMAL(10, 2) NOT NULL,
	Quantity INT NULL
);


CREATE TABLE Reviews(
	ReviewID INT NOT NULL IDENTITY PRIMARY KEY,
	UserID INT NULL FOREIGN KEY REFERENCES Users(UserID),
	ProductID INT NULL FOREIGN KEY REFERENCES Products(ProductID),
	Rating INT NOT NULL,
	ReviewText VARCHAR(MAX) NULL,
	DateOfReview DATETIME NOT NULL
);



CREATE TABLE Baskets(
    BasketID INT NOT NULL IDENTITY PRIMARY KEY,
    UserID INT NULL FOREIGN KEY REFERENCES Users(UserID)
);

 

CREATE TABLE BasketItems(
    BasketItemID INT NOT NULL IDENTITY PRIMARY KEY,
    BasketID INT NOT NULL FOREIGN KEY REFERENCES Baskets(BasketID),
    ProductID INT NOT NULL FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NULL
);

 

CREATE TABLE Creators(
    CreatorID INT NOT NULL IDENTITY PRIMARY KEY,
    CreatorName NVARCHAR(100) NULL,
    ProfileImage NVARCHAR(MAX) NULL,
    [Description] VARCHAR(MAX) NULL,
    GitHubLink NVARCHAR(MAX) NULL,
    Website NVARCHAR(MAX) NULL
);


-- have more data input here if wanted
INSERT INTO Products VALUES (NULL, 'Hoodie', 'Hoodie with sparta logo', 20, 30.00);
INSERT INTO Products VALUES (NULL, 'Cap', 'Cap with sparta logo', 25, 15.00);
INSERT INTO Products VALUES (NULL, 'Key chain', 'Key chain in the shape of sparta logo', 45, 10.00);

INSERT INTO UserType VALUES ('Admin', 'This user can modify the website');
INSERT INTO UserType VALUES ('Customer', 'This user can only user the website commercially');

INSERT INTO Creators(CreatorName) VALUES ('Alykhan Esmail');
INSERT INTO Creators(CreatorName) VALUES ('Fuat Kalay');
INSERT INTO Creators(CreatorName) VALUES ('Jonathan Freire-Benites');
INSERT INTO Creators(CreatorName) VALUES ('Miguel Vieira');
INSERT INTO Creators(CreatorName) VALUES ('Mohssin Abihilal');
INSERT INTO Creators(CreatorName) VALUES ('Myles Muda');
INSERT INTO Creators(CreatorName) VALUES ('Ruoyi Jiang');
INSERT INTO Creators(CreatorName) VALUES ('Ryan Burdus');
INSERT INTO Creators(CreatorName) VALUES ('Samuel Ribeiro');


SELECT * FROM BasketItems;
SELECT * FROM Baskets;
SELECT * FROM Creators;
SELECT * FROM OrderDetails;
SELECT * FROM Orders;
SELECT * FROM OrderStatus;
SELECT * FROM Products;
SELECT * FROM Reviews;
SELECT * FROM Users;
SELECT * FROM UserType;