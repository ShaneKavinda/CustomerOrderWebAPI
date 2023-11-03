using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using testApi.Models;

namespace testApi.Controllers
{
    public class CustomerDataAccessLayer : Controller
    {
        private readonly string connectionString = "data source=SHANE_K_99\\SQLEXPRESS;initial catalog=Inventory;trusted_connection=true";
        
        public IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("spGetAllCustomers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                { 
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Customer customer = new Customer();

                            customer.UserID = rdr.GetGuid(rdr.GetOrdinal("UserId"));
                            customer.Username = rdr.GetString(rdr.GetOrdinal("Username"));
                            customer.Email = rdr.GetString(rdr.GetOrdinal("Email"));
                            customer.FirstName = rdr.GetString(rdr.GetOrdinal("FirstName"));
                            customer.LastName = rdr.GetString(rdr.GetOrdinal("LastName"));
                            customer.CreatedOn = rdr.GetDateTime(rdr.GetOrdinal("CreatedOn"));
                            customer.IsActive = rdr.GetBoolean(rdr.GetOrdinal("IsActive"));
                            
                            customers.Add(customer);
                        }
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    conn.Close();
                    throw;
                    
                }
                conn.Close();
                return customers;                  
            }
        }

        //check if the generated UserID is unique
        private bool IsUserIdUnique(Guid userId, SqlConnection con)
        {
            using (var command = new SqlCommand("SELECT COUNT(*) FROM Customer WHERE UserID = @UserId", con))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                int count = (int)command.ExecuteScalar();

                // If the count is 0, the UserID is unique
                return count == 0;
            }
        }

        // To Add a new Customer Record
        public void AddCustomer(Customer customer)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spInsertCustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                // Keep generating a new GUID until a unique one is found
                Guid UserID;
                do
                {
                    UserID = Guid.NewGuid();
                }
                while (!IsUserIdUnique(UserID, con));

                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Username", customer.Username);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);

                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                } catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    con.Close();
                    throw;
                }
            }
        }


        // Edit a customer record
        public void UpdateCustomer(Customer customer)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateCustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", customer.Username);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);
                cmd.Parameters.AddWithValue("@UserID", customer.UserID);
                con.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                } catch(Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    con.Close();
                    throw;
                }
            }
        }

        // Delete a Customer record
        public void DeleteCustomer(Guid id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteCustomer", con);
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", id);
                con.Open() ;
                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();

                } catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    con.Close();
                    throw;
                }
            }
        }

        //Get all active orders by a Customer
        public List<OrderWithDetails> GetActiveOrdersByCustomer(Guid customerId)
        {
            List<OrderWithDetails> orders = new List<OrderWithDetails>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("spGetActiveOrdersByCustomer", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            OrderWithDetails order = new OrderWithDetails
                            {
                                OrderId = rdr.GetGuid(rdr.GetOrdinal("OrderId")),
                                ProductId = rdr.GetGuid(rdr.GetOrdinal("ProductId")),
                                OrderStatus = rdr.GetInt32(rdr.GetOrdinal("OrderStatus")),
                                OrderType = rdr.GetInt32(rdr.GetOrdinal("OrderType")),
                                OrderedOn = rdr.GetDateTime(rdr.GetOrdinal("OrderedOn")),
                                ShippedOn = rdr.IsDBNull(rdr.GetOrdinal("ShippedOn")) ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("ShippedOn")),
                                IsActive = rdr.GetBoolean(rdr.GetOrdinal("IsActive")),
                                ProductName = rdr.GetString(rdr.GetOrdinal("ProductName")),
                                UnitPrice = rdr.GetDecimal(rdr.GetOrdinal("UnitPrice")),
                                SupplierName = rdr.GetString(rdr.GetOrdinal("SupplierName"))
                            };

                            orders.Add(order);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            return orders;
        }

    }
}
