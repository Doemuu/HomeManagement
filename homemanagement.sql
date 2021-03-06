Create Database HomeManagement;
use HomeManagement;
Create table Note (
	NoteId int not null auto_increment,
    NoteTitle varchar(255) not null,
    UploadDate datetime not null,
    Ending varchar(255) not null,
    IsDeleted tinyint not null,
    Primary Key(NoteId)
);
Create table ShoppingList (
	Id int not null auto_increment,
    ItemName varchar(255) not null,
    Section varchar(255) not null,
    Amount int not null,
    Priority int not null,
    IsBought tinyint not null,
    IsFavourite tinyint not null,
    IsDeleted tinyint not null,
    Primary Key (Id)
);
Create table Users (
	Id int not null auto_increment,
    FirstName varchar(255) not null,
    LastName varchar(255) not null,
    UserName varchar(255) not null,
    UserPassword varchar(255) not null,
    IsAdmin tinyint,
    Primary Key (Id)
);
Create table RefreshToken (
	Id int not null auto_increment,
    UserId int not null,
    ExpiresOn datetime not null,
    IsRevoked tinyint,
    Primary Key (Id),
    foreign key (UserId) References Users(Id)
);