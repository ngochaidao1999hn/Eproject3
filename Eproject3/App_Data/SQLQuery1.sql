create table clients (
	id int primary key identity(1,1),
	FullName nvarchar(50),
	accNumber nvarchar(15),
	IdentificationId nvarchar(15),
	BankId int foreign key references Banks(id),
	balance real,
	PhoneNumber nvarchar(50),
	tokenkey nvarchar(50) null,
	expdate datetime null
)
drop table clients
drop table transactions
create table Banks(
	id int primary key identity(1,1),
	BankName nvarchar(50),
)

create table transactions(
	id int primary key identity(1,1),
	senderId int foreign key references clients(id),
	receiverId int foreign key references clients(id),
	amount real
)

insert into Banks("BankName") values('BIDV'),('VietCom Bank'),('ACB'),('TechCom Bank'),('VietTin Bank'),('MB'),('Agribank'),('Vp bank')
update  clients set balance=200000
 where id = 8
select * from banks
select * from clients