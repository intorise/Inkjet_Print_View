using System;
using System.Collections.Generic;
using PR_SPC;

namespace PR_DAL
{
    public static class DbSchemaInitializer
    {
        private static readonly object _initLock = new object();
        private static bool _hasChecked;

        public static string EnsureSizeDvColumns()
        {
            lock (_initLock)
            {
                if (_hasChecked)
                {
                    return "数据库结构检查: 已执行过，跳过重复检查";
                }

                _hasChecked = true;
            }

            var messages = new List<string>();
            messages.Add(EnsureColumnExists("SizeDV50"));
            messages.Add(EnsureColumnExists("SizeDV90"));
            return string.Join("; ", messages);
        }

        private static string EnsureColumnExists(string columnName)
        {
            try
            {
                const string existsSql = @"SELECT COUNT(*)
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE()
  AND TABLE_NAME = 'test_data'
  AND COLUMN_NAME = @ColumnName";

                int exists = Convert.ToInt32(DapperHelper.ExecuteScalar(existsSql, new { ColumnName = columnName }));
                if (exists > 0)
                {
                    return $"数据库结构检查: test_data.{columnName} 已存在";
                }

                string alterSql = $"ALTER TABLE test_data ADD COLUMN {columnName} DOUBLE DEFAULT 0";
                DapperHelper.Execute(alterSql);
                return $"数据库结构检查: 已自动添加 test_data.{columnName}";
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrLog($"数据库结构检查失败: {columnName} -> {ex.Message}\r\n{ex.StackTrace}");
                return $"数据库结构检查失败: {columnName} -> {ex.Message}";
            }
        }
    }
}
