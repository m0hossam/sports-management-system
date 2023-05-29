System_User(Username , password);
Systemp_Usser.username is PK

Fan(Fan_Id, System_user.Username, name, phone, address, birth_date)
Fan.Fan_Id is PK
Fan.Systemp_Usser.username is FK

System_Admin(Admin_Id, System_user.Username)
System_Admin.Admin_Id is PK
System_Admin.Systemp_Usser.username is FK

Block(System_Admin.Admin_Id, Fan.Fan_Id, Blocked)
Block.Fan_Id is PK
Block.System_Admin.Admin_Id is FK

Stadium(Stad_Id, name, location, capacity, status, System_Admin.Admin_Id)
Stadium.Stad_Id is PK
Stadium.System_Admin.Admin_Id is FK

Stadium_Manager(Stad_Manager_Id, Stadium.Stad_Id, System_user.Username ,name)
Stadium_Manager.Stad_Manager_Id and Stadium_Manager.Stadium.Stad_Id are PK
Stadium_Manager.System_user.Username is FK

Association_Manager(System_user.Username, Association_Manager_Id)
Association_Manager.Association_Manager_Id is PK

Match(Match_Id, Association_Manager.Association_Manager_Id, st_time, en_time, attendes_number, Stadium.Stad_Id)
Match.Match_Id is PK
Match.Association_Manager.Association_Manager_Id and Stadium.Stad_Id is FK

Ticket(Ticket_Id, Fan.Fan_Id, status, stadium)
Ticket.Ticket_Id is PK
Ticket.Fan.Fan_Id is FK

Club(Club_Id, name, location, System_Admin.Admin_Id)
Club.Club_Id is PK
Club.System_Admin.Admin_Id is FK

Plays(Match.Match_Id, Club.Club_Id_first, Club.Club_Id_second, Host_club)
Plays.Match.Match_Id is PK
Club.Club_Id_first and  Club.Club_Id_second is FK


Club_Representative(Clup_Rep_Id, System_user.Username , Club.Club_id, name)
Club_Representative.Clup_Rep_Id is PK
Club_Representative.System_user.Username and Club.Club_id is FK

Request(Club_Representative.Clup_Rep_Id, Stadium_Manager.Stad_Manager_Id, is_approved)
Request.Club_Representative.Clup_Rep_Id and Request.Stadium_Manager.Stad_Manager_Id is PK















