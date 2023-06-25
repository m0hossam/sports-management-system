-- Sports Management System --
-- SportsSystemDB --

/* Updates:
-Added 	Delete Match proc.
*/

go 

-- PROCEDURES ######################################################################################

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
		end_time as start_time + '02:00:00',
		attendes_number int, --should be incremented whenever purchaseTicket is called
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
		match_id int,
		constraint PK_Ticket primary key (ticket_id),
		constraint FK_Ticket_SportsMatch foreign key (match_id) references SportsMatch on delete cascade
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

	create table HostRequest
	(
		request_id int identity,
		club_rep_id int,
		stadium_id int,
		match_id int,
		is_approved bit,
		constraint PK_HostRequest primary key (request_id),
		constraint FK_HostRequest_ClubRep foreign key (club_rep_id) references ClubRep,
		constraint FK_HostRequest_Stadium foreign key (stadium_id) references Stadium,
		constraint FK_HostRequest_SportsMatch foreign key (match_id) references SportsMatch on delete cascade
	);

	create table TicketTransaction
	(
		transaction_id int identity,
		ticket_id int, 
		match_id int, 
		fan_id varchar(20),
		constraint PK_TicketTransaction primary key (transaction_id),
		constraint FK_TicketTransaction_Ticket foreign key (ticket_id) references Ticket,
		constraint FK_TicketTransaction_SportsMatch foreign key (match_id) references SportsMatch on delete cascade,
		constraint FK_TicketTransaction_Fan foreign key (fan_id) references Fan
	);
-------------------------

go

-- Drop All Tables --
create proc dropAllTables as

	drop table TicketTransaction;
	drop table HostRequest;
	drop table StadiumManager;
	drop table Ticket;
	drop table SportsMatch;
	drop table ClubRep;	
	drop table Club;
	drop table Stadium;
	drop table AssociationManager;
	drop table Fan;
	drop table SystemAdmin;
---------------------

go

-- Drop All Procedures/Functions/Views --
create proc dropAllProceduresFunctionsViews as

	drop proc createAllTables;
	drop proc dropAllTables;
	drop proc clearAllTables;
	drop proc addAssociationManager; 
	drop proc addNewMatch;
	drop proc addClub;
	drop proc addStadium;
	drop proc addStadiumManager;
	drop proc addHostRequest;
	drop proc acceptRequest;
	drop proc deleteMatchesOnStadium;
	drop proc addTicket;

	drop view allAssocManager;
	drop view allClubRepresentatives;
	drop view allStadiumManagers;
	drop view allFans;
	drop view allMatches;
	drop view allTickets;
	drop view allClubs;
	drop view allStadiums;
	drop view allRequests;
	drop view clubsWithNoMatches;
	drop view matchesPerTeam;
	drop view clubsNeverMatched;

	drop function viewAvailableStadiumsOn;
	drop function allUnassignedMatches;
	drop function clubsNeverPlayed;
	drop function matchWithHighestAttendance;
	drop function matchesRankedByAttendance;
	drop function requestsFromClub;
-----------------------------------------

go

-- Clear All Tables --
create proc clearAllTables as
	
	-- Drop FKs first --
	alter table TicketTransaction drop constraint FK_TicketTransaction_Fan;
	alter table TicketTransaction drop constraint FK_TicketTransaction_SportsMatch;
	alter table TicketTransaction drop constraint FK_TicketTransaction_Ticket;
	alter table HostRequest drop constraint FK_HostRequest_Stadium;
	alter table HostRequest drop constraint FK_HostRequest_ClubRep;
	alter table StadiumManager drop constraint FK_StadiumManager_Stadium;
	alter table Ticket drop constraint FK_Ticket_SportsMatch;
	alter table SportsMatch drop constraint FK_SportsMatch_Stadium;
	alter table SportsMatch drop constraint FK_SportsMatch_HomeClub;
	alter table SportsMatch drop constraint FK_SportsMatch_AwayClub;
	alter table SportsMatch drop constraint FK_SportsMatch_AssociationManager;
	alter table ClubRep drop constraint FK_ClubRep_Club;	
	alter table Club drop constraint FK_Club_SystemAdmin;
	alter table Stadium drop constraint FK_Stadium_SystemAdmin;

	-- Clear tables --
	truncate table TicketTransaction;
	truncate table HostRequest;
	truncate table StadiumManager;
	truncate table Ticket;
	truncate table SportsMatch;
	truncate table ClubRep;	
	truncate table Club;
	truncate table Stadium;
	truncate table AssociationManager;
	truncate table Fan;
	truncate table SystemAdmin;

	-- Add FKs again --
	alter table TicketTransaction add constraint FK_TicketTransaction_Fan foreign key (fan_id) references Fan;
	alter table TicketTransaction add constraint FK_TicketTransaction_SportsMatch foreign key (match_id) references SportsMatch;
	alter table TicketTransaction add constraint FK_TicketTransaction_Ticket foreign key (ticket_id) references Ticket;
	alter table HostRequest add constraint FK_HostRequest_Stadium foreign key (stadium_id) references Stadium;
	alter table HostRequest add constraint FK_HostRequest_ClubRep foreign key (club_rep_id) references ClubRep;
	alter table StadiumManager add constraint FK_StadiumManager_Stadium foreign key (stadium_id) references Stadium;
	alter table Ticket add constraint FK_Ticket_SportsMatch foreign key (match_id) references SportsMatch;
	alter table SportsMatch add constraint FK_SportsMatch_HomeClub foreign key (home_club_id) references Club;
	alter table SportsMatch add constraint FK_SportsMatch_AwayClub foreign key (away_club_id) references Club;
	alter table SportsMatch add constraint FK_SportsMatch_Stadium foreign key (stadium_id) references Stadium;
	alter table SportsMatch add constraint FK_SportsMatch_AssociationManager foreign key (assoc_manager_username) references AssociationManager;
	alter table ClubRep add constraint FK_ClubRep_Club foreign key (club_id) references Club;
	alter table Club add constraint FK_Club_SystemAdmin foreign key (admin_username) references SystemAdmin;
	alter table Stadium add constraint FK_Stadium_SystemAdmin foreign key (admin_username) references SystemAdmin;
----------------------

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

-- Add Match --
create proc addNewMatch
	@club1 varchar(20),
	@club2 varchar(20),
	@host_club varchar(20),
	@match_time datetime
as
	declare @host_id int = (select club_id from Club where full_name = @host_club);
	declare @guest_id int = (select club_id from Club where club_id <> @host_id and (full_name = @club1 or full_name = @club2));
	insert into SportsMatch(start_time, home_club_id, away_club_id)
	values(@match_time, @host_id, @guest_id);
---------------

go

-- Add Club --
create proc addClub
	@name varchar(20),
	@location varchar(20)
as
	insert into Club(full_name, club_location)
	values(@name, @location);
--------------

go

-- Add Stadium --
create proc addStadium
	@name varchar(20),
	@location varchar(20),
	@capacity int
as
	insert into Stadium(full_name, stad_location, capacity)
	values(@name, @location, @capacity);
-----------------

go

-- Add Stadium Manager --
create proc addStadiumManager
	@name varchar(20),
	@stad_name varchar(20),
	@username varchar(20),
	@password varchar(20)
as
	declare @stad_id int = (select stadium_id from Stadium where full_name = @stad_name);
	insert into StadiumManager
	values(@stad_id, @username, @password, @name);
-------------------------

go 

-- Add Host Request --
create proc addHostRequest
	@club_name varchar(20),
	@stadium_name varchar(20),
	@start_time datetime
as
	declare @host_id int = (select club_id from Club where full_name = @club_name);
	declare @club_rep_id int = (select club_rep_id from ClubRep where club_id = @host_id);
	declare @stadium_id int = (
		select S.stadium_id
		from Stadium S
		inner join StadiumManager SM on S.stadium_id = SM.stadium_manager_id
		where S.full_name = @stadium_name
	);
	declare @match_id int = (select * from SportsMatch where home_club_id = @host_id and start_time = @start_time);
	insert into HostRequest(club_rep_id, stadium_id, match_id)
	values(@club_rep_id, @stadium_id, @match_id);
----------------------

go

-- Accept Host Request --
create proc acceptRequest
	@stad_manager_name varchar(20),
	@host_club_name varchar(20),
	@guest_club_name varchar(20),
	@start_time datetime
as
	declare @host_id int = (select club_id from Club where full_name = @host_club_name);
	declare @guest_id int = (select club_id from Club where full_name = @guest_club_name);
	declare @match_id int = (select match_id from SportsMatch where home_club_id = @host_id and away_club_id = @guest_id and start_time = @start_time);
	declare @request_id int = (select request_id from HostRequest where match_id = @match_id);
	declare @stadium_id int = (select stadium_id from HostRequest where request_id = @request_id);
	update HostRequest set is_approved = 1 where request_id = @request_id;
	update SportsMatch set stadium_id = @stadium_id where match_id = @match_id;
-------------------------

go

-- Delete Matches On Stadium --
create proc deleteMatchesOnStadium
	@stad_name varchar(20)
as
	delete from SportsMatch where match_id in 
	(
		select SM.match_id
		from SportsMatch SM
		inner join Stadium S on SM.stadium_id = S.stadium_id
		where S.full_name = @stad_name and SM.start_time > GETDATE()
	);
-------------------------------

go

-- Add Ticket --
create proc addTicket
	@host_clup_name varchar(20),
	@competing_club_name varchar(20),
	@start_time datetime
as
	declare @host_clup_id int=(select club_id from Club where full_name=@host_clup_name);
	declare @competing_club_id int=(select club_id from Club where full_name=@competing_club_name);
	declare @match_id int=(select SportsMatch.match_id from SportsMatch where SportsMatch.home_club_id=@host_clup_id and SportsMatch.away_club_id=@competing_club_id and SportsMatch.start_time=@start_time);
	insert into Ticket values (1, @match_id);
-------------------------------

go

-- Delete Match On Stadium --
create proc deleteMatchesOnStadium
	@stad_name varchar(20)
as
	declare @stad_id int=(select Stadium.stadium_id from Stadium where @stad_name=Stadium.full_name);
	delete from SportsMatch where SportsMatch.stadium_id=@stad_id and SportsMatch.start_time >= GETDATE();
-----------------------------
go

-- Delete Match --
create proc deleteMatch
	@first_club varchar(20),
	@second_club varchar(20),
	@host_club varchar(20)
as	
	declare @first_id int = (select club_id from Club where Club.full_name=@first_club);
	declare @second_id int = (select club_id from Club where Club.full_name=@second_club);
	declare @host_id int = (select club_id from Club where Club.full_name=@host_club);
	delete from SportsMatch where ((SportsMatch.home_club_id = @first_id and SportsMatch.away_club_id = @second_id) or ( SportsMatch.home_club_id = @second_id and SportsMatch.away_club_id = @first_id)) and SportsMatch.home_club_id = @host_id;
----------------------

go

-- VIEWS ###########################################################################################

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

	select C1.full_name as host_club, C2.full_name as guest_club, SM.start_time
	from SportsMatch SM 
	inner join Club C1 on SM.home_club_id = C1.club_id
	inner join Club C2 on SM.away_club_id = C2.club_id;
----------------------

go

-- View All Tickets --
create view allTickets as

	select S.full_name , C1.full_name, C2.full_name ,SM.start_time
	from Ticket as T 
	inner join SportsMatch as SM on T.match_id = SM.match_id
	inner join Stadium as S on S.stadium_id = SM.stadium_id
	inner join Club as C1 on SM.home_club_id = C1.club_id
	inner join Club as C2 on SM.home_club_id = C2.club_id;
-------------------------

go

-- View All Clubs --
create view allClubs as

	select C.full_name, C.club_location
	from Club as C;
-------------------------

go

-- View All Stadiums --
create view allStadiums as

	select S.full_name, S.stad_location, S.capacity, S.availability_status
	from Stadium as S;
-------------------------	

go

-- View All Requests --
create view allRequests as

	select CR.full_name, R.is_approved, SM.full_name
	from HostRequest as R
	inner join ClubRep as CR on CR.club_rep_id=R.club_rep_id
	inner join StadiumManager as SM on R.stadium_manager_id=SM.stadium_manager_id
-------------------------

go

-- View Clubs With No Matches --
create view clubsWithNoMatches as

	select full_name
	from Club
	where not exists
	(
		select * 
		from SportsMatch 
		where club_id = home_club_id or club_id = away_club_id
	);
--------------------------------

go

-- View No. Of Matches Played Per Team --
create view matchesPerTeam as

	select C.full_name, count(DISTINCT SM.match_id) as num_matches
	from Club as C 
	left join SportsMatch as SM on (SM.home_club_id = C.club_id or SM.away_club_id = C.club_id) and SM.end_time < GETDATE() -- and match has already played -> end time ?--
	group by C.full_name;
-----------------------------------------

go

-- View Club Pairs Never Matched --
create view clubsNeverMatched as

	select C1.full_name first_club, C2.full_name second_club
	from Club C1
	inner join Club C2 on C1.club_id > C2.club_id -- The (>) eliminates unordered duplicates
	where not exists(
		select *
		from SportsMatch SM
		where (SM.home_club_id = C1.club_id and SM.away_club_id = C2.club_id) 
		or (SM.home_club_id = C2.club_id and SM.away_club_id = C1.club_id)
	);
-----------------------------------

go

-- FUNCTIONS #######################################################################################

-- Return Available Stadiums On Date --
create function viewAvailableStadiumsOn(@start_time datetime)
	returns table
as
	return
	(
		select full_name, stad_location, capacity
		from Stadium
		where availability_status = 1
		except
		(
			select S.full_name, S.stad_location, S.capacity
			from Stadium S
			inner join SportsMatch SM on S.stadium_id = SM.stadium_id
			where not (@start_time+'03:00:00' <= SM.start_time or @start_time >= SM.end_time+'01:00:00')
		)
	);
-------------------------------------

go

-- Return Unassigned Matches --
create function allUnassignedMatches(@host_club varchar(20))
	returns table
as
	return
	(
		select C2.full_name, SM.start_time
		from SportsMatch SM
		inner join Club C1 on SM.home_club_id = C1.club_id
		inner join Club C2 on SM.away_club_id = C2.club_id and SM.home_club_id = C1.club_id
		where C1.full_name = @host_club and SM.stadium_id is null
	);
-----------------------------

go

-- Return Clubs That Never Played Given Club --
create function clubsNeverPlayed(@given_club_name varchar(20))
	returns @clubs table(club_name varchar(20))
as
begin
	declare @given_club_id int = (
		select club_id
		from Club
		where full_name = @given_club_name
	)
	insert into @clubs
	select full_name
	from Club
	where club_id not in(
		select away_club_id
		from SportsMatch
		where home_club_id = @given_club_id
	)
	and club_id not in(
		select home_club_id
		from SportsMatch
		where away_club_id = @given_club_id
	)
	and club_id <> @given_club_id
	return
end
------------------------------------------------

go

-- Return Match With Highest Attendance --
create function matchWithHighestAttendance()
	returns table
as
	return
	(
		select top 1 C1.full_name as host_club, C2.full_name as guest_club, attendes_number
		from SportsMatch SM 
		inner join Club C1 on SM.home_club_id = C1.club_id
		inner join Club C2 on SM.away_club_id = C2.club_id
		order by attendes_number desc
	);
------------------------------------------

go

-- Return Matches Ranked Desc. By Attendance --
create function matchesRankedByAttendance()
	returns table
as
	return 
	(
		select C1.full_name as host_club, C2.full_name as guest_club, attendes_number
		from SportsMatch SM 
		inner join Club C1 on SM.home_club_id = C1.club_id
		inner join Club C2 on SM.away_club_id = C2.club_id
		where GETDATE() > SM.end_time
		order by attendes_number desc
	);
------------------------------------------

go

-- Return Club Requests On Stadium --
create function requestsFromClub(@stadium_name varchar(20), @host_name varchar(20))
	returns table
as	
	return
	(
		select C1.full_name as host_club, C2.full_name as guest_club
		from Club C1
		inner join SportsMatch SM on SM.home_club_id = C1.club_id
		inner join Club C2 on SM.away_club_id = C2.club_id
		inner join HostRequest HR on HR.match_id = SM.match_id
		where C1.full_name = @host_name
		and HR.stadium_id = (select stadium_id from Stadium where full_name = @stadium_name)
		and HR.club_rep_id = (select club_rep_id from ClubRep where club_id = C1.club_id)
	);
-------------------------------------

/* TODO :

Procedures(10):

c incomplete
vii
viii
x
xi
xiii
xx
xxi
xxiv
xxv

Functions(3):

xviii
xxii
xxiii

*/
