create proc Offers_CreateOffers(
@CategoryId int, @OrganisationName nvarchar(225), @Title nvarchar(100), @PercentageDiscount int, @FixedDiscount int, @OrganisationAddress nvarchar(225),@City nvarchar(225),
@Email nvarchar(225), @Phone nvarchar(100), @NationalValidity BIT, @StartDate datetime,
@ExpiryDate datetime, @ImagePath nvarchar(400) , @OfferDescription varchar(MAX))

As
Begin
	
	 INSERT INTO Offers (CategoryId, OrganisationName, Title, PercentageDiscount, FixedDiscount, OrganisationAddress,City, Email, Phone, NationalValidity, StartDate, ExpiryDate, ImagePath , OfferDescription)
       VALUES (@CategoryId, @OrganisationName, @Title, @PercentageDiscount, @FixedDiscount, @OrganisationAddress,@City, @Email, @Phone, @NationalValidity, @StartDate, @ExpiryDate, @ImagePath, @OfferDescription )


End

create proc OfferCategories_CreateCategories(
@CategoryName varchar(225)
)
As
Begin
Insert into OfferCategories(CategoryName) values (@CategoryName)
end




select * from OfferCategories;

select * from Offers;

Exec OfferCategories_CreateCategories 'Covid-19';
delete from  Offers;

 
Exec Offers_CreateOffers 2,'Oyo Rooms', 'Diwali offers on travels!!!!', 15,0,'Hydrabad', 'Hydrabad', 'oyo@test.com', '123456789', True, '2021-08-15 00:00:00','2022-01-01 23:59:59', 'no_path_given', 'This is the description of the this offers';


Exec Offers_CreateOffers 2,'New Hotel', 'Holi offers on travels!!!!', 15,0,'Chandani Chowk', 'Delhi', 'Newhotel@test.com', '123456789', True, '2021-08-15 00:00:00','2022-01-01 23:59:59', 'no_path_given', 'This ths description column';



create proc Offers_GetOffers
As
Begin

select * from Offers where ExpiryDate>=(select getdate()); 

End

Exec Offers_GetOffers;

create proc Offers_CreateOfferDescription(
@offerId int, @priceDescription nvarchar(Max), @description1 nvarchar(Max), @description2 nvarchar(Max), @description3 nvarchar(Max))
As
Begin
INSERT INTO OffersDescription(offerId, priceDescription, description1, description2, description3)
VALUES (@offerId, @priceDescription, @description1, @description2, @description3)

End

select * from News;