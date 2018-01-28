using Bootcamp.Payroll.Simulator.Classes;
using Bootcamp.Payroll.Simulator.ClientClasses;
using Bootcamp.Payroll.Simulator.Enums;
using Bootcamp.Payroll.Simulator.Exceptions;
using Bootcamp.Payroll.Simulator.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bootcamp.Payroll.Simulator.Services
{
    public class DBService
    {
        private string connString;
        private ILogger logger;
        private IOptions<SimAppSettings> appSettings;
        public DBService(ILoggerFactory loggerFactory, IOptions<SimAppSettings> appSettings)
        {
            this.connString = appSettings.Value.DBConnectionString;
            this.appSettings = appSettings;
            this.logger = loggerFactory.CreateLogger<DBService>();
        }

        private SqlConnection getConnection()
        {
            return new SqlConnection(this.connString);
        }

        private bool isConnectionValid(SqlConnection conn)
        {
            try
            {
                using (var command = new SqlCommand())
                {
                    command.CommandText = @"

IF(OBJECT_ID('BankDetails') IS NULL)
BEGIN

CREATE TABLE BankDetails(
	AccountNum NVARCHAR(16), 
	AccountBalance DECIMAL(18,2), 
	BankCode INT, 
	AccountStatus INT
)
END

IF(OBJECT_ID('CardDetails') IS NULL)
BEGIN
CREATE TABLE CardDetails(
	CardNumber NVARCHAR(20), 
	AccountBalance DECIMAL (18, 2), 
	BankCode INT, 
	CurrencyCode INT, 
	CardStatus INT, 
    CardClassification INT
)
END


IF(OBJECT_ID('Employee') IS NULL)
BEGIN
CREATE TABLE Employee(
	ID NVARCHAR(100), 
	IDNumber NVARCHAR(20), 
	FirstName NVARCHAR(50), 
	LastName NVARCHAR(50), 
	AccountNumber NVARCHAR(50), 
	BankCode INT, 
	BusinessUnit NVARCHAR(50), 
	Position NVARCHAR(50), 
	Salary DECIMAL(18, 2), 
	CardNumber NVARCHAR(50), 
	PayFrequency INT, 
	PayDate INT, 
	PaymentType INT, 
	EmployeeStatus INT, 
	SyncDate DATETIME
)
END
";
                    command.CommandType = CommandType.Text;
                    command.Connection = conn;
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        #region Bank Details
        public List<BankAccDetail> GetBankDetails(string whereClause = "")
        {
            List<BankAccDetail> bankAccounts = new List<BankAccDetail>();
            var query = "SELECT * FROM [dbo].[BankDetails]";
            if (!whereClause.IsNullOrEmpty())
                query += $" {whereClause}";

            using (var conn = getConnection())
            {
                if (isConnectionValid(conn))
                {
                    try
                    {
                        using (var command = new SqlCommand())
                        {
                            command.CommandText = query;
                            command.CommandType = CommandType.Text;
                            command.Connection = conn;
                            conn.Open();

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {

                                    var accNum = reader[0].ToString();
                                    var balance = reader[1].ToString();
                                    var bCode = reader[2].ToString();
                                    var accStatus = reader[3].ToString();

                                    double castBalance;

                                    if (!double.TryParse(balance, out castBalance))
                                    {
                                        throw new ExceptionLogger(this.logger, $"Could not parse balance into a double.");
                                    }

                                    var bankDet = new BankAccDetail()
                                    {
                                        AccountBalance = castBalance,
                                        AccountNum = accNum,
                                        AccStatus = (AccountStatus)accStatus.ToString().GetEnum(typeof(AccountStatus)),
                                        BankCode = (BankCode)bCode.ToString().GetEnum(typeof(BankCode))
                                    };

                                    bankAccounts.Add(bankDet);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ExceptionLogger(this.logger, "Could not load the Bank Accounts.", ex);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return bankAccounts;
        }

        public void SaveBankDetails(BankAccDetail bankDet)
        {
            var query = $@"
INSERT INTO [dbo].[BankDetails](AccountNum, AccountBalance, BankCode, AccountStatus)
VALUES
('{bankDet.AccountNum}', {bankDet.AccountBalance.ToString("F").Replace(',', '.')}, {(int)bankDet.BankCode}, {(int)bankDet.AccStatus})";

            using (var conn = getConnection())
            {
                if (isConnectionValid(conn))
                {
                    try
                    {
                        using (var command = new SqlCommand())
                        {
                            command.CommandText = query;
                            command.CommandType = CommandType.Text;
                            command.Connection = conn;
                            conn.Open();

                            command.ExecuteNonQuery();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ExceptionLogger(this.logger, "Could not sve the Bank Account.", ex);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        #endregion Bank Details
        #region Card Details
        public void SaveCardDetails(CardDetail cardDet)
        {
            var query = $@"
INSERT INTO [dbo].[CardDetails](CardNumber, AccountBalance, BankCode, CurrencyCode, CardStatus, CardClassification)
VALUES
('{cardDet.CardNumber}', {cardDet.AccountBalance.ToString("F").Replace(',', '.')}, {(int)cardDet.BankCode}, {(int)cardDet.Currency}, {(int)cardDet.CardStatus}, {(int)cardDet.CardClass})";

            using (var conn = getConnection())
            {
                if (isConnectionValid(conn))
                {
                    try
                    {
                        using (var command = new SqlCommand())
                        {
                            command.CommandText = query;
                            command.CommandType = CommandType.Text;
                            command.Connection = conn;
                            conn.Open();

                            command.ExecuteNonQuery();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ExceptionLogger(this.logger, "Could not sve the Bank Account.", ex);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public List<CardDetail> GetCardDetails(string whereClause = "")
        {
            List<CardDetail> cardAccounts = new List<CardDetail>();
            var query = "SELECT * FROM [dbo].[CardDetails]";
            if (!whereClause.IsNullOrEmpty())
                query += $" {whereClause}";

            using (var conn = getConnection())
            {
                if (isConnectionValid(conn))
                {
                    try
                    {
                        using (var command = new SqlCommand())
                        {
                            command.CommandText = query;
                            command.CommandType = CommandType.Text;
                            command.Connection = conn;
                            conn.Open();
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {

                                    var cardNum = reader[0].ToString();
                                    var balance = reader[1].ToString();
                                    var bCode = reader[2].ToString();
                                    var currencyCode = reader[2].ToString();
                                    var cardStatus = reader[4].ToString();
                                    var cardClass = reader[5].ToString();
                                    double castBalance;

                                    if (!double.TryParse(balance, out castBalance))
                                    {
                                        throw new ExceptionLogger(this.logger, $"Could not parse balance into a double.");
                                    }

                                    var cardDet = new CardDetail()
                                    {
                                        AccountBalance = castBalance,
                                        CardNumber = cardNum,
                                        CardStatus = (AccountStatus)cardStatus.GetEnum(typeof(AccountStatus)),
                                        BankCode = (BankCode)bCode.GetEnum(typeof(BankCode)), 
                                        Currency = (CurrencyEnum)currencyCode.GetEnum(typeof(CurrencyEnum)) , 
                                        CardClass = (CardClassification)cardClass.GetEnum(typeof(CardClassification))
                                    };

                                    cardAccounts.Add(cardDet);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ExceptionLogger(this.logger, "Could not load the Card Accounts.", ex);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return cardAccounts;
        }
        #endregion Card Details

        #region Client Employee
        public void SaveEmployee(ClientEmployee emp)
        {
            var query = $@"
INSERT INTO [dbo].[Employee](ID , IDNumber, FirstName, LastName, AccountNumber, BankCode, BusinessUnit, Position, Salary, CardNumber, PayFrequency, PayDate, PaymentType, EmployeeStatus, SyncDate)
VALUES
('{emp.ID}', '{emp.IdNumber}', '{emp.FirstName}', '{emp.LastName}', '{emp.AccountNumber}', {emp.Bank.BankId}, '{emp.BusinessUnit}', '{emp.Position}', {emp.Salary}, '{emp.CardNumber}'
, {(int)emp.PayFrequency}, {emp.PayDate}, {(int)emp.PaymentType}, {(int)emp.EmployeeStatus}, '{emp.SyncDate.ToString("yyyy-MM-dd HH:mm:ss")}')";

            using (var conn = getConnection())
            {
                if (isConnectionValid(conn))
                {
                    try
                    {
                        using (var command = new SqlCommand())
                        {
                            command.CommandText = query;
                            command.CommandType = CommandType.Text;
                            command.Connection = conn;
                            conn.Open();

                            command.ExecuteNonQuery();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ExceptionLogger(this.logger, "Could not save the Client Employee.", ex);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        public List<ClientEmployee> getEmployees(string whereClause)
        {
            List<ClientEmployee> employees = new List<ClientEmployee>();
            var query = "SELECT * FROM [dbo].[Employee]";
            if (!whereClause.IsNullOrEmpty())
                query += $" {whereClause}";

            using (var conn = getConnection())
            {
                if (isConnectionValid(conn))
                {
                    try
                    {
                        using (var command = new SqlCommand())
                        {
                            command.CommandText = query;
                            command.CommandType = CommandType.Text;
                            command.Connection = conn;
                            conn.Open();
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {

                                    var bCode = (BankCode)reader[5].ToString().GetEnum(typeof(BankCode));
                                    var payFreq = (PayFrequency)reader[10].ToString().GetEnum(typeof(PayFrequency));
                                    var payType = (ClientPaymentType)reader[12].ToString().GetEnum(typeof(ClientPaymentType));
                                    var empStatus = (EmployeeStatus)reader[13].ToString().GetEnum(typeof(EmployeeStatus));
                                    var salary = reader[8].ToString();
                                    double castSalary;

                                    if (!double.TryParse(salary, out castSalary))
                                    {
                                        throw new ExceptionLogger(this.logger, $"Could not parse balance into a double.");
                                    }

                                    var emp = new ClientEmployee()
                                    {
                                        ID = reader[0].ToString(),
                                        IdNumber = reader[1].ToString(),
                                        FirstName = reader[2].ToString(),
                                        LastName = reader[3].ToString(),
                                        AccountNumber = reader[4].ToString(),
                                        Bank = new ClientBank() { BankId = (int)bCode, BankName = bCode.ToString() },
                                        BusinessUnit = reader[6].ToString(), 
                                        Position = reader[7].ToString(), 
                                        Salary = castSalary, 
                                        CardNumber = reader[9].ToString(), 
                                        PayFrequency = payFreq, 
                                        PayDate = Convert.ToInt32( reader[11].ToString()), 
                                        PaymentType = payType, 
                                        EmployeeStatus = empStatus, 
                                        SyncDate = Convert.ToDateTime(reader[14])
                                    };

                                    employees.Add(emp);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ExceptionLogger(this.logger, "Could not load the Clients Employees.", ex);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return employees;
        }
        #endregion Client Employee
    }
}
