USE [Testv3]
GO

/****** Object:  Table [dbo].[AnecdotalRecord]    Script Date: 3/9/2018 9:29:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AnecdotalRecord](
	[AnecdotalRecordID] [int] IDENTITY(1,1) NOT NULL,
	[StudentUserID] [nvarchar](128) NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CompletionDate] [datetime] NULL,
	[DateTimeObserved] [datetime] NULL,
	[Place] [nvarchar](128) NULL,
	[Observer] [nvarchar](128) NULL,
	[BehaviorObserved] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[Summary] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[AnecdotalRecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AnecdotalRecord]  WITH CHECK ADD FOREIGN KEY([StudentUserID])
REFERENCES [dbo].[Student] ([UserID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AnecdotalRecord]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Counsellor] ([UserID])
GO

