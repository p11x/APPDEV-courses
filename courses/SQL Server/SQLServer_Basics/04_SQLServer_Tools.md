# SQL Server Tools

## Overview

To work with SQL Server, you need tools to connect, query, and manage databases. Here are the most common tools:

## 1. SQL Server Management Studio (SSMS)

### What is SSMS?
SSMS is the primary graphical tool for SQL Server administration and development.

### Features:
- Create and manage databases
- Write and execute SQL queries
- Manage tables, views, stored procedures
- Backup and restore databases
- User management and security
- Visual query builder

### When to use:
- Full database administration
- Complex queries
- Visual debugging

### Download:
- Free from Microsoft
- Works with all SQL Server versions

## 2. Azure Data Studio

### What is Azure Data Studio?
A modern, lightweight cross-platform tool for working with SQL Server.

### Features:
- Intellisense code editor
- Customizable dashboards
- Notebook support (for documentation)
- Extension support
- Lightweight and fast

### When to use:
- Cross-platform (Windows, Mac, Linux)
- Modern interface preference
- Quick data exploration

### Download:
- Free from Microsoft
- Open source

## 3. Command Line Tools

### sqlcmd
The command-line utility for executing SQL statements.

```bash
# Connect to local SQL Server
sqlcmd -S localhost -U sa -P password

# Execute a query
sqlcmd -S localhost -d DatabaseName -Q "SELECT * FROM Students"
```

### bcp (Bulk Copy Program)
For bulk data import/export.

```bash
# Export data to file
bcp DatabaseName.dbo.Students out students.txt -c -S localhost -U sa -P password
```

### PowerShell with SqlServer module
Modern scripting for SQL Server administration.

```powershell
# Get SQL Server info
Get-SqlInstance

# Execute query
Invoke-Sqlcmd -ServerInstance "localhost" -Database "CollegeDB" -Query "SELECT * FROM Students"
```

## Tool Comparison

| Feature | SSMS | Azure Data Studio | Command Line |
|---------|------|-------------------|--------------|
| Platform | Windows | All platforms | All platforms |
| GUI | Full | Modern | None |
| Learning Curve | Steeper | Easier | Steep |
| Speed | Medium | Fast | Fast |
| Best For | Admin tasks | Development | Automation |

## Recommendation for Beginners

- **Start with SSMS**: Most features, best for learning
- **Move to Azure Data Studio**: Once comfortable
- **Learn sqlcmd**: For automation later

## Key Takeaways

- **SSMS**: Primary tool for Windows users
- **Azure Data Studio**: Modern, cross-platform option
- **Command Line**: For automation and scripting

---

*This topic should take about 5 minutes to explain in class.*
