USE [master]
GO
/****** Object:  Database [Version3_Customer]    Script Date: 2024/7/22 下午 02:22:20 ******/
CREATE DATABASE [Version3_Customer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Version3_Customer', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Version3_Customer.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Version3_Customer_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Version3_Customer_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Version3_Customer] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Version3_Customer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Version3_Customer] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Version3_Customer] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Version3_Customer] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Version3_Customer] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Version3_Customer] SET ARITHABORT OFF 
GO
ALTER DATABASE [Version3_Customer] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Version3_Customer] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Version3_Customer] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Version3_Customer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Version3_Customer] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Version3_Customer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Version3_Customer] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Version3_Customer] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Version3_Customer] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Version3_Customer] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Version3_Customer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Version3_Customer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Version3_Customer] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Version3_Customer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Version3_Customer] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Version3_Customer] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Version3_Customer] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Version3_Customer] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Version3_Customer] SET  MULTI_USER 
GO
ALTER DATABASE [Version3_Customer] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Version3_Customer] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Version3_Customer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Version3_Customer] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Version3_Customer] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Version3_Customer] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Version3_Customer] SET QUERY_STORE = ON
GO
ALTER DATABASE [Version3_Customer] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Version3_Customer]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 2024/7/22 下午 02:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[AccountNumber] [nvarchar](20) NOT NULL,
	[AccountTypeID] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[AccountStatusId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[AccountNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountStatus]    Script Date: 2024/7/22 下午 02:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountStatus](
	[AccountStatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountTypes]    Script Date: 2024/7/22 下午 02:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountTypes](
	[AccountTypeID] [int] IDENTITY(1,1) NOT NULL,
	[AccountTypeName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Budget]    Script Date: 2024/7/22 下午 02:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Budget](
	[BudgetID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Period] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BudgetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 2024/7/22 下午 02:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 2024/7/22 下午 02:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](15) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[ZipCode] [nvarchar](10) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenses]    Script Date: 2024/7/22 下午 02:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[ExpenseID] [int] IDENTITY(1,1) NOT NULL,
	[BudgetID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[ExpenseDate] [datetime] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 2024/7/22 下午 02:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[TransactionType] [nvarchar](20) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[BalanceAfterTransaction] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[InteractiveAccountNumber] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Budget] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Expenses] ADD  DEFAULT (getdate()) FOR [ExpenseDate]
GO
ALTER TABLE [dbo].[Expenses] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Transactions] ADD  DEFAULT (getdate()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD FOREIGN KEY([AccountTypeID])
REFERENCES [dbo].[AccountTypes] ([AccountTypeID])
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_AccountStatus] FOREIGN KEY([AccountStatusId])
REFERENCES [dbo].[AccountStatus] ([AccountStatusId])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Transactions_AccountStatus]
GO
ALTER TABLE [dbo].[Budget]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Budget]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD FOREIGN KEY([BudgetID])
REFERENCES [dbo].[Budget] ([BudgetID])
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Expenses]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([AccountID])
GO
/****** Object:  StoredProcedure [dbo].[TransferFunds]    Script Date: 2024/7/22 下午 02:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TransferFunds]
    @FromAccountID INT,
    @ToAccountID INT,
    @Amount DECIMAL(18, 2),
    @Description NVARCHAR(200) = null  -- 新增描述參數
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FromAccountBalance DECIMAL(18, 2);
    DECLARE @ToAccountBalance DECIMAL(18, 2);
    DECLARE @FromAccountNumber NVARCHAR(50);
    DECLARE @ToAccountNumber NVARCHAR(50);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 獲取來源帳戶和目標帳戶的餘額和帳號
        SELECT @FromAccountBalance = Balance, @FromAccountNumber = AccountNumber FROM Accounts WHERE AccountID = @FromAccountID;
        SELECT @ToAccountBalance = Balance, @ToAccountNumber = AccountNumber FROM Accounts WHERE AccountID = @ToAccountID;

        -- 檢查來源帳戶餘額是否足夠
        IF @FromAccountBalance < @Amount
        BEGIN
            THROW 50001, 'Insufficient funds in the source account.', 1;
        END

        -- 扣除來源帳戶的金額
        UPDATE Accounts
        SET Balance = Balance - @Amount
        WHERE AccountID = @FromAccountID;

        -- 增加目標帳戶的金額
        UPDATE Accounts
        SET Balance = Balance + @Amount
        WHERE AccountID = @ToAccountID;

        -- 更新餘額
        SET @FromAccountBalance = @FromAccountBalance - @Amount;
        SET @ToAccountBalance = @ToAccountBalance + @Amount;

        -- 新增來源帳戶的交易紀錄
        INSERT INTO Transactions (AccountID, TransactionDate, TransactionType, Amount, BalanceAfterTransaction, Description, InteractiveAccountNumber)
        VALUES (@FromAccountID, GETDATE(), 'Debit', @Amount, @FromAccountBalance, @Description, @ToAccountNumber);

        -- 新增目標帳戶的交易紀錄
        INSERT INTO Transactions (AccountID, TransactionDate, TransactionType, Amount, BalanceAfterTransaction, Description, InteractiveAccountNumber)
        VALUES (@ToAccountID, GETDATE(), 'Credit', @Amount, @ToAccountBalance, @Description, @FromAccountNumber);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO
USE [master]
GO
ALTER DATABASE [Version3_Customer] SET  READ_WRITE 
GO
