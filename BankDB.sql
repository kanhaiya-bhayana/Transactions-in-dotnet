

CREATE DATABASE BankDB;

USE BankDB;

CREATE TABLE Accounts
(
	AccountNumber VARCHAR(60) PRIMARY KEY,
	CustomerNumber VARCHAR(60),
	Balance int
);

EXEC sp_rename 'Accounts.CustomerNumber', 'CustomerName','Column';

INSERT INTO Accounts VALUES ('Account1','James',1000), ('Account2','Smith',1000)

SELECT * FROM Accounts;