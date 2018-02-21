USE [Testv3]
GO

/****** Object:  Table [dbo].[CounsellingContract]    Script Date: 2/15/2018 8:54:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CounsellingContract](
	[CounsellingContractID] [int] IDENTITY(1,1) NOT NULL,
	[StudentUserID] [nvarchar](128) NOT NULL,
	[CompletionDate] [datetime] NULL,

PRIMARY KEY CLUSTERED 
(
	[CounsellingContractID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[CounsellingContract]  WITH CHECK ADD FOREIGN KEY([StudentUserID])
REFERENCES [dbo].[Student] ([UserID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CounsellingContract]
  ADD StudentAgrees [bit] NULL

