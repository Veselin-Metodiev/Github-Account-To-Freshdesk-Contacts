CREATE DATABASE GithubAccountToFreshdeskContact

USE GithubAccountToFreshdeskContact

CREATE TABLE GithubAccount(
    Id INT PRIMARY KEY IDENTITY,
    [Login] VARCHAR(50) UNIQUE NOT NULL,
    [Name] VARCHAR(50) UNIQUE NOT NULL,
    CreationDate DATETIME2 NOT NULL
)