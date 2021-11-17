CREATE DATABASE fmaDatabase;

GO
USE fmaDatabase;
GO

CREATE TABLE Fuelcard(
    FuelcardId INT IDENTITY(1,1) PRIMARY KEY,
    Cardnumber VARCHAR(255) NOT NULL,
    ExpiryDate DATE NOT NULL,
    Pincode INT NULL,
    Fueltypes VARCHAR(255) NULL,
    IsActive BIT NULL,
    DriverId INT NULL
);

CREATE TABLE Address(
    AddressId INT IDENTITY(1,1) PRIMARY KEY,
    Street VARCHAR(255) NOT NULL,
    Housenumber VARCHAR(10) NOT NULL,
    Addendum VARCHAR(10) NULL,
    Postalcode INT NOT NULL,
    City VARCHAR(255) NOT NULL,
    DriverId INT NULL
);

CREATE TABLE Car(
    CarId INT IDENTITY(1,1) PRIMARY KEY,
    Vin VARCHAR(18) NOT NULL,
    Licenseplate VARCHAR(15) NOT NULL,
    Make VARCHAR(255) NOT NULL,
    Model VARCHAR(255) NOT NULL,
    VehicleType VARCHAR(255) NOT NULL,
    Fueltypes VARCHAR(255) NOT NULL,
    Doors VARCHAR(255) NULL,
    Colour VARCHAR(255) NULL,
    DriverId INT NULL
);

CREATE TABLE Driver(
    DriverId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARChaR(255) NOT NULL,
    DateOfBirth DATE NOT NULL,
    NationalIdentificationNumber INT NOT NULL,
    Licenses VARCHAR(255) NULL,
    AddressId INT NULL,
    FuelcardId INT NULL,
    CarId INT NULL
);

ALTER TABLE Fuelcard ADD CONSTRAINT FK_DriverIdFuelcard FOREIGN KEY (DriverId) REFERENCES Driver(DriverId);
ALTER TABLE Address ADD CONSTRAINT FK_DriverIdAddress FOREIGN KEY (DriverId) REFERENCES Driver(DriverId);
ALTER TABLE Car ADD CONSTRAINT FK_DriverIdCar FOREIGN KEY (DriverId) REFERENCES Driver(DriverId);
ALTER TABLE Driver ADD CONSTRAINT FK_AddressIdDriver FOREIGN KEY (AddressId) REFERENCES Address(AddressId);
ALTER TABLE Driver ADD CONSTRAINT FKC_CarIdDriver FOREIGN KEY (CarId) REFERENCES Car(CarId);
ALTER TABLE Driver ADD CONSTRAINT FK_FuelcardIdDriver FOREIGN KEY (FuelcardId) REFERENCES Fuelcard(Fuelcardid);