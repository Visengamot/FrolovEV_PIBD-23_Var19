using Microsoft.EntityFrameworkCore;
using Npgsql;
using ShoeStore.WinForms.Data;

namespace ShoeStore.WinForms.Repositories.Implementations;

internal static class RepositoryOperation
{
    internal static T Read<T>(Serilog.ILogger logger, string operation, Func<T> action)
    {
        try
        {
            var result = action();
            if (result is System.Collections.ICollection collection)
                logger.Information("{Operation} completed: {Count} records", operation, collection.Count);
            else
                logger.Information("{Operation} completed: found={Found}", operation, result is not null);
            return result;
        }
        catch (Exception error) { logger.Error(error, "{Operation} failed", operation); throw; }
    }

    internal static void Write(DataContext context, Serilog.ILogger logger, string operation, object input, Action action)
    {
        logger.Information("{Operation} started: {@Input}", operation, input);
        context.ChangeTracker.Clear();
        try
        {
            using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            // Every application writer uses the same transaction-scoped ledger lock.
            // PostgreSQL releases it on commit/rollback; serialization conflicts ask the user to retry.
            context.Database.ExecuteSqlRaw("SELECT pg_advisory_xact_lock(1919002)");
            action();
            transaction.Commit();
            logger.Information("{Operation} completed", operation);
        }
        catch (DbUpdateException error) when (error.InnerException is PostgresException postgres)
        {
            logger.Error(error, "{Operation} failed; SQLSTATE {SqlState}", operation, postgres.SqlState);
            throw new InvalidOperationException(postgres.SqlState switch
            {
                PostgresErrorCodes.UniqueViolation => "Запись с таким идентификатором или уникальным значением уже существует.",
                PostgresErrorCodes.ForeignKeyViolation => "Связанная запись не найдена или используется другими записями.",
                PostgresErrorCodes.CheckViolation => "Данные нарушают ограничения базы данных.",
                PostgresErrorCodes.SerializationFailure => "Данные изменены другим пользователем. Повторите операцию.",
                _ => "Не удалось сохранить данные. Подробности записаны в журнал."
            });
        }
        catch (PostgresException error) when (error.SqlState == PostgresErrorCodes.SerializationFailure || error.SqlState == PostgresErrorCodes.DeadlockDetected)
        {
            logger.Warning(error, "{Operation} conflicted with another transaction", operation);
            throw new InvalidOperationException("Данные изменены другим пользователем. Повторите операцию.");
        }
        catch (Exception error) { logger.Error(error, "{Operation} failed", operation); throw; }
        finally { context.ChangeTracker.Clear(); }
    }
}
