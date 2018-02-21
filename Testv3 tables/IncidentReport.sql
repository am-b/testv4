USE [Testv3]
GO

/****** Object:  Table [dbo].[ExitInterview]    Script Date: 2/2/2018 1:39:01 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IncidentReport](
	[IncidentReportID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[StudentUserID] [nvarchar](128) NOT NULL,
	[CompletionDate] [datetime] NULL,

	[TypeOfIncident] [nvarchar](max) NULL,
	[PlaceOfIncident] [nvarchar](max) NULL,
	[DateTimeOfIncident] [datetime] NULL,
	[Witness] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
	[CounsellorNotes] [nvarchar](max) NULL,


PRIMARY KEY CLUSTERED 
(
	[IncidentReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[IncidentReport]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Counsellor] ([UserID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[IncidentReport]  WITH CHECK ADD FOREIGN KEY([StudentUserID])
REFERENCES [dbo].[Student] ([UserID])
GO


