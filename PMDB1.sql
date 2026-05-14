USE [master]
GO
/****** Объект:  Database [PM]    Дата создания скрипта: 14.05.2026 7:59:51 ******/ 
CREATE DATABASE [PM]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PM', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\PM.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PM_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\PM_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [PM] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PM].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PM] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PM] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PM] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PM] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PM] SET ARITHABORT OFF 
GO
ALTER DATABASE [PM] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PM] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PM] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PM] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PM] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PM] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PM] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PM] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PM] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PM] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PM] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PM] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PM] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PM] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PM] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PM] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PM] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PM] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PM] SET  MULTI_USER 
GO
ALTER DATABASE [PM] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PM] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PM] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PM] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PM] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PM] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [PM] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [PM] SET QUERY_STORE = ON
GO
ALTER DATABASE [PM] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [PM]
GO
/****** Объект:  Table [dbo].[Departments]    Дата создания скрипта: 14.05.2026 7:59:51 ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Объект:  Table [dbo].[Employees]    Дата создания скрипта: 14.05.2026 7:59:51 ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[ID] [uniqueidentifier] NOT NULL,
	[LastName] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[Patronymic] [nvarchar](max) NULL,
	[EmploymentTypesID] [int] NULL,
	[DepartmentID] [int] NULL,
	[PositionID] [int] NULL,
	[HireDate] [date] NULL,
	[MedicalExamDate] [date] NULL,
	[WorkExperience] [int] NULL,
	[PlannedHours] [int] NULL,
	[ActualHours] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Объект:  Table [dbo].[EmploymentTypes]    Дата создания скрипта: 14.05.2026 7:59:51 ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmploymentTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Объект:  Table [dbo].[Positions]    Дата создания скрипта: 14.05.2026 7:59:51 ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Positions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Объект:  Table [dbo].[RateNorms]    Дата создания скрипта: 14.05.2026 7:59:51 ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RateNorms](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PositionID] [int] NULL,
	[HoursPerRate] [int] NULL,
	[Year] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Объект:  Table [dbo].[Users]    Дата создания скрипта: 14.05.2026 7:59:51 ******/ 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [uniqueidentifier] NULL,
	[Email] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[Role] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Departments] ON 
GO
INSERT [dbo].[Departments] ([ID], [Name]) VALUES (1, N'Отделение информационных технологий')
GO
INSERT [dbo].[Departments] ([ID], [Name]) VALUES (2, N'Отделение экономики и управления')
GO
INSERT [dbo].[Departments] ([ID], [Name]) VALUES (3, N'Отделение машиностроения')
GO
INSERT [dbo].[Departments] ([ID], [Name]) VALUES (4, N'Отделение электроэнергетики')
GO
INSERT [dbo].[Departments] ([ID], [Name]) VALUES (5, N'Отдел кадров')
GO
INSERT [dbo].[Departments] ([ID], [Name]) VALUES (6, N'Бухгалтерия')
GO
SET IDENTITY_INSERT [dbo].[Departments] OFF
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'dd54744f-d5ec-4bb0-b2d6-057774a9cec3', N'Петров', N'Дмитрий', N'Алексеевич', 1, 3, 3, CAST(N'2026-05-11' AS Date), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'7a0661db-d2ee-4973-8a10-08eb4223b7e6', N'Иванова', N'Анна', N'Сергеевна', 1, 1, 1, CAST(N'2026-05-11' AS Date), NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111101', N'Кузнецов', N'Алексей', N'Викторович', 1, 1, 4, CAST(N'2010-09-01' AS Date), CAST(N'2026-03-15' AS Date), 16, 360, 200)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111102', N'Смирнова', N'Елена', N'Игоревна', 1, 2, 4, CAST(N'2012-09-01' AS Date), CAST(N'2026-05-20' AS Date), 14, 360, 180)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111103', N'Петров', N'Сергей', N'Николаевич', 1, 3, 4, CAST(N'2015-09-01' AS Date), CAST(N'2026-04-10' AS Date), 11, 360, 190)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111104', N'Иванова', N'Мария', N'Александровна', 1, 1, 2, CAST(N'2015-09-01' AS Date), CAST(N'2026-02-10' AS Date), 11, 720, 450)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111105', N'Соколов', N'Дмитрий', N'Сергеевич', 1, 1, 1, CAST(N'2018-09-01' AS Date), CAST(N'2026-06-20' AS Date), 8, 540, 320)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111106', N'Козлова', N'Анна', N'Павловна', 2, 1, 1, CAST(N'2020-09-01' AS Date), CAST(N'2026-01-25' AS Date), 6, 360, 200)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111107', N'Новиков', N'Артём', N'Олегович', 1, 1, 3, CAST(N'2016-09-01' AS Date), CAST(N'2026-07-12' AS Date), 10, 900, 550)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111108', N'Морозова', N'Татьяна', N'Викторовна', 1, 2, 2, CAST(N'2014-09-01' AS Date), CAST(N'2026-03-05' AS Date), 12, 720, 480)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111109', N'Волков', N'Игорь', N'Анатольевич', 1, 2, 1, CAST(N'2019-09-01' AS Date), CAST(N'2026-08-30' AS Date), 7, 1080, 650)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111110', N'Лебедева', N'Ольга', N'Сергеевна', 3, 2, 1, CAST(N'2022-09-01' AS Date), CAST(N'2026-11-15' AS Date), 4, 180, 100)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111111', N'Егоров', N'Павел', N'Дмитриевич', 1, 3, 2, CAST(N'2011-09-01' AS Date), CAST(N'2026-05-18' AS Date), 15, 720, 420)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111112', N'Фёдорова', N'Наталья', N'Ивановна', 1, 3, 1, CAST(N'2017-09-01' AS Date), CAST(N'2026-02-28' AS Date), 9, 720, 500)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111113', N'Васильев', N'Роман', N'Алексеевич', 1, 3, 3, CAST(N'2013-09-01' AS Date), CAST(N'2026-09-01' AS Date), 13, 900, 600)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111114', N'Григорьева', N'Светлана', N'Юрьевна', 1, 4, 1, CAST(N'2021-09-01' AS Date), CAST(N'2026-04-22' AS Date), 5, 1440, 900)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111115', N'Зайцев', N'Андрей', N'Владимирович', 2, 4, 1, CAST(N'2023-09-01' AS Date), CAST(N'2026-12-10' AS Date), 3, 360, 220)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111116', N'Белова', N'Ирина', N'Станиславовна', 1, 5, 6, CAST(N'2009-05-15' AS Date), CAST(N'2026-01-20' AS Date), 17, NULL, NULL)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111117', N'Тихонова', N'Вера', N'Михайловна', 1, 5, 6, CAST(N'2016-03-10' AS Date), CAST(N'2026-10-05' AS Date), 10, NULL, NULL)
GO
INSERT [dbo].[Employees] ([ID], [LastName], [FirstName], [Patronymic], [EmploymentTypesID], [DepartmentID], [PositionID], [HireDate], [MedicalExamDate], [WorkExperience], [PlannedHours], [ActualHours]) VALUES (N'11111111-1111-1111-1111-111111111118', N'Медведева', N'Людмила', N'Петровна', 1, 6, 7, CAST(N'2008-11-20' AS Date), CAST(N'2026-07-30' AS Date), 18, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[EmploymentTypes] ON 
GO
INSERT [dbo].[EmploymentTypes] ([ID], [Name]) VALUES (1, N'Штатный')
GO
INSERT [dbo].[EmploymentTypes] ([ID], [Name]) VALUES (2, N'Внутренний совместитель')
GO
INSERT [dbo].[EmploymentTypes] ([ID], [Name]) VALUES (3, N'Внешний совместитель')
GO
INSERT [dbo].[EmploymentTypes] ([ID], [Name]) VALUES (4, N'Почасовик')
GO
SET IDENTITY_INSERT [dbo].[EmploymentTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Positions] ON 
GO
INSERT [dbo].[Positions] ([ID], [Name]) VALUES (1, N'Преподаватель')
GO
INSERT [dbo].[Positions] ([ID], [Name]) VALUES (2, N'Старший преподаватель')
GO
INSERT [dbo].[Positions] ([ID], [Name]) VALUES (3, N'Мастер производственного обучения')
GO
INSERT [dbo].[Positions] ([ID], [Name]) VALUES (4, N'Заведующий отделением')
GO
INSERT [dbo].[Positions] ([ID], [Name]) VALUES (5, N'Методист')
GO
INSERT [dbo].[Positions] ([ID], [Name]) VALUES (6, N'Специалист отдела кадров')
GO
INSERT [dbo].[Positions] ([ID], [Name]) VALUES (7, N'Бухгалтер')
GO
INSERT [dbo].[Positions] ([ID], [Name]) VALUES (8, N'Лаборант')
GO
SET IDENTITY_INSERT [dbo].[Positions] OFF
GO
SET IDENTITY_INSERT [dbo].[RateNorms] ON 
GO
INSERT [dbo].[RateNorms] ([ID], [PositionID], [HoursPerRate], [Year]) VALUES (1, 1, 720, N'2025/2026')
GO
INSERT [dbo].[RateNorms] ([ID], [PositionID], [HoursPerRate], [Year]) VALUES (2, 2, 720, N'2025/2026')
GO
INSERT [dbo].[RateNorms] ([ID], [PositionID], [HoursPerRate], [Year]) VALUES (3, 3, 900, N'2025/2026')
GO
INSERT [dbo].[RateNorms] ([ID], [PositionID], [HoursPerRate], [Year]) VALUES (4, 4, 720, N'2025/2026')
GO
INSERT [dbo].[RateNorms] ([ID], [PositionID], [HoursPerRate], [Year]) VALUES (5, 5, 720, N'2025/2026')
GO
SET IDENTITY_INSERT [dbo].[RateNorms] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([ID], [EmployeeID], [Email], [PasswordHash], [Role]) VALUES (1, N'11111111-1111-1111-1111-111111111101', N'admin', N'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', N'Manager')
GO
INSERT [dbo].[Users] ([ID], [EmployeeID], [Email], [PasswordHash], [Role]) VALUES (2, N'11111111-1111-1111-1111-111111111102', N'smirnova@tpek.ru', N'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', N'Manager')
GO
INSERT [dbo].[Users] ([ID], [EmployeeID], [Email], [PasswordHash], [Role]) VALUES (3, N'11111111-1111-1111-1111-111111111103', N'petrov@tpek.ru', N'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', N'Manager')
GO
INSERT [dbo].[Users] ([ID], [EmployeeID], [Email], [PasswordHash], [Role]) VALUES (4, N'11111111-1111-1111-1111-111111111116', N'belova@tpek.ru', N'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', N'Admin')
GO
INSERT [dbo].[Users] ([ID], [EmployeeID], [Email], [PasswordHash], [Role]) VALUES (5, N'11111111-1111-1111-1111-111111111117', N'tihonova@tpek.ru', N'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', N'Admin')
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Объект:  Index [UQ__Users__7AD04FF0240AABEA]    Дата создания скрипта: 14.05.2026 7:59:51 ******/ 
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Departments] ([ID])
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD FOREIGN KEY([EmploymentTypesID])
REFERENCES [dbo].[EmploymentTypes] ([ID])
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD FOREIGN KEY([PositionID])
REFERENCES [dbo].[Positions] ([ID])
GO
ALTER TABLE [dbo].[RateNorms]  WITH CHECK ADD FOREIGN KEY([PositionID])
REFERENCES [dbo].[Positions] ([ID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employees] ([ID])
GO
USE [master]
GO
ALTER DATABASE [PM] SET  READ_WRITE 
GO
