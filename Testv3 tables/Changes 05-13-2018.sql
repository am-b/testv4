use Testv3

--Drop TypeOfIncident column
ALTER TABLE [CounsellingForm]
DROP COLUMN [Case];

--create seperate tables

/****** Object:  Table [dbo].[[CounsellingFormCasesList]]   ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CounsellingFormCasesList](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[CounsellingFormCasesList] ([Type]) VALUES ('School Adjustment'); 
INSERT INTO [dbo].[CounsellingFormCasesList] ([Type]) VALUES ('Intrapersonal Relationship'); 
INSERT INTO [dbo].[CounsellingFormCasesList] ([Type]) VALUES ('Interpersonal Relationship'); 
INSERT INTO [dbo].[CounsellingFormCasesList] ([Type]) VALUES ('Emotional Issue'); 
INSERT INTO [dbo].[CounsellingFormCasesList] ([Type]) VALUES ('Physical Issue'); 
INSERT INTO [dbo].[CounsellingFormCasesList] ([Type]) VALUES ('Family Problems'); 
INSERT INTO [dbo].[CounsellingFormCasesList] ([Type]) VALUES ('Career Guidance'); 


/****** Object:  Table [dbo].[CounsellingFormCases] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CounsellingFormCases](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[CounsellingFormID]  [int] NOT NULL,
	[TypeID] [int] NOT NULL

PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CounsellingFormCases]  WITH CHECK ADD FOREIGN KEY([CounsellingFormID])
REFERENCES [dbo].[CounsellingForm] ([CounsellingFormID])
GO

ALTER TABLE [dbo].[CounsellingFormCases]  WITH CHECK ADD FOREIGN KEY([TypeID])
REFERENCES [dbo].[CounsellingFormCasesList]([TypeID])
GO


--Add column [STUDENT] table
ALTER TABLE [STUDENT]
ADD IsActive [bit] NULL
