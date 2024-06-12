-- �������� ���� ������
CREATE DATABASE FintechBank;
GO

-- ������������� ���� ������
USE FintechBank;
GO

-- ������� �������������
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    PasswordHash NVARCHAR(256) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- ������� �����
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- ������� ������ ������������� � �����
CREATE TABLE UserRoles (
    UserID INT,
    RoleID INT,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    PRIMARY KEY (UserID, RoleID)
);
GO

-- ������� ������
CREATE TABLE Accounts (
    AccountID INT PRIMARY KEY IDENTITY,
    UserID INT,
    AccountNumber NVARCHAR(20) NOT NULL UNIQUE,
    Balance DECIMAL(18,2) DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
GO

-- ������� ����� ����������
CREATE TABLE TransactionTypes (
    TransactionTypeID INT PRIMARY KEY IDENTITY,
    TransactionTypeName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- ������� ����������
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

-- ������� �������� ����
CREATE TABLE CardStatuses (
    CardStatusID INT PRIMARY KEY IDENTITY,
    CardStatusName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- ������� ����
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

-- ������� ��������� ������ �����
INSERT INTO Roles (RoleName) VALUES ('Admin'), ('User');
GO

-- ������� ��������� ������ ����� ����������
INSERT INTO TransactionTypes (TransactionTypeName) VALUES ('Transfer'), ('Withdrawal'), ('Payment');
GO

-- ������� ��������� ������ �������� ����
INSERT INTO CardStatuses (CardStatusName) VALUES ('Active'), ('Blocked'), ('Expired');
GO
