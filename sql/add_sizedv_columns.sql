-- 在修改表结构前请先备份数据库
-- 示例：添加 SizeDV50 与 SizeDV90 字段到 test_data 表

ALTER TABLE test_data
ADD COLUMN SizeDV50 DOUBLE DEFAULT 0;

ALTER TABLE test_data
ADD COLUMN SizeDV90 DOUBLE DEFAULT 0;

-- 注意：根据目标数据库（MySQL/Postgres/SQLite）语法可能需要微调：
-- MySQL 示例（已适配）：上面语句适用于 MySQL
-- 执行完迁移后，请验证已有插入/更新语句是否包含新列，且应用连接用户有 ALTER 权限。