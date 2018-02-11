USE [Testv3]
GO

/****** Object:  Table [dbo].[Answers]    Script Date: 1/25/2018 12:41:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InitialInterview](
	[InitialInterviewID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CompletionDate] [datetime] NULL,
	[ReasonForProgram] [nvarchar](max) NULL,
	[ReasonForMMCC] [nvarchar](max) NULL,
	[CollegeLifeAdjustments] [nvarchar](max) NULL,
	[ChoiceOfProgramAdjustments] [nvarchar](max) NULL,
	[PeersAdjustments] [nvarchar](max) NULL,
	[MMCCStaffAdjustments] [nvarchar](max) NULL,
	[FamilyAdjustments] [nvarchar](max) NULL,
	[CounselorNotes] [nvarchar](max) NULL,

PRIMARY KEY CLUSTERED 
(
	[InitialInterviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[InitialInterview]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Student] ([UserID])
ON DELETE CASCADE
GO

