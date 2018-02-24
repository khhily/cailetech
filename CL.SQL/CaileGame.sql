USE [master]
GO

/****** Object:  Database [CaileGame]    Script Date: 2018/1/18 14:14:53 ******/
CREATE DATABASE [CaileGame]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CaileGame', FILENAME = N'D:\DataBase\CaileGame.mdf' , SIZE = 606208KB , MAXSIZE = UNLIMITED, FILEGROWTH = 102400KB )
 LOG ON 
( NAME = N'CaileGame_log', FILENAME = N'D:\DataBase\CaileGame.ldf' , SIZE = 2919424KB , MAXSIZE = 2048GB , FILEGROWTH = 204800KB )
GO

ALTER DATABASE [CaileGame] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CaileGame].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [CaileGame] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [CaileGame] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [CaileGame] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [CaileGame] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [CaileGame] SET ARITHABORT OFF 
GO

ALTER DATABASE [CaileGame] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [CaileGame] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [CaileGame] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [CaileGame] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [CaileGame] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [CaileGame] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [CaileGame] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [CaileGame] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [CaileGame] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [CaileGame] SET  DISABLE_BROKER 
GO

ALTER DATABASE [CaileGame] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [CaileGame] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [CaileGame] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [CaileGame] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [CaileGame] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [CaileGame] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [CaileGame] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [CaileGame] SET RECOVERY FULL 
GO

ALTER DATABASE [CaileGame] SET  MULTI_USER 
GO

ALTER DATABASE [CaileGame] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [CaileGame] SET DB_CHAINING OFF 
GO

ALTER DATABASE [CaileGame] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [CaileGame] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [CaileGame] SET DELAYED_DURABILITY = DISABLED 
GO

USE [CaileGame]
GO

EXEC [CaileGame].sys.sp_addextendedproperty @name=N'SQLSourceControl Database Revision', @value=286 
GO

USE [master]
GO

ALTER DATABASE [CaileGame] SET  READ_WRITE 
GO

/****** Object:  UserDefinedFunction [dbo].[F_GetSchemesPlayList]    Script Date: 2018/1/18 14:19:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--���ݷ����ź�ȥ�����淨
CREATE FUNCTION [dbo].[F_GetSchemesPlayList](@SchemeID bigint)
RETURNS VARCHAR(512)
AS
BEGIN
	DECLARE @Result VARCHAR(512)
	SET @Result = STUFF((SELECT ','+ b.PlayName FROM dbo.CT_SchemesDetail a
			LEFT JOIN dbo.CT_PlayTypes b ON b.PlayCode=a.PlayCode
		 WHERE a.SchemeID=@SchemeID FOR XML PATH('')), 1 , 1 , '') 
	RETURN @Result
END

go

--�����ض����ŷָ��Ķ�������ַ������һ�����һ�У������ַ���'1,2,3,4,5' ��ת����һ����
CREATE FUNCTION [dbo].[F_SplitStrToTable](@str VARCHAR(8000), @split CHAR(1))
	RETURNS @table TABLE(id INT IDENTITY(1,1), Value VARCHAR(128))
AS
BEGIN
	SET @str = @str+@split
	DECLARE @insertStr VARCHAR(128)
	DECLARE @newstr VARCHAR(8000)
	SET @insertStr = LEFT(@str,CHARINDEX(@split,@str)-1)
	SET @newstr = STUFF(@str,1,CHARINDEX(@split,@str),'')
	INSERT @table VALUES(@insertStr)
	WHILE(LEN(@newstr)>0)
	BEGIN
	   SET @insertStr = LEFT(@newstr,CHARINDEX(@split,@newstr)-1)
	   INSERT @table VALUES(@insertStr)
	   SET @newstr = STUFF(@newstr,1,CHARINDEX(@split,@newstr),'')
	END
	RETURN
END

GO


create function fnGetField(@p_inStr varchar(8000), @p_inPos int, @p_inSpr char)
	returns varchar(8000)    
as
begin
  declare @p_chr char(1), @iPos int, @iStart int, @iEnd int, @rets varchar(8000)
  set @iPos    = 1
  set @iEnd    = 1
  if @p_inPos  = 1
	  set @iStart = 1
  else
	  set @iStart = 0

  while @iPos <= len(@p_inStr)
  begin
	set @p_chr = substring(@p_inStr, @iPos, 1)
	if @p_chr  = @p_inSpr
		set @p_inPos = @p_inPos - 1
	if @p_inPos = 1 and @iStart = 0
		set @iStart = @iPos + 1
	if @p_inPos < 1
	begin
		set @iEnd = @iPos
		break
	end
	set @iPos = @iPos + 1
  end

  if @iEnd < @iStart  --��������򲻴��ڷָ���
	set @rets = substring(@p_inStr, @iStart, len(@p_inStr)-@iStart+1)
  else if @iEnd = @iStart
	set @rets = ''
  else
	set @rets = substring(@p_inStr, @iStart, @iEnd-@iStart)
  return(ltrim(rtrim(@rets)))
end

GO

USE [CaileGame]
GO
/****** Object:  Table [dbo].[CT_Activity]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_Activity](
	[ActivityID] [int] IDENTITY(1,1) NOT NULL,
	[ActivityType] [int] NOT NULL,
	[ActivitySubject] [varchar](50) NOT NULL,
	[ActivityDescribe] [text] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[ModifyTime] [datetime] NULL,
	[ModifyDescribe] [varchar](2000) NULL,
	[ActivityMoney] [bigint] NOT NULL,
	[ModifyMoney] [bigint] NULL,
	[IsModify] [bit] NOT NULL,
	[ActivityApply] [int] NOT NULL,
	[CurrencyUnit] [tinyint] NOT NULL,
	[LandingPage] [varchar](200) NULL,
	[ADUrl] [varchar](500) NULL,
 CONSTRAINT [PK_CT_ACTIVITY] PRIMARY KEY CLUSTERED 
(
	[ActivityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_ActivityAward]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_ActivityAward](
	[RegularID] [int] IDENTITY(1,1) NOT NULL,
	[ActivityID] [int] NOT NULL,
	[RegularType] [int] NOT NULL,
	[LotteryCode] [int] NOT NULL,
	[TotalAwardMoney] [bigint] NOT NULL,
	[RegularStatus] [int] NOT NULL,
 CONSTRAINT [PK_CT_ACTIVITYAWARD] PRIMARY KEY CLUSTERED 
(
	[RegularID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_ChaseTaskDetails]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_ChaseTaskDetails](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ChaseTaskID] [bigint] NOT NULL,
	[SchemeID] [bigint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsuseID] [bigint] NOT NULL,
	[Multiple] [int] NOT NULL,
	[Amount] [bigint] NOT NULL,
	[RedPacketId] [varchar](50) NOT NULL,
	[RedPacketMoney] [money] NOT NULL,
	[QuashStatus] [smallint] NOT NULL,
	[IsExecuted] [bit] NOT NULL,
	[SecrecyLevel] [smallint] NOT NULL,
	[LotteryNumber] [varchar](max) NOT NULL,
	[IsShare] [tinyint] NOT NULL,
	[IsSendOut] [bit] NULL,
 CONSTRAINT [PK_CT_CHASETASKDETAILS] PRIMARY KEY CLUSTERED 
(
	[ID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_ChaseTasks]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_ChaseTasks](
	[ChaseTaskID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[SchemeID] [bigint] NOT NULL,
	[LotteryCode] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[CreateTime] [datetime] NULL,
	[IsuseCount] [int] NOT NULL,
	[BetType] [smallint] NOT NULL,
	[StopTypeWhenWin] [smallint] NOT NULL,
	[StopTypeWhenWinMoney] [bigint] NOT NULL,
	[QuashStatus] [smallint] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Descriptions] [varchar](max) NOT NULL,
	[SchemeBonusScale] [float] NOT NULL,
	[FromClient] [int] NOT NULL,
 CONSTRAINT [PK_CT_CT_ChaseTaskID] PRIMARY KEY CLUSTERED 
(
	[ChaseTaskID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_IsuseBonuses]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_IsuseBonuses](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AdminID] [int] NOT NULL,
	[IsuseID] [bigint] NOT NULL,
	[defaultMoney] [int] NOT NULL,
	[DefaultMoneyNoWithTax] [int] NOT NULL,
	[WinNumber] [varchar](100) NOT NULL,
	[WinBet] [nvarchar](50) NOT NULL,
	[WinLevel] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_CT_IsuseBonuses_ID] PRIMARY KEY CLUSTERED 
(
	[ID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_Isuses]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_Isuses](
	[IsuseID] [bigint] IDENTITY(1,1) NOT NULL,
	[LotteryCode] [int] NOT NULL,
	[IsuseName] [varchar](32) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[IsExecuteChase] [bit] NOT NULL,
	[IsOpened] [bit] NOT NULL,
	[OpenNumber] [varchar](64) NOT NULL,
	[OpenOperatorID] [int] NOT NULL,
	[IsuseState] [tinyint] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[OpenNotice] [varchar](max) NOT NULL,
	[TotalSales] [bigint] NOT NULL,
	[OpenTime] [datetime] NULL,
	[WinRollover] [varchar](50) NOT NULL,
	[BettingPrompt] [varchar](200) NOT NULL,
 CONSTRAINT [PK_CT_Isuses_ID] PRIMARY KEY CLUSTERED 
(
	[IsuseID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_Lotteries]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_Lotteries](
	[LotteryID] [int] IDENTITY(1,1) NOT NULL,
	[LotteryCode] [int] NOT NULL,
	[LotteryName] [nvarchar](50) NOT NULL,
	[Shorthand] [varchar](20) NOT NULL,
	[Subhead] [nvarchar](200) NOT NULL,
	[MaxChaseIsuse] [varchar](20) NOT NULL,
	[Sort] [int] NULL DEFAULT ((999)),
	[TypeID] [tinyint] NULL,
	[WinNumberExemple] [varchar](100) NULL,
	[IntervalType] [varchar](50) NULL,
	[PrintOutType] [smallint] NULL,
	[Price] [int] NULL,
	[MaxMultiple] [int] NULL,
	[OffTime] [int] NULL,
	[ChaseDeferTime] [int] NULL,
	[QuashChaseTime] [int] NULL,
	[Kinformation] [ntext] NULL,
	[IsEmphasis] [bit] NULL CONSTRAINT [DF_CT_Lotteries_Emphasis]  DEFAULT ((0)),
	[ModuleVersion] [int] NULL CONSTRAINT [DF_CT_Lotteries_ModuleVersion]  DEFAULT ((100)),
	[IsAddaward] [bit] NOT NULL CONSTRAINT [DF_CT_Lotteries_Addaward]  DEFAULT ((0)),
	[IsEnable] [bit] NOT NULL CONSTRAINT [DF_CT_Lotteries_IsEnable]  DEFAULT ((1)),
	[StopReason] [varchar](200) NULL,
	[IsStop] [bit] NULL CONSTRAINT [DF_CT_Lotteries_IsStop]  DEFAULT ((0)),
	[IsHot] [bit] NULL,
	[AdvanceEndTime] [int] NULL CONSTRAINT [DF_CT_Lotteries_AdvanceEndTime]  DEFAULT ((0)),
	[PresellTime] [int] NULL CONSTRAINT [DF_CT_Lotteries_PresellTime]  DEFAULT ((0)),
	[ChatGroups] [varchar](50) NULL,
	[ChatRooms] [varchar](50) NULL,
 CONSTRAINT [PK_CT_LOTTERIES] PRIMARY KEY CLUSTERED 
(
	[LotteryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_News]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_News](
	[NewsID] [int] IDENTITY(1,1) NOT NULL,
	[TypeID] [int] NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Keys] [varchar](100) NOT NULL,
	[RichText] [text] NOT NULL,
	[PlainText] [text] NOT NULL,
	[LotteryCode] [int] NULL,
	[Author] [varchar](50) NULL,
	[Source] [varchar](200) NULL,
	[Sort] [int] NULL,
	[IsRecommend] [bit] NULL,
	[Equipment] [int] NULL,
	[PublishID] [bigint] NULL,
	[Publish] [varchar](50) NULL,
	[PublishTime] [datetime] NULL,
	[ModifyTime] [datetime] NULL,
	[Modify] [varchar](50) NULL,
	[ModifyID] [bigint] NULL,
	[AuditingID] [bigint] NULL,
	[Auditing] [varchar](50) NULL,
	[AuditingTime] [datetime] NULL,
	[ReadNum] [int] NULL,
	[SupportNum] [int] NULL,
	[OpposeNum] [int] NULL,
	[AuditingStatus] [int] NULL,
	[IsDel] [bit] NULL,
	[LotNumber] [varchar](200) NULL,
 CONSTRAINT [PK_T_News] PRIMARY KEY CLUSTERED 
(
	[NewsID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_NewsTypes]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_NewsTypes](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL DEFAULT ((0)),
	[TypeName] [varchar](50) NOT NULL DEFAULT (''),
	[IsShow] [bit] NOT NULL DEFAULT ((1)),
	[IsSystem] [bit] NOT NULL DEFAULT ((1)),
	[Sort] [int] NOT NULL DEFAULT ((0)),
	[Remarks] [varchar](512) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_NewsTypes_ID] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_OutETickets]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_OutETickets](
	[OutETicketsID] [bigint] IDENTITY(1,1) NOT NULL,
	[MerchantCode] [int] NOT NULL,
	[LotteryCode] [int] NOT NULL,
	[MerchantTicket] [varchar](50) NOT NULL,
	[HuaYangTicket] [varchar](50) NOT NULL,
	[Money] [bigint] NOT NULL,
	[Multiple] [int] NOT NULL,
	[Bonus] [bigint] NOT NULL,
	[SendTicketDateTime] [datetime] NOT NULL,
	[OutTicketDateTime] [datetime] NOT NULL,
	[OutTicketStauts] [tinyint] NOT NULL,
 CONSTRAINT [PK_CT_OutETickets] PRIMARY KEY CLUSTERED 
(
	[OutETicketsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_PlayTypes]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_PlayTypes](
	[PlayID] [int] IDENTITY(1,1) NOT NULL,
	[PlayCode] [int] NOT NULL DEFAULT ((0)),
	[LotteryCode] [int] NOT NULL DEFAULT ((0)),
	[PlayName] [varchar](32) NOT NULL DEFAULT (''),
	[Price] [int] NOT NULL DEFAULT ((0)),
	[ModuleName] [varchar](255) NOT NULL DEFAULT (''),
	[MaxMultiple] [int] NOT NULL DEFAULT ((0)),
	[Sort] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_CT_PLAYTYPES] PRIMARY KEY CLUSTERED 
(
	[PlayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_RegularAwardInterval]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_RegularAwardInterval](
	[RAwardIntervalID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[AwardInterval] [xml] NOT NULL,
 CONSTRAINT [PK_CT_REGULARAWARDINTERVAL] PRIMARY KEY CLUSTERED 
(
	[RAwardIntervalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_RegularAwardRanking]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_RegularAwardRanking](
	[RAwardRanID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[AwardRanking] [xml] NOT NULL,
 CONSTRAINT [PK_CT_REGULARAWARDRANKING] PRIMARY KEY CLUSTERED 
(
	[RAwardRanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_RegularBall]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_RegularBall](
	[RBallID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[AwardMoney] [bigint] NOT NULL,
	[TopLimit] [bigint] NOT NULL,
	[BallType] [int] NOT NULL,
	[Ball] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CT_REGULARBALL] PRIMARY KEY CLUSTERED 
(
	[RBallID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_RegularBetInterval]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_RegularBetInterval](
	[RBetIntervalID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[BetInterval] [xml] NOT NULL,
 CONSTRAINT [PK_CT_REGULARBETINTERVAL] PRIMARY KEY CLUSTERED 
(
	[RBetIntervalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_RegularBetRanking]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_RegularBetRanking](
	[RBetRanID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[BetRanking] [xml] NOT NULL,
 CONSTRAINT [PK_CT_REGULARBETRANKING] PRIMARY KEY CLUSTERED 
(
	[RBetRanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_RegularChase]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_RegularChase](
	[RChaseID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[RChaseType] [int] NOT NULL,
	[Unit] [bigint] NOT NULL,
	[AwardMoney] [bigint] NOT NULL,
	[TopLimit] [bigint] NOT NULL,
	[PlayCode] [int] NOT NULL,
 CONSTRAINT [PK_CT_REGULARCHASE] PRIMARY KEY CLUSTERED 
(
	[RChaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_RegularDanTuo]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_RegularDanTuo](
	[RDanTuoID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[DanNums] [int] NOT NULL,
	[TuoNums] [int] NOT NULL,
	[AwardMoney] [bigint] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[TopLimit] [bigint] NOT NULL,
 CONSTRAINT [PK_CT_REGULARDANTUO] PRIMARY KEY CLUSTERED 
(
	[RDanTuoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_RegularHoliday]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_RegularHoliday](
	[RHolidayID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[HolidayType] [int] NOT NULL,
	[AwardMoney] [bigint] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[TopLimitDay] [bigint] NOT NULL,
 CONSTRAINT [PK_CT_REGULARHOLIDAY] PRIMARY KEY CLUSTERED 
(
	[RHolidayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_RegularNorm]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_RegularNorm](
	[RNormID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[AwardMoney] [bigint] NOT NULL,
	[TopLimit] [bigint] NOT NULL,
 CONSTRAINT [PK_CT_REGULARNORM] PRIMARY KEY CLUSTERED 
(
	[RNormID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_RegularTopLimit]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_RegularTopLimit](
	[RTopLimitID] [int] IDENTITY(1,1) NOT NULL,
	[RegularID] [int] NOT NULL,
	[TotalMoney] [bigint] NOT NULL,
	[AwardMoney] [bigint] NOT NULL,
	[PlayCode] [int] NOT NULL,
 CONSTRAINT [PK_CT_REGULARTOPLIMIT] PRIMARY KEY CLUSTERED 
(
	[RTopLimitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_SalePoint]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_SalePoint](
	[SalePointID] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketSourceID] [int] NOT NULL,
	[LotteryCode] [int] NOT NULL,
	[LotteryName] [varchar](50) NULL,
	[SalesRebate] [varchar](200) NOT NULL,
	[FileSign] [bigint] NOT NULL,
	[OperatorID] [bigint] NOT NULL,
	[OperatorName] [varchar](20) NOT NULL,
	[OperatorTime] [datetime] NOT NULL,
	[AuditorID] [bigint] NULL,
	[AuditorName] [varchar](20) NULL,
	[AuditTime] [datetime] NULL,
	[SalePointStatus] [tinyint] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NULL,
	[Describe] [varchar](200) NULL,
 CONSTRAINT [PK_CT_SalePoint] PRIMARY KEY CLUSTERED 
(
	[SalePointID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_SalePointFile]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_SalePointFile](
	[SalePointFileID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreateTime] [datetime] NULL,
	[FileUrl] [varchar](200) NOT NULL,
	[FileName] [varchar](50) NULL,
	[FileEXT] [varchar](10) NULL,
	[FileSign] [bigint] NOT NULL,
 CONSTRAINT [PK_CT_SalePointFile] PRIMARY KEY CLUSTERED 
(
	[SalePointFileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_SalePointRecord]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_SalePointRecord](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketSourceID] [int] NOT NULL,
	[LotteryCode] [int] NOT NULL,
	[SalesRebate] [varchar](200) NOT NULL,
	[LastSalesRebate] [varchar](200) NULL,
	[StartTime] [datetime] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_SchemeETickets]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_SchemeETickets](
	[SchemeETicketsID] [bigint] IDENTITY(1,1) NOT NULL,
	[SDID] [bigint] NOT NULL,
	[TicketSourceID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[SchemeID] [bigint] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[TicketMoney] [bigint] NOT NULL,
	[Multiple] [int] NOT NULL,
	[Number] [varchar](2048) NOT NULL,
	[Sends] [smallint] NOT NULL,
	[HandleDateTime] [datetime] NOT NULL,
	[HandleDescribe] [varchar](100) NULL,
	[Identifiers] [varchar](50) NOT NULL,
	[Ticket] [varchar](max) NOT NULL,
	[WinMoney] [bigint] NOT NULL,
	[TicketStatus] [tinyint] NOT NULL,
	[ChaseTaskDetailsID] [bigint] NOT NULL,
	[IsRobot] [bit] NULL,
 CONSTRAINT [PK_CT_SCHEMEETICKETS] PRIMARY KEY CLUSTERED 
(
	[SchemeETicketsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_Schemes]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_Schemes](
	[SchemeID] [bigint] IDENTITY(1,1) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[SchemeNumber] [varchar](50) NOT NULL,
	[Title] [varchar](60) NOT NULL,
	[InitiateUserID] [bigint] NOT NULL,
	[LotteryCode] [int] NOT NULL,
	[IsuseID] [bigint] NOT NULL,
	[IsuseName] [varchar](20) NOT NULL,
	[LotteryNumber] [varchar](max) NOT NULL,
	[SchemeMoney] [bigint] NOT NULL,
	[SecrecyLevel] [smallint] NOT NULL,
	[SchemeStatus] [smallint] NOT NULL,
	[PrintOutType] [tinyint] NOT NULL,
	[Describe] [varchar](200) NULL,
	[FromClient] [tinyint] NOT NULL,
	[BuyType] [tinyint] NOT NULL,
	[IsSplit] [bit] NOT NULL,
	[FollowSchemeID] [bigint] NOT NULL,
	[FollowSchemeBonus] [int] NOT NULL,
	[FollowSchemeBonusScale] [int] NOT NULL,
	[PlusAwards] [int] NOT NULL,
	[IsSendOut] [bit] NULL,
	[RoomCode] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CT_Schemes] PRIMARY KEY CLUSTERED 
(
	[SchemeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_SchemesAwards]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_SchemesAwards](
	[AwardsID] [bigint] IDENTITY(1,1) NOT NULL,
	[SchemeID] [bigint] NOT NULL,
	[SchemeETicketID] [bigint] NOT NULL,
	[RegularID] [int] NOT NULL,
	[AwardType] [tinyint] NOT NULL,
	[AwardTime] [datetime] NOT NULL,
	[AwardMoney] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
 CONSTRAINT [PK_CT_SCHEMESAWARDS] PRIMARY KEY CLUSTERED 
(
	[AwardsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_SchemesDetail]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_SchemesDetail](
	[SDID] [bigint] IDENTITY(1,1) NOT NULL,
	[SchemeID] [bigint] NOT NULL,
	[PlayCode] [int] NOT NULL,
	[Multiple] [int] NOT NULL,
	[BetMoney] [bigint] NOT NULL,
	[BetNum] [int] NOT NULL,
	[BetNumber] [varchar](max) NULL,
	[IsBuyed] [bit] NULL,
	[IsNorm] [tinyint] NOT NULL,
	[IsWin] [tinyint] NOT NULL,
	[IsOpened] [bit] NOT NULL,
	[WinMoney] [bigint] NOT NULL,
	[WinMoneyNoWithTax] [bigint] NOT NULL,
	[Schedule] [int] NOT NULL,
	[WinDescribe] [varchar](max) NULL,
	[PrintOutTime] [datetime] NULL,
	[WinImageUrl] [varchar](300) NULL,
	[UpdateTime] [datetime] NOT NULL,
	[OpenOperatorID] [int] NOT NULL,
 CONSTRAINT [PK_CT_SCHEMESDETAIL] PRIMARY KEY CLUSTERED 
(
	[SDID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_SchemesWin]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_SchemesWin](
	[SchemesDetailsWinID] [bigint] IDENTITY(1,1) NOT NULL,
	[SchemeETicketsID] [bigint] NOT NULL,
	[SchemeID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[LotteryCode] [int] NOT NULL,
	[WinCode] [int] NOT NULL,
	[SplitNumber] [varchar](max) NULL,
	[WinNumber] [varchar](128) NOT NULL,
	[Multiple] [int] NOT NULL,
	[WinMoney] [bigint] NOT NULL,
	[WinMoneyNoWithTax] [bigint] NOT NULL,
	[IsAward] [tinyint] NOT NULL,
	[Descriptions] [varchar](128) NOT NULL,
	[SupplierID] [int] NOT NULL,
	[BackWinMoney] [bigint] NULL,
	[BackWinMoneyNoWithTax] [bigint] NULL,
	[BackDateTime] [datetime] NULL,
	[AddDateTime] [datetime] NULL,
	[WinStatus] [tinyint] NOT NULL,
	[IsDel] [bit] NULL,
	[IsFirstPrize] [tinyint] NOT NULL,
 CONSTRAINT [PK_CT_SchemesDetailsWin] PRIMARY KEY CLUSTERED 
(
	[SchemesDetailsWinID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_SystemSetInfo]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_SystemSetInfo](
	[SetID] [int] IDENTITY(1,1) NOT NULL,
	[SetKey] [varchar](32) NOT NULL DEFAULT (''),
	[SetValue] [varchar](128) NOT NULL DEFAULT (''),
	[SetName] [nvarchar](32) NOT NULL DEFAULT (''),
	[SetDetail] [varchar](1024) NOT NULL DEFAULT (''),
	[Sort] [int] NOT NULL DEFAULT ((0)),
	[IsUse] [tinyint] NOT NULL DEFAULT ((0)),
	[DataType] [tinyint] NOT NULL DEFAULT ((0)),
	[DataValue] [varchar](512) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_CT_SYSTEMSETINFO] PRIMARY KEY CLUSTERED 
(
	[SetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_SystemStaticdata]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_SystemStaticdata](
	[dateday] [varchar](10) NOT NULL,
	[recharge] [bigint] NOT NULL,
	[online_recharge] [bigint] NOT NULL,
	[offline_recharge] [bigint] NOT NULL,
	[withdraw] [bigint] NOT NULL,
	[users] [int] NOT NULL,
	[largess] [int] NOT NULL,
	[buy_jlk] [bigint] NOT NULL,
	[win_jlk] [bigint] NOT NULL,
	[buy_jxk] [bigint] NOT NULL,
	[win_jxk] [bigint] NOT NULL,
	[buy_hbsyydj] [bigint] NOT NULL,
	[win_hbsyydj] [bigint] NOT NULL,
	[buy_sdsyydj] [bigint] NOT NULL,
	[win_sdsyydj] [bigint] NOT NULL,
	[buy_cqssc] [bigint] NOT NULL,
	[win_cqssc] [bigint] NOT NULL,
	[buy_ssq] [bigint] NOT NULL,
	[win_ssq] [bigint] NOT NULL,
	[buy_dlt] [bigint] NOT NULL,
	[win_dlt] [bigint] NOT NULL,
 CONSTRAINT [PK_CT_SystemStaticdata] PRIMARY KEY CLUSTERED 
(
	[dateday] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_TemplateConfig]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_TemplateConfig](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL CONSTRAINT [DF__CT_Templa__Title__3B56A726]  DEFAULT (''),
	[TemplateContent] [nvarchar](900) NOT NULL CONSTRAINT [DF__CT_Templa__Conte__3C4ACB5F]  DEFAULT (''),
	[TemplateType] [tinyint] NOT NULL CONSTRAINT [DF__CT_Templa__Templ__3D3EEF98]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF__CT_Templa__Creat__3E3313D1]  DEFAULT (getdate()),
	[AdminID] [int] NOT NULL CONSTRAINT [DF__CT_Templa__Admin__3F27380A]  DEFAULT ((0)),
 CONSTRAINT [PK_CT_TemplateConfig] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_Users]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_Users](
	[UserID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](20) NOT NULL,
	[UserMobile] [varchar](13) NULL,
	[UserPassword] [varchar](32) NOT NULL,
	[PayPassword] [varchar](32) NULL,
	[Balance] [bigint] NOT NULL,
	[GoldBean] [bigint] NOT NULL,
	[Freeze] [bigint] NOT NULL,
	[IsRobot] [bit] NOT NULL,
	[IsCanLogin] [bit] NOT NULL,
 CONSTRAINT [PK_CT_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_UsersBanks]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_UsersBanks](
	[BankID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[BankType] [smallint] NOT NULL,
	[BankName] [varchar](60) NOT NULL,
	[CardNumber] [varchar](20) NOT NULL,
	[Area] [varchar](60) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ReservedPhone] [varchar](20) NULL,
 CONSTRAINT [PK_CT_UsersBanks] PRIMARY KEY CLUSTERED 
(
	[BankID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_UsersExtend]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_UsersExtend](
	[UserID] [bigint] NOT NULL,
	[NickName] [varchar](20) NOT NULL CONSTRAINT [DF__CT_UsersE__NickN__3C5FD9F8]  DEFAULT ('����'),
	[UserLevel] [tinyint] NOT NULL CONSTRAINT [DF_CT_UsersExtend_UserLevel]  DEFAULT ((0)),
	[SpecialLevel] [tinyint] NOT NULL CONSTRAINT [DF_CT_UsersExtend_SpecialLevel]  DEFAULT ((0)),
	[FullName] [varchar](26) NULL,
	[IDNumber] [varchar](18) NULL,
	[AvatarAddress] [varchar](200) NULL CONSTRAINT [DF__CT_UsersE__Avata__3F3C46A3]  DEFAULT ('����'),
	[WechatID] [varchar](100) NULL,
	[WechatToken] [varchar](200) NULL,
	[AliPayID] [varchar](100) NULL,
	[AliPayToken] [varchar](200) NULL,
	[QQID] [varchar](100) NULL,
	[QQToken] [varchar](200) NULL,
	[Email] [varchar](50) NULL CONSTRAINT [DF__CT_UsersE__Email__22C00386]  DEFAULT (''),
	[BindType] [tinyint] NULL CONSTRAINT [DF_CT_UsersExtend_BindType]  DEFAULT ((0)),
	[CreateTime] [datetime] NULL CONSTRAINT [DF_CT_UsersExtend_CreateTime]  DEFAULT (getdate()),
	[BindTime] [datetime] NULL,
	[Idols] [int] NULL,
	[SourceType] [tinyint] NULL,
	[IpAddress] [varchar](32) NULL,
	[RelationID] [varchar](100) NULL,
	[IsVerify] [bit] NULL CONSTRAINT [DF_CT_UsersExtend_IsVerify]  DEFAULT ((0)),
	[IsBindTel] [bit] NULL,
	[WithdrawMoney] [bigint] NULL DEFAULT ((0)),
 CONSTRAINT [PK_CT_UsersExtend] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_UsersLoginRecord]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_UsersLoginRecord](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[SourceType] [tinyint] NOT NULL,
	[Token] [varchar](200) NOT NULL,
	[LoginTime] [datetime] NOT NULL,
	[IpAddress] [varchar](32) NULL,
 CONSTRAINT [PK_CT_USERLOGINRECORD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_UsersPayDetail]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_UsersPayDetail](
	[PayID] [bigint] IDENTITY(2410000001,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[OrderNo] [varchar](50) NOT NULL,
	[RechargeNo] [varchar](200) NOT NULL,
	[PayType] [varchar](50) NOT NULL,
	[Amount] [bigint] NOT NULL,
	[FormalitiesFees] [money] NOT NULL,
	[Result] [smallint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CompleteTime] [datetime] NULL,
	[OutRechargeNo] [varchar](200) NULL,
	[IsDel] [bit] NULL,
 CONSTRAINT [PK_CT_USERPAYDETAILS] PRIMARY KEY CLUSTERED 
(
	[PayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_UsersPayRefund]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_UsersPayRefund](
	[ReID] [bigint] IDENTITY(1,1) NOT NULL,
	[PayID] [bigint] NOT NULL,
	[RefundNo] [varchar](32) NOT NULL,
	[RechargeNo] [varchar](128) NOT NULL,
	[Amount] [bigint] NOT NULL,
	[FormalitiesFees] [int] NOT NULL,
	[Result] [tinyint] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CompleteTime] [datetime] NULL,
 CONSTRAINT [PK_CT_UserPayRefund] PRIMARY KEY CLUSTERED 
(
	[ReID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_UsersPush]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_UsersPush](
	[UserId] [bigint] NOT NULL,
	[ModifyTime] [datetime] NULL,
	[PushIdentify] [varchar](100) NULL,
 CONSTRAINT [PK_CT_UsersPush] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_UsersRecord]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_UsersRecord](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[TradeType] [tinyint] NOT NULL,
	[TradeAmount] [bigint] NOT NULL,
	[Balance] [bigint] NOT NULL,
	[TradeRemark] [nvarchar](100) NULL,
	[RelationID] [varchar](32) NULL,
	[OperatorID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CouponsID] [bigint] NULL,
 CONSTRAINT [PK_CT_UsersRecord_ID] PRIMARY KEY CLUSTERED 
(
	[ID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_UsersStaticdata]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_UsersStaticdata](
	[Date] [int] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Buy] [bigint] NOT NULL,
	[Win] [bigint] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_UsersWithdraw]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_UsersWithdraw](
	[PayOutID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Amount] [bigint] NOT NULL,
	[BankID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[OperaterID] [bigint] NULL,
	[OperTime] [datetime] NULL,
	[PayOutStatus] [tinyint] NOT NULL,
	[IsDel] [bit] NULL,
	[Remark] [varchar](100) NULL,
 CONSTRAINT [PK_CT_USERPAYOUT] PRIMARY KEY CLUSTERED 
(
	[PayOutID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_WinTypes]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_WinTypes](
	[WinID] [int] IDENTITY(1,1) NOT NULL,
	[WinCode] [int] NULL CONSTRAINT [DF_CT_WinTypes_WinCode]  DEFAULT ((0)),
	[LotteryCode] [int] NULL CONSTRAINT [DF_CT_WinTypes_LotteryCode]  DEFAULT ((0)),
	[PlayCode] [int] NULL CONSTRAINT [DF_CT_WinTypes_PlayCode]  DEFAULT ((0)),
	[WinName] [varchar](50) NULL,
	[WinNumber] [varchar](max) NULL,
	[IsSumValue] [tinyint] NULL CONSTRAINT [DF_CT_WinTypes_IsSumValue]  DEFAULT ((0)),
	[SumValue] [int] NULL DEFAULT ((0)),
	[Sort] [int] NULL CONSTRAINT [DF_CT_WinTypes_Sort]  DEFAULT ((0)),
	[DefaultMoney] [int] NULL CONSTRAINT [DF_CT_WinTypes_DefaultMoney]  DEFAULT ((0)),
	[DefaultMoneyNoWithTax] [int] NULL CONSTRAINT [DF_CT_WinTypes_DefaultMoneyNoWithTax]  DEFAULT ((0)),
	[IsDel] [bit] NULL CONSTRAINT [DF_CT_WinTypes_IsDel]  DEFAULT ((0)),
 CONSTRAINT [PK_CT_WINTYPES] PRIMARY KEY CLUSTERED 
(
	[WinID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[udv_ActivityApply]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[udv_ActivityApply]
AS
SELECT   ac.ActivityType, ac.ActivitySubject, ac.CreateTime, ac.StartTime, ac.EndTime, ac.ActivityMoney, ac.CurrencyUnit, 
                ac.ActivityApply, ac.ActivityID, COUNT(aa.RegularID) AS RegularCount
FROM      dbo.CT_Activity AS ac LEFT OUTER JOIN
                dbo.CT_ActivityAward AS aa ON aa.ActivityID = ac.ActivityID
GROUP BY ac.ActivityType, ac.ActivitySubject, ac.CreateTime, ac.StartTime, ac.EndTime, ac.ActivityMoney, ac.CurrencyUnit, 
                ac.ActivityApply, ac.ActivityID


GO
/****** Object:  View [dbo].[udv_BettingTickets]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_BettingTickets]
AS
	SELECT c.UserName AS TicketUser, d.IDNumber AS Identify, c.UserMobile AS Phone, d.Email, a.SchemeETicketsID, b.LotteryCode, 
		a.PlayCode,ISNULL(i.IsuseName,b.IsuseName) AS IsuseName, a.Number, a.Multiple, 
		a.TicketMoney AS Amount, b.SchemeID, sd.BetNum AS Bet, a.TicketStatus,a.ChaseTaskDetailsID
	FROM dbo.CT_SchemeETickets AS a 
	LEFT OUTER JOIN dbo.CT_Schemes AS b ON b.SchemeID = a.SchemeID 
	LEFT OUTER JOIN dbo.CT_SchemesDetail AS sd ON sd.SchemeID=b.SchemeID AND sd.SDID = a.SDID
	LEFT OUTER JOIN dbo.CT_Users AS c ON c.UserID = b.InitiateUserID 
	LEFT OUTER JOIN dbo.CT_UsersExtend AS d ON d.UserID = c.UserID
	LEFT OUTER JOIN dbo.CT_ChaseTaskDetails AS ctd ON ctd.SchemeID=a.SchemeID AND ctd.ID=a.ChaseTaskDetailsID
	LEFT OUTER JOIN dbo.CT_Isuses AS i ON i.IsuseID=ctd.IsuseID 


GO
/****** Object:  View [dbo].[udv_ChaseList]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_ChaseList]
AS
SELECT   ct.ChaseTaskID, ct.SchemeID, u.UserID, u.UserName, s.CreateTime, s.LotteryCode, l.LotteryName, s.IsuseName, 
                ct.Title, s.SchemeMoney, ct.IsuseCount, ct.QuashStatus, ct.StopTypeWhenWin, ct.StopTypeWhenWinMoney, 
                ct.StartTime, ct.EndTime, s.SchemeNumber, s.SchemeStatus, COUNT(cd.ID) AS BuyedIsuseNum, COUNT(cd1.ID) 
                AS QuashedIsuseNum
FROM      dbo.CT_Schemes AS s INNER JOIN
                dbo.CT_ChaseTasks AS ct ON ct.SchemeID = s.SchemeID AND s.BuyType = 1 LEFT OUTER JOIN
                dbo.CT_ChaseTaskDetails AS cd ON cd.SchemeID = s.SchemeID AND cd.ChaseTaskID = ct.ChaseTaskID AND 
                cd.IsExecuted = 1 AND cd.QuashStatus = 0 LEFT OUTER JOIN
                dbo.CT_ChaseTaskDetails AS cd1 ON cd1.SchemeID = s.SchemeID AND cd1.ChaseTaskID = ct.ChaseTaskID AND 
                cd1.IsExecuted = 0 AND cd1.QuashStatus = 0 INNER JOIN
                dbo.CT_Users AS u ON u.UserID = s.InitiateUserID INNER JOIN
                dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
GROUP BY ct.SchemeID, ct.ChaseTaskID, u.UserID, u.UserName, s.CreateTime, s.LotteryCode, l.LotteryName, s.IsuseName, 
                ct.Title, s.SchemeMoney, ct.IsuseCount, ct.QuashStatus, ct.StopTypeWhenWin, ct.StopTypeWhenWinMoney, 
                ct.StartTime, ct.EndTime, s.SchemeNumber, s.SchemeStatus

GO
/****** Object:  View [dbo].[udv_ChaseRevoke]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_ChaseRevoke]
AS
	SELECT DISTINCT d.*,u.UserID,s.LotteryCode,i.IsuseName FROM dbo.CT_Schemes AS s
	INNER JOIN dbo.CT_ChaseTasks AS c ON c.SchemeID=s.SchemeID AND s.SchemeStatus=19
	INNER JOIN dbo.CT_ChaseTaskDetails AS  d ON d.SchemeID=s.SchemeID AND d.ChaseTaskID= c.ChaseTaskID AND d.IsSendOut=0 AND d.IsExecuted=0
	INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID=d.IsuseID AND GETDATE() >i.EndTime AND i.IsOpened=1
	INNER JOIN dbo.CT_Users AS u ON u.UserID=s.InitiateUserID

GO
/****** Object:  View [dbo].[udv_ChaseTaskDetails]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_ChaseTaskDetails]
AS
	SELECT s.SchemeID,s.LotteryCode,i.StartTime,i.EndTime,i.IsuseName,ctd.ID AS ChaseTaskDetailsID,ctd.Amount,
	ctd.Multiple,s.InitiateUserID AS UserID
	FROM dbo.CT_Schemes AS s
	INNER JOIN dbo.CT_ChaseTaskDetails AS ctd ON ctd.SchemeID=s.SchemeID AND s.BuyType=1 AND s.SchemeStatus=19 AND ctd.QuashStatus=0
	INNER JOIN dbo.CT_ChaseTasks AS cs ON cs.ChaseTaskID=ctd.ChaseTaskID AND cs.SchemeID=s.SchemeID AND cs.QuashStatus=0
	INNER JOIN dbo.CT_Isuses  AS i ON i.IsuseID=ctd.IsuseID AND ctd.IsExecuted=0 AND i.LotteryCode=s.LotteryCode 
	INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode 
	AND DATEADD(MINUTE,-l.PresellTime,i.StartTime) < GETDATE() 
	AND DATEADD(MINUTE,-l.AdvanceEndTime,i.EndTime) > GETDATE() 


GO
/****** Object:  View [dbo].[udv_ComputeTicket]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--�㽱��ѯ
CREATE VIEW [dbo].[udv_ComputeTicket]
AS
SELECT DISTINCT a.SchemeETicketsID, b.LotteryCode, a.PlayCode, a.Multiple, a.Number, 
      a.SchemeID, c.IsuseName, c.OpenNumber, a.TicketStatus, c.IsOpened, c.IsuseState, 
      0 AS WinCode, 0 AS SumWinMoney, 0 AS SumWinMoneyNoWithTax, 
      '' AS Description, 0 AS IsFirstPrize,a.TicketMoney
FROM dbo.CT_SchemeETickets AS a LEFT OUTER JOIN
      dbo.CT_Schemes AS b ON b.SchemeID = a.SchemeID AND 
      b.BuyType != 1 LEFT OUTER JOIN
      dbo.CT_Isuses AS c ON c.IsuseID = b.IsuseID AND 
      c.OpenNumber <> '' LEFT OUTER JOIN
      dbo.CT_SchemesDetail AS d ON d.SchemeID = b.SchemeID



GO
/****** Object:  View [dbo].[udv_ComputeTicketChaseTasks]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                  
CREATE VIEW [dbo].[udv_ComputeTicketChaseTasks]
AS

SELECT a.SchemeETicketsID, b.LotteryCode, sd.PlayCode, a.Multiple, 
      a.Number, a.SchemeID, c.IsuseName, c.OpenNumber, a.TicketStatus, 
      c.IsOpened, c.IsuseState, 0 AS WinCode, 0 AS SumWinMoney, 
      0 AS SumWinMoneyNoWithTax, '' AS Description, 0 AS IsFirstPrize,a.ChaseTaskDetailsID
FROM dbo.CT_SchemeETickets AS a INNER JOIN
      dbo.CT_Schemes AS b ON b.SchemeID = a.SchemeID AND 
      b.BuyType = 1 INNER JOIN
      dbo.CT_SchemesDetail AS sd ON sd.SchemeID = a.SchemeID INNER JOIN
      dbo.CT_ChaseTaskDetails AS cd ON cd.SchemeID = a.SchemeID AND 
      cd.ID = a.ChaseTaskDetailsID AND cd.QuashStatus = 0 AND 
      cd.IsExecuted = 1 INNER JOIN
      dbo.CT_Isuses AS c ON c.IsuseID = cd.IsuseID AND c.OpenNumber <> ''




GO
/****** Object:  View [dbo].[udv_OrderDetailReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[udv_OrderDetailReport]
AS
	SELECT u.UserID,u.UserName,u.UserMobile,ue.IsVerify,u.IsRobot,u.IsCanLogin,
	s.BuyType,s.SchemeNumber,s.LotteryNumber,s.CreateTime,s.IsuseName,s.SchemeStatus,s.SchemeMoney,
	l.LotteryName,ISNULL(t.IsuseCount,0) AS IsuseCount,t.ChaseTaskID,l.LotteryCode,SUM(sd.WinMoney) as WinMoney,s.SchemeID
	FROM dbo.CT_Users AS u
	LEFT JOIN dbo.CT_UsersExtend AS ue ON ue.UserID=u.UserID
	INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=u.UserID
	INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode=s.LotteryCode
	INNER JOIN dbo.CT_SchemesDetail AS sd ON sd.SchemeID=s.SchemeID
	LEFT JOIN dbo.CT_ChaseTasks AS t ON t.SchemeID=s.SchemeID
	GROUP BY u.UserID,u.UserName,u.UserMobile,ue.IsVerify,u.IsRobot,u.IsCanLogin,
	s.BuyType,s.SchemeNumber,s.LotteryNumber,s.CreateTime,s.IsuseName,s.SchemeStatus,s.SchemeMoney,
	l.LotteryName,t.IsuseCount,t.ChaseTaskID,l.LotteryCode,s.SchemeID


GO
/****** Object:  View [dbo].[udv_OutTickets]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[udv_OutTickets]
AS
	SELECT	a.SchemeETicketsID, a.SchemeID, b.LotteryCode, a.TicketStatus,a.ChaseTaskDetailsID FROM 
		dbo.CT_SchemeETickets AS a 
		LEFT JOIN dbo.CT_Schemes AS b ON b.SchemeID = a.SchemeID AND a.TicketSourceID != 0



GO
/****** Object:  View [dbo].[udv_OverChaseTasksExamine]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_OverChaseTasksExamine]
AS
	
			SELECT s.SchemeID,cs.ChaseTaskID,cs.StopTypeWhenWinMoney,s.SchemeStatus AS OrderStatus,cs.QuashStatus,s.LotteryCode FROM CT_Schemes AS s
			INNER JOIN dbo.CT_ChaseTasks AS cs ON cs.SchemeID=s.SchemeID AND s.BuyType=1 AND s.SchemeStatus=19
			INNER JOIN dbo.CT_ChaseTaskDetails AS cd ON cd.SchemeID=s.SchemeID AND cd.ChaseTaskID=cs.ChaseTaskID
			INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID = cd.IsuseID AND i.LotteryCode=s.LotteryCode AND i.IsuseState>=4


GO
/****** Object:  View [dbo].[udv_PlayTypes]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_PlayTypes]
AS
SELECT   a.PlayID, a.PlayCode, a.LotteryCode, a.PlayName, a.Price, a.ModuleName, a.MaxMultiple, a.Sort, b.LotteryName
FROM      dbo.CT_PlayTypes AS a LEFT OUTER JOIN
                dbo.CT_Lotteries AS b ON b.LotteryCode = a.LotteryCode

GO
/****** Object:  View [dbo].[udv_RechargeDetailReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_RechargeDetailReport]
AS
SELECT   u.UserID, u.UserName, u.UserMobile, ue.IsVerify, u.IsRobot, u.IsCanLogin, c.TradeAmount, c.Balance, c.TradeRemark, 
                c.CreateTime, c.ID AS ChangeID, d.OrderNo, d.FormalitiesFees, d.PayID, d.RechargeNo, d.OutRechargeNo, d.PayType, 
                d.CompleteTime, d.Result
FROM      dbo.CT_Users AS u INNER JOIN
                dbo.CT_UsersExtend AS ue ON ue.UserID = u.UserID LEFT JOIN
                dbo.CT_UsersRecord AS c ON c.UserID = u.UserID AND c.TradeType = 0 INNER JOIN
                dbo.CT_UsersPayDetail AS d ON d.UserID = u.UserID AND (d.PayID = c.RelationID OR d.OrderNo = c.RelationID)


GO
/****** Object:  View [dbo].[udv_SchemeChaseTask]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_SchemeChaseTask]
AS
SELECT DISTINCT 
                a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, a.LotteryCode, 
                a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, a.BuyType, a.FromClient, a.IsSendOut, 
                a.FollowSchemeID, a.FollowSchemeBonus, a.FollowSchemeBonusScale, b.UserName, f.LotteryName, sd.Multiple, 
                sd.BetNum, sd.BetNumber, pt.ModuleName, pt.PlayName, ct.BetType, ct.IsuseCount, ct.StartTime, ct.EndTime, 
                ct.StopTypeWhenWinMoney, ct.QuashStatus
FROM      dbo.CT_Schemes AS a INNER JOIN
                dbo.CT_SchemesDetail AS sd ON a.SchemeID = sd.SchemeID INNER JOIN
                dbo.CT_PlayTypes AS pt ON pt.PlayCode = sd.PlayCode INNER JOIN
                dbo.CT_Users AS b ON b.UserID = a.InitiateUserID AND a.BuyType = 1 LEFT OUTER JOIN
                dbo.CT_Lotteries AS f ON f.LotteryCode = a.LotteryCode LEFT OUTER JOIN
                dbo.CT_ChaseTasks AS ct ON ct.SchemeID = a.SchemeID
GROUP BY a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, a.LotteryCode, 
                a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, a.BuyType, a.FromClient, a.IsSendOut, 
                a.FollowSchemeID, a.FollowSchemeBonus, a.FollowSchemeBonusScale, b.UserName, ct.BetType, ct.IsuseCount, 
                f.LotteryName, sd.Multiple, sd.BetNum, sd.BetNumber, pt.ModuleName, pt.PlayName, ct.StartTime, ct.EndTime, 
                ct.StopTypeWhenWinMoney, ct.QuashStatus

GO
/****** Object:  View [dbo].[udv_SchemeChaseTaskDetail]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_SchemeChaseTaskDetail]
AS
SELECT   s.SchemeID, i.IsuseName, i.StartTime, i.EndTime, ctd.Amount, se.WinMoney, se.TicketStatus
FROM      dbo.CT_Schemes AS s INNER JOIN
                dbo.CT_ChaseTaskDetails AS ctd ON ctd.SchemeID = s.SchemeID LEFT OUTER JOIN
                dbo.CT_Isuses AS i ON i.IsuseID = ctd.IsuseID LEFT OUTER JOIN
                dbo.CT_SchemeETickets AS se ON se.SchemeID = s.SchemeID

GO
/****** Object:  View [dbo].[udv_Schemes]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* Ͷע��ѯ*/
CREATE VIEW [dbo].[udv_Schemes]
AS
WITH Tab AS (SELECT   a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, 
                                       a.LotteryCode, a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, 
                                       c.IsOpened AS SchemeIsOpened, a.BuyType, a.FromClient, a.IsSendOut, a.FollowSchemeID, 
                                       a.FollowSchemeBonus, a.FollowSchemeBonusScale, b.UserName, c.IsuseName, c.StartTime, c.EndTime, 
                                       c.IsExecuteChase, c.IsOpened AS IsOpen, c.OpenNumber, c.IsuseState, 
                                       dbo.F_GetSchemesPlayList(a.SchemeID) AS PlayName, f.LotteryName, SUM(e.WinMoney) AS WinMoney, 
                                       SUM(e.WinMoneyNoWithTax) AS WinMoneyNoWithTax
                       FROM      dbo.CT_Schemes AS a INNER JOIN
                                       dbo.CT_Users AS b ON b.UserID = a.InitiateUserID AND a.BuyType <> 1 LEFT OUTER JOIN
                                       dbo.CT_Isuses AS c ON c.IsuseID = a.IsuseID LEFT OUTER JOIN
                                       dbo.CT_Lotteries AS f ON f.LotteryCode = a.LotteryCode LEFT OUTER JOIN
                                       dbo.CT_SchemesDetail AS e ON e.SchemeID = a.SchemeID
                       GROUP BY a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, 
                                       a.LotteryCode, a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, c.IsOpened, 
                                       a.BuyType, a.FromClient, a.IsSendOut, a.FollowSchemeID, a.FollowSchemeBonus, 
                                       a.FollowSchemeBonusScale, b.UserName, c.IsuseName, c.StartTime, c.EndTime, c.IsExecuteChase, 
                                       c.IsOpened, c.OpenNumber, c.IsuseState, dbo.F_GetSchemesPlayList(a.SchemeID), f.LotteryName
                       UNION
                       SELECT DISTINCT 
                                       a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, 
                                       a.LotteryCode, a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, 
                                       c.IsOpened AS SchemeIsOpened, a.BuyType, a.FromClient, a.IsSendOut, a.FollowSchemeID, 
                                       a.FollowSchemeBonus, a.FollowSchemeBonusScale, b.UserName, '-' AS Expr1, '' AS Expr2, '' AS Expr3, 
                                       c.IsExecuteChase, c.IsOpened AS IsOpen, c.OpenNumber, c.IsuseState, 
                                       dbo.F_GetSchemesPlayList(a.SchemeID) AS PlayName, f.LotteryName, ISNULL(SUM(e.WinMoney), 0) 
                                       AS WinMoney, ISNULL(SUM(e.WinMoney), 0) AS WinMoneyNoWithTax
                       FROM      dbo.CT_Schemes AS a INNER JOIN
                                       dbo.CT_Users AS b ON b.UserID = a.InitiateUserID AND a.BuyType = 1 LEFT OUTER JOIN
                                       dbo.CT_ChaseTaskDetails AS cd ON cd.SchemeID = a.SchemeID LEFT OUTER JOIN
                                       dbo.CT_Isuses AS c ON c.IsuseID = cd.IsuseID LEFT OUTER JOIN
                                       dbo.CT_Lotteries AS f ON f.LotteryCode = a.LotteryCode LEFT OUTER JOIN
                                       dbo.CT_SchemeETickets AS e ON e.SchemeID = a.SchemeID AND e.ChaseTaskDetailsID = cd.ID
                       GROUP BY a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, 
                                       a.LotteryCode, a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, c.IsOpened, 
                                       a.BuyType, a.FromClient, a.IsSendOut, a.FollowSchemeID, a.FollowSchemeBonus, 
                                       a.FollowSchemeBonusScale, b.UserName, c.IsuseName, c.StartTime, c.EndTime, c.IsExecuteChase, 
                                       c.IsOpened, c.OpenNumber, c.IsuseState, dbo.F_GetSchemesPlayList(a.SchemeID), f.LotteryName)
    SELECT   SchemeID, CreateTime, SchemeNumber, Title, Describe, InitiateUserID, IsuseID, LotteryCode, LotteryNumber, 
                    SchemeMoney, SecrecyLevel, SchemeStatus, SchemeIsOpened, BuyType, FromClient, IsSendOut, FollowSchemeID, 
                    FollowSchemeBonus, FollowSchemeBonusScale, UserName, IsuseName, StartTime, EndTime, IsExecuteChase, 
                    IsOpen, OpenNumber, IsuseState, PlayName, LotteryName, WinMoney, WinMoneyNoWithTax
    FROM      Tab AS Tab_1

GO
/****** Object:  View [dbo].[udv_SchemesDetail]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_SchemesDetail]
AS
SELECT   a.SDID, a.SchemeID, a.PlayCode, a.Multiple, a.BetNum, a.BetNumber, a.IsNorm, a.IsWin, b.PlayName, b.LotteryCode
FROM      dbo.CT_SchemesDetail AS a LEFT OUTER JOIN
                dbo.CT_PlayTypes AS b ON b.PlayCode = a.PlayCode

GO
/****** Object:  View [dbo].[udv_SchemesMain]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Ͷע��ѯ*/
CREATE VIEW [dbo].[udv_SchemesMain]
AS
WITH Tab AS (SELECT   a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, 
                                       a.LotteryCode, a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, 
                                       c.IsOpened AS SchemeIsOpened, a.BuyType, a.FromClient, a.IsSendOut, a.FollowSchemeID, 
                                       a.FollowSchemeBonus, a.FollowSchemeBonusScale, b.UserName, c.IsuseName, c.StartTime, c.EndTime, 
                                       c.IsExecuteChase, c.IsOpened AS IsOpen, c.IsuseState, 
                                       dbo.F_GetSchemesPlayList(a.SchemeID) AS PlayName, f.LotteryName, SUM(e.WinMoney) AS WinMoney, 
                                       SUM(e.WinMoneyNoWithTax) AS WinMoneyNoWithTax
                       FROM      dbo.CT_Schemes AS a INNER JOIN
                                       dbo.CT_Users AS b ON b.UserID = a.InitiateUserID AND a.BuyType <> 1 LEFT OUTER JOIN
                                       dbo.CT_Isuses AS c ON c.IsuseID = a.IsuseID LEFT OUTER JOIN
                                       dbo.CT_Lotteries AS f ON f.LotteryCode = a.LotteryCode LEFT OUTER JOIN
                                       dbo.CT_SchemesDetail AS e ON e.SchemeID = a.SchemeID
                       GROUP BY a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, 
                                       a.LotteryCode, a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, c.IsOpened, 
                                       a.BuyType, a.FromClient, a.IsSendOut, a.FollowSchemeID, a.FollowSchemeBonus, 
                                       a.FollowSchemeBonusScale, b.UserName, c.IsuseName, c.StartTime, c.EndTime, c.IsExecuteChase, 
                                       c.IsOpened, c.IsuseState, dbo.F_GetSchemesPlayList(a.SchemeID), f.LotteryName
                       UNION
                       SELECT DISTINCT 
                                       a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, 
                                       a.LotteryCode, a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, 
                                       c.IsOpened AS SchemeIsOpened, a.BuyType, a.FromClient, a.IsSendOut, a.FollowSchemeID, 
                                       a.FollowSchemeBonus, a.FollowSchemeBonusScale, b.UserName, '-' AS Expr1, '' AS Expr2, '' AS Expr3, 
                                       c.IsExecuteChase, c.IsOpened AS IsOpen, c.IsuseState, 
                                       dbo.F_GetSchemesPlayList(a.SchemeID) AS PlayName, f.LotteryName, ISNULL(SUM(e.WinMoney), 0) 
                                       AS WinMoney, ISNULL(SUM(e.WinMoney), 0) AS WinMoneyNoWithTax
                       FROM      dbo.CT_Schemes AS a INNER JOIN
                                       dbo.CT_Users AS b ON b.UserID = a.InitiateUserID AND a.BuyType = 1 LEFT OUTER JOIN
                                       dbo.CT_ChaseTaskDetails AS cd ON cd.SchemeID = a.SchemeID LEFT OUTER JOIN
                                       dbo.CT_Isuses AS c ON c.IsuseID = cd.IsuseID LEFT OUTER JOIN
                                       dbo.CT_Lotteries AS f ON f.LotteryCode = a.LotteryCode LEFT OUTER JOIN
                                       dbo.CT_SchemeETickets AS e ON e.SchemeID = a.SchemeID AND e.ChaseTaskDetailsID = cd.ID
                       GROUP BY a.SchemeID, a.CreateTime, a.SchemeNumber, a.Title, a.Describe, a.InitiateUserID, a.IsuseID, 
                                       a.LotteryCode, a.LotteryNumber, a.SchemeMoney, a.SecrecyLevel, a.SchemeStatus, c.IsOpened, 
                                       a.BuyType, a.FromClient, a.IsSendOut, a.FollowSchemeID, a.FollowSchemeBonus, 
                                       a.FollowSchemeBonusScale, b.UserName, c.IsuseName, c.StartTime, c.EndTime, c.IsExecuteChase, 
                                       c.IsOpened, c.IsuseState, dbo.F_GetSchemesPlayList(a.SchemeID), f.LotteryName)
    SELECT   SchemeID, CreateTime, SchemeNumber, Title, Describe, InitiateUserID, IsuseID, LotteryCode, LotteryNumber, 
                    SchemeMoney, SecrecyLevel, SchemeStatus, SchemeIsOpened, BuyType, FromClient, IsSendOut, FollowSchemeID, 
                    FollowSchemeBonus, FollowSchemeBonusScale, UserName, IsuseName, StartTime, EndTime, IsExecuteChase, 
                    IsOpen, IsuseState, PlayName, LotteryName, WinMoney, WinMoneyNoWithTax
    FROM      Tab AS Tab_1


GO
/****** Object:  View [dbo].[udv_TradeDetailReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_TradeDetailReport]
AS
SELECT u.UserID,u.UserName,u.UserMobile,ue.IsVerify,u.IsRobot,u.IsCanLogin,
	  c.TradeAmount,c.Balance,c.TradeRemark,c.CreateTime,c.ID AS ChangeID,c.TradeType
FROM dbo.CT_Users AS u
LEFT JOIN dbo.CT_UsersExtend AS ue  ON ue.UserID=u.UserID
INNER JOIN dbo.CT_UsersRecord AS c ON c.UserID=u.UserID

GO
/****** Object:  View [dbo].[udv_UserAccountReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_UserAccountReport]
AS
SELECT   UserID, UserName, UserMobile, IsRobot, Balance, CreateTime, IsCanLogin, IsVerify, SUM(recharge) AS Recharge, 
                SUM(buyCP) AS Buycp, SUM(winning) AS Winning, SUM(Withdraw) AS Withdraw
FROM      (SELECT   u.UserID, u.UserName, u.UserMobile, u.IsRobot, u.IsCanLogin, ue.IsVerify, u.Balance, ue.CreateTime, 
                                 (CASE ur.TradeType WHEN 0 THEN SUM(ur.TradeAmount) END) AS recharge, 
                                 (CASE ur.TradeType WHEN 1 THEN SUM(ur.TradeAmount) END) AS buyCP, 
                                 (CASE ur.TradeType WHEN 5 THEN SUM(ur.TradeAmount) END) AS winning, 
                                 (CASE ur.TradeType WHEN 4 THEN SUM(ur.TradeAmount) END) AS Withdraw
                 FROM      dbo.CT_Users AS u INNER JOIN
                                 dbo.CT_UsersExtend AS ue ON ue.UserID = u.UserID LEFT OUTER JOIN
                                 dbo.CT_UsersRecord AS ur ON ur.UserID = u.UserID
                 GROUP BY u.UserID, u.UserName, u.UserMobile, u.IsRobot, u.Balance, ue.CreateTime, u.IsCanLogin, ue.IsVerify, 
                                 ur.TradeType) AS tab
GROUP BY UserID, UserName, UserMobile, IsRobot, Balance, CreateTime, IsCanLogin, IsVerify

GO
/****** Object:  View [dbo].[udv_UserInfo]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[udv_UserInfo]
AS
SELECT   a.UserID, a.UserName, a.UserMobile, a.UserPassword, a.PayPassword, a.Balance, a.GoldBean, a.Freeze, 
                --a.ReCommenderID, 
                a.IsRobot, a.IsCanLogin, 
                --a.AccID, 
                --a.Token, 
                b.BindType, b.CreateTime, b.BindTime, b.RelationID, 
                b.IsVerify, b.IsBindTel, b.Idols, 
                --a.Fans, 
                b.IDNumber, b.FullName, b.AvatarAddress, b.NickName
FROM      dbo.CT_Users AS a LEFT OUTER JOIN
                dbo.CT_UsersExtend AS b ON a.UserID = b.UserID


GO
/****** Object:  View [dbo].[udv_UserPay]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_UserPay]
AS
	SELECT a.*, b.UserName, b.UserMobile FROM dbo.CT_UsersPayDetail a
		LEFT JOIN dbo.CT_Users b ON b.UserID=a.UserID


GO
/****** Object:  View [dbo].[udv_UserPayReRefund]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_UserPayReRefund]
AS
	SELECT a.*, b.OrderNo, b.UserID, b.Amount AS PayAmount, b.RechargeNo AS PayRechargeNo, c.UserName, c.UserMobile FROM  dbo.CT_UsersPayRefund a 
		LEFT JOIN dbo.CT_UsersPayDetail b ON b.PayID = a.PayID
		LEFT JOIN dbo.CT_Users c ON c.UserID = b.UserID


GO
/****** Object:  View [dbo].[udv_UsersWithdraw]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[udv_UsersWithdraw]
AS
--udv_PayOut
SELECT   a.PayOutID, a.PayOutStatus, a.UserID, a.Amount, a.CreateTime, u.UserName, ue.FullName, ue.IDNumber, cb.BankType, 
                cb.BankName, cb.CardNumber,a.Remark
FROM      dbo.CT_UsersWithdraw AS a INNER JOIN
                dbo.CT_Users AS u ON a.UserID = u.UserID LEFT OUTER JOIN
                dbo.CT_UsersExtend AS ue ON a.UserID = ue.UserID LEFT OUTER JOIN
                dbo.CT_UsersBanks AS cb ON cb.BankID = a.BankID AND cb.UserID = a.UserID


GO
/****** Object:  View [dbo].[udv_WinningDetailReport]    Script Date: 2018/1/18 14:14:11 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--CREATE VIEW [dbo].[udv_WinningDetailReport]
--AS
--SELECT u.UserID,u.UserName,u.UserMobile,u.IsVerify,u.IsRobot,u.IsCanLogin,
--	  c.TradeAmount,c.Balance,c.TradeRemark,c.CreateTime,c.ID AS ChangeID
--FROM dbo.CT_Users AS u
--INNER JOIN dbo.CT_AccountChange AS c ON c.UserID=u.UserID AND c.TradeType=5

--GO
/****** Object:  View [dbo].[udv_WinTypes]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_WinTypes]
AS
SELECT   a.WinID, a.WinCode, a.LotteryCode, a.PlayCode, a.WinName, a.WinNumber, a.IsSumValue, a.SumValue, a.Sort, 
                a.DefaultMoney, a.DefaultMoneyNoWithTax, b.LotteryName
FROM      dbo.CT_WinTypes AS a LEFT OUTER JOIN
                dbo.CT_Lotteries AS b ON b.LotteryCode = a.LotteryCode

GO
/****** Object:  View [dbo].[udv_WithdrawDetailReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[udv_WithdrawDetailReport]
AS
SELECT u.UserID,u.UserName,u.UserMobile,ue.IsVerify,u.IsRobot,u.IsCanLogin,
	   p.PayOutID,p.Amount,p.OperTime,p.CreateTime,p.PayOutStatus,
	   b.BankName,b.CardNumber,b.ReservedPhone,b.Area,b.BankType,b.BankID
FROM dbo.CT_Users AS u
LEFT JOIN dbo.CT_UsersExtend AS ue ON ue.UserID=u.UserID
INNER JOIN dbo.CT_UsersWithdraw AS p ON p.UserID=u.UserID
LEFT JOIN dbo.CT_UsersBanks AS b ON b.UserID=u.UserID AND b.BankID=p.BankID

GO
SET IDENTITY_INSERT [dbo].[CT_Lotteries] ON 

GO
INSERT [dbo].[CT_Lotteries] ([LotteryID], [LotteryCode], [LotteryName], [Shorthand], [Subhead], [MaxChaseIsuse], [Sort], [TypeID], [WinNumberExemple], [IntervalType], [PrintOutType], [Price], [MaxMultiple], [OffTime], [ChaseDeferTime], [QuashChaseTime], [Kinformation], [IsEmphasis], [ModuleVersion], [IsAddaward], [IsEnable], [StopReason], [IsStop], [IsHot], [AdvanceEndTime], [PresellTime], [ChatGroups], [ChatRooms]) VALUES (1, 101, N'���3', N'JLK3', N'2ԪӮ240', N'10', 10, 4, N'111', N'601@��@87@08:20:06-08:30:00@3', 0, 200, 0, 90, 0, 0, N'', 1, 100, 0, 1, N'', 0, 1, 2, 2, N'', N'1')
GO
INSERT [dbo].[CT_Lotteries] ([LotteryID], [LotteryCode], [LotteryName], [Shorthand], [Subhead], [MaxChaseIsuse], [Sort], [TypeID], [WinNumberExemple], [IntervalType], [PrintOutType], [Price], [MaxMultiple], [OffTime], [ChaseDeferTime], [QuashChaseTime], [Kinformation], [IsEmphasis], [ModuleVersion], [IsAddaward], [IsEnable], [StopReason], [IsStop], [IsHot], [AdvanceEndTime], [PresellTime], [ChatGroups], [ChatRooms]) VALUES (2, 102, N'Ӯ��3', N'JXK3', N'2ԪӮ240', N'10', 10, 4, N'111', N'589@��@84@08:55:00-09:04:49@3', 0, 200, 0, 3000, 0, 0, N'', 0, 100, 0, 0, N'', 1, 0, 3, 3, N'', N'7')
GO
INSERT [dbo].[CT_Lotteries] ([LotteryID], [LotteryCode], [LotteryName], [Shorthand], [Subhead], [MaxChaseIsuse], [Sort], [TypeID], [WinNumberExemple], [IntervalType], [PrintOutType], [Price], [MaxMultiple], [OffTime], [ChaseDeferTime], [QuashChaseTime], [Kinformation], [IsEmphasis], [ModuleVersion], [IsAddaward], [IsEnable], [StopReason], [IsStop], [IsHot], [AdvanceEndTime], [PresellTime], [ChatGroups], [ChatRooms]) VALUES (3, 801, N'˫ɫ��', N'SSQ', N'2Ԫ�������', N'10', 8, 1, N'2,5,6,7,8,9+5', N'2,4,7@��@152-154@20:30:00-19:30:00@3', 1, 200, 0, 0, 0, 0, N'', 1, 100, 0, 1, N'', 0, 1, 20, 20, N'', N'6')
GO
INSERT [dbo].[CT_Lotteries] ([LotteryID], [LotteryCode], [LotteryName], [Shorthand], [Subhead], [MaxChaseIsuse], [Sort], [TypeID], [WinNumberExemple], [IntervalType], [PrintOutType], [Price], [MaxMultiple], [OffTime], [ChaseDeferTime], [QuashChaseTime], [Kinformation], [IsEmphasis], [ModuleVersion], [IsAddaward], [IsEnable], [StopReason], [IsStop], [IsHot], [AdvanceEndTime], [PresellTime], [ChatGroups], [ChatRooms]) VALUES (4, 901, N'��������͸', N'DLT', N'�׳�һƬ����', N'10', 9, 2, N'3,6,9,10,11+2,4', N'1,3,6@��@152-154@20:30:00-19:30:00@3', 1, 200, 0, 0, 0, 0, N'', 1, 100, 0, 1, N'', 0, 1, 20, 20, N'', N'5')
GO
INSERT [dbo].[CT_Lotteries] ([LotteryID], [LotteryCode], [LotteryName], [Shorthand], [Subhead], [MaxChaseIsuse], [Sort], [TypeID], [WinNumberExemple], [IntervalType], [PrintOutType], [Price], [MaxMultiple], [OffTime], [ChaseDeferTime], [QuashChaseTime], [Kinformation], [IsEmphasis], [ModuleVersion], [IsAddaward], [IsEnable], [StopReason], [IsStop], [IsHot], [AdvanceEndTime], [PresellTime], [ChatGroups], [ChatRooms]) VALUES (5, 201, N'��11ѡ5', N'HB11X5', N'2ԪӮ1170', N'10', 1, 4, N'01,02,03,04,05', N'600@��@81@08:25:00-08:35:00@2', 0, 200, 50, 2000, 0, 0, N'', 0, 100, 0, 0, N'', 1, 0, 2, 2, N'', N'4')
GO
INSERT [dbo].[CT_Lotteries] ([LotteryID], [LotteryCode], [LotteryName], [Shorthand], [Subhead], [MaxChaseIsuse], [Sort], [TypeID], [WinNumberExemple], [IntervalType], [PrintOutType], [Price], [MaxMultiple], [OffTime], [ChaseDeferTime], [QuashChaseTime], [Kinformation], [IsEmphasis], [ModuleVersion], [IsAddaward], [IsEnable], [StopReason], [IsStop], [IsHot], [AdvanceEndTime], [PresellTime], [ChatGroups], [ChatRooms]) VALUES (6, 202, N'��11ѡ5', N'SD11X5', N'2ԪӮ1170', N'10', 2, 4, N'01,02,03,04,05', N'600@��@87@08:25:20-08:35:20@2@22:56:00', 0, 200, 50, 2000, 0, 0, N'', 0, 100, 0, 1, N'', 0, 0, 2, 2, N'', N'3')
GO
INSERT [dbo].[CT_Lotteries] ([LotteryID], [LotteryCode], [LotteryName], [Shorthand], [Subhead], [MaxChaseIsuse], [Sort], [TypeID], [WinNumberExemple], [IntervalType], [PrintOutType], [Price], [MaxMultiple], [OffTime], [ChaseDeferTime], [QuashChaseTime], [Kinformation], [IsEmphasis], [ModuleVersion], [IsAddaward], [IsEnable], [StopReason], [IsStop], [IsHot], [AdvanceEndTime], [PresellTime], [ChatGroups], [ChatRooms]) VALUES (7, 301, N'��ʱʱ��', N'CQSSC', N'ʮ��󽱵�������', N'10', 3, 4, N'0,1,2,3,4,5', N'5@��@120@00:00:00@3@23-5|24-485|96-10|120-5', 0, 200, 50, 2000, 0, 0, N'', 0, 100, 0, 1, N'', 0, 0, 3, 3, N'', N'2')
GO
SET IDENTITY_INSERT [dbo].[CT_Lotteries] OFF
GO
SET IDENTITY_INSERT [dbo].[CT_NewsTypes] ON 

GO
INSERT [dbo].[CT_NewsTypes] ([TypeID], [ParentID], [TypeName], [IsShow], [IsSystem], [Sort], [Remarks]) VALUES (10002, 0, N'����', 1, 1, 1, N'������Ѷ��Ŀ')
GO
INSERT [dbo].[CT_NewsTypes] ([TypeID], [ParentID], [TypeName], [IsShow], [IsSystem], [Sort], [Remarks]) VALUES (10003, 0, N'����', 1, 1, 2, N'������Ѷ��Ŀ')
GO
INSERT [dbo].[CT_NewsTypes] ([TypeID], [ParentID], [TypeName], [IsShow], [IsSystem], [Sort], [Remarks]) VALUES (10004, 0, N'����', 1, 1, 3, N'������Ѷ��Ŀ')
GO
SET IDENTITY_INSERT [dbo].[CT_NewsTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[CT_PlayTypes] ON 

GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (1, 10101, 101, N'��ֵ', 2, N'4,5,15', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (2, 10102, 101, N'��ͬ��ͨѡ', 2, N'������ͬ�ţ��磺111,222,333,444,555,666', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (3, 10103, 101, N'��ͬ�ŵ�ѡ', 2, N'������ͬ�ţ��磺111,222,333,444,555,666', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (4, 10104, 101, N'����ͬ��', 2, N'��������ͬ�ţ��磺123��124��125��126��134��135', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (5, 10105, 101, N'������ͨѡ', 2, N'���������ţ��磺123,234,345,456', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (6, 10106, 101, N'��ͬ�Ÿ�ѡ', 2, N'������ͬ�ţ��磺11*��22*��33*��44*��55*��66*', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (7, 10107, 101, N'��ͬ�ŵ�ѡ', 2, N'������ͬ�ţ��磺112��113��114��115��116��122', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (8, 10108, 101, N'����ͬ��', 2, N'������ͬ�ţ��磺12*��13*��14*��15*��16*', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (9, 10201, 102, N'��ֵ', 2, N'4,5,15', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (10, 10202, 102, N'��ͬ��ͨѡ', 2, N'������ͬ�ţ��磺111,222,333,444,555,666', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (11, 10203, 102, N'��ͬ�ŵ�ѡ', 2, N'������ͬ�ţ��磺111,222,333,444,555,666', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (12, 10204, 102, N'����ͬ��', 2, N'��������ͬ�ţ��磺123��124��125��126��134��135', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (13, 10205, 102, N'������ͨѡ', 2, N'���������ţ��磺123,234,345,456', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (14, 10206, 102, N'��ͬ�Ÿ�ѡ', 2, N'������ͬ�ţ��磺11*��22*��33*��44*��55*��66*', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (15, 10207, 102, N'��ͬ�ŵ�ѡ', 2, N'������ͬ�ţ��磺112��113��114��115��116��122', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (16, 10208, 102, N'����ͬ��', 2, N'������ͬ�ţ��磺12*��13*��14*��15*��16*', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (17, 20101, 201, N'��ѡ��', 2, N'������:01 02 03 04 05 Ͷע����:01 05 �н�����:ѡ2������,���п�����������2������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (18, 20102, 201, N'��ѡ��', 2, N'������:01 02 03 04 05 Ͷע����:01 05 04 �н�����:ѡ3������,���п�����������3������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (19, 20103, 201, N'��ѡ��', 2, N'������:01 02 03 04 05 Ͷע����:01 02 04 05 �н�����:ѡ4������,���п�����������4������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (20, 20104, 201, N'��ѡ��', 2, N'������:01 02 03 04 05 Ͷע����:01 02 03 04 05 �н�����:ѡ5������,���п�����������5������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (21, 20105, 201, N'��ѡ��', 2, N'������:01 02 03 04 05 Ͷע����:01 02 03 04 05 06 �н�����:ѡ6������,���п�����������5������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (22, 20106, 201, N'��ѡ��', 2, N'������:01 02 03 04 05 Ͷע����:01 02 03 04 05 06 07 �н�����:ѡ7������,���п�����������5������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (23, 20107, 201, N'��ѡ��', 2, N'������:01 02 03 04 05 Ͷע����:01 02 03 04 05 06 07 08 �н�����:ѡ8������,���п�����������5������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (24, 20108, 201, N'ǰһֱѡ', 200, N'������:01 02 03 04 05 Ͷע����:01 �н�����:ѡ1�����룬���п��������1������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (25, 20109, 201, N'ǰ��ֱѡ', 200, N'������:01 02 03 04 05 Ͷע����:01 02 �н�����:ѡ2�������뿪����ǰ2��������ͬ��˳��һ��', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (26, 20110, 201, N'ǰ����ѡ', 200, N'������:01 02 03 04 05 Ͷע����:02 01 �н�����:ѡ2�������뿪����ǰ2��������ͬ', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (27, 20111, 201, N'ǰ��ֱѡ', 200, N'������:01 02 03 04 05 Ͷע����:01 02 03 �н�����:ѡ3�������뿪����ǰ3��������ͬ��˳��һ��', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (28, 20112, 201, N'ǰ����ѡ', 200, N'������:01 02 03 04 05 Ͷע����:02 01 03 �н�����:ѡ3�������뿪����ǰ3��������ͬ', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (29, 20201, 202, N'��ѡ��', 200, N'������:01 02 03 04 05 Ͷע����:01 05 �н�����:ѡ2������,���п�����������2������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (30, 20202, 202, N'��ѡ��', 200, N'������:01 02 03 04 05 Ͷע����:01 05 04 �н�����:ѡ3������,���п�����������3������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (31, 20203, 202, N'��ѡ��', 200, N'������:01 02 03 04 05 Ͷע����:01 02 04 05 �н�����:ѡ4������,���п�����������4������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (32, 20204, 202, N'��ѡ��', 200, N'������:01 02 03 04 05 Ͷע����:01 02 03 04 05 �н�����:ѡ5������,���п�����������5������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (33, 20205, 202, N'��ѡ��', 200, N'������:01 02 03 04 05 Ͷע����:01 02 03 04 05 06 �н�����:ѡ6������,���п�����������5������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (34, 20206, 202, N'��ѡ��', 200, N'������:01 02 03 04 05 Ͷע����:01 02 03 04 05 06 07 �н�����:ѡ7������,���п�����������5������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (35, 20207, 202, N'��ѡ��', 200, N'������:01 02 03 04 05 Ͷע����:01 02 03 04 05 06 07 08 �н�����:ѡ8������,���п�����������5������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (36, 20208, 202, N'ǰһֱѡ', 200, N'������:01 02 03 04 05 Ͷע����:01 �н�����:ѡ1�����룬���п��������1������', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (37, 20209, 202, N'ǰ��ֱѡ', 200, N'������:01 02 03 04 05 Ͷע����:01 02 �н�����:ѡ2�������뿪����ǰ2��������ͬ��˳��һ��', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (38, 20210, 202, N'ǰ����ѡ', 200, N'������:01 02 03 04 05 Ͷע����:02 01 �н�����:ѡ2�������뿪����ǰ2��������ͬ', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (39, 20211, 202, N'ǰ��ֱѡ', 200, N'������:01 02 03 04 05 Ͷע����:01 02 03 �н�����:ѡ3�������뿪����ǰ3��������ͬ��˳��һ��', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (40, 20212, 202, N'ǰ����ѡ', 200, N'������:01 02 03 04 05 Ͷע����:02 01 03 �н�����:ѡ3�������뿪����ǰ3��������ͬ', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (41, 80101, 801, N'��׼�淨', 200, N'', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (42, 90101, 901, N'��׼�淨', 200, N'������:01 02 03 04 05 06 07', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (43, 90102, 901, N'׷���淨', 200, N'������:01 02 03 04 05 06 07', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (48, 30101, 301, N'����ֱѡ', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (49, 30102, 301, N'����ͨѡ', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (50, 30103, 301, N'����ֱѡ', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (51, 30104, 301, N'��������', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (52, 30105, 301, N'��������', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (53, 30106, 301, N'����ֱѡ', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (54, 30107, 301, N'����ֱѡ��ֵ', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (55, 30108, 301, N'������ѡ', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (56, 30109, 301, N'������ѡ��ֵ', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (57, 30110, 301, N'һ��', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (58, 30111, 301, N'��С��˫', 200, N'������:6 2 1 0 9', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (59, 20213, 202, N'��ѡ3', 600, N'ɽ��11�˶����������淨����λ�¶�ǰ3λ����1384Ԫ������λ�¶�ǰ3����214Ԫ������λ�¶�����3λ����19Ԫ��', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (60, 20214, 202, N'��ѡ4', 1000, N'ɽ��11�˶����������淨���¶�����3����19Ԫ���¶�4����154Ԫ', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (61, 20215, 202, N'��ѡ5', 1400, N'ɽ��11�˶����������淨���¶�����4����90Ԫ���¶�5����1080', 50, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (62, 4001001, 40010, N'��С��˫', 100, N'', 0, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (63, 4001002, 40010, N'��С��˫_�ر���', 100, N'', 0, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (64, 4001003, 40010, N'������', 100, N'', 0, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (65, 4001004, 40010, N'Ȥζ�淨', 100, N'', 0, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (66, 4002001, 40020, N'��С��˫', 100, N'', 0, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (67, 4002002, 40020, N'������', 100, N'', 0, 1)
GO
INSERT [dbo].[CT_PlayTypes] ([PlayID], [PlayCode], [LotteryCode], [PlayName], [Price], [ModuleName], [MaxMultiple], [Sort]) VALUES (68, 4002003, 40020, N'Ȥζ�淨', 100, N'', 0, 1)
GO
SET IDENTITY_INSERT [dbo].[CT_PlayTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[CT_SystemSetInfo] ON 

GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (1, N'SiteName', N'������', N'վ������', N'վ������', 1, 0, 0, N'')
GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (2, N'SiteOn', N'1', N'վ��״̬', N'վ��״̬', 2, 0, 3, N'0.��,1.��')
GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (3, N'InitiateSchemeBonusScale', N'10', N'Ӷ�����', N'���𷽰�(������ӯ��)Ӷ�����(%)', 3, 0, 1, N'')
GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (4, N'QuashSchemeMaxNum', N'10', N'��Աÿ�ڳ�������', N'ÿ����Աÿ�����ֻ�ܳ�������', 4, 0, 1, N'')
GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (5, N'RedPacketsOfSelfBuy', N'20', N'��Ա���ʺ��', N'��Ա�Լ����ʺ��(��Աע��ʱȡ��ֵ��ΪĬ��ֵ)', 5, 0, 1, N'')
GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (6, N'RedPacketsOfRecommendBuy', N'15', N'�Ƽ���Ա���ʺ��/Ԫ', N'�Ƽ��Ļ�Ա���ʺ��/Ԫ(��Աע��ʱȡ��ֵ��ΪĬ��ֵ)', 6, 0, 1, N'')
GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (7, N'AccumulativeWinMoney', N'500000000', N'�ۼ��н����', N'ϵͳ�ۼ��н����', 7, 0, 1, N'')
GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (8, N'IsUpdateAPP', N'1', N'����Ӧ�ó���', N'�Ƿ����Ӧ�ó���', 8, 0, 3, N'0.��,1.��')
GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (9, N'AppUpdateAddress', N' ', N'Ӧ�ó����������', N'Ӧ�ó����������', 9, 0, 1, N' ')
GO
INSERT [dbo].[CT_SystemSetInfo] ([SetID], [SetKey], [SetValue], [SetName], [SetDetail], [Sort], [IsUse], [DataType], [DataValue]) VALUES (10, N'APPBottomSwitch', N'1,2,3,4,5', N'Ӧ�ó���ײ�����', N'Ӧ�ó���ײ�����', 10, 0, 2, N'1.����,2.��̨,3.����԰,4.Ȧ��,5.��')
GO
SET IDENTITY_INSERT [dbo].[CT_SystemSetInfo] OFF
GO
SET IDENTITY_INSERT [dbo].[CT_TemplateConfig] ON 

GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (1, N'��Ա��֤�����', N'������֤��Ϊ��{code}��{valid}��������Ч��', 2, CAST(N'2017-01-12 20:37:39.660' AS DateTime), 1)
GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (2, N'��ӭ���û�����', N'��ӭ��Ϊ{webname}��Ա������û���Ϊ��{username}��ע�Ᵽ�ܺ������˻���Ϣ��', 3, CAST(N'2017-01-12 20:39:00.790' AS DateTime), 1)
GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (3, N'����ȷ��֪ͨ', N'�𾴵�{username}������{webname}�Ķ�����ȷ�ϣ�������Ϊ��{orderno}������{amount}Ԫ�����ǻᾡ�찲�ŷ�����', 2, CAST(N'2017-01-13 15:25:11.553' AS DateTime), 1)
GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (5, N'�н�����', N'�װ���{0}��Ա����ϲ���ڲ��ֹ����{1}���������Ϊ{2}Ԫ', 4, CAST(N'2017-07-04 14:23:00.000' AS DateTime), 1)
GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (6, N'��������', N'�װ���{0}��Ա���㹺���{1}��{2}�ڶ���[���ţ�{3}]�������������{4}�ѷ��������˻���', 4, CAST(N'2017-07-31 14:47:19.523' AS DateTime), 1)
GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (7, N'�н�����', N'<color=#ffffff>��ϲ��</color><color=#fffea9>{0}</color><color=#ffffff>Ԫ{1}�ʽ�</color><color=#00F7E8>{2}</color><color=#ffffff>��������</color>', 1, CAST(N'2017-09-21 15:27:01.000' AS DateTime), 1)
GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (8, N'�����������', N'�װ���{0}��Ա������{1}Ԫ����������ڣ��뼰ʱʹ�á�', 4, CAST(N'2017-10-16 14:18:41.000' AS DateTime), 1)
GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (9, N'Open', N'<color=#ffffff>[{0}]</color><color=#0efff1>�� {1} ��</color><br/><color=#ffffff>���ڿ���Ͷע��</color><color=#fffea9>{2}</color><br/><color=#fffff>����Ͷע��һ���󽱾��������</color>', 5, CAST(N'2017-11-23 10:47:33.000' AS DateTime), 1)
GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (10, N'Prep', N'<color=#ffffff>[{0}]</color><color=#0efff1>�� {1} ��</color><color=#ffffff>����ڻ���</color><color=#fffea9>{2}����</color><br/><color=#ffffff>��ץ��ʱ��ͶעŶ</color>', 5, CAST(N'2017-11-23 10:48:00.000' AS DateTime), 1)
GO
INSERT [dbo].[CT_TemplateConfig] ([ID], [Title], [TemplateContent], [TemplateType], [CreateTime], [AdminID]) VALUES (11, N'End', N'<color=#ffffff>[{0}]</color><color=#0efff1>�� {1} ��</color><br/><color=#ffffff>�ѽ�����ע�����ϵͳʵ�ʳ�Ʊ���Ϊ׼</color><br/><color=#ffffff>������������ѯ�ͷ�</color>', 5, CAST(N'2017-11-23 10:48:32.000' AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[CT_TemplateConfig] OFF
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20162, N'�����ֵ�������', 0, 0, N'', N'', N'http://182.92.104.230:2010/Images/WeChat/o_3zdwG0D8294A2KfDYsSHt_U3QY_Icon.jpg', N'o_3zdwG0D8294A2KfDYsSHt_U3QY', N'4_uy9CLxKrlKOKi2Yvp9Se-_tCfNo2Uxx-MVKzJ8hYy1VgI20zYw1GyxvtVFjKLfFbNd54-BH0E5Mn_Rs_uzSAbQ', N'', N'', N'', N'', N'', 2, CAST(N'2017-12-04 09:14:50.647' AS DateTime), CAST(N'2017-12-04 09:14:50.647' AS DateTime), 0, 1, N'', N'', 0, 0, 400)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20163, N'17620171454', 0, 0, N'����', N'330719196804253671', N'', N'', N'', N'', N'', N'', N'', N'', 1, CAST(N'2017-12-04 11:02:21.880' AS DateTime), CAST(N'2017-12-04 11:02:21.880' AS DateTime), 0, 0, N'', N'', 1, 1, 30284200)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20167, N'ܳܳܳܳܳܳܳܳ', 0, 0, N'', N'', N'http://182.92.104.230:2010/Images/WeChat/o_3zdwNIEGO8ofw-osdy7NHMZ2wg_Icon.jpg', N'o_3zdwNIEGO8ofw-osdy7NHMZ2wg', N'', N'', N'', N'', N'', N'', 2, CAST(N'2017-12-04 11:19:10.683' AS DateTime), CAST(N'2017-12-04 11:19:10.683' AS DateTime), 0, 2, N'', N'20162', 0, 0, 8502000)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20168, N'OMG', 0, 0, N'����', N'421182199610111710', N'', N'', N'', N'', N'', N'', N'', N'', 1, CAST(N'2017-12-04 11:47:31.463' AS DateTime), CAST(N'2017-12-04 11:47:31.463' AS DateTime), 0, 0, N'', N'', 1, 1, 1080000)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20169, N'1122', 0, 0, N'', N'', N'http://182.92.104.230:2010/Images/QQ/4DB0819DFB42A719996EBD23E839B8A4_Icon.jpg', N'', N'', N'', N'', N'4DB0819DFB42A719996EBD23E839B8A4', N'4BD9638963E9C4F2FE0155A6A364FDA5', N'', 3, CAST(N'2017-12-04 12:59:28.300' AS DateTime), CAST(N'2017-12-04 12:59:28.300' AS DateTime), 0, 1, N'', N'', 0, 0, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20170, N'�㲻�������Ŀ���', 0, 0, N'', N'', N'http://182.92.104.230:2010/WeChat/o_3zdwLWiXgSocUrGbVCrFj-uHFc_Icon.jpg', N'o_3zdwLWiXgSocUrGbVCrFj-uHFc', N'4_7Kqppr0mUwkbM43324KOTS-AkwziI-jhgA4aCIN5yFMvgMl6OBb1-dftmyJjSpkeZj8ZvhFiWMqiYulR4NxTqiNyT8FaFPXN6owGKb3ePEg', N'', N'', N'', N'', N'', 2, CAST(N'2017-12-04 12:59:54.697' AS DateTime), CAST(N'2017-12-04 12:59:54.697' AS DateTime), 0, 1, N'', N'', 0, 0, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20171, N'������', 0, 0, N'', N'', N'http://182.92.104.230:2010/Images/WeChat/o_3zdwFEq2sRIshlPfYUVu865lno_Icon.jpg', N'o_3zdwFEq2sRIshlPfYUVu865lno', N'4_4BUdXXAfbBESB-DETBW8OcLtJP3OXetLNEhBWjJpxht3rsjEv3ipb68T61Sa5ybsTbEZnrZzqcM_3ojwuE2ueX91JLttADqOC-LMeFoMTes', N'', N'', N'', N'', N'', 2, CAST(N'2017-12-04 13:44:16.220' AS DateTime), CAST(N'2017-12-04 13:44:16.220' AS DateTime), 0, 1, N'', N'', 0, 0, 160000000)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20172, N'ZGF0Ye6AtSDugLU', 0, 0, NULL, NULL, N'aHR0cDovL3d4LnFsb2dvLmNuL21tb3Blbi92aV8zMi9uUFNOSklpY3FhVEFIQ2ljeHVCeEJCNXYyaWJWdDFCZ0RXV2xtakVERldkcGg2OUdyR0kzaWNQRmJJZVdEaWM0cENKaWNTQTN0MFZTUHBxZUNualJlRjN0cVpJUS8w', N'o_3zdwOnP4iWWwlfb3Y0IsmVzZ6c', NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2017-12-05 13:00:34.777' AS DateTime), CAST(N'2017-12-05 13:00:34.777' AS DateTime), 0, 2, NULL, N'20167', 0, 0, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20173, N'SWRlYXI', 0, 0, NULL, NULL, N'aHR0cDovL3d4LnFsb2dvLmNuL21tb3Blbi92aV8zMi9EUnUxUG9BaWNlUlQ4SmpMRWdOa2JsYlZrOHBNTnRhSTY4N0tJSEpJN0JaOWJTOGFsSVozc3d2QlYyMFB6c3NtaE44amtmWnJKQTJHbDNNeG9JY1JpYzdRLzA', N'o_3zdwAoWXvFsc7A8_J1IaVJ4ibk', NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2017-12-05 13:04:20.790' AS DateTime), CAST(N'2017-12-05 13:04:20.790' AS DateTime), 0, 2, NULL, N'20167', 0, 0, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20176, N'13900000000', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2017-12-11 06:26:14.233' AS DateTime), CAST(N'2017-12-11 06:26:14.233' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20177, N'13900000001', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2017-12-11 06:29:30.520' AS DateTime), CAST(N'2017-12-11 06:29:30.520' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20178, N'13900000002', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20179, N'13900000003', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20180, N'13900000004', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20181, N'13900000005', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20182, N'13900000006', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20183, N'13900000007', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20184, N'13900000008', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20185, N'13900000009', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20186, N'13900000010', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20187, N'13900000011', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20188, N'13900000012', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20189, N'13900000013', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20190, N'13900000014', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20191, N'13900000015', 0, 0, NULL, NULL, N'����', NULL, NULL, NULL, NULL, NULL, NULL, N'', 1, CAST(N'2017-12-11 06:32:20.097' AS DateTime), CAST(N'2017-12-11 06:32:20.097' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20192, N'Technology', 0, 0, N'��', N'433125198301035939', N'http://182.92.104.230:2010/WeChat/o_3zdwJ5ZzmllzwrnWb9vfBksKAk_Icon.jpg', N'o_3zdwJ5ZzmllzwrnWb9vfBksKAk', N'4_k6uEXGggaTASNV9x3zq67pkNt9-7syIISUrZB9v5sWGjy9sRxcL3gfFHHjLp-xmUGOt2jY6YIdJsRiMkaoI1yzZw7ZInrLxvtzsoWE7cvaI', N'', N'', N'', N'', N'', 2, CAST(N'2017-12-13 11:08:59.887' AS DateTime), CAST(N'2017-12-13 11:08:59.887' AS DateTime), 0, 1, N'', N'', 1, 0, 248403200)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20193, N'13322222222', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2018-01-10 10:46:16.140' AS DateTime), CAST(N'2018-01-10 10:46:16.140' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20194, N'13168395999', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2018-01-12 13:37:55.683' AS DateTime), CAST(N'2018-01-12 13:37:55.683' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20195, N'13117507995', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2018-01-12 14:11:12.797' AS DateTime), CAST(N'2018-01-12 14:11:12.797' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20196, N'18507300878', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2018-01-13 01:23:58.800' AS DateTime), CAST(N'2018-01-13 01:23:58.800' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20209, N'13660297340', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2018-01-15 06:39:58.357' AS DateTime), CAST(N'2018-01-15 06:39:58.357' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
INSERT [dbo].[CT_UsersExtend] ([UserID], [NickName], [UserLevel], [SpecialLevel], [FullName], [IDNumber], [AvatarAddress], [WechatID], [WechatToken], [AliPayID], [AliPayToken], [QQID], [QQToken], [Email], [BindType], [CreateTime], [BindTime], [Idols], [SourceType], [IpAddress], [RelationID], [IsVerify], [IsBindTel], [WithdrawMoney]) VALUES (20210, N'13430257014', 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2018-01-15 10:45:37.493' AS DateTime), CAST(N'2018-01-15 10:45:37.493' AS DateTime), 0, 0, NULL, NULL, 0, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[CT_WinTypes] ON 

GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (1, 1010104, 101, 10101, N'��ֵ4', NULL, 1, 0, 1, 8000, 8000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (2, 1010105, 101, 10101, N'��ֵ5', NULL, 1, 0, 1, 4000, 4000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (3, 1010106, 101, 10101, N'��ֵ6', NULL, 1, 0, 1, 2500, 2500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (4, 1010107, 101, 10101, N'��ֵ7', NULL, 1, 0, 1, 1600, 1600, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (5, 1010108, 101, 10101, N'��ֵ8', NULL, 1, 0, 1, 1200, 1200, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (6, 1010109, 101, 10101, N'��ֵ9', NULL, 1, 0, 1, 1000, 1000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (7, 1010110, 101, 10101, N'��ֵ10', NULL, 1, 0, 1, 900, 900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (8, 1010111, 101, 10101, N'��ֵ11', NULL, 1, 0, 1, 900, 900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (9, 1010112, 101, 10101, N'��ֵ12', NULL, 1, 0, 1, 1000, 1000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (10, 1010113, 101, 10101, N'��ֵ13', NULL, 1, 0, 1, 1200, 1200, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (11, 1010114, 101, 10101, N'��ֵ14', NULL, 1, 0, 1, 1600, 1600, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (12, 1010115, 101, 10101, N'��ֵ15', NULL, 1, 0, 1, 2500, 2500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (13, 1010116, 101, 10101, N'��ֵ16', NULL, 1, 0, 1, 4000, 4000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (14, 1010117, 101, 10101, N'��ֵ17', NULL, 1, 0, 1, 8000, 8000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (15, 1020104, 102, 10201, N'��ֵ4', NULL, 1, 0, 1, 8000, 8000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (16, 1020105, 102, 10201, N'��ֵ5', NULL, 1, 0, 1, 4000, 4000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (17, 1020106, 102, 10201, N'��ֵ6', NULL, 1, 0, 1, 2500, 2500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (18, 1020107, 102, 10201, N'��ֵ7', NULL, 1, 0, 1, 1600, 1600, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (19, 1020108, 102, 10201, N'��ֵ8', NULL, 1, 0, 1, 1200, 1200, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (20, 1020109, 102, 10201, N'��ֵ9', NULL, 1, 0, 1, 1000, 1000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (21, 1020110, 102, 10201, N'��ֵ10', NULL, 1, 0, 1, 900, 900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (22, 1020111, 102, 10201, N'��ֵ11', NULL, 1, 0, 1, 900, 900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (23, 1020112, 102, 10201, N'��ֵ12', NULL, 1, 0, 1, 1000, 1000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (24, 1020113, 102, 10201, N'��ֵ13', NULL, 1, 0, 1, 1200, 1200, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (25, 1020114, 102, 10201, N'��ֵ14', NULL, 1, 0, 1, 1600, 1600, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (26, 1020115, 102, 10201, N'��ֵ15', NULL, 1, 0, 1, 2500, 2500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (27, 1020116, 102, 10201, N'��ֵ16', NULL, 1, 0, 1, 4000, 4000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (28, 1020117, 102, 10201, N'��ֵ17', NULL, 1, 0, 1, 8000, 8000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (29, 1010201, 101, 10102, N'��ͬ��ͨѡ', N',1-1-1,2-2-2,3-3-3,4-4-4,5-5-5,6-6-6,', NULL, 0, 1, 4000, 4000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (30, 1010301, 101, 10103, N'��ͬ�ŵ�ѡ', N',1-1-1,2-2-2,3-3-3,4-4-4,5-5-5,6-6-6,', NULL, 0, 1, 24000, 24000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (31, 1010401, 101, 10104, N'����ͬ�ŵ�ѡ', N',1-2-3,1-2-4,1-2-5,1-2-6,1-3-4,1-3-5,1-3-6,1-4-5,1-4-6,1-5-6,2-3-4,2-3-5,2-3-6,', NULL, 0, 1, 4000, 4000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (32, 1010501, 101, 10105, N'������ͨѡ', N',1-2-3,2-3-4,3-4-5,4-5-6,', NULL, 0, 1, 1000, 1000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (33, 1010601, 101, 10106, N'��ͬ�Ÿ�ѡ', N',1,2,3,4,5,6,', NULL, 0, 1, 1500, 1500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (34, 1010701, 101, 10107, N'��ͬ�ŵ�ѡ', N',1-1-2,1-1-3,1-1-4,1-1-5,1-1-6,1-2-2,2-2-3,2-2-4,2-2-5,2-2-6,1-3-3,2-3-3,3-3-4,', NULL, 0, 1, 8000, 8000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (35, 1010801, 101, 10108, N'����ͬ�Ÿ�ѡ', N',12*,13*,14*,15*,16*,23*,24*,25*,26*,34*,35*,36*,45*,46*,56*,', NULL, 0, 1, 800, 800, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (36, 1020201, 102, 10202, N'��ͬ��ͨѡ', N',1-1-1,2-2-2,3-3-3,4-4-4,5-5-5,6-6-6,', NULL, 0, 1, 4000, 4000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (37, 1020301, 102, 10203, N'��ͬ�ŵ�ѡ', N',1-1-1,2-2-2,3-3-3,4-4-4,5-5-5,6-6-6,', NULL, 0, 1, 24000, 24000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (38, 1020401, 102, 10204, N'����ͬ�ŵ�ѡ', N',1-2-3,1-2-4,1-2-5,1-2-6,1-3-4,1-3-5,1-3-6,1-4-5,1-4-6,1-5-6,2-3-4,2-3-5,2-3-6,', NULL, 0, 1, 4000, 4000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (39, 1020501, 102, 10205, N'������ͨѡ', N',1-2-3,2-3-4,3-4-5,4-5-6,', NULL, 0, 1, 1000, 1000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (40, 1020601, 102, 10206, N'��ͬ�Ÿ�ѡ', N',1,2,3,4,5,6,', NULL, 0, 1, 1500, 1500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (41, 1020701, 102, 10207, N'��ͬ�ŵ�ѡ', N',1-1-2,1-1-3,1-1-4,1-1-5,1-1-6,1-2-2,2-2-3,2-2-4,2-2-5,2-2-6,1-3-3,2-3-3,3-3-4,', NULL, 0, 1, 8000, 8000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (42, 1020801, 102, 10208, N'����ͬ�Ÿ�ѡ', N',12*,13*,14*,15*,16*,23*,24*,25*,26*,34*,35*,36*,45*,46*,56*,', NULL, 0, 1, 800, 800, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (43, 2010101, 201, 20101, N'��ѡ��', N'', 0, 0, 0, 600, 600, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (44, 2010201, 201, 20102, N'��ѡ��', N'', 0, 0, 0, 900, 900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (45, 2010301, 201, 20103, N'��ѡ��', N'', 0, 0, 0, 7800, 7800, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (46, 2010401, 201, 20104, N'��ѡ��', N'', 0, 0, 0, 54000, 54000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (47, 2010501, 201, 20105, N'��ѡ��', N'', 0, 0, 0, 9000, 9000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (48, 2010601, 201, 20106, N'��ѡ��', N'', 0, 0, 0, 2600, 2600, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (49, 2010701, 201, 20107, N'��ѡ��', N'', 0, 0, 0, 900, 900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (50, 2010801, 201, 20108, N'ǰһֱѡ', N'', 0, 0, 0, 1300, 1300, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (51, 2010901, 201, 20109, N'ǰ��ֱѡ', N'', 0, 0, 0, 13000, 13000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (52, 2011001, 201, 20110, N'ǰ����ѡ', N'', 0, 0, 0, 6500, 6500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (53, 2011101, 201, 20111, N'ǰ��ֱѡ', N'', 0, 0, 0, 117000, 117000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (54, 2011201, 201, 20112, N'ǰ����ѡ', N'', 0, 0, 0, 19500, 19500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (55, 2020101, 202, 20201, N'��ѡ��', N'', 0, 0, 0, 600, 600, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (56, 2020201, 202, 20202, N'��ѡ��', N'', 0, 0, 0, 900, 900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (57, 2020301, 202, 20203, N'��ѡ��', N'', 0, 0, 0, 7800, 7800, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (58, 2020401, 202, 20204, N'��ѡ��', N'', 0, 0, 0, 54000, 54000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (59, 2020501, 202, 20205, N'��ѡ��', N'', 0, 0, 0, 9000, 9000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (60, 2020601, 202, 20206, N'��ѡ��', N'', 0, 0, 0, 2600, 2600, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (61, 2020701, 202, 20207, N'��ѡ��', N'', 0, 0, 0, 900, 900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (62, 2020801, 202, 20208, N'ǰһֱѡ', N'', 0, 0, 0, 1300, 1300, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (63, 2020901, 202, 20209, N'ǰ��ֱѡ', N'', 0, 0, 0, 13000, 13000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (64, 2021001, 202, 20210, N'ǰ����ѡ', N'', 0, 0, 0, 6500, 6500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (65, 2021101, 202, 20211, N'ǰ��ֱѡ', N'', 0, 0, 0, 117000, 117000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (66, 2021201, 202, 20212, N'ǰ����ѡ', N'', 0, 0, 0, 19500, 19500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (67, 8010101, 801, 20213, N'һ�Ƚ�', N'��6+1', 0, 0, 0, 500000000, 500000000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (68, 8010102, 801, 20213, N'���Ƚ�', N'��6+0', 0, 0, 0, 500000000, 500000000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (69, 8010103, 801, 20213, N'���Ƚ�', N'��5+1', 0, 0, 0, 300000, 300000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (70, 8010104, 801, 20213, N'�ĵȽ�', N'��5+0����4+1', 0, 0, 0, 20000, 20000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (71, 8010105, 801, 20213, N'��Ƚ�', N'��4+0����3+1', 0, 0, 0, 1000, 1000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (72, 8010106, 801, 20213, N'���Ƚ�', N'��2+1����1+1����0+1', 0, 0, 0, 500, 500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (95, 9010101, 901, 20213, N'һ�Ƚ�', N'5+2', 0, 0, 0, 500000000, 500000000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (96, 9010102, 901, 20213, N'���Ƚ�', N'5+1', 0, 0, 0, 500000000, 500000000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (97, 9010103, 901, 20213, N'���Ƚ�', N'5+0/4+2', 0, 0, 0, 300000, 300000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (98, 9010104, 901, 20213, N'�ĵȽ�', N'4+1/3+2', 0, 0, 0, 20000, 20000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (99, 9010105, 901, 20213, N'��Ƚ�', N'4+0/3+1/2+2', 0, 0, 0, 1000, 1000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (100, 9010106, 901, 20213, N'���Ƚ�', N'3+0/2+1/1+2/0+2', 0, 0, 0, 500, 500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (101, 9010201, 901, 20213, N'һ�Ƚ�׷��', N'5+2', 0, 0, 0, 500000000, 500000000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (102, 9010202, 901, 20213, N'���Ƚ�׷��', N'5+1', 0, 0, 0, 500000000, 500000000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (103, 9010203, 901, 20213, N'���Ƚ�׷��', N'5+0/4+2', 0, 0, 0, 300000, 300000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (104, 9010204, 901, 20213, N'�ĵȽ�׷��', N'4+1/3+2', 0, 0, 0, 30000, 30000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (105, 9010205, 901, 20213, N'��Ƚ�׷��', N'4+0/3+1/2+2', 0, 0, 0, 1500, 1500, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (106, 3010101, 301, 30101, N'����ֱѡ', N'ѡ5�����룬�뿪��������ȫ��λȫ�����', 0, 0, 1, 10000000, 8000000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (107, 3010201, 301, 30102, N'����ͨѡ', N'ѡ5�����룬�뿪��������ȫ��λȫ�����', 0, 0, 99, 2044000, 1635200, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (108, 3010202, 301, 30102, N'����ͨѡ', N'ѡ5�����룬�뿪������ǰ��λ�����λ��λ���', 0, 0, 99, 22000, 22000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (109, 3010203, 301, 30102, N'����ͨѡ', N'ѡ5�����룬�뿪������ǰ��λ����λ��λ���', 0, 0, 99, 2000, 2000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (110, 3010301, 301, 30103, N'����ֱѡ', N'ѡ3�����룬�뿪��������������λ��λ���', 0, 0, 99, 100000, 100000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (111, 3010401, 301, 30104, N'��������', N'ѡ2�����룬����������Ͷע���뿪�������������ͬ��˳����(������ָ�����������λ������λ������ͬ����188��)', 0, 0, 99, 32000, 32000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (112, 3010501, 301, 30105, N'��������', N'ѡ3�����룬����������Ͷע���뿪���������λ��ͬ��˳���ޣ�������ָ�����������λ�������������ͬ����135����', 0, 0, 99, 16000, 16000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (113, 3010601, 301, 30106, N'����ֱѡ', N'ѡ2�����룬�뿪�������������λ��λ���', 0, 0, 99, 10000, 10000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (114, 3010701, 301, 30107, N'����ֱѡ��ֵ', N'���Ǻ�ֵָʮλ�͸�λ���֮�ͣ���ѡ��ֵ�뿪���������λ֮����ͬ�����н�100Ԫ��', 1, 0, 99, 10000, 10000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (115, 3010801, 301, 30108, N'������ѡ', N'ѡ2�����룬�뿪�������������λ���', 0, 0, 99, 5000, 5000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (116, 3010901, 301, 30109, N'������ѡ��ֵ', N'��ѡ��ֵ�����뿪���������λ֮����ͬ�����н�50Ԫ���罱��Ϊ���Ӻţ����н�100Ԫ��', 1, 0, 99, 5000, 5000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (117, 3011001, 301, 30110, N'һ��', N'ѡ1�����룬�뿪�������λ���', 0, 0, 99, 1000, 1000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (118, 3011101, 301, 30111, N'��С��˫', N'�뿪��������λ�������԰�λ���', 0, 0, 99, 400, 400, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (119, 2021301, 202, 20213, N'��ѡ3_һ�Ƚ�', N'', 0, 0, 99, 138400, 138400, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (120, 2021302, 202, 20213, N'��ѡ3_���Ƚ�', N'', 0, 0, 99, 21400, 21400, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (121, 2021303, 202, 20213, N'��ѡ3_���Ƚ�', N'', 0, 0, 99, 1900, 1900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (122, 2021401, 202, 20214, N'��ѡ4_һ�Ƚ�', N'', 0, 0, 99, 15400, 15400, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (123, 2021402, 202, 20214, N'��ѡ4_���Ƚ�', N'', 0, 0, 99, 1900, 1900, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (124, 2021501, 202, 20215, N'��ѡ5_һ�Ƚ�', N'', 0, 0, 99, 108000, 108000, 0)
GO
INSERT [dbo].[CT_WinTypes] ([WinID], [WinCode], [LotteryCode], [PlayCode], [WinName], [WinNumber], [IsSumValue], [SumValue], [Sort], [DefaultMoney], [DefaultMoneyNoWithTax], [IsDel]) VALUES (125, 2021502, 202, 20215, N'��ѡ5_���Ƚ�', N'', 0, 0, 99, 9000, 9000, 0)
GO
SET IDENTITY_INSERT [dbo].[CT_WinTypes] OFF
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__Chase__5FB4032D]  DEFAULT ((0)) FOR [ChaseTaskID]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__Schem__60A82766]  DEFAULT ((0)) FOR [SchemeID]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__Creat__619C4B9F]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__Isuse__62906FD8]  DEFAULT ((0)) FOR [IsuseID]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__Multi__63849411]  DEFAULT ((1)) FOR [Multiple]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__Amoun__6478B84A]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__RedPa__656CDC83]  DEFAULT ((0)) FOR [RedPacketId]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__RedPa__666100BC]  DEFAULT ((0)) FOR [RedPacketMoney]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__Quash__675524F5]  DEFAULT ((0)) FOR [QuashStatus]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__IsExe__6849492E]  DEFAULT ((0)) FOR [IsExecuted]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__Secre__693D6D67]  DEFAULT ((0)) FOR [SecrecyLevel]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__Lotte__6A3191A0]  DEFAULT ((0)) FOR [LotteryNumber]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF__CT_ChaseT__IsSha__6B25B5D9]  DEFAULT ((1)) FOR [IsShare]
GO
ALTER TABLE [dbo].[CT_ChaseTaskDetails] ADD  CONSTRAINT [DF_CT_ChaseTaskDetails_IsSendOut]  DEFAULT ((0)) FOR [IsSendOut]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__UserI__4F7D9B64]  DEFAULT ((0)) FOR [UserID]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__Schem__5071BF9D]  DEFAULT ((0)) FOR [SchemeID]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__Lotte__5165E3D6]  DEFAULT ((0)) FOR [LotteryCode]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__Start__525A080F]  DEFAULT (getdate()) FOR [StartTime]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__EndTi__534E2C48]  DEFAULT (getdate()) FOR [EndTime]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__Isuse__54425081]  DEFAULT ((0)) FOR [IsuseCount]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__BetTy__553674BA]  DEFAULT ((0)) FOR [BetType]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__StopT__562A98F3]  DEFAULT ((0)) FOR [StopTypeWhenWin]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__StopT__571EBD2C]  DEFAULT ((0)) FOR [StopTypeWhenWinMoney]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__Quash__5812E165]  DEFAULT ((0)) FOR [QuashStatus]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__Title__5907059E]  DEFAULT ('') FOR [Title]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__Descr__59FB29D7]  DEFAULT ('') FOR [Descriptions]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__Schem__5AEF4E10]  DEFAULT ((0)) FOR [SchemeBonusScale]
GO
ALTER TABLE [dbo].[CT_ChaseTasks] ADD  CONSTRAINT [DF__CT_ChaseT__FromC__5BE37249]  DEFAULT ((0)) FOR [FromClient]
GO
ALTER TABLE [dbo].[CT_IsuseBonuses] ADD  DEFAULT ((0)) FOR [AdminID]
GO
ALTER TABLE [dbo].[CT_IsuseBonuses] ADD  DEFAULT ((0)) FOR [IsuseID]
GO
ALTER TABLE [dbo].[CT_IsuseBonuses] ADD  DEFAULT ((0)) FOR [defaultMoney]
GO
ALTER TABLE [dbo].[CT_IsuseBonuses] ADD  DEFAULT ((0)) FOR [DefaultMoneyNoWithTax]
GO
ALTER TABLE [dbo].[CT_IsuseBonuses] ADD  DEFAULT ('') FOR [WinNumber]
GO
ALTER TABLE [dbo].[CT_IsuseBonuses] ADD  DEFAULT ('') FOR [WinBet]
GO
ALTER TABLE [dbo].[CT_IsuseBonuses] ADD  DEFAULT ('') FOR [WinLevel]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__Lotte__55AAAAAF]  DEFAULT ((0)) FOR [LotteryCode]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__Isuse__569ECEE8]  DEFAULT ('') FOR [IsuseName]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__IsExe__5792F321]  DEFAULT ((0)) FOR [IsExecuteChase]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__IsOpe__5887175A]  DEFAULT ((0)) FOR [IsOpened]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__OpenN__597B3B93]  DEFAULT ('') FOR [OpenNumber]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__OpenO__5A6F5FCC]  DEFAULT ((0)) FOR [OpenOperatorID]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__Isuse__5B638405]  DEFAULT ((0)) FOR [IsuseState]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__Updat__5C57A83E]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__OpenN__5D4BCC77]  DEFAULT ('') FOR [OpenNotice]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__Total__5E3FF0B0]  DEFAULT ((0)) FOR [TotalSales]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__WinRo__5F3414E9]  DEFAULT ('') FOR [WinRollover]
GO
ALTER TABLE [dbo].[CT_Isuses] ADD  CONSTRAINT [DF__CT_Isuses__Betti__60283922]  DEFAULT ('') FOR [BettingPrompt]
GO
ALTER TABLE [dbo].[CT_News] ADD  CONSTRAINT [DF__CT_News__TypeID__6FBF826D]  DEFAULT ((0)) FOR [TypeID]
GO
ALTER TABLE [dbo].[CT_OutETickets] ADD  CONSTRAINT [DF_CT_OutETickets_LotteryCode]  DEFAULT ((0)) FOR [LotteryCode]
GO
ALTER TABLE [dbo].[CT_SalePoint] ADD  CONSTRAINT [DF_CT_SalePoint_TicketSourceID]  DEFAULT ((1)) FOR [TicketSourceID]
GO
ALTER TABLE [dbo].[CT_SalePoint] ADD  CONSTRAINT [DF_CT_SalePoint_LotteryCode]  DEFAULT ((0)) FOR [LotteryCode]
GO
ALTER TABLE [dbo].[CT_SalePoint] ADD  CONSTRAINT [DF_CT_SalePoint_OperatorID]  DEFAULT ((0)) FOR [OperatorID]
GO
ALTER TABLE [dbo].[CT_SalePoint] ADD  CONSTRAINT [DF_CT_SalePoint_OperatorTime]  DEFAULT (getdate()) FOR [OperatorTime]
GO
ALTER TABLE [dbo].[CT_SalePoint] ADD  CONSTRAINT [DF_CT_SalePoint_Describe]  DEFAULT ('') FOR [Describe]
GO
ALTER TABLE [dbo].[CT_SalePointFile] ADD  CONSTRAINT [DF_CT_SalePointFile_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[CT_SalePointRecord] ADD  CONSTRAINT [DF_Table_1_TicketSourceID]  DEFAULT ((1)) FOR [TicketSourceID]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__Ticke__26A5A303]  DEFAULT ((1)) FOR [TicketSourceID]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__Creat__2799C73C]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__Schem__288DEB75]  DEFAULT ('') FOR [SchemeID]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__Sends__2D52A092]  DEFAULT ((1)) FOR [Sends]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__Handl__2E46C4CB]  DEFAULT (getdate()) FOR [HandleDateTime]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__Ident__2F3AE904]  DEFAULT ('') FOR [Identifiers]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__Ticke__302F0D3D]  DEFAULT ('') FOR [Ticket]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__WinMo__31233176]  DEFAULT ((0)) FOR [WinMoney]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__Ticke__321755AF]  DEFAULT ((0)) FOR [TicketStatus]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  CONSTRAINT [DF__CT_Scheme__Chase__2F10CBD2]  DEFAULT ((0)) FOR [ChaseTaskDetailsID]
GO
ALTER TABLE [dbo].[CT_SchemeETickets] ADD  DEFAULT ((0)) FOR [IsRobot]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Creat__6438C128]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Schem__652CE561]  DEFAULT ('') FOR [SchemeNumber]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Title__6621099A]  DEFAULT ('') FOR [Title]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Initi__67152DD3]  DEFAULT ((0)) FOR [InitiateUserID]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Lotte__6809520C]  DEFAULT ((0)) FOR [LotteryCode]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Isuse__69F19A7E]  DEFAULT ((0)) FOR [IsuseID]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Isuse__6AE5BEB7]  DEFAULT ('') FOR [IsuseName]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Lotte__6BD9E2F0]  DEFAULT ((0)) FOR [LotteryNumber]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Schem__6EB64F9B]  DEFAULT ((0)) FOR [SchemeMoney]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Secre__6FAA73D4]  DEFAULT ((0)) FOR [SecrecyLevel]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Order__709E980D]  DEFAULT ((0)) FOR [SchemeStatus]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Print__746F28F1]  DEFAULT ((0)) FOR [PrintOutType]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Descr__7B1C2680]  DEFAULT ('') FOR [Describe]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__FromC__7EECB764]  DEFAULT ((0)) FOR [FromClient]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__BuyTy__7FE0DB9D]  DEFAULT ((0)) FOR [BuyType]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__IsSpl__01C9240F]  DEFAULT ((0)) FOR [IsSplit]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Follo__02BD4848]  DEFAULT ((0)) FOR [FollowSchemeID]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Follo__03B16C81]  DEFAULT ((0)) FOR [FollowSchemeBonus]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__Follo__04A590BA]  DEFAULT ((0)) FOR [FollowSchemeBonusScale]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF__CT_Scheme__PlusA__0599B4F3]  DEFAULT ((0)) FOR [PlusAwards]
GO
ALTER TABLE [dbo].[CT_Schemes] ADD  CONSTRAINT [DF_CT_Schemes_IsSendOut]  DEFAULT ((0)) FOR [IsSendOut]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_SchemeID]  DEFAULT ((0)) FOR [SchemeID]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_PlayCode]  DEFAULT ((0)) FOR [PlayCode]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_Multiple]  DEFAULT ((1)) FOR [Multiple]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_Bet]  DEFAULT ((0)) FOR [BetNum]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_IsBuyed]  DEFAULT ((0)) FOR [IsBuyed]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_IsNorm]  DEFAULT ((0)) FOR [IsNorm]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_IsWin]  DEFAULT ((0)) FOR [IsWin]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_IsOpened]  DEFAULT ((0)) FOR [IsOpened]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_WinMoney]  DEFAULT ((0)) FOR [WinMoney]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_WinMoneyNoWithTax]  DEFAULT ((0)) FOR [WinMoneyNoWithTax]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_Schedule]  DEFAULT ((0)) FOR [Schedule]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_WinDescribe]  DEFAULT ('') FOR [WinDescribe]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_WinImage]  DEFAULT ('') FOR [WinImageUrl]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_UpdateTime]  DEFAULT (getdate()) FOR [UpdateTime]
GO
ALTER TABLE [dbo].[CT_SchemesDetail] ADD  CONSTRAINT [DF_CT_SchemesDetail_OpenOperatorID]  DEFAULT ((0)) FOR [OpenOperatorID]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__Schem__64C2D10D]  DEFAULT ((0)) FOR [SchemeETicketsID]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__Schem__65B6F546]  DEFAULT ((0)) FOR [SchemeID]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__UserI__66AB197F]  DEFAULT ((0)) FOR [UserID]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__Lotte__679F3DB8]  DEFAULT ((0)) FOR [LotteryCode]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__WinCo__689361F1]  DEFAULT ((0)) FOR [WinCode]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__Split__6987862A]  DEFAULT ('') FOR [SplitNumber]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__WinNu__6A7BAA63]  DEFAULT ('') FOR [WinNumber]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__Multi__6B6FCE9C]  DEFAULT ('') FOR [Multiple]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__WinMo__6C63F2D5]  DEFAULT ((0)) FOR [WinMoney]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__WinMo__6D58170E]  DEFAULT ((0)) FOR [WinMoneyNoWithTax]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__IsAwa__6E4C3B47]  DEFAULT ((0)) FOR [IsAward]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__Descr__6F405F80]  DEFAULT ('') FOR [Descriptions]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__Suppl__703483B9]  DEFAULT ((0)) FOR [SupplierID]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__BackW__7128A7F2]  DEFAULT ((0)) FOR [BackWinMoney]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__BackW__721CCC2B]  DEFAULT ((0)) FOR [BackWinMoneyNoWithTax]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__WinSt__34DEB3C1]  DEFAULT ((0)) FOR [WinStatus]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF_CT_SchemesDetailsWin_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
ALTER TABLE [dbo].[CT_SchemesWin] ADD  CONSTRAINT [DF__CT_Scheme__IsFir__1F0F6E33]  DEFAULT ((0)) FOR [IsFirstPrize]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [recharge]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [online_recharge]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [offline_recharge]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [withdraw]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [users]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [largess]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [buy_jlk]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [win_jlk]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [buy_jxk]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [win_jxk]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [buy_hbsyydj]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [win_hbsyydj]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [buy_sdsyydj]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [win_sdsyydj]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [buy_cqssc]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [win_cqssc]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [buy_ssq]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [win_ssq]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [buy_dlt]
GO
ALTER TABLE [dbo].[CT_SystemStaticdata] ADD  DEFAULT ((0)) FOR [win_dlt]
GO
ALTER TABLE [dbo].[CT_Users] ADD  CONSTRAINT [DF_CT_Users_Balance]  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[CT_Users] ADD  CONSTRAINT [DF_CT_Users_RedPackets]  DEFAULT ((0)) FOR [GoldBean]
GO
ALTER TABLE [dbo].[CT_Users] ADD  CONSTRAINT [DF_CT_Users_Freeze]  DEFAULT ((0)) FOR [Freeze]
GO
ALTER TABLE [dbo].[CT_Users] ADD  CONSTRAINT [DF_CT_Users_IsRobot]  DEFAULT ((0)) FOR [IsRobot]
GO
ALTER TABLE [dbo].[CT_Users] ADD  CONSTRAINT [DF_CT_Users_IsCanLogin]  DEFAULT ((0)) FOR [IsCanLogin]
GO
ALTER TABLE [dbo].[CT_UsersBanks] ADD  CONSTRAINT [DF__CT_UsersB__BankT__70D3A237]  DEFAULT ((0)) FOR [BankType]
GO
ALTER TABLE [dbo].[CT_UsersBanks] ADD  CONSTRAINT [DF__CT_UsersB__BankN__71C7C670]  DEFAULT ('') FOR [BankName]
GO
ALTER TABLE [dbo].[CT_UsersBanks] ADD  CONSTRAINT [DF__CT_UsersB__CardN__72BBEAA9]  DEFAULT ('') FOR [CardNumber]
GO
ALTER TABLE [dbo].[CT_UsersBanks] ADD  CONSTRAINT [DF__CT_UsersBa__Area__73B00EE2]  DEFAULT ('') FOR [Area]
GO
ALTER TABLE [dbo].[CT_UsersBanks] ADD  CONSTRAINT [DF__CT_UsersB__Creat__74A4331B]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[CT_UsersLoginRecord] ADD  CONSTRAINT [DF__CT_UserLo__UserI__163A3110]  DEFAULT ((0)) FOR [UserID]
GO
ALTER TABLE [dbo].[CT_UsersLoginRecord] ADD  CONSTRAINT [DF__CT_UserLo__Sourc__172E5549]  DEFAULT ((0)) FOR [SourceType]
GO
ALTER TABLE [dbo].[CT_UsersLoginRecord] ADD  CONSTRAINT [DF__CT_UserLo__Token__18227982]  DEFAULT ('') FOR [Token]
GO
ALTER TABLE [dbo].[CT_UsersLoginRecord] ADD  CONSTRAINT [DF__CT_UserLo__Login__19169DBB]  DEFAULT (getdate()) FOR [LoginTime]
GO
ALTER TABLE [dbo].[CT_UsersPayDetail] ADD  CONSTRAINT [DF__CT_UserPa__Order__607D3EDD]  DEFAULT ('') FOR [OrderNo]
GO
ALTER TABLE [dbo].[CT_UsersPayDetail] ADD  CONSTRAINT [DF__CT_UserPa__Alipa__61716316]  DEFAULT ('') FOR [RechargeNo]
GO
ALTER TABLE [dbo].[CT_UsersPayDetail] ADD  CONSTRAINT [DF__CT_UserPa__PayTy__6265874F]  DEFAULT ('') FOR [PayType]
GO
ALTER TABLE [dbo].[CT_UsersPayDetail] ADD  CONSTRAINT [DF__CT_UserPa__Amoun__6359AB88]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[CT_UsersPayDetail] ADD  CONSTRAINT [DF_T_UserPayDetails_FormalitiesFees]  DEFAULT ((0)) FOR [FormalitiesFees]
GO
ALTER TABLE [dbo].[CT_UsersPayDetail] ADD  CONSTRAINT [DF_T_UserPayDetails_Result]  DEFAULT ((0)) FOR [Result]
GO
ALTER TABLE [dbo].[CT_UsersPayDetail] ADD  CONSTRAINT [DF_T_UserPayDetails_DateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[CT_UsersPayDetail] ADD  CONSTRAINT [DF_CT_UserPayDetails_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
ALTER TABLE [dbo].[CT_UsersPayRefund] ADD  CONSTRAINT [DF__CT_UserPa__PayID__39F86E99]  DEFAULT ((0)) FOR [PayID]
GO
ALTER TABLE [dbo].[CT_UsersPayRefund] ADD  CONSTRAINT [DF__CT_UserPa__Refun__3AEC92D2]  DEFAULT ('') FOR [RefundNo]
GO
ALTER TABLE [dbo].[CT_UsersPayRefund] ADD  CONSTRAINT [DF__CT_UserPa__Recha__3BE0B70B]  DEFAULT ('') FOR [RechargeNo]
GO
ALTER TABLE [dbo].[CT_UsersPayRefund] ADD  CONSTRAINT [DF__CT_UserPa__Amoun__3CD4DB44]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[CT_UsersPayRefund] ADD  CONSTRAINT [DF__CT_UserPa__Forma__3DC8FF7D]  DEFAULT ((0)) FOR [FormalitiesFees]
GO
ALTER TABLE [dbo].[CT_UsersPayRefund] ADD  CONSTRAINT [DF__CT_UserPa__Resul__3EBD23B6]  DEFAULT ((0)) FOR [Result]
GO
ALTER TABLE [dbo].[CT_UsersPayRefund] ADD  CONSTRAINT [DF__CT_UserPa__Creat__3FB147EF]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[CT_UsersPush] ADD  CONSTRAINT [DF_CT_UsersPush_ModifyTime]  DEFAULT (getdate()) FOR [ModifyTime]
GO
ALTER TABLE [dbo].[CT_UsersRecord] ADD  CONSTRAINT [DF_CT_AccountChange_UserID]  DEFAULT ((0)) FOR [UserID]
GO
ALTER TABLE [dbo].[CT_UsersRecord] ADD  CONSTRAINT [DF_CT_AccountChange_OperatorID]  DEFAULT ((0)) FOR [OperatorID]
GO
ALTER TABLE [dbo].[CT_UsersRecord] ADD  CONSTRAINT [DF_CT_UsersRecord_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[CT_UsersRecord] ADD  DEFAULT ((0)) FOR [CouponsID]
GO
ALTER TABLE [dbo].[CT_UsersStaticdata] ADD  CONSTRAINT [DF_CT_UsersStaticdata_Buy]  DEFAULT ((0)) FOR [Buy]
GO
ALTER TABLE [dbo].[CT_UsersStaticdata] ADD  CONSTRAINT [DF_CT_UsersStaticdata_Win]  DEFAULT ((0)) FOR [Win]
GO
ALTER TABLE [dbo].[CT_UsersWithdraw] ADD  CONSTRAINT [DF__CT_UserPa__UserI__4400FBC0]  DEFAULT ((0)) FOR [UserID]
GO
ALTER TABLE [dbo].[CT_UsersWithdraw] ADD  CONSTRAINT [DF__CT_UserPa__Amoun__44F51FF9]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[CT_UsersWithdraw] ADD  CONSTRAINT [DF__CT_UserPa__BankI__45E94432]  DEFAULT ((0)) FOR [BankID]
GO
ALTER TABLE [dbo].[CT_UsersWithdraw] ADD  CONSTRAINT [DF__CT_UserPa__Creat__46DD686B]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[CT_UsersWithdraw] ADD  CONSTRAINT [DF_CT_UserPayOut_OperaterID]  DEFAULT ((0)) FOR [OperaterID]
GO
ALTER TABLE [dbo].[CT_UsersWithdraw] ADD  CONSTRAINT [DF_CT_UserPayOut_OperTime]  DEFAULT ((0)) FOR [OperTime]
GO
ALTER TABLE [dbo].[CT_UsersWithdraw] ADD  CONSTRAINT [DF_CT_UserPayOut_IsDel]  DEFAULT ((0)) FOR [IsDel]
GO
ALTER TABLE [dbo].[CT_UsersPayDetail]  WITH CHECK ADD  CONSTRAINT [CKC_FORMALITIESFEES_CT_USERP] CHECK  (([FormalitiesFees]>=(0)))
GO
ALTER TABLE [dbo].[CT_UsersPayDetail] CHECK CONSTRAINT [CKC_FORMALITIESFEES_CT_USERP]
GO
/****** Object:  StoredProcedure [dbo].[CP_AddIssueLuck]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-12-13
-- Description:	��������28�ں�
-- =============================================
CREATE PROCEDURE [dbo].[CP_AddIssueLuck]
	@IsseuName INT,         -- ��ʼ�ں�
	@LotteryCode	INT,    -- ����
	@Date	DATETIME,       -- ����
	@Days	INT,            -- ����
	@ReturnValue INT OUTPUT -- �ص�ֵ
AS
BEGIN
	IF @Days <=0
	BEGIN
		SET @ReturnValue = -1
		RETURN -1
	END

	DECLARE @T TABLE(id int, value varchar(128))
	DECLARE @LotteryID INT, @IntervalType VARCHAR(64)
	
	SELECT @LotteryID=LotteryID, @IntervalType=IntervalType FROM dbo.CT_Lotteries WHERE LotteryCode=@LotteryCode
	IF @@ROWCOUNT = 0
	BEGIN
		SET @ReturnValue = -2
		RETURN -2
	END
	
	INSERT @T SELECT * FROM dbo.F_SplitStrToTable(@IntervalType,'@')
	IF(SELECT COUNT(*) FROM @T ) < 5
	BEGIN
		SET @ReturnValue = -3
		RETURN -3
	END
	DECLARE @Type varchar(5), @Num INT, @Time VARCHAR(20), @interval INT, @WS INT,@GTKS VARCHAR(20),@TLen INT
	SELECT @interval=value FROM @T WHERE id=1	--�������
	SELECT @Type=value FROM @T WHERE id=2	--��������
	SELECT @Num=value FROM @T WHERE id=3	--����
	SELECT @Time=value FROM @T WHERE id=4	--��Ƶ�ʣ���һ�ڿ�ʼʱ������һ�ڿ�ʼʱ�䡢�����ʣ���ʼʱ��--��ֹʱ��
	SELECT @WS=value FROM @T WHERE id=5 --�ں�λ��
	SELECT @TLen = COUNT(*) FROM @T
	IF @TLen > 5
		BEGIN
			SELECT @GTKS=value FROM @T WHERE id=6 --���쿪��ʱ��
		END
	ELSE
		BEGIN
			SET @GTKS = NULL
		END 
	DECLARE @NumTable TABLE(LotteryCode INT, IsuseName varchar(20), StartTime DATETIME, EndTime DATETIME)--�ں���ʱ��
	DECLARE @index INT=1, @i INT=1, @IsuseName VARCHAR(20), @StartDate DATETIME, @EndDate DATETIME, @Date2 VARCHAR(8)

	WHILE(@i <= @Days)
		BEGIN
		    SET @Date2 = CONVERT(VARCHAR(8),@Date,112)
			SET @StartDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@Date) + ' ' + dbo.fnGetField(@Time, 1, '-'))
		    SET @EndDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@Date) + ' ' + dbo.fnGetField(@Time, 2, '-'))
			WHILE(@index <= @Num)
				BEGIN
				    SET @IsuseName = CONVERT(VARCHAR(200),(@IsseuName + ((@i-1)*@Num) + @index));

					INSERT @NumTable(LotteryCode, IsuseName, StartTime, EndTime) 
					VALUES (@LotteryCode, @IsuseName, @StartDate, @EndDate);

					SET @index = @index + 1
					SET @StartDate=DATEADD(SECOND,@interval,@StartDate)
					SET @EndDate=DATEADD(SECOND,@interval,@EndDate)
				END
			INSERT dbo.CT_Isuses(LotteryCode,IsuseName,StartTime,EndTime,IsExecuteChase,IsOpened,OpenNumber,OpenOperatorID,IsuseState,UpdateTime)
				SELECT a.LotteryCode, a.IsuseName, a.StartTime, a.EndTime, 0 AS IsExecuteChase, 0 AS IsOpened,
					'' AS OpenNumber,0 AS OpenOperatorID,0 AS IsuseState,GETDATE() AS UpdateTime
				FROM @NumTable a LEFT JOIN dbo.CT_Isuses b ON b.IsuseName=a.IsuseName AND b.LotteryCode=a.LotteryCode
					WHERE b.IsuseID IS NULL

			SET @i = @i + 1
			SET @index = 1
			SET @Date = DATEADD(DAY,1,@Date)
			DELETE @NumTable
		END
END

GO
/****** Object:  StoredProcedure [dbo].[CP_Rebate]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2018-01-11
-- Description:	�û���ˮ
-- =============================================
CREATE PROCEDURE [dbo].[CP_Rebate]
	@RebateTables XML                -- ��ˮ�б�
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		DECLARE @ItemRebateTables TABLE(uuid BIGINT,rid BIGINT,am BIGINT);  --����ȯ�б�
		INSERT INTO @ItemRebateTables (uuid ,rid ,am ) 
		SELECT 
		T.c.value('(uuid/text())[1]','BIGINT'), 
		T.c.value('(rid/text())[1]','BIGINT'), 
		T.c.value('(am/text())[1]','BIGINT')
		FROM @RebateTables.nodes('/ArrayOfCv_Rebate/cv_Rebate') AS T(c)

		--�����¼
		INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
		SELECT t.uuid,26,t.am,(u.Balance+t.am),'��Ա��ˮ',t.rid,0,GETDATE(),0 FROM @ItemRebateTables AS t
		INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

		--������
		UPDATE u SET u.Balance = u.Balance + t.am  FROM @ItemRebateTables AS t
		INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid


	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[CP_UserRebate]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2018-01-09
-- Description:	��Ա��ˮ��¼
-- =============================================
CREATE PROCEDURE [dbo].[CP_UserRebate]
	@UserID BIGINT,                   --�û����
	@PageIndex INT,                   --��ǰҳ
	@PageSize INT,                    --ҳ��С
	@RecordCount INT OUTPUT           --�ص��ܼ�¼
AS
BEGIN
	
	SELECT ROW_NUMBER() OVER(ORDER BY ur.CreateTime DESC) AS Num ,ur.TradeAmount,ur.CreateTime,lr.RoomName,lr.RoomID FROM dbo.CT_UsersRecord AS ur
	LEFT JOIN dbo.CT_LotteryRoom AS lr ON (lr.RoomID = CONVERT(BIGINT,ur.RelationID) OR lr.RoomCode = ur.RelationID) AND ur.UserID=@UserID
	WHERE ur.TradeType = 26
	ORDER BY Num OFFSET (@PageIndex - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;
	SELECT @RecordCount = COUNT(1) FROM dbo.CT_UsersRecord
	WHERE UserID=@UserID AND TradeType = 26;

END

GO
/****** Object:  StoredProcedure [dbo].[udp_AccountDetail]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--�˻���ұ仯��ϸ��¼
--2017-5-19
CREATE PROC [dbo].[udp_AccountDetail]
  @UserID  BIGINT,
  @tradeType  INT,
  @startTime  datetime,
  @endTime  datetime,
  @pageSize INT,  --ÿҳ��¼��
  @pageIndex INT,  --��ǰҳ��
  @recordCount INT OUTPUT,    --��¼����
  @SumMoneyAdd BIGINT OUTPUT, 
  @SumMoneySub BIGINT OUTPUT, 
  @SumReward BIGINT OUTPUT
AS
  SET NOCOUNT ON;
  BEGIN
  
  DECLARE @SqlStr NVARCHAR(2000),@SqlWhere NVARCHAR(1500),@SqlCount NVARCHAR(4000)
  --ƴ��
  SET @SqlStr = 'SELECT ROW_NUMBER() OVER(ORDER BY a.ID DESC) as Num,a.ID, a.CreateTime, a.TradeType, a.Balance, b.SchemeID,
          CASE a.TradeType 
            WHEN 0 THEN a.TradeAmount --��ֵ
            WHEN 3 THEN a.TradeAmount --����ʧ�ܽⶳ
            WHEN 4 THEN a.TradeAmount --�𶹶һ�
            WHEN 5 THEN a.TradeAmount --�н�
            WHEN 11 THEN a.TradeAmount --�û�����
            WHEN 12 THEN a.TradeAmount --ϵͳ����
            WHEN 13 THEN a.TradeAmount --׷�ų���
            WHEN 14 THEN a.TradeAmount --Ͷעʧ���˿�
            WHEN 15 THEN a.TradeAmount --��Ʊʧ���˿�
            WHEN 17 THEN a.TradeAmount --�˿�ʧ�ܷ��ؽ��
            ELSE 0
          END AS MoneyAdd,
          CASE a.TradeType 
            WHEN 5 THEN a.TradeAmount --�н�
            ELSE 0
          END AS SumReward,
          CASE a.TradeType 
            WHEN 1 THEN a.TradeAmount --��������
            WHEN 2 THEN a.TradeAmount --���ֶ���
            WHEN 16 THEN a.TradeAmount --�����˿��
            ELSE 0 
          END AS MoneySub,
          CASE a.TradeType 
            WHEN 0 THEN a.TradeRemark 
            WHEN 1 THEN c.IsuseName + d.LotteryName + ''�������ѽ��,������:'' + b.SchemeNumber
            WHEN 2 THEN ''�û�����'' 
            WHEN 3 THEN ''�û����ʧ�ܽⶳ'' 
            WHEN 4 THEN ''�𶹶һ�'' 
            WHEN 5 THEN c.IsuseName + d.LotteryName + ''�н����佱��,������:'' + b.SchemeNumber
            WHEN 11 THEN ''�û�����''
            WHEN 12 THEN ''ϵͳ����''
            WHEN 13 THEN ''׷�ų���''
            WHEN 14 THEN c.IsuseName + d.LotteryName + ''Ͷעʧ���˿���,������:'' + b.SchemeNumber
            WHEN 15 THEN c.IsuseName + d.LotteryName + ''��Ʊʧ���˿���,������:'' + b.SchemeNumber
            WHEN 16 THEN ''�����˿����''
            WHEN 17 THEN ''�˿�ʧ�ܷ��ؽ��''
          END AS Memo
          FROM CT_UsersRecord a
          LEFT JOIN dbo.CT_Schemes b on b.SchemeID =a.RelationID
          LEFT JOIN dbo.CT_Isuses c on c.IsuseID = b.IsuseID
          LEFT JOIN dbo.CT_Lotteries d on d.LotteryCode = b.LotteryCode'
  --����
  SET @SqlWhere = ' WHERE a.UserID = ' + CONVERT(varchar(20),@UserID) + 'AND a.TradeAmount <> 0 and a.CreateTime >= ''' + convert(VARCHAR(20),@startTime)+''' and a.CreateTime <= ''' + convert(VARCHAR(20),@endTime)+''''
  if @tradeType != -1
    BEGIN
      SET @SqlWhere = @SqlWhere + ' and a.tradeType = '+ convert(VARCHAR(20),@tradeType);
    END

  --�ص�
  SET @SqlCount = N'SELECT @a = COUNT(*),@b = ISNULL(SUM(MoneyAdd),0),@c = ISNULL(SUM(MoneySub),0),@d = ISNULL(SUM(SumReward),0) FROM ( '  + @SqlStr + @SqlWhere + ' ) tab ';

  --��ҳ
  SET @SqlWhere = @SqlWhere + ' ORDER BY Num
  OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
  FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';
  

  DECLARE @SqlData NVARCHAR(4000);
  SET @SqlData = (@SqlStr + @SqlWhere)
  EXEC sp_executesql @SqlData
  EXEC sp_executesql @SqlCount,N'@a INT OUTPUT, @b BIGINT OUTPUT, @c BIGINT OUTPUT, @d BIGINT OUTPUT',@recordCount OUTPUT,@SumMoneyAdd OUTPUT,@SumMoneySub OUTPUT,@SumReward OUTPUT

  END
GO
/****** Object:  StoredProcedure [dbo].[udp_AddAwardInfo]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--���ӽ�����Ϣ
--2017-5-9
CREATE PROC [dbo].[udp_AddAwardInfo]
	@AdminID		INT,
	@LotteryCode	INT,
	@IsuseName	VARCHAR(32),
	@WinRollover	VARCHAR(50),
	@BettingPrompt	VARCHAR(200),
	@TotalSales		BIGINT,
	@AllValues		VARCHAR(1024),
	@ReturnValue int OUTPUT,
	@ReturnDescription varchar(100) OUTPUT
	--WITH ENCRYPTION
AS
	SET NOCOUNT ON;
	
	DECLARE @IsuseID BIGINT=0, @WinNumber VARCHAR(32), @WinLevel VARCHAR(100), @index INT=1, @IBID BIGINT, @IsOpened BIT
	DECLARE @defaultMoney INT, @DefaultMoneyNoWithTax INT, @WinBet VARCHAR(50), @WinID INT, @Values VARCHAR(100)
	--1,240,240,1#2,80,80,2#3,40,40,3#4,25,25,4#5,16,16,5#
	
	SELECT @IsuseID=IsuseID, @WinNumber=OpenNumber, @IsOpened=IsOpened FROM dbo.CT_Isuses WHERE LotteryCode = @LotteryCode AND IsuseName = @IsuseName
	IF @@ROWCOUNT = 0
		BEGIN
			SET @ReturnValue=-1
			SET @ReturnDescription=@IsuseName + '�ںŲ�����'
			RETURN -1
		END
	IF @IsOpened = 0
		BEGIN
			SET @ReturnValue=-2
			SET @ReturnDescription=@IsuseName + '�ں�û�п���'
			RETURN -2
		END
	IF LEN(@WinNumber) = 0
		BEGIN
			SET @ReturnValue=-3
			SET @ReturnDescription=@IsuseName + '�ںſ�������Ϊ��'
			RETURN -3
		END
	
	SET @Values = dbo.fnGetField(@AllValues, @index, '#')
	WHILE LEN(@Values) > 0
		BEGIN 
			SET @WinID = dbo.fnGetField(@Values, 1, ',')
			SET @defaultMoney = dbo.fnGetField(@Values, 2, ',')
			SET @DefaultMoneyNoWithTax = dbo.fnGetField(@Values, 3, ',')
			SET @WinBet = dbo.fnGetField(@Values, 4, ',')

			SELECT @WinLevel=WinName FROM dbo.CT_WinTypes WHERE WinID = @WinID
			SELECT @IBID=ID FROM dbo.CT_IsuseBonuses WHERE IsuseID=@IsuseID AND WinLevel=@WinLevel
			IF @@ROWCOUNT = 0
			BEGIN
				INSERT dbo.CT_IsuseBonuses(AdminID, IsuseID, defaultMoney, DefaultMoneyNoWithTax, WinNumber, WinBet, WinLevel)
					VALUES(@AdminID, @IsuseID, @defaultMoney, @DefaultMoneyNoWithTax, @WinNumber, @WinBet , @WinLevel)
			END
			ELSE
			BEGIN
				UPDATE dbo.CT_IsuseBonuses SET defaultMoney=@defaultMoney, DefaultMoneyNoWithTax=@DefaultMoneyNoWithTax, WinNumber=@WinNumber,
					WinBet=@WinBet, WinLevel=@WinLevel WHERE ID=@IBID
			END
			
			SET @index = @index + 1
			SET @Values = dbo.fnGetField(@AllValues, @index, '#')
		END
	
	UPDATE dbo.CT_Isuses SET WinRollover=@WinRollover, BettingPrompt=@BettingPrompt, TotalSales=@TotalSales WHERE IsuseID = @IsuseID

	SET @ReturnValue=0
	SET @ReturnDescription='¼�뽱����Ϣ�ɹ�'
	RETURN 0


GO
/****** Object:  StoredProcedure [dbo].[udp_AddIssue]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--���������ں�
CREATE PROC [dbo].[udp_AddIssue]
	@LotteryCode	INT,
	@Date	DATETIME,
	@Days	INT,
	@ReturnValue INT OUTPUT
	--WITH ENCRYPTION
AS
	IF @Days <=0
	BEGIN
		SET @ReturnValue = -1
		RETURN -1
	END

	DECLARE @T TABLE(id int, value varchar(128))
	DECLARE @LotteryID INT, @IntervalType VARCHAR(64)
	
	SELECT @LotteryID=LotteryID, @IntervalType=IntervalType FROM dbo.CT_Lotteries WHERE LotteryCode=@LotteryCode
	IF @@ROWCOUNT = 0
	BEGIN
		SET @ReturnValue = -2
		RETURN -2
	END
	
	INSERT @T SELECT * FROM dbo.F_SplitStrToTable(@IntervalType,'@')
	IF(SELECT COUNT(*) FROM @T ) < 5
	BEGIN
		SET @ReturnValue = -3
		RETURN -3
	END

	DECLARE @Type varchar(5), @Num INT, @Time VARCHAR(20), @interval INT, @WS INT,@GTKS VARCHAR(20),@TLen INT
	SELECT @interval=value FROM @T WHERE id=1	--�������
	SELECT @Type=value FROM @T WHERE id=2	--��������
	SELECT @Num=value FROM @T WHERE id=3	--����
	SELECT @Time=value FROM @T WHERE id=4	--��Ƶ�ʣ���һ�ڿ�ʼʱ������һ�ڿ�ʼʱ�䡢�����ʣ���ʼʱ��--��ֹʱ��
	SELECT @WS=value FROM @T WHERE id=5 --�ں�λ��
	SELECT @TLen = COUNT(*) FROM @T
	IF @TLen > 5
		BEGIN
			SELECT @GTKS=value FROM @T WHERE id=6 --���쿪��ʱ��
		END
	ELSE
		BEGIN
			SET @GTKS = NULL
		END 
	DECLARE @NumTable TABLE(LotteryCode INT, IsuseName varchar(20), StartTime DATETIME, EndTime DATETIME)--�ں���ʱ��
	DECLARE @index INT=1, @i INT=1, @IsuseName VARCHAR(20), @StartDate DATETIME, @EndDate DATETIME, @Date2 VARCHAR(8)
	
	IF @Type = '��'  --��Ƶ��
	BEGIN
		WHILE(@i <= @Days)
		BEGIN
			SET @Date2 = CONVERT(VARCHAR(8),@Date,112)
			SET @StartDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@Date) + ' ' + dbo.fnGetField(@Time, 1, '-')) --LEFT(@Time,CHARINDEX('-',@Time)-1))
		    SET @EndDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@Date) + ' ' + dbo.fnGetField(@Time, 2, '-'))

			WHILE(@index <= @Num)
			BEGIN
				IF @TLen > 5 AND @index = 1
					BEGIN
						SET @StartDate = CONVERT(VARCHAR(10),DATEADD(DAY,-1,CONVERT(DATE,@Date)))  + ' ' + @GTKS
					END 
					
				SET @IsuseName = @Date2 + REPLICATE(0,@WS-LEN(@index)) + CONVERT(VARCHAR(3),@index)

				INSERT @NumTable(LotteryCode, IsuseName, StartTime, EndTime) 
					VALUES (@LotteryCode, @IsuseName, @StartDate, @EndDate)
					
				
				IF @TLen > 5 AND @index = 1
					BEGIN
						SET @StartDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@Date) + ' ' + dbo.fnGetField(@Time, 1, '-'))
					END 
				SET @index = @index + 1
				SET @StartDate=DATEADD(SECOND,@interval,@StartDate)
				SET @EndDate=DATEADD(SECOND,@interval,@EndDate)
			END

			INSERT dbo.CT_Isuses(LotteryCode,IsuseName,StartTime,EndTime,IsExecuteChase,IsOpened,OpenNumber,OpenOperatorID,IsuseState,UpdateTime)
				SELECT a.LotteryCode, a.IsuseName, a.StartTime, a.EndTime, 0 AS IsExecuteChase, 0 AS IsOpened,
					'' AS OpenNumber,0 AS OpenOperatorID,0 AS IsuseState,GETDATE() AS UpdateTime
				FROM @NumTable a LEFT JOIN dbo.CT_Isuses b ON b.IsuseName=a.IsuseName AND b.LotteryCode=a.LotteryCode
					WHERE b.IsuseID IS NULL

			SET @i = @i + 1
			SET @index = 1
			SET @Date = DATEADD(DAY,1,@Date)
			DELETE @NumTable
		END	
	END
	ELSE IF @Type = '��'
	BEGIN
		WHILE(@i <= @Days)
		BEGIN
			SET @Date2 = CONVERT(VARCHAR(8),@Date,112)
			SET @StartDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@Date) + ' ' + dbo.fnGetField(@Time, 1, '-')) --LEFT(@Time,CHARINDEX('-',@Time)-1))
		    SET @EndDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@Date) + ' ' + dbo.fnGetField(@Time, 2, '-'))

			WHILE(@index <= @Num)
			BEGIN
				SET @IsuseName = @Date2 + REPLICATE(0,@WS-LEN(@index)) + CONVERT(VARCHAR(3),@index)

				INSERT @NumTable(LotteryCode, IsuseName, StartTime, EndTime) 
					VALUES (@LotteryCode, @IsuseName, @StartDate, @EndDate)

				SET @index = @index + 1
				SET @StartDate=DATEADD(MINUTE,@interval,@StartDate)
				SET @EndDate=DATEADD(MINUTE,@interval,@EndDate)
			END

			INSERT dbo.CT_Isuses(LotteryCode,IsuseName,StartTime,EndTime,IsExecuteChase,IsOpened,OpenNumber,OpenOperatorID,IsuseState,UpdateTime)
				SELECT a.LotteryCode, a.IsuseName, a.StartTime, a.EndTime, 0 AS IsExecuteChase, 0 AS IsOpened,
					'' AS OpenNumber,0 AS OpenOperatorID,0 AS IsuseState,GETDATE() AS UpdateTime
				FROM @NumTable a LEFT JOIN dbo.CT_Isuses b ON b.IsuseName=a.IsuseName AND b.LotteryCode=a.LotteryCode
					WHERE b.IsuseID IS NULL

			SET @i = @i + 1
			SET @index = 1
			SET @Date = DATEADD(DAY,1,@Date)
			DELETE @NumTable
		END	
	END
	
	SET @ReturnValue = 0
	RETURN 0


GO
/****** Object:  StoredProcedure [dbo].[udp_AddIssueFC]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--��Ӹ����ں��磺˫ɫ�򡢴���͸
CREATE PROC [dbo].[udp_AddIssueFC]
	@NewYear DATETIME,	--����żٵ�һ�� 
	@LotteryCode INT,			--���ֱ��
	@ReturnValue INT OUTPUT
	--WITH ENCRYPTION
AS
	DECLARE @LotteryID INT, @IntervalType VARCHAR(64), @Time VARCHAR(32), @WS INT
	SELECT @IntervalType=IntervalType FROM dbo.CT_Lotteries WHERE LotteryCode=@LotteryCode
	IF @@ROWCOUNT = 0
	BEGIN
		SET @ReturnValue = -1
		RETURN -1
	END
	SET @Time = dbo.fnGetField(@IntervalType, 4, '@')
	SET @WS = dbo.fnGetField(@IntervalType, 5, '@')

	DECLARE @NewYearT TABLE(NewDT DATETIME)
	INSERT @NewYearT(NewDT) VALUES (DATEADD(DAY,0,@NewYear))
	INSERT @NewYearT(NewDT) VALUES (DATEADD(DAY,1,@NewYear))
	INSERT @NewYearT(NewDT) VALUES (DATEADD(DAY,2,@NewYear))
	INSERT @NewYearT(NewDT) VALUES (DATEADD(DAY,3,@NewYear))
	INSERT @NewYearT(NewDT) VALUES (DATEADD(DAY,4,@NewYear))
	INSERT @NewYearT(NewDT) VALUES (DATEADD(DAY,5,@NewYear))
	INSERT @NewYearT(NewDT) VALUES (DATEADD(DAY,6,@NewYear))

	DECLARE @FristDay DATETIME, @i INT=1, @IsuseName VARCHAR(10), @StartDate DATETIME, @EndDate DATETIME
	DECLARE @YearFirstDay DATETIME =DATEADD(yy, DATEDIFF(yy, 0, @NewYear), 0) --�����һ��
	DECLARE @YearCount INT=0
	DECLARE @NumTable TABLE(LotteryCode INT, IsuseName varchar(20), StartTime DATETIME, EndTime DATETIME)--�ں���ʱ��

	WHILE @YearCount < 12
	BEGIN
		--���µ�һ��
		SET @FristDay = DATEADD(MONTH, @YearCount, @YearFirstDay)
		--���¶�����
		DECLARE @MonthDayCount INT= (32 - DAY(@FristDay + 32 - DAY(@FristDay)))
		DECLARE @index INT=0, @ToDay DATETIME

		WHILE @index<@MonthDayCount
		BEGIN
			SET @index = @index + 1
			SET @ToDay = DATEADD(DAY,@index - 1, @FristDay) --���µ�ÿһ��
			IF NOT EXISTS(SELECT * FROM @NewYearT WHERE NewDT = @ToDay)
			BEGIN
				IF @LotteryCode = 801 --˫ɫ��
				BEGIN
					IF (DATEPART(WEEKDAY,@ToDay)) = 1 OR (DATEPART(WEEKDAY,@ToDay)) = 3 OR (DATEPART(WEEKDAY,@ToDay)) = 5
					BEGIN 
						SET @IsuseName = CONVERT(VARCHAR(4),@ToDay,112) + REPLICATE(0,@WS-LEN(@i)) + CONVERT(VARCHAR(3),@i)
						SET @StartDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@ToDay) + ' ' + dbo.fnGetField(@Time, 1, '-'))
						IF (DATEPART(WEEKDAY,@ToDay)) = 1
							SET @StartDate = DATEADD(DAY,-3,@StartDate)
						ELSE IF (DATEPART(WEEKDAY,@ToDay)) = 3 OR (DATEPART(WEEKDAY,@ToDay)) = 5
							SET @StartDate = DATEADD(DAY,-2,@StartDate)
						SET @EndDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@ToDay) + ' ' + dbo.fnGetField(@Time, 2, '-'))
						INSERT @NumTable(LotteryCode, IsuseName, StartTime, EndTime)
							VALUES (@LotteryCode, @IsuseName, @StartDate, @EndDate )
						SET @i = @i + 1
					END
				END
				ELSE IF @LotteryCode =901	--����͸
				BEGIN
					IF(DATEPART(WEEKDAY,@ToDay)) = 2 OR (DATEPART(WEEKDAY,@ToDay)) = 4 OR (DATEPART(WEEKDAY,@ToDay)) = 7
					BEGIN 
						SET @IsuseName = CONVERT(VARCHAR(4),@ToDay,112) + REPLICATE(0,@WS-LEN(@i)) + CONVERT(VARCHAR(3),@i)
						SET @StartDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@ToDay) + ' ' + dbo.fnGetField(@Time, 1, '-'))
						IF (DATEPART(WEEKDAY,@ToDay)) = 7
							SET @StartDate = DATEADD(DAY,-3,@StartDate)
						ELSE IF (DATEPART(WEEKDAY,@ToDay)) = 2 OR (DATEPART(WEEKDAY,@ToDay)) = 4
							SET @StartDate = DATEADD(DAY,-2,@StartDate)
						SET @EndDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@ToDay) + ' ' + dbo.fnGetField(@Time, 2, '-'))
						INSERT @NumTable(LotteryCode, IsuseName, StartTime, EndTime)
							VALUES (@LotteryCode, @IsuseName, @StartDate, @EndDate )
						SET @i = @i + 1
					END
				END
			END
		END
		SET @YearCount = @YearCount + 1
	END
	
	INSERT dbo.CT_Isuses(LotteryCode,IsuseName,StartTime,EndTime,IsExecuteChase,IsOpened,OpenNumber,OpenOperatorID,IsuseState,UpdateTime)
		SELECT a.LotteryCode, a.IsuseName, a.StartTime, a.EndTime, 0 AS IsExecuteChase, 0 AS IsOpened,
			'' AS OpenNumber,0 AS OpenOperatorID,0 AS IsuseState,GETDATE() AS UpdateTime
		FROM @NumTable a LEFT JOIN dbo.CT_Isuses b ON b.IsuseName=a.IsuseName AND b.LotteryCode=a.LotteryCode
			WHERE b.IsuseID IS NULL
	SET @ReturnValue = 0
	RETURN 0


GO
/****** Object:  StoredProcedure [dbo].[udp_AddSalePointRecord]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--���µ�λ�����ϸ��¼
--2017-9-8
CREATE PROC [dbo].[udp_AddSalePointRecord]
  @TicketSource  INT,
  @LotteryCode  BIGINT,
  @SalesRebate VARCHAR(500),
  @StartTime DATETIME,
  @ReturnValue INT OUTPUT,
  @ReturnDescription VARCHAR(100) OUTPUT
  --WITH ENCRYPTION
AS
  --SET NOCOUNT ON
  SELECT @ReturnDescription=''
  SELECT @ReturnValue = 0
  
  DECLARE 
    @SalesRebateOLD  VARCHAR(500),  --���۽��ݼ���λ���ַ���ƴ��
    @CreateTime DATETIME --����ʱ��

  SELECT Top 1 @CreateTime=CreateTime, @SalesRebateOLD = SalesRebate
    FROM dbo.CT_SalePointRecord WHERE TicketSourceID = @TicketSource AND LotteryCode = @LotteryCode 
	ORDER BY CreateTime DESC
  IF @@ROWCOUNT = 0
  BEGIN
    INSERT INTO CT_SalePointRecord (TicketSourceID,LotteryCode,SalesRebate,StartTime,CreateTime) VALUES (@TicketSource,@LotteryCode,@SalesRebate,@StartTime,GETDATE())
	IF @@ROWCOUNT = 0
    BEGIN
      ROLLBACK TRAN
      SELECT @ReturnValue=-2, @ReturnDescription='���µ�λ�����ϸ��¼ʧ��'
      RETURN -5
    END
  END
  ELSE
  BEGIN
	INSERT INTO CT_SalePointRecord (TicketSourceID,LotteryCode,SalesRebate,LastSalesRebate,StartTime,CreateTime) VALUES (@TicketSource,@LotteryCode,@SalesRebate,@SalesRebateOLD,@StartTime,GETDATE())
	IF @@ROWCOUNT = 0
    BEGIN
      ROLLBACK TRAN
      SELECT @ReturnValue=-2, @ReturnDescription='���µ�λ�����ϸ��¼ʧ��'
      RETURN -5
    END
  END

  
    
  COMMIT TRAN 
  
  SELECT @ReturnValue=0, @ReturnDescription='���µ�λ�����ϸ��¼�ɹ�'
  RETURN 0






























GO
/****** Object:  StoredProcedure [dbo].[udp_ApplyForWithdraw]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--��������洢����
--2017-6-15
CREATE PROC [dbo].[udp_ApplyForWithdraw]
	@UserCode	BIGINT, --�����û�
	@BankCode BIGINT,	--��������
	@Amount BIGINT,	    --���ֽ��
	@PayPwd VARCHAR(100),	--���ֽ��
	@Result INT OUTPUT -- ���ؽ��
	--WITH ENCRYPTION
AS
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		BEGIN
		--��������ֽ�� strat
		--�����ֽ�� �˻���� �˻�֧������
		DECLARE @MayWithdrawBalance BIGINT,@Balance BIGINT,@PayPassword VARCHAR(100),@PayOutID BIGINT;
		--��ֵʱ��
		DECLARE @PayDateTime DATETIME
		SELECT TOP 1 @PayDateTime = CompleteTime from dbo.CT_UsersPayDetail WHERE Result=1 AND UserID=@UserCode ORDER BY CompleteTime DESC
		--SELECT @MayWithdrawBalance = SUM(BackWinMoney) FROM dbo.CT_SchemesWin WHERE UserID=@UserCode AND AddDateTime > @PayDateTime
		SELECT MayWithdrawBalance=WithdrawMoney FROM dbo.CT_UsersExtend WHERE UserID=@UserCode
		SELECT @Balance = Balance FROM dbo.CT_Users WHERE UserID=@UserCode
		SELECT @PayPassword = PayPassword FROM dbo.CT_Users WHERE UserID=@UserCode
		--��������ֽ�� end AddDateTime
		
		--�û����
		IF @Amount > @Balance
			BEGIN
				SET @Result= -2;
			END 
		ELSE IF @Amount > @MayWithdrawBalance --���ֽ��
			BEGIN
				SET @Result= -3;
			END
		ELSE IF @PayPwd != @PayPassword
			BEGIN
				SET @Result= -4;
			END 
		ELSE IF @Result = 0 OR  @Result IS NULL
			BEGIN
				--�û�������
				UPDATE dbo.CT_Users SET Balance=Balance-@Amount,Freeze=Freeze+@Amount WHERE UserID=@UserCode
				--���ּ�¼
				INSERT INTO dbo.CT_UsersWithdraw(UserID ,Amount ,BankID ,CreateTime ,PayOutStatus)
										 VALUES (@UserCode , @Amount , @BankCode , GETDATE(), 0)
				SELECT @PayOutID = @@IDENTITY
				--���׼�¼
				INSERT INTO dbo.CT_UsersRecord(UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,CreateTime,RelationID,OperatorID)
									   VALUES (@UserCode ,2 , @Amount , @Balance-@Amount ,N'��������' , GETDATE(),CONVERT(VARCHAR(32),@PayOutID),0)
				
				--�����ֽ��(��ֵ�ٷ�֮��ʮ��) S 
				UPDATE dbo.CT_UsersExtend SET WithdrawMoney = WithdrawMoney - @Amount WHERE UserID=@UserCode
				--�����ֽ�� E
				SET @Result=0
			END
		END

        
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN
			SET @Result = -1; -- δ֪�쳣
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	RETURN @Result;





GO
/****** Object:  StoredProcedure [dbo].[udp_AuditPayFailure]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:    Joan
-- Create date: 2017-07-06
-- Update date: 2017-08-11
-- Description:  ����ʧ��
-- =============================================
CREATE PROCEDURE [dbo].[udp_AuditPayFailure]
  @PayOutID BIGINT,
  @OperaterID INT,
  @Remark varchar(100),  --����ʧ�ܱ�ע
  @Result INT OUTPUT -- ���ؽ��
  --WITH ENCRYPTION
AS
BEGIN
  SET XACT_ABORT ON
  BEGIN TRAN
  BEGIN TRY
  DECLARE @UserID BIGINT,@Amount BIGINT,@Balance BIGINT;
  SELECT @UserID=UserID,@Amount=Amount FROM CT_UsersWithdraw WHERE PayOutID=@PayOutID
  IF @@TRANCOUNT > 0
    BEGIN
      SET @Result = 2; -- ��ѯʧ��
    END
  UPDATE CT_UsersWithdraw set PayOutStatus=6,OperaterID=@OperaterID,OperTime=GETDATE(),Remark=@Remark where PayOutID=@PayOutID
  IF @@TRANCOUNT > 0
    BEGIN
      SET @Result = 3; -- ����ʧ��
    END
    
  UPDATE CT_Users set Balance=Balance+@Amount,Freeze=Freeze-@Amount where UserID=@UserID;
  SELECT @Balance = Balance FROM dbo.CT_Users WHERE UserID=@UserID;
  IF @@TRANCOUNT > 0
    BEGIN
      SET @Result = 3; -- ����ʧ��
    END

	UPDATE dbo.CT_UsersExtend SET WithdrawMoney = WithdrawMoney + @Amount where UserID=@UserID;
	IF @@TRANCOUNT > 0
		BEGIN
		  SET @Result = 3; -- ����ʧ��
		END

  INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
  VALUES  ( @UserID ,3 ,@Amount ,@Balance ,'����ʧ�ܽⶳ' ,CONVERT(VARCHAR(20),@PayOutID) ,@OperaterID ,GETDATE())
  IF @@TRANCOUNT > 0
    BEGIN
      SET @Result = 4; -- ��������ʧ��
    END


  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
      ROLLBACK TRAN
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
  END CATCH
  IF @@TRANCOUNT > 0
  COMMIT TRAN
  SET @Result = 0;
END






GO
/****** Object:  StoredProcedure [dbo].[udp_AuditPaySuccess]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joan
-- Create date: 2017-07-06
-- Description:	���ֳɹ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_AuditPaySuccess]
	@PayOutID BIGINT,
	@OperaterID INT,
	@Result INT OUTPUT -- ���ؽ��
	--WITH ENCRYPTION
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @UserID BIGINT,@Amount BIGINT,@Freeze BIGINT
	SELECT @UserID=UserID,@Amount=Amount FROM CT_UsersWithdraw WHERE PayOutID=@PayOutID
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Result = 2; -- ��ѯʧ��
		END
    update CT_UsersWithdraw set PayOutStatus=4,OperaterID=@OperaterID,OperTime=GETDATE() where PayOutID=@PayOutID ; 
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Result = 3; -- ����ʧ��
		END

	SELECT @Freeze=Freeze FROM dbo.CT_Users
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Result = 2; -- ��ѯʧ��
		END
    SET @Freeze = @Freeze - @Amount;
	update CT_Users set Freeze=@Freeze where UserID=@UserID ;
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Result = 3; -- ����ʧ��
		END
	SELECT * FROM dbo.CT_SchemeETickets
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN
			--SET @Result = -1; -- δ֪�쳣
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	SET @Result = 0;
END


GO
/****** Object:  StoredProcedure [dbo].[udp_AutoCalculatePrize]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--�Զ��㽱�ɽ� ����ִ�нű�ʹ��
--2017-5-12
CREATE PROC [dbo].[udp_AutoCalculatePrize]
	@SchemeETicketsID BIGINT,
	@WinStatus	TINYINT, --1.δ�н�	2.�н�
	@WinMoney INT,
	@WinMoneyNoWithTax INT,
	@SupplierName VARCHAR(10)
	--WITH ENCRYPTION
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		DECLARE @SchemeID BIGINT, @SchemesDetailsWinID BIGINT, @UserID BIGINT, @SupplierID INT=0, @NewWinStatus TINYINT,@SDID BIGINT,@LotteryCode INT;
		DECLARE @WinNumber VARCHAR(50),@SplitNumber VARCHAR(128),@Multiple INT,@Time DATETIME = GETDATE(),@BuyType TINYINT,@ChaseTaskDetailsID BIGINT,@IsuseID BIGINT;
		SELECT @SchemeID = SchemeID,@SDID = SDID,@SplitNumber = Number,@Multiple = Multiple,@ChaseTaskDetailsID = ChaseTaskDetailsID FROM dbo.CT_SchemeETickets 
		WHERE SchemeETicketsID = @SchemeETicketsID;
		SELECT @UserID = InitiateUserID,@LotteryCode = LotteryCode,@BuyType=BuyType,@IsuseID = IsuseID FROM dbo.CT_Schemes WHERE SchemeID = @SchemeID;
		IF @BuyType = 1
			BEGIN
				SELECT @WinNumber = i.OpenNumber FROM dbo.CT_ChaseTaskDetails AS cd
				INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID = cd.IsuseID AND cd.ID = @ChaseTaskDetailsID
			END
		ELSE
			BEGIN
			    SELECT @WinNumber = OpenNumber FROM CT_Isuses WHERE IsuseID = @IsuseID
			END
		IF @SupplierName = 'HuaYang'
			BEGIN
			    SET @SupplierID=1
			END
		IF ISNULL(@WinMoneyNoWithTax,0) > 0
			BEGIN
			    --�н�����
				INSERT INTO dbo.CT_SchemesWin( SchemeETicketsID ,SchemeID ,UserID ,LotteryCode ,WinCode ,SplitNumber ,WinNumber ,Multiple ,WinMoney ,WinMoneyNoWithTax ,
											   IsAward ,Descriptions ,SupplierID ,BackWinMoney ,BackWinMoneyNoWithTax ,BackDateTime ,AddDateTime ,WinStatus ,IsDel ,IsFirstPrize)
				VALUES  ( @SchemeETicketsID ,@SchemeID ,@UserID ,@LotteryCode ,0 ,@SplitNumber ,@WinNumber ,@Multiple ,ISNULL(@WinMoney,0) ,ISNULL(@WinMoneyNoWithTax,0) ,1 ,
							'�ӿڷ��� ' + CONVERT(VARCHAR(50),(ISNULL(@WinMoneyNoWithTax,0)/100)) + 'Ԫ' ,@SupplierID ,@WinMoney ,@WinMoneyNoWithTax ,@Time ,@Time ,1 ,0 ,1);
				--��������
				UPDATE dbo.CT_SchemesDetail SET WinMoney = ISNULL(WinMoney,0) + ISNULL(@WinMoney,0), WinMoneyNoWithTax = ISNULL(WinMoneyNoWithTax,0) + ISNULL(@WinMoneyNoWithTax,0),IsWin=1,IsOpened=1,UpdateTime=@Time WHERE SDID = @SDID;
				--����Ʊ
				UPDATE dbo.CT_SchemeETickets SET TicketStatus = 10,WinMoney = @WinMoneyNoWithTax WHERE SchemeETicketsID = @SchemeETicketsID;
				--��¼
				INSERT dbo.CT_UsersRecord(UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,CreateTime ,RelationID ,OperatorID)
				SELECT @UserID, 5, ISNULL(@WinMoneyNoWithTax,0), (Balance+ISNULL(@WinMoneyNoWithTax,0)), '����Ʊ�н����', @Time, @SchemeETicketsID, 1 AS OperatorID FROM CT_Users WHERE UserID = @UserID;
				--�û����
				UPDATE dbo.CT_Users SET Balance =Balance + ISNULL(@WinMoneyNoWithTax,0) WHERE UserID = @UserID;
				--���ӿ����ֽ��
				UPDATE dbo.CT_UsersExtend SET WithdrawMoney = WithdrawMoney + @WinMoneyNoWithTax WHERE UserID = @UserID;
				--����
				IF @BuyType != 1
					BEGIN
					    UPDATE dbo.CT_Schemes SET SchemeStatus=14 WHERE SchemeID = @SchemeID;
					END
				
			END
		ELSE
			BEGIN
				DECLARE @SchemeStatus SMALLINT ;
			    --����Ʊ
				UPDATE dbo.CT_SchemeETickets SET TicketStatus=11 WHERE SchemeETicketsID = @SchemeETicketsID;
				--��������
				UPDATE dbo.CT_SchemesDetail SET IsWin=0,IsOpened=1,UpdateTime=@Time WHERE SDID = @SDID;
				--����
				IF @BuyType != 1
					BEGIN
						SELECT @SchemeStatus = SchemeStatus FROM dbo.CT_Schemes WHERE SchemeID = @SchemeID;
						IF	@SchemeStatus != 14
							BEGIN
								UPDATE dbo.CT_Schemes SET SchemeStatus=18 WHERE SchemeID = @SchemeID;
							END
					END
			END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	SELECT 1;
END




GO
/****** Object:  StoredProcedure [dbo].[udp_AwardActivity]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-11
-- Description:	�ӽ��ɷ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_AwardActivity]
	@Awards XML  --�ӽ�����
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @Time DATETIME = GETDATE()
	--��������Ʊ / ������ʶ / �����ʶ / �ӽ����� / �ӽ����
	DECLARE @AwardsTable TABLE(tid BIGINT ,oid BIGINT,rid INT,at INT,am BIGINT);
	INSERT INTO @AwardsTable  (tid,oid,rid,at,am) 
	SELECT 
	T.c.value('(tid/text())[1]','BIGINT'), 
	T.c.value('(oid/text())[1]','BIGINT'), 
	T.c.value('(rid/text())[1]','INT'), 
	T.c.value('(at/text())[1]','INT'), 
	T.c.value('(am/text())[1]','BIGINT')
	FROM @Awards.nodes('/ArrayOfUdv_Awards/udv_Awards') AS T(c)

	--������¼
	INSERT INTO dbo.CT_SchemesAwards( SchemeID ,SchemeETicketID ,RegularID ,AwardType ,AwardTime ,AwardMoney ,UserID)
	SELECT t.oid,t.tid,t.rid,t.at,@Time,t.am,s.InitiateUserID FROM @AwardsTable AS t 
	INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID=t.oid

	--�����¼ 20 �ٷ��ӽ� 21 ���ּӽ�
	INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
	SELECT s.InitiateUserID,(CASE t.at WHEN 0 THEN 20 ELSE 21 END) AS TradeType,t.am,u.Balance
	,(CASE t.at WHEN 0 THEN '���ֹٷ��ӽ�' ELSE '����ƽ̨�ӽ�' END) AS TradeRemark, CONVERT(VARCHAR(32),t.tid),0,@Time
	FROM @AwardsTable AS t
	INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID=t.oid
	INNER JOIN dbo.CT_Users AS u ON u.UserID=s.InitiateUserID

	--�����û����
	UPDATE u SET u.Balance = u.Balance + t.am FROM @AwardsTable AS t
	INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID = t.oid
	INNER JOIN dbo.CT_Users AS u ON u.UserID = s.InitiateUserID


	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[udp_AwardActivityAwardInterval]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-13
-- Description:	�н��������ӽ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_AwardActivityAwardInterval]
	@ActivityID INT,           --���ʶ
	@RegularID INT,            --������ʶ
	@PlayCode INT,             --�淨���
	@Min BIGINT,               --��С�ۼƽ��
	@Max BIGINT,               --����ۼƽ��
	@AwardMoney BIGINT         --�ӽ����
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @AwardsTable table(uuid bigint,sm bigint)
	DECLARE @Time DATETIME = GETDATE(),@TotalAwardMoney BIGINT /*�ۼƽ��*/,@ActivityMoney BIGINT /*����*/,@IsModify BIT /*�Ƿ���*/,@ModifyMoney BIGINT /*������*/;
	DECLARE @StartTime DATETIME /*��ʼʱ��*/,@EndTime DATETIME /*����ʱ��*/,@ActivityType INT /*��淨����*/;

	SELECT @ActivityMoney = ActivityMoney,@IsModify = IsModify,@ModifyMoney = ModifyMoney,@StartTime = StartTime,@EndTime = EndTime,@ActivityType = ActivityType
	FROM dbo.CT_Activity WHERE ActivityID = @ActivityID
	SELECT @TotalAwardMoney = TotalAwardMoney FROM dbo.CT_ActivityAward WHERE RegularID = @RegularID

	IF @IsModify = 1
		BEGIN 
			SET @ActivityMoney = @ModifyMoney
		END
	IF @ActivityMoney > @TotalAwardMoney
		BEGIN
			IF @Max = 0
				BEGIN
					insert into @AwardsTable (uuid,sm)
					select UserID,@AwardMoney from (
					select s.InitiateUserID as UserID,sum(e.WinMoney) as WinMoney from CT_Schemes as s
					inner join CT_SchemeETickets as e 
					on e.SchemeID = s.SchemeID and s.BuyType != 1 and e.TicketStatus = 10 and e.PlayCode = @PlayCode
					and s.CreateTime >= @StartTime and s.CreateTime <= @EndTime
					and s.SchemeStatus > 4 and s.SchemeStatus != 12 and s.SchemeStatus !=13 
					group by s.InitiateUserID
					) tab where tab.WinMoney>= @Min order by tab.WinMoney desc


					
				END
			ELSE
				BEGIN
					insert into @AwardsTable (uuid,sm)
					select UserID,@AwardMoney from (
					select s.InitiateUserID as UserID,sum(e.WinMoney) as WinMoney from CT_Schemes as s
					inner join CT_SchemeETickets as e 
					on e.SchemeID = s.SchemeID and s.BuyType != 1 and e.TicketStatus = 10 and e.PlayCode = @PlayCode
					and s.CreateTime >= @StartTime and s.CreateTime <= @EndTime
					and s.SchemeStatus > 4 and s.SchemeStatus != 12 and s.SchemeStatus !=13 
					group by s.InitiateUserID
					) tab where tab.WinMoney >= @Min and tab.WinMoney < @Max order by tab.WinMoney desc
				END
			
			--������¼
			INSERT INTO dbo.CT_SchemesAwards( SchemeID ,SchemeETicketID ,RegularID ,AwardType ,AwardTime ,AwardMoney ,UserID)
			SELECT 0, 0, @RegularID, @ActivityType,@Time, @AwardMoney,uuid FROM @AwardsTable 

			--�����¼ 20 �ٷ��ӽ� 21 ���ּӽ�
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
			SELECT t.uuid,(CASE @ActivityType WHEN 0 THEN 20 ELSE 21 END) AS TradeType,@AwardMoney,u.Balance
			,(CASE @ActivityType WHEN 0 THEN '���ֹٷ��ӽ�' ELSE '����ƽ̨�ӽ�' END) AS TradeRemark, CONVERT(VARCHAR(32),@RegularID),0,@Time
			FROM @AwardsTable as t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--�����û����
			UPDATE u SET u.Balance = u.Balance + @AwardMoney FROM @AwardsTable AS t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--���¼ӽ������ӽ��ۼƽ�����������ѯ
			DECLARE @TotalMoney BIGINT;
			SELECT @TotalMoney = SUM(sm) FROM @AwardsTable
			UPDATE dbo.CT_ActivityAward SET TotalAwardMoney = @TotalMoney,RegularStatus = 4 WHERE RegularID = @RegularID

		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[udp_AwardActivityAwardRanking]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-13
-- Description:	�н�����ۼ������ӽ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_AwardActivityAwardRanking]
	@ActivityID INT,           --���ʶ
	@RegularID INT,            --������ʶ
	@PlayCode INT,             --�淨���
	@Placing INT,              --����
	@AwardMoney BIGINT         --�ӽ����
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY

	DECLARE @AwardsTable table(uuid bigint,sm bigint)
	DECLARE @Time DATETIME = GETDATE(),@TotalAwardMoney BIGINT /*�ۼƽ��*/,@ActivityMoney BIGINT /*����*/,@IsModify BIT /*�Ƿ���*/,@ModifyMoney BIGINT /*������*/;
	DECLARE @StartTime DATETIME /*��ʼʱ��*/,@EndTime DATETIME /*����ʱ��*/,@ActivityType INT /*��淨����*/;

	SELECT @ActivityMoney = ActivityMoney,@IsModify = IsModify,@ModifyMoney = ModifyMoney,@StartTime = StartTime,@EndTime = EndTime,@ActivityType = ActivityType
	FROM dbo.CT_Activity WHERE ActivityID = @ActivityID
	SELECT @TotalAwardMoney = TotalAwardMoney FROM dbo.CT_ActivityAward WHERE RegularID = @RegularID

	IF @IsModify = 1
		BEGIN 
			SET @ActivityMoney = @ModifyMoney
		END
	IF @ActivityMoney > @TotalAwardMoney
		BEGIN
			insert into @AwardsTable (uuid ,sm)
			select UserID,@AwardMoney from (
			select rank() over(order by sum(e.WinMoney) desc) Placing,s.InitiateUserID as UserID,sum(e.WinMoney) as WinMoney from CT_Schemes as s
			inner join CT_SchemeETickets as e 
			on e.SchemeID = s.SchemeID and s.BuyType != 1 and e.PlayCode = @PlayCode
			and s.CreateTime >= @StartTime and s.CreateTime <= @EndTime
			and s.SchemeStatus > 4 and s.SchemeStatus != 12 and s.SchemeStatus !=13 
			group by s.InitiateUserID
			) tab where tab.Placing = @Placing

			--������¼
			INSERT INTO dbo.CT_SchemesAwards( SchemeID ,SchemeETicketID ,RegularID ,AwardType ,AwardTime ,AwardMoney ,UserID)
			SELECT 0, 0, @RegularID, @ActivityType,@Time, @AwardMoney,uuid FROM @AwardsTable 

			--�����¼ 20 �ٷ��ӽ� 21 ���ּӽ�
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
			SELECT t.uuid,(CASE @ActivityType WHEN 0 THEN 20 ELSE 21 END) AS TradeType,@AwardMoney,u.Balance
			,(CASE @ActivityType WHEN 0 THEN '���ֹٷ��ӽ�' ELSE '����ƽ̨�ӽ�' END) AS TradeRemark, CONVERT(VARCHAR(32),@RegularID),0,@Time
			FROM @AwardsTable as t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--�����û����
			UPDATE u SET u.Balance = u.Balance + @AwardMoney FROM @AwardsTable AS t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--���¼ӽ������ӽ��ۼƽ�����������ѯ
			DECLARE @TotalMoney BIGINT;
			SELECT @TotalMoney = SUM(sm) FROM @AwardsTable
			UPDATE dbo.CT_ActivityAward SET TotalAwardMoney = @TotalMoney,RegularStatus = 4 WHERE RegularID = @RegularID


		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[udp_AwardActivityBetInterval]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-13
-- Description:	Ͷע����ۼ�����ӽ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_AwardActivityBetInterval]
	@ActivityID INT,           --���ʶ
	@RegularID INT,            --������ʶ
	@PlayCode INT,             --�淨���
	@Min BIGINT,               --��С�ۼƽ��
	@Max BIGINT,               --����ۼƽ��
	@AwardMoney BIGINT         --�ӽ����
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	
	DECLARE @AwardsTable table(uuid bigint,sm bigint)
	DECLARE @Time DATETIME = GETDATE(),@TotalAwardMoney BIGINT /*�ۼƽ��*/,@ActivityMoney BIGINT /*����*/,@IsModify BIT /*�Ƿ���*/,@ModifyMoney BIGINT /*������*/;
	DECLARE @StartTime DATETIME /*��ʼʱ��*/,@EndTime DATETIME /*����ʱ��*/,@ActivityType INT /*��淨����*/;

	SELECT @ActivityMoney = ActivityMoney,@IsModify = IsModify,@ModifyMoney = ModifyMoney,@StartTime = StartTime,@EndTime = EndTime,@ActivityType = ActivityType
	FROM dbo.CT_Activity WHERE ActivityID = @ActivityID
	SELECT @TotalAwardMoney = TotalAwardMoney FROM dbo.CT_ActivityAward WHERE RegularID = @RegularID

	IF @IsModify = 1
		BEGIN 
			SET @ActivityMoney = @ModifyMoney
		END
	IF @ActivityMoney > @TotalAwardMoney
		BEGIN
			IF @Max = 0
				BEGIN
					insert into @AwardsTable (uuid,sm)
					select UserID,@AwardMoney from (
					select s.InitiateUserID as UserID,sum(s.SchemeMoney) as SchemeMoney from CT_Schemes as s
					inner join CT_SchemeETickets as e 
					on e.SchemeID = s.SchemeID and s.BuyType != 1 and e.PlayCode = @PlayCode
					and s.CreateTime >= @StartTime and s.CreateTime <= @EndTime
					and s.SchemeStatus > 4 and s.SchemeStatus != 12 and s.SchemeStatus !=13 
					group by s.InitiateUserID
					) tab where tab.SchemeMoney>= @Min order by tab.SchemeMoney desc
				END
			ELSE
				BEGIN
					insert into @AwardsTable (uuid,sm)
					select UserID,@AwardMoney from (
					select s.InitiateUserID as UserID,sum(s.SchemeMoney) as SchemeMoney from CT_Schemes as s
					inner join CT_SchemeETickets as e 
					on e.SchemeID = s.SchemeID and s.BuyType != 1 and e.PlayCode = @PlayCode
					and s.CreateTime >= @StartTime and s.CreateTime <= @EndTime
					and s.SchemeStatus > 4 and s.SchemeStatus != 12 and s.SchemeStatus !=13 
					group by s.InitiateUserID
					) tab where tab.SchemeMoney >= @Min and tab.SchemeMoney < @Max order by tab.SchemeMoney desc
				END

			--������¼
			INSERT INTO dbo.CT_SchemesAwards( SchemeID ,SchemeETicketID ,RegularID ,AwardType ,AwardTime ,AwardMoney ,UserID)
			SELECT 0, 0, @RegularID, @ActivityType,@Time, @AwardMoney,uuid FROM @AwardsTable 

			--�����¼ 20 �ٷ��ӽ� 21 ���ּӽ�
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
			SELECT t.uuid,(CASE @ActivityType WHEN 0 THEN 20 ELSE 21 END) AS TradeType,@AwardMoney,u.Balance
			,(CASE @ActivityType WHEN 0 THEN '���ֹٷ��ӽ�' ELSE '����ƽ̨�ӽ�' END) AS TradeRemark, CONVERT(VARCHAR(32),@RegularID),0,@Time
			FROM @AwardsTable as t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--�����û����
			UPDATE u SET u.Balance = u.Balance + @AwardMoney FROM @AwardsTable AS t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--���¼ӽ������ӽ��ۼƽ�����������ѯ
			DECLARE @TotalMoney BIGINT;
			SELECT @TotalMoney = SUM(sm) FROM @AwardsTable
			UPDATE dbo.CT_ActivityAward SET TotalAwardMoney = @TotalMoney,RegularStatus = 4 WHERE RegularID = @RegularID

		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[udp_AwardActivityBetRanking]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-13
-- Description:	Ͷע����ۼ������ӽ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_AwardActivityBetRanking]
	@ActivityID INT,           --���ʶ
	@RegularID INT,            --������ʶ
	@PlayCode INT,             --�淨���
	@Placing INT,              --����
	@AwardMoney BIGINT         --�ӽ����
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY

	DECLARE @AwardsTable table(uuid bigint,sm bigint)
	DECLARE @Time DATETIME = GETDATE(),@TotalAwardMoney BIGINT /*�ۼƽ��*/,@ActivityMoney BIGINT /*����*/,@IsModify BIT /*�Ƿ���*/,@ModifyMoney BIGINT /*������*/;
	DECLARE @StartTime DATETIME /*��ʼʱ��*/,@EndTime DATETIME /*����ʱ��*/,@ActivityType INT /*��淨����*/;

	SELECT @ActivityMoney = ActivityMoney,@IsModify = IsModify,@ModifyMoney = ModifyMoney,@StartTime = StartTime,@EndTime = EndTime,@ActivityType = ActivityType
	FROM dbo.CT_Activity WHERE ActivityID = @ActivityID
	SELECT @TotalAwardMoney = TotalAwardMoney FROM dbo.CT_ActivityAward WHERE RegularID = @RegularID

	IF @IsModify = 1
		BEGIN 
			SET @ActivityMoney = @ModifyMoney
		END
	IF @ActivityMoney > @TotalAwardMoney
		BEGIN
			insert into @AwardsTable (uuid ,sm)
			select UserID,@AwardMoney from (
			select rank() over(order by sum(s.SchemeMoney) desc) Placing,s.InitiateUserID as UserID,sum(s.SchemeMoney) as SchemeMoney from CT_Schemes as s
			inner join CT_SchemeETickets as e 
			on e.SchemeID = s.SchemeID and s.BuyType != 1 and e.PlayCode = @PlayCode
			and s.CreateTime >= @StartTime and s.CreateTime <= @EndTime
			and s.SchemeStatus > 4 and s.SchemeStatus != 12 and s.SchemeStatus !=13 
			group by s.InitiateUserID
			) tab where tab.Placing = @Placing

			--������¼
			INSERT INTO dbo.CT_SchemesAwards( SchemeID ,SchemeETicketID ,RegularID ,AwardType ,AwardTime ,AwardMoney ,UserID)
			SELECT 0, 0, @RegularID, @ActivityType,@Time, @AwardMoney,uuid FROM @AwardsTable 

			--�����¼ 20 �ٷ��ӽ� 21 ���ּӽ�
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
			SELECT t.uuid,(CASE @ActivityType WHEN 0 THEN 20 ELSE 21 END) AS TradeType,@AwardMoney,u.Balance
			,(CASE @ActivityType WHEN 0 THEN '���ֹٷ��ӽ�' ELSE '����ƽ̨�ӽ�' END) AS TradeRemark, CONVERT(VARCHAR(32),@RegularID),0,@Time
			FROM @AwardsTable as t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--�����û����
			UPDATE u SET u.Balance = u.Balance + @AwardMoney FROM @AwardsTable AS t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--���¼ӽ������ӽ��ۼƽ�����������ѯ
			DECLARE @TotalMoney BIGINT;
			SELECT @TotalMoney = SUM(sm) FROM @AwardsTable
			UPDATE dbo.CT_ActivityAward SET TotalAwardMoney = @TotalMoney,RegularStatus = 4 WHERE RegularID = @RegularID


		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[udp_AwardActivityChase]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-11
-- Description:	׷�żӽ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_AwardActivityChase]
	@RegularID INT,       --�淨�����ʶ
	@AwardType INT,       --�ӽ�����
	@AwardMoney BIGINT,   --�ӽ����
	@StartTime DATETIME,  --�ӽ���ʼʱ��
	@EndTime DATETIME,    --�ӽ�����ʱ��
	@RChaseType INT,      --�н����ͣ�1 �ۼ��н�������2 �ۼ��н����
	@Unit BIGINT          --��λ����������
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @Time DATETIME = GETDATE();
	IF @RChaseType = 2
		BEGIN
			-- ������ʶ / �����ʶ / �ӽ����� / �ӽ���� / ���ӽ��û�
			DECLARE @AwardsTable TABLE(oid BIGINT,rid INT,at INT,am BIGINT,uuid BIGINT);
			INSERT INTO @AwardsTable (oid, rid ,at ,am ,uuid)
			SELECT s.SchemeID,@RegularID,@AwardType,@AwardMoney,s.InitiateUserID
			FROM dbo.CT_Schemes AS s 
			INNER JOIN dbo.CT_SchemeETickets AS e ON e.SchemeID=s.SchemeID AND s.BuyType = 1 
			AND s.CreateTime>=@StartTime AND s.CreateTime <= @EndTime AND e.TicketStatus = 10
			GROUP BY s.SchemeID,s.InitiateUserID
			--���Դ�ӡ����
			--PRINT @AwardsTable

			--������¼ �����������ɽ�
			INSERT INTO dbo.CT_SchemesAwards(SchemeID ,SchemeETicketID ,RegularID ,AwardType ,AwardTime ,AwardMoney ,UserID)
			SELECT oid, 0, rid , at, @Time, am,uuid FROM @AwardsTable WHERE am >= @Unit

			--�����¼ 20 �ٷ��ӽ� 21 ���ּӽ�
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
			SELECT t.uuid,(CASE t.at WHEN 0 THEN 20 ELSE 21 END) AS TradeType, t.am,s.Balance,
			(CASE t.at WHEN 0 THEN '���ֹٷ��ӽ�' ELSE '����ƽ̨�ӽ�' END) AS TradeRemark, CONVERT(VARCHAR(32),t.oid),0,@Time FROM @AwardsTable AS t
			INNER JOIN dbo.CT_Users AS s ON s.UserID = t.uuid
			WHERE t.am >= @Unit

			--�����û����
			UPDATE u SET u.Balance = u.Balance + t.am FROM @AwardsTable AS t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid
			WHERE t.am >= @Unit

		END
	ELSE IF @RChaseType = 1
		BEGIN
			-- ������ʶ / �����ʶ / �ӽ����� / �ӽ���� / ���ӽ��û� / �ۼ��н�����
			DECLARE @AwardsTableCount TABLE(oid BIGINT,rid INT,at INT,am BIGINT,uuid BIGINT,cts INT);

			INSERT INTO @AwardsTableCount(oid ,rid ,at ,am ,uuid ,cts)
			SELECT s.SchemeID,@RegularID AS RegularID,@AwardType AS AwardType,@AwardMoney,s.InitiateUserID,COUNT(e.SchemeETicketsID) AS Wins
			FROM dbo.CT_Schemes AS s 
			INNER JOIN dbo.CT_SchemeETickets AS e ON e.SchemeID=s.SchemeID AND s.BuyType = 1 
			AND s.CreateTime>=@StartTime AND s.CreateTime <= @EndTime AND e.TicketStatus = 10
			GROUP BY s.SchemeID,s.InitiateUserID

			--������¼ �����������ɽ�
			INSERT INTO dbo.CT_SchemesAwards(SchemeID ,SchemeETicketID ,RegularID ,AwardType ,AwardTime ,AwardMoney ,UserID)
			SELECT oid, 0, rid , at, @Time, am,uuid FROM @AwardsTableCount WHERE cts = @Unit

			--�����¼ 20 �ٷ��ӽ� 21 ���ּӽ�
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
			SELECT t.uuid,(CASE t.at WHEN 0 THEN 20 ELSE 21 END) AS TradeType, t.am,s.Balance,
			(CASE t.at WHEN 0 THEN '���ֹٷ��ӽ�' ELSE '����ƽ̨�ӽ�' END) AS TradeRemark, CONVERT(VARCHAR(32),t.oid),0,@Time FROM @AwardsTableCount AS t
			INNER JOIN dbo.CT_Users AS s ON s.UserID = t.uuid
			WHERE t.cts = @Unit

			--�����û����
			UPDATE u SET u.Balance = u.Balance + t.am FROM @AwardsTableCount AS t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid
			WHERE t.cts = @Unit
		END

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[udp_AwardActivityTopLimit]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-14
-- Description:	Ͷע������޼ӽ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_AwardActivityTopLimit]
	@ActivityID INT,           --���ʶ
	@RegularID INT,            --������ʶ
	@PlayCode INT,             --�淨���
	@AwardMoney BIGINT,        --�ӽ����
	@TotalMoney BIGINT         --���޽��
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY

	DECLARE @AwardsTable table(uuid bigint,sm bigint)
	DECLARE @Time DATETIME = GETDATE(),@TotalAwardMoney BIGINT /*�ۼƽ��*/,@ActivityMoney BIGINT /*����*/,@IsModify BIT /*�Ƿ���*/,@ModifyMoney BIGINT /*������*/;
	DECLARE @StartTime DATETIME /*��ʼʱ��*/,@EndTime DATETIME /*����ʱ��*/,@ActivityType INT /*��淨����*/;

	SELECT @ActivityMoney = ActivityMoney,@IsModify = IsModify,@ModifyMoney = ModifyMoney,@StartTime = StartTime,@EndTime = EndTime,@ActivityType = ActivityType
	FROM dbo.CT_Activity WHERE ActivityID = @ActivityID
	SELECT @TotalAwardMoney = TotalAwardMoney FROM dbo.CT_ActivityAward WHERE RegularID = @RegularID

	IF @IsModify = 1
		BEGIN 
			SET @ActivityMoney = @ModifyMoney
		END

		IF @ActivityMoney > @TotalAwardMoney
		BEGIN
			INSERT INTO @AwardsTable (uuid ,sm)
			SELECT UserID,@AwardMoney FROM (
			SELECT s.InitiateUserID AS UserID,SUM(s.SchemeMoney) AS SchemeMoney FROM CT_Schemes AS s
			INNER JOIN CT_SchemeETickets AS e 
			ON e.SchemeID = s.SchemeID AND s.BuyType != 1 AND e.PlayCode = @PlayCode
			AND s.CreateTime >= @StartTime AND s.CreateTime <= @EndTime
			AND s.SchemeStatus > 4 AND s.SchemeStatus != 12 AND s.SchemeStatus !=13 
			GROUP BY s.InitiateUserID
			) tab WHERE tab.SchemeMoney >= @TotalMoney

			--������¼
			INSERT INTO dbo.CT_SchemesAwards( SchemeID ,SchemeETicketID ,RegularID ,AwardType ,AwardTime ,AwardMoney ,UserID)
			SELECT 0, 0, @RegularID, @ActivityType,@Time, @AwardMoney,uuid FROM @AwardsTable 

			--�����¼ 20 �ٷ��ӽ� 21 ���ּӽ�
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
			SELECT t.uuid,(CASE @ActivityType WHEN 0 THEN 20 ELSE 21 END) AS TradeType,@AwardMoney,u.Balance
			,(CASE @ActivityType WHEN 0 THEN '���ֹٷ��ӽ�' ELSE '����ƽ̨�ӽ�' END) AS TradeRemark, CONVERT(VARCHAR(32),@RegularID),0,@Time
			FROM @AwardsTable AS t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--�����û����
			UPDATE u SET u.Balance = u.Balance + @AwardMoney FROM @AwardsTable AS t
			INNER JOIN dbo.CT_Users AS u ON u.UserID = t.uuid

			--���¼ӽ������ӽ��ۼƽ�����������ѯ
			DECLARE @TotalMoneys BIGINT;
			SELECT @TotalMoneys = SUM(sm) FROM @AwardsTable
			UPDATE dbo.CT_ActivityAward SET TotalAwardMoney = @TotalMoneys,RegularStatus = 4 WHERE RegularID = @RegularID

		END

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[udp_BackstagePay]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--��̨��ֵ
--2017-6-2
CREATE PROC [dbo].[udp_BackstagePay]
	@AdminID INT,
	@UserName varchar(32),
	@OrderNo VARCHAR(32),
	@Money BIGINT,
	@Memo VARCHAR(1024),
	@ReturnValue BIGINT OUTPUT,
	@ReturnDescription varchar(100) OUTPUT
	--WITH ENCRYPTION
AS
	DECLARE @UserID INT, @PayID BIGINT, @Balance BIGINT
	SELECT @UserID=UserID, @Balance = Balance FROM dbo.CT_Users WHERE UserName=@UserName	
	IF @@ROWCOUNT = 0
	BEGIN
		SELECT @ReturnValue = -1, @ReturnDescription = '�û�������'	
		RETURN -1
	END  
	IF @Money < 0 AND @Balance < ABS(@Money)
	BEGIN
		SELECT @ReturnValue = -2, @ReturnDescription = '�ۼ����ܴ������'	
		RETURN -2
	END

	BEGIN TRAN
		INSERT CT_UsersPayDetail(UserID, OrderNo, RechargeNo, PayType, Amount, FormalitiesFees, Result, CreateTime, CompleteTime)
			VALUES(@UserID, @OrderNo, '', '����', @Money, 0, 1,GETDATE(), GETDATE())			
		IF @@rowcount = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -3, @ReturnDescription = '��ӳ�ֵ��¼ʧ��'	
			RETURN -3
		END
		SET @PayID = @@IDENTITY

		UPDATE dbo.CT_Users SET Balance = Balance + @Money WHERE UserID = @UserID
		IF @@rowcount = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -4, @ReturnDescription = '�����û����ʧ��'	
			RETURN -4
		END
		
		INSERT dbo.CT_UsersRecord (UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,CreateTime ,RelationID ,OperatorID)
		VALUES(@UserID, 0, @Money, (@Balance+@Money), @Memo, GETDATE(), @OrderNo, @AdminID)	
		IF @@rowcount = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -5, @ReturnDescription = '��ӽ�ұ仯��¼ʧ��'	
			RETURN -5
		END
			BEGIN
		SELECT @ReturnValue = @UserID, @ReturnDescription = '��̨�ֹ���ֵ�ɹ�'	
		COMMIT TRAN
	RETURN 0

	
GO
/****** Object:  StoredProcedure [dbo].[udp_BettingRevoke]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joan
-- Create date: 2017-11-27
-- Description:	Ͷעʧ�ܳ���(�洢�����Ż�)
-- =============================================
CREATE PROC [dbo].[udp_BettingRevoke]
	@SchemeID  BIGINT,
	@HandleDescribe VARCHAR(100)='',
	@ReturnValue  INT OUTPUT,
	@ReturnDescription VARCHAR(100) OUTPUT
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		DECLARE @UserID BIGINT, @SchemeMoney BIGINT=0, @OrderStatus SMALLINT,@BetError INT,@BetSuccess INT,@BuyType TINYINT,@SchemeCount INT,@Balance BIGINT,@Time DATETIME = GETDATE(),@GoldBean BIGINT;
		--��ѯ
		SELECT @SchemeCount = COUNT(1) FROM dbo.CT_Schemes WHERE SchemeID = @SchemeID;

		IF @SchemeCount = 0
			BEGIN
			    SELECT @ReturnValue=-1, @ReturnDescription='�����ţ�'+@SchemeID+'������,����ʧ��';
			END
		ELSE
			BEGIN
				IF @OrderStatus = 12  --�������Ѿ�����ֻҪ���µ���Ʊ״̬���������˿�
					BEGIN
						SELECT @ReturnValue=0, @ReturnDescription='�����ɹ�';
					END
				ELSE
					BEGIN
					    SELECT @UserID =  InitiateUserID, @OrderStatus=SchemeStatus, @BuyType = BuyType FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID;
						-- �������
						SELECT @SchemeMoney = TradeAmount FROM CT_UsersRecord WHERE UserID=@UserID AND TradeType=1 AND RelationID=CONVERT(VARCHAR(32),@SchemeID);

						--����Ͷע�ɹ��Ķ��� Start
						UPDATE CT_SchemeETickets SET TicketStatus=1 WHERE SchemeID=@SchemeID AND (HandleDescribe='Ͷע�ɹ�,' or TicketStatus=0);
						--��ѯ�ɹ��ĵ���Ʊ
						SELECT @BetSuccess = COUNT(*) FROM CT_SchemeETickets WHERE SchemeID=@SchemeID AND TicketStatus=1;
						--��ѯʧ�ܵĵ���Ʊ
						SELECT @BetError = COUNT(*) FROM CT_SchemeETickets WHERE SchemeID=@SchemeID AND TicketStatus=3;
						--��֤
						IF @BetSuccess > 0 AND @BetError > 0 AND @BuyType != 1
							BEGIN
							    UPDATE dbo.CT_Schemes SET SchemeStatus=8 WHERE SchemeID=@SchemeID;
							END
						ELSE
							BEGIN
								--���µ���Ʊ״̬
							    UPDATE dbo.CT_SchemeETickets SET TicketStatus = 3, HandleDescribe=HandleDescribe+','+@HandleDescribe, HandleDateTime=@Time WHERE SchemeID=@SchemeID;
								IF	@BuyType != 1
									BEGIN
									    --����Ʊ״̬����
										UPDATE dbo.CT_Schemes SET SchemeStatus=12, Describe=Describe+','+@HandleDescribe WHERE SchemeID = @SchemeID;
									END
								--�����û����
								UPDATE dbo.CT_Users SET Balance = Balance+@SchemeMoney WHERE UserID = @UserID;
								SELECT @Balance =Balance FROM dbo.CT_Users WHERE UserID = @UserID;
								--������¼
								INSERT dbo.CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
								SELECT UserID,12,TradeAmount,@Balance,'ϵͳ����',@Time,CONVERT(VARCHAR(32),@SchemeID),1 FROM CT_UsersRecord 
								WHERE UserID=@UserID AND RelationID = CONVERT(VARCHAR(32),@SchemeID) AND TradeType = 1;

								--��ȯ������¼
								INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
								SELECT UserID ,23 ,TradeAmount ,Balance + TradeAmount ,'ϵͳ����:����ȯ' ,RelationID ,OperatorID ,@Time ,CouponsID FROM CT_UsersRecord 
								WHERE UserID=@UserID AND TradeType=22 AND RelationID=CONVERT(VARCHAR(32),@SchemeID);
								----��������ȯ
								UPDATE r SET r.Balance = r.Balance + u.TradeAmount  FROM CT_UsersRecord AS u
								INNER JOIN CaileCoupons.dbo.CT_Coupons AS r 
								ON r.UserID=u.UserID AND u.RelationID=CONVERT(VARCHAR(32),@SchemeID)
								AND u.TradeType=22 AND r.CouponsID = u.CouponsID;

								--�ʶ�����
								SELECT @GoldBean = GoldBean FROM dbo.CT_Users WHERE UserID = @UserID;
								-- �ʶ�������¼
								INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
								SELECT UserID ,25 ,TradeAmount ,@GoldBean,'ϵͳ����:���ʶ�' ,RelationID ,OperatorID ,@Time ,CouponsID FROM CT_UsersRecord 
								WHERE UserID=@UserID AND TradeType=24 AND RelationID=CONVERT(VARCHAR(32),@SchemeID);
								--���³����ʶ�
								UPDATE s SET s.GoldBean = s.GoldBean + r.TradeAmount FROM dbo.CT_Users AS s
								INNER JOIN dbo.CT_UsersRecord AS r ON r.UserID = s.UserID AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID) AND r.TradeType = 24;
							END
					END
			END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT @ReturnValue=-3, @ReturnDescription='����ʧ��'
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	SELECT @ReturnValue=0, @ReturnDescription='�����ɹ�'
	RETURN 0
END

GO
/****** Object:  StoredProcedure [dbo].[udp_ChaseRevoke]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--δ���׷�ų���
--2017-5-26
CREATE PROC [dbo].[udp_ChaseRevoke]
	@SchemeID BIGINT,	--������ID
	@ChaseTaskDetailID BIGINT,	--׷������ID
	@UserID BIGINT,	--�û����
	@Amount BIGINT  --���
	
--WITH ENCRYPTION
AS
	UPDATE dbo.CT_ChaseTaskDetails SET IsExecuted=1,QuashStatus=2 WHERE SchemeID=@SchemeID AND ID=@ChaseTaskDetailID
	IF @@ROWCOUNT = 0
		BEGIN
			RETURN -1
		END	
	UPDATE dbo.CT_Users SET Balance = Balance + @Amount WHERE UserID=@UserID
	IF @@ROWCOUNT = 0
		BEGIN
			RETURN -2
		END	
	INSERT dbo.CT_UsersRecord(UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,CreateTime ,RelationID ,OperatorID)
	SELECT @UserID, 13, @Amount, Balance, '׷�ų���', GETDATE(), @ChaseTaskDetailID, 1 AS OperatorID FROM CT_Users WHERE UserID = @UserID
	IF @@ROWCOUNT = 0
		BEGIN
			RETURN -3
		END
	RETURN 0	


GO
/****** Object:  StoredProcedure [dbo].[udp_CheckingReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--���˱���
--2017-08-08
create PROCEDURE [dbo].[udp_CheckingReport]
	@Day varchar(20)
AS
	SET NOCOUNT ON;
	select ur.TradeType,sum(ur.TradeAmount) as TradeAmount from CT_UsersRecord as ur 
	inner join CT_Users u on u.UserID=ur.UserID
	where convert(date,CreateTime)=convert(date,@Day)
	group by ur.TradeType
	RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[udp_CollectOpenPrizeRecord]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--��Ӳɼ������ݼ�¼
--2017-5-9
CREATE PROC [dbo].[udp_CollectOpenPrizeRecord]
	@LotteryCode	INT,			--���ֱ��
	@IsuseName	VARCHAR(20),	--�ں���
	@OpenNumber	VARCHAR(100),	--��������
	@StartTime	DATETIME,		--��ʼʱ��
	@EndTime	DATETIME,		--��ֹʱ��
	@OpenTime	DATETIME,		--����ʱ��
	@ReturnValue	INT OUTPUT,
	@ReturnDescription VARCHAR(100) OUTPUT
	--WITH ENCRYPTION
AS
	SELECT @ReturnValue=0, @ReturnDescription=''
	
	DECLARE @IsOpened BIT	--�Ƿ񿪽���
	DECLARE @IsuseID INT=0
	
	SELECT @IsuseID=IsuseID, @IsOpened=IsOpened FROM CT_Isuses WHERE LotteryCode=@LotteryCode AND IsuseName=@IsuseName
	IF @@ROWCOUNT > 0
	BEGIN
		IF @IsOpened = 0 AND LEN(@OpenNumber) > 0
		BEGIN
			UPDATE CT_Isuses SET OpenNumber=@OpenNumber, OpenTime=@OpenTime, UpdateTime=GETDATE(), IsuseState=4, IsOpened=1,
				StartTime=CASE WHEN @StartTime is NOT NULL THEN StartTime ELSE @StartTime END, 
				EndTime=CASE WHEN @EndTime is NOT NULL THEN EndTime ELSE @EndTime END
			WHERE IsuseID=@IsuseID
		END
	END
	ELSE
	BEGIN
		INSERT CT_Isuses(LotteryCode, IsuseName, StartTime, EndTime, IsExecuteChase, IsOpened, OpenNumber, OpenOperatorID, IsuseState, UpdateTime, OpenTime)
		VALUES(@LotteryCode, @IsuseName, @StartTime, @EndTime, 0, CASE WHEN LEN(@OpenNumber)>0 THEN 1 ELSE 0 END, @OpenNumber, 1, 4, GETDATE(), @OpenTime)
		SET @IsuseID =@@IDENTITY
	END
	--������3ʱ��ά��
	IF @LotteryCode = 102
	BEGIN
		IF @StartTime IS NOT NULL AND @EndTime IS NOT NULL
		BEGIN
			DECLARE @PreviousStartTime DATETIME, @interval INT
			SELECT TOP(1) @PreviousStartTime=StartTime FROM dbo.CT_Isuses WHERE DATEDIFF(d,GETDATE(),StartTime)=0 
				AND IsuseID<@IsuseID AND LotteryCode=@LotteryCode AND OpenNumber<>'' 
			ORDER BY IsuseID DESC
			SET @interval = DATEDIFF(SECOND,@PreviousStartTime,@StartTime) - 600 --���Խӿڣ�589 ��ʽ�ӿڣ�600
			IF @interval < 0
			BEGIN
				UPDATE dbo.CT_Isuses SET StartTime = DATEADD(SECOND,@interval,StartTime), EndTime = DATEADD(SECOND,@interval,EndTime) 
				WHERE DATEDIFF(d,GETDATE(),StartTime)=0 AND LotteryCode=@LotteryCode AND OpenNumber='' 
			END			
		END
	END

	SELECT @ReturnValue=@IsuseID, @ReturnDescription='��ӿ�����Ϣ�ɹ�'
	RETURN @IsuseID


GO
/****** Object:  StoredProcedure [dbo].[udp_DealOutTicket]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--�������Ʊ��Ʊ״̬
--2017-5-10
CREATE PROC [dbo].[udp_DealOutTicket]
	@SchemeETicketsID	BIGINT,
	@TicketStatus TINYINT,
	@Ticket	VARCHAR(128)
	--WITH ENCRYPTION
AS
	DECLARE @SchemeMoney INT, @UserID BIGINT, @SchemeID BIGINT,@ChaseTaskDetailsID BIGINT,@IsExecuted BIT,@BuyType TINYINT,@TicketsCount INT;
	
	BEGIN TRAN
	SELECT @ChaseTaskDetailsID = ChaseTaskDetailsID,@SchemeID=SchemeID FROM dbo.CT_SchemeETickets WHERE SchemeETicketsID=@SchemeETicketsID
	SELECT @SchemeMoney=SUM(SchemeMoney) FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
	
	UPDATE dbo.CT_SchemeETickets SET TicketStatus=@TicketStatus, Ticket=@Ticket WHERE SchemeETicketsID=@SchemeETicketsID
	IF @@rowcount = 0
	BEGIN
		ROLLBACK TRAN
		--SELECT @ReturnValue = -1, @ReturnDescription = '���µ���Ʊʧ��'	
		RETURN -1
	END
	
	--��֤�Ƿ�ִ��׷��
	IF @ChaseTaskDetailsID > 0 
		BEGIN
			SELECT @IsExecuted = IsExecuted  FROM CT_ChaseTaskDetails WHERE ID = @ChaseTaskDetailsID
			IF @IsExecuted = 0
				BEGIN
					UPDATE dbo.CT_ChaseTaskDetails SET IsExecuted = 1 WHERE ID = @ChaseTaskDetailsID
					IF @@rowcount = 0
						BEGIN
							ROLLBACK TRAN
							--SELECT @ReturnValue = -1, @ReturnDescription = '���µ���Ʊʧ��'	
							RETURN -1
						END
				END
		END
	

	IF @TicketStatus = 4 --��Ʊʧ�ܽ����˿�
	BEGIN
		IF @BuyType != 1
			BEGIN
				SELECT @UserID=InitiateUserID FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
				
				UPDATE dbo.CT_Users SET Balance = Balance+@SchemeMoney WHERE UserID = @UserID
				IF @@rowcount = 0
				BEGIN
					ROLLBACK TRAN
					--SELECT @ReturnValue = -2, @ReturnDescription = '�����û����ʧ��'	
					RETURN -2
				END
				INSERT dbo.CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
					SELECT @UserID, 15, @SchemeMoney, (Balance-@SchemeMoney), '��Ʊʧ���˿�', GETDATE(), @SchemeID, 1 FROM dbo.CT_Users WHERE UserID=@UserID
				IF @@rowcount = 0
				BEGIN
					ROLLBACK TRAN
					--SELECT @ReturnValue = -3, @ReturnDescription = '��¼��Ʊʧ���˿��¼ʧ��'	
					RETURN -3
				END
			END
		ELSE IF @BuyType = 1
			BEGIN
				SELECT @UserID=InitiateUserID FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
				--��ѯ�Ƿ�������Ʊ״̬
				SELECT @TicketsCount = COUNT(SchemeETicketsID) FROM CT_SchemeETickets WHERE ChaseTaskDetailsID = @ChaseTaskDetailsID AND TicketStatus IN (0,1)--(0,1,2)
				
				SELECT @SchemeMoney=SUM(Amount) from dbo.CT_ChaseTaskDetails WHERE ID = @ChaseTaskDetailsID
				
				UPDATE dbo.CT_Users SET Balance = Balance+@SchemeMoney WHERE UserID = @UserID
				IF @@rowcount = 0
				BEGIN
					ROLLBACK TRAN
					--SELECT @ReturnValue = -2, @ReturnDescription = '�����û����ʧ��'	
					RETURN -2
				END
				INSERT dbo.CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
					SELECT @UserID, 15, @SchemeMoney, (Balance-@SchemeMoney), '��Ʊʧ���˿�', GETDATE(), @SchemeID, 1 FROM dbo.CT_Users WHERE UserID=@UserID
				IF @@rowcount = 0
				BEGIN
					ROLLBACK TRAN
					--SELECT @ReturnValue = -3, @ReturnDescription = '��¼��Ʊʧ���˿��¼ʧ��'	
					RETURN -3
				END
				--ȫ��Ʊʧ�������׷�ų���
				IF @TicketsCount = 0
					BEGIN
						UPDATE dbo.CT_ChaseTaskDetails SET QuashStatus = 2 WHERE ID = @ChaseTaskDetailsID
						IF @@rowcount = 0
						BEGIN
							ROLLBACK TRAN
							--SELECT @ReturnValue = -3, @ReturnDescription = '����׷������ʧ��'	
							RETURN -4
						END
					END
			END
	END
	COMMIT TRAN
	
	--SELECT @ReturnValue=0, @ReturnDescription='�����ɹ�'
	RETURN 0



GO
/****** Object:  StoredProcedure [dbo].[udp_EditOrderStatus]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--�༭����״̬
--2017-5-31
CREATE PROCEDURE [dbo].[udp_EditOrderStatus]	
	@SchemeID BIGINT,
	@WinType INT
    --WITH ENCRYPTION
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @BuyType BIGINT = 0,@Count INT = 0,@WinMoneyNoWithTax BIGINT,@LotteryCode INT,@SchemeStatus SMALLINT;
	SELECT @BuyType = BuyType,@LotteryCode=LotteryCode FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
	IF @BuyType = 1
		BEGIN
			SELECT @Count=COUNT(*) FROM dbo.CT_ChaseTaskDetails WHERE SchemeID=@SchemeID AND IsSendOut=0;
			IF @Count = 0
				BEGIN
					UPDATE dbo.CT_Schemes SET SchemeStatus=20 WHERE SchemeID = @SchemeID;
				END 
		END
	ELSE
		BEGIN
			SELECT @WinMoneyNoWithTax = ISNULL(SUM(WinMoney),0) FROM dbo.CT_SchemeETickets WHERE SchemeID = @SchemeID;
			IF @WinMoneyNoWithTax > 0
				BEGIN
					UPDATE dbo.CT_Schemes SET SchemeStatus=14 WHERE SchemeID=@SchemeID;
				END
			ELSE
				BEGIN
					SELECT @SchemeStatus = SchemeStatus FROM dbo.CT_Schemes WHERE SchemeID = @SchemeID;
					IF @SchemeStatus != 14
						BEGIN
							UPDATE dbo.CT_Schemes SET SchemeStatus=18 WHERE SchemeID=@SchemeID;
						END
				END
		END

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			
			SELECT -1;

            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END




GO
/****** Object:  StoredProcedure [dbo].[udp_EnteringDrawResults]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--¼�뿪�����
--2017-08-22
CREATE PROC [dbo].[udp_EnteringDrawResults]
	@LotteryCode	INT,	    --���ֱ��
	@IsuseName	VARCHAR(20),	--�ں���
	@OpenNumber	VARCHAR(100),	--��������
	@OpenTime	DATETIME,		--����ʱ��
	@ReturnValue	BIGINT OUTPUT,
	@ReturnDescription VARCHAR(100) OUTPUT
	--WITH ENCRYPTION
AS
	SELECT @ReturnValue=0, @ReturnDescription=''
	
	DECLARE @IsOpened BIT	--�Ƿ񿪽���
	DECLARE @IsuseID INT=0
	
	SELECT @IsuseID=IsuseID, @IsOpened=IsOpened FROM CT_Isuses WHERE LotteryCode=@LotteryCode AND IsuseName=@IsuseName
	IF @@ROWCOUNT > 0
		BEGIN
			IF @IsOpened = 0 AND LEN(@OpenNumber) > 0
			BEGIN
				UPDATE CT_Isuses SET OpenNumber=@OpenNumber, OpenTime=@OpenTime, UpdateTime=GETDATE(), IsuseState=4, IsOpened=1
				WHERE IsuseID=@IsuseID
			END
		END
	SELECT @ReturnValue=@IsuseID, @ReturnDescription='��ӿ�����Ϣ�ɹ�'
	RETURN @IsuseID



GO
/****** Object:  StoredProcedure [dbo].[udp_ExchangeCaileBean]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Joan
-- Create date: 2017-11-23
-- Description:	�ʶ��һ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_ExchangeCaileBean]
	@UserCode BIGINT,          --�û�
	@Amount BIGINT,            --�һ����
	@Bean BIGINT,              --�һ��ʶ�
	@RecordValue INT OUTPUT    --�ص�ֵ
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		DECLARE @Balance BIGINT,@GoldBean BIGINT,@Time DATETIME = GETDATE();
		SELECT @Balance = Balance,@GoldBean = GoldBean FROM dbo.CT_Users WHERE UserID=@UserCode;
		IF @Amount > @Balance
			BEGIN
			    SET @RecordValue = 18; --����
			END
		ELSE
			BEGIN
				--�������Ͳʶ�
			    UPDATE dbo.CT_Users SET Balance = Balance - @Amount,GoldBean = GoldBean + @Bean WHERE UserID = @UserCode;
				INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
				VALUES  ( @UserCode ,4 ,@Amount ,@Balance - @Amount ,N'�һ��ʶ�:'+CONVERT(VARCHAR(50),@Bean) ,@UserCode ,0 ,@Time ,0)
				
				--������Ϸ��¼
				INSERT INTO CaileMiniGame.dbo.CT_Record( RecordType ,Amount ,RelationID ,Balance ,UserCode ,CreateTime)
				VALUES  ( 1 ,@Bean,CONVERT(VARCHAR(50),@UserCode) ,(@GoldBean+@Bean) ,@UserCode ,@Time)
				SET @RecordValue = 0
			END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SET @RecordValue = -1
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END


GO
/****** Object:  StoredProcedure [dbo].[udp_ExpireRevokeChase]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-10-21
-- Description:	�ںŹ���׷�ų���
-- =============================================
CREATE PROCEDURE [dbo].[udp_ExpireRevokeChase]
	@LotteryCode INT,             --����
	@IsuseName VARCHAR(200)       --�ں�
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY

	DECLARE @Time DATETIME;
	DECLARE @ChaseTable TABLE(SchemeID BIGINT,ChaseTaskDetailID BIGINT,	Amount BIGINT,UserID BIGINT,Balance BIGINT,Mobile VARCHAR(20),SchemeNumber VARCHAR(50),Nick VARCHAR(20));
	INSERT INTO @ChaseTable( SchemeID ,ChaseTaskDetailID ,Amount ,UserID,Balance,Mobile,SchemeNumber,Nick)
	SELECT d.SchemeID,d.ID AS ChaseTaskDetailID,d.Amount,u.UserID,u.Balance,u.UserMobile AS Mobile,s.SchemeNumber,u.UserName AS Nick FROM dbo.CT_ChaseTaskDetails AS d
	INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID = d.IsuseID
	INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID=d.SchemeID
	INNER JOIN dbo.CT_Users AS u ON u.UserID=s.InitiateUserID
	LEFT JOIN dbo.CT_SchemeETickets AS e ON e.SchemeID=d.SchemeID AND e.ChaseTaskDetailsID = d.ID
	WHERE i.LotteryCode=@LotteryCode AND i.IsuseName=@IsuseName AND d.IsExecuted=0 AND e.SchemeETicketsID IS NULL

	--��ʼ���� ������¼
	INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime)
	SELECT UserID,13,Amount,(Balance+Amount),'׷��ʧ�ܳ���',SchemeID,UserID,NULL FROM @ChaseTable

	--��ʼ���� �����ֽ�
	UPDATE u SET u.Balance = u.Balance + t.Amount  FROM dbo.CT_Users AS u 
	INNER JOIN @ChaseTable AS t ON t.UserID=u.UserID

	--��ʼ���� ����״̬
	UPDATE d SET d.QuashStatus=2 FROM dbo.CT_ChaseTaskDetails AS d
	INNER JOIN @ChaseTable AS t ON t.ChaseTaskDetailID = d.ID AND t.SchemeID=d.SchemeID
	
	--�������֮��ط����ͻ�����redisͬ�������Ͷ��ż�����
	SELECT SchemeID ,ChaseTaskDetailID ,Amount ,UserID,Mobile,SchemeNumber,Nick FROM @ChaseTable
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[udp_FollowBetting]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		joan
-- Create date: 2017-9-11
-- Description:	��������Ͷע
-- =============================================
CREATE PROCEDURE [dbo].[udp_FollowBetting]
	-- Add the parameters for the stored procedure here
	@UserID BIGINT,              --�������û����
	@SchemeID BIGINT,            --�����ķ������
	@SchemeNumber VARCHAR(50),   --������
	@IsuseName VARCHAR(20)       --��ǰ�ں�
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @VerifyIsuseCount INT = 0,@SchemeMoney BIGINT ,@Amount BIGINT,@BuyType TINYINT,@LotteryCode INT,@DateTime DATETIME = GETDATE(),@OrderCode BIGINT = 0;
	
	SELECT @SchemeMoney = SchemeMoney,@BuyType =  BuyType,@LotteryCode = LotteryCode FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
	SELECT @Amount = Balance FROM dbo.CT_Users WHERE UserID = @UserID;
	IF @BuyType != 1
		BEGIN
			IF @Amount < @SchemeMoney
				BEGIN
					SELECT -2; --����
				END
			ELSE 
				BEGIN
					--��֤�ںſ�ʼ����ʱ�� ����Ԥ�ۺ���ǰ��ֹ
					SELECT @VerifyIsuseCount = COUNT(1) FROM dbo.CT_Isuses a
					JOIN [dbo].[CT_Lotteries] b ON a.LotteryCode = b.LotteryCode
					WHERE a.LotteryCode = @LotteryCode AND a.IsuseName = @IsuseName AND DATEADD(MINUTE,(-b.PresellTime),StartTime) <= @DateTime AND DATEADD(MINUTE,(-b.AdvanceEndTime),EndTime) >= @DateTime;
					IF @VerifyIsuseCount = 0
					BEGIN
						SELECT -3; --��ǰ���ѽ���
					END
					ELSE
						BEGIN 
							--���ɸ�������
							INSERT INTO dbo.CT_Schemes
							        ( CreateTime ,SchemeNumber ,Title ,InitiateUserID ,LotteryCode ,IsuseID ,IsuseName ,LotteryNumber ,SchemeMoney ,SecrecyLevel ,
							          SchemeStatus ,PrintOutType ,Describe ,FromClient ,BuyType ,IsSplit ,FollowSchemeID ,FollowSchemeBonus ,FollowSchemeBonusScale ,
							          PlusAwards ,IsSendOut ,RoomCode)
							SELECT @DateTime ,@SchemeNumber ,Title ,@UserID ,LotteryCode ,IsuseID ,@IsuseName ,LotteryNumber ,SchemeMoney ,SecrecyLevel ,
							          4 ,PrintOutType ,Describe ,FromClient ,2 ,0 ,@SchemeID ,FollowSchemeBonus ,FollowSchemeBonusScale ,
							          PlusAwards ,0 ,RoomCode
							FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
							SELECT @OrderCode = @@IDENTITY

							--��������
							INSERT INTO dbo.CT_SchemesDetail(BetMoney,SchemeID ,PlayCode ,Multiple ,BetNum ,BetNumber ,IsBuyed ,IsNorm ,IsWin ,
							IsOpened ,WinMoney ,WinMoneyNoWithTax ,Schedule ,WinDescribe ,PrintOutTime ,WinImageUrl ,UpdateTime ,OpenOperatorID)
							SELECT BetMoney,@OrderCode ,PlayCode ,Multiple ,BetNum ,BetNumber ,0 ,IsNorm ,0 ,
							0 ,0 ,0 ,Schedule ,'' ,NULL ,WinImageUrl ,@DateTime ,0 
							FROM dbo.CT_SchemesDetail WHERE SchemeID=@SchemeID
							
							-- �������
							UPDATE dbo.CT_Users SET Balance = Balance - @SchemeMoney WHERE UserID = @UserID;

							--������¼
							INSERT INTO [dbo].CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
							SELECT @UserID,1,@SchemeMoney,Balance,'����',@DateTime,@SchemeID,@UserID FROM dbo.CT_Users WHERE UserID = @UserID


							--�����ֽ��(��ֵ�ٷ�֮��ʮ��) S 
							DECLARE @WithdrawMoney BIGINT,@WithdrawBalance BIGINT
							SET @WithdrawBalance = @Amount - @SchemeMoney;
							SELECT @WithdrawMoney = WithdrawMoney FROM CT_UsersExtend WHERE UserID=@UserID
							IF @WithdrawBalance < @WithdrawMoney
								BEGIN
									IF @WithdrawMoney > @SchemeMoney
										BEGIN
											SET @WithdrawMoney = @WithdrawMoney - @SchemeMoney
										END
									UPDATE dbo.CT_UsersExtend SET WithdrawMoney=WithdrawMoney - @WithdrawMoney WHERE UserID=@UserID
								END
							--�����ֽ�� E
							SELECT @OrderCode
						END
				END
		END 
	ELSE
		BEGIN 
			SELECT -1; --׷�Ų��������
		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END




GO
/****** Object:  StoredProcedure [dbo].[udp_GenerateIssue]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--���������ں�(����ʱʱ��)
--��1~23��(����ʱ��00:00 ~ 02:00)����97~120��(����ʱ��22:00 ~ 24:00)��ÿ�����һ��������47�ڡ�
--��24�� (����ʱ��09:30 ~ 10:00)��ÿ��ʮ����һ��������1�ڡ�
--��25 ~ 96�� (����ʱ��10:00 ~ 22:00)��ÿʮ����һ��������72�ڡ�
--������Ϸ�������©U�������Ϊ���� (��Ǫ���ۡ�ʰ����)��������Ϊ1 ��2 ��3 ��4��5��
CREATE PROC [dbo].[udp_GenerateIssue]
	@LotteryCode	INT,
	@Date	DATETIME,
	@Days	INT,
	@ReturnValue INT OUTPUT
	--WITH ENCRYPTION
AS
	IF @Days <=0
	BEGIN
		SET @ReturnValue = -1
		RETURN -1
	END

	DECLARE @T TABLE(id int, value varchar(128))
	DECLARE @T1 TABLE(id int, value varchar(128))
	DECLARE @LotteryID INT, @IntervalType VARCHAR(64)
	
	SELECT @LotteryID=LotteryID, @IntervalType=IntervalType FROM dbo.CT_Lotteries WHERE LotteryCode=@LotteryCode
	IF @@ROWCOUNT = 0
	BEGIN
		SET @ReturnValue = -2
		RETURN -2
	END
	
	INSERT @T SELECT * FROM dbo.F_SplitStrToTable(@IntervalType,'@')
	IF(SELECT COUNT(*) FROM @T ) < 5
	BEGIN
		SET @ReturnValue = -3
		RETURN -3
	END

	DECLARE @Type varchar(5), @Num INT, @Time VARCHAR(20), @interval INT, @WS INT,@QJ VARCHAR(30)
	DECLARE @JD1 VARCHAR(10),@JD2 VARCHAR(10),@JD3 VARCHAR(10),@JD4 VARCHAR(10)
	
	SELECT @interval=value FROM @T WHERE id=1	--�������
	SELECT @Type=value FROM @T WHERE id=2	--��������
	SELECT @Num=value FROM @T WHERE id=3	--����
	SELECT @Time=value FROM @T WHERE id=4	--��Ƶ�ʣ���һ�ڿ�ʼʱ��
	SELECT @WS=value FROM @T WHERE id=5 --�ں�λ��
	SELECT @QJ=value FROM @T WHERE id=6 --�ں�ʱ������
	INSERT @T1 SELECT * FROM dbo.F_SplitStrToTable(@QJ,'|')
	SELECT @JD1=value FROM @T1 WHERE id=1	--��һ�׶�ʱ����
	SELECT @JD2=value FROM @T1 WHERE id=2	--�ڶ��׶�ʱ����
	SELECT @JD3=value FROM @T1 WHERE id=3	--�����׶�ʱ����
	SELECT @JD4=value FROM @T1 WHERE id=4	--���Ľ׶�ʱ����
	
	
	DECLARE @NumTable TABLE(LotteryCode INT, IsuseName varchar(20), StartTime DATETIME, EndTime DATETIME)--�ں���ʱ��
	DECLARE @index INT=1, @i INT=1, @IsuseName VARCHAR(20), @StartDate DATETIME, @EndDate DATETIME, @Date2 VARCHAR(8)
	
	IF @Type = '��'  --��Ƶ��
	BEGIN
		WHILE(@i <= @Days)
		BEGIN
			SET @Date2 = CONVERT(VARCHAR(8),@Date,112)
			SET @StartDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@Date) + ' ' + dbo.fnGetField(@Time, 1, '-')) --LEFT(@Time,CHARINDEX('-',@Time)-1))
		    SET @EndDate = CONVERT(DATETIME, CONVERT(VARCHAR(10),@Date) + ' ' + DATEADD(MINUTE,@interval,dbo.fnGetField(@Time, 1, '-')))

			WHILE(@index <= @Num)
			BEGIN
				
				SET @IsuseName = @Date2 + REPLICATE(0,@WS-LEN(@index)) + CONVERT(VARCHAR(3),@index)

				INSERT @NumTable(LotteryCode, IsuseName, StartTime, EndTime) 
					VALUES (@LotteryCode, @IsuseName, @StartDate, @EndDate)
					
				
				SET @index = @index + 1
				
				SET @StartDate=DATEADD(MINUTE,@interval,@StartDate)
				
				IF @index > CONVERT(INT,dbo.fnGetField(@JD1, 1, '-')) AND @index <= CONVERT(INT,dbo.fnGetField(@JD2, 1, '-'))
					BEGIN
						SET @interval = dbo.fnGetField(@JD2, 2, '-')
					END
				ELSE IF @index > CONVERT(INT,dbo.fnGetField(@JD2, 1, '-')) AND @index <= CONVERT(INT,dbo.fnGetField(@JD3, 1, '-'))
					BEGIN
						SET @interval = dbo.fnGetField(@JD3, 2, '-')
					END 
				ELSE IF @index > CONVERT(INT,dbo.fnGetField(@JD3, 1, '-'))  AND @index <= CONVERT(INT,dbo.fnGetField(@JD4, 1, '-'))
					BEGIN
						SET @interval = dbo.fnGetField(@JD4, 2, '-')
					END

				SET @EndDate=DATEADD(MINUTE,@interval,@EndDate)
			END

			INSERT dbo.CT_Isuses(LotteryCode,IsuseName,StartTime,EndTime,IsExecuteChase,IsOpened,OpenNumber,OpenOperatorID,IsuseState,UpdateTime)
				SELECT a.LotteryCode, a.IsuseName, a.StartTime, a.EndTime, 0 AS IsExecuteChase, 0 AS IsOpened,
					'' AS OpenNumber,0 AS OpenOperatorID,0 AS IsuseState,GETDATE() AS UpdateTime
				FROM @NumTable a LEFT JOIN dbo.CT_Isuses b ON b.IsuseName=a.IsuseName AND b.LotteryCode=a.LotteryCode
					WHERE b.IsuseID IS NULL

			SET @i = @i + 1
			SET @index = 1
			SET @Date = DATEADD(DAY,1,@Date)
			DELETE @NumTable
		END	
	END
	
	SET @ReturnValue = 0
	RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[udp_IsuseStopRevokeSchemes]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--�����ںŽ�ֹû�в�ƱͶע�ķ���
--2017-5-10
CREATE PROC [dbo].[udp_IsuseStopRevokeSchemes]
	@LotteryCode INT
AS
	DECLARE @Time DATETIME = GETDATE();
	DECLARE @T TABLE(ID INT IDENTITY(1,1), SchemeID BIGINT, UserID BIGINT,UserName VARCHAR(20),UserMobile VARCHAR(13));
	DECLARE @RecTab TABLE(name VARCHAR(20),issue VARCHAR(20),num VARCHAR(50),amount BIGINT,mobile VARCHAR(13),uuid BIGINT,ssid BIGINT);

	INSERT @T(SchemeID, UserID,UserName,UserMobile)
	SELECT s.SchemeID,s.InitiateUserID,u.UserName,u.UserMobile FROM dbo.CT_Schemes  AS s
	INNER JOIN dbo.CT_Isuses AS i ON i.LotteryCode = s.LotteryCode
	AND i.IsuseID=s.IsuseID AND s.SchemeStatus=4 AND s.IsSendOut=0
	INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode=i.LotteryCode
	AND l.LotteryCode=s.LotteryCode AND  DATEADD(MINUTE,-(l.AdvanceEndTime+1),i.EndTime) < @Time
	AND @Time > DATEADD(MINUTE,2,s.CreateTime) --���������û�г�Ʊ������
	INNER JOIN dbo.CT_Users AS u ON u.UserID=s.InitiateUserID
	WHERE s.LotteryCode=@LotteryCode
	GROUP BY s.SchemeID,s.InitiateUserID,u.UserName,u.UserMobile
	
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		DECLARE @UserMobile VARCHAR(13),@SchemeID BIGINT, @UserID BIGINT, @ID INT, @SchemeMoney BIGINT, @SchemeStatus SMALLINT,@BuyType TINYINT,@IsuseName VARCHAR(20),
		@SchemeNumber VARCHAR(50),@UserName VARCHAR(20),@GoldBean BIGINT,@Balance BIGINT;
		SELECT @ID = MIN(ID) FROM @T
		WHILE @ID IS NOT NULL
		BEGIN
			SELECT @SchemeID=SchemeID, @UserID=UserID,@UserName=UserName,@UserMobile=UserMobile FROM @T WHERE ID = @ID
		
			SELECT @SchemeMoney = SchemeMoney, @SchemeStatus=SchemeStatus,@BuyType=BuyType,@IsuseName=IsuseName,@SchemeNumber=SchemeNumber FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
			IF @@rowcount = 0
			BEGIN
				SELECT @ID = MIN(ID) FROM @T WHERE  ID>@ID
				CONTINUE
			END
		
			IF @SchemeStatus=12  --�Ѿ�����������ֻҪ���µ���Ʊ״̬���������˿�
			BEGIN
				SELECT @ID = MIN(ID) FROM @T WHERE  ID>@ID
				CONTINUE
			END
			--�ڶ�����ȴ��ķ����Ų��ܿ��ƣ���Ҫ������Ʊ�Ƿ�������
			UPDATE dbo.CT_SchemeETickets SET TicketStatus=3 WHERE SchemeID=@SchemeID
			IF @BuyType != 1
				BEGIN
					UPDATE dbo.CT_Schemes SET SchemeStatus=12 WHERE SchemeID=@SchemeID
				END

			UPDATE s SET s.Balance = s.Balance + r.TradeAmount FROM dbo.CT_Users AS s
			INNER JOIN dbo.CT_UsersRecord AS r ON r.UserID = s.UserID AND r.TradeType = 1 AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID);
		
			--��ȯ������¼
			PRINT @SchemeID
			PRINT @UserID
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
			SELECT UserID ,23 ,TradeAmount ,Balance + TradeAmount ,'�ںŽ�ֹ��ûͶע���г���:����ȯ' ,RelationID ,OperatorID ,@Time ,CouponsID FROM CT_UsersRecord 
			WHERE UserID=@UserID AND TradeType=22 AND RelationID=CONVERT(VARCHAR(32),@SchemeID)
		
			UPDATE r SET r.Balance = r.Balance + u.TradeAmount  FROM CT_UsersRecord AS u
			INNER JOIN CaileCoupons.dbo.CT_Coupons AS r 
			ON r.UserID=u.UserID AND u.RelationID=CONVERT(VARCHAR(32),@SchemeID)
			AND u.TradeType=22 AND r.CouponsID = u.CouponsID
			
			--�ʶ�����
			SELECT @GoldBean = GoldBean,@Balance = Balance FROM dbo.CT_Users WHERE UserID = @UserID;
			-- �ʶ�������¼
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
			SELECT UserID ,25 ,TradeAmount ,@GoldBean,'�ںŽ�ֹ��ûͶע���г���:���ʶ�' ,RelationID ,OperatorID ,@Time ,CouponsID FROM CT_UsersRecord 
			WHERE UserID=@UserID AND TradeType=24 AND RelationID=CONVERT(VARCHAR(32),@SchemeID);
			--���³����ʶ�
			UPDATE s SET s.GoldBean = s.GoldBean + r.TradeAmount FROM dbo.CT_Users AS s
			INNER JOIN dbo.CT_UsersRecord AS r ON r.UserID = s.UserID AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID) AND r.TradeType = 24;
			
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
			SELECT UserID ,12 ,TradeAmount ,@Balance,'�ںŽ�ֹ��ûͶע���г���' ,RelationID ,OperatorID ,@Time ,CouponsID FROM CT_UsersRecord 
			WHERE UserID=@UserID AND TradeType=1 AND RelationID=CONVERT(VARCHAR(32),@SchemeID);

			INSERT @RecTab( name, issue, num, amount, mobile,uuid,ssid) VALUES ( @UserName,@IsuseName,@SchemeNumber,@SchemeMoney,@UserMobile,@UserID,@SchemeID) ;
			SELECT @ID = MIN(ID) FROM @T WHERE  ID>@ID
		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			
			SELECT -1;

            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	SELECT * FROM @RecTab





GO
/****** Object:  StoredProcedure [dbo].[udp_OmissionRecord]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-11-9
-- Description:	��ȡ��©���ںż�¼
-- =============================================
CREATE PROCEDURE [dbo].[udp_OmissionRecord]
	@LotteryCode INT,
	@DateDay DATETIME
AS
BEGIN
	DECLARE @Time DATETIME = GETDATE();
	IF ISNULL(@DateDay,'')=''
		BEGIN
		    SELECT LotteryCode, IsuseName, StartTime, EndTime FROM dbo.CT_Isuses WHERE LotteryCode=@LotteryCode AND OpenNumber <> '' AND CONVERT(DATE,StartTime)=CONVERT(DATE,@Time)
		END
	ELSE
		BEGIN
		    SELECT LotteryCode, IsuseName, StartTime, EndTime FROM dbo.CT_Isuses WHERE LotteryCode=@LotteryCode AND OpenNumber <> '' AND CONVERT(DATE,StartTime)=CONVERT(DATE,@DateDay)
		END
END

GO
/****** Object:  StoredProcedure [dbo].[udp_OpenAwardHall]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-11
-- Description:	���ֿ���������ѯ
-- =============================================
CREATE PROCEDURE [dbo].[udp_OpenAwardHall]
AS
BEGIN
	SET NOCOUNT ON;
	WITH tab
	AS 
	(
		SELECT TOP 1 i.LotteryCode,i.IsuseName,i.OpenNumber,i.OpenTime FROM dbo.CT_Lotteries AS l 
		LEFT JOIN dbo.CT_Isuses AS i ON i.LotteryCode = l.LotteryCode AND i.OpenNumber<>''
		AND l.LotteryCode=101
		GROUP BY i.LotteryCode,i.OpenNumber,i.OpenTime,i.IsuseName
		ORDER BY i.IsuseName DESC
		UNION 
		SELECT TOP 1 i.LotteryCode,i.IsuseName,i.OpenNumber,i.OpenTime FROM dbo.CT_Lotteries AS l 
		LEFT JOIN dbo.CT_Isuses AS i ON i.LotteryCode = l.LotteryCode AND i.OpenNumber<>''
		AND l.LotteryCode=102
		GROUP BY i.LotteryCode,i.OpenNumber,i.OpenTime,i.IsuseName
		ORDER BY i.IsuseName DESC
		UNION 
		SELECT TOP 1 i.LotteryCode,i.IsuseName,i.OpenNumber,i.OpenTime FROM dbo.CT_Lotteries AS l 
		LEFT JOIN dbo.CT_Isuses AS i ON i.LotteryCode = l.LotteryCode AND i.OpenNumber<>''
		AND l.LotteryCode=201
		GROUP BY i.LotteryCode,i.OpenNumber,i.OpenTime,i.IsuseName
		ORDER BY i.IsuseName DESC
		UNION 
		SELECT TOP 1 i.LotteryCode,i.IsuseName,i.OpenNumber,i.OpenTime FROM dbo.CT_Lotteries AS l 
		LEFT JOIN dbo.CT_Isuses AS i ON i.LotteryCode = l.LotteryCode AND i.OpenNumber<>''
		AND l.LotteryCode=202
		GROUP BY i.LotteryCode,i.OpenNumber,i.OpenTime,i.IsuseName
		ORDER BY i.IsuseName DESC
		UNION 
		SELECT TOP 1 i.LotteryCode,i.IsuseName,i.OpenNumber,i.OpenTime FROM dbo.CT_Lotteries AS l 
		LEFT JOIN dbo.CT_Isuses AS i ON i.LotteryCode = l.LotteryCode AND i.OpenNumber<>''
		AND l.LotteryCode=301
		GROUP BY i.LotteryCode,i.OpenNumber,i.OpenTime,i.IsuseName
		ORDER BY i.IsuseName DESC
		UNION 
		SELECT TOP 1 i.LotteryCode,i.IsuseName,i.OpenNumber,i.OpenTime FROM dbo.CT_Lotteries AS l 
		LEFT JOIN dbo.CT_Isuses AS i ON i.LotteryCode = l.LotteryCode AND i.OpenNumber<>''
		AND l.LotteryCode=801
		GROUP BY i.LotteryCode,i.OpenNumber,i.OpenTime,i.IsuseName
		ORDER BY i.IsuseName DESC
		UNION 
		SELECT TOP 1 i.LotteryCode,i.IsuseName,i.OpenNumber,i.OpenTime FROM dbo.CT_Lotteries AS l 
		LEFT JOIN dbo.CT_Isuses AS i ON i.LotteryCode = l.LotteryCode AND i.OpenNumber<>''
		AND l.LotteryCode=901
		GROUP BY i.LotteryCode,i.OpenNumber,i.OpenTime,i.IsuseName
		ORDER BY i.IsuseName DESC
	)
	SELECT * FROM tab
END

GO
/****** Object:  StoredProcedure [dbo].[udp_OutETicketsLst]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[udp_OutETicketsLst]
  @merchantCode  INT,
  @lotteryCode  INT,
  @outTicketStauts  INT,
  @startTime  DATETIME,
  @endTime  DATETIME,
  @pageSize INT,  --ÿҳ��¼��
  @pageIndex INT,  --��ǰҳ��
  @SumMoney BIGINT OUTPUT,    --��¼����
  @SumBonus BIGINT OUTPUT,   --��¼����
  @recordCount INT OUTPUT    --��¼����
AS
  SET NOCOUNT ON;
  BEGIN
  
  DECLARE @SqlStr NVARCHAR(2000),@SqlWhere NVARCHAR(1500),@SqlCount NVARCHAR(4000)
  --ƴ��
  SET @SqlStr = 'SELECT ROW_NUMBER() OVER(ORDER BY a.OutETicketsID DESC) as Num
		, a.OutETicketsID
		, a.MerchantCode
		, a.LotteryCode
		, a.MerchantTicket
		, a.HuaYangTicket
		, a.Money
		, a.Multiple
		, a.Bonus
		, a.SendTicketDateTime
		, a.OutTicketDateTime
		, a.OutTicketStauts
          FROM CT_OutETickets a '
  --����
  SET @SqlWhere = ' where 1 = 1 '
 if @merchantCode >0
    BEGIN
      SET @SqlWhere = @SqlWhere + ' AND a.MerchantCode = '+ CONVERT(VARCHAR(20),@merchantCode);
    END
 if @lotteryCode > 0
    BEGIN
      SET @SqlWhere = @SqlWhere + ' AND a.LotteryCode = ' + CONVERT(VARCHAR(20),@lotteryCode);
    END
if @outTicketStauts > 0
BEGIN
	SET @SqlWhere = @SqlWhere + ' AND a.OutTicketStauts = ' + CONVERT(VARCHAR(20),@outTicketStauts);
END

  SET @SqlWhere = @SqlWhere + ' AND a.SendTicketDateTime >= '''+ CONVERT(VARCHAR(20),@startTime)+'''';
  SET @SqlWhere = @SqlWhere + ' AND a.SendTicketDateTime <= '''+ CONVERT(VARCHAR(20),@endTime)+'''';

  --�ص�
  SET @SqlCount = N'SELECT @a = COUNT(1), @b = ISNULL(SUM(Money), 0), @C = ISNULL(SUM(Bonus), 0) FROM ( '  + @SqlStr + @SqlWhere + ' ) tab ';

  --��ҳ
  SET @SqlWhere = @SqlWhere + ' ORDER BY Num
  OFFSET '+ CONVERT(VARCHAR(20),((@pageIndex - 1) * @pageSize)) +' ROWS
  FETCH NEXT '+ CONVERT(VARCHAR(20),@pageSize) +' ROWS ONLY';
  

  DECLARE @SqlData NVARCHAR(4000);
  SET @SqlData = (@SqlStr + @SqlWhere)
  EXEC sp_executesql @SqlData
  EXEC sp_executesql @SqlCount,N'@a INT OUTPUT, @b BIGINT OUTPUT, @c BIGINT OUTPUT',@recordCount OUTPUT, @SumMoney OUTPUT, @SumBonus OUTPUT

  END
GO
/****** Object:  StoredProcedure [dbo].[udp_PayInfo]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Joan
-- Create date: 2017-07-11
-- Description:	��ֵ���
-- =============================================
CREATE PROCEDURE [dbo].[udp_PayInfo]
	@RechargeNo VARCHAR(200),
	@OutRechargeNo VARCHAR(200),
	@PayType VARCHAR(50),
	@Result TINYINT,
	@PayID BIGINT,
	@Balance BIGINT,
	@UserID BIGINT,
	@RelationID VARCHAR(200),
	@WithdrawMoney BIGINT,  --�����ֽ��
	@Rec INT OUTPUT -- ���ؽ��
	--WITH ENCRYPTION
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @Amount BIGINT
	SELECT @Amount=Amount FROM CT_UsersPayDetail WHERE PayID=@PayID ;
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Rec = 2; -- ��ѯʧ��
		END

	UPDATE CT_UsersPayDetail SET RechargeNo=@RechargeNo,OutRechargeNo=@OutRechargeNo,CompleteTime=GETDATE(),Result=@Result,PayType=@PayType WHERE PayID=@PayID ;
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Rec = 3; -- ����ʧ��
		END

	UPDATE CT_Users SET Balance=@Balance WHERE UserID=@UserID ;
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Rec = 3; -- ����ʧ��
		END

	INSERT INTO CT_UsersRecord(UserID,TradeType,TradeAmount,Balance,TradeRemark,CreateTime,RelationID)
		               SELECT @UserID,0,@Amount,@Balance,@PayType,GETDATE(),@RelationID FROM dbo.CT_Users WHERE UserID=@UserID
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Rec = 4; -- ��������ʧ��
		END
	--�����ֽ��(��ֵ�ٷ�֮��ʮ��) S 
	UPDATE dbo.CT_UsersExtend SET WithdrawMoney=WithdrawMoney + @WithdrawMoney WHERE UserID=@UserID
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Rec = 3; -- ����ʧ��
		END
	--�����ֽ�� E

	--��ֵ�Ͳ�ȯ(������ȯ����ֵ��20(��)�Ͳ�ȯ) ������ȯ 20 - 3
	IF @Amount >= 2000
		BEGIN 
			DECLARE @CouponsID BIGINT,@Time DATETIME = GETDATE();
			--���ɲ�ȯ
			INSERT INTO CaileCoupons.dbo.CT_Coupons
			        ( ActivityID ,GenerateTime ,ReleaseTime ,FirstTime ,LastTime ,CouponsStatus ,CouponsType ,StartTime ,ExpireTime ,FaceValue ,
					  Balance ,SatisfiedMoney ,IsGive ,LotteryCode ,IsChaseTask ,IsSuperposition ,IsTimes ,IsJoinBuy ,UserID ,CouponsSource)
			VALUES  ( 0 ,@Time ,@Time ,NULL ,NULL ,1 ,2 ,@Time ,DATEADD(DAY,3,@Time) ,300 ,
			          300 ,2000 ,0 ,0 ,0 ,0 ,0 ,0 ,@UserID ,1)
			SELECT @CouponsID = @@IDENTITY

			--��ȡ��ȯ��¼
			INSERT INTO CaileCoupons.dbo.CT_CouponsRecord( CouponsID ,LogType ,CreateTime ,UserID ,RelationID ,Amount)
			VALUES  ( @CouponsID ,1 ,@Time ,@UserID ,CONVERT(VARCHAR(32),@UserID) ,@Amount)
		END
	--��ֵ�Ͳ�ȯ
	
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	SET @Rec = 0;
END





GO
/****** Object:  StoredProcedure [dbo].[udp_QuertPayDetailReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-11-6
-- Description:	����:��ֵ��ѯͳ��
-- ɸѡ���û�ID��ƽ̨������ţ�������������ţ���ֵ��ʽ[΢�š�֧��������̨���� ]����ֹʱ�䣬״̬[�ύ��ȡ�������] 
-- ��ʾ��ƽ̨������ţ��û�ID���û��˺ţ��ֻ����룬��ֵ��ʽ���ύʱ�䣬֧���������������ţ�����״̬
-- ���ܣ���ֵ�ܶ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_QuertPayDetailReport]
	@StartTime DATETIME,                        --��ֹʱ��_��
	@EndTime DATETIME,                          --��ֹʱ��_ֹ
	@UserID BIGINT,                             --�û�ID
	@OrderNo VARCHAR(50),                       --ƽ̨�������
	@RechargeNo VARCHAR(200),	                --�������������
	@PayType VARCHAR(50),                       --��ֵ��ʽ
	@Result SMALLINT,                           --״̬ ֧����� 0.δ�ɹ� 1.�ѳɹ� 2.���˿� 3.�˿����,
	@PageIndex INT,                             --��ǰҳ
	@PageSize INT,                              --ҳ��С
	@RecordPayAmount BIGINT OUTPUT,             --��ֵ�ܶ�
	@RecordCount INT OUTPUT                     --��ѯ����
AS
BEGIN
	DECLARE @DataSql NVARCHAR(4000),@RecordSql NVARCHAR(4000),@RecordPaySql NVARCHAR(2000),@WhereSql NVARCHAR(1500),@OrderSql NVARCHAR(500);
	SET @DataSql = 'SELECT ROW_NUMBER() OVER(ORDER BY upd.CreateTime DESC) AS Num,upd.OrderNo,ur.UserID,u.UserName,u.UserMobile,upd.PayType,upd.CreateTime,ur.TradeAmount,upd.RechargeNo,upd.Result FROM dbo.CT_UsersRecord AS ur
					INNER JOIN dbo.CT_Users AS u ON u.UserID = ur.UserID AND ur.TradeType=0 AND ur.TradeAmount > 0
					INNER JOIN dbo.CT_UsersPayDetail AS upd ON upd.UserID=ur.UserID AND upd.UserID=u.UserID AND (upd.PayID=ur.RelationID OR upd.OrderNo=ur.RelationID)';

	SET @WhereSql = ' WHERE upd.CreateTime>='''+ CONVERT(VARCHAR(20),@StartTime) +''' and upd.CreateTime<='''+ CONVERT(VARCHAR(20),@EndTime) +'''';
	IF @UserID > 0
		BEGIN
			SET @WhereSql = @WhereSql + ' AND ur.UserID=' + CONVERT(VARCHAR(20),@UserID);
		END
	IF ISNULL(@OrderNo,'') !=''
		BEGIN
			SET @WhereSql = @WhereSql + ' AND upd.OrderNo like ''%'+ @OrderNo +'%''';
		END
	IF ISNULL(@RechargeNo,'') !=''
		BEGIN
			SET @WhereSql = @WhereSql + ' AND upd.RechargeNo like ''%'+ @RechargeNo +'%''';
		END
	IF ISNULL(@PayType,'') !=''
		BEGIN
			SET @WhereSql = @WhereSql + ' AND upd.PayType like ''%'+ @PayType +'%''';
		END
	IF @Result != -1
		BEGIN
			SET @WhereSql = @WhereSql + ' AND upd.Result=' + CONVERT(VARCHAR(20),@Result);
		END
	SET @RecordSql = 'SELECT @b = COUNT(1) FROM dbo.CT_UsersRecord AS ur
					INNER JOIN dbo.CT_Users AS u ON u.UserID = ur.UserID AND ur.TradeType=0 AND ur.TradeAmount > 0
					INNER JOIN dbo.CT_UsersPayDetail AS upd ON upd.UserID=ur.UserID AND upd.UserID=u.UserID AND (upd.PayID=ur.RelationID OR upd.OrderNo=ur.RelationID)';

	SET @RecordPaySql = 'SELECT  @a = ISNULL(SUM(ur.TradeAmount),0) FROM dbo.CT_UsersRecord AS ur
				      INNER JOIN dbo.CT_UsersPayDetail AS upd ON upd.UserID = ur.UserID AND ur.TradeType=0 AND (upd.PayID=ur.RelationID OR upd.OrderNo=ur.RelationID)
				      AND upd.Result=1';
	SET @OrderSql = ' ORDER BY Num
					  OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
					  FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';

	SET @DataSql = @DataSql + @WhereSql + @OrderSql;
	SET @RecordSql = @RecordSql + @WhereSql;

	EXEC sp_executesql @DataSql
	EXEC sp_executesql @RecordSql,N'@b INT OUTPUT',@RecordCount OUTPUT
	EXEC sp_executesql @RecordPaySql,N'@a BIGINT OUTPUT',@RecordPayAmount OUTPUT
END


GO
/****** Object:  StoredProcedure [dbo].[udp_QuertWithdrawReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-11-6
-- Description:	���ֲ�ѯͳ��
-- ɸѡ���û�ID���û��˺ţ���ֹʱ�䣬״̬���ύ����ˡ���ɣ�
-- ��ʾ��ƽ̨������ţ��û�ID���û��˺ţ��ֻ����룬�ύʱ�䣬���ֽ����ֶ���״̬
-- ���ܣ������ܶ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_QuertWithdrawReport]
	@StartTime DATETIME,                        --��ֹʱ��_��
	@EndTime DATETIME,                          --��ֹʱ��_ֹ
	@UserName VARCHAR(20),                      --�û��˺�
	@UserID BIGINT,                             --�û�ID	
	@PayOutStatus TINYINT,                      --״̬��0.���룬2.�����У�4.������ɣ�6.����ʧ��
	@PageIndex INT,                             --��ǰҳ
	@PageSize INT,                              --ҳ��С
	@RecordWithdrawAmount BIGINT OUTPUT,        --��ֵ�ܶ�
	@RecordCount INT OUTPUT                     --��ѯ����
AS
BEGIN
	DECLARE @DataSql NVARCHAR(4000),@RecordSql NVARCHAR(4000),@RecordPaySql NVARCHAR(2000),@WhereSql NVARCHAR(1500),@OrderSql NVARCHAR(500);

	SET @DataSql = 'SELECT ROW_NUMBER() OVER(ORDER BY w.CreateTime DESC) AS Num,w.PayOutID,ur.UserID,u.UserName,u.UserMobile,w.CreateTime,ur.TradeAmount,w.PayOutStatus FROM dbo.CT_UsersRecord AS ur
					INNER JOIN dbo.CT_Users AS u ON u.UserID=ur.UserID AND (ur.TradeType=2 OR ur.TradeType=3) AND ur.TradeAmount > 0
					INNER JOIN dbo.CT_UsersWithdraw AS w ON w.UserID=ur.UserID AND w.UserID=u.UserID AND w.PayOutID = ur.RelationID';

	SET @WhereSql = ' WHERE w.CreateTime>='''+ CONVERT(VARCHAR(20),@StartTime) +''' and w.CreateTime<='''+ CONVERT(VARCHAR(20),@EndTime) +'''';
	IF ISNULL(@UserName,'')!=''
		BEGIN
			SET @WhereSql = @WhereSql + ' AND u.UserName like ''%'+ @UserName +'%''';
		END
	IF ISNULL(@UserID,0) > 0
		BEGIN
			SET @WhereSql = @WhereSql + ' AND ur.UserID=' + CONVERT(VARCHAR(20),@UserID);
		END
	IF ISNULL(@PayOutStatus,-1)!=-1
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND w.PayOutStatus=' + CONVERT(VARCHAR(20),@PayOutStatus);
		END
	SET @RecordSql = 'SELECT @b = COUNT(1) FROM dbo.CT_UsersRecord AS ur
					INNER JOIN dbo.CT_Users AS u ON u.UserID=ur.UserID AND (ur.TradeType=2 OR ur.TradeType=3) AND ur.TradeAmount > 0
					INNER JOIN dbo.CT_UsersWithdraw AS w ON w.UserID=ur.UserID AND w.UserID=u.UserID AND w.PayOutID = ur.RelationID';
 
	SET @RecordPaySql = 'SELECT @a = ISNULL(SUM(ur.TradeAmount),0) FROM dbo.CT_UsersRecord AS ur
						 INNER JOIN dbo.CT_UsersWithdraw AS w ON w.UserID=ur.UserID AND (ur.TradeType=2 OR ur.TradeType=3) AND w.PayOutID = ur.RelationID';

	SET @OrderSql = ' ORDER BY Num
					  OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
					  FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';

	SET @DataSql = @DataSql + @WhereSql + @OrderSql;
	SET @RecordSql = @RecordSql + @WhereSql;

	EXEC sp_executesql @DataSql
	EXEC sp_executesql @RecordSql,N'@b INT OUTPUT',@RecordCount OUTPUT
	EXEC sp_executesql @RecordPaySql,N'@a INT OUTPUT',@RecordWithdrawAmount OUTPUT

END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryActivityList]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-10-8
-- Description:	��б��ѯ
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryActivityList]
	@Keys VARCHAR(20),        --�ؼ��ֲ�ѯ
	@ActivityType INT,        --���Ͳ�ѯ
	@StartTime DATETIME,      --���ʼʱ��
	@EndTime DATETIME,        --�����ʱ��
	@IsModify INT,            --�Ƿ���»
	@ActivityApply INT,       --���
	@CurrencyUnit INT,        --�����
	@PageIndex INT,           --��ǰҳ
	@PageSize INT,            --ҳ��С
	@RecordCount INT OUTPUT   --����
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @DataSQL NVARCHAR(4000),@SQLCount NVARCHAR(4000),@SQLStr NVARCHAR(2000),@SQLWhere NVARCHAR(1500),@SQLPage NVARCHAR(500);
	
	--��ҳ��ѯ���
	SET @SQLStr = 'SELECT ROW_NUMBER() OVER(ORDER BY CreateTime DESC,ActivityApply ASC) AS Num,ActivityID,ActivityType,ActivitySubject,ActivityDescribe,CreateTime,
				   StartTime,EndTime,ModifyTime,ModifyDescribe,ActivityMoney,ModifyMoney,IsModify,ActivityApply,CurrencyUnit,LandingPage FROM dbo.CT_Activity ';

	--��������ѯ���
	SET @SQLCount = 'SELECT @a = COUNT(1) FROM dbo.CT_Activity ';

	--��ѯ����
	SET @SQLWhere = ' WHERE CreateTime >= ''' + CONVERT(VARCHAR(20),@StartTime) + ''' AND CreateTime <= ''' + CONVERT(VARCHAR(20),@EndTime) + ''' ';
	IF ISNULL(@Keys,'') != '' --�ؼ���
		BEGIN 
			SET @SQLWhere = @SQLWhere + ' AND (ActivitySubject LIKE ''%' + @Keys +'%'' OR ActivityDescribe LIKE ''%' + @Keys +'%'' OR ModifyDescribe LIKE ''%' + @Keys +'%'') ';
		END
	IF @IsModify != -1 --�Ƿ���»
		BEGIN
		    SET @SQLWhere = @SQLWhere + ' AND IsModify = ' + CONVERT(VARCHAR(10),@IsModify);
		END
	IF @ActivityApply != -1 --����
		BEGIN
		    SET @SQLWhere = @SQLWhere + ' AND ActivityApply = ' + CONVERT(VARCHAR(10),@ActivityApply) ;
		END
	IF @ActivityType != -1 --�����
		BEGIN
		    SET @SQLWhere = @SQLWhere + ' AND ActivityType = ' + CONVERT(VARCHAR(10),@ActivityType);
		END
	IF @CurrencyUnit != -1 --�����
		BEGIN
		    SET @SQLWhere = @SQLWhere + ' AND CurrencyUnit = ' + CONVERT(VARCHAR(10),@CurrencyUnit);
		END

	--��ҳ
	SET @SQLPage = ' ORDER BY Num
	OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
	FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';

	--�ϲ�
	SET @DataSQL = @SQLStr + @SQLWhere + @SQLPage
	SET @SQLCount = @SQLCount + @SQLWhere;

	--ִ��
	PRINT @DataSQL; --��ӡ
	EXEC sp_executesql @DataSQL
	EXEC sp_executesql @SQLCount,N'@a INT OUTPUT',@RecordCount OUTPUT
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryAwardLot]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-09-09
-- Description:	��ѯ�ӽ�����
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryAwardLot]
	-- Add the parameters for the stored procedure here
	@ActivityType INT,  --����ͣ�0 �ٷ����1 ����ƽ̨�
	@LotteryCode INT    --��ѯĳ�������Ƿ�ӽ�
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @SqlData NVARCHAR(1000);
	SET @SqlData = 'SELECT aa.LotteryCode FROM dbo.CT_Activity AS a 
					INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID=a.ActivityID 
					WHERE a.ActivityApply = 1
					AND a.StartTime <= GETDATE()
					AND a.EndTime >= GETDATE()
					AND aa.RegularStatus = 2 
					AND a.ActivityType = '+CONVERT(VARCHAR(10),@ActivityType);
	IF @LotteryCode > 0
		BEGIN
			SET @SqlData = @SqlData + ' AND aa.LotteryCode = ' + CONVERT(VARCHAR(10),@LotteryCode);
		END
	PRINT @SqlData
	EXEC sp_executesql @SqlData
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryAwardPlayCode]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		joan
-- Create date: 2017-09-09
-- Description:	��ѯ�ӽ�������ϸ�淨
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryAwardPlayCode]
	@LotteryCode INT    --��ѯĳ�������Ƿ�ӽ�
AS
BEGIN
	SET NOCOUNT ON;
	--�������ͣ�0 ��׼�淨�ӽ���1 ׷�żӽ���2 �����淨�ӽ���3 ��Ӽӽ���4 ���ؼӽ���5 Ͷע����ۼ�����ӽ���6 �н��������ӽ���
	--7 ��ڼ�Ͷע����ۼ����μӽ���8 ��ڼ��н�����ۼ����μӽ���9 ���ֲ�����ӽ���10 �ڼ��ռӽ�
	DECLARE @Time DATETIME = GETDATE();
	WITH tab AS
	(
		--��׼�淨�ӽ�
		SELECT b.PlayCode,b.AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=0 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN CT_RegularNorm AS b ON b.RegularID = aa.RegularID 
		UNION
		--׷�żӽ�
		SELECT b.PlayCode,b.AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=1 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN dbo.CT_RegularChase AS b ON b.RegularID = aa.RegularID
		UNION
		--�����淨�ӽ�
		SELECT b.PlayCode,b.AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=2 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN dbo.CT_RegularDanTuo AS b ON b.RegularID = aa.RegularID
		UNION
		--Ͷע����ۼ�����ӽ�
		SELECT b.PlayCode,0 AS AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=5 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN dbo.CT_RegularBetInterval AS b ON b.RegularID = aa.RegularID
		UNION
		--�н��������ӽ�
		SELECT b.PlayCode,0 AS AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=6 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN dbo.CT_RegularAwardInterval AS b ON b.RegularID = aa.RegularID
		UNION
		--��ڼ�Ͷע����ۼ����μӽ�
		SELECT b.PlayCode,0 AS AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=7 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN dbo.CT_RegularBetRanking AS b ON b.RegularID = aa.RegularID
		UNION
		--��ڼ��н�����ۼ����μӽ�
		SELECT b.PlayCode,0 AS AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=8 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN dbo.CT_RegularAwardRanking AS b ON b.RegularID = aa.RegularID
		UNION 
		--���ֲ�����ӽ�
		SELECT b.PlayCode,b.AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=9 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN dbo.CT_RegularBall AS b ON b.RegularID = aa.RegularID
		UNION
		--�ڼ��ռӽ�
		SELECT b.PlayCode,b.AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=10 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN dbo.CT_RegularHoliday AS b ON b.RegularID = aa.RegularID
		UNION
		--Ͷע������޼ӽ�
		SELECT b.PlayCode,b.AwardMoney FROM dbo.CT_Activity AS a 
		INNER JOIN dbo.CT_ActivityAward AS aa 
		ON aa.ActivityID=a.ActivityID AND aa.RegularType=11 AND aa.LotteryCode=@LotteryCode AND a.StartTime <= @Time AND a.EndTime >= @Time
		LEFT JOIN dbo.CT_RegularTopLimit AS b ON b.RegularID = aa.RegularID
	)
	SELECT PlayCode,SUM(AwardMoney) AS AwardMoney FROM tab
	GROUP BY PlayCode
END


GO
/****** Object:  StoredProcedure [dbo].[udp_QueryBuyLotReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-11-7
-- Description:	������ƱͶע��ѯ
-- ɸѡ���û�ID���û��˺ţ���ֹʱ�䣬���֡�ʱ�䡢״̬[�µ��ɹ�����Ʊ�ɹ�������]����Ʊ�̣��������
-- ��ʾ��������ţ����֣�Ͷע���û�ID���û��˺ţ��µ�ʱ�䣬����״̬����Ʊ�̣�Ͷע��ʽ(��ͨͶע����������׷��)
-- ���ܣ�Ͷע���
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryBuyLotReport]
	@StartTime DATETIME,                        --��ֹʱ��_��
	@EndTime DATETIME,                          --��ֹʱ��_ֹ
	@UserID BIGINT,							    --�û�ID
	@UserName VARCHAR(20),                      --�û��˺�
	@LotteryCode INT,			                --����
	@SchemeStatus INT,                          --״̬
	@SchemeNumber VARCHAR(50),                  --�������
	@PrintOutType INT,                          --��Ʊ��
	@PageIndex INT,                             --��ǰҳ
	@PageSize INT,                              --ҳ��С
	@RecordAmount BIGINT OUTPUT,                --Ͷע�ܶ�
	@RecordCount INT OUTPUT                     --��ѯ����
AS
BEGIN
	DECLARE @DataSql NVARCHAR(4000),@RecordSql NVARCHAR(4000),@RecordAmountSql NVARCHAR(2000),@WhereSql NVARCHAR(1500),@OrderSql NVARCHAR(500);
	SET @DataSql = 'SELECT ROW_NUMBER() OVER(ORDER BY s.CreateTime DESC) AS Num,s.SchemeID,s.SchemeNumber,s.LotteryCode,s.SchemeMoney,u.UserID,u.UserName,
				    s.CreateTime,s.SchemeStatus,s.PrintOutType,s.BuyType FROM dbo.CT_Schemes AS s
				    INNER JOIN dbo.CT_UsersRecord AS ur ON ur.UserID=s.InitiateUserID AND s.SchemeID = ur.RelationID AND (ur.TradeType=1 OR ur.TradeType=22)
				    INNER JOIN dbo.CT_Users AS u ON u.UserID = ur.UserID';

	SET @WhereSql = ' WHERE s.CreateTime>='''+ CONVERT(VARCHAR(20),@StartTime) +''' and s.CreateTime<='''+ CONVERT(VARCHAR(20),@EndTime) +'''';

	IF ISNULL(@UserID,0)>0
		BEGIN
			SET @WhereSql = @WhereSql + ' AND u.UserID=' + CONVERT(VARCHAR(20),@UserID);
		END
	IF ISNULL(@UserName,'')!=''
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND u.UserName like ''%'+ @UserName +'%''';
		END
	IF ISNULL(@LotteryCode,0)>0
		BEGIN
			SET @WhereSql = @WhereSql + ' AND s.LotteryCode=' + CONVERT(VARCHAR(20),@LotteryCode);
		END
	IF ISNULL(@SchemeStatus,-1)!=-1
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND s.SchemeStatus=' + CONVERT(VARCHAR(20),@SchemeStatus);
		END
	IF ISNULL(@SchemeNumber,'')!=''
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND s.SchemeNumber like ''%'+ @SchemeNumber +'%''';
		END
	IF ISNULL(@PrintOutType,-1)!=-1
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND s.PrintOutType=' + CONVERT(VARCHAR(20),@PrintOutType);
		END
	SET @WhereSql = @WhereSql + 'GROUP BY s.SchemeID,s.SchemeNumber,s.LotteryCode,s.SchemeMoney,u.UserID,u.UserName,s.CreateTime,s.SchemeStatus,s.PrintOutType,s.BuyType';

	SET @RecordSql = 'SELECT @b = COUNT(1) FROM (SELECT s.SchemeID,s.SchemeNumber,s.LotteryCode,s.SchemeMoney,u.UserID,u.UserName,
				    s.CreateTime,s.SchemeStatus,s.PrintOutType,s.BuyType FROM dbo.CT_Schemes AS s
				    INNER JOIN dbo.CT_UsersRecord AS ur ON ur.UserID=s.InitiateUserID AND s.SchemeID = ur.RelationID AND (ur.TradeType=1 OR ur.TradeType=22)
				    INNER JOIN dbo.CT_Users AS u ON u.UserID = ur.UserID';

	SET @RecordAmountSql = 'SELECT @a = ISNULL(SUM(SchemeMoney),0) FROM dbo.CT_Schemes';

	SET @OrderSql = ' ORDER BY Num
					  OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
					  FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';

	SET @DataSql = @DataSql + @WhereSql + @OrderSql;
	SET @RecordSql = @RecordSql + @WhereSql + ')tab';

	EXEC sp_executesql @DataSql
	EXEC sp_executesql @RecordSql,N'@b INT OUTPUT',@RecordCount OUTPUT
	EXEC sp_executesql @RecordAmountSql,N'@a INT OUTPUT',@RecordAmount OUTPUT
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivity]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-11
-- Description:	��ѯ�Ƿ���ڼӽ��
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivity]
	-- Add the parameters for the stored procedure here
	@LotteryCode INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus,rn.RNormID,rn.PlayCode,rn.AwardMoney,rn.TopLimit FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 AND a.StartTime <= @Time AND a.EndTime >= @Time AND a.CurrencyUnit=0
	INNER JOIN dbo.CT_RegularNorm AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=0 AND aa.LotteryCode=@LotteryCode
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivityAwardInterval]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-13
-- Description:	�н��������ӽ���ѯ
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivityAwardInterval]
	@LotteryCode INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.StartTime,a.EndTime,a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus
	,rn.RAwardIntervalID,rn.PlayCode,rn.AwardInterval FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 AND a.EndTime < @Time AND a.CurrencyUnit=0 AND (aa.RegularStatus = 2 or aa.RegularStatus = 3)
	INNER JOIN dbo.CT_RegularAwardInterval AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=6 AND aa.LotteryCode=@LotteryCode

END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivityAwardRanking]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-13
-- Description:	�н�����ۼ����μӽ�(CT_RegularAwardRanking)
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivityAwardRanking]
	@LotteryCode INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.StartTime,a.EndTime,a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus
	,rn.RAwardRanID,rn.PlayCode,rn.AwardRanking FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 AND a.EndTime < @Time AND a.CurrencyUnit=0 AND (aa.RegularStatus = 2 or aa.RegularStatus = 3)
	INNER JOIN dbo.CT_RegularAwardRanking AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=8 AND aa.LotteryCode=@LotteryCode
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivityBall]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-13
-- Description:	���ֲ������ѯ
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivityBall]
	@LotteryCode INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.StartTime,a.EndTime,a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus
	,rn.RBallID,rn.PlayCode,rn.AwardMoney,rn.TopLimit,rn.BallType,rn.Ball FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 
	AND a.StartTime <= @Time AND a.EndTime >= @Time AND a.CurrencyUnit=0 AND aa.RegularStatus = 2
	INNER JOIN dbo.CT_RegularBall AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=9 AND aa.LotteryCode=@LotteryCode
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivityBetInterval]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-13
-- Description:	Ͷע����ۼ�����ӽ�(CT_RegularBetInterval)
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivityBetInterval]
	@LotteryCode INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.StartTime,a.EndTime,a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus
	,rn.RBetIntervalID,rn.PlayCode,rn.BetInterval FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 AND a.EndTime < @Time AND a.CurrencyUnit=0 AND (aa.RegularStatus = 2 or aa.RegularStatus = 3)
	INNER JOIN dbo.CT_RegularBetInterval AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=5 AND aa.LotteryCode=@LotteryCode
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivityBetRanking]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-13
-- Description:	Ͷע����ۼ����μӽ�(CT_RegularBetRanking)
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivityBetRanking]
	@LotteryCode INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.StartTime,a.EndTime,a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus
	,rn.RBetRanID,rn.PlayCode,rn.BetRanking FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 AND a.EndTime < @Time AND a.CurrencyUnit=0 AND (aa.RegularStatus = 2 or aa.RegularStatus = 3)
	INNER JOIN dbo.CT_RegularBetRanking AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=5 AND aa.LotteryCode=@LotteryCode
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivityChase]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-11
-- Description:	׷�żӽ���ѯ
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivityChase]
	@LotteryCode INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.StartTime,a.EndTime,a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus,rn.RChaseID,rn.PlayCode,rn.RChaseType,rn.Unit,rn.AwardMoney,rn.TopLimit FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 AND a.EndTime < @Time AND a.CurrencyUnit=0 AND (aa.RegularStatus=2 OR aa.RegularStatus=3 )
	INNER JOIN dbo.CT_RegularChase AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=1 AND aa.LotteryCode=@LotteryCode
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivityDanTuo]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		joan
-- Create date: 2017-9-11
-- Description:	��ѯ�Ƿ���ڼӽ��
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivityDanTuo]
	-- Add the parameters for the stored procedure here
	@LotteryCode INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus,rn.RDanTuoID,
	rn.PlayCode,rn.AwardMoney,rn.TopLimit,rn.DanNums,rn.TuoNums FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 AND a.StartTime <= @Time AND a.EndTime >= @Time AND a.CurrencyUnit=0
	INNER JOIN dbo.CT_RegularDanTuo AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=2 AND aa.LotteryCode=@LotteryCode
END


GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivityHoliday]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-12
-- Description:	��ѯ�ڼ��ռӽ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivityHoliday]
	@LotteryCode INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.StartTime,a.EndTime,a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus
	,rn.RHolidayID,rn.PlayCode,rn.HolidayType,rn.AwardMoney,rn.TopLimitDay FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 AND a.StartTime <= @Time AND a.EndTime >= @Time AND a.CurrencyUnit=0
	INNER JOIN dbo.CT_RegularHoliday AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=10 AND aa.LotteryCode=@LotteryCode
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryIsAwardActivityTopLimit]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-9-14
-- Description:	Ͷע������޼ӽ���ѯ
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryIsAwardActivityTopLimit]
	@LotteryCode INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE()
	SELECT a.ActivityID,a.ActivityType,a.ActivityMoney,a.ModifyMoney,a.IsModify,aa.RegularID,aa.LotteryCode,aa.TotalAwardMoney,aa.RegularStatus,
	rn.RTopLimitID,rn.PlayCode,rn.AwardMoney,rn.TotalMoney FROM dbo.CT_Activity AS a 
	INNER JOIN dbo.CT_ActivityAward AS aa ON aa.ActivityID = a.ActivityID AND a.ActivityApply=1 AND a.EndTime < @Time 
	AND a.CurrencyUnit=0 AND (aa.RegularStatus = 2 OR aa.RegularStatus = 3 )
	INNER JOIN dbo.CT_RegularTopLimit AS rn ON rn.RegularID = aa.RegularID AND aa.RegularType=11 AND aa.LotteryCode=@LotteryCode
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryOutTicketReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-11-8
-- Description:	��Ʊ��ѯͳ��
-- ɸѡ�����֣���Ʊʱ�䣬��Ʊ�̣��������
-- ��ʾ��������ţ����֣���Ʊ�̣���Ʊʱ�䣬����������״̬������Ʊ�ţ�Ͷע���ݣ��н����
-- ���ܣ������ܶ�н����
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryOutTicketReport]
	@StartTime DATETIME,                        --��ֹʱ��_��
	@EndTime DATETIME,                          --��ֹʱ��_ֹ
	@LotteryCode INT,			                --����
	@SourceID INT,			                    --��Ʊ��
	@SchemeNumber VARCHAR(50),                  --�������
	@PageIndex INT,                             --��ǰҳ
	@PageSize INT,                              --ҳ��С
	@RecordAmount BIGINT OUTPUT,                --�����ܶ�
	@RecordWin BIGINT OUTPUT,                   --�н��ܶ�
	@RecordCount INT OUTPUT                     --��ѯ����
AS
BEGIN
	DECLARE @DataSql NVARCHAR(4000),@RecordSql NVARCHAR(4000),@RecordAmountSql NVARCHAR(2000),@WhereSql NVARCHAR(1500),@OrderSql NVARCHAR(500);
	SET @DataSql = 'SELECT ROW_NUMBER() OVER(ORDER BY se.CreateTime DESC) AS Num,se.TicketMoney,s.SchemeID,s.SchemeNumber,s.LotteryCode,se.TicketSourceID,se.HandleDateTime,s.SchemeStatus,se.Ticket,se.Number,se.WinMoney FROM dbo.CT_Schemes AS s 
				    INNER JOIN dbo.CT_SchemeETickets AS se ON se.SchemeID = s.SchemeID';

    SET @WhereSql = ' WHERE se.HandleDateTime>='''+ CONVERT(VARCHAR(20),@StartTime) +''' and se.HandleDateTime<='''+ CONVERT(VARCHAR(20),@EndTime) +'''';
	IF ISNULL(@LotteryCode,-1)!=-1
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND s.LotteryCode=' + CONVERT(VARCHAR(10),@LotteryCode);
		END
	IF ISNULL(@SourceID,-1)!=-1
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND se.TicketSourceID=' + CONVERT(VARCHAR(10),@SourceID);
		END
	IF ISNULL(@SchemeNumber,'')!=''
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND s.SchemeNumber  like ''%'+ @SchemeNumber +'%''';
		END

	SET @RecordSql = 'SELECT @c = COUNT(1) FROM dbo.CT_Schemes AS s 
				      INNER JOIN dbo.CT_SchemeETickets AS se ON se.SchemeID = s.SchemeID';

	SET @RecordAmountSql = 'SELECT @a = SUM(s.SchemeMoney),@b = SUM(se.WinMoney) FROM dbo.CT_Schemes AS s 
						    INNER JOIN dbo.CT_SchemeETickets AS se ON se.SchemeID = s.SchemeID';
	
	SET @OrderSql = ' ORDER BY Num
					  OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
					  FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';

	SET @DataSql = @DataSql + @WhereSql + @OrderSql;
	SET @RecordSql = @RecordSql + @WhereSql ;
	
	PRINT @RecordSql;
	EXEC sp_executesql @DataSql
	EXEC sp_executesql @RecordSql,N'@c INT OUTPUT',@RecordCount OUTPUT
	EXEC sp_executesql @RecordAmountSql,N'@a INT OUTPUT,@b INT OUTPUT',@RecordAmount OUTPUT,@RecordWin OUTPUT
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryPushList]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-10-7
-- Description:	��ѯ�����û���Ϣ
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryPushList]
	@UserName VARCHAR(25)   --�û���
AS
BEGIN
	SET NOCOUNT ON;
	IF ISNULL(@UserName,'') = ''
		BEGIN 
			SET @UserName = '';
		END
	SELECT TOP 20 u.UserID,u.UserName,p.PushIdentify FROM dbo.CT_Users AS u 
	INNER JOIN dbo.CT_UsersPush AS p ON p.UserId=u.UserID
	WHERE u.UserName LIKE '%' + @UserName + '%'
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QuerySchemeRecord]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:    Joan
-- Create date: 2017-07-21
-- Description:  Ӷ����ϸ
-- =============================================
CREATE PROCEDURE [dbo].[udp_QuerySchemeRecord]
  @UserID INT,                   --��Ա���
  @IsWin INT,                    --�Ƿ��н�
  @IsChase INT,                  --�Ƿ�׷��
  @PageIndex INT,                --��ǰҳ
  @PageSize INT                 --ÿҳ��С
  --WITH ENCRYPTION
AS
BEGIN
  SET XACT_ABORT ON
  DECLARE @SQLStr NVARCHAR(4000),@WhereStr NVARCHAR(1000),@SQLStrCount NVARCHAR(2000);
  SET NOCOUNT ON;
  --��ѯ��ҳ
  SET @SQLStr = 'select ROW_NUMBER() OVER(ORDER BY s.SchemeID DESC) AS Num,s.SchemeID as OrderCode,s.CreateTime as OrderTime,i.LotteryName,
				s.SchemeMoney as Amount,s.SchemeStatus  as  OrderStatus,s.BuyType,s.IsuseName as IsuseNum,sum(isnull(w.WinMoneyNoWithTax,0)) as WinMoney from CT_Schemes as s
				inner join CT_Lotteries as i on i.LotteryCode = s.LotteryCode
				left join CT_SchemesWin w on w.SchemeID = s.SchemeID ';
	
	SET @WhereStr = 'where s.InitiateUserID = ' + CONVERT(VARCHAR(20),@UserID);
	IF @IsWin = 1
		BEGIN 
			SET @WhereStr = @WhereStr + ' and WinMoney > 0 ';
		END
	IF @IsChase = 1
		BEGIN 
			SET @WhereStr = @WhereStr + ' and s.BuyType = 1 ';
		END
	SET @WhereStr = @WhereStr + ' group by s.SchemeID,s.CreateTime,i.LotteryName,
				s.SchemeMoney,s.SchemeStatus,s.BuyType,s.IsuseName';

    SET @SQLStr = @SQLStr + @WhereStr;

  SET @SQLStr = @SQLStr + ' ORDER BY Num
  OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
  FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';

  PRINT @SQLStr
  --��ѯ�ܼ�¼�� 
  EXEC sp_executesql @SQLStr
  SET NOCOUNT OFF;
END






GO
/****** Object:  StoredProcedure [dbo].[udp_QuerySystemStaticdata]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-11-4
-- Description:	��ѯ��̬����
-- =============================================
CREATE PROCEDURE [dbo].[udp_QuerySystemStaticdata]
	@Month VARCHAR(10),
	@RecordBuy BIGINT OUTPUT,
	@RecordWin BIGINT OUTPUT,
	@RecordUsers BIGINT OUTPUT,
	@RecordRecharge BIGINT OUTPUT,
	@RecordLargess BIGINT OUTPUT,
	@RecordWithdraw BIGINT  OUTPUT
AS
BEGIN
	
	--ע������ ��ֵ���� ��������
	SELECT @RecordUsers = ISNULL(SUM(users),0),@RecordRecharge = ISNULL(SUM(recharge),0),@RecordLargess = ISNULL(SUM(largess),0),@RecordWithdraw = ISNULL(SUM(withdraw),0) FROM CT_SystemStaticdata;
	--Ͷע���
	WITH tabbuy AS 
    (
		SELECT SUM(buy_jlk) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(buy_jxk) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(buy_hbsyydj) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(buy_sdsyydj) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(buy_cqssc) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(buy_ssq) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(buy_dlt) AS buy FROM CT_SystemStaticdata
	)
	SELECT @RecordBuy = ISNULL(SUM(tabbuy.buy),0) FROM tabbuy;
	--�н����
	WITH tabwin AS 
    (
		SELECT SUM(win_jlk) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(win_jxk) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(win_hbsyydj) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(win_sdsyydj) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(win_cqssc) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(win_ssq) AS buy FROM CT_SystemStaticdata
		UNION 
		SELECT SUM(win_dlt) AS buy FROM CT_SystemStaticdata
	)
	SELECT @RecordWin = ISNULL(SUM(tabwin.buy),0) FROM tabwin;


	SELECT dateday,recharge,online_recharge,offline_recharge,withdraw,users,largess,buy_jlk, 
           win_jlk,buy_jxk,win_jxk,buy_hbsyydj,win_hbsyydj,buy_sdsyydj,win_sdsyydj, 
           buy_cqssc,win_cqssc,buy_ssq,win_ssq,buy_dlt,win_dlt 
	FROM CT_SystemStaticdata 
    WHERE SUBSTRING(dateday,1,7) = SUBSTRING(@Month,1,7)
END

GO
/****** Object:  StoredProcedure [dbo].[udp_QueryWinAwardReport]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-11-8
-- Description:	�н���ѯͳ��
-- ɸѡ���������֡�ʱ�䡢�û�ID���û��˺š��ֻ����롢�������
-- ��ʾ��������ţ����֣��û�ID���û��˺ţ��µ�ʱ�䣬����״̬����Ʊ�̣� Ͷע���н����
-- ���ܣ�Ͷע���н����   
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryWinAwardReport]
	@StartTime DATETIME,                        --��ֹʱ��_��
	@EndTime DATETIME,                          --��ֹʱ��_ֹ
	@UserID BIGINT,							    --�û�ID
	@UserName VARCHAR(20),                      --�û��˺�
	@Mobile VARCHAR(20),                        --�û��ֻ�
	@LotteryCode INT,			                --����
	@SchemeNumber VARCHAR(50),                  --�������
	@PageIndex INT,                             --��ǰҳ
	@PageSize INT,                              --ҳ��С
	@RecordAmount BIGINT OUTPUT,                --Ͷע�ܶ�
	@RecordWin BIGINT OUTPUT,                --�н��ܶ�
	@RecordCount INT OUTPUT                     --��ѯ����
AS
BEGIN
	DECLARE @DataSql NVARCHAR(4000),@RecordSql NVARCHAR(4000),@RecordAmountSql NVARCHAR(2000),@WhereSql NVARCHAR(1500),@OrderSql NVARCHAR(500);
	SET @DataSql = 'SELECT ROW_NUMBER() OVER(ORDER BY s.CreateTime DESC) AS Num,s.SchemeID,s.SchemeNumber,s.LotteryCode,u.UserID,u.UserName,s.CreateTime,s.SchemeStatus,s.PrintOutType,s.SchemeMoney,(SUM(se.WinMoney)) AS TradeAmount,u.UserMobile FROM CT_Schemes AS s
					INNER JOIN CT_Users AS u ON u.UserID=s.InitiateUserID
					INNER JOIN dbo.CT_SchemeETickets AS se  ON se.SchemeID=s.SchemeID AND se.TicketStatus=10';

	SET @WhereSql = ' WHERE s.CreateTime>='''+ CONVERT(VARCHAR(20),@StartTime) +''' and s.CreateTime<='''+ CONVERT(VARCHAR(20),@EndTime) +'''';
	IF ISNULL(@UserID,-1)>0
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND u.UserID='+CONVERT(VARCHAR(20),@UserID);
		END
	IF ISNULL(@UserName,'')!=''
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND u.UserName  like ''%'+ @UserName +'%''';
		END
	IF ISNULL(@Mobile,'')!=''
		BEGIN
			SET @WhereSql = @WhereSql + ' AND u.UserMobile  like ''%'+ @Mobile +'%''';
		END
	IF ISNULL(@LotteryCode,-1)!=-1
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND s.LotteryCode='+CONVERT(VARCHAR(20),@LotteryCode);
		END
	IF ISNULL(@SchemeNumber,'')!=''
		BEGIN
		    SET @WhereSql = @WhereSql + ' AND s.SchemeNumber  like ''%'+ @SchemeNumber +'%''';
		END

		
	SET @RecordSql = 'SELECT @b = COUNT(1) FROM( SELECT ROW_NUMBER() OVER(ORDER BY s.CreateTime DESC) AS Num,s.SchemeID,s.SchemeNumber,s.LotteryCode,u.UserID,u.UserName,s.CreateTime,s.SchemeStatus,s.PrintOutType,s.SchemeMoney,(SUM(se.WinMoney)) AS TradeAmount,u.UserMobile FROM CT_Schemes AS s
					INNER JOIN CT_Users AS u ON u.UserID=s.InitiateUserID
					INNER JOIN dbo.CT_SchemeETickets AS se  ON se.SchemeID=s.SchemeID AND se.TicketStatus=10';


	SET @RecordAmountSql = 'SELECT @a = ISNULL(SUM(se.WinMoney),0),@c = ISNULL(SUM(se.TicketMoney),0) FROM CT_Schemes AS s
							INNER JOIN CT_Users AS u ON u.UserID=s.InitiateUserID
							INNER JOIN dbo.CT_SchemeETickets AS se  ON se.SchemeID=s.SchemeID ';

	SET @OrderSql = ' ORDER BY Num
					  OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
					  FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';
					  
	SET @WhereSql = @WhereSql + 'GROUP BY s.SchemeID,s.SchemeNumber,s.LotteryCode,u.UserID,u.UserName,s.CreateTime,s.SchemeStatus,s.PrintOutType,s.SchemeMoney,u.UserMobile';
	SET @RecordSql = @RecordSql + @WhereSql + ')tab' ;
	SET @DataSql = @DataSql + @WhereSql + @OrderSql;
	

	EXEC sp_executesql @DataSql
	EXEC sp_executesql @RecordSql,N'@b INT OUTPUT',@RecordCount OUTPUT
	EXEC sp_executesql @RecordAmountSql,N'@a INT OUTPUT,@c INT OUTPUT',@RecordWin OUTPUT,@RecordAmount OUTPUT
END

GO
/****** Object:  StoredProcedure [dbo].[udp_RecordWinPrize]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--��¼�н���Ϣ ����ִ�нű�ʹ��
--2017-5-25
CREATE PROC [dbo].[udp_RecordWinPrize]
	@SchemeID BIGINT,	--������ID
	@SchemeETicketsID BIGINT,	--����ƱID
	@LotteryCode INT,	--���ֱ��
	@WinCode INT,	--���ȱ���
	@WinNumber VARCHAR(128),	--Ͷע����
	@WinMoney INT,	--�н�����
	@WinMoneyNoWithTax INT,	--˰���н�����
	@Descriptions VARCHAR(128), --����
	@IsFirstPrize INT=0 --1.������˫ɫ�����͸�Ȳ����Զ�����һ�Ƚ��Ͷ��Ƚ�����Ĳ���
	--WITH ENCRYPTION
AS
	DECLARE @UserID INT, @Multiple INT, @SplitNumber VARCHAR(128),@SDID BIGINT,@SchemesWinCount INT;
	
	--���ܼ�״̬  �����п��¼�ύ Ҫ���ύǰ��״̬�ж�
	SELECT @UserID=InitiateUserID FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID --AND OrderStatus IN(6,8)
	IF @@ROWCOUNT = 0
	BEGIN
		RETURN -1
	END
	SELECT @Multiple=Multiple, @SplitNumber=Number,@SDID=SDID FROM dbo.CT_SchemeETickets WHERE SchemeETicketsID=@SchemeETicketsID AND TicketStatus=2
	IF @@ROWCOUNT = 0
	BEGIN
		RETURN -2
	END
	

	IF @IsFirstPrize = 0
	BEGIN
		UPDATE dbo.CT_SchemeETickets SET WinMoney=@WinMoneyNoWithTax,TicketStatus=CASE WHEN @WinMoneyNoWithTax>0 THEN 10 ELSE 11 END, HandleDescribe = HandleDescribe+','+@Descriptions 
			WHERE SchemeETicketsID=@SchemeETicketsID AND TicketStatus=2
		IF @WinMoneyNoWithTax > 0  --һ��Ľ��ȸ��ݽ�������¼�н���¼
		BEGIN
			SELECT @SchemesWinCount = COUNT(1) FROM CT_SchemesWin WHERE SchemeID = @SchemeID AND SchemeETicketsID = @SchemeETicketsID;
			IF @SchemesWinCount=0
				BEGIN
				    INSERT dbo.CT_SchemesWin (SchemeETicketsID, SchemeID, UserID, LotteryCode, WinCode, SplitNumber, WinNumber, Multiple, WinMoney, WinMoneyNoWithTax, Descriptions, AddDateTime,IsFirstPrize)
					VALUES(@SchemeETicketsID, @SchemeID, @UserID, @LotteryCode, @WinCode, @SplitNumber, @WinNumber, @Multiple, @WinMoney, @WinMoneyNoWithTax, @Descriptions, GETDATE(),0)
				END
		END
		
		--��ʼ�ɽ� Strat ��Ƶ���ɽ�  ��Ƶ�ʽӿ��ɽ�
		UPDATE CT_SchemesDetail SET WinMoney=@WinMoney,WinMoneyNoWithTax=@WinMoneyNoWithTax,UpdateTime=GETDATE(),WinDescribe='�н�' WHERE SDID=@SDID
		IF @@ROWCOUNT = 0
		BEGIN
			RETURN -3
		END
		
		INSERT dbo.CT_UsersRecord(UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,CreateTime ,RelationID ,OperatorID)
		SELECT @UserID, 5, @WinMoneyNoWithTax, (Balance+@WinMoneyNoWithTax), '����Ʊ�н�', GETDATE(), @SchemeETicketsID, 1 AS OperatorID FROM CT_Users WHERE UserID = @UserID
		IF @@ROWCOUNT = 0
		BEGIN
			RETURN -4
		END
		
		UPDATE dbo.CT_Users SET Balance =Balance + @WinMoneyNoWithTax WHERE UserID = @UserID
		IF @@ROWCOUNT = 0
		BEGIN
			RETURN -5
		END
		--�����ֽ�� �н�����ۼ�
		UPDATE dbo.CT_UsersExtend SET WithdrawMoney = WithdrawMoney + @WinMoneyNoWithTax WHERE UserID = @UserID
		IF @@ROWCOUNT = 0
		BEGIN
			RETURN -5
		END
		--�����ֽ�� E
		--��ʼ�ɽ� End
	END
	ELSE IF @IsFirstPrize = 1 --1.������˫ɫ�� ����͸�� �����Զ����㽱��Ĳ���
	BEGIN
		--@WinMoney @WinMoneyNoWithTax �������Լ����㽱�����ж�Ϊ��
		UPDATE dbo.CT_SchemeETickets SET TicketStatus=8, HandleDescribe = HandleDescribe+','+@Descriptions WHERE SchemeETicketsID=@SchemeETicketsID AND TicketStatus=2
		
		INSERT dbo.CT_SchemesWin (SchemeETicketsID, SchemeID, UserID, LotteryCode, WinCode, SplitNumber, WinNumber, Multiple, WinMoney, WinMoneyNoWithTax, Descriptions, AddDateTime,IsFirstPrize)
			VALUES(@SchemeETicketsID, @SchemeID, @UserID, @LotteryCode, @WinCode, @SplitNumber, @WinNumber, @Multiple, @WinMoney, @WinMoneyNoWithTax, @Descriptions, GETDATE(),1)
	END

	RETURN 0	




GO
/****** Object:  StoredProcedure [dbo].[udp_ReplenishAward]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joan
-- Create date: 2017-11-28
-- Description:	�����ɽ�
-- =============================================
CREATE PROCEDURE [dbo].[udp_ReplenishAward]
	@AwardTable XML --�������б�
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		DECLARE @Time DATETIME = GETDATE(),@SchemeETicketID BIGINT,@SchemeID BIGINT,@WinMoney BIGINT,@WinMoneyNoWithTax BIGINT,@BuyType TINYINT,@UserID BIGINT,@Balance BIGINT;
		DECLARE @ItemAwardTable TABLE(tid BIGINT/*����Ʊ��ʶ*/,uuid BIGINT/*�û���ʶ*/,wm BIGINT/*˰ǰ����*/,nwm BIGINT/*˰�󽱽�*/);
		
		INSERT INTO @ItemAwardTable
		        ( tid, uuid, wm, nwm )
		SELECT T.c.value('(tid/text())[1]','BIGINT'), 
			   T.c.value('(uuid/text())[1]','BIGINT'), 
			   T.c.value('(wm/text())[1]','BIGINT'), 
			   T.c.value('(nwm/text())[1]','BIGINT')
		FROM @AwardTable.nodes('/ArrayOfUdv_Award/udv_Award') AS T(c);
		SELECT @SchemeETicketID = tid FROM @ItemAwardTable ORDER BY tid ASC;
		WHILE(ISNULL(@SchemeETicketID,0)!=0)
			BEGIN
				SELECT @SchemeID = SchemeID FROM dbo.CT_SchemeETickets WHERE SchemeETicketsID = @SchemeETicketID;
				SELECT @BuyType = BuyType FROM dbo.CT_Schemes WHERE SchemeID = @SchemeID;
				SELECT @WinMoneyNoWithTax = nwm,@WinMoney = wm,@UserID = uuid FROM @ItemAwardTable WHERE tid = @SchemeETicketID;
				IF ISNULL(@WinMoneyNoWithTax,0)>0
					BEGIN
						--�û�������
						UPDATE dbo.CT_Users SET Balance = Balance + @WinMoneyNoWithTax WHERE UserID = @UserID;
						SELECT @Balance = Balance FROM dbo.CT_Users WHERE UserID=@UserID;
						--���µ���Ʊ
						UPDATE dbo.CT_SchemeETickets SET WinMoney = @WinMoneyNoWithTax,TicketStatus=10 WHERE SchemeETicketsID = @SchemeETicketID;
						--���·�������
						UPDATE dbo.CT_SchemesDetail SET WinMoney = WinMoney + @WinMoney,WinMoneyNoWithTax = WinMoneyNoWithTax + @WinMoneyNoWithTax,UpdateTime=@Time WHERE SchemeID = @SchemeID;
						INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
						VALUES (@UserID,5,@WinMoneyNoWithTax,@Balance,'�н�',@SchemeETicketID,@UserID,@Time,0);

						IF	@BuyType != 1
							BEGIN
							    UPDATE dbo.CT_Schemes SET SchemeStatus=14 WHERE SchemeID = @SchemeID;
							END
					END
				ELSE
					BEGIN
					    --���µ���Ʊ
						UPDATE se SET se.TicketStatus = 11 FROM @ItemAwardTable AS it
						INNER JOIN dbo.CT_SchemeETickets AS se ON se.SchemeETicketsID = it.tid
						WHERE it.tid = @SchemeETicketID;
						IF	@BuyType != 1
							BEGIN
							    UPDATE dbo.CT_Schemes SET SchemeStatus=18 WHERE SchemeID = @SchemeID;
							END
					END
				SELECT @SchemeETicketID = tid FROM @ItemAwardTable WHERE tid > @SchemeETicketID ORDER BY tid ASC;
			END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	SELECT 0;
END

GO
/****** Object:  StoredProcedure [dbo].[udp_Replenishment]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		joan
-- Create date: 2017-08-15
-- Description:	��ֵ����
-- =============================================
CREATE PROCEDURE [dbo].[udp_Replenishment]
	@PayID BIGINT, 
	@RechargeNo VARCHAR(200),
	@OutRechargeNo VARCHAR(200),
	@ReturnValue INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @UserID BIGINT,@Amount BIGINT,@Result SMALLINT,@OrderNo VARCHAR(50),@Balance BIGINT,@Time DATETIME = GETDATE();
	BEGIN TRAN
	SELECT @Result=Result,@Amount=Amount,@UserID=UserID,@OrderNo=OrderNo FROM CT_UsersPayDetail WHERE PayID=@PayID
	IF @Result = 0
		BEGIN
			UPDATE CT_UsersPayDetail SET RechargeNo = @RechargeNo,OutRechargeNo = @OutRechargeNo,CompleteTime=@Time,Result=1 WHERE PayID=@PayID
			IF @@ROWCOUNT = 0
			  BEGIN
				ROLLBACK TRAN 
				SELECT @ReturnValue = -3
				RETURN -3  --�ӿڽ��׺Ÿ���ʧ��
			  END
			UPDATE CT_Users SET Balance=Balance+@Amount WHERE UserID=@UserID
			IF @@ROWCOUNT = 0
			  BEGIN
				ROLLBACK TRAN 
				SELECT @ReturnValue = -4
				RETURN -4  --�û�������ʧ��
			  END
			SELECT @Balance=Balance FROM dbo.CT_Users WHERE UserID=@UserID
			INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
			VALUES  ( @UserID ,0 ,@Amount ,@Balance ,N'��ֵ����' ,@OrderNo , 1 ,@Time ,0)
			IF @@ROWCOUNT = 0
			  BEGIN
				ROLLBACK TRAN 
				SELECT @ReturnValue = -3
				RETURN -3  --�û�������ʧ��
			  END
		END
	COMMIT TRAN
	SELECT @ReturnValue = 0
	RETURN 0
END



GO
/****** Object:  StoredProcedure [dbo].[udp_RevokeChase]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--(����������
--2017-5-9
CREATE PROCEDURE [dbo].[udp_RevokeChase]
	@ChaseTaskID BIGINT,	-- ׷������ID
	@isSystemQuash BIT,		-- �Ƿ�ϵͳ����

	@ReturnValue INT OUTPUT,
	@ReturnDescription VARCHAR(512) OUTPUT
AS
	SET NOCOUNT ON;
	
	DECLARE @SumIsuseNum INT, 
			@BuyedIsuseNum INT, 
			@QuashedIsuseNum INT, 
			@QuashStatus SMALLINT, 
			@UserID BIGINT, 
			@LotteryName VARCHAR(30),
			@Balance BIGINT

	SELECT @UserID = a.UserID, @LotteryName = c.LotteryName, @QuashStatus = a.QuashStatus,
			@SumIsuseNum = ISNULL(d.SumIsuseNum, 0) , --������
			@BuyedIsuseNum = ISNULL(d.BuyedIsuseNum, 0) , --�������
			@QuashedIsuseNum = ISNULL(d.QuashedIsuseNum, 0) --ȡ������
	FROM dbo.CT_ChaseTasks a
		LEFT JOIN dbo.CT_Users b ON a.UserID = b.UserID 
		LEFT JOIN dbo.CT_Lotteries c ON a.LotteryCode = c.LotteryCode
		LEFT JOIN (
			SELECT ChaseTaskID, COUNT(*) AS SumIsuseNum, 
				COUNT(CASE WHEN IsExecuted=1 AND QuashStatus=0 THEN 1 END) AS BuyedIsuseNum,
				COUNT(CASE WHEN QuashStatus <> 0 THEN 1 END) AS QuashedIsuseNum
			FROM dbo.CT_ChaseTaskDetails GROUP BY ChaseTaskID
		) d ON d.ChaseTaskID = a.ChaseTaskID
	WHERE a.ChaseTaskID=@ChaseTaskID
	IF @@ROWCOUNT = 0
	BEGIN
		SET @ReturnValue = -1
		SET @ReturnDescription = '׷�����񲻴���'
		RETURN -1
	END

	IF @QuashStatus > 0
	BEGIN
		SET @ReturnValue = -2
		SET @ReturnDescription = '׷�������ѱ�' + CASE @QuashStatus WHEN 1 THEN '�û�' WHEN 2 THEN 'ϵͳ' ELSE '' END + '����'
		RETURN -2
	END

	IF (@BuyedIsuseNum + @QuashedIsuseNum >= @SumIsuseNum)
	BEGIN
		SET @ReturnValue = -3
		SET @ReturnDescription = '׷�������Ѿ���ֹ'
		RETURN -3
	END

	DECLARE @Money INT=0, @SumMoney INT=0, @ChaseTaskDetailID BIGINT, @AllIsuseName VARCHAR(1024), @IsuseID BIGINT
	
	--��ȡû��ִ��û�г�����
	SELECT @ChaseTaskDetailID = MIN(ID) FROM dbo.CT_ChaseTaskDetails WHERE ChaseTaskID = @ChaseTaskID AND QuashStatus=0 AND IsExecuted=0
	WHILE @ChaseTaskDetailID IS NOT NULL
	BEGIN
		SELECT @Money = Amount, @IsuseID = IsuseID FROM dbo.CT_ChaseTaskDetails WHERE ID=@ChaseTaskDetailID
		SELECT @AllIsuseName = @AllIsuseName +','+ IsuseName FROM dbo.CT_Isuses WHERE IsuseID=@IsuseID
		SET @SumMoney = @SumMoney+@Money

		UPDATE CT_ChaseTaskDetails SET QuashStatus = (CASE @isSystemQuash WHEN 0 THEN 1 ELSE 2 END) WHERE ID = @ChaseTaskDetailID

		SELECT @ChaseTaskDetailID = MIN(ID) FROM dbo.CT_ChaseTaskDetails WHERE ChaseTaskID = @ChaseTaskID AND QuashStatus=0 AND IsExecuted=0 AND ID>@ChaseTaskDetailID
	END

	--�����û����
	UPDATE dbo.CT_Users SET Balance = Balance + @SumMoney WHERE UserID = @UserID
	
	SELECT @Balance=Balance FROM dbo.CT_Users WHERE UserID = @UserID
	
	INSERT INTO CT_UsersRecord (UserID, TradeType, TradeAmount, TradeRemark, CreateTime, RelationID, OperatorID,Balance)
		VALUES(@UserID, dbo.F_GetDetailsOperatorType('ϵͳ����'), @SumMoney, '����������' + @LotteryName+'��' + @AllIsuseName + '׷������', GETDATE(), @ChaseTaskDetailID, 1,@Balance)

	UPDATE CT_ChaseTasks set QuashStatus = (CASE @isSystemQuash WHEN 0 THEN 1 ELSE 2 END) WHERE ChaseTaskID = @ChaseTaskID

	SET @ReturnValue = 0
	SET @ReturnDescription = @AllIsuseName
	RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[udp_RevokeChaseDetail]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--������ϸ׷��
--2017-5-9
CREATE PROC [dbo].[udp_RevokeChaseDetail]
	@ChaseTaskDetailID BIGINT,	-- ׷����ϸ����ID
	@isSystemQuash BIT,			-- �Ƿ�ϵͳ����
	@ReturnValue INT OUTPUT,
	@ReturnDescription VARCHAR(100) OUTPUT
AS
	SET NOCOUNT ON;

	DECLARE @UserID BIGINT, 
			@Money INT, 
			@LotteryName VARCHAR(20), 
			@IsuseID BIGINT, 
			@IsuseName VARCHAR(20), 
			@ChaseTaskID BIGINT, 
			@QuashStatus SMALLINT, 
			@Executed BIT,
			@Balance BIGINT

	SELECT @ChaseTaskID=ChaseTaskID, @QuashStatus=QuashStatus, @Executed=IsExecuted, @Money=Amount, @IsuseID=IsuseID FROM dbo.CT_ChaseTaskDetails WHERE ID=@ChaseTaskDetailID
	IF @@ROWCOUNT = 0
		BEGIN
			SET @ReturnValue = -1
			SET @ReturnDescription = '׷����ϸ���񲻴���'
			RETURN -1
		END
	
	SELECT @UserID=UserID FROM dbo.CT_ChaseTasks WHERE ChaseTaskID=@ChaseTaskID
	SELECT @IsuseName=a.IsuseName, @LotteryName=b.LotteryName FROM dbo.CT_Isuses a
		LEFT JOIN dbo.CT_Lotteries b ON b.LotteryCode = a.LotteryCode
	WHERE a.IsuseID=@IsuseID

	IF @QuashStatus > 0
		BEGIN
			SET @ReturnValue = -2
			SET @ReturnDescription = '׷����ϸ�����ѱ�' + CASE @QuashStatus WHEN 1 THEN '�û�' WHEN 2 THEN 'ϵͳ' ELSE '' END + '����'
			RETURN -2
		END
	IF @Executed = 1
		BEGIN
			SET @ReturnValue = -3
			SET @ReturnDescription = '׷����ϸ�����Ѿ���ִ�У�Ҫ�����������볷���������ִ�з���'
			RETURN -3
		END

	-- ����һ��׷��
	UPDATE CT_ChaseTaskDetails SET QuashStatus = (CASE @isSystemQuash WHEN 0 THEN 1 ELSE 2 END) WHERE ID = @ChaseTaskDetailID
	--�����û����
	--UPDATE dbo.CT_Users SET Balance = Balance + @Money, Freeze = Freeze - @Money WHERE UserID = @UserID
	UPDATE dbo.CT_Users SET Balance = Balance + @Money WHERE UserID = @UserID
	
	SELECT @Balance=Balance FROM dbo.CT_Users WHERE UserID = @UserID
	
	INSERT INTO CT_UsersRecord (UserID, TradeType, TradeAmount, TradeRemark, CreateTime, RelationID, OperatorID,Balance)
		VALUES(@UserID, dbo.F_GetDetailsOperatorType('ϵͳ����'), @Money, '����' + @LotteryName + @IsuseName + '׷������', GETDATE(), @ChaseTaskDetailID, 1,@Balance)

	SET @ReturnValue = 0
	SET @ReturnDescription = @IsuseName
	RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[udp_RevokeScheme]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--������������
--2017-5-9
CREATE PROC [dbo].[udp_RevokeScheme]
  @SchemeID  BIGINT,
  @IsSystemQuash  BIT,  --�Ƿ�ϵͳ����
  @ReturnValue  INT OUTPUT,
  @ReturnDescription VARCHAR(100) OUTPUT
  --WITH ENCRYPTION
AS
  --SET NOCOUNT ON
  SELECT @ReturnValue=0, @ReturnDescription=''
  
  DECLARE @UserID BIGINT, @IsuseID BIGINT, @Money BIGINT, --�ܽ�� 
    @Share  INT,  --�������
    @BuyedShare INT  ,  --���������
    @AssureMoney BIGINT=0, --���򱣵׽��
    @QuashStatus SMALLINT, --����״̬
    @IsBuyed BIT, --�Ƿ��ѳ�Ʊ
    @IsOpened BIT, --����״̬
    @BuyType TINYINT, --��������
    @StartTime DATETIME, --Ͷע��ʼʱ��
    @EndTime DATETIME, --Ͷע����ʱ��
	@GoldBean BIGINT, --�ʶ�
	@Balance BIGINT,  --���
	@Time DATETIME = GETDATE(); --��ǰʱ��

  SELECT @UserID = InitiateUserID, @IsuseID=IsuseID, @QuashStatus=SchemeStatus, @IsBuyed=IsSendOut, @BuyType=BuyType 
    FROM dbo.CT_Schemes WHERE SchemeID = @SchemeID
  IF @@ROWCOUNT = 0
  BEGIN
    SELECT @ReturnValue=-1, @ReturnDescription = '������' + CONVERT(VARCHAR(16),@SchemeID) + '������!'
    RETURN -1
  END
  --�������
  SELECT @Money = TradeAmount FROM CT_UsersRecord 
  WHERE UserID=@UserID AND TradeType=1 AND RelationID=CONVERT(VARCHAR(32),@SchemeID)


  SELECT @StartTime=StartTime, @EndTime=EndTime,@IsOpened = IsOpened FROM dbo.CT_Isuses WHERE IsuseID=@IsuseID
  IF @@ROWCOUNT = 0
  BEGIN
    SELECT @ReturnValue=-2, @ReturnDescription = '�ں�' + CONVERT(VARCHAR(16),@IsuseID) + '������!'
    RETURN -2
  END
  --�жϳ���״̬   0�����2.�������ڣ�4�µ��ɹ���6.��Ʊ�ɹ���8.���ֳ�Ʊ�ɹ���10.�µ�ʧ�ܣ��޺ţ���12.����������14.�н���15.�ɽ��У�16.�ɽ���ɣ�18.���н���ɣ�19.׷�Ž����У�20.׷�����
  IF @QuashStatus=2 OR @QuashStatus=6 OR @QuashStatus=8 OR @QuashStatus=10 OR @QuashStatus=12 OR (@QuashStatus>12 AND @QuashStatus <> 19)
  BEGIN
      SELECT @ReturnValue=-3, @ReturnDescription = CASE WHEN @QuashStatus=2 THEN '��������'
      WHEN @QuashStatus=6 OR @QuashStatus=8 THEN '�����ѳ�Ʊ'
      WHEN @QuashStatus=10 THEN '������Ч'
      WHEN @QuashStatus=12 THEN '�����ѱ�'+CASE @IsSystemQuash WHEN 1 THEN '�û�' WHEN 2 THEN 'ϵͳ' END+'����'
      WHEN @QuashStatus>12 THEN '���������' END
      RETURN -3
  END
  --�ж��Ƿ��ѳ�Ʊ
  IF @BuyType = 0
  BEGIN
    IF @IsBuyed=1 OR @IsOpened=1
	BEGIN
		SELECT @ReturnValue=-4, @ReturnDescription = '�������Ѿ���Ʊ�����Ѿ�����'
		RETURN -4
	END
  END
  ELSE
  BEGIN
	DECLARE @RowCount INT --Ӱ������
    SELECT @RowCount=COUNT(1) FROM CT_ChaseTaskDetails ct
	JOIN CT_Schemes s ON s.SchemeID = ct.SchemeID
	JOIN CT_Isuses i ON i.IsuseID = ct.IsuseID
	WHERE s.SchemeID = @SchemeID
	AND ct.IsSendOut = 0
	AND i.IsOpened = 0
	IF @RowCount = 0
	BEGIN
		SELECT @ReturnValue=-4, @ReturnDescription = '׷�ŷ������Ѿ�ȫ����Ʊ�����Ѿ�ȫ������'
		RETURN -4
	END
  END

  DECLARE @QuashedDescription VARCHAR(50), --��������
    @QuashedType INT --��������
  
  --�û����������߼�
  IF @IsSystemQuash = 1
  BEGIN
    SET @QuashedDescription='ϵͳ����'
    SET @QuashedType = 12
    SET @QuashStatus = 2
  END
  ELSE
  BEGIN
    SET @QuashedDescription='�����˳���'
    SET @QuashedType = 11
    SET @QuashStatus = 1
  END
  
  BEGIN TRAN
  --׷�ų��� �����׷��ִ�з�������񣬻�Ҫ����׷����ϸ��
  IF @BuyType = 1
  BEGIN
    SELECT @Money = SUM(Amount) FROM dbo.CT_ChaseTaskDetails ctd
	left join CT_SchemeETickets se on se.ChaseTaskDetailsID = ctd.ID
	WHERE ctd.SchemeID = @SchemeID AND ctd.IsExecuted = 0 AND (se.TicketStatus=0 OR se.TicketStatus IS NULL)
    
    UPDATE CT_ChaseTaskDetails SET QuashStatus = @QuashStatus WHERE ID IN (
		SELECT ctd.ID FROM dbo.CT_ChaseTaskDetails ctd
		left join CT_SchemeETickets se on se.ChaseTaskDetailsID = ctd.ID
		WHERE ctd.SchemeID = @SchemeID AND ctd.IsExecuted = 0 AND (se.TicketStatus=0 OR se.TicketStatus IS NULL)
	)
    IF @@ROWCOUNT = 0
    BEGIN
      ROLLBACK TRAN
      SELECT @ReturnValue=-5, @ReturnDescription='����ʧ��'
      RETURN -5
    END
    UPDATE CT_Users set Balance = Balance + @Money  WHERE UserID = @UserID
    IF @@ROWCOUNT = 0
    BEGIN
      ROLLBACK TRAN
      SELECT @ReturnValue=-6, @ReturnDescription='����ʧ��'
      RETURN -6
    END
	SELECT @Balance = Balance,@GoldBean=GoldBean FROM dbo.CT_Users WHERE UserID = @UserID;

	INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
	SELECT UserID ,12 ,TradeAmount ,@Balance,@QuashedDescription ,RelationID ,OperatorID ,@Time ,CouponsID FROM CT_UsersRecord 
	WHERE UserID=@UserID AND TradeType=1 AND RelationID=CONVERT(VARCHAR(32),@SchemeID);
    IF @@ROWCOUNT = 0
    BEGIN
      ROLLBACK TRAN
      SELECT @ReturnValue=-7, @ReturnDescription='����ʧ��'
      RETURN -7
    END
    -- ����Ϊ�ѳ�����־
    --UPDATE CT_Schemes SET SchemeStatus = 20, Schedule = 0, TicketStatus=3, UpdateTime = GETDATE() WHERE SchemeID = @SchemeID
    --IF @@ROWCOUNT = 0
    --BEGIN
    --  ROLLBACK TRAN
    --  SELECT @ReturnValue=-3, @ReturnDescription='����ʧ��'
    --  RETURN -3
    --END
  END
  ELSE --��������
  BEGIN
    SELECT a.SchemeID FROM CT_Schemes a
		JOIN CT_SchemeETickets b ON b.SchemeID = a.SchemeID
		WHERE a.SchemeID = @SchemeID AND b.TicketStatus=0
    IF @@ROWCOUNT = 0
    BEGIN
      ROLLBACK TRAN
      SELECT @ReturnValue=-4, @ReturnDescription='�������ѳ�Ʊ��������'
      RETURN -5
    END

    UPDATE CT_Users set Balance = Balance + @Money, Freeze = Freeze - @Money  WHERE UserID = @UserID
    IF @@ROWCOUNT = 0
    BEGIN
      ROLLBACK TRAN
      SELECT @ReturnValue=-1, @ReturnDescription='����ʧ��'
      RETURN -1
    END
    INSERT INTO CT_UsersRecord (UserID, TradeType, TradeAmount, Balance, TradeRemark, RelationID, OperatorID,CreateTime)
      SELECT @UserID, @QuashedType, @Money, Balance, @QuashedDescription, @SchemeID, 1 AS OperatorID,@Time FROM CT_Users WHERE UserID = @UserID
    IF @@ROWCOUNT = 0
    BEGIN
      ROLLBACK TRAN
      SELECT @ReturnValue=-2, @ReturnDescription='����ʧ��'
      RETURN -2
    END  
    -- ����Ϊ�ѳ�����־
    UPDATE CT_Schemes SET SchemeStatus = 12 WHERE SchemeID = @SchemeID
    IF @@ROWCOUNT = 0
    BEGIN
      ROLLBACK TRAN
      SELECT @ReturnValue=-3, @ReturnDescription='����ʧ��'
      RETURN -3
    END
	--��ȯ������¼
	INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
	SELECT UserID ,23 ,TradeAmount ,Balance + TradeAmount ,@QuashedDescription ,RelationID ,OperatorID ,CreateTime ,CouponsID FROM CT_UsersRecord 
	WHERE UserID=@UserID AND TradeType=22 AND RelationID=CONVERT(VARCHAR(32),@SchemeID)
	IF @@ROWCOUNT = 0
	BEGIN
	  ROLLBACK TRAN
	  RETURN -2
	END
	--��������ȯ
	UPDATE r SET r.Balance = r.Balance + u.TradeAmount  FROM CT_UsersRecord AS u
	INNER JOIN CaileCoupons.dbo.CT_Coupons AS r 
	ON r.UserID=u.UserID AND u.RelationID=CONVERT(VARCHAR(32),@SchemeID)
	AND u.TradeType=22 AND r.CouponsID = u.CouponsID
	IF @@ROWCOUNT = 0
	BEGIN
	  ROLLBACK TRAN
	  RETURN -2
	END
	--�ʶ�����
	-- �ʶ�������¼
	INSERT INTO dbo.CT_UsersRecord( UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,RelationID ,OperatorID ,CreateTime ,CouponsID)
	SELECT UserID ,25 ,TradeAmount ,@GoldBean,@QuashedDescription ,RelationID ,OperatorID ,@Time ,CouponsID FROM CT_UsersRecord 
	WHERE UserID=@UserID AND TradeType=24 AND RelationID=CONVERT(VARCHAR(32),@SchemeID);
	--���³����ʶ�
	UPDATE s SET s.GoldBean = s.GoldBean + r.TradeAmount FROM dbo.CT_Users AS s
	INNER JOIN dbo.CT_UsersRecord AS r ON r.UserID = s.UserID AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID) AND r.TradeType = 24;



	
	  
  END

  --���µ���Ʊ״̬
  UPDATE dbo.CT_SchemeETickets SET TicketStatus=3 WHERE SchemeID = @SchemeID
  IF @@ERROR<>0
  BEGIN
    ROLLBACK TRAN
    SELECT @ReturnValue=-4, @ReturnDescription='����ʧ��'
    RETURN -4
  END

   --���·���״̬
  IF @BuyType = 1
  BEGIN
	UPDATE dbo.CT_Schemes set SchemeStatus=20 where SchemeID = @SchemeID
	IF @@ERROR<>0
	BEGIN
		ROLLBACK TRAN
		SELECT @ReturnValue=-4, @ReturnDescription='����ʧ��'
		RETURN -4
	END
  END
  ELSE
  BEGIN
	UPDATE dbo.CT_Schemes SET SchemeStatus=12 WHERE SchemeID = @SchemeID
	IF @@ERROR<>0
	BEGIN
		ROLLBACK TRAN
		SELECT @ReturnValue=-4, @ReturnDescription='����ʧ��'
		RETURN -4
	END
  END
    
  COMMIT TRAN 
  
  SELECT @ReturnValue=0, @ReturnDescription='�����ɹ�'
  RETURN 0
































GO
/****** Object:  StoredProcedure [dbo].[udp_SalePointLst]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[udp_SalePointLst]
  @ticketSource  INT,
  @lotteryCode  INT,
  @salePointStatus  INT,
  @pageSize INT,  --ÿҳ��¼��
  @pageIndex INT,  --��ǰҳ��
  @recordCount INT OUTPUT    --��¼����
AS
  SET NOCOUNT ON;
  BEGIN
  
  DECLARE @SqlStr NVARCHAR(2000),@SqlWhere NVARCHAR(1500),@SqlCount NVARCHAR(4000)
  --ƴ��
  SET @SqlStr = 'SELECT ROW_NUMBER() OVER(ORDER BY a.SalePointID DESC) as Num
		, a.SalePointID
		, a.TicketSourceID
		, a.LotteryCode
		, a.LotteryName
		, a.SalesRebate
		, a.OperatorID
		, a.OperatorName
		, a.OperatorTime
		, a.AuditorID
		, a.AuditorName
		, a.AuditTime
		, a.SalePointStatus
		, a.StartTime
		, a.EndTime
          FROM CT_SalePoint a '
  --����
  SET @SqlWhere = ' where 1 = 1 '
 if @ticketSource != -1
    BEGIN
      SET @SqlWhere = @SqlWhere + ' and a.TicketSourceID = '+ convert(VARCHAR(20),@ticketSource);
    END
 if @lotteryCode > 0
    BEGIN
      SET @SqlWhere = @SqlWhere + ' and charindex('+convert(VARCHAR(20),@lotteryCode+', a.LotteryCode) > 0   = ');
    END
 if @salePointStatus <> 0
    BEGIN
      SET @SqlWhere = @SqlWhere + ' and a.SalePointStatus = '+ convert(VARCHAR(20),@salePointStatus);
    END

  --�ص�
  SET @SqlCount = N'SELECT @a = COUNT(1) FROM ( '  + @SqlStr + @SqlWhere + ' ) tab ';

  --��ҳ
  SET @SqlWhere = @SqlWhere + ' ORDER BY Num
  OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
  FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';
  

  DECLARE @SqlData NVARCHAR(4000);
  SET @SqlData = (@SqlStr + @SqlWhere)
  EXEC sp_executesql @SqlData
  EXEC sp_executesql @SqlCount,N'@a INT OUTPUT',@recordCount OUTPUT

  END
GO
/****** Object:  StoredProcedure [dbo].[udp_SalePointRecordLst]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[udp_SalePointRecordLst]
  @ticketSource  INT,
  @lotteryCode  INT,
  @startTime  DATETIME,
  @endTime  DATETIME,
  @pageSize INT,  --ÿҳ��¼��
  @pageIndex INT,  --��ǰҳ��
  @recordCount INT OUTPUT    --��¼����
AS
  SET NOCOUNT ON;
  BEGIN
  
  DECLARE @SqlStr NVARCHAR(2000),@SqlWhere NVARCHAR(1500),@SqlCount NVARCHAR(4000)
  --ƴ��
  SET @SqlStr = 'SELECT ROW_NUMBER() OVER(ORDER BY a.ID DESC) as Num
		, a.ID
		, a.TicketSourceID
		, a.LotteryCode
		, a.SalesRebate
		, a.LastSalesRebate
		, a.StartTime
		, a.CreateTime
          FROM CT_SalePointRecord a '
  --����
  SET @SqlWhere = ' where 1 = 1 '
 if @ticketSource != -1
    BEGIN
      SET @SqlWhere = @SqlWhere + ' AND a.TicketSourceID = '+ CONVERT(VARCHAR(20),@ticketSource);
    END
 if @lotteryCode > 0
    BEGIN
      SET @SqlWhere = @SqlWhere + ' AND a.LotteryCode = ' + CONVERT(VARCHAR(20),@lotteryCode);
    END

  SET @SqlWhere = @SqlWhere + ' AND a.CreateTime >= '''+ CONVERT(VARCHAR(20),@startTime)+'''';
  SET @SqlWhere = @SqlWhere + ' AND a.CreateTime <= '''+ CONVERT(VARCHAR(20),@endTime)+'''';

  --�ص�
  SET @SqlCount = N'SELECT @a = COUNT(1) FROM ( '  + @SqlStr + @SqlWhere + ' ) tab ';

  --��ҳ
  SET @SqlWhere = @SqlWhere + ' ORDER BY Num
  OFFSET '+ CONVERT(VARCHAR(20),((@pageIndex - 1) * @pageSize)) +' ROWS
  FETCH NEXT '+ CONVERT(VARCHAR(20),@pageSize) +' ROWS ONLY';
  

  DECLARE @SqlData NVARCHAR(4000);
  SET @SqlData = (@SqlStr + @SqlWhere)
  EXEC sp_executesql @SqlData
  EXEC sp_executesql @SqlCount,N'@a INT OUTPUT',@recordCount OUTPUT

  END
GO
/****** Object:  StoredProcedure [dbo].[udp_SavePushIdentify]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Joan
-- Create date: 2017-09-01
-- Description:	�������ͱ�ʶ(��ʱֻ֧�ּ���)
-- =============================================
CREATE PROCEDURE [dbo].[udp_SavePushIdentify]
	@UserId BIGINT, 
	@PushIdentify VARCHAR(100),
	@Result INT OUTPUT -- ���ؽ��
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		DECLARE @Count INT;
		SELECT @Count = COUNT(*) FROM CT_UsersPush WHERE UserId = @UserId
		IF @Count = 0
			BEGIN 
				INSERT INTO dbo.CT_UsersPush( UserId ,PushIdentify) VALUES (@UserId ,@PushIdentify)
				IF @@TRANCOUNT > 0
				BEGIN
				  SET @Result = -1; -- ��������ʧ��
				END
			END 
		ELSE
			BEGIN
				UPDATE CT_UsersPush SET PushIdentify = @PushIdentify WHERE UserId = @UserId
			END 
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		 ROLLBACK TRAN
			DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
			RAISERROR(@msg,16,-1);
			RETURN @@ERROR;
		END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	SET @Result = 0;
END


GO
/****** Object:  StoredProcedure [dbo].[udp_SetSystemStaticdata]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		joan
-- Create date: 2017-11-4
-- Description:	��̬����ͳ��
-- =============================================
CREATE PROCEDURE [dbo].[udp_SetSystemStaticdata]
	@DateDay VARCHAR(10),           --��������
	@StaticdataType INT,            --��̬�������� 1���ճ�ֵ�ܶ� 2�������ϳ�ֵ 3�������³�ֵ 4���������ܶ� 5����������Ա 6�������ͽ�� 7����Ͷע 8�����н�
	@Amount BIGINT,                 --��̬���ݽ��
	@Users INT,                     --�����û�
	@LotteryCode INT                --����
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY

	IF (SELECT COUNT(1) FROM dbo.CT_SystemStaticdata WHERE dateday=@DateDay) = 0
		BEGIN
			INSERT INTO dbo.CT_SystemStaticdata(dateday) VALUES (@DateDay);
		END
	--recharge          ���ճ�ֵ�ܶ�
	IF @StaticdataType = 1
		BEGIN
			UPDATE CT_SystemStaticdata SET recharge = recharge + @Amount WHERE dateday=@DateDay
		END
	--online_recharge   �������ϳ�ֵ
	IF @StaticdataType = 2
		BEGIN
			UPDATE CT_SystemStaticdata SET online_recharge = online_recharge + @Amount WHERE dateday=@DateDay
		END
	--offline_recharge  �������³�ֵ
	IF @StaticdataType = 3
		BEGIN
			UPDATE CT_SystemStaticdata SET offline_recharge = offline_recharge + @Amount WHERE dateday=@DateDay
		END
	--withdraw        ���������ܶ�
	IF @StaticdataType = 4
		BEGIN
			UPDATE CT_SystemStaticdata SET withdraw = withdraw + @Amount WHERE dateday=@DateDay
		END
	--users           ����������Ա
	IF @StaticdataType = 5
		BEGIN
			UPDATE CT_SystemStaticdata SET users = users + @Users WHERE dateday=@DateDay
		END
	--largess         �������ͽ��
	IF @StaticdataType = 6
		BEGIN
			UPDATE CT_SystemStaticdata SET largess = largess + @Amount WHERE dateday=@DateDay
		END
	--����Ͷע
	IF @StaticdataType = 7
		BEGIN
			--buy_jlk         ���ռ��ֿ���Ͷע���
			IF @LotteryCode = 101
				BEGIN
					UPDATE CT_SystemStaticdata SET buy_jlk = buy_jlk + @Amount WHERE dateday=@DateDay
				END
			--buy_jxk         ���ս�������Ͷע���
			IF @LotteryCode = 102
				BEGIN
					UPDATE CT_SystemStaticdata SET buy_jxk = buy_jxk + @Amount WHERE dateday=@DateDay
				END
			--buy_hbsyydj     ���պ���ʮһ�ƶ��Ͷע���
			IF @LotteryCode = 201
				BEGIN
					UPDATE CT_SystemStaticdata SET buy_hbsyydj = buy_hbsyydj + @Amount WHERE dateday=@DateDay
				END
			--buy_sdsyydj     ����ɽ��ʮһ�ƶ��Ͷע���
			IF @LotteryCode = 202
				BEGIN
					UPDATE CT_SystemStaticdata SET buy_sdsyydj = buy_sdsyydj + @Amount WHERE dateday=@DateDay
				END
			--buy_cqssc       ��������ʱʱ��Ͷע���
			IF @LotteryCode = 301
				BEGIN
					UPDATE CT_SystemStaticdata SET buy_cqssc = buy_cqssc + @Amount WHERE dateday=@DateDay
				END
			--buy_ssq         ����˫ɫ��Ͷע���
			IF @LotteryCode = 801
				BEGIN
					UPDATE CT_SystemStaticdata SET buy_ssq = buy_ssq + @Amount WHERE dateday=@DateDay
				END
			--buy_dlt         ���մ���͸Ͷע���
			IF @LotteryCode = 801
				BEGIN
					UPDATE CT_SystemStaticdata SET buy_dlt = buy_dlt + @Amount WHERE dateday=@DateDay
				END
		END
	--�����н�
	IF @StaticdataType = 8
		BEGIN
			--win_jlk         ���ռ��ֿ���Ͷע���
			IF @LotteryCode = 101
				BEGIN
					UPDATE CT_SystemStaticdata SET win_jlk = win_jlk + @Amount WHERE dateday=@DateDay
				END
			--win_jxk         ���ս�������Ͷע���
			IF @LotteryCode = 102
				BEGIN
					UPDATE CT_SystemStaticdata SET win_jxk = win_jxk + @Amount WHERE dateday=@DateDay
				END
			--win_hbsyydj     ���պ���ʮһ�ƶ��Ͷע���
			IF @LotteryCode = 201
				BEGIN
					UPDATE CT_SystemStaticdata SET win_hbsyydj = win_hbsyydj + @Amount WHERE dateday=@DateDay
				END
			--win_sdsyydj     ����ɽ��ʮһ�ƶ��Ͷע���
			IF @LotteryCode = 202
				BEGIN
					UPDATE CT_SystemStaticdata SET win_sdsyydj = win_sdsyydj + @Amount WHERE dateday=@DateDay
				END
			--win_cqssc       ��������ʱʱ��Ͷע���
			IF @LotteryCode = 301
				BEGIN
					UPDATE CT_SystemStaticdata SET win_cqssc = win_cqssc + @Amount WHERE dateday=@DateDay
				END
			--win_ssq         ����˫ɫ��Ͷע���
			IF @LotteryCode = 801
				BEGIN
					UPDATE CT_SystemStaticdata SET win_ssq = win_ssq + @Amount WHERE dateday=@DateDay
				END
			--win_dlt         ���մ���͸Ͷע���
			IF @LotteryCode = 801
				BEGIN
					UPDATE CT_SystemStaticdata SET win_dlt = win_dlt + @Amount WHERE dateday=@DateDay
				END
		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			
			SELECT -1;

            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END

GO
/****** Object:  StoredProcedure [dbo].[udp_SetWinPrize]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--�ֶ������н��ɹ�
--2017-5-10
CREATE PROC [dbo].[udp_SetWinPrize]
	@SchemeID BIGINT,
	@AllValues VARCHAR(512),
	@ReturnValue int OUTPUT,
	@ReturnDescription varchar(100) OUTPUT
	--WITH ENCRYPTION
AS
	DECLARE @Values VARCHAR(20), 
			@index INT = 1, 
			@SumWinMoney BIGINT=0, 
			@SumWinMoneyNoWithTax BIGINT=0, 
			@Multiple INT, @Bet INT
	DECLARE @IsuseID BIGINT, 
			@WinID INT = 0, 
			@id BIGINT, 
			@DefaultMoney INT=0, 
			@DefaultMoneyNoWithTax INT=0, 
			@UserID BIGINT, 
			@Balance BIGINT=0
	
	SELECT @IsuseID=IsuseID, @UserID=InitiateUserID FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
	IF @@ROWCOUNT = 0
		BEGIN
			SELECT @ReturnValue=-1, @ReturnDescription='�����Ų�����'
			RETURN -1
		END
	IF(SELECT LEN(OpenNumber) FROM dbo.CT_Isuses WHERE IsuseID=@IsuseID) = 0
		BEGIN
			SELECT @ReturnValue=-2, @ReturnDescription='�ںŻ�û����'
			RETURN -2
		END
	SELECT @Balance=Balance FROM dbo.CT_Users WHERE UserID=@UserID
	IF @@ROWCOUNT = 0
		BEGIN
			SELECT @ReturnValue=-3, @ReturnDescription='�����û��˺Ų�����'
			RETURN -3
		END
	
	BEGIN TRAN
		SET @Values =dbo.fnGetField(@AllValues, @index, '#')
		WHILE LEN(@Values) > 0
		BEGIN 
			SET @id = dbo.fnGetField(@Values, 1,',')
			SET @WinID = dbo.fnGetField(@Values, 2,',')

			SELECT @Multiple=Multiple, @Bet=BetNum FROM dbo.CT_SchemesDetail WHERE SDID = @id
			SELECT @DefaultMoney=DefaultMoney, @DefaultMoneyNoWithTax=DefaultMoneyNoWithTax FROM dbo.CT_WinTypes WHERE WinID=@WinID
			SET @SumWinMoney =@SumWinMoney + (@Multiple * @DefaultMoney)
			SET @SumWinMoneyNoWithTax =@SumWinMoneyNoWithTax + (@Multiple * @DefaultMoneyNoWithTax)
			
			UPDATE dbo.CT_SchemesDetail SET IsWin = 1,WinMoney = WinMoney+@SumWinMoney,WinMoneyNoWithTax = WinMoneyNoWithTax+@SumWinMoneyNoWithTax WHERE SDID=@id
			--IF @@ROWCOUNT = 0
			--BEGIN
			--	ROLLBACK TRAN
			--	SELECT @ReturnValue=-3, @ReturnDescription='�ֶ������н�ʧ��'
			--	RETURN -3
			--END
			
			SET @index = @index + 1
			SET @Values = dbo.fnGetField(@AllValues, @index, '#')
		END
		
		UPDATE CT_Schemes set SchemeStatus=14 WHERE SchemeID=@SchemeID
		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue=-4, @ReturnDescription='�ֶ������н�ʧ��'
			RETURN -4
		END

		INSERT dbo.CT_UsersRecord (UserID ,TradeType ,TradeAmount ,Balance ,TradeRemark ,CreateTime ,RelationID ,OperatorID)
			VALUES(@UserID, 0, @SumWinMoneyNoWithTax, (@Balance+@SumWinMoneyNoWithTax), '��Ʊ�н�', GETDATE(), @SchemeID, 1)
		IF @@rowcount = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -5, @ReturnDescription = '��ӽ�ұ仯��¼ʧ��'	
			RETURN -5
		END
		UPDATE dbo.CT_Users SET Balance = Balance + @SumWinMoneyNoWithTax WHERE UserID = @UserID
		IF @@rowcount = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -6, @ReturnDescription = '�����û����ʧ��'	
			RETURN -6
		END
	COMMIT TRAN
	
	SELECT @ReturnValue=0, @ReturnDescription='�ֶ������н��ɹ�'
	RETURN 0


GO
/****** Object:  StoredProcedure [dbo].[udp_StopChaseTask]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--ֹͣ׷��
CREATE PROCEDURE [dbo].[udp_StopChaseTask]	
    @SchemeID BIGINT,	         --������
    @ChaseTaskDetailsID BIGINT,  --׷��ID
    @Amount  BIGINT,             --���
    @StopType INT                --ֹͣ����  1׷�����ֹͣ ,2׷����������ֹͣ
    --WITH ENCRYPTION
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		IF @StopType=1
			BEGIN
				--���׷��ֹͣ
				UPDATE dbo.CT_Schemes SET SchemeStatus = 20 WHERE SchemeID = @SchemeID
			END
		ELSE IF @StopType=2
			BEGIN
				DECLARE @UserID BIGINT,@IsExecuted BIT;
				SELECT @UserID = InitiateUserID FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
				SELECT @IsExecuted = IsExecuted FROM dbo.CT_ChaseTaskDetails WHERE ID = @ChaseTaskDetailsID
				
				UPDATE dbo.CT_Schemes SET SchemeStatus = 20 WHERE SchemeID = @SchemeID
				IF @IsExecuted=0
					BEGIN
					UPDATE dbo.CT_ChaseTaskDetails SET IsExecuted=1,QuashStatus=2 WHERE ID= @ChaseTaskDetailsID
					--�˻��������
					UPDATE dbo.CT_Users SET Balance = Balance + @Amount WHERE UserID = @UserID
					
					INSERT INTO CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
					SELECT @UserID,13,@Amount,Balance,'׷�ų���',GETDATE(),@SchemeID,@UserID FROM dbo.CT_Users WHERE UserID = @UserID
					END
				
			END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN
            SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END




GO
/****** Object:  StoredProcedure [dbo].[udp_StopChaseTaskQuery]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--׷�����ݲ�ѯ
CREATE PROC [dbo].[udp_StopChaseTaskQuery]
	@ChaseTaskID	BIGINT, --׷�����
	@QueryType INT --��ѯ����  1 ׷�����ֹͣ��2 ׷���н����н�������ڶ���ֹͣ
	--WITH ENCRYPTION
AS
	SET NOCOUNT ON;
	IF @QueryType=1
	 BEGIN
		SELECT COUNT(ID) AS Nums FROM dbo.CT_ChaseTaskDetails WHERE ChaseTaskID=@ChaseTaskID AND IsExecuted=0
	 END
	ELSE IF @QueryType=2
		BEGIN
			--�н�ֹͣ׷��, �н�����ֹͣ׷��
			SELECT ISNULL(SUM(se.WinMoney),0) AS WinMoney FROM CT_ChaseTaskDetails AS ctd
			INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID=ctd.SchemeID
			LEFT JOIN dbo.CT_SchemeETickets AS se ON se.SchemeID=s.SchemeID AND se.SchemeID=ctd.SchemeID AND se.ChaseTaskDetailsID = ctd.ID
			--WITH tab AS (
			--	SELECT ISNULL(SUM(s.WinMoney),0) AS WinMoney FROM dbo.CT_ChaseTaskDetails AS ctd
			--	INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID =ctd.SchemeID
			--	INNER JOIN dbo.CT_Isuses AS i ON i.IsuseID=ctd.IsuseID AND i.IsuseState = 6 
			--	WHERE ctd.ChaseTaskID=@ChaseTaskID
			--	UNION
			--	SELECT ISNULL(SUM(win.WinMoney),0) AS WinMoney FROM dbo.CT_ChaseTaskDetails AS ctd
			--	INNER JOIN dbo.CT_Isuses AS s  ON s.IsuseID=ctd.IsuseID AND s.IsuseState = 4
			--	INNER JOIN dbo.CT_SchemeETickets AS se ON se.ChaseTaskDetailsID=ctd.ID
			--	INNER JOIN dbo.CT_SchemesWin AS win ON win.SchemeETicketsID=se.SchemeETicketsID
			--	WHERE ctd.ChaseTaskID=@ChaseTaskID
			--)
			--SELECT SUM(WinMoney) as Nums FROM tab
		END 


GO
/****** Object:  StoredProcedure [dbo].[udp_SubmitBetting]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Joan
-- Create date: 2017-11-24
-- Description:	���,��ȯ,�ʶ� Ͷע
-- =============================================
CREATE PROCEDURE [dbo].[udp_SubmitBetting] 
	@UserID BIGINT,              -- �ύ��id
	@LotteryCode INT,            -- ���ֱ���
    @IsuseName VARCHAR(20),	     -- ����id
	@SchemeNumber VARCHAR(50),   -- ������
	@SchemeMoney BIGINT,         -- �������
	@LotteryNumber VARCHAR(MAX), -- Ͷע����
	@IsSplit BIT,
	@BuyType INT = 0,            -- 0���� 1׷�� 2����
	@RoomCode VARCHAR(50),       -- ������
	@SchemesDetailTables XML,    -- �������� 
	@TasksBeginTime DATETIME,    -- ��ʼʱ�� --׷��
	@TasksEndTime DATETIME,      -- ����ʱ�� --׷��
	@IsuseCount INT,             -- ׷�������� --׷��
	@Stops BIGINT,               -- ׷������  -1׷�Ž���ֹͣ 0�н�ֹͣ  ����0 �ۼ��н����ﵽֹͣ --׷��
	@TasksDetailsTables XML,     -- ׷������ --׷��
	@CouponsTables XML,                -- ��ȯ��Ϣ(����滻)
	@PaymentType INT,            -- ֧�����ͣ�1.���ʶ�����ȯ֧�� 2.���֧�� 3.����ȯ֧�� 4.�ʶ�֧�� 5.���͹���ȯ֧�� 6.���Ͳʶ�֧�� 7.�ʶ��͹���ȯ֧��
	@Gold BIGINT,                -- �ʶ� 
	@ChaseTaskDetailsID BIGINT OUTPUT     --׷�������һ��
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
		DECLARE @SchemeTotalMoney BIGINT /*�����ܽ��*/,@CurrCount INT /*Ԥ��*/,@VerifyIsuseCount INT = 0 /*��֤�ں���*/,@CouponsCount INT = 0/*��ȯ��(�滻�����)*/,@Balance BIGINT = 0/*���*/,
				@IsuseID BIGINT = 0/*�ں�id*/,@SchemeID BIGINT = 0/*����id*/,@OrderStatus TINYINT = 0/*����״̬*/,@DateTime DATETIME = GETDATE()/*��ǰʱ��*/,@ChaseTaskID BIGINT = 0/*׷�ű�id*/,
				@PrintOutType TINYINT/*��Ʊ���ͣ�1.ϵͳ����Ʊ 2.��������Ʊ*/,@OutType TINYINT/*���ֳ�Ʊ���ͣ�0.ϵͳ����Ʊ 1.��������Ʊ*/,@IsVerifySchemeMoney BIT = 1,@GoldBean BIGINT/*�ʶ�*/;
		DECLARE @ItemSchemesTable TABLE(SchemeID BIGINT,SDID BIGINT,Bet INT ,Multiple INT,PlayCode INT,Number VARCHAR(500),Amount BIGINT,IsNorm INT,UserCode BIGINT,LotteryCode INT); --Ͷע��������
		DECLARE @ItemTasksTable TABLE(IsuseID BIGINT ,Amount BIGINT,Multiple INT); --׷�������б�
		DECLARE @ItemCouponsTable TABLE(pid BIGINT,ba BIGINT);  --����ȯ�б�
		
		--PRINT '001';
		--���ò���
		SET @ChaseTaskDetailsID = 0; 
		SELECT @OutType = PrintOutType FROM dbo.CT_Lotteries WHERE LotteryCode=@LotteryCode
		SET @PrintOutType = 1;
		IF @OutType = 1
			BEGIN
				SET @PrintOutType = 2;
			END
		--��ȡ�û���
		SELECT @Balance = Balance,@GoldBean = GoldBean FROM dbo.CT_Users WHERE UserID = @UserID;
		--��ȡ�ںű�
		SELECT @IsuseID = IsuseID FROM dbo.CT_Isuses WHERE IsuseName=@IsuseName AND LotteryCode = @LotteryCode;
		--��ȡ�ںű�����(���Ԥ������)
		SELECT @CurrCount = COUNT(1) FROM dbo.CT_Isuses 
		WHERE LotteryCode=@LotteryCode AND CONVERT(DATE,EndTime)=CONVERT(DATE,@DateTime) AND EndTime > @DateTime
		--��֤�ںſ�ʼ����ʱ�� ����Ԥ�ۺ���ǰ��ֹ
		SELECT @VerifyIsuseCount = COUNT(1) FROM dbo.CT_Isuses a
		JOIN [dbo].[CT_Lotteries] b ON a.LotteryCode = b.LotteryCode
		WHERE a.IsuseID=@IsuseID AND DATEADD(MINUTE,(-b.PresellTime),StartTime) <= @DateTime AND DATEADD(MINUTE,(-b.AdvanceEndTime),EndTime) >= @DateTime;
		
		--���Table����
		--��䷽������table��¼
		INSERT INTO @ItemSchemesTable 
		(SchemeID,SDID,Bet,Multiple,PlayCode ,Number,Amount,IsNorm,UserCode,LotteryCode) 
		SELECT 
		T.c.value('(SchemeID/text())[1]','BIGINT'), 
		T.c.value('(SDID/text())[1]','BIGINT'), 
		T.c.value('(Bet/text())[1]','INT'), 
		T.c.value('(Multiple/text())[1]','INT'), 
		T.c.value('(PlayCode/text())[1]','INT'), 
		T.c.value('(Number/text())[1]','VARCHAR(500)'), 
		T.c.value('(Amount/text())[1]','BIGINT'), 
		T.c.value('(IsNorm/text())[1]','BIT'),
		T.c.value('(UserCode/text())[1]','BIGINT'),
		T.c.value('(LotteryCode/text())[1]','INT')
		FROM @SchemesDetailTables.nodes('/ArrayOfUdv_Parameter/udv_Parameter') AS T(c)
		--���׷������table��¼
		IF @BuyType=1
			BEGIN
				INSERT INTO @ItemTasksTable 
				( 
				IsuseID,Amount,Multiple
				) 
				SELECT 
				T.c.value('(IsuseID/text())[1]','BIGINT'), 
				T.c.value('(Amount/text())[1]','BIGINT'), 
				T.c.value('(Multiple/text())[1]','INT')
				FROM @TasksDetailsTables.nodes('/ArrayOfUdv_TasksDetails/udv_TasksDetails') AS T(c)
			END
		--��乺��ȯtable��¼(����ʹ�ù���ȯ����)
		IF (@PaymentType = 1 OR @PaymentType = 3 OR @PaymentType = 5 OR @PaymentType = 7) AND ISNULL(CONVERT(VARCHAR(MAX),@CouponsTables),'')!=''
			BEGIN
				INSERT INTO @ItemCouponsTable (pid ,ba)
				SELECT
				T.c.value('(pid/text())[1]','BIGINT'), 
				T.c.value('(ba/text())[1]','BIGINT')
				FROM @CouponsTables.nodes('/ArrayOfUdv_Coupons/udv_Coupons') AS T(c)

				SELECT @CouponsCount = COUNT(r.CouponsID) FROM @ItemCouponsTable AS t
				INNER JOIN CaileCoupons.dbo.CT_Coupons AS r ON r.CouponsID = t.pid AND r.CouponsID = @UserID AND r.Balance < t.ba
			END
		IF @VerifyIsuseCount = 0 AND @CurrCount > 0
			BEGIN
				SELECT -42;
				SET @IsVerifySchemeMoney = 0;
			END
		ELSE
			BEGIN
			    -- ֧�����ͣ�1.���ʶ�����ȯ֧�� 2.���֧�� 3.����ȯ֧�� 4.�ʶ�֧�� 5.���͹���ȯ֧�� 6.���Ͳʶ�֧�� 7.�ʶ��͹���ȯ֧��
				IF @PaymentType = 1 --1.���ʶ�����ȯ֧��
					BEGIN
					    IF @SchemeMoney > @Balance --����
							BEGIN
								SELECT -5;
							    SET @IsVerifySchemeMoney = 0;
							END
						ELSE IF @Gold > @GoldBean
							BEGIN
							    SELECT -6; --�ʶ�����
							    SET @IsVerifySchemeMoney = 0;
							END
						ELSE IF @CouponsCount > 0
							BEGIN
							    SELECT -50; --����ȯ����
							    SET @IsVerifySchemeMoney = 0;
							END
						--���÷����ܶ�
						SET @SchemeTotalMoney = @SchemeMoney + (SELECT SUM(ba) FROM @ItemCouponsTable) + (@Gold/1000)*100;
					END
				ELSE IF @PaymentType = 2 --2.���֧��
					BEGIN
					    IF @SchemeMoney > @Balance --����
							BEGIN
								SELECT -5;
							    SET @IsVerifySchemeMoney = 0;
							END
						--���÷����ܶ�
						SET @SchemeTotalMoney = @SchemeMoney;
					END
				ELSE IF @PaymentType = 3 --3.����ȯ֧��
					BEGIN
					    IF @CouponsCount > 0
							BEGIN
							    SELECT -50; --����ȯ����
							    SET @IsVerifySchemeMoney = 0;
							END
						--���÷����ܶ�
						SET @SchemeTotalMoney = (SELECT SUM(ba) FROM @ItemCouponsTable);
					END
				ELSE IF @PaymentType = 4 --4.�ʶ�֧��
					BEGIN
					    IF @Gold > @GoldBean
							BEGIN
							    SELECT -6; --�ʶ�����
							    SET @IsVerifySchemeMoney = 0;
							END
						--���÷����ܶ�
						SET @SchemeTotalMoney = (@Gold/1000)*100;
					END 
				ELSE IF @PaymentType = 5 --5.���͹���ȯ֧��
					BEGIN
					    IF @SchemeMoney > @Balance --����
							BEGIN
								SELECT -5;
							    SET @IsVerifySchemeMoney = 0;
							END
						ELSE IF @CouponsCount > 0
							BEGIN
							    SELECT -50; --����ȯ����
							    SET @IsVerifySchemeMoney = 0;
							END
						--���÷����ܶ�
						SET @SchemeTotalMoney = @SchemeMoney + (SELECT SUM(ba) FROM @ItemCouponsTable);
					END 
				ELSE IF @PaymentType = 6 --6.���Ͳʶ�֧��
					BEGIN
					    IF @SchemeMoney > @Balance --����
							BEGIN
								SELECT -5;
							    SET @IsVerifySchemeMoney = 0;
							END
						ELSE IF @Gold > @GoldBean
							BEGIN
							    SELECT -6; --�ʶ�����
							    SET @IsVerifySchemeMoney = 0;
							END
						--���÷����ܶ�
						SET @SchemeTotalMoney = @SchemeMoney + (@Gold/1000)*100;
					END 
				ELSE IF @PaymentType = 7 --7.�ʶ��͹���ȯ֧��
					BEGIN
						IF @Gold > @GoldBean
							BEGIN
							    SELECT -6; --�ʶ�����
							    SET @IsVerifySchemeMoney = 0;
							END
						ELSE IF @CouponsCount > 0
							BEGIN
							    SELECT -50; --����ȯ����
							    SET @IsVerifySchemeMoney = 0;
							END
						--���÷����ܶ�
						SET @SchemeTotalMoney = (SELECT SUM(ba) FROM @ItemCouponsTable) + (@Gold/1000)*100;
					END
			END
		
		PRINT '�����ܶ�'+CONVERT(VARCHAR(50),@SchemeTotalMoney);
		--��֤ͨ����ִ�к�������
		IF @IsVerifySchemeMoney = 1
			BEGIN
				SET @OrderStatus = 4
				IF @BuyType = 1
					BEGIN
						SET @OrderStatus = 19
					END 
			    --����
				INSERT INTO dbo.CT_Schemes( CreateTime ,SchemeNumber ,Title ,InitiateUserID ,LotteryCode ,IsuseID ,IsuseName ,LotteryNumber ,SchemeMoney ,SecrecyLevel ,SchemeStatus ,
											PrintOutType ,Describe ,FromClient ,BuyType ,IsSplit ,FollowSchemeID ,FollowSchemeBonus ,FollowSchemeBonusScale ,PlusAwards ,IsSendOut,RoomCode)
				VALUES( @DateTime ,@SchemeNumber ,'' ,@UserID ,@LotteryCode ,@IsuseID ,@IsuseName ,@LotteryNumber ,@SchemeTotalMoney ,0 ,@OrderStatus ,@PrintOutType ,'' ,0 ,
											@BuyType ,@IsSplit ,0 ,0 ,0 ,0 ,0,@RoomCode)
				SELECT @SchemeID = @@IDENTITY;

				--��������
				INSERT INTO dbo.CT_SchemesDetail(BetMoney,SchemeID ,PlayCode ,Multiple ,BetNum ,BetNumber ,IsBuyed ,IsNorm ,IsWin ,IsOpened ,WinMoney ,WinMoneyNoWithTax ,Schedule ,WinDescribe ,PrintOutTime ,WinImageUrl ,UpdateTime ,OpenOperatorID)
				SELECT  Amount,@SchemeID ,PlayCode ,Multiple ,Bet ,ISNULL(Number,'') , 0 ,IsNorm ,0 ,0 ,0 ,0 ,0 ,'' ,NULL ,'' ,@DateTime ,0 FROM @ItemSchemesTable;
				
				--׷��
				IF @BuyType = 1
					BEGIN 
						INSERT INTO dbo.CT_ChaseTasks( UserID ,SchemeID,LotteryCode ,StartTime ,EndTime ,CreateTime ,IsuseCount ,BetType ,StopTypeWhenWin,StopTypeWhenWinMoney,QuashStatus,Title)
						VALUES (@UserID,@SchemeID,@LotteryCode,@TasksBeginTime,@TasksEndTime,@DateTime,@IsuseCount,2,1,@Stops,0,'׷��')
						SET @ChaseTaskID = @@IDENTITY
						INSERT INTO dbo.CT_ChaseTaskDetails(ChaseTaskID,CreateTime,IsuseID,Multiple,Amount,RedPacketId,RedPacketMoney,QuashStatus,IsExecuted,SchemeID,SecrecyLevel,LotteryNumber,IsShare)
						SELECT @ChaseTaskID,@DateTime,IsuseID,Multiple,Amount,'',0,0,0,@SchemeID,0,@LotteryNumber,1 FROM @ItemTasksTable
						--SELECT @ChaseTaskDetailsID = @@IDENTITY
						SET @ChaseTaskDetailsID = (SELECT TOP 1 ID FROM dbo.CT_ChaseTaskDetails WHERE ChaseTaskID = @ChaseTaskID AND IsuseID = @IsuseID)
					END
				
				--�����ֽ��
				IF @SchemeMoney > 0
				BEGIN
					--�����ֽ��(��ֵ�ٷ�֮��ʮ��) S 
					DECLARE @WithdrawMoney BIGINT,@WithdrawBalance BIGINT;
					SET @WithdrawBalance = @Balance - @SchemeMoney;
					SELECT @WithdrawMoney = WithdrawMoney FROM dbo.CT_UsersExtend WHERE UserID=@UserID;
					IF @WithdrawBalance < @WithdrawMoney
						BEGIN
						    IF @WithdrawMoney > @SchemeMoney
								BEGIN
									SET @WithdrawMoney = @WithdrawMoney - @SchemeMoney
								END
							UPDATE dbo.CT_UsersExtend SET WithdrawMoney = WithdrawMoney - @WithdrawMoney WHERE UserID=@UserID;
						END
					--�����ֽ�� E	
				END
				-- @PaymentType ֧�����ͣ�1.���ʶ�����ȯ֧�� 2.���֧�� 3.����ȯ֧�� 4.�ʶ�֧�� 5.���͹���ȯ֧�� 6.���Ͳʶ�֧�� 7.�ʶ��͹���ȯ֧��
				--���֧��
				IF @PaymentType=1 OR @PaymentType=2 OR @PaymentType=5 OR @PaymentType=6
					BEGIN
						-- �������
						UPDATE dbo.CT_Users SET Balance = Balance - @SchemeMoney WHERE UserID = @UserID;
						IF @OrderStatus = 4
							BEGIN
								INSERT INTO [dbo].CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
								SELECT @UserID,1,@SchemeMoney,Balance,'����',@DateTime,@SchemeID,@UserID FROM dbo.CT_Users WHERE UserID = @UserID
							END
						ELSE IF @OrderStatus = 19
							BEGIN
								INSERT INTO [dbo].CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
								SELECT @UserID,1,@SchemeMoney,Balance,'׷��',@DateTime,@SchemeID,@UserID FROM dbo.CT_Users WHERE UserID = @UserID
							END
					END
				--��ȯ֧��
				IF @PaymentType=1 OR @PaymentType=3 OR @PaymentType=5 OR @PaymentType=7
					BEGIN
					    --��ȯ���ʼ�¼
						INSERT INTO CaileCoupons.dbo.CT_CouponsRecord( CouponsID ,LogType ,CreateTime ,UserID ,RelationID,Amount)
						SELECT r.CouponsID,0,@DateTime,r.UserID,@SchemeID,t.ba FROM @ItemCouponsTable AS t
						INNER JOIN CaileCoupons.dbo.CT_Coupons AS r ON r.CouponsID = t.pid AND r.UserID = @UserID

						--���²�ȯ ���ʹ��
						UPDATE r SET r.Balance = r.Balance - t.ba,r.CouponsStatus=3,r.LastTime=@DateTime FROM @ItemCouponsTable AS t
						INNER JOIN CaileCoupons.dbo.CT_Coupons AS r ON r.CouponsID = t.pid AND r.UserID = @UserID AND r.FirstTime IS NOT NULL

						--���²�ȯ ��һ��ʹ��
						UPDATE r SET r.Balance = r.Balance - t.ba,r.CouponsStatus=3,r.LastTime=@DateTime,r.FirstTime = @DateTime FROM @ItemCouponsTable AS t
						INNER JOIN CaileCoupons.dbo.CT_Coupons AS r ON r.CouponsID = t.pid AND r.UserID = @UserID AND r.FirstTime IS NULL

						IF @OrderStatus = 4
							BEGIN
							    --��ȯ���ʼ�¼
								INSERT INTO [dbo].CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID,CouponsID)
								SELECT r.UserID,22,t.ba,r.Balance,'��ȯ����',@DateTime,@SchemeID,r.UserID,t.pid FROM @ItemCouponsTable AS t
								INNER JOIN CaileCoupons.dbo.CT_Coupons AS r ON r.CouponsID = t.pid AND r.UserID = @UserID
							END
					END
				--�ʶ�֧��
				IF @PaymentType=1 OR @PaymentType=4 OR @PaymentType=6 OR @PaymentType=7
					BEGIN
						IF @OrderStatus = 4
							BEGIN
								INSERT INTO CaileMiniGame.dbo.CT_Record( RecordType ,Amount ,RelationID ,Balance ,UserCode ,CreateTime)
								VALUES  ( 5 ,@Gold ,CONVERT(VARCHAR(50),@SchemeID) ,@GoldBean-@Gold ,@UserID ,@DateTime);

								INSERT INTO [dbo].CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
								SELECT @UserID,24,@Gold,@GoldBean-@Gold,'�ʶ�����',@DateTime,@SchemeID,@UserID FROM dbo.CT_Users WHERE UserID = @UserID;

								UPDATE dbo.CT_Users SET GoldBean = GoldBean - @Gold WHERE UserID = @UserID;
							END
						
					END
				SELECT @SchemeID;
			END

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END



GO
/****** Object:  StoredProcedure [dbo].[udp_TicketBetting]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Joan
-- Create date: 2017-07-19
-- Description:	����ƱͶע
-- =============================================
CREATE PROCEDURE [dbo].[udp_TicketBetting]
	@SchemeID BIGINT, 
	@ChaseTaskDetailsID BIGINT,
	@TicketTable XML,
	@ReturnValue	INT OUTPUT,
	@ReturnDescription VARCHAR(100) OUTPUT
    --WITH ENCRYPTION
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @ItemTicketTable TABLE(seid BIGINT /*����Ʊ��ʶ*/ ,msg VARCHAR(100) /*��ע*/,idf VARCHAR(50) /*��Ʊ��ʶ��*/,s TINYINT /*����Ʊ״̬*/);
	DECLARE @UserID BIGINT, @WithdrawalMoney BIGINT=0, @SchemeMoney BIGINT=0, @OrderStatus SMALLINT,@BuyType INT,@Time DateTime = GETDATE() ;

	INSERT INTO @ItemTicketTable 
		(seid,msg,idf,s) 
	SELECT T.c.value('(seid/text())[1]','BIGINT'), 
		   T.c.value('(msg/text())[1]','VARCHAR(100)'), 
		   T.c.value('(idf/text())[1]','VARCHAR(50)'), 
		   T.c.value('(s/text())[1]','TINYINT')
	FROM @TicketTable.nodes('/ArrayOfUdv_tb/udv_tb') AS T(c);
	--����
	UPDATE se SET TicketStatus=t.s, Identifiers=ISNULL(t.idf,''), HandleDateTime=@Time, HandleDescribe=t.msg FROM 
	CT_SchemeETickets AS se INNER JOIN @ItemTicketTable AS t ON se.SchemeETicketsID=t.seid
	IF @@ROWCOUNT = 0
		BEGIN
	   		SELECT @ReturnValue = -1, @ReturnDescription = '����Ʊ״̬����ʧ��'	
		END
	-- ******************************************************** Ц�� ***************************************************
	SELECT @UserID = InitiateUserID, @SchemeMoney = SchemeMoney, @OrderStatus=SchemeStatus,@BuyType=BuyType FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
	IF @@ROWCOUNT = 0
	BEGIN
		SELECT @ReturnValue=-1, @ReturnDescription='�����ţ�'+@SchemeID+'������'
	END
	IF @OrderStatus=12  --�Ѿ����������ٸ��µ���Ʊ״̬���������˿�
	BEGIN
		SELECT @ReturnValue=0, @ReturnDescription='�����ɹ�'
	END

	IF @BuyType != 1
		BEGIN
			SELECT @WithdrawalMoney=SUM(TicketMoney) FROM dbo.CT_SchemeETickets WHERE SchemeID=@SchemeID AND TicketStatus=3--Ͷעʧ��Ʊ��
			--��ǰ�����ڽᣬͶעʧ��
			DECLARE @HandleDescribe VARCHAR(100)
			SELECT @HandleDescribe=HandleDescribe FROM dbo.CT_SchemeETickets WHERE SchemeID=@SchemeID AND TicketStatus=3 AND HandleDescribe LIKE '%��ǰ�����ڽᣬͶעʧ��%' --Ͷעʧ��Ʊ��
			
			--ʧ�ܵĽ��н���
			IF @WithdrawalMoney > 0
			BEGIN
				IF @WithdrawalMoney < @SchemeMoney	--����Ͷעʧ�ܿ��Ե��ɳ�Ʊʧ��
				BEGIN
					UPDATE dbo.CT_Schemes SET SchemeStatus=8, Describe=Describe+@HandleDescribe WHERE SchemeID=@SchemeID
					IF @@ROWCOUNT = 0
					BEGIN
						SELECT @ReturnValue = -1, @ReturnDescription = '���·���״̬ʧ��'	
					END
				END
				ELSE IF @WithdrawalMoney = @SchemeMoney	--ȫ��Ͷעʧ�ܿ��Ե��ɶ�������
				BEGIN
					UPDATE dbo.CT_Schemes SET SchemeStatus=12, Describe=Describe+@HandleDescribe WHERE SchemeID=@SchemeID
					IF @@ROWCOUNT = 0
					BEGIN
						SELECT @ReturnValue = -1, @ReturnDescription = '���·���״̬ʧ��'	
					END
				END

				--����
				UPDATE u SET u.Balance = u.Balance + r.TradeAmount FROM dbo.CT_UsersRecord AS r
				INNER JOIN dbo.CT_Users AS u ON u.UserID=r.UserID AND r.TradeType=1 
				WHERE r.UserID = @UserID AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID);

				INSERT dbo.CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
				SELECT r.UserID,12,r.TradeAmount,u.Balance,'Ͷעʧ���˿�',@Time,r.RelationID,r.OperatorID FROM dbo.CT_UsersRecord AS r
				INNER JOIN dbo.CT_Users AS u ON u.UserID=r.UserID AND r.TradeType=1 
				WHERE r.UserID = @UserID AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID);
				--��ȯ����
				UPDATE c SET c.Balance = c.Balance + r.TradeAmount FROM dbo.CT_UsersRecord AS r
				INNER JOIN CaileCoupons.dbo.CT_Coupons AS c ON c.CouponsID = r.CouponsID 
				AND c.UserID = r.UserID AND r.TradeType=22
				WHERE r.UserID = @UserID AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID);

				INSERT dbo.CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID,CouponsID)
				SELECT r.UserID,23,r.TradeAmount,c.Balance,'��ȯͶעʧ���˿�',@Time,r.RelationID,r.OperatorID,r.CouponsID FROM dbo.CT_UsersRecord AS r
				INNER JOIN CaileCoupons.dbo.CT_Coupons AS c ON c.CouponsID = r.CouponsID 
				AND c.UserID = r.UserID AND r.TradeType=22
				WHERE r.UserID = @UserID AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID);
				--�ʶ�����
				UPDATE u SET u.GoldBean = u.GoldBean + r.TradeAmount FROM dbo.CT_UsersRecord AS r
				INNER JOIN dbo.CT_Users AS u ON u.UserID=r.UserID AND r.TradeType=24 
				WHERE r.UserID = @UserID AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID);

				INSERT dbo.CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
				SELECT r.UserID,25,r.TradeAmount,u.Balance,'Ͷעʧ���˿�',@Time,r.RelationID,r.OperatorID FROM dbo.CT_UsersRecord AS r
				INNER JOIN dbo.CT_Users AS u ON u.UserID=r.UserID AND r.TradeType=24 
				WHERE r.UserID = @UserID AND r.RelationID = CONVERT(VARCHAR(32),@SchemeID);

				COMMIT TRAN
			END
		END
	ELSE
		BEGIN
			SELECT @WithdrawalMoney=SUM(TicketMoney) FROM dbo.CT_SchemeETickets WHERE SchemeID=@SchemeID AND ChaseTaskDetailsID=@ChaseTaskDetailsID AND TicketStatus=3--Ͷעʧ��Ʊ��
			--ʧ�ܵĽ��н���
			IF @WithdrawalMoney > 0
				BEGIN
					UPDATE dbo.CT_Users SET Balance = Balance+@WithdrawalMoney WHERE UserID = @UserID
					IF @@ROWCOUNT = 0
					BEGIN
						SELECT @ReturnValue = -2, @ReturnDescription = '�����û����ʧ��'	
					END
					INSERT dbo.CT_UsersRecord(UserID, TradeType, TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
						SELECT @UserID, 14, @WithdrawalMoney, (Balance-@WithdrawalMoney), 'Ͷעʧ���˿�', GETDATE(), @SchemeID, 1 FROM dbo.CT_Users WHERE UserID=@UserID
					IF @@ROWCOUNT = 0
					BEGIN
						SELECT @ReturnValue = -3, @ReturnDescription = '��¼Ͷעʧ���˿��¼ʧ��'	
					END
				END
		END 

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	SELECT @ReturnValue=0, @ReturnDescription='�����ɹ�'
	RETURN 0;
END



GO
/****** Object:  StoredProcedure [dbo].[udp_TicketsStorage]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joan
-- Create date: 2017-07-19
-- Description:	����Ʊ���
-- =============================================
CREATE PROCEDURE [dbo].[udp_TicketsStorage]
	@SchemeID BIGINT, 
	@ChaseTaskDetailsID BIGINT,
	@IsRobot BIT,
	@TicketTable XML
    --WITH ENCRYPTION
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @ItemTicketTable TABLE(pc INT /*�淨����*/ ,at BIGINT /*Ʊ���*/,mp INT /*Ͷע����*/,n VARCHAR(200)/*Ͷע����*/,sdid BIGINT /*������ϸ���ʶ*/,ts INT /*��Ʊƽ̨*/,s TINYINT /*����Ʊ״̬ Ĭ��0 ��������ֱ�ӳ�Ʊ*/);
	DECLARE @error INT,@LotteryCode INT,@OutType SMALLINT,@BuyType TINYINT
	INSERT INTO @ItemTicketTable 
		(pc,at,mp,n,sdid ,ts,s) 
	SELECT T.c.value('(pc/text())[1]','INT'), 
		   T.c.value('(at/text())[1]','BIGINT'), 
		   T.c.value('(mp/text())[1]','INT'), 
		   T.c.value('(n/text())[1]','VARCHAR(200)'), 
		   T.c.value('(sdid/text())[1]','BIGINT'), 
		   T.c.value('(ts/text())[1]','INT'),
		   T.c.value('(s/text())[1]','TINYINT')
	FROM @TicketTable.nodes('/ArrayOfUdv_ts/udv_ts') AS T(c);
	--ע��䶯
	--����Ǳ��س�Ʊ������Ϊ��Ʊ�ɹ�
	--��ѯ������Ϣ
	SELECT @LotteryCode=LotteryCode,@BuyType=BuyType FROM dbo.CT_Schemes WHERE SchemeID=@SchemeID
	--��ѯ�����Ƿ����ó�Ʊ״̬
	SELECT @OutType = PrintOutType FROM dbo.CT_Lotteries WHERE LotteryCode = @LotteryCode
	
	IF @IsRobot=1
		BEGIN
			UPDATE dbo.CT_Schemes SET IsSplit=1,SchemeStatus=6 WHERE SchemeID=@SchemeID;
		END
	ELSE
		BEGIN
			UPDATE dbo.CT_Schemes SET IsSplit=1 WHERE SchemeID=@SchemeID;
		END
	--���س�Ʊ  ����Ʊ״̬Ϊ 0 
	IF @OutType = 0
		BEGIN
			INSERT INTO CT_SchemeETickets(TicketSourceID, SchemeID, PlayCode, TicketMoney, Multiple, Number, TicketStatus, HandleDescribe, ChaseTaskDetailsID,SDID)
			SELECT 0,@SchemeID,pc,at,mp,n,2,'Ͷע�ɹ�,',@ChaseTaskDetailsID,sdid FROM @ItemTicketTable;
			IF @BuyType != 1
				BEGIN
					UPDATE dbo.CT_Schemes SET IsSplit=1,SchemeStatus=6 WHERE SchemeID=@SchemeID;
				END
		END
	ELSE
		BEGIN
			INSERT INTO CT_SchemeETickets(TicketSourceID, SchemeID, PlayCode, TicketMoney, Multiple, Number, TicketStatus, HandleDescribe, ChaseTaskDetailsID,SDID)
			SELECT ts,@SchemeID,pc,at,mp,n,s,'',@ChaseTaskDetailsID,sdid FROM @ItemTicketTable;
		END

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			SELECT -1;
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END





GO
/****** Object:  StoredProcedure [dbo].[udp_UpdateSchemesOutTicketStatus]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--���·����ų�Ʊ״̬
--2017-5-10
CREATE PROC [dbo].[udp_UpdateSchemesOutTicketStatus]
	@SchemeID BIGINT
	--WITH ENCRYPTION
AS
	DECLARE @Count BIGINT,@BuyType TINYINT,@ChaseTaskCount INT;
	SELECT @Count=COUNT(1) FROM dbo.CT_SchemeETickets WHERE SchemeID=@SchemeID AND (TicketStatus=3 OR TicketStatus=4)
	SELECT @BuyType=BuyType FROM CT_Schemes WHERE SchemeID=@SchemeID
	IF @BuyType!=1
		BEGIN
			IF @Count > 0
				UPDATE dbo.CT_Schemes SET SchemeStatus = 8 WHERE SchemeID=@SchemeID
			ELSE
				UPDATE dbo.CT_Schemes SET SchemeStatus = 6 WHERE SchemeID=@SchemeID
		END



GO
/****** Object:  StoredProcedure [dbo].[udp_UpUserPayRefund]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--�����˿������¼״̬
--2017-5-10
CREATE PROC [dbo].[udp_UpUserPayRefund]
	@ReID BIGINT,
	@Result TINYINT, --0.�˿���� 1.�˿�ɹ� 2.�˿�ʧ�� 3.�˿�ʧ��,���ظ��˿� 4.�˿���Ҫ�������¸�Ԥ
	@ReturnValue	INT OUTPUT,
	@ReturnDescription VARCHAR(100) OUTPUT
	--WITH ENCRYPTION
AS
	IF @Result = 0
	BEGIN
		SELECT @ReturnValue = 0, @ReturnDescription = '�����ɹ�'
		RETURN 0
	END
	
	DECLARE @UserID BIGINT, @PayID BIGINT, @RefundFee BIGINT
	SELECT @PayID=PayID, @RefundFee=Amount FROM dbo.CT_UserPayRefund WHERE ReID = @ReID
	IF @@ROWCOUNT= 0
	BEGIN
		SELECT @ReturnValue = -1, @ReturnDescription = '�˿������¼�����ڣ��˿�ʧ��'	
		RETURN -1
	END
	SELECT @UserID=UserID FROM dbo.CT_UserPayDetails WHERE PayID = @PayID
	IF @@ROWCOUNT= 0
	BEGIN
		SELECT @ReturnValue = -2, @ReturnDescription = '��ֵ��¼�����ڣ��˿�ʧ��'	
		RETURN -2
	END
	
	BEGIN TRAN
	IF @Result = 1
	BEGIN
		UPDATE dbo.CT_UserPayRefund SET Result = @Result WHERE ReID = @ReID
		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -3, @ReturnDescription = '�����˿������¼״̬ʧ�ܣ��˿�ʧ��'	
			RETURN -3
		END
		UPDATE dbo.CT_UserPayDetails SET Result=2 WHERE PayID=@PayID
		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -4, @ReturnDescription = '�����˿������¼״̬ʧ�ܣ��˿�ʧ��'	
			RETURN -4
		END
		UPDATE dbo.CT_Users SET Freeze=Freeze-@RefundFee WHERE UserID=@UserID
		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -5, @ReturnDescription = '�����û�������ʧ��'	
			RETURN -5
		END
	END
	ELSE --IF @Result = 2 OR @Result = 3 OR @Result = 4
	BEGIN
		UPDATE dbo.CT_UserPayRefund SET Result = @Result WHERE ReID = @ReID
		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -3, @ReturnDescription = '�����˿������¼״̬ʧ��'	
			RETURN -3
		END
		UPDATE dbo.CT_UserPayDetails SET Result=1 WHERE PayID=@PayID
		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -4, @ReturnDescription = '�����˿������¼״̬ʧ��'	
			RETURN -4
		END
		INSERT dbo.CT_AccountChange(UserID,TradeType,TradeAmount, Balance, TradeRemark, CreateTime, RelationID, OperatorID)
			SELECT UserID,17, @RefundFee, Balance, '�����˿�ʧ�ܷ��ؽ��', GETDATE(), @PayID, 1 FROM dbo.CT_Users WHERE UserID=@UserID
		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -5, @ReturnDescription = '��ӽ��䶯��¼ʧ��'	
			RETURN -5
		END
		UPDATE dbo.CT_Users SET Balance=Balance+@RefundFee, Freeze=Freeze-@RefundFee WHERE UserID=@UserID
		IF @@ROWCOUNT = 0
		BEGIN
			ROLLBACK TRAN
			SELECT @ReturnValue = -6, @ReturnDescription = '�����û����ʧ��'	
			RETURN -6
		END
	END
	COMMIT TRAN
	
	SELECT @ReturnValue = 0, @ReturnDescription = '�����ɹ�'	
	RETURN 0	


GO
/****** Object:  StoredProcedure [dbo].[udp_UsersExtend]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joan
-- Create date: 2017-07-11
-- Description:	�û�������Ϣ
-- =============================================
CREATE PROCEDURE [dbo].[udp_UsersExtend]
	@UsersExtendTable xml,
	@Rec INT OUTPUT -- ���ؽ��
	--WITH ENCRYPTION
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @ItemUsersExtendTable TABLE(UserID BIGINT ,NickName VARCHAR(20),UserLevel TINYINT,SpecialLevel TINYINT,FullName VARCHAR(26),IDNumber VARCHAR(18),AvatarAddress VARCHAR(200),WechatID VARCHAR(100),WechatToken VARCHAR(200),AliPayID VARCHAR(100),AliPayToken VARCHAR(200),QQID VARCHAR(100),QQToken VARCHAR(200),Email VARCHAR(50),BindType TINYINT,CreateTime DATETIME,BindTime DATETIME,Idols INT, SourceType TINYINT,IpAddress VARCHAR(32),RelationID VARCHAR(100),IsVerify BIT,IsBindTel BIT);
	
	INSERT INTO @ItemUsersExtendTable 
	(UserID ,NickName ,UserLevel ,SpecialLevel ,FullName ,IDNumber ,AvatarAddress,WechatID ,WechatToken ,AliPayID ,AliPayToken ,QQID ,QQToken ,Email ,BindType ,CreateTime ,BindTime ,Idols , SourceType ,IpAddress ,RelationID ,IsVerify ,IsBindTel) 
	SELECT T.c.value('(UserID/text())[1]','BIGINT'), 
		   T.c.value('(NickName/text())[1]','VARCHAR(20)'), 
		   T.c.value('(UserLevel/text())[1]','TINYINT'), 
		   T.c.value('(SpecialLevel/text())[1]','TINYINT'), 
		   T.c.value('(FullName/text())[1]','VARCHAR(26)'), 
		   T.c.value('(IDNumber/text())[1]','VARCHAR(18)'), 
		   T.c.value('(AvatarAddress/text())[1]','VARCHAR(200)'), 
		   T.c.value('(WechatID/text())[1]','VARCHAR(100)'), 
		   T.c.value('(WechatToken/text())[1]','VARCHAR(200)'), 
		   T.c.value('(AliPayID/text())[1]','VARCHAR(100)'), 
		   T.c.value('(AliPayToken/text())[1]','VARCHAR(200)'), 
		   T.c.value('(QQID/text())[1]','VARCHAR(100)'), 
		   T.c.value('(QQToken/text())[1]','VARCHAR(200)'), 
		   T.c.value('(Email/text())[1]','VARCHAR(50)'), 
		   T.c.value('(BindType/text())[1]','TINYINT'), 
		   T.c.value('(CreateTime/text())[1]','DATETIME'), 
		   T.c.value('(BindTime/text())[1]','DATETIME'), 
		   T.c.value('(Idols/text())[1]','INT'), 
		   T.c.value('(SourceType/text())[1]','TINYINT'), 
		   T.c.value('(IpAddress/text())[1]','VARCHAR(32)'), 
		   T.c.value('(RelationID/text())[1]','VARCHAR(100)'), 
		   T.c.value('(IsVerify/text())[1]','BIT'), 
		   T.c.value('(IsBindTel/text())[1]','BIT')
		FROM @UsersExtendTable.nodes('/UsersExtendEntity') AS T(c)
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Rec = 2; -- ��ѯʧ��
		END
	INSERT INTO dbo.CT_UsersExtend( UserID ,NickName ,UserLevel ,SpecialLevel ,FullName ,IDNumber ,AvatarAddress ,WechatID ,WechatToken ,AliPayID ,AliPayToken ,QQID ,QQToken ,Email ,BindType ,CreateTime ,BindTime ,Idols ,SourceType ,IpAddress ,RelationID ,IsVerify ,IsBindTel)
	                         SELECT UserID ,NickName ,UserLevel ,SpecialLevel ,FullName ,IDNumber ,AvatarAddress,WechatID ,WechatToken ,AliPayID ,AliPayToken ,QQID ,QQToken ,Email ,BindType ,CreateTime ,BindTime ,Idols , SourceType ,IpAddress ,RelationID ,IsVerify ,IsBindTel FROM @ItemUsersExtendTable
	IF @@TRANCOUNT > 0
		BEGIN
			SET @Rec =  3; -- ����ʧ��
		END

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN
            DECLARE @msg VARCHAR(200)= N'' + ERROR_MESSAGE();
            RAISERROR(@msg,16,-1);
            RETURN @@ERROR;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
	SET @Rec = 0;
END



GO
/****** Object:  StoredProcedure [dbo].[udp_UsersRecord]    Script Date: 2018/1/18 14:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		joan
-- Create date: 2017-10-23
-- Description:	�û���ˮ�洢����
-- =============================================
CREATE PROCEDURE [dbo].[udp_UsersRecord]
	@UserID BIGINT,                   --�û����
	@PageIndex INT,                   --��ǰҳ
	@PageSize INT,                    --ҳ��С
	@RecordCount INT OUTPUT,          --�ص��ܼ�¼
	@RecordWithdraw BIGINT OUTPUT,    --�����ֽ��
	@RecordBalance BIGINT OUTPUT      --�˻����
AS
BEGIN
	DECLARE @TabRecord TABLE(Num INT,ID BIGINT,UserID BIGINT,CreateTime DATETIME,TradeAmount BIGINT,Balance BIGINT,RelationID VARCHAR(32),TradeType TINYINT);
	INSERT INTO @TabRecord( Num ,ID ,UserID ,CreateTime ,TradeAmount ,Balance ,RelationID ,TradeType)
	SELECT ROW_NUMBER() OVER(ORDER BY CreateTime DESC) AS Num,ID,UserID,CreateTime,TradeAmount,Balance,RelationID,TradeType FROM dbo.CT_UsersRecord
	WHERE TradeAmount>0 AND UserID=@UserID
	ORDER BY Num OFFSET (@PageIndex - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY;
	SELECT @RecordCount = COUNT(1) FROM dbo.CT_UsersRecord
	WHERE TradeAmount>0 AND UserID=@UserID;

	SELECT @RecordBalance = Balance FROM dbo.CT_Users WHERE UserID=@UserID;
	SELECT @RecordWithdraw = WithdrawMoney FROM dbo.CT_UsersExtend WHERE UserID=@UserID;
	--��������,0.��ֵ 1.�������� 2.���ֶ��� 3.����ʧ�ܽⶳ 4.�𶹶һ� 5.�н� 11.�û����� 
	--12.ϵͳ���� 13.׷�ų��� 14.Ͷעʧ���˿� 15.��Ʊʧ���˿� 16.��ֵ�˿�� 
	--17.�˿�ʧ�ܷ��ؽ��, 20 �ٷ��ӽ� 21 ���ּӽ� 22������� 23�������
	WITH tab AS
	(
		--0��ֵ
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - '+upd.PayType+'��ֵ') AS remarks  FROM @TabRecord AS ur
		INNER JOIN CT_UsersPayDetail AS upd ON upd.UserID=ur.UserID AND upd.Result = 1 AND ur.TradeType=0
		AND (CONVERT(VARCHAR(32),upd.PayID) = ur.RelationID OR CONVERT(VARCHAR(50),upd.OrderNo) = ur.RelationID)
		UNION ALL
		--1��������
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('֧�� - '+l.LotteryName+'Ͷע') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID = ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID)=ur.RelationID AND ur.TradeType=1
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--2���ֶ���
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('֧�� - ����') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_UsersWithdraw AS uw ON uw.UserID=ur.UserID AND ur.TradeType=2
		AND (uw.PayOutStatus=0 OR uw.PayOutStatus=2 OR uw.PayOutStatus=4)
		UNION ALL
		--3����ʧ�ܽⶳ
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - ���ֳ���:'+uw.Remark) AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_UsersWithdraw AS uw ON uw.UserID=ur.UserID AND uw.PayOutStatus=6 AND ur.TradeType=3
		UNION ALL
		--4�ʶ��һ�(�ͻ��˲��Ķ�������� ��λ����һ�ٱ�)
		SELECT UserID,CreateTime,TradeAmount*100,Balance*100,('���� - �ʶ��һ�') AS remarks FROM @TabRecord WHERE TradeType=4
		UNION ALL
		--5�н�
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - '+l.LotteryName+'�н�') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND ur.TradeType=5
		INNER JOIN dbo.CT_SchemeETickets AS se ON CONVERT(VARCHAR(32),se.SchemeETicketsID) = ur.RelationID AND se.SchemeID=s.SchemeID
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--11 �û�����
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - '+l.LotteryName+'����') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID) = ur.RelationID AND ur.TradeType=11
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--12 ϵͳ����
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - '+l.LotteryName+'����') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID) = ur.RelationID AND ur.TradeType=12
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--13 ׷�ų���
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - '+l.LotteryName+'����') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID) = ur.RelationID AND ur.TradeType=13
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--14 Ͷעʧ���˿�
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - '+l.LotteryName+'����') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID) = ur.RelationID AND ur.TradeType=14
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--15 Ͷעʧ���˿�
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - '+l.LotteryName+'����') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID) = ur.RelationID AND ur.TradeType=15
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--16 ��ֵ�˿��
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('֧�� - ��ֵ�˿�') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_UsersPayDetail AS upd ON upd.UserID = ur.UserID  AND ur.TradeType=16
		INNER JOIN dbo.CT_UsersPayRefund AS upr ON upr.PayID = upd.PayID 
		AND (upr.Result=0 OR upr.Result=1 OR upr.Result=4)
		UNION ALL
		--17 �˿�ʧ�ܷ��ؽ��
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - ��ֵ�˿�ʧ��') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_UsersPayDetail AS upd ON upd.UserID = ur.UserID  AND ur.TradeType=17
		INNER JOIN dbo.CT_UsersPayRefund AS upr ON upr.PayID = upd.PayID 
		AND (upr.Result=2 OR upr.Result=3)
		UNION ALL
		--20 �ٷ��ӽ�
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - '+l.LotteryName+'�ٷ��ӽ�') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_SchemesAwards AS sa ON sa.UserID=ur.UserID AND CONVERT(VARCHAR(32),sa.SchemeID) = ur.RelationID AND ur.TradeType=20
		INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID = sa.SchemeID
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode=s.LotteryCode
		UNION ALL
		--21 ���ּӽ�
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - ���ּӽ�') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_SchemesAwards AS sa ON sa.UserID=ur.UserID AND CONVERT(VARCHAR(32),sa.SchemeID) = ur.RelationID AND ur.TradeType=21
		INNER JOIN dbo.CT_Schemes AS s ON s.SchemeID = sa.SchemeID
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode=s.LotteryCode
		UNION ALL
		--22 ��ȯ����
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('֧�� - '+l.LotteryName+'��ȯͶע') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID)=ur.RelationID AND ur.TradeType=22 
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--23 ��ȯ����
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount,ur.Balance,('���� - '+l.LotteryName+'��ȯ����') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID)=ur.RelationID AND ur.TradeType=23 
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--24 ��ȯ����(�ͻ��˲��Ķ�������� ��λ����һ�ٱ�)
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount*100,ur.Balance*100,('֧�� - '+l.LotteryName+'�ʶ�Ͷע') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID)=ur.RelationID AND ur.TradeType=24 
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
		UNION ALL
		--25 ��ȯ����(�ͻ��˲��Ķ�������� ��λ����һ�ٱ�)
		SELECT ur.UserID,ur.CreateTime,ur.TradeAmount*100,ur.Balance*100,('���� - '+l.LotteryName+'�ʶ�����') AS remarks FROM @TabRecord AS ur
		INNER JOIN dbo.CT_Schemes AS s ON s.InitiateUserID=ur.UserID AND CONVERT(VARCHAR(32),s.SchemeID)=ur.RelationID AND ur.TradeType=25 
		INNER JOIN dbo.CT_Lotteries AS l ON l.LotteryCode = s.LotteryCode
	)
	SELECT * FROM tab ORDER BY tab.CreateTime DESC
END




GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷����ϸ��ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'ChaseTaskID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'SchemeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ں�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'IsuseID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'Multiple'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����׷�Ž��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷��ʹ�ú�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'RedPacketId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʹ�ú��֧���Ľ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'RedPacketMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬:0 δ���� 1 �û����� 2 ϵͳ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'QuashStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ִ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'IsExecuted'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ܼ���0 ������ 1 ����ֹ 2 ������ 3 ��Զ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'SecrecyLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ͷע����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'LotteryNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails', @level2type=N'COLUMN',@level2name=N'IsShare'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷��������ϸ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTaskDetails'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷��������ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'ChaseTaskID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����˱��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֱ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʼʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'StartTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'IsuseCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ͷע��ʽ��1.��ѡ  2.��ѡ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'BetType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֹ������1.ȫ������  2.һ�Ƚ�����Ƚ�   3.���Ƚ����ϣ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'StopTypeWhenWin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֹ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'StopTypeWhenWinMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬:0 δ���� 1 �û����� 2 ϵͳ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'QuashStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'Descriptions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ӷ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'SchemeBonusScale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷����Դ��׷���������ԵĿͻ��ˣ�1��ʾ������ҳ��2��ʾ�����ֻ�Ӧ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks', @level2type=N'COLUMN',@level2name=N'FromClient'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_ChaseTasks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_IsuseBonuses', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ԱID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_IsuseBonuses', @level2type=N'COLUMN',@level2name=N'AdminID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ں�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_IsuseBonuses', @level2type=N'COLUMN',@level2name=N'IsuseID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ԭ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_IsuseBonuses', @level2type=N'COLUMN',@level2name=N'defaultMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'˰�󽱽�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_IsuseBonuses', @level2type=N'COLUMN',@level2name=N'DefaultMoneyNoWithTax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_IsuseBonuses', @level2type=N'COLUMN',@level2name=N'WinNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�н�ע��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_IsuseBonuses', @level2type=N'COLUMN',@level2name=N'WinBet'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ںſ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_IsuseBonuses'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ں���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'IsuseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʼʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'StartTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֹʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷�������Ƿ�ִ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'IsExecuteChase'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ񿪽���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'IsOpened'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'OpenNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������ԱID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'OpenOperatorID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬��0 δ���� 1 ��ʼ 2 ��ͣ 3 ��ֹ 4 �ڽ� 5 ���� 6 ��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'IsuseState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬�ĸ���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'OpenNotice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ܶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'TotalSales'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'OpenTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���棺��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'WinRollover'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ͷע��ʾ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses', @level2type=N'COLUMN',@level2name=N'BettingPrompt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ںż�¼��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Isuses'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'LotteryID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֱ�����򣬲�ȡ��λ���룬�������£�����һ��10����һ�ڵģ��¿���101����������102������ʱʱ��121   ���Ͷ��������һ�ڵģ������˿�201 ��  �����������쿪���ģ�˫ɫ��801������͸901}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'LotteryName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ƴ����д' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'Shorthand'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����⣬�൱��С��棬�硰��Ԫ�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'Subhead'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���׷������ (�磺 ''10:��'' ��ʾ�����׷10�� ''1:��'' ��ʾ��Ƶ�淨��׷1����������)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'MaxChaseIsuse'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ���ͣ�1.���� 2.��� 3.��� 4.��Ƶ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'TypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������ʽ  ��������Ŵ���������ر���루�硤14��˫ɫ�����ӵ���������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'WinNumberExemple'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������ڹ���10�֣�5�֣�[2,4,7]��[1,3,6]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'IntervalType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ��ʽ(0 - ���س�Ʊ 1 - ��������Ʊ)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'PrintOutType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'Price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ͷע�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'MaxMultiple'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֹͶעʱ�䣬��λ ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'OffTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷���ӳ�ִ�м��ʱ�䣬��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'ChaseDeferTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����׷����ǰʱ��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'QuashChaseTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֽ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'Kinformation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��ص��Ƽ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'IsEmphasis'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ϸ�汾��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'ModuleVersion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ӽ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'IsAddaward'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ֹͣ����ԭ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'StopReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ֹͣ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'IsStop'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ����Ų���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'IsHot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ں���ǰ����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'AdvanceEndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ԥ��ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'PresellTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ⱥ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'ChatGroups'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������ұ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Lotteries', @level2type=N'COLUMN',@level2name=N'ChatRooms'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'վ��ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'NewsID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ѷ��ĿID T_NewsTypes��ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'TypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ؼ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'Keys'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ı�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'RichText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ı�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'PlainText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'Author'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Դ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'Source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��Ƽ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'IsRecommend'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʾ�豸��1.ȫ�� 2.��վ 3.�ͻ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'Equipment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'PublishID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'Publish'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'PublishTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸�ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'ModifyTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'Modify'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸���ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'ModifyID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'AuditingID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'Auditing'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'AuditingTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ķ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'ReadNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'SupportNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'OpposeNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���״̬��1.����� 2.���ͨ�� 3.���δͨ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'AuditingStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ɾ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'IsDel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƽ�Ͷע����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News', @level2type=N'COLUMN',@level2name=N'LotNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ѯ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_News'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ID��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_NewsTypes', @level2type=N'COLUMN',@level2name=N'TypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ڵ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_NewsTypes', @level2type=N'COLUMN',@level2name=N'ParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_NewsTypes', @level2type=N'COLUMN',@level2name=N'TypeName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ���ʾ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_NewsTypes', @level2type=N'COLUMN',@level2name=N'IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ϵͳ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_NewsTypes', @level2type=N'COLUMN',@level2name=N'IsSystem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_NewsTypes', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_NewsTypes', @level2type=N'COLUMN',@level2name=N'Remarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ϣ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_NewsTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ��ϸ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'OutETicketsID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ�̣�1��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'MerchantCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�淨��� ���򣬲�ȡ��λ���룬��λ���ֱ��+��λ��ţ��������£��¿�����ֵ3,10101  �¿�����ֵ4,10102  �¿�����ͬ��,10117˫ɫ���ѡһע,80101' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�̻�Ʊ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'MerchantTicket'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ʊ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'HuaYangTicket'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'Money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ͷע����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'Multiple'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'Bonus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'SendTicketDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'OutTicketDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ״̬(1-δ��Ʊ��2-��Ʊ�ɹ���3-��Ʊʧ��)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_OutETickets', @level2type=N'COLUMN',@level2name=N'OutTicketStauts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_PlayTypes', @level2type=N'COLUMN',@level2name=N'PlayID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�淨��� ���򣬲�ȡ��λ���룬��λ���ֱ��+��λ��ţ��������£��¿�����ֵ3,10101  �¿�����ֵ4,10102  �¿�����ͬ��,10117˫ɫ���ѡһע,80101' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_PlayTypes', @level2type=N'COLUMN',@level2name=N'PlayCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_PlayTypes', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�淨����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_PlayTypes', @level2type=N'COLUMN',@level2name=N'PlayName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_PlayTypes', @level2type=N'COLUMN',@level2name=N'Price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_PlayTypes', @level2type=N'COLUMN',@level2name=N'ModuleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ͷע�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_PlayTypes', @level2type=N'COLUMN',@level2name=N'MaxMultiple'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_PlayTypes', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�淨��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_PlayTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊƽ̨��0����:1����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'TicketSourceID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��PS������ֶ������ű�����Ƕ����Ʊ��ŵ�ƴ���ַ������淨��� ���򣬲�ȡ��λ���룬��λ���ֱ��+��λ��ţ��������£��¿�����ֵ3,10101  �¿�����ֵ4,10102  �¿�����ͬ��,10117˫ɫ���ѡһע,80101' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ����ƴ���ַ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'LotteryName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���۽��ݺͷ���ƴ�ӵ��ַ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'SalesRebate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ļ�ǩ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'FileSign'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ա���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'OperatorID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'OperatorName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'OperatorTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����˱��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'AuditorID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'AuditorName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'AuditTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬��-2�����ڣ�-1����˲�ͨ����1������ˣ�2����Ч��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'SalePointStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'StartTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePoint', @level2type=N'COLUMN',@level2name=N'Describe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��λ����ļ�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointFile', @level2type=N'COLUMN',@level2name=N'SalePointFileID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointFile', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ļ�·��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointFile', @level2type=N'COLUMN',@level2name=N'FileUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ļ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointFile', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ļ���׺' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointFile', @level2type=N'COLUMN',@level2name=N'FileEXT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ļ�ǩ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointFile', @level2type=N'COLUMN',@level2name=N'FileSign'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊƽ̨��0����:1����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointRecord', @level2type=N'COLUMN',@level2name=N'TicketSourceID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointRecord', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���۽��ݼ���λ���ַ���ƴ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointRecord', @level2type=N'COLUMN',@level2name=N'SalesRebate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��һ�ε����۽��ݼ���λ���ַ���ƴ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointRecord', @level2type=N'COLUMN',@level2name=N'LastSalesRebate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Чʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointRecord', @level2type=N'COLUMN',@level2name=N'StartTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SalePointRecord', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ʊ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'SchemeETicketsID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������ʶ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'SDID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊƽ̨��0����:1����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'TicketSourceID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ӧ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'SchemeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�淨���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'PlayCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ʊ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'TicketMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ͷע����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'Multiple'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ͷע����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'Number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʧ���ظ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'Sends'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'HandleDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'HandleDescribe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ��ʾ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'Identifiers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ʊ����Ϣ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'Ticket'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'WinMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬,0.����Ʊ��1.Ͷע�ɹ���2.��Ʊ��ɣ�3.Ͷעʧ�ܣ�4.��Ʊʧ�ܣ�8.�ҽ��У�10�н���11.����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'TicketStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'׷����ϸID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'ChaseTaskDetailsID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets', @level2type=N'COLUMN',@level2name=N'IsRobot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ʊ����������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemeETickets'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'SchemeNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����˱��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'InitiateUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�淨��� ���򣬲�ȡ��λ���룬��λ���ֱ��+��λ��ţ��������£��¿�����ֵ3,10101  �¿�����ֵ4,10102  �¿�����ͬ��,10117˫ɫ���ѡһע,80101' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ں���ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'IsuseID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ں�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'IsuseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������루�������ݸ�ʽ���£�ע��ע֮���á�|���ָ�淨��ע֮���á�-���ָͶע����֮���á�,���ָ����������֮���á�+���ָ�,���������������á�(���ָ���硾10101-3|10113-15|10119-3,3,3|80101-1,2,3,5,6,7+5(2|90101-4,5,6,7,8+4,8����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'LotteryNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ܽ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'SchemeMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ܼ��� 0������ 1 ���ܵ���ֹ 2���ܵ����� 3��Զ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'SecrecyLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����״̬ 0�����2.�������ڣ�4�µ��ɹ���6.��Ʊ�ɹ���8.���ֳ�Ʊ�ɹ���10.�µ�ʧ�ܣ��޺ţ���12.����������14.�н���15.�ɽ��У�16.�ɽ���ɣ�18.���н���ɣ�19.׷�Ž����У�20.׷�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'SchemeStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ��ʽ 1���س�Ʊ 2��������Ʊ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'PrintOutType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'Describe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������Դ 1��ҳ 2 IOS 3 Andiord' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'FromClient'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������� 0.���� 1׷��2.����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'BuyType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����Ƿ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'IsSplit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'FollowSchemeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���Ƹ���Ӷ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'FollowSchemeBonus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���Ƹ���Ӷ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'FollowSchemeBonusScale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ӽ�������ʱ�򣬻���ּӽ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes', @level2type=N'COLUMN',@level2name=N'PlusAwards'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Schemes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'SDID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'SchemeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�淨����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'PlayCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'Multiple'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ע��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'BetNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ͷע����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'BetNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��ѳ�Ʊ 0δ��Ʊ 1�ѳ�Ʊ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'IsBuyed'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ǵ��ϲ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'IsNorm'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��н�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'IsWin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ񿪽�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'IsOpened'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'WinMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'˰�󽱽�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'WinMoneyNoWithTax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�н�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'WinDescribe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'PrintOutTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�н�ͼƬURL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'WinImageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������Ա���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail', @level2type=N'COLUMN',@level2name=N'OpenOperatorID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ϸ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'SchemesDetailsWinID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ƱID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'SchemeETicketsID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'SchemeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ȱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'WinCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֺ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'SplitNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'WinNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ͷע����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'Multiple'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�н����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'WinMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�н�˰����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'WinMoneyNoWithTax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��ɽ� 0.�� 1.��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'IsAward'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'Descriptions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ӿ� 1.����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'SupplierID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'BackWinMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����˰����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'BackWinMoneyNoWithTax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'BackDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'AddDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɽ��Ա��Ƿ��н� 0.δ�н� 1.�н�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'WinStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��ֶ��ɽ�,ʹ����˫ɫ�򡢴���͸�� 0.�� 1.��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin', @level2type=N'COLUMN',@level2name=N'IsFirstPrize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�н���¼��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SchemesWin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo', @level2type=N'COLUMN',@level2name=N'SetID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ü�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo', @level2type=N'COLUMN',@level2name=N'SetKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo', @level2type=N'COLUMN',@level2name=N'SetValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo', @level2type=N'COLUMN',@level2name=N'SetName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo', @level2type=N'COLUMN',@level2name=N'SetDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo', @level2type=N'COLUMN',@level2name=N'IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������� 0.string 1.int 2.checked 3.radio 4.url 5datetime 6.other' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo', @level2type=N'COLUMN',@level2name=N'DataType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ֵ(��checked��radio��ֵ)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo', @level2type=N'COLUMN',@level2name=N'DataValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ϵͳ���ñ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemSetInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ��:2017-11-04' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'dateday'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ճ�ֵ�ܶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'recharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������ϳ�ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'online_recharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������³�ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'offline_recharge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������ܶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'withdraw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������Ա' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'users'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������ͽ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'largess'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ռ��ֿ���Ͷע���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'buy_jlk'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ռ��ֿ����н����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'win_jlk'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ս�������Ͷע���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'buy_jxk'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ս��������н����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'win_jxk'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���պ���ʮһ�ƶ��Ͷע���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'buy_hbsyydj'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���պ���ʮһ�ƶ���н����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'win_hbsyydj'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ɽ��ʮһ�ƶ��Ͷע���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'buy_sdsyydj'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ɽ��ʮһ�ƶ���н����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'win_sdsyydj'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������ʱʱ��Ͷע���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'buy_cqssc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������ʱʱ���н����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'win_cqssc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����˫ɫ��Ͷע���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'buy_ssq'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����˫ɫ���н����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'win_ssq'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���մ���͸Ͷע���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'buy_dlt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���մ���͸�н����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_SystemStaticdata', @level2type=N'COLUMN',@level2name=N'win_dlt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_TemplateConfig', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_TemplateConfig', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_TemplateConfig', @level2type=N'COLUMN',@level2name=N'TemplateContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ϣ���� 1.����Announce 2.����Remind 3.˽��Message 4.����Push 5.�ں�����IssuePush' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_TemplateConfig', @level2type=N'COLUMN',@level2name=N'TemplateType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_TemplateConfig', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ԱID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_TemplateConfig', @level2type=N'COLUMN',@level2name=N'AdminID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ģ�����ñ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_TemplateConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ա��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û��ֻ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'UserMobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'UserPassword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧������(6λ���ֺ��MD5)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'PayPassword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'Balance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������Ķһ�������1:100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'GoldBean'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'Freeze'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��ǻ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'IsRobot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�˻��Ƿ������½' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_Users', @level2type=N'COLUMN',@level2name=N'IsCanLogin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���б��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersBanks', @level2type=N'COLUMN',@level2name=N'BankID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersBanks', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������  1�������� 2�������� 3ũҵ���� 4��ͨ���� 5�������� 6��������,100.΢�ţ�101.�Ƹ�ͨ��102.֧����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersBanks', @level2type=N'COLUMN',@level2name=N'BankType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersBanks', @level2type=N'COLUMN',@level2name=N'BankName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���п���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersBanks', @level2type=N'COLUMN',@level2name=N'CardNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersBanks', @level2type=N'COLUMN',@level2name=N'Area'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������ŵ�ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersBanks', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ԥ���绰����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersBanks', @level2type=N'COLUMN',@level2name=N'ReservedPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û��ǳ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'NickName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'UserLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ȼ�(��������н���)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'SpecialLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'FullName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���֤����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'IDNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�ͷ���ַ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'AvatarAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'΢��Ψһ��ʾ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'WechatID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'΢��Ʊ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'WechatToken'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧������ʶ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'AliPayID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧����Ʊ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'AliPayToken'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QQ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'QQID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QQƱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'QQToken'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ʼ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ͣ�0.δ�󶨣�1.�ֻ���2.΢�ţ�3.QQ��4.֧����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'BindType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ע��ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'BindTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'Idols'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Դ���ͣ�0.pc��1.android��2.ios��3.ipad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'SourceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP��ַ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'IpAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'RelationID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ���֤ͨ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'IsVerify'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ���ֻ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'IsBindTel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ֽ��(��ֵ�ٷ�֮��ʮ������)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersExtend', @level2type=N'COLUMN',@level2name=N'WithdrawMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersLoginRecord', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersLoginRecord', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Դ���ͣ�0.pc��1.android��2.ios��3.ipad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersLoginRecord', @level2type=N'COLUMN',@level2name=N'SourceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��½��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersLoginRecord', @level2type=N'COLUMN',@level2name=N'Token'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��½ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersLoginRecord', @level2type=N'COLUMN',@level2name=N'LoginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP��ַ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersLoginRecord', @level2type=N'COLUMN',@level2name=N'IpAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û���½��¼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersLoginRecord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'PayID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���׺�(�ӿڽ��׺�)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'RechargeNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧������,��"Alipay","WeChatPay"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'PayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'FormalitiesFees'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧����� 0.δ�ɹ� 1.�ѳɹ� 2.���˿� 3.�˿����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'Result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'֧��ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'CompleteTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������׺�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail', @level2type=N'COLUMN',@level2name=N'OutRechargeNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֵ��¼��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�˿�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayRefund', @level2type=N'COLUMN',@level2name=N'ReID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayRefund', @level2type=N'COLUMN',@level2name=N'PayID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�˿��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayRefund', @level2type=N'COLUMN',@level2name=N'RefundNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ӿ��˿�׺�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayRefund', @level2type=N'COLUMN',@level2name=N'RechargeNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�˿���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayRefund', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayRefund', @level2type=N'COLUMN',@level2name=N'FormalitiesFees'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ 0.�˿���� 1.�˿�ɹ� 2.�˿�ʧ�� 3.�˿�ʧ��,�����ظ��˿� 4.�˿���Ҫ�������¸�Ԥ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayRefund', @level2type=N'COLUMN',@level2name=N'Result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ύʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayRefund', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPayRefund', @level2type=N'COLUMN',@level2name=N'CompleteTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֻ�Ա��ʶ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPush', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersPush', @level2type=N'COLUMN',@level2name=N'ModifyTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������,0.��ֵ 1.�������� 2.���ֶ��� 3.����ʧ�ܽⶳ 4.�ʶ��һ� 5.�н� 11.�û����� 12.ϵͳ���� 13.׷�ų��� 14.Ͷעʧ���˿� 15.��Ʊʧ���˿� 16.��ֵ�˿�� 17.�˿�ʧ�ܷ��ؽ�� 20 �ٷ��ӽ� 21 ���ּӽ� 22������� 23������� 24�ʶ����� 25�ʶ����� 26.�����ˮ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'TradeType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���׽��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'TradeAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�仯ǰ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'Balance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'TradeRemark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'RelationID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����Ա���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'OperatorID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����Ż�ȯ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord', @level2type=N'COLUMN',@level2name=N'CouponsID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�˻��仯��¼��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersRecord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ��ʽ20170101' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersStaticdata', @level2type=N'COLUMN',@level2name=N'Date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersStaticdata', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����ܶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersStaticdata', @level2type=N'COLUMN',@level2name=N'Buy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�н��ܶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersStaticdata', @level2type=N'COLUMN',@level2name=N'Win'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw', @level2type=N'COLUMN',@level2name=N'PayOutID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����˱��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���б��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw', @level2type=N'COLUMN',@level2name=N'BankID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����˱��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw', @level2type=N'COLUMN',@level2name=N'OperaterID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw', @level2type=N'COLUMN',@level2name=N'OperTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬��0.���룬2.�����У�4.������ɣ�6.����ʧ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw', @level2type=N'COLUMN',@level2name=N'PayOutStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��˱�ע��ʧ�ܿɱ�ע��Ϣ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ּ�¼��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_UsersWithdraw'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������ˮ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'WinID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ȱ��룬��λ��������+��λ�Ա���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'WinCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ֱ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'LotteryCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�淨������򣬲�����λ���ֱ���+��λ�Ա��룬���¿����ġ���ֵ��10101����ͬ��10102' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'PlayCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'WinName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�н����룬�г����з����н��ĺ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'WinNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��ֵ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'IsSumValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ֵ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'SumValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ĭ�Ͻ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'DefaultMoney'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ĭ��˰�󽱽�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes', @level2type=N'COLUMN',@level2name=N'DefaultMoneyNoWithTax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ȱ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CT_WinTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ac"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 145
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "aa"
            Begin Extent = 
               Top = 6
               Left = 273
               Bottom = 145
               Right = 484
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_ActivityApply'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_ActivityApply'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "s"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 146
               Right = 291
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct"
            Begin Extent = 
               Top = 6
               Left = 329
               Bottom = 146
               Right = 584
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cd"
            Begin Extent = 
               Top = 6
               Left = 622
               Bottom = 146
               Right = 828
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cd1"
            Begin Extent = 
               Top = 150
               Left = 38
               Bottom = 290
               Right = 244
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "u"
            Begin Extent = 
               Top = 150
               Left = 282
               Bottom = 290
               Right = 470
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "l"
            Begin Extent = 
               Top = 150
               Left = 508
               Bottom = 290
               Right = 732
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
   ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_ChaseList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'      Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_ChaseList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_ChaseList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 109
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 264
               Bottom = 109
               Right = 476
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 217
               Right = 202
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 114
               Left = 240
               Bottom = 217
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_ComputeTicket'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_ComputeTicket'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 109
               Right = 226
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 264
               Bottom = 109
               Right = 476
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "sd"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 217
               Right = 220
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cd"
            Begin Extent = 
               Top = 114
               Left = 258
               Bottom = 217
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 325
               Right = 202
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_ComputeTicketChaseTasks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_ComputeTicketChaseTasks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 145
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 126
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_Manager'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ni"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 145
               Right = 217
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "un"
            Begin Extent = 
               Top = 6
               Left = 255
               Bottom = 145
               Right = 429
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_NotifyInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_NotifyInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 145
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 145
               Right = 458
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_PlayTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_PlayTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "u"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 146
               Right = 210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ue"
            Begin Extent = 
               Top = 6
               Left = 248
               Bottom = 146
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 150
               Left = 38
               Bottom = 290
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 150
               Left = 244
               Bottom = 290
               Right = 427
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_RechargeDetailReport'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_RechargeDetailReport'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 146
               Right = 291
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "sd"
            Begin Extent = 
               Top = 6
               Left = 329
               Bottom = 146
               Right = 560
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pt"
            Begin Extent = 
               Top = 6
               Left = 598
               Bottom = 146
               Right = 783
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 821
               Bottom = 146
               Right = 1009
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "f"
            Begin Extent = 
               Top = 150
               Left = 38
               Bottom = 290
               Right = 262
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ct"
            Begin Extent = 
               Top = 150
               Left = 300
               Bottom = 290
               Right = 555
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
     ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_SchemeChaseTask'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_SchemeChaseTask'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_SchemeChaseTask'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "s"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 146
               Right = 275
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ctd"
            Begin Extent = 
               Top = 6
               Left = 313
               Bottom = 146
               Right = 503
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "i"
            Begin Extent = 
               Top = 6
               Left = 541
               Bottom = 146
               Right = 729
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "se"
            Begin Extent = 
               Top = 6
               Left = 767
               Bottom = 146
               Right = 970
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_SchemeChaseTaskDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_SchemeChaseTaskDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Tab_1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 146
               Right = 275
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_Schemes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_Schemes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 145
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 145
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_SchemesDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_SchemesDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tab"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 146
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_UserAccountReport'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_UserAccountReport'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 145
               Right = 227
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 265
               Bottom = 145
               Right = 439
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_UserInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_UserInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 145
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 310
               Bottom = 145
               Right = 518
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_WinTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'udv_WinTypes'
GO

