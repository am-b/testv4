USE [Testv3]
GO

/****** Object:  Table [dbo].[Counsellor]    Script Date: 3/9/2018 9:34:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Counsellor](
	[UserID] [nvarchar](128) NOT NULL,
	[CounsellorID] [nvarchar](50) NULL,
	[CounsellorLastName] [nvarchar](50) NULL,
	[CounsellorFirstName] [nvarchar](50) NULL,
	[CounsellorMiddleName] [nvarchar](50) NULL,
	[CounsellorEmail] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Counsellor]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

