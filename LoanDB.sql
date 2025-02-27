USE [Version3_Loan]
GO
/****** Object:  Table [dbo].[CustomersInLoan]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomersInLoan](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[Address] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IncomeRangeRate]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncomeRangeRate](
	[IncomeRangeID] [int] IDENTITY(1,1) NOT NULL,
	[IncomeRange] [nvarchar](50) NULL,
	[AdjustmentRate] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[IncomeRangeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanApplications]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanApplications](
	[LoanApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[LoanProductID] [int] NOT NULL,
	[ApplicationDate] [date] NOT NULL,
	[LoanAmount] [decimal](18, 2) NOT NULL,
	[LoanStatus] [nvarchar](50) NOT NULL,
	[EconomicProof] [nvarchar](255) NULL,
	[DisbursementAccount] [nvarchar](255) NULL,
	[RepaymentMonths] [int] NULL,
	[TotalRepaymentAmount] [decimal](18, 2) NULL,
	[RepaymentAccountID] [int] NULL,
	[InterestRate] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[LoanApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanProducts]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanProducts](
	[LoanProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[InterestRate] [decimal](5, 2) NOT NULL,
	[LoanTerm] [int] NOT NULL,
	[MaxLoanAmount] [decimal](18, 2) NOT NULL,
	[MinLoanAmount] [decimal](18, 2) NOT NULL,
	[ProductDescription] [nvarchar](255) NULL,
	[ImageFileName] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[LoanProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OccupationGroupRate]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OccupationGroupRate](
	[OccupationGroupRateID] [int] IDENTITY(1,1) NOT NULL,
	[OccupationGroup] [nvarchar](100) NULL,
	[SubGroup] [nvarchar](100) NULL,
	[AdjustmentRate] [decimal](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[OccupationGroupRateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RepaymentAccounts]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RepaymentAccounts](
	[RepaymentAccountID] [int] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [varchar](50) NOT NULL,
	[TotalRepaymentAmount] [decimal](18, 2) NOT NULL,
	[AmountPaid] [decimal](18, 2) NOT NULL,
	[RemainingAmount]  AS ([TotalRepaymentAmount]-[AmountPaid]) PERSISTED,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RepaymentAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RepaymentSchedules]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RepaymentSchedules](
	[RepaymentScheduleID] [int] IDENTITY(1,1) NOT NULL,
	[LoanApplicationID] [int] NOT NULL,
	[RepaymentDate] [date] NOT NULL,
	[RepaymentAmount] [decimal](18, 2) NOT NULL,
	[RepaymentStatus] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RepaymentScheduleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionLogs]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionLogs](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[RepaymentAccountID] [int] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[TransactionType] [varchar](50) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TransactionLogs] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RepaymentAccounts] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[LoanApplications]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[CustomersInLoan] ([CustomerID])
GO
ALTER TABLE [dbo].[LoanApplications]  WITH CHECK ADD FOREIGN KEY([LoanProductID])
REFERENCES [dbo].[LoanProducts] ([LoanProductID])
GO
ALTER TABLE [dbo].[LoanApplications]  WITH CHECK ADD  CONSTRAINT [FK_LoanApplications_RepaymentAccounts] FOREIGN KEY([RepaymentAccountID])
REFERENCES [dbo].[RepaymentAccounts] ([RepaymentAccountID])
GO
ALTER TABLE [dbo].[LoanApplications] CHECK CONSTRAINT [FK_LoanApplications_RepaymentAccounts]
GO
ALTER TABLE [dbo].[RepaymentSchedules]  WITH CHECK ADD FOREIGN KEY([LoanApplicationID])
REFERENCES [dbo].[LoanApplications] ([LoanApplicationID])
GO
ALTER TABLE [dbo].[TransactionLogs]  WITH CHECK ADD  CONSTRAINT [FK_TransactionLogs_RepaymentAccounts] FOREIGN KEY([RepaymentAccountID])
REFERENCES [dbo].[RepaymentAccounts] ([RepaymentAccountID])
GO
ALTER TABLE [dbo].[TransactionLogs] CHECK CONSTRAINT [FK_TransactionLogs_RepaymentAccounts]
GO
/****** Object:  StoredProcedure [dbo].[CalculateRepaymentStatus]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CalculateRepaymentStatus]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        rs.RepaymentScheduleID,
        rs.LoanApplicationID,
        rs.RepaymentDate,
        rs.RepaymentAmount,
        rs.RepaymentStatus,
        CASE 
            WHEN EXISTS (
                SELECT 1
                FROM (
                    SELECT 
                        t.RepaymentAccountID,
                        CONCAT(YEAR(t.TransactionDate), '-', MONTH(t.TransactionDate)) AS YearMonth,
                        SUM(t.Amount) AS TotalTransactionAmount
                    FROM 
                        TransactionLogs t
                    WHERE 
                        t.TransactionDate < rs.RepaymentDate -- 交易日期在還款日期之前
                        AND t.RepaymentAccountID = (SELECT RepaymentAccountID FROM LoanApplications WHERE LoanApplicationID = rs.LoanApplicationID)
                    GROUP BY 
                        t.RepaymentAccountID,
                        CONCAT(YEAR(t.TransactionDate), '-', MONTH(t.TransactionDate))
                ) AS Subquery
                WHERE 
                    Subquery.RepaymentAccountID = (SELECT RepaymentAccountID FROM LoanApplications WHERE LoanApplicationID = rs.LoanApplicationID)
                    AND Subquery.YearMonth = CONCAT(YEAR(rs.RepaymentDate), '-', MONTH(rs.RepaymentDate))
                    AND Subquery.TotalTransactionAmount >= rs.RepaymentAmount -- 總交易金額足夠支付還款金額
            ) THEN '已結清'
            ELSE '尚欠 '
                + CAST(rs.RepaymentAmount - COALESCE((
                    SELECT SUM(t.Amount)
                    FROM TransactionLogs t
                    WHERE t.RepaymentAccountID = (SELECT RepaymentAccountID FROM LoanApplications WHERE LoanApplicationID = rs.LoanApplicationID)
                    AND YEAR(t.TransactionDate) = YEAR(rs.RepaymentDate)
                    AND MONTH(t.TransactionDate) = MONTH(rs.RepaymentDate)
                    AND t.TransactionDate < rs.RepaymentDate
                ), 0) AS VARCHAR(20)) 
                + ' 元'
        END AS RepaymentStatusWithTransaction
    FROM 
        RepaymentSchedules rs
    GROUP BY 
        rs.RepaymentScheduleID,
        rs.LoanApplicationID,
        rs.RepaymentDate,
        rs.RepaymentAmount,
        rs.RepaymentStatus;
END;
GO
/****** Object:  StoredProcedure [dbo].[CheckRepaymentStatus]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckRepaymentStatus]
AS
BEGIN
    -- 檢查並刪除臨時表（如果已存在）
    IF OBJECT_ID('tempdb..#RepaymentStatus') IS NOT NULL
        DROP TABLE #RepaymentStatus;

    -- 創建臨時表來存儲檢查結果
    CREATE TABLE #RepaymentStatus (
        LoanApplicationID INT,
        RepaymentScheduleID INT,
        RepaymentDate DATE,
        RequiredAmount DECIMAL(18, 2),
        TotalTransactionAmount DECIMAL(18, 2),
        IsSatisfied BIT
    );

    -- 計算每個還款計劃期限前的交易總額並檢查是否滿足當期還款金額
    INSERT INTO #RepaymentStatus (LoanApplicationID, RepaymentScheduleID, RepaymentDate, RequiredAmount, TotalTransactionAmount, IsSatisfied)
    SELECT
        rs.LoanApplicationID,
        rs.RepaymentScheduleID,
        rs.RepaymentDate,
        rs.RepaymentAmount AS RequiredAmount,
        COALESCE(SUM(t.Amount), 0) AS TotalTransactionAmount,
        CASE WHEN COALESCE(SUM(t.Amount), 0) >= rs.RepaymentAmount THEN 1 ELSE 0 END AS IsSatisfied
    FROM
        RepaymentSchedules rs
        LEFT JOIN TransactionLogs t ON t.RepaymentAccountID = (SELECT RepaymentAccountID FROM LoanApplications WHERE LoanApplicationID = rs.LoanApplicationID)
            AND t.TransactionDate <= rs.RepaymentDate
    GROUP BY
        rs.LoanApplicationID,
        rs.RepaymentScheduleID,
        rs.RepaymentDate,
        rs.RepaymentAmount;

    -- 查看結果
    SELECT * FROM #RepaymentStatus;

    -- 如果需要，可以將結果存儲在永久表中
    -- INSERT INTO PermanentTable (LoanApplicationID, RepaymentScheduleID, RepaymentDate, RequiredAmount, TotalTransactionAmount, IsSatisfied)
    -- SELECT LoanApplicationID, RepaymentScheduleID, RepaymentDate, RequiredAmount, TotalTransactionAmount, IsSatisfied
    -- FROM #RepaymentStatus;
END;
GO
/****** Object:  StoredProcedure [dbo].[GenerateRepaymentSchedules]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GenerateRepaymentSchedules]
    @LoanApplicationID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LoanAmount DECIMAL(18, 2)
    DECLARE @RepaymentMonths INT
    DECLARE @InterestRate DECIMAL(18, 2)
    DECLARE @MonthlyRepaymentAmount DECIMAL(18, 2)
    DECLARE @TotalRepaymentAmount DECIMAL(18, 2)
    DECLARE @RepaymentDate DATE
    DECLARE @CurrentMonth INT = 1

    -- 獲取貸款申請的金額、還款月數和利率
    SELECT @LoanAmount = LoanAmount, 
           @RepaymentMonths = RepaymentMonths, 
           @InterestRate = InterestRate
    FROM LoanApplications
    WHERE LoanApplicationID = @LoanApplicationID

    -- 計算月利率
    DECLARE @MonthlyInterestRate DECIMAL(18, 10) = @InterestRate / 12 / 100

    -- 計算每月應還款金額
    SET @MonthlyRepaymentAmount = ROUND(@LoanAmount * @MonthlyInterestRate * POWER(1 + @MonthlyInterestRate, @RepaymentMonths) / (POWER(1 + @MonthlyInterestRate, @RepaymentMonths) - 1), 0)

    -- 計算總還款金額
    SET @TotalRepaymentAmount = @MonthlyRepaymentAmount * @RepaymentMonths

    -- 調整第一期還款金額，保證總還款金額是整數
    DECLARE @FirstMonthRepayment DECIMAL(18, 2)
    SET @FirstMonthRepayment = @MonthlyRepaymentAmount + (@TotalRepaymentAmount - @MonthlyRepaymentAmount * @RepaymentMonths)

    -- 獲取當前日期，避免在循環中多次獲取
    DECLARE @StartDate DATE = GETDATE()

    -- 生成還款計畫
    WHILE @CurrentMonth <= @RepaymentMonths
    BEGIN
        SET @RepaymentDate = DATEADD(MONTH, @CurrentMonth, @StartDate)

        -- 插入每月的還款計劃
        IF @CurrentMonth = 1
        BEGIN
            INSERT INTO RepaymentSchedules (LoanApplicationID, RepaymentDate, RepaymentAmount, RepaymentStatus)
            VALUES (@LoanApplicationID, @RepaymentDate, @FirstMonthRepayment, 'Pending')
        END
        ELSE
        BEGIN
            INSERT INTO RepaymentSchedules (LoanApplicationID, RepaymentDate, RepaymentAmount, RepaymentStatus)
            VALUES (@LoanApplicationID, @RepaymentDate, @MonthlyRepaymentAmount, 'Pending')
        END

        SET @CurrentMonth = @CurrentMonth + 1
    END

    -- 更新 LoanApplications 表中的 TotalRepaymentAmount
    UPDATE LoanApplications
    SET TotalRepaymentAmount = @TotalRepaymentAmount
    WHERE LoanApplicationID = @LoanApplicationID
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateRepaymentStatusForCurrentMonth]    Script Date: 2024/7/22 下午 02:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateRepaymentStatusForCurrentMonth]
AS
BEGIN
    -- 更新 RepaymentSchedules 表中的 RepaymentStatus 列
    UPDATE rs
    SET rs.RepaymentStatus = CASE 
        WHEN EXISTS (
            SELECT 1
            FROM (
                SELECT 
                    t.RepaymentAccountID,
                    CONCAT(YEAR(t.TransactionDate), '-', MONTH(t.TransactionDate)) AS YearMonth,
                    SUM(t.Amount) AS TotalTransactionAmount
                FROM 
                    TransactionLogs t
                WHERE 
                    t.TransactionDate < rs.RepaymentDate -- 交易日期在還款日期之前
                    AND t.RepaymentAccountID = (SELECT RepaymentAccountID FROM LoanApplications WHERE LoanApplicationID = rs.LoanApplicationID)
                GROUP BY 
                    t.RepaymentAccountID,
                    CONCAT(YEAR(t.TransactionDate), '-', MONTH(t.TransactionDate))
            ) AS Subquery
            WHERE 
                Subquery.RepaymentAccountID = (SELECT RepaymentAccountID FROM LoanApplications WHERE LoanApplicationID = rs.LoanApplicationID)
                AND Subquery.YearMonth = CONCAT(YEAR(rs.RepaymentDate), '-', MONTH(rs.RepaymentDate))
                AND Subquery.TotalTransactionAmount >= rs.RepaymentAmount -- 總交易金額足夠支付還款金額
        ) THEN 'Paid'
        ELSE '尚欠 ' 
            + FORMAT(rs.RepaymentAmount - COALESCE((
                SELECT SUM(t.Amount)
                FROM TransactionLogs t
                WHERE t.RepaymentAccountID = (SELECT RepaymentAccountID FROM LoanApplications WHERE LoanApplicationID = rs.LoanApplicationID)
                AND YEAR(t.TransactionDate) = YEAR(rs.RepaymentDate)
                AND MONTH(t.TransactionDate) = MONTH(rs.RepaymentDate)
                AND t.TransactionDate < rs.RepaymentDate
            ), 0), 'N0') -- 使用 'N0' 格式化為不含小數點的千分位數字
            + ' 元'
    END
    FROM RepaymentSchedules rs
    WHERE 
        YEAR(rs.RepaymentDate) = YEAR(GETDATE()) -- 本年度
        AND MONTH(rs.RepaymentDate) = MONTH(GETDATE()); -- 本月份
END
GO
