CREATE DATABASE TaskManagementDb;
GO

USE TaskManagementDb;
GO

CREATE TABLE Tasks
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    IsCompleted BIT NOT NULL DEFAULT 0,
    DueDate DATETIME NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

INSERT INTO Tasks
(
    Title,
    Description,
    IsCompleted,
    DueDate,
    CreatedDate
)
VALUES
(
    'Sample Task',
    'Initial task',
    0,
    GETDATE(),
    GETDATE()
);
GO