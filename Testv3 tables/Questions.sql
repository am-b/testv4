USE [Testv3]
GO

/****** Object:  Table [dbo].[Questions]    Script Date: 1/15/2018 3:44:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Questions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[Question] [nvarchar](max) NOT NULL,
	[QuestionTag] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I feel good about the way I look.','Physical'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I can get along with most people.','Social'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('Im not much help to others.','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('My friends seek my advice when theyre in trouble','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I enjoy being with members of the opposite sex.','Social'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('Sometimes I wonder if my family really cares about me','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I have many good qualities','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('My future looks good','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I take pride in my apperance','Physical'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I often feel awkward','Social'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I avoid parties and other social functions','Social'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('Sometimes I cant decide what I want to do','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I enjoy making new friends','Social'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I can be an irritating person sometimes','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I usually fail to achieve my goals','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I would be happier if I were more attractive','Physical'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I usually quit too soon','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I dont learn as fast as my friends','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('Im happy with my sex life','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('My family is a source of help and support for me','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I sometimes behave badly toward other people','Emotional'); 
INSERT INTO [dbo].[Questions] ([Question] ,[QuestionTag]) VALUES ('I am able to manage my life and myself all the time','Emotional'); 