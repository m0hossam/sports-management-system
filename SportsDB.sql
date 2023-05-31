-- Sports Management System --


-- Create All Tables --
create proc createAllTables as

begin

create table SystemAdmin(
username varchar(20),
pass varchar(20)
constraint PK_SystemAdmin primary key (username)
);

create table Fan(
national_id varchar(20), 
username varchar(20), 
pass varchar(20), 
full_name varchar(20), 
phone varchar(20), 
full_address varchar(20),
birth_date date
constraint PK_Fan primary key (national_id)
);

create table AssociationManager(
username varchar(20),
pass varchar(20)
constraint PK_AssociationManager primary key (username)
);

create table Stadium(
stadium_id int identity, 
full_name varchar(20), 
stad_location varchar(20), 
capacity int, 
availability_status bit, 
admin_username varchar(20)
constraint PK_Stadium primary key (stadium_id)
constraint FK_Stadium_SystemAdmin foreign key (admin_username) references SystemAdmin
);

create table SportsMatch(
match_id int identity, 
start_time datetime, 
end_time datetime, 
attendes_number int, 
stadium_id int, 
assoc_manager_username varchar(20)
constraint PK_SportsMatch primary key (match_id)
constraint FK_SportsMatch_Stadium foreign key (stadium_id) references Stadium
constraint FK_SportsMatch_AssociationManager foreign key (assoc_manager_username) references AssociationManager
);

create table Ticket(
ticket_id int identity, 
availability_status bit, 
stadium_id int
constraint PK_Ticket primary key (ticket_id)
constraint FK_Ticket_Stadium foreign key (stadium_id) references Stadium
);

create table Club(
club_id int identity, 
full_name varchar(20), 
club_location varchar(20), 
admin_username varchar(20)
constraint PK_Club primary key (club_id)
constraint FK_Club_SystemAdmin foreign key (admin_username) references SystemAdmin
);

create table AdminBlocksFan(
sys_admin_username varchar(20), 
fan_national_id varchar(20)
constraint PK_AdminBlocksFan primary key (sys_admin_username, fan_national_id)
constraint FK_AdminBlocksFan_SystemAdmin foreign key (sys_admin_username) references SystemAdmin
constraint FK_AdminBlocksFan_Fan foreign key (fan_national_id) references Fan
);

create table StadiumManager(
stadium_manager_id int identity,
stadium_id int,
username varchar(20), 
pass varchar(20), 
full_name varchar(20)
constraint PK_StadiumManager primary key (stadium_manager_id, stadium_id)
constraint FK_StadiumManager_Stadium foreign key (stadium_id) references Stadium
);

create table ClubRep(
club_rep_id int identity,
club_id int,
username varchar(20), 
pass varchar(20), 
full_name varchar(20)
constraint PK_ClubRep primary key (club_rep_id, club_id)
constraint FK_ClubRep_Club foreign key (club_id) references Club
);

create table ClubsPlayMatch(
match_id int, 
home_club_id int,
away_club_id int
constraint PK_ClubsPlayMatch primary key (match_id, home_club_id, away_club_id)
constraint FK_ClubsPlayMatch_Club foreign key (home_club_id) references Club
constraint FK_ClubsPlayMatch_Club foreign key (away_club_id) references Club
constraint FK_ClubsPlayMatch_SportsMatch foreign key (match_id) references SportsMatch
);

create table ManagerRequestsRep(
clup_rep_id int, 
stadium_manager_id int, 
is_approved bit
constraint PK_ManagerRequestsRep primary key (club_rep_id, stadium_manager_id)
constraint FK_ManagerRequestsRep_ClubRep foreign key (club_rep_id) references ClubRep
constraint FK_ManagerRequestsRep_StadiumManager foreign key (stadium_manager_id) references StadiumManager
);

create table TicketTransaction(
ticket_id int, 
match_id int, 
fan_id varchar(20)
constraint PK_TicketTransaction primary key (ticket_id, match_id, fan_id)
constraint FK_TicketTransaction_Ticket foreign key (ticket_id) references Ticket
constraint FK_TicketTransaction_SportsMatch foreign key (match_id) references SportsMatch
constraint FK_TicketTransaction_Fan foreign key (fan_id) references Fan
);

end
-------------------------


go

-- Drop All Tables --
create proc dropAllTables as

begin

drop table SystemAdmin;
drop table Fan;
drop table AssociationManager;
drop table Stadium;
drop table SportsMatch;
drop table Ticket;
drop table Club;
drop table AdminBlocksFan;
drop table StadiumManager;
drop table ClubRep;
drop table ClubsPlayMatch;
drop table ManagerRequestsRep;
drop table TicketTransaction;

end
---------------------