USE [Testv3]
GO

/****** Object:  Table [dbo].[Student]    Script Date: 12/23/2017 12:06:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Student](
	[UserID] [nvarchar](128) NOT NULL,
	[StudentID] [nvarchar](50) NULL,
	[StudentLastName] [nvarchar](50) NULL,
	[StudentFirstName] [nvarchar](50) NULL,
	[StudentMiddleName] [nvarchar](50) NULL,
	[StudentEmail] [nvarchar](256) NULL,
	[CourseID] [int] NULL,
	[Address] [nvarchar](256) NULL,
	[Sex] [nchar](10) NULL,
	[Civil Status
CivilStatus] [nchar](10) NULL,
	[Religion] [nchar](10) NULL,
	[Nationality] [nvarchar](50) NULL,
	[Birthdate] [date] NULL,
	[PhoneNumber] [nvarchar](10) NULL,
	[Birthplace] [nvarchar](256) NULL,
	[Dialect] [nvarchar](256) NULL,
	[Hobbies] [nvarchar](max) NULL,
	[BirthRank] [nchar](10) NULL,
	[DistanceFromSchool] [nvarchar](50) NULL,
	[Scholarship] [nvarchar](250) NULL,
	[DateOfMarriage] [date] NULL,
	[PlaceOfMarriage] [nvarchar](250) NULL,
	[SpouseName] [nvarchar](max) NULL,
	[SpouseAge] [nvarchar](2) NULL,
	[SpouseEducationalAttainment] [nvarchar](50) NULL,
	[Occupation] [nvarchar](50) NULL,
	[StudentEmployerAddress] [nvarchar](250) NULL,
	[NumberOfChildren] [nvarchar](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([CourseID])
GO

ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

