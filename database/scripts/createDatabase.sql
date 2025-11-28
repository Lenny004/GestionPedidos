-- Main Database
CREATE DATABASE OrderManagementDB;
GO

USE OrderManagementDB;
GO

-- Roles Table
CREATE TABLE Roles (
    idRole INT PRIMARY KEY IDENTITY(1,1),
    roleName VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(200),
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME NULL,
    deletedAt DATETIME NULL
);

-- Users Table
CREATE TABLE Users (
    idUser INT PRIMARY KEY IDENTITY(1,1),
    userName VARCHAR(50) NOT NULL UNIQUE,
    passwordHash VARCHAR(255) NOT NULL,
    fullName VARCHAR(100) NOT NULL,
    email VARCHAR(100),
    idRole INT NOT NULL,
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME NULL,
    deletedAt DATETIME NULL,
    FOREIGN KEY (idRole) REFERENCES Roles(idRole)
);

-- Departments Table (catalogue)
CREATE TABLE Departments (
    idDepartment INT PRIMARY KEY IDENTITY(1,1),
    departmentName VARCHAR(100) NOT NULL,
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME NULL,
    deletedAt DATETIME NULL
);

-- Cities Table
CREATE TABLE Cities (
    idCity INT PRIMARY KEY IDENTITY(1,1),
    cityName VARCHAR(100) NOT NULL,
    idDepartment INT NOT NULL,
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME NULL,
    deletedAt DATETIME NULL,
    FOREIGN KEY (idDepartment) REFERENCES Departments(idDepartment)
);

-- Customers Table
CREATE TABLE Customers (
    idCustomer INT PRIMARY KEY IDENTITY(1,1),
    firstName VARCHAR(100) NOT NULL,
    lastName VARCHAR(100) NOT NULL,
    phone VARCHAR(20),
    address VARCHAR(255),
    idDepartment INT,
    idCity INT,
    userCreation INT,
    userModification INT,
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME NULL,
    deletedAt DATETIME NULL,
    FOREIGN KEY (idDepartment) REFERENCES Departments(idDepartment),
    FOREIGN KEY (idCity) REFERENCES Cities(idCity),
    FOREIGN KEY (userCreation) REFERENCES Users(idUser),
    FOREIGN KEY (userModification) REFERENCES Users(idUser)
);

-- Products Table
CREATE TABLE Products (
    idProduct INT PRIMARY KEY IDENTITY(1,1),
    productName VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    stockQuantity INT NOT NULL DEFAULT 0,
    salePrice DECIMAL(10,2) NOT NULL,
    userCreation INT,
    userModification INT,
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME NULL,
    deletedAt DATETIME NULL,
    FOREIGN KEY (userCreation) REFERENCES Users(idUser),
    FOREIGN KEY (userModification) REFERENCES Users(idUser)
);

-- Delivery Table
CREATE TABLE Orders (
    idOrder INT PRIMARY KEY IDENTITY(1,1),
    idCustomer INT NOT NULL,
    orderDate DATETIME DEFAULT GETDATE(),
    deliveryDate DATETIME,
    comments VARCHAR(500),
    orderStatus VARCHAR(20) DEFAULT 'Pending', -- Pending, InProcess, Delivered, Cancelled
    total DECIMAL(10,2) DEFAULT 0,
    userCreation INT,
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME NULL,
    deletedAt DATETIME NULL,
    FOREIGN KEY (idCustomer) REFERENCES Customers(idCustomer),
    FOREIGN KEY (userCreation) REFERENCES Users(idUser)
);

-- Order Details Table
CREATE TABLE OrderDetails (
    idDetail INT PRIMARY KEY IDENTITY(1,1),
    idOrder INT NOT NULL,
    idProduct INT NOT NULL,
    quantity INT NOT NULL,
    unitPrice DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME NULL,
    deletedAt DATETIME NULL,
    FOREIGN KEY (idOrder) REFERENCES Orders(idOrder),
    FOREIGN KEY (idProduct) REFERENCES Products(idProduct)
);

-- History Table (Audit)
CREATE TABLE History (
    idHistory INT PRIMARY KEY IDENTITY(1,1),
    affectedTable VARCHAR(50) NOT NULL,
    actionType VARCHAR(20) NOT NULL, -- INSERT, UPDATE, DELETE
    recordId INT NOT NULL,
    description VARCHAR(500),
    idUser INT NOT NULL,
    createdAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (idUser) REFERENCES Users(idUser)
);