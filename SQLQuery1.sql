CREATE TABLE OfferCategories(
Id int IDENTITY(1,1) PRIMARY KEY,
CategoryName varchar(255) NOT NULL
);

select * from OfferCategories;



insert into OfferCategories (CategoryName)
values ('Hospital');
insert into OfferCategories (CategoryName)
values ('Hotels');
insert into OfferCategories (CategoryName)
values ('Insurence');
insert into OfferCategories (CategoryName)
values ('Resturent');

create table Offers(

Id int IDENTITY(1,1) PRIMARY KEY,
CategoryId int FOREIGN KEY REFERENCES OfferCategories(Id),
OrganisationName nvarchar(MAX),
Title varchar(100),
PercentageDiscount int,
FixedDiscount int,
OrganisationAddress varchar(225),
City varchar(225),
Email varchar(225),
Phone varchar(100),
NationalValidity BIT,
StartDate datetime,
ExpiryDate datetime,
ImagePath varchar(255),
OfferDescription varchar(MAX)

);
select * from Offers;

select * from News; 

ALTER TABLE Offers ALTER COLUMN title VARCHAR (100);

Alter table Offers Add OfferDescription varchar(MAX);

create table OffersDescription(
id int identity(1,1),
offerId int FOREIGN KEY REFERENCES Offers(id),
priceDescription varchar(Max),
description1 varchar(Max),
description2 varchar(Max),
description3 varchar(Max)

);
drop table OffersDescription;