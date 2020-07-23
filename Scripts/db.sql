CREATE DATABASE TempRepnovIB_2

GO 
 
USE TempRepnovIB_2
 
GO 

CREATE TABLE AdresType 
  ( 
     Id   BIGINT IDENTITY(1, 1) NOT NULL, 
     NAME NVARCHAR(50) NOT NULL, 
     PRIMARY KEY (Id) 
  ) 
 
GO  
 
CREATE TABLE DocumentType 
  ( 
     Id   BIGINT IDENTITY(1, 1) NOT NULL, 
     NAME NVARCHAR(50) NOT NULL, 
     PRIMARY KEY (Id) 
  ) 
 
GO 

CREATE TABLE People 
  ( 
     Id   BIGINT IDENTITY(1, 1) NOT NULL, 
     FirstName NVARCHAR(50) NOT NULL, --Имя
	 MiddleName NVARCHAR(50) NOT NULL, --Отчество
	 LastName NVARCHAR(50) NULL, --Фамилия
	 BirthDate DATETIME NOT NULL,
     PRIMARY KEY (Id) 
  ) 
 
GO 
 
CREATE TABLE PeopleDocuments 
  ( 
     PeopleId      BIGINT NOT NULL, 
     DocumentTypeId BIGINT NOT NULL, 
     Seria       NVARCHAR(20) NULL, 
	 Number       NVARCHAR(20) NOT NULL, 
     PRIMARY KEY (PeopleId, DocumentTypeId, Number), 
     FOREIGN KEY (PeopleId) REFERENCES People(Id),
	 FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id), 
  ) 
 
GO 


CREATE TABLE PeopleAdreses 
  ( 
     PeopleId      BIGINT NOT NULL, 
     AdresTypeId BIGINT NOT NULL, 
     Adres       NVARCHAR(200) NOT NULL, 
     PRIMARY KEY (PeopleId, AdresTypeId, Adres), 
     FOREIGN KEY (PeopleId) REFERENCES People(Id),
	 FOREIGN KEY (AdresTypeId) REFERENCES AdresType(Id), 
  ) 
 
GO 
 


SET DATEFORMAT DMY

GO

INSERT INTO DocumentType
VALUES
('Свидетельство о рождении'),
('Паспорт РФ'),
('Водительские права')
 
INSERT INTO AdresType
VALUES
('Адрес места регистрации'),
('Адрес места временного пребывания')
 
 
INSERT INTO People
VALUES
('Иван', 'Иванович', 'Иванов', '01.01.1990'),
('Пётр', 'Петрович', 'Петров', '10.10.1990'),
('Иван', 'Семенович', 'Иванов', '11.11.1999'),
('Семен', 'Семенович', 'Семенов', '01.01.2000'),
('Игорь', 'Борисович', 'Иванов', '10.10.2010'),
('Михаил', 'Иванович', 'Ефремов', '01.01.1990')
 
INSERT INTO PeopleDocuments
VALUES
(1, 2, '0305', '545646'),
(2, 2, '0303', '1232123'),
(4, 2, '0300', '5452222'),
(4, 3, 'DFR', '11111'),
(5, 1, 'AF-67', '545346'),
(6, 2, '0304', '333344')

INSERT INTO PeopleAdreses
VALUES
(1, 1, 'ldksjhgf ldksfghfjgkhd kjhgdg'),
(2, 2, 'asdkhak asjdh akjs adjsas dasd'),
(3, 2, 'sdgsdg sdgs sdgsg'),
(4, 2, 'sdgsdg sdgs asad'),
(5, 1, 'длрва порап лвоаваопр в ваопвап  вапорвалп'),
(5, 2, 'вап вап ва вап вапвап')




/*
exec command

Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore.SqlServer


Scaffold-DbContext "Server=10.0.1.156;Database=TempRepnovIB_2;User Id=sa;Password=VVal2787;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

*/