USE [Testv3]
GO

/****** Object:  Table [dbo].[ExitInterview]    Script Date: 3/9/2018 9:34:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ExitInterview](
	[ExitInterviewID] [int] IDENTITY(1,1) NOT NULL,
	[StudentUserID] [nvarchar](128) NOT NULL,
	[CompletionDate] [datetime] NULL,
	[MMCCLikes] [nvarchar](max) NULL,
	[MMCCDislikes] [nvarchar](max) NULL,
	[MMCCMoments] [nvarchar](max) NULL,
	[Professors] [nvarchar](max) NULL,
	[Staff] [nvarchar](max) NULL,
	[Future] [nvarchar](max) NULL,
	[Others] [nvarchar](max) NULL,
	[GuidanceNotes] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ExitInterviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExitInterview]  WITH CHECK ADD FOREIGN KEY([StudentUserID])
REFERENCES [dbo].[Student] ([UserID])
ON DELETE CASCADE
GO

