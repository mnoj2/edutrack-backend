using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EduTrack.EduTrack.Data.Data.Repositories {
    public class SqlTokenRepository : ITokenRepository {

        private readonly string _connectionString;

        public SqlTokenRepository(IConfiguration config) {
            _connectionString = config.GetConnectionString("DefaultConnection")
                                ?? throw new Exception("ConnectionString not configured");
        }

        public async Task<User?> GetUserByIdAsync(Guid userId) {

            User? user = null;
            using(var conn = new SqlConnection(_connectionString)) {
                using(var cmd = new SqlCommand("SELECT * FROM users WHERE Id = @Id", conn)) {
                    cmd.Parameters.AddWithValue("@Id", userId);

                    await conn.OpenAsync();

                    using(var reader = await cmd.ExecuteReaderAsync()) {
                        if(await reader.ReadAsync()) {
                            user = new User {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                RefreshToken = reader.IsDBNull(reader.GetOrdinal("RefreshToken")) ? null : reader.GetString(reader.GetOrdinal("RefreshToken")),
                                RefreshTokenExpiryTime = reader.IsDBNull(reader.GetOrdinal("RefreshTokenExpiryTime")) ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiryTime"))
                            };
                        }
                    }
                }
            }
            return user;
        }

        public async Task StoreRefreshTokenAsync(User user) {

            using(var conn = new SqlConnection(_connectionString)) {
                using(var cmd = new SqlCommand("sp_add_refreshtoken_details", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@RefreshToken", user.RefreshToken);
                    cmd.Parameters.AddWithValue("@RefreshTokenExpiryTime", user.RefreshTokenExpiryTime);

                    var pCode = cmd.Parameters.Add("@status_code", SqlDbType.VarChar, 1);
                    pCode.Direction = ParameterDirection.Output;
                    var pMsg = cmd.Parameters.Add("@status_msg", SqlDbType.VarChar, -1);
                    pMsg.Direction = ParameterDirection.Output;

                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    if(pCode.Value.ToString() == "F") {

                    }
                }
            }
        }
    }
}
