use PRUEBAS_DIEGO
GO


if not exists(select * from sysobjects where name = 'version' and xtype = 'u')
begin

CREATE TABLE version (
    VersionNumber bigint NOT NULL,
)

INSERT INTO version (VersionNumber)
VALUES (0);

end

GO



--select * from version

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET XACT_ABORT ON
GO
BEGIN TRAN

DECLARE @newVersion INT
SET @newVersion = 1

 -- version number here


IF NOT EXISTS ( SELECT  *
                FROM    dbo.sysobjects
                WHERE   id = OBJECT_ID(N'[dbo].[Version]')
                        AND OBJECTPROPERTY(id, N'IsUserTable') = 1 ) 
    RAISERROR('Update to version %d cannot be run - actual version is 0', 1, 127, @newVersion) WITH LOG

DECLARE @actualVersion INT
SELECT  @actualVersion = ISNULL(MAX(VersionNumber), 0)
FROM    Version
IF @actualVersion <> @newVersion - 1 
    RAISERROR('Update to version %d cannot be run - actual version is %d', 1, 127, @newVersion, @actualVersion) WITH LOG
	
UPDATE  [dbo].[Version]
SET     [VersionNumber] = @newVersion
	
RAISERROR('Updating to version %d...', 1, 1, @newVersion) WITH LOG
	-- START	
GO
---------------------------------------------------------------------------------

if not exists(select * from sysobjects where name = 'RinkuEmployees' and xtype = 'u')
begin

CREATE TABLE RinkuEmployees (
    id bigint identity(1,1) NOT NULL,
	employeeNumber bigint NOT NULL,
    firstName varchar(250) NOT NULL,
	secondName varchar(250) NOT NULL,
	registrationDate datetime not null,
	leaveDate datetime null,
    roleId int NOT NULL,
	IsActive bit NOT NULL
 
)

end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuEmployees_GetAll]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuEmployees_GetAll]
GO

CREATE proc RinkuEmployees_GetAll
as
begin
	select * from RinkuEmployees with(nolock)
end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuEmployees_Insert]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuEmployees_Insert]
GO

create proc [dbo].[RinkuEmployees_Insert]
@employeeNumber bigint ,
@firstName varchar(250) ,
@secondName varchar(250) ,
@registrationDate datetime ,
@roleId int
as
begin
	insert into [RinkuEmployees] (employeeNumber, firstName, secondName, registrationDate,leaveDate, roleId,IsActive)
	values ( @employeeNumber, @firstName, @secondName, @registrationDate, NULL, @roleId,1)
	select @@IDENTITY
end

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuEmployees_Update]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuEmployees_Update]
GO

create proc [dbo].[RinkuEmployees_Update]
@id bigint,
@employeeNumber bigint ,
@firstName varchar(250) ,
@secondName varchar(250) ,
@registrationDate datetime ,
@leaveDate datetime ,
@roleId int 
as
begin
	update [RinkuEmployees]
	set 
	employeeNumber = @employeeNumber, 
	firstName = @firstName, 
	secondName = @secondName, 
	registrationDate = @registrationDate, 
	leaveDate = @leaveDate,
	roleId = @roleId
	Where id = @id
end

go

if not exists(select * from sysobjects where name = 'RinkuRoles' and xtype = 'u')
begin

CREATE TABLE RinkuRoles (
    id int identity NOT NULL,
    roleName varchar(250) NOT NULL,
	payPerHour money NOT NULL,
	payPerDelivery money NOT NULL,
	payBonus money NOT NULL,
	hoursPerDay int NOT NULL,
	daysPerWeek int NOT NULL,
	isActive bit NOT NULL
)

INSERT INTO RinkuRoles (roleName, payPerHour,payPerDelivery, payBonus,hoursPerDay,daysPerWeek,isActive)
VALUES ('Chofer','30','5','10',8,6,1);

INSERT INTO RinkuRoles (roleName, payPerHour,payPerDelivery, payBonus,hoursPerDay,daysPerWeek,isActive)
VALUES ('Cargador','30','5','5',8,6,1);

INSERT INTO RinkuRoles (roleName, payPerHour,payPerDelivery, payBonus,hoursPerDay,daysPerWeek,isActive)
VALUES ('Auxiliar','30','5','0',8,6,1);

end

GO

if not exists(select * from sysobjects where name = 'RinkuTaxes' and xtype = 'u')
begin

CREATE TABLE RinkuTaxes (
    id int identity NOT NULL,
    taxName varchar(250) NOT NULL,
	baseTax decimal(4,2) NOT NULL,
	secondaryTax decimal(4,2) NOT NULL,
	isActive bit NOT NULL
)

INSERT INTO RinkuTaxes (taxName, baseTax,secondaryTax, isActive)
VALUES ('ISR','0.09', '0.03', 1);

end

if not exists(select * from sysobjects where name = 'RinkuPantryVouchers' and xtype = 'u')
begin

CREATE TABLE RinkuPantryVouchers (
    id int identity NOT NULL,
    voucherName varchar(250) NOT NULL,
	percentVoucher decimal(4,2) NOT NULL,
	isActive bit NOT NULL
)

INSERT INTO RinkuPantryVouchers (voucherName, percentVoucher,isActive)
VALUES ('Vales de despensa','0.04',1);

end



GO

if not exists(select * from sysobjects where name = 'RinkuMovementsControl' and xtype = 'u')
begin

CREATE TABLE RinkuMovementsControl (
	id bigint identity NOT NULL,
	employeeId bigint NOT NULL,
	quantityOfDeliveries int NOT NULL,
	registrationDate datetime not null,
	monthNumber int NOT NULL,
	monthlyWorkedHours int NOT NULL,
	subTotal money NOT NULL, 
    monthlyPayPerDeliveries money NOT NULL,
	monthlyPayPerBonus money NOT NULL,
	monthlyRetention  money NOT NULL,
	monthlyVouchers  money NOT NULL,
	totalPayed  money NOT NULL
)
 end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuMovementsControl_GetAll]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuMovementsControl_GetAll]
GO

CREATE proc RinkuMovementsControl_GetAll
as
begin
	select rm.id,rm.employeeId,re.employeeNumber,re.firstName, re.secondName, re.roleId, re.IsActive,rm.RegistrationDate,rm.quantityOfDeliveries, rm.monthNumber,
	rm.monthlyWorkedHours, rm.monthlyPayPerDeliveries, rm.monthlyPayPerBonus, rm.subTotal, rm.monthlyRetention, 
	rm.monthlyVouchers, rm.totalPayed from RinkuMovementsControl rm with(nolock)   
    inner join RinkuEmployees re with(nolock) on rm.employeeId = re.id 
end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuMovementsControl_GetById]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuMovementsControl_GetById]
GO

CREATE proc RinkuMovementsControl_GetById
@id bigint
as
begin
	select rm.id,rm.employeeId,re.employeeNumber,re.firstName, re.secondName, re.roleId, re.IsActive,rm.quantityOfDeliveries, rm.monthNumber,
	rm.RegistrationDate,rm.monthlyWorkedHours, rm.monthlyPayPerDeliveries, rm.monthlyPayPerBonus, rm.subTotal, rm.monthlyRetention, 
	rm.monthlyVouchers, rm.totalPayed from RinkuMovementsControl rm with(nolock)   
    inner join RinkuEmployees re with(nolock) on rm.employeeId = re.id 
	where rm.id = @id

end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuMovementsControl_Insert]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuMovementsControl_Insert]
GO

create proc [dbo].[RinkuMovementsControl_Insert]
@employeeId bigint ,
@quantityOfDeliveries int,
@registrationDate datetime ,
@monthNumber int ,
@monthlyWorkedHours int,
@subTotal money , 
@monthlyPayPerDeliveries money  ,
@monthlyPayPerBonus money  ,
@monthlyRetention  money  ,
@monthlyVouchers  money  ,
@totalPayed  money  
as
begin
	insert into [RinkuMovementsControl] (employeeId,quantityOfDeliveries, registrationDate,  monthNumber, 
	monthlyWorkedHours, subTotal, monthlyPayPerDeliveries, monthlyPayPerBonus, monthlyRetention,monthlyVouchers,totalPayed)
	values (@employeeId, @quantityOfDeliveries,@registrationDate, @monthNumber, 
	@monthlyWorkedHours, @subTotal, @monthlyPayPerDeliveries, @monthlyPayPerBonus, @monthlyRetention,@monthlyVouchers,@totalPayed)
	select @@IDENTITY
end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuMovementsControl_Update]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuMovementsControl_Update]
GO

create proc [dbo].[RinkuMovementsControl_Update]
@id bigint,
@employeeId bigint ,
@quantityOfDeliveries int,
@registrationDate datetime ,
@monthNumber int ,
@monthlyWorkedHours int ,
@subTotal money , 
@monthlyPayPerDeliveries money ,
@monthlyPayPerBonus money ,
@monthlyRetention  money ,
@monthlyVouchers  money ,
@totalPayed  money 
as
begin
	update [RinkuMovementsControl]
	set 
	employeeId = @employeeId, 
	quantityOfDeliveries = @quantityOfDeliveries,
	registrationDate = @registrationDate, 
	monthNumber = @monthNumber, 
	monthlyWorkedHours = @monthlyWorkedHours, 
	subTotal = @subTotal, 
	monthlyPayPerDeliveries = @monthlyPayPerDeliveries, 
	monthlyPayPerBonus = @monthlyPayPerBonus,
	monthlyRetention = @monthlyRetention,
	monthlyVouchers = @monthlyVouchers,
	totalPayed = @totalPayed
	Where id = @id
end

go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuEmployees_GetAll]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuEmployees_GetAll]
GO
  
create proc [RinkuEmployees_GetAll]  
  
as  
begin  
 select re.*,ro.roleName roleName,ro.payPerHour,ro.payPerDelivery, ro.payBonus,ro.hoursPerDay,ro.daysPerWeek,ro.isActive roleIsActive  from RinkuEmployees re with(nolock)   
 inner join RinkuRoles ro with(nolock) on re.roleId = ro.id  
   
   
end  

GO 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuEmployees_GetById]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuEmployees_GetById]
GO

CREATE proc [RinkuEmployees_GetById]
@id bigint
as
begin
 select re.*,ro.roleName roleName,ro.payPerHour,ro.payPerDelivery, ro.payBonus,ro.hoursPerDay,ro.daysPerWeek,ro.isActive roleIsActive  from RinkuEmployees re with(nolock)   
 inner join RinkuRoles ro with(nolock) on re.roleId = ro.id  
	where re.id = @id
end

GO

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuRoles_GetAll]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuRoles_GetAll]
GO

CREATE proc RinkuRoles_GetAll
as
begin
	select * from RinkuRoles with(nolock) 
end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuTaxes_GetISRData]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuTaxes_GetISRData]
GO

CREATE proc RinkuTaxes_GetISRData
as
begin
	select * from RinkuTaxes with(nolock) 
end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuTaxes_GetISRDataByName]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuTaxes_GetISRDataByName]
GO

CREATE proc RinkuTaxes_GetISRDataByName
@taxName varchar(250)
as
begin
	select * from RinkuTaxes with(nolock)
	where taxName = @taxName
end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuPantryVouchers_GetAll]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuPantryVouchers_GetAll]
GO

CREATE proc RinkuPantryVouchers_GetAll
as
begin
	select * from RinkuPantryVouchers with(nolock) 
end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RinkuTaxes_GetVoucherByName]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[RinkuTaxes_GetVoucherByName]
GO

CREATE proc RinkuTaxes_GetVoucherByName
@voucherName varchar(250)
as
begin
	select * from RinkuPantryVouchers with(nolock)
	where voucherName = @voucherName
end

GO

---------------------------------------------------------------------------------
GO
--	END
COMMIT

select * from RinkuTaxes
select * from RinkuEmployees
select * from RinkuPantryVouchers
select * from RinkuRoles
select * from RinkuMovementsControl




