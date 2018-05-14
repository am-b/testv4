use Testv3

--Drop TypeOfIncident column
ALTER TABLE IncidentReport
DROP COLUMN TypeOfIncident;

--create seperate tables

/****** Object:  Table [dbo].[IncidentReportTags]    Script Date: 5/06/2018 12:08:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IncidentReportIncidents](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[IncidentReportIncidents] ([Type]) VALUES ('Incident Type 1'); 
INSERT INTO [dbo].[IncidentReportIncidents] ([Type]) VALUES ('Incident Type 2'); 
INSERT INTO [dbo].[IncidentReportIncidents] ([Type]) VALUES ('Incident Type 3'); 
INSERT INTO [dbo].[IncidentReportIncidents] ([Type]) VALUES ('Incident Type 4'); 
INSERT INTO [dbo].[IncidentReportIncidents] ([Type]) VALUES ('Incident Type 5'); 


/****** Object:  Table [dbo].[IncidentReportTags]    Script Date: 5/06/2018 12:10:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IncidentReportTags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[IncidentReportID]  [int] NOT NULL,
	[TypeID] [int] NOT NULL

PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[IncidentReportTags]  WITH CHECK ADD FOREIGN KEY([IncidentReportID])
REFERENCES [dbo].[IncidentReport] ([IncidentReportID])
GO

ALTER TABLE [dbo].[IncidentReportTags]  WITH CHECK ADD FOREIGN KEY([TypeID])
REFERENCES [dbo].[IncidentReportIncidents] ([TypeID])
GO

