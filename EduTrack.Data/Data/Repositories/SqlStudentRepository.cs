using EduTrack.EduTrack.Data.Data.Interfaces;
using EduTrack.EduTrack.Data.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EduTrack.EduTrack.Data.Repositories {
    public class SqlStudentRepository : IStudentRepository {

        private readonly string _connectionString;

        public SqlStudentRepository(IConfiguration config) {
            _connectionString = config.GetConnectionString("DefaultConnection")
                                ?? throw new Exception("ConnectionString not configured");
        }

        public async Task AddStudentAsync(Student student) {
            using(var conn = new SqlConnection(_connectionString)) {
                using(var cmd = new SqlCommand("sp_add_student", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", student.FullName);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@MobileNumber", student.MobileNumber);
                    cmd.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);
                    cmd.Parameters.AddWithValue("@Course", student.Course);
                    cmd.Parameters.AddWithValue("@TermsAccepted", student.TermsAccepted);

                    var pCode = cmd.Parameters.Add("@status_code", SqlDbType.VarChar, 1);
                    pCode.Direction = ParameterDirection.Output;
                    var pMsg = cmd.Parameters.Add("@status_msg", SqlDbType.VarChar, -1);
                    pMsg.Direction = ParameterDirection.Output;

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    if(pCode.Value?.ToString() == "F") {

                    }
                }
            }
        }

        public async Task DeleteStudentAsync(string email) {
            using(var conn = new SqlConnection(_connectionString)) {
                using(var cmd = new SqlCommand("sp_delete_student", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Email", email);

                    var pCode = cmd.Parameters.Add("@status_code", SqlDbType.VarChar, 1);
                    pCode.Direction = ParameterDirection.Output;
                    var pMsg = cmd.Parameters.Add("@status_msg", SqlDbType.VarChar, -1);
                    pMsg.Direction = ParameterDirection.Output;

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    if(pCode.Value?.ToString() == "F") {

                    }
                }
            }
        }

        public async Task<List<Student>> GetAllAsync() {
            var students = new List<Student>();

            using(var conn = new SqlConnection(_connectionString)) {
                using(var cmd = new SqlCommand("sp_get_students", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var pCode = cmd.Parameters.Add("@status_code", SqlDbType.VarChar, 1);
                    pCode.Direction = ParameterDirection.Output;
                    var pMsg = cmd.Parameters.Add("@status_msg", SqlDbType.VarChar, -1);
                    pMsg.Direction = ParameterDirection.Output;

                    await conn.OpenAsync();

                    using(var reader = await cmd.ExecuteReaderAsync()) {
                        while(await reader.ReadAsync()) {
                            students.Add(new Student {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                                DateOfBirth = reader.GetString(reader.GetOrdinal("DateOfBirth")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                Course = reader.GetString(reader.GetOrdinal("Course")),
                                TermsAccepted = reader.GetBoolean(reader.GetOrdinal("TermsAccepted")),
                            });
                        }
                    }

                    if(pCode.Value.ToString() == "F") {

                    }
                }
            }

            return students;
        }

        public async Task<Student?> GetByEmailAsync(string email) {
            Student? student = null;

            using(var conn = new SqlConnection(_connectionString)) {
                using(var cmd = new SqlCommand("sp_get_by_email", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Email", email);

                    var pCode = cmd.Parameters.Add("@status_code", SqlDbType.VarChar, 1);
                    pCode.Direction = ParameterDirection.Output;
                    var pMsg = cmd.Parameters.Add("@status_msg", SqlDbType.VarChar, -1);
                    pMsg.Direction = ParameterDirection.Output;

                    await conn.OpenAsync();

                    using(var reader = await cmd.ExecuteReaderAsync()) {
                        while(await reader.ReadAsync()) {
                            student = new Student {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                                DateOfBirth = reader.GetString(reader.GetOrdinal("DateOfBirth")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                Course = reader.GetString(reader.GetOrdinal("Course")),
                                TermsAccepted = reader.GetBoolean(reader.GetOrdinal("TermsAccepted")),
                            };
                        }
                    }

                    if(pCode.Value.ToString() == "F") {

                    }
                }
            }

            return student;
        }
    }
}