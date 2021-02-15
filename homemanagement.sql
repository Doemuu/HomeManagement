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
    IsFavourite tinyint not null,
    IsDeleted tinyint not null,
    Primary Key (Id)
);