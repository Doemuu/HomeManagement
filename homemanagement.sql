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
Create table User (
	Id int not null auto_increment,
    FirstName varchar(255) not null,
    LastName varchar(255) not null,
    UserName varchar(255) not null,
    UserPassword varchar(255) not null,
    Primary Key (Id)
);
Create table RefreshToken (
	Id int not null auto_increment,
    UserId int not null,
    Token varchar(255) not null,
    IsRevoked tinyint not null,
    ExpiresOn datetime not null,
    CreatedAt datetime not null,
    UpdatedAt datetime not null,
    Primary Key (Id),
    foreign key (UserId) References User(Id)
);