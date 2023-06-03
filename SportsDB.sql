-- Sports Management System --


-- Create All Tables --
create proc createAllTables as

	create table SystemAdmin
	(
		username varchar(20),
		pass varchar(20),
		constraint PK_SystemAdmin primary key (username)
	);

	create table Fan
	(
		national_id varchar(20), 
		username varchar(20), 
		pass varchar(20), 
		full_name varchar(20), 
		phone varchar(20), 
		full_address varchar(20),
		birth_date date,
		fan_status bit,
		constraint PK_Fan primary key (national_id)
	);

	create table AssociationManager
	(
		username varchar(20),
		full_name varchar(20),
		pass varchar(20),
		constraint PK_AssociationManager primary key (username)
	);

	create table Stadium
	(
		stadium_id int identity, 
		full_name varchar(20), 
		stad_location varchar(20), 
		capacity int, 
		availability_status bit, 
		admin_username varchar(20),
		constraint PK_Stadium primary key (stadium_id),
		constraint FK_Stadium_SystemAdmin foreign key (admin_username) references SystemAdmin
	);

	create table Club
	(
		club_id int identity, 
		full_name varchar(20), 
		club_location varchar(20), 
		admin_username varchar(20),
		constraint PK_Club primary key (club_id),
		constraint FK_Club_SystemAdmin foreign key (admin_username) references SystemAdmin
	);

	create table ClubRep
	(
		club_rep_id int identity,
		club_id int,
		username varchar(20), 
		pass varchar(20), 
		full_name varchar(20),
		constraint PK_ClubRep primary key (club_rep_id),
		constraint FK_ClubRep_Club foreign key (club_id) references Club
	);

	create table SportsMatch
	(
		match_id int identity, 
		start_time datetime, 
		end_time datetime, 
		attendes_number int,
		home_club_id int,
		away_club_id int,
		stadium_id int, 
		assoc_manager_username varchar(20),
		constraint PK_SportsMatch primary key (match_id),
		constraint FK_SportsMatch_HomeClub foreign key (home_club_id) references Club,
		constraint FK_SportsMatch_AwayClub foreign key (away_club_id) references Club,
		constraint FK_SportsMatch_Stadium foreign key (stadium_id) references Stadium,
		constraint FK_SportsMatch_AssociationManager foreign key (assoc_manager_username) references AssociationManager
	);

	create table Ticket
	(
		ticket_id int identity, 
		availability_status bit, 
		stadium_id int,
		constraint PK_Ticket primary key (ticket_id),
		constraint FK_Ticket_Stadium foreign key (stadium_id) references Stadium
	);

	create table StadiumManager
	(
		stadium_manager_id int identity,
		stadium_id int,
		username varchar(20), 
		pass varchar(20), 
		full_name varchar(20),
		constraint PK_StadiumManager primary key (stadium_manager_id),
		constraint FK_StadiumManager_Stadium foreign key (stadium_id) references Stadium
	);

	create table RepRequestsStadium
	(
		request_id int identity,
		club_rep_id int, 
		stadium_manager_id int, 
		is_approved bit,
		constraint PK_RepRequestsStadium primary key (request_id),
		constraint FK_RepRequestsStadium_ClubRep foreign key (club_rep_id) references ClubRep,
		constraint FK_RepRequestsStadium_StadiumManager foreign key (stadium_manager_id) references StadiumManager
	);

	create table TicketTransaction
	(
		transaction_id int identity,
		ticket_id int, 
		match_id int, 
		fan_id varchar(20),
		constraint PK_TicketTransaction primary key (transaction_id),
		constraint FK_TicketTransaction_Ticket foreign key (ticket_id) references Ticket,
		constraint FK_TicketTransaction_SportsMatch foreign key (match_id) references SportsMatch,
		constraint FK_TicketTransaction_Fan foreign key (fan_id) references Fan
	);
-------------------------

go

-- Drop All Tables --
create proc dropAllTables as

	drop table SystemAdmin;
	drop table Fan;
	drop table AssociationManager;
	drop table Stadium;
	drop table SportsMatch;
	drop table Ticket;
	drop table Club;
	drop table ClubRep;
	drop table StadiumManager;
	drop table RepRequestsStadium;
	drop table TicketTransaction;
---------------------

go

-- Add Assoc. Manager --
create proc addAssociationManager
	@name varchar(20), 
	@username varchar(20), 
	@password varchar(20)
as
	insert into AssociationManager
	values(@username, @name, @password);
------------------------

go

-- View All Assoc. Managers --
create view allAssocManager as

	select username, full_name
	from AssociationManager;
------------------------------

go

-- View All Club Rep.s --
create view allClubRepresentatives as

	select CR.username, CR.full_name, C.full_name as club_name
	from ClubRep CR inner join Club on CR.club_id = C.club_id;
-------------------------

go

-- View All Stadium Managers --
create view allStadiumManagers as

	Select SM.username, SM.full_name, S.full_name AS stadium_name
	from StadiumManager SM inner join Stadium S on SM.stadium_id = S.stadium_id;
-------------------------------

go

-- View All Fans --
create view allFans as
	
	select full_name, national_id, birth_date, fan_status 
	from Fan;
-------------------

go

-- View All Matches --
create view allMatches as

	select C1.full_name as first_club, C2.full_name as second_club, C1.full_name as host_club, SM.start_time
	from SportsMatch SM 
	inner join Club C1 on SM.home_club_id = C1.club_id
	inner join Club C2 on SM.away_club_id = C2.club_id
	where SM.home_club_id = C1.club_id and SM.away_club_id = C2.club_id;
----------------------