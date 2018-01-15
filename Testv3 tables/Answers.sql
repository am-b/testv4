USE [Testv3]
GO

/****** Object:  Table [dbo].[Answers]    Script Date: 1/8/2018 12:18:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Answers](
	[AnswerID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[Answer] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AnswerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Answers]  WITH CHECK ADD FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Questions] ([QuestionID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Answers]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Student] ([UserID])
ON DELETE CASCADE
GO
