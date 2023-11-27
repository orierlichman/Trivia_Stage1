CREATE DATABASE [TriviaGames]
GO

USE [TriviaGames]
GO

CREATE TABLE [Ranks](
	[RankId] INT IDENTITY (1,1) NOT NULL,
	[RankStatus] NCHAR (30) NOT NULL,
	CONSTRAINT [PK_Ranks] PRIMARY KEY CLUSTERED ([RankId] ASC) 
);
GO

CREATE TABLE [Subjects](
	[SubjectId] INT IDENTITY (1,1) NOT NULL,
	[SubjectName] NCHAR (30) NOT NULL,
	CONSTRAINT [PK_Subject] PRIMARY KEY CLUSTERED ([SubjectId] ASC) 
);
GO

CREATE TABLE [QuestionsStatus](
	[QuestionStatusId] INT IDENTITY (1,1) NOT NULL,
	[StatusName] NCHAR (30) NOT NULL,
	CONSTRAINT [PK_QuestionsStatus] PRIMARY KEY CLUSTERED ([QuestionStatusId] ASC) 
);
GO

CREATE TABLE [Players](
	[PlayerId] INT IDENTITY (1,1) NOT NULL,
	[Name] NCHAR (20) NOT NULL,
	[Email] NCHAR (40) NOT NULL,
	[Password] NCHAR (20) NOT NULL,
	[Score] INT NOT NULL,
	[RankId] INT NOT NULL,
	[NumOfQuestions] INT NOT NULL,
	CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED ([PlayerId] ASC),
	CONSTRAINT [FK_Player] FOREIGN KEY ([RankId]) REFERENCES [Ranks] ([RankId])
);
GO

CREATE TABLE [Questions](
	[QuestionId] INT IDENTITY (1,1) NOT NULL,
	[Question] NCHAR (300) NOT NULL,
	[SubjectId] INT NOT NULL,
	[StatusId] INT NOT NULL,
	[WriterId] INT NOT NULL,
	[CorrectAnswer] NCHAR NOT NULL,
	[WrongAnswer1] NCHAR NOT NULL,
	[WrongAnswer2] NCHAR NOT NULL,
	[WrongAnswer3] NCHAR NOT NULL,
	CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED ([QuestionId] ASC),
	CONSTRAINT [FK_QuestionSubjcet] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([SubjectId]),
	CONSTRAINT [FK_QuestionStatus] FOREIGN KEY ([StatusId]) REFERENCES [QuestionsStatus] ([QuestionStatusId]),
	CONSTRAINT [FK_QuestionWriter] FOREIGN KEY ([WriterId]) REFERENCES [Players] ([PlayerId])
);
GO

SELECT * FROM Players


