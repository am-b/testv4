USE [Testv3]
GO

/****** Object:  Table [dbo].[Announcement]    Script Date: 12/23/2017 12:06:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Announcement](
	[AnnouncementID] [int] IDENTITY(1,1) NOT NULL,
	[AnnouncementDate] [datetime] NULL,
	[AnnouncementTitle] [nvarchar](256) NULL,
	[AnnouncementBody] [text] NULL,
	[PostedBy] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[AnnouncementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

