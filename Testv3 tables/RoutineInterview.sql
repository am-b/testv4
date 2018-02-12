USE [Testv3]
GO

/****** Object:  Table [dbo].[Answers]    Script Date: 1/25/2018 12:41:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoutineInterview](
	[RoutineInterviewID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CompletionDate] [datetime] NULL,
	[Q1] [nvarchar](max) NULL,
	[Q2] [nvarchar](max) NULL,
	[Q3] [nvarchar](max) NULL,
	[Q4] [nvarchar](max) NULL,
	[Q5] [nvarchar](max) NULL,
	[OtherMatters] [nvarchar](max) NULL,


PRIMARY KEY CLUSTERED 
(
	[RoutineInterviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RoutineInterview]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Student] ([UserID])
ON DELETE CASCADE
GO
