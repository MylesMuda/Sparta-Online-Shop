use master 
go

drop database if exists SpartaShop
go

create database SpartaShop
go

use SpartaShop
go


drop table if exists UserType
go 
create table UserType(
	UserTypeID int not null identity primary key,
	TypeName nvarchar(100) null,
	TypeDescription varchar(MAX) null, 
);


drop table if exists Users
go
create table Users(
	UserID int not null identity primary key,
	UserTypeID int null foreign key references UserType(UserTypeID),
	FirstName nvarchar(50) not null,
	LastName nvarchar(50) not null,
	UserPassword nvarchar(50) not null, 
	UserEmail nvarchar(320) not null, 
	--UserAddress //needs to be looked into more before adding
	isVerified bit not null, 
	ActivationCode nvarchar(MAX), 
	LastLogin DateTime null,
	Locked bit not null
);


drop table if exists OrderStatus
go
create table OrderStatus(
	OrderStatusID int not null identity primary key,
	StatusName nvarchar(100) null,
	StatusDescription varchar(MAX) null
);


drop table if exists Orders
go
create table Orders(
	OrderID int not null identity primary key, 
	UserID int null foreign key references Users(UserID),
	OrderStatusID int null foreign key references OrderStatus(OrderStatusID), 
	TotalCost decimal(10, 2) null, 
	OrderDate DateTime not null,
	ShipDate DateTime null
);


drop table if exists Products
go
create table Products(
	ProductID int not null identity primary key,
	SKU nvarchar(100), 
	ProductName nvarchar(100),
	ProductDescription varchar(MAX) null, 
	Stock int null, 
	Price decimal(10, 2) not null
);

drop table if exists OrderDetails
go
create table OrderDetails(
	OrderDetailID int not null identity primary key,
	OrderID int null foreign key references Orders(OrderID),
	ProductID int null foreign key references Products(ProductID),
	ProductPrice decimal(10, 2) not null,
	Quantity int null
);

drop table if exists Reviews
go
create table Reviews(
	ReviewID int not null identity primary key,
	UserID int null foreign key references Users(UserID),
	ProductID int null foreign key references Products(ProductID),
	Rating int not null,
	ReviewText varchar(MAX) null,
	DateOfReview DateTime not null
);


drop table if exists Baskets
go
CREATE TABLE Baskets(
    BasketID INT NOT NULL IDENTITY PRIMARY KEY,
    UserID INT NULL FOREIGN KEY REFERENCES Users(UserID)
);

 
drop table if exists BasketItems
go
CREATE TABLE BasketItems(
    BasketItemID INT NOT NULL IDENTITY PRIMARY KEY,
    BasketID INT NULL FOREIGN KEY REFERENCES Baskets(BasketID),
    ProductID INT NULL FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NULL
);

 
drop table if exists Creators
go
CREATE TABLE Creators(
    CreatorID INT NOT NULL IDENTITY PRIMARY KEY,
    CreatorName NVARCHAR(100) NULL,
    ProfileImage NVARCHAR(MAX) NULL,
    Description VARCHAR(MAX) NULL,
    GitHubLink NVARCHAR(MAX) NULL,
    Website NVARCHAR(MAX) NULL
);


-- have more data input here if wanted
INSERT INTO Products VALUES (null, 'Hoodie', 'Hoodie with sparta logo', 20, 30.00);
INSERT INTO Products VALUES (null, 'Cap', 'Cap with sparta logo', 25, 15.00);
INSERT INTO Products VALUES (null, 'Key chain', 'Key chain in the shape of sparta logo', 45, 10.00);

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


select * from BasketItems;
select * from Baskets;
select * from Creators;
select * from OrderDetails;
select * from Orders;
select * from OrderStatus;
select * from Products;
select * from Reviews;
select * from Users;
select * from UserType;