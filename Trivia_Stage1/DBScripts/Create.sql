drop database [TriviaGames]
go

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
	[CorrectAnswer] NCHAR (150) NOT NULL,
	[WrongAnswer1] NCHAR (150) NOT NULL,
	[WrongAnswer2] NCHAR (150) NOT NULL,
	[WrongAnswer3] NCHAR (150) NOT NULL,
	CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED ([QuestionId] ASC),
	CONSTRAINT [FK_QuestionSubjcet] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects] ([SubjectId]),
	CONSTRAINT [FK_QuestionStatus] FOREIGN KEY ([StatusId]) REFERENCES [QuestionsStatus] ([QuestionStatusId]),
	CONSTRAINT [FK_QuestionWriter] FOREIGN KEY ([WriterId]) REFERENCES [Players] ([PlayerId])
);
GO




INSERT INTO [Ranks] ([RankStatus]) VALUES ('Manager');
INSERT INTO [Ranks] ([RankStatus]) VALUES ('Maestro');
INSERT INTO [Ranks] ([RankStatus]) VALUES ('Rookie');

INSERT INTO [Subjects] ([SubjectName]) VALUES ('Sport');
INSERT INTO [Subjects] ([SubjectName]) VALUES ('Politics');
INSERT INTO [Subjects] ([SubjectName]) VALUES ('History');
INSERT INTO [Subjects] ([SubjectName]) VALUES ('Science');
INSERT INTO [Subjects] ([SubjectName]) VALUES ('Ramon HighSchool');

INSERT INTO [QuestionsStatus] ([StatusName]) VALUES ('Pending');
INSERT INTO [QuestionsStatus] ([StatusName]) VALUES ('Accepted');
INSERT INTO [QuestionsStatus] ([StatusName]) VALUES ('Not Accepted');

INSERT INTO [Players] ([Name], [Email], [Password], [Score], [RankId], [NumOfQuestions]) VALUES ('Ofer', 'Ofer123@Gmail.com', 'Ofer123', 0, 1, 5);

INSERT INTO [Questions] ([Question], [SubjectId], [StatusId], [WriterId], [CorrectAnswer], [WrongAnswer1], [WrongAnswer2], [WrongAnswer3])
VALUES ('What shirt number does Cristiano Ronaldo wears ?', 1, 2, 1, '7', '4', '11', '20');
INSERT INTO [Questions] ([Question], [SubjectId], [StatusId], [WriterId], [CorrectAnswer], [WrongAnswer1], [WrongAnswer2], [WrongAnswer3])
VALUES ('Who is the Prime Minister of Israel ?', 2, 2, 1, 'Binyamin Nethanyahu', 'Ofer Zadikario', 'Naftali Benet', 'Hanna Dalmadigo');
INSERT INTO [Questions] ([Question], [SubjectId], [StatusId], [WriterId], [CorrectAnswer], [WrongAnswer1], [WrongAnswer2], [WrongAnswer3])
VALUES ('When did the Holocaust ended ?', 3, 2, 1, '1945', '1941', '1933', '2006');
INSERT INTO [Questions] ([Question], [SubjectId], [StatusId], [WriterId], [CorrectAnswer], [WrongAnswer1], [WrongAnswer2], [WrongAnswer3])
VALUES ('What is the boiling temprature of H2O ?', 4, 2, 1, '100', '95', '105', '110');
INSERT INTO [Questions] ([Question], [SubjectId], [StatusId], [WriterId], [CorrectAnswer], [WrongAnswer1], [WrongAnswer2], [WrongAnswer3])
VALUES ('Who is the best teacher in school ?', 5, 2, 1, 'Ofer Zadikario', 'Anat Bracha', 'Tali Shulk', 'Yoav Ben Aroya');



--scaffold-DbContext "Server = LAB2-10\SQLEXPRESS;Initial Catalog=TriviaGames;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Encrypt=False;Trust Server Certificate=False;Command Timeout=0;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context TriviaGamesDbContext -DataAnnotations -force
