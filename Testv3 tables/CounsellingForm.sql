USE [Testv3]
GO

/****** Object:  Table [dbo].[CounsellingForm]    Script Date: 3/9/2018 9:33:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CounsellingForm](
	[CounsellingFormID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[StudentUserID] [nvarchar](128) NOT NULL,
	[CompletionDate] [datetime] NULL,
	[Case] [nvarchar](max) NULL,
	[Session] [nvarchar](max) NULL,
	[PctionPlan] [nvarchar](max) NULL,
	[Recommendation] [nvarchar](max) NULL,
	[Followup] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[CounsellingFormID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CounsellingForm]  WITH CHECK ADD FOREIGN KEY([StudentUserID])
REFERENCES [dbo].[Student] ([UserID])
GO

ALTER TABLE [dbo].[CounsellingForm]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Counsellor] ([UserID])
ON DELETE CASCADE
GO

