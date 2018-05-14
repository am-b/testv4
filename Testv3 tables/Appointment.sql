use Testv3

create table Appointment
(
	Apptmnt_ID int primary key,
	Apptmnt_Date date,
	Apptmnt_Time varchar(30),
	StudentID varchar(50),
	StudentEmail varchar(256),
)

