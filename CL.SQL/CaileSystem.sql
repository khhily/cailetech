USE [master]
GO

/****** Object:  Database [CaileSystem]    Script Date: 2018/1/18 14:11:57 ******/
CREATE DATABASE [CaileSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CaileSystem', FILENAME = N'D:\CaileGame\CL.Database\Server\CaileSystem.mdf' , SIZE = 158720KB , MAXSIZE = UNLIMITED, FILEGROWTH = 51200KB )
 LOG ON 
( NAME = N'CaileSystem_log', FILENAME = N'D:\CaileGame\CL.Database\Server\CaileSystem_log.ldf' , SIZE = 1025024KB , MAXSIZE = 2048GB , FILEGROWTH = 102400KB )
GO

ALTER DATABASE [CaileSystem] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CaileSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [CaileSystem] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [CaileSystem] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [CaileSystem] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [CaileSystem] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [CaileSystem] SET ARITHABORT OFF 
GO

ALTER DATABASE [CaileSystem] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [CaileSystem] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [CaileSystem] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [CaileSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [CaileSystem] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [CaileSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [CaileSystem] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [CaileSystem] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [CaileSystem] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [CaileSystem] SET  DISABLE_BROKER 
GO

ALTER DATABASE [CaileSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [CaileSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [CaileSystem] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [CaileSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [CaileSystem] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [CaileSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [CaileSystem] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [CaileSystem] SET RECOVERY FULL 
GO

ALTER DATABASE [CaileSystem] SET  MULTI_USER 
GO

ALTER DATABASE [CaileSystem] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [CaileSystem] SET DB_CHAINING OFF 
GO

ALTER DATABASE [CaileSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [CaileSystem] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [CaileSystem] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [CaileSystem] SET  READ_WRITE 
GO




USE [CaileSystem]
GO
/****** Object:  Table [dbo].[CT_DataLog]    Script Date: 2018/1/18 14:11:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_DataLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MethedDescribe] [varchar](200) NULL,
	[MethedName] [varchar](100) NULL,
	[DataType] [int] NULL CONSTRAINT [DF_CT_DataLog_DataType]  DEFAULT ((1)),
	[OperateType] [int] NULL CONSTRAINT [DF_CT_DataLog_OperateType]  DEFAULT ((3)),
	[OperateContent] [text] NULL,
	[RecordTime] [datetime] NULL CONSTRAINT [DF_CT_DataLog_RecordTime]  DEFAULT (getdate()),
 CONSTRAINT [PK_CT_DataLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_ErrorLog]    Script Date: 2018/1/18 14:11:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_ErrorLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MethedDescribe] [varchar](200) NULL,
	[MethedName] [varchar](100) NULL,
	[OperateParamters] [varchar](max) NULL,
	[Message] [varchar](500) NULL,
	[StackTrace] [text] NULL,
	[RecordTime] [datetime] NULL,
 CONSTRAINT [PK_CT_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_Manager]    Script Date: 2018/1/18 14:11:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_Manager](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](32) NOT NULL,
	[PassWord] [varchar](50) NOT NULL,
	[NickName] [varchar](32) NOT NULL,
	[RoleID] [int] NOT NULL,
	[RoleType] [tinyint] NOT NULL,
	[IsLock] [tinyint] NOT NULL,
	[AddTime] [datetime] NOT NULL,
 CONSTRAINT [PK_CT_Manager] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_ManagerLog]    Script Date: 2018/1/18 14:11:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_ManagerLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[UserName] [varchar](32) NOT NULL,
	[ActionType] [varchar](16) NOT NULL,
	[Remark] [varchar](255) NOT NULL,
	[UserIP] [varchar](30) NULL,
	[AddTime] [datetime] NOT NULL,
 CONSTRAINT [PK_CT_ManagerLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_Navigation]    Script Date: 2018/1/18 14:11:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_Navigation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[LinkUrl] [varchar](255) NULL,
	[SortID] [int] NOT NULL,
	[IsLock] [tinyint] NULL,
	[Remark] [varchar](256) NOT NULL,
	[ActionType] [varchar](255) NOT NULL,
	[IsSys] [tinyint] NOT NULL,
 CONSTRAINT [PK_CT_Navigation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_Rosle]    Script Date: 2018/1/18 14:11:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_Rosle](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](32) NOT NULL,
	[RoleType] [tinyint] NOT NULL,
 CONSTRAINT [PK_CT_Rosle] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_RosleValue]    Script Date: 2018/1/18 14:11:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_RosleValue](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[NavName] [varchar](32) NOT NULL,
	[ActionType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CT_RosleValue] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[udv_Manager]    Script Date: 2018/1/18 14:11:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_Manager]
AS
SELECT   a.id, a.UserName, a.PassWord, a.NickName, a.RoleID, a.RoleType, a.IsLock, a.AddTime, b.RoleName
FROM      dbo.CT_Manager AS a LEFT OUTER JOIN
                dbo.CT_Rosle AS b ON b.RoleID = a.RoleID

GO
SET IDENTITY_INSERT [dbo].[CT_DataLog] ON 

GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572024, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:42.627' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572025, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:42.627' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572026, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:45.220' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572027, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:45.220' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572028, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:53.147' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572029, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:53.147' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572030, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:53.353' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572031, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:53.353' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572032, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:54.837' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572033, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:20:54.837' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572034, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:21:00.917' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572035, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:21:00.917' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572036, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:21:01.170' AS DateTime))
GO
INSERT [dbo].[CT_DataLog] ([ID], [MethedDescribe], [MethedName], [DataType], [OperateType], [OperateContent], [RecordTime]) VALUES (572037, N'��¼������־', N'OnActionExecuting', 1, 1, N'{}', CAST(N'2018-01-17 21:21:01.170' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[CT_DataLog] OFF
GO
SET IDENTITY_INSERT [dbo].[CT_Manager] ON 

GO
INSERT [dbo].[CT_Manager] ([id], [UserName], [PassWord], [NickName], [RoleID], [RoleType], [IsLock], [AddTime]) VALUES (1, N'admin', N'e10adc3949ba59abbe56e057f20f883e', N'��������Ա', 1, 1, 0, CAST(N'2016-11-05 16:59:56.483' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[CT_Manager] OFF
GO
SET IDENTITY_INSERT [dbo].[CT_Navigation] ON 

GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (1, 0, N'sys_controller', N'ϵͳ����', N'', 100, 0, N'ϵͳĬ�ϵ��������ɸ��ĵ���ID', N'Show', 1)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (2, 0, N'sys_content ', N'���ݹ���', N'', 101, 0, N'ϵͳĬ�ϵ��������ɸ��ĵ���ID', N'Show', 1)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (3, 0, N'sys_business', N'ҵ������', N'', 102, 0, N'ϵͳĬ�ϵ��������ɸ��ĵ���ID', N'Show', 1)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (4, 0, N'sys_message', N'��Ϣ����', N'', 103, 0, N'ϵͳĬ�ϵ��������ɸ��ĵ���ID', N'Show', 1)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (5, 0, N'sys_activity', N'�����', N'', 104, 0, N'ϵͳĬ�ϵ��������ɸ��ĵ���ID', N'Show', 1)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (6, 0, N'sys_users', N'�û�����', N'', 105, 0, N'ϵͳĬ�ϵ��������ɸ��ĵ���ID', N'Show', 1)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (8, 0, N'sys_finance', N'��������', N'', 107, 0, N'ϵͳĬ�ϵ��������ɸ��ĵ���ID', N'Show', 1)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (9, 1, N'sys_manager', N'ϵͳ�û�', N'', 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (10, 9, N'manager_list', N'����Ա����', N'manager/managerlist.aspx', 100, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (11, 9, N'manager_role', N'��ɫ����', N'manager/rolelist.aspx', 101, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (12, 9, N'manager_log', N'������־', N'manager/managerlog.aspx', 102, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (13, 1, N'sys_settings', N'ϵͳ����', N'', 100, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (14, 13, N'sys_navigation', N'��������', N'settings/nav_list.aspx', 100, 1, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (15, 13, N'sys_config', N'ϵͳ����', N'settings/systemconfig_edit.aspx', 101, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (16, 3, N'sys_lotteries', N'���ֹ���', N'', 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (17, 3, N'sys_businesfind', N'ҵ���ѯ', N'', 100, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (18, 3, N'sys_businesoperat', N'ҵ�����', N'', 101, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (19, 16, N'lotteries_list', N'�����б�', N'lotteries/lotteries_list.aspx', 99, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (20, 16, N'lotteries_isuses', N'�ںŹ���', N'lotteries/isuses_list.aspx', 102, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (21, 17, N'scheme_list', N'Ͷע��ѯ', N'lotteries/scheme_list.aspx', 99, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (22, 17, N'chase_List', N'׷�Ų�ѯ', N'lotteries/chase_List.aspx', 100, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (23, 17, N'winfind_list', N'�н���ѯ', N'lotteries/winfind_list.aspx', 101, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (24, 18, N'resultwinsetup', N'�ֶ�����', N'lotteries/resultwinsetup.aspx', 99, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (25, 18, N'inputopenlottery', N'¼�뽱����Ϣ', N'lotteries/openlottery_edit.aspx', 100, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (26, 6, N'users_manager', N'�û�����', N'', 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (27, 26, N'users_list', N'�û��б�', N'users/user_list.aspx', 99, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (28, 26, N'user_recharge', N'�û���ֵ', N'users/user_recharge.aspx', 101, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (29, 26, N'arena_list', N'��̨�������', N'users/arena_list.aspx', 102, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (33, 2, N'information_manager', N'վ����Ѷ', N'', 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (34, 33, N'article_list', N'���¹���', N'news/article_list.aspx', 99, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (35, 33, N'news_list', N'���Ź���', N'news/news_list.aspx', 100, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (36, 33, N'newstypes_list', N'������Ŀ����', N'news/newstypes_list.aspx', 101, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (37, 8, N'pay_withdraw', N'���ֹ���', N'', 77, 0, N'ϵͳĬ�ϵ��������ɸ��ĵ���ID', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (38, 37, N'audit', N'�������', N'withdraw/audit.aspx', 78, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (39, 37, N'operation', N'���ֲ���', N'withdraw/operation.aspx', 78, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (42, 4, N'message_management', N'��Ϣ����', N'', 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (43, 42, N'template_list', N'ģ������', N'message/template_list.aspx', 99, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (44, 42, N'notifyinfo_list', N'��Ϣ�б�', N'message/notifyinfo_list.aspx', 100, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (45, 26, N'useracc_list', N'�û�Ԥע��', N'users/useracc_list.aspx', 103, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (46, 37, N'pay_list', N'��ֵ��¼����', N'withdraw/pay_list.aspx', 80, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (47, 37, N'refund_list', N'�˿��¼����', N'withdraw/refund_list.aspx', 81, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (48, 3, N'sys_report', N'����', N'', 102, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (49, 48, N'orderdetail_list', N'������ϸ', N'report/orderdetailreport.aspx', 99, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (50, 48, N'rechargedetail_list', N'��ֵ��ϸ', N'report/rechargedetailreport.aspx', 99, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (51, 48, N'tradedetail_list', N'������ϸ', N'report/tradedetailreport.aspx', 99, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (52, 48, N'winningdetail_list', N'�н���ϸ', N'report/winningdetailreport.aspx', 99, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (53, 48, N'withdrawdetail_list', N'������ϸ', N'report/withdrawdetailreport.aspx', 99, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (54, 18, N'manualopenlot', N'�ֶ�����', N'lotteries/manualopenlottery.aspx', 99, 0, N'', N'Show,View,Add', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (57, 8, N'report_checking', N'���񱨱�', N'', 77, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (58, 57, N'checking_list', N'ͳ�Ʊ���', N'report/checkingreport.aspx', 81, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (63, 48, N'useraccountdetail_list', N'�û����ܱ���', N'report/useraccountreport.aspx', 99, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (66, 18, N'salepoint_list', N'��λ��ѯ', N'lotteries/salepoint_list.aspx', 101, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (68, 48, N'ETicketsReport', N'��Ʊ��ϸ', N'report/ETicketsReport.aspx', 99, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (69, 3, N'sys_redpacket', N'��ȯ����', NULL, 103, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (70, 69, N'redpacket_cdkeylist', N'�һ����б�', N'coupons/cdkeylist.aspx', 100, 0, N'', N'Show,View,Add', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (71, 69, N'redpacket_list', N'��ȯ�б�', N'coupons/couponslist.aspx', 101, 0, N'', N'Show,View,Add', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (72, 8, N'report_redpacket', N'��ȯ����', N'', 77, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (73, 72, N'redpacket_list', N'��ȯ����', N'coupons/report/reportcoupons.aspx', 100, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (75, 72, N'redpacket_cdkey', N'�һ��뱨��', N'coupons/report/reportcdkey.aspx', 99, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (76, 3, N'sys_push', N'���͹���', NULL, 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (77, 76, N'sys_jiguangpush', N'��������', N'push/JiGuangPush.aspx', 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (79, 5, N'activity_manager', N'�����', NULL, 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (80, 79, N'activity_list', N'��б�', N'activity/activitylist.aspx', 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (81, 79, N'sys_auditinglist', N'����', N'activity/activityauditinglist.aspx', 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (82, 57, N'staticdata_list', N'��̬����ͳ��', N'report/staticdataReport.aspx', 81, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (84, 57, N'buylotre_list', N'Ͷעͳ��', N'report/finance/buylotreport.aspx', 81, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (85, 57, N'outticket_list', N'��Ʊͳ��', N'report/finance/outticketreport.aspx', 81, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (86, 57, N'paydetail_list', N'��ֵͳ��', N'report/finance/paydetailreport.aspx', 81, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (87, 57, N'winaward_list', N'�н�ͳ��', N'report/finance/winawardreport.aspx', 81, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (88, 57, N'withdraw_list', N'����ͳ��', N'report/finance/withdrawreport.aspx', 81, 0, N'', N'Show,View', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (89, 42, N'marquee_push', N'�����', N'push/marquee.aspx', 81, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (90, 33, N'auditing_list', N'������Ѷ���', N'news/auditinglist.aspx', 99, 0, N'', N'Show', 0)
GO
INSERT [dbo].[CT_Navigation] ([id], [ParentID], [Name], [Title], [LinkUrl], [SortID], [IsLock], [Remark], [ActionType], [IsSys]) VALUES (94, 13, N'sys_programa', N'��Ŀ����', N'settings/programa_list.aspx', 102, 0, N'', N'Show,View,Add,Edit,Delete', 0)
GO
SET IDENTITY_INSERT [dbo].[CT_Navigation] OFF
GO
SET IDENTITY_INSERT [dbo].[CT_Rosle] ON 

GO
INSERT [dbo].[CT_Rosle] ([RoleID], [RoleName], [RoleType]) VALUES (1, N'����������', 1)
GO
SET IDENTITY_INSERT [dbo].[CT_Rosle] OFF
GO
ALTER TABLE [dbo].[CT_ErrorLog] ADD  CONSTRAINT [DF_CT_ErrorLog_RecordTime]  DEFAULT (getdate()) FOR [RecordTime]
GO
ALTER TABLE [dbo].[CT_ManagerLog] ADD  DEFAULT ((0)) FOR [UserID]
GO
ALTER TABLE [dbo].[CT_ManagerLog] ADD  DEFAULT ('') FOR [UserName]
GO
ALTER TABLE [dbo].[CT_ManagerLog] ADD  DEFAULT ('') FOR [ActionType]
GO
ALTER TABLE [dbo].[CT_ManagerLog] ADD  DEFAULT ('') FOR [Remark]
GO
ALTER TABLE [dbo].[CT_ManagerLog] ADD  DEFAULT (getdate()) FOR [AddTime]
GO
/****** Object:  StoredProcedure [dbo].[udp_DelManagerLog]    Script Date: 2018/1/18 14:11:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		joan
-- Create date: 2017-1-10
-- Description:	ɾ����־����
-- =============================================
CREATE PROCEDURE [dbo].[udp_DelManagerLog]
	-- Add the parameters for the stored procedure here
	@dayCount INT,
	@ReturnDelCount INT OUTPUT
	--WITH ENCRYPTION
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT @ReturnDelCount=COUNT(1) FROM CT_ManagerLog WHERE DATEDIFF(DAY, AddTime, GETDATE()) > @dayCount
    -- Insert statements for procedure here
	DELETE FROM CT_ManagerLog WHERE DATEDIFF(DAY, AddTime, GETDATE()) > @dayCount

END



GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ݲ�����־��ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_DataLog', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_DataLog', @level2type=N'COLUMN',@level2name=N'MethedDescribe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_DataLog', @level2type=N'COLUMN',@level2name=N'MethedName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������(1.Table;2.View;3.StoredProcedure)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_DataLog', @level2type=N'COLUMN',@level2name=N'DataType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������(1.Insert 2.Delete 3.Select 4.Update)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_DataLog', @level2type=N'COLUMN',@level2name=N'OperateType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_DataLog', @level2type=N'COLUMN',@level2name=N'OperateContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��¼ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_DataLog', @level2type=N'COLUMN',@level2name=N'RecordTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������־��ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ErrorLog', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ErrorLog', @level2type=N'COLUMN',@level2name=N'MethedDescribe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ErrorLog', @level2type=N'COLUMN',@level2name=N'MethedName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ErrorLog', @level2type=N'COLUMN',@level2name=N'OperateParamters'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ErrorLog', @level2type=N'COLUMN',@level2name=N'Message'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ErrorLog', @level2type=N'COLUMN',@level2name=N'StackTrace'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��¼ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ErrorLog', @level2type=N'COLUMN',@level2name=N'RecordTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ManagerLog', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ManagerLog', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ManagerLog', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ManagerLog', @level2type=N'COLUMN',@level2name=N'ActionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ManagerLog', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ManagerLog', @level2type=N'COLUMN',@level2name=N'UserIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ManagerLog', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������־��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ManagerLog'
GO
