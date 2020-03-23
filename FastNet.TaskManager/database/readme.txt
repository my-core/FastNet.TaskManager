
quartz.net数据驱动对应关系:
	SqlServer - SQL Server driver for default
    SqlServer-20 - SQL Server driver for .NET Framework 2.0
    OracleODP-20 - Oracle's Oracle Driver
    OracleODPManaged-1123-40 Oracle's managed driver for Oracle 11
    OracleODPManaged-1211-40 Oracle's managed driver for Oracle 12
    MySql-50 - MySQL Connector/.NET v. 5.0 (.NET 2.0)
    MySql-51 - MySQL Connector/:NET v. 5.1 (.NET 2.0)
    MySql-65 - MySQL Connector/:NET v. 6.5 (.NET 2.0)
    SQLite-10 - SQLite ADO.NET 2.0 Provider v. 1.0.56 (.NET 2.0)
    Firebird-201 - Firebird ADO.NET 2.0 Provider v. 2.0.1 (.NET 2.0)
    Firebird-210 - Firebird ADO.NET 2.0 Provider v. 2.1.0 (.NET 2.0)
    Npgsql-20 - PostgreSQL Npgsql



2.X 和 3.X 的不同点如下:

1)2.X都是同步的,而3.X很多方法改成了异步;

2)线程池类型配置 :
　　2.X   quartz.threadPool.type = Quartz.Simpl.SimpleThreadPool, Quartz
　　3.X   quartz.threadPool.type = Quartz.Simpl.DefaultThreadPool, Quartz

3)序列化方式配置 :
　　2.X 不需要指定;
	3.X则需要指定  quartz.serializer.type = json  或者 quartz.serializer.type = binary

4)数据库连接配置 : 此乃深坑!!
　  3.X 很简单, 　　quartz.dataSource.myDS.provider = MySql    
	2.X　　quartz.dataSource.myDS.provider = MySql-65

　　下面的其实都可以
    MySql-50 - MySQL Connector/.NET v. 5.0 (.NET 2.0)
    MySql-51 - MySQL Connector/:NET v. 5.1 (.NET 2.0)
    MySql-65 - MySQL Connector/:NET v. 6.5 (.NET 2.0)
    MySql-69 - MySQL Connector/:NET v. 6.9 (.NET 2.0)
　　但是,MySql-69 需要  MySql.Data.dll 6.9.5 版本,小弟找了N久.硬是没找到.最后满世界找,好不容易找到了  MySql-65 需要的  6.5.4 ...