-- Создание базы данных
CREATE DATABASE FintechBank;
GO

-- Использование базы данных
USE FintechBank;
GO

-- Таблица пользователей
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    PasswordHash NVARCHAR(256) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Таблица ролей
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- Таблица связей пользователей и ролей
CREATE TABLE UserRoles (
    UserID INT,
    RoleID INT,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    PRIMARY KEY (UserID, RoleID)
);
GO

-- Таблица счетов
CREATE TABLE Accounts (
    AccountID INT PRIMARY KEY IDENTITY,
    UserID INT,
    AccountNumber NVARCHAR(20) NOT NULL UNIQUE,
    Balance DECIMAL(18,2) DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO

-- Таблица типов транзакций
CREATE TABLE TransactionTypes (
    TransactionTypeID INT PRIMARY KEY IDENTITY,
    TransactionTypeName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- Таблица транзакций
CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY IDENTITY,
    SenderAccountID INT,
    ReceiverAccountID INT,
    Amount DECIMAL(18,2) NOT NULL,
    TransactionTypeID INT,
    Description NVARCHAR(500),
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SenderAccountID) REFERENCES Accounts(AccountID),
    FOREIGN KEY (ReceiverAccountID) REFERENCES Accounts(AccountID),
    FOREIGN KEY (TransactionTypeID) REFERENCES TransactionTypes(TransactionTypeID)
);
GO

-- Таблица статусов карт
CREATE TABLE CardStatuses (
    CardStatusID INT PRIMARY KEY IDENTITY,
    CardStatusName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- Таблица карт
CREATE TABLE Cards (
    CardID INT PRIMARY KEY IDENTITY,
    AccountID INT,
    CardNumber NVARCHAR(16) NOT NULL UNIQUE,
    CardType NVARCHAR(50) NOT NULL,
    ExpirationDate DATETIME NOT NULL,
    CardStatusID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID),
    FOREIGN KEY (CardStatusID) REFERENCES CardStatuses(CardStatusID)
);
GO

-- Вставка начальных данных ролей
INSERT INTO Roles (RoleName) VALUES ('Admin'), ('User');
GO

-- Вставка начальных данных типов транзакций
INSERT INTO TransactionTypes (TransactionTypeName) VALUES ('Transfer'), ('Withdrawal'), ('Payment');
GO

-- Вставка начальных данных статусов карт
INSERT INTO CardStatuses (CardStatusName) VALUES ('Active'), ('Blocked'), ('Expired');
GO
