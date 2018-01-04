USE [Testv3]
GO

/****** Object:  Table [dbo].[IndividualInventoryRecord]    Script Date: 1/4/2018 6:58:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IndividualInventoryRecord](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[FathersName] [nvarchar](max) NULL,
	[FathersAddress] [nvarchar](250) NULL,
	[FathersAge] [nvarchar](2) NULL,
	[FathersEducationalAttainment] [nvarchar](50) NULL,
	[FathersOccupation] [nvarchar](50) NULL,
	[FathersEmployerAddress] [nvarchar](250) NULL,
	[MothersName] [nvarchar](max) NULL,
	[MothersAddress] [nvarchar](250) NULL,
	[MothersAge] [nvarchar](2) NULL,
	[MothersEducationalAttainment] [nvarchar](50) NULL,
	[MothersOccupation] [nvarchar](50) NULL,
	[MothersEmployerAddress] [nvarchar](250) NULL,
	[FamilyDwelling] [nvarchar](250) NULL,
	[EmergencyContactName] [nvarchar](250) NULL,
	[EmergencyContactNumber] [nvarchar](250) NULL,
	[ParentsStatus] [nvarchar](250) NULL,
	[EconomicStatus] [nvarchar](250) NULL,
	[NoOfSiblings] [nvarchar](3) NULL,
	[PresentlyLivingWith] [nvarchar](250) NULL,
	[PresentlyStayingAt] [nvarchar](250) NULL,
	[ElementarySchool] [nvarchar](250) NULL,
	[ElementaryAddress] [nvarchar](250) NULL,
	[YearsAttendedElem] [nvarchar](250) NULL,
	[HighSchool] [nvarchar](250) NULL,
	[HighSchoolAddress] [nvarchar](250) NULL,
	[YearsAttendedHS] [nvarchar](250) NULL,
	[CollegeSchool] [nvarchar](250) NULL,
	[CollegeAddress] [nvarchar](250) NULL,
	[YearsAttendedCollege] [nvarchar](250) NULL,
	[Honors] [nvarchar](250) NULL,
	[FaveSubject] [nvarchar](250) NULL,
	[LeastSubject] [nvarchar](250) NULL,
	[HowStudieIssFinanced] [nvarchar](250) NULL,
	[IsCoursePersonalChoice] [nvarchar](250) NULL,
	[CourseNotPersonalChoice] [nvarchar](250) NULL,
	[CourseChoiceInfluence] [nvarchar](max) NULL,
	[CoursePersonalChoice] [nvarchar](250) NULL,
	[WhyMMCC] [nvarchar](max) NULL,
	[ReferredToMMCCBy] [nvarchar](250) NULL,
	[Position] [nvarchar](250) NULL,
	[Salary] [nvarchar](250) NULL,
	[Employer] [nvarchar](250) NULL,
	[EmployerAddress] [nvarchar](250) NULL,
	[EmploymentStatus] [nvarchar](250) NULL,
	[MentalAbilityTestDate] [nvarchar](250) NULL,
	[MentalAbilityTestScore] [nvarchar](250) NULL,
	[MentalAbilityTestPercentile] [nvarchar](250) NULL,
	[PersonalityTestDate] [nvarchar](250) NULL,
	[PersonalityTestScore] [nvarchar](250) NULL,
	[PersonalityTestPercentile] [nvarchar](250) NULL,
	[VocationalTestDate] [nvarchar](250) NULL,
	[VocationalTestScore] [nvarchar](250) NULL,
	[VocationalTestPercentile] [nvarchar](250) NULL,
	[Disabilities] [nvarchar](250) NULL,
	[ChronicIllness] [nvarchar](250) NULL,
	[PreviousAccidents] [nvarchar](250) NULL,
	[PreviousSurgery] [nvarchar](250) NULL,
	[MaintenanceMedicines] [nvarchar](250) NULL,
	[Immunization] [nvarchar](250) NULL,
	[HaveTalkedWithACounselor] [nvarchar](250) NULL,
	[HaveTalkedWithACounselorWhen] [nvarchar](250) NULL,
	[HaveTalkedWithACounselorWhy] [nvarchar](max) NULL,
	[HaveTalkedWithAPsychiatrist] [nvarchar](250) NULL,
	[HaveTalkedWithAPsychiatristWhen] [nvarchar](250) NULL,
	[HaveTalkedWithAPsychiatristWhy] [nvarchar](max) NULL,
	[HaveTalkedWithAPsychologist] [nvarchar](250) NULL,
	[HaveTalkedWithAPsychologistWhen] [nvarchar](250) NULL,
	[HaveTalkedWithAPsychologistWhy] [nvarchar](max) NULL,
	[AboutYourself] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

