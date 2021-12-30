CREATE DATABASE fmaDatabase;

GO
USE fmaDatabase
GO

CREATE TABLE Fuelcard(
    FuelcardId INT IDENTITY(1,1) PRIMARY KEY,
	Cardnumber VARCHAR(255) NOT NULL UNIQUE,
	ExpiryDate DATE NOT NULL,
	Pincode INT NULL,
	Fueltypes VARCHAR(255) NULL,
	IsActive BIT NOT NULL,
);

CREATE TABLE Address(
	AddressId INT IDENTITY(1,1) PRIMARY KEY,
	Street VARCHAR(255) NOT NULL,
	Housenumber VARCHAR(10) NOT NULL,
	Addendum VARCHAR(10) NULL,
	Postalcode INT NOT NULL,
	City VARCHAR(255) NOT NULL,
);

CREATE TABLE Car(
	CarId INT IDENTITY(1,1) PRIMARY KEY,
	Vin VARCHAR(18) NOT NULL UNIQUE,
	Licenseplate VARCHAR(15) NOT NULL UNIQUE,
	Make VARCHAR(255) NOT NULL,
	Model VARCHAR(255) NOT NULL,
	VehicleType VARCHAR(255) NOT NULL,
	Fueltypes VARCHAR(255) NOT NULL,
	Doors VARCHAR(255) NULL,
	Colour VARCHAR(255) NULL,
);

CREATE TABLE Driver(
	DriverId INT IDENTITY(1,1) PRIMARY KEY,
	FirstName VARCHAR(255) NOT NULL,
	LastName VARChaR(255) NOT NULL,
	DateOfBirth DATE NOT NULL,
	NationalIdentificationNumber VARCHAR(255) NOT NULL UNIQUE,
	Licenses VARCHAR(255) NULL,
	AddressId INT NULL,
	FuelcardId INT NULL UNIQUE,
	CarId INT NULL UNIQUE
);

ALTER TABLE Driver ADD CONSTRAINT FK_AddressIdDriver FOREIGN KEY (AddressId) REFERENCES Address(AddressId);
ALTER TABLE Driver ADD CONSTRAINT FKC_CarIdDriver FOREIGN KEY (CarId) REFERENCES Car(CarId);
ALTER TABLE Driver ADD CONSTRAINT FK_FuelcardIdDriver FOREIGN KEY (FuelcardId) REFERENCES Fuelcard(Fuelcardid);

USE [fmaDatabase]
GO

INSERT INTO [dbo].[Fuelcard]
           ([Cardnumber]
           ,[ExpiryDate]
           ,[Pincode]
           ,[Fueltypes]
           ,[IsActive])
     VALUES
           ('069341696715257749'
           ,'2023-07-29'
           ,496578
           ,'Diesel, Electrisch'
           ,1),
           ('079142696789257435'
           ,'2022-08-01'
           ,5379
           ,'Diesel'
           ,0),
           ('719991647715487749'
           ,'2022-12-19'
           ,1478    
           ,'Benzine'
           ,1),
           ('888114696789257435'
           ,'2022-08-01'
           ,5377
           ,'Diesel'
           ,0),
		   ('719988647715557747'
           ,'2022-12-19'
           ,1478    
           ,'Benzine, Electrisch'
           ,1)
GO

USE [fmaDatabase]
GO

INSERT INTO [dbo].[Car]
           ([Vin]
           ,[Licenseplate]
           ,[Make]
           ,[Model]
           ,[VehicleType]
           ,[Fueltypes]
           ,[Doors]
           ,[Colour])
     VALUES
           ('2G2GS69H2G2228132'
           ,'1-yxi-999'
           ,'Toyota'
           ,'Yaris'
           ,'Hatchback'
           ,'Benzine'
           ,4
           ,'Blue'),
           ('1FMZU73K45ZA04598'
           ,'2-ayu-809'
           ,'Ford'
           ,'Focus'
           ,'Hatchback'
           ,'Diesel, Electrisch'
           ,2
           ,'Black'),
           ('1FADP3K22DL218948'
           ,'1-bou-109'
           ,'Toyota'
           ,'RAV 4'
           ,'SUV'
           ,'Benzine'
           ,NULL
           ,NULL),
           ('1HGCR2F81DA186144'
           ,'1-zzz-889'
           ,'Ford'
           ,'Focus'
           ,'Hatchback'
           ,'Electrisch'
           ,2
           ,NULL),
		   ('1FAHP35N88W222708'
           ,'2-bou-109'
           ,'Toyota'
           ,'RAV 4'
           ,'SUV'
           ,'Benzine, Electrisch'
           ,4
           ,'Groen')
GO

USE [fmaDatabase]
GO

INSERT INTO [dbo].[Address]
			([Street],
			[Housenumber],
			[Addendum],
			[Postalcode],
			[City])
		VALUES
			('Hogeweg',
			'4',
			'A',
			9500,
			'Geraardsbergen'),
			('Steenbakkerij',
			'21',
			NULL,
			9500,
			'Geraardsbergen'),
			('Hogeweg',
			'4',
			'B',
			9500,
			'Geraardsbergen'),
			('Assestraat',
			'4',
			NULL,
			9550,
			'Steenhuize'),
			('Ganzestraat',
			'45',
			NULL,
			9550,
			'Steenhuize'
			),
			('Winkelstraat',
			'21',
			NULL,
			9550,
			'Steenhuize'
			),
			('Aaigemstraat',
			'148',
			NULL,
			9000,
			'Gent'
			)
GO

