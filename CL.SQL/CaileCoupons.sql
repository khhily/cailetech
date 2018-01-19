USE [master]
GO

/****** Object:  Database [CaileCoupons]    Script Date: 2018/1/18 14:01:40 ******/
CREATE DATABASE [CaileCoupons]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CaileCoupons', FILENAME = N'D:\CaileGame\CL.Database\Server\CaileCoupons.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 51200KB )
 LOG ON 
( NAME = N'CaileCoupons_log', FILENAME = N'D:\CaileGame\CL.Database\Server\CaileCoupons_log.ldf' , SIZE = 103424KB , MAXSIZE = 2048GB , FILEGROWTH = 102400KB )
GO

ALTER DATABASE [CaileCoupons] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CaileCoupons].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [CaileCoupons] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [CaileCoupons] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [CaileCoupons] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [CaileCoupons] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [CaileCoupons] SET ARITHABORT OFF 
GO

ALTER DATABASE [CaileCoupons] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [CaileCoupons] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [CaileCoupons] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [CaileCoupons] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [CaileCoupons] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [CaileCoupons] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [CaileCoupons] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [CaileCoupons] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [CaileCoupons] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [CaileCoupons] SET  DISABLE_BROKER 
GO

ALTER DATABASE [CaileCoupons] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [CaileCoupons] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [CaileCoupons] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [CaileCoupons] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [CaileCoupons] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [CaileCoupons] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [CaileCoupons] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [CaileCoupons] SET RECOVERY FULL 
GO

ALTER DATABASE [CaileCoupons] SET  MULTI_USER 
GO

ALTER DATABASE [CaileCoupons] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [CaileCoupons] SET DB_CHAINING OFF 
GO

ALTER DATABASE [CaileCoupons] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [CaileCoupons] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [CaileCoupons] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [CaileCoupons] SET  READ_WRITE 
GO







USE [CaileCoupons]
GO
/****** Object:  Table [dbo].[CT_Coupons]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_Coupons](
	[CouponsID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActivityID] [int] NOT NULL,
	[GenerateTime] [datetime] NOT NULL,
	[ReleaseTime] [datetime] NULL,
	[FirstTime] [datetime] NULL,
	[LastTime] [datetime] NULL,
	[CouponsStatus] [int] NULL,
	[CouponsType] [int] NULL,
	[StartTime] [datetime] NULL,
	[ExpireTime] [datetime] NULL,
	[FaceValue] [bigint] NULL,
	[Balance] [bigint] NULL,
	[SatisfiedMoney] [bigint] NULL,
	[IsGive] [bit] NULL,
	[LotteryCode] [int] NULL,
	[IsChaseTask] [bit] NULL,
	[IsSuperposition] [bit] NULL,
	[IsTimes] [bit] NULL,
	[IsJoinBuy] [bit] NULL,
	[UserID] [bigint] NULL,
	[CouponsSource] [int] NULL,
 CONSTRAINT [PK_CT_RedPacket] PRIMARY KEY CLUSTERED 
(
	[CouponsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CT_CouponsCDKey]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_CouponsCDKey](
	[CDKeyID] [bigint] IDENTITY(1,1) NOT NULL,
	[CouponsID] [bigint] NOT NULL,
	[PartnerCode] [varchar](20) NULL,
	[GenerateTime] [datetime] NOT NULL,
	[ExpireTime] [datetime] NOT NULL,
	[CDKey] [varchar](50) NULL,
	[EncryptKey] [varchar](20) NULL,
	[IsExchanger] [bit] NULL,
	[ExchangerUserID] [bigint] NULL,
	[ExchangerTime] [datetime] NULL,
 CONSTRAINT [PK_CT_REDPACKETKEY] PRIMARY KEY CLUSTERED 
(
	[CDKeyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CT_CouponsRecord]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CT_CouponsRecord](
	[RecordID] [bigint] IDENTITY(1,1) NOT NULL,
	[CouponsID] [bigint] NOT NULL,
	[LogType] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[RelationID] [varchar](32) NULL,
	[Amount] [bigint] NULL,
 CONSTRAINT [PK_CT_RedPacketLog] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[udp_CouponsCDKeyList]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		joan
-- Create date: 2017-9-26
-- Description:	彩券兑换码列表查询
-- =============================================
CREATE PROCEDURE [dbo].[udp_CouponsCDKeyList]
	@UserName VARCHAR(20),     --用户名
	@StartTime DATETIME,       --彩券失效时间
	@EndTime DATETIME,         --彩券失效时间
	@PageIndex INT,            --当前页
	@PageSize INT,             --每页大小
	@RecordCount INT OUTPUT    --统计
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @SqlStr NVARCHAR(3000),@SqlWhere NVARCHAR(1000),@SqlCount NVARCHAR(1000);
	SET @SqlStr = 'SELECT ROW_NUMBER() OVER(ORDER BY k.CDKeyID DESC,k.ExpireTime DESC) AS Num,u.UserName,u.UserMobile,k.CDKeyID,
				   k.CouponsID,k.PartnerCode,k.GenerateTime,k.ExpireTime,k.CDKey,k.EncryptKey,k.IsExchanger,k.ExchangerUserID,k.ExchangerTime
				   FROM CT_CouponsCDKey AS k
				   INNER JOIN dbo.CT_Coupons AS r ON r.CouponsID = k.CouponsID
				   LEFT JOIN CaileGame.dbo.CT_Users AS u ON u.UserID=k.ExchangerUserID';
	SET @SqlCount = 'SELECT @a = COUNT(k.CDKeyID) FROM CT_CouponsCDKey AS k
                     INNER JOIN dbo.CT_Coupons AS r ON r.CouponsID = k.CouponsID
                     LEFT JOIN CaileGame.dbo.CT_Users AS u ON u.UserID=k.ExchangerUserID';
	--查询条件
	SET @SqlWhere = ' WHERE k.ExpireTime>= ''' + CONVERT(VARCHAR(20),@StartTime) + ''' AND k.ExpireTime <= ''' + CONVERT(VARCHAR(20),@EndTime) + '''';
	IF ISNULL(@UserName,'') != ''
		BEGIN
			SET @SqlWhere = @SqlWhere + ' AND u.UserMobile like ''%' + @UserName + '%''';
		END

	DECLARE @SqlData NVARCHAR(4000),@SqlDatas NVARCHAR(4000);
	SET @SqlData = @SqlStr + @SqlWhere + ' ORDER BY Num
	OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
	FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';

	SET @SqlDatas = @SqlCount + @SqlWhere;
	PRINT @SqlData;
	EXEC sp_executesql @SqlData
	EXEC sp_executesql @SqlDatas,N'@a INT OUTPUT',@RecordCount OUTPUT
END


GO
/****** Object:  StoredProcedure [dbo].[udp_ExchangeCoupons]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		joan
-- Create date: 2017-9-23
-- Description:	彩券兑换
-- =============================================
CREATE PROCEDURE [dbo].[udp_ExchangeCoupons]
	@UserID BIGINT,       --用户编码
	@CouponsID BIGINT     --彩券编码
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @IsExchange BIT,@Time DATETIME = GETDATE(),@ExpireTime DATETIME;
	SELECT @IsExchange = IsExchanger,@ExpireTime = ExpireTime FROM CT_CouponsCDKey WHERE CouponsID = @CouponsID
	IF @IsExchange = 1
		BEGIN
			ROLLBACK TRAN;
			RETURN -2;  --已被兑换
		END
	ELSE IF @ExpireTime < @Time
		BEGIN
			ROLLBACK TRAN;
			RETURN -3; --兑换码已过期
		END 
	ELSE
		BEGIN
			UPDATE CT_CouponsCDKey SET IsExchanger=1,ExchangerUserID=@UserID,ExpireTime=@Time WHERE CouponsID = @CouponsID
			UPDATE CT_Coupons SET UserID=@UserID,ReleaseTime=@Time WHERE CouponsID=@CouponsID
			SELECT 0;
		END 
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT = 0
			ROLLBACK TRAN
			RETURN -1;
	END CATCH
	IF @@TRANCOUNT > 0
	COMMIT TRAN
END




GO
/****** Object:  StoredProcedure [dbo].[udp_QueryCoupons]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		joan
-- Create date: 2017-9-18
-- Description:	查询个人彩券所属列表
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryCoupons]
	@UserID BIGINT,           --用户编号
	@IsCoupons BIT,           --是否可用彩券
	@PageIndex INT,           --当前页
	@PageSize INT,            --每页大小
	@Counts INT OUTPUT        --总数
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE();
	--sql语句
	DECLARE @SqlData NVARCHAR(4000);

	--SQL主体
	IF @IsCoupons = 1
		BEGIN
			SET @SqlData = ' WITH tab AS
							(
								SELECT CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,Balance,SatisfiedMoney,IsGive,
									   LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource FROM CT_Coupons WHERE CouponsType = 3 
									   AND UserID='+CONVERT(VARCHAR(20),@UserID)+' AND Balance > 0 AND (IsTimes=0 AND FaceValue=Balance)
									   UNION 
								SELECT CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,Balance,SatisfiedMoney,IsGive,
									   LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource FROM CT_Coupons WHERE CouponsType != 3
									   AND Balance > 0 AND (IsTimes=0 AND FaceValue=Balance) AND UserID = '+CONVERT(VARCHAR(20),@UserID)+' AND (StartTime <= ''' + CONVERT(VARCHAR(20),@Time) + ''' AND ExpireTime >= ''' + CONVERT(VARCHAR(20),@Time) + ''')
							)
							SELECT ROW_NUMBER() OVER(ORDER BY ExpireTime ASC,FaceValue DESC) AS Num,CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,
							Balance,SatisfiedMoney,IsGive,LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource FROM tab ';
			WITH tab AS
			(
				SELECT CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,Balance,SatisfiedMoney,IsGive,
					   LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource FROM CT_Coupons WHERE CouponsType = 3 
					   AND UserID=@UserID AND Balance > 0 AND (IsTimes=0 AND FaceValue=Balance)
					   UNION ALL
				SELECT CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,Balance,SatisfiedMoney,IsGive,
					   LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource FROM CT_Coupons WHERE CouponsType != 3
					   AND Balance > 0 AND (IsTimes=0 AND FaceValue=Balance) AND UserID = @UserID AND (StartTime <= @Time AND ExpireTime >= @Time)
			)
			SELECT @Counts = COUNT(1) FROM tab
		END
	ELSE
		BEGIN
			SET @SqlData = ' WITH tab AS
							(
								SELECT CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,Balance,SatisfiedMoney,IsGive,
									   LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource FROM CT_Coupons WHERE CouponsType = 3 
									   AND UserID='+CONVERT(VARCHAR(20),@UserID)+' AND (Balance = 0 or (IsTimes=0 AND FaceValue!=Balance))
									   UNION 
								SELECT CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,Balance,SatisfiedMoney,IsGive,
									   LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource FROM CT_Coupons WHERE CouponsType != 3
									   AND UserID = '+CONVERT(VARCHAR(20),@UserID)+' AND ((IsTimes=0 AND FaceValue!=Balance)  OR ExpireTime < ''' + CONVERT(VARCHAR(20),@Time) + ''' OR Balance = 0)
							)
							SELECT ROW_NUMBER() OVER(ORDER BY ExpireTime ASC,FaceValue DESC) AS Num,CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,
							Balance,SatisfiedMoney,IsGive,LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource FROM tab ';
		END
	--SQL条件

	--分页SQL
	SET @SqlData = @SqlData + ' ORDER BY Num
	OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
	FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';

	PRINT '...'+@SqlData
	EXEC sp_executesql @SqlData
END






GO
/****** Object:  StoredProcedure [dbo].[udp_QueryCouponsList]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		joan
-- Create date: 2017-9-26
-- Description:	彩券列表查询
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryCouponsList]
	@UserName VARCHAR(20),     --用户名
	@LotteryCode INT,          --彩种
	@CouponsStatus INT,		   --彩券状态
	@CouponsType INT,		   --彩券类型
	@CouponsSource INT,        --彩券来源
	@StartTime DATETIME,       --彩券失效时间
	@EndTime DATETIME,         --彩券失效时间
	@PageIndex INT,            --当前页
	@PageSize INT,             --每页大小
	@RecordCount INT OUTPUT,   --统计
	@RecordFaceValue BIGINT OUTPUT, --发放金额
	@RecordEmploy BIGINT OUTPUT     --使用金额
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @SqlStr NVARCHAR(3000),@SqlWhere NVARCHAR(1000),@SqlCount NVARCHAR(1000);
	SET @SqlStr = 'SELECT ROW_NUMBER() OVER(ORDER BY r.CouponsID DESC,r.ExpireTime DESC) AS Num,a.ActivitySubject,u.UserName,u.UserMobile,
				   r.CouponsID,r.ActivityID,r.GenerateTime,r.ReleaseTime,r.FirstTime,r.LastTime,r.CouponsStatus,r.CouponsType,r.StartTime,r.ExpireTime
				   ,r.FaceValue,r.Balance,r.SatisfiedMoney,r.IsGive,r.LotteryCode,r.IsChaseTask,r.IsSuperposition,r.IsTimes,r.IsJoinBuy,r.UserID,r.CouponsSource
				   FROM dbo.CT_Coupons AS r
				   INNER JOIN CaileGame.dbo.CT_Activity AS a ON a.ActivityID = r.ActivityID
				   LEFT JOIN CaileGame.dbo.CT_Users AS u ON u.UserID=r.UserID
				   LEFT JOIN dbo.CT_CouponsCDKey AS k ON k.CouponsID = r.CouponsID ';

	SET @SqlCount = 'SELECT @a = COUNT(r.CouponsID) FROM dbo.CT_Coupons AS r
				   INNER JOIN CaileGame.dbo.CT_Activity AS a ON a.ActivityID = r.ActivityID
				   LEFT JOIN CaileGame.dbo.CT_Users AS u ON u.UserID=r.UserID
				   LEFT JOIN dbo.CT_CouponsCDKey AS k ON k.CouponsID = r.CouponsID ';
	--查询条件
	SET @SqlWhere = ' WHERE r.ExpireTime>= ''' + CONVERT(VARCHAR(20),@StartTime) + ''' AND r.ExpireTime <= ''' + CONVERT(VARCHAR(20),@EndTime) + '''';
	IF ISNULL(@UserName,'') != ''
		BEGIN
			SET @SqlWhere = @SqlWhere + ' AND u.UserMobile like ''%' + @UserName + '%''';
		END
	IF @LotteryCode != -1
		BEGIN
			SET @SqlWhere = @SqlWhere +  ' AND r.LotteryCode = ' + CONVERT(VARCHAR(10),@LotteryCode);
		END
	IF @CouponsStatus != -1
		BEGIN
			SET @SqlWhere = @SqlWhere +  ' AND r.CouponsStatus = ' + CONVERT(VARCHAR(10),@CouponsStatus);
		END
	IF @CouponsType != -1
		BEGIN
			SET @SqlWhere = @SqlWhere +  ' AND r.CouponsType = ' + CONVERT(VARCHAR(10),@CouponsType);
		END
	IF @CouponsSource != -1
		BEGIN 
			SET @SqlWhere = @SqlWhere +  ' AND r.CouponsSource = ' + CONVERT(VARCHAR(10),@CouponsSource);
		END

	--统计
	SELECT @RecordFaceValue = SUM(FaceValue),@RecordEmploy = (SUM(FaceValue)-SUM(Balance)) FROM dbo.CT_Coupons

	DECLARE @SqlData NVARCHAR(4000),@SqlDatas NVARCHAR(4000);
	SET @SqlData = @SqlStr + @SqlWhere + ' ORDER BY Num
	OFFSET '+ CONVERT(VARCHAR(20),((@PageIndex - 1) * @PageSize)) +' ROWS
	FETCH NEXT '+ CONVERT(VARCHAR(20),@PageSize) +' ROWS ONLY';

	SET @SqlDatas = @SqlCount + @SqlWhere;
	PRINT @SqlData;
	EXEC sp_executesql @SqlData
	EXEC sp_executesql @SqlDatas,N'@a INT OUTPUT',@RecordCount OUTPUT
END



GO
/****** Object:  StoredProcedure [dbo].[udp_QueryCouponsPayment]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		joan
-- Create date: 2017-9-18
-- Description:	查询可支付彩券
-- =============================================
CREATE PROCEDURE [dbo].[udp_QueryCouponsPayment]
	@UserID	BIGINT,                  --红包所属用户
	@OrderMoney BIGINT,              --订单金额
	@LotteryCode INT                 --订单彩种
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Time DATETIME = GETDATE();
	--红包类型：0固定时间段，1固定时长，2满减，3永不过期
	WITH tab AS
    (
		--满减红包
		SELECT CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,Balance,SatisfiedMoney,IsGive,
			   LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource
			   FROM CT_Coupons WHERE CouponsType = 2 AND UserID = @UserID AND SatisfiedMoney <= @OrderMoney AND Balance > 0
			   AND (LotteryCode = 0 OR LotteryCode = @LotteryCode) AND StartTime <= @Time AND ExpireTime >= @Time
		UNION 
		--永不过期
		SELECT CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,Balance,SatisfiedMoney,IsGive,
			   LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource
			   FROM CT_Coupons WHERE CouponsType = 3 AND UserID = @UserID AND (LotteryCode = 0 OR LotteryCode = @LotteryCode)
			   AND (IsTimes=0 AND FaceValue=Balance) AND Balance > 0
		UNION 
		--0固定时间段，1固定时长
		SELECT CouponsID,ActivityID,CouponsStatus,CouponsType,StartTime,ExpireTime,FaceValue,Balance,SatisfiedMoney,IsGive,
			   LotteryCode,IsChaseTask,IsSuperposition,IsTimes,IsJoinBuy,CouponsSource
			   FROM CT_Coupons WHERE (CouponsType = 0 OR CouponsType = 1) AND (LotteryCode = 0 OR LotteryCode = @LotteryCode) 
			   AND StartTime <= @Time AND ExpireTime >= @Time AND (IsTimes=0 AND FaceValue=Balance) AND UserID=@UserID AND Balance > 0
	)
	SELECT * FROM tab
END





GO
/****** Object:  StoredProcedure [dbo].[udp_RegisterGiveCoupons]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		joan
-- Create date: 2017-9-28
-- Description:	注册送彩券(临时)
-- =============================================
CREATE PROCEDURE [dbo].[udp_RegisterGiveCoupons]
	@UserID	BIGINT,             --用户编号
	@ActivityID INT,            --活动编号
	@Amount BIGINT,             --赠送金额
	@LotteryCode INT,           --红包使用范围
	@Day INT                    --有效期多少天
AS
BEGIN
	SET XACT_ABORT ON
	BEGIN TRAN
	BEGIN TRY
	DECLARE @Time DATETIME = GETDATE(),@ActivityCount INT = 0,@CouponsID BIGINT;
	SELECT @ActivityCount = COUNT(1) FROM CaileGame.dbo.CT_Activity WHERE StartTime <= @Time AND EndTime >= @Time ;
	IF @Day = 0
		BEGIN 
			SET @Day = 3;
		END
	IF @ActivityCount > 0
		BEGIN 
			--生成彩券
			INSERT INTO dbo.CT_Coupons
			        ( ActivityID ,GenerateTime ,ReleaseTime ,FirstTime ,LastTime ,CouponsStatus ,CouponsType ,StartTime ,ExpireTime ,FaceValue ,
					  Balance ,SatisfiedMoney ,IsGive ,LotteryCode ,IsChaseTask ,IsSuperposition ,IsTimes ,IsJoinBuy ,UserID ,CouponsSource)
			VALUES  ( @ActivityID ,@Time ,@Time ,NULL ,NULL ,1 ,1 ,@Time ,DATEADD(DAY,@Day,@Time) ,@Amount ,
			          @Amount ,0 ,0 ,@LotteryCode ,0 ,0 ,0 ,0 ,@UserID ,1)
			SELECT @CouponsID = @@IDENTITY

			--领取彩券记录
			INSERT INTO dbo.CT_CouponsRecord( CouponsID ,LogType ,CreateTime ,UserID ,RelationID ,Amount)
			VALUES  ( @CouponsID ,1 ,@Time ,@UserID ,CONVERT(VARCHAR(32),@UserID) ,@Amount)
		END
	ELSE
		BEGIN
			SELECT -2 ;
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
/****** Object:  StoredProcedure [dbo].[udp_ReportCDKey]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Jaon
-- Create date: 2017-9-27
-- Description:	兑换码报表查询
-- =============================================
CREATE PROCEDURE [dbo].[udp_ReportCDKey]
	@PartnerCode VARCHAR(20),      --合作商代码
	@TimeType INT,				   --时间段查询类型  1生成时间 2失效时间 3兑换时间
	@StartTime DATETIME,           --开始时间
	@EndTime DATETIME              --结束时间
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @DataMain NVARCHAR(4000);
	DECLARE @DataSql NVARCHAR(1000),@DataWhere NVARCHAR(1000),@DataSqlList NVARCHAR(2000);

	SET @DataSql = 'DECLARE @CDKeyTab TABLE(CDKeyID bigint,PartnerCode varchar(20),GenerateTime DATE,ExpireTime DATE,ExchangerTime DATE,IsExchanger BIT)
					INSERT INTO @CDKeyTab( CDKeyID ,PartnerCode ,GenerateTime ,ExpireTime ,ExchangerTime ,IsExchanger)
					SELECT CDKeyID,PartnerCode,CONVERT(DATE,GenerateTime) AS GenerateTime,CONVERT(DATE,ExpireTime) AS ExpireTime,CONVERT(DATE,ExchangerTime) AS ExchangerTime,IsExchanger 
					FROM dbo.CT_CouponsCDKey';

	SET @DataWhere= ' WHERE PartnerCode = ''' + @PartnerCode + '''';
	IF @TimeType = 1
		BEGIN
			SET @DataWhere = @DataWhere + ' AND GenerateTime >= ''' + CONVERT(VARCHAR(20),@StartTime) + ''' AND GenerateTime <= ''' +  CONVERT(VARCHAR(20),@EndTime) + '''';
		END
	ELSE IF @TimeType = 2
		BEGIN
			SET @DataWhere = @DataWhere + ' AND ExpireTime >= ''' + CONVERT(VARCHAR(20),@StartTime) + ''' AND ExpireTime <= ''' +  CONVERT(VARCHAR(20),@EndTime) + '''';
		END
	ELSE IF @TimeType = 3
		BEGIN
			SET @DataWhere = @DataWhere + ' AND ExchangerTime >= ''' + CONVERT(VARCHAR(20),@StartTime) + ''' AND ExchangerTime <= ''' +  CONVERT(VARCHAR(20),@EndTime) + '''';
		END

	SET @DataSqlList = '; WITH tabs
							AS
							(
								--生成时间
								SELECT GenerateTime AS DTime,COUNT(CDKeyID) AS GenerateCount,0 AS ExpireCount,0 AS ExchangerCount  FROM @CDKeyTab
								GROUP BY GenerateTime
								UNION
								--过期时间
								SELECT ExpireTime AS DTime,0 AS GenerateCount,COUNT(CDKeyID) AS ExpireCount,0 AS ExchangerCount  FROM @CDKeyTab
								GROUP BY ExpireTime
								UNION
								--兑换条数
								SELECT ExchangerTime AS DTime,0 AS GenerateCount,0 AS ExpireCount,COUNT(CDKeyID) AS ExchangerCount  FROM @CDKeyTab WHERE IsExchanger=1
								GROUP BY ExchangerTime

							)
							SELECT DTime,SUM(GenerateCount) AS GenerateCount,SUM(ExpireCount) AS ExpireCount,SUM(ExchangerCount) AS ExchangerCount FROM tabs
							GROUP BY tabs.DTime';
	SET @DataMain = @DataSql + @DataWhere + @DataSqlList ;
	PRINT @DataMain;
	EXEC sp_executesql @DataMain
END


GO
/****** Object:  StoredProcedure [dbo].[udp_ReportCoupons]    Script Date: 2018/1/18 13:59:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		joan
-- Create date: 2017-9-27
-- Description:	彩券报表查询
-- =============================================
CREATE PROCEDURE [dbo].[udp_ReportCoupons]
	@TimeType INT,				   --时间段查询类型  1生成时间 2过期时间
	@StartTime DATETIME,           --开始时间
	@EndTime DATETIME              --结束时间
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @DataMain NVARCHAR(4000);
	DECLARE @DataSql NVARCHAR(1000),@DataWhere NVARCHAR(500),@DataSqlList NVARCHAR(3500);
	--发放数，发放金额，过期数，使用金额，生成固定时段彩券，生成固定时长彩券，生成满减彩券，生成永不过期彩券
	SET @DataSql = ' DECLARE @RedTable TABLE(CouponsID BIGINT,GenerateTime DATE,ReleaseTime DATE,ExpireTime DATE,Amount BIGINT,LogType INT,FaceValue BIGINT,Balance BIGINT,CreateTime DATE,CouponsType INT)
					 INSERT INTO @RedTable( CouponsID ,GenerateTime ,ReleaseTime ,ExpireTime ,Amount ,LogType,FaceValue,Balance,CreateTime,CouponsType)
					 SELECT r.CouponsID,CONVERT(DATE,r.GenerateTime),CONVERT(DATE,r.ReleaseTime),CONVERT(DATE,r.ExpireTime),l.Amount,l.LogType,
					 r.FaceValue,r.Balance,CONVERT(DATE,l.CreateTime),r.CouponsType
					 FROM dbo.CT_Coupons AS r
					 LEFT JOIN dbo.CT_CouponsRecord AS l ON l.CouponsID = r.CouponsID AND l.LogType = 0
					 AND CONVERT(DATE,l.CreateTime) >= CONVERT(DATE,''' + CONVERT(VARCHAR(20),@StartTime) + ''') AND CONVERT(DATE,l.CreateTime) <= CONVERT(DATE,''' + CONVERT(VARCHAR(20),@EndTime) + ''') ';
	SET @DataWhere= ' WHERE ';
	IF @TimeType = 1
		BEGIN
			SET @DataWhere = @DataWhere + ' CONVERT(DATE,r.GenerateTime) >= CONVERT(DATE,''' + CONVERT(VARCHAR(20),@StartTime) + ''') AND CONVERT(DATE,r.GenerateTime) <= CONVERT(DATE,''' + CONVERT(VARCHAR(20),@EndTime) + ''') ';;
		END
	ELSE IF @TimeType = 2
		BEGIN
			SET @DataWhere = @DataWhere + ' CONVERT(DATE,r.ExpireTime) >= CONVERT(DATE,''' + CONVERT(VARCHAR(20),@StartTime) + ''') AND CONVERT(DATE,r.ExpireTime) <= CONVERT(DATE,''' + CONVERT(VARCHAR(20),@EndTime) + ''') ';;
		END

	SET @DataSqlList = '; WITH t
							AS
							(	SELECT ReleaseTime AS DTime,COUNT(CouponsID) AS ReleaseCount,SUM(FaceValue) AS ReleaseMoney,0 AS ExpireCount,0 AS ExpireAmount,0 AS Amount
								,0 AS GDTimeCount,0 AS GDTimeAmount,0 AS GDCount,0 AS GDAmount,0 AS MJCount,0 AS MJAmount
								,0 AS NoExpireCount,0 AS NoExpireAmount
								FROM @RedTable WHERE ReleaseTime IS NOT NULL GROUP BY ReleaseTime
								UNION
								SELECT ExpireTime AS DTime,0 AS ReleaseCount,0 AS ReleaseMoney,COUNT(CouponsID) AS ExpireCount ,SUM(Balance) AS ExpireAmount,0 AS Amount
								,0 AS GDTimeCount,0 AS GDTimeAmount,0 AS GDCount,0 AS GDAmount,0 AS MJCount,0 AS MJAmount
								,0 AS NoExpireCount,0 AS NoExpireAmount
								FROM @RedTable WHERE ExpireTime IS NOT NULL GROUP BY ExpireTime
								UNION
								SELECT CreateTime AS DTime,0 AS ReleaseCount,0 AS ReleaseMoney,0 AS ExpireCount , 0 AS ExpireAmount,SUM(Amount) AS Amount
								,0 AS GDTimeCount,0 AS GDTimeAmount,0 AS GDCount,0 AS GDAmount,0 AS MJCount,0 AS MJAmount
								,0 AS NoExpireCount,0 AS NoExpireAmount
								FROM @RedTable WHERE CreateTime IS NOT NULL GROUP BY CreateTime
								UNION
								SELECT GenerateTime AS DTime,0 AS ReleaseCount,0 AS ReleaseMoney,0 AS ExpireCount , 0 AS ExpireAmount,0 AS Amount
								,COUNT(CouponsID) AS GDTimeCount,SUM(FaceValue) AS GDTimeAmount,0 AS GDCount,0 AS GDAmount,0 AS MJCount,0 AS MJAmount
								,0 AS NoExpireCount,0 AS NoExpireAmount
								FROM @RedTable WHERE GenerateTime IS NOT NULL AND CouponsType = 0 GROUP BY GenerateTime
								UNION
								SELECT GenerateTime AS DTime,0 AS ReleaseCount,0 AS ReleaseMoney,0 AS ExpireCount , 0 AS ExpireAmount,0 AS Amount
								,0 AS GDTimeCount,0 AS GDTimeAmount,COUNT(CouponsID) AS GDCount,SUM(FaceValue) AS GDAmount,0 AS MJCount,0 AS MJAmount
								,0 AS NoExpireCount,0 AS NoExpireAmount
								FROM @RedTable WHERE GenerateTime IS NOT NULL AND CouponsType = 1 GROUP BY GenerateTime
								UNION
								SELECT GenerateTime AS DTime,0 AS ReleaseCount,0 AS ReleaseMoney,0 AS ExpireCount , 0 AS ExpireAmount,0 AS Amount
								,0 AS GDTimeCount,0 AS GDTimeAmount,0 AS GDCount,0 AS GDAmount,COUNT(CouponsID) AS MJCount,SUM(FaceValue) AS MJAmount
								,0 AS NoExpireCount,0 AS NoExpireAmount
								FROM @RedTable WHERE GenerateTime IS NOT NULL AND CouponsType = 2 GROUP BY GenerateTime
								UNION
								SELECT GenerateTime AS DTime,0 AS ReleaseCount,0 AS ReleaseMoney,0 AS ExpireCount , 0 AS ExpireAmount,0 AS Amount
								,0 AS GDTimeCount,0 AS GDTimeAmount,0 AS GDCount,0 AS GDAmount,0 AS MJCount,0 AS MJAmount
								,COUNT(CouponsID) AS NoExpireCount,SUM(FaceValue) AS NoExpireAmount
								FROM @RedTable WHERE GenerateTime IS NOT NULL AND CouponsType = 3 GROUP BY GenerateTime
							)SELECT * FROM t
							GROUP BY t.DTime,t.ReleaseCount,t.ReleaseMoney,t.ExpireCount,t.ExpireAmount,t.Amount,t.GDTimeCount
							,t.GDTimeAmount,t.GDCount,t.GDAmount,t.MJCount,t.MJAmount,t.NoExpireCount,t.NoExpireAmount ';
	
	SET @DataMain = @DataSql + @DataWhere + @DataSqlList ;
	PRINT LEN(@DataMain);
	PRINT @DataMain;
	EXEC sp_executesql @DataMain

END


GO
