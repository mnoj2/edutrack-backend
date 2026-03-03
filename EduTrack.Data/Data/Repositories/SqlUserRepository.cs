using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EduTrack.EduTrack.Data.Data.Repositories {
    public class SqlUserRepository : IUserRepository {

        private readonly string _connectionString;

        public SqlUserRepository(IConfiguration config) {
            _connectionString = config.GetConnectionString("DefaultConnection") 
                                ?? throw new Exception("ConnectionString not configured");
        }

        public async Task AddUserAsync(User user) {

            using (var conn = new SqlConnection(_connectionString)) {
                using(var cmd = new SqlCommand("sp_add_user", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
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

        public async Task<List<User>> GetAllAsync() {

            var users = new List<User>();

            using (var conn = new SqlConnection(_connectionString)) {
                using(var cmd = new SqlCommand("sp_get_users", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var pCode = cmd.Parameters.Add("@status_code", SqlDbType.VarChar, 1);
                    pCode.Direction = ParameterDirection.Output;
                    var pMsg = cmd.Parameters.Add("@status_msg", SqlDbType.VarChar, -1);
                    pMsg.Direction = ParameterDirection.Output;

                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync()) {
                        while(await reader.ReadAsync()) {
                            users.Add(new User {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash"))
                            });
                        }
                    }

                    if (pCode.Value.ToString() == "F") {

                    }    
                }
            }

            return users;
        }

        public async Task<User?> GetByUsernameAsync(string username) {

            User? user = null;

            using(var conn = new SqlConnection(_connectionString)) {
                using(var cmd = new SqlCommand("sp_get_by_username", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Username", username);

                    var pCode = cmd.Parameters.Add("@status_code", SqlDbType.VarChar, 1);
                    pCode.Direction = ParameterDirection.Output;
                    var pMsg = cmd.Parameters.Add("@status_msg", SqlDbType.VarChar, -1);
                    pMsg.Direction = ParameterDirection.Output;

                    await conn.OpenAsync();

                    using(var reader = await cmd.ExecuteReaderAsync()) {
                        while(await reader.ReadAsync()) {
                            user = new User {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                RefreshToken = reader.IsDBNull(reader.GetOrdinal("RefreshToken"))
                                               ? null
                                                : reader.GetString(reader.GetOrdinal("RefreshToken")),
                                RefreshTokenExpiryTime = reader.IsDBNull(reader.GetOrdinal("RefreshTokenExpiryTime"))
                                               ? null
                                                : reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiryTime"))
                            };
                        }
                    }

                    if(pCode.Value.ToString() == "F") {

                    }
                }
            }

            return user;
        }
    }
}
