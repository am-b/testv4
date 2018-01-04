USE [Testv3]
GO

/****** Object:  Table [dbo].[PsychTest]    Script Date: 1/4/2018 6:58:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PsychTest](
	[QuestionID] [int] NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[Answer] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PsychTest]  WITH CHECK ADD FOREIGN KEY([QuestionID])
REFERENCES [dbo].[PsychTestQuestions] ([QuestionID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PsychTest]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Student] ([UserID])
ON DELETE CASCADE
GO

