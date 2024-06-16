CREATE TABLE CompanyClient (
    CompanyClientId INT PRIMARY KEY IDENTITY(1,1),
    CompanyName NVARCHAR(MAX),
    Address NVARCHAR(MAX),
    Email NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    KRS NVARCHAR(MAX)
);

CREATE TABLE IndividualClient (
    IndividualClientId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(MAX),
    LastName NVARCHAR(MAX),
    Address NVARCHAR(MAX),
    Email NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    PESEL NVARCHAR(MAX)
);

CREATE TABLE Product (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    CurrentVersion NVARCHAR(MAX),
    Category NVARCHAR(MAX),
    Price DECIMAL(18, 2)
);

CREATE TABLE RevenueRecognition (
    RevenueRecognitionId INT PRIMARY KEY IDENTITY(1,1),
    Amount DECIMAL(18, 2),
    RecognitionDate DATETIME,
    ProductId INT,
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);

CREATE TABLE Contract (
    ContractId INT PRIMARY KEY IDENTITY(1,1),
    StartDate DATETIME,
    EndDate DATETIME,
    Price DECIMAL(18, 2),
    IsSigned BIT,
    IndividualClientId INT NULL,
    CompanyClientId INT NULL,
    ProductId INT,
    FOREIGN KEY (IndividualClientId) REFERENCES IndividualClient(IndividualClientId),
    FOREIGN KEY (CompanyClientId) REFERENCES CompanyClient(CompanyClientId),
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);

CREATE TABLE Discount (
    DiscountId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX),
    OfferValue NVARCHAR(MAX),
    StartDate DATETIME,
    EndDate DATETIME,
    Percentage DECIMAL(5, 2)
);

CREATE TABLE Subscription (
    SubscriptionId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX),
    RenewalPeriod NVARCHAR(MAX),
    Price DECIMAL(18, 2),
    StartDate DATETIME,
    EndDate DATETIME,
    IndividualClientId INT NULL,
    CompanyClientId INT NULL,
    ProductId INT,
    FOREIGN KEY (IndividualClientId) REFERENCES IndividualClient(IndividualClientId),
    FOREIGN KEY (CompanyClientId) REFERENCES CompanyClient(CompanyClientId),
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);
