using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Infra
{
    public static class SqlScriptRunner
    {
        public static async Task RunScriptAsync(string connectionString, string scriptPath)
        {
            var script = await File.ReadAllTextAsync(scriptPath, Encoding.UTF8);

            // Divide por líneas "GO" (estilo SSMS)
            var batches = SplitOnGo(script);

            await using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            foreach (var batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch)) continue;

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = batch;
                cmd.CommandTimeout = 180;

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private static IEnumerable<string> SplitOnGo(string script)
        {
            var lines = script.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var sb = new System.Text.StringBuilder();

            foreach (var line in lines)
            {
                if (line.Trim().Equals("GO", StringComparison.OrdinalIgnoreCase))
                {
                    yield return sb.ToString();
                    sb.Clear();
                    continue;
                }
                sb.AppendLine(line);
            }

            if (sb.Length > 0)
                yield return sb.ToString();
        }
    }
}
