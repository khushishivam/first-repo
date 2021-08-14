using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace IIABackend
{
    /// <summary>
    /// Class to manange all Cosmos DB functions.
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="phoneNumber">Phone Number of the User.</param>
        /// <returns>id of the new user</returns>
        public static int CreateUserIfNotExists(string phoneNumber)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Users_CreateUserIfNotExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = phoneNumber;

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            return rdr.GetInt32(0);
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Get user.
        /// </summary>
        /// <param name="userId">userId of the User.</param>
        /// <param name="chapter">chapter of the membership.</param>
        /// <returns>id of the new user</returns>
        public static List<Membership> GetMembershipDetails(string userId, dynamic chapter)
        {
            var membershipList = new List<Membership>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Membership_GetDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@Chapter", SqlDbType.Int).Value = chapter != null ? chapter : 0;

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var membership = new Membership(rdr.GetInt32(0).ToString(), rdr.GetInt32(1).ToString(), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetDateTime(9), rdr.GetDateTime(10), rdr.GetDateTime(11));
                            membershipList.Add(membership);
                        }
                    }
                }
            }

            return membershipList;
        }

        /// <summary>
        /// Is Membership Active
        /// </summary>
        /// <param name="userId">userId of the User.</param>
        /// <returns>id of the new user</returns>
        public static Membership GetActiveMembership(string userId)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Membership_GetDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@Chapter", SqlDbType.Int).Value = 0;

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if ((DateTime.Today.Date - rdr.GetDateTime(9).Date).TotalDays >= 0)
                            {
                                return new Membership("Membership Not Active");
                            }
                            else
                            {
                                var membership = new Membership(rdr.GetInt32(0).ToString(), rdr.GetInt32(1).ToString(), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetDateTime(9), rdr.GetDateTime(10), rdr.GetDateTime(11));
                                return membership;
                            }
                        }
                    }
                }
            }

            return new Membership("Membership Details Not Found");
        }

        /// <summary>
        /// Creates a new Membership
        /// </summary>
        /// <param name="membership">membership Object</param>
        public static void InsertMembership(Membership membership)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("Membership_InsertDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = membership.UserId;
                        cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = membership.FirstName;
                        cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = membership.LastName;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = membership.Email;
                        cmd.Parameters.Add("@MembershipId", SqlDbType.VarChar).Value = membership.MembershipId != null ? membership.MembershipId : string.Empty;
                        cmd.Parameters.Add("@ProfileImagePath", SqlDbType.VarChar).Value = membership.ProfileImagePath != null ? membership.ProfileImagePath : string.Empty;
                        cmd.Parameters.Add("@Chapter", SqlDbType.Int).Value = membership.Chapter;
                        cmd.Parameters.Add("@MembershipFees", SqlDbType.Int).Value = membership.MembershipFees;
                        cmd.Parameters.Add("@MembershipExpiryDate", SqlDbType.DateTime).Value = DateTime.Today.AddDays(365);
                        cmd.Parameters.Add("@MembershipJoinDate", SqlDbType.DateTime).Value = DateTime.Today;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get user.
        /// </summary>
        /// <param name="userId">Id of the User.</param>
        /// <returns>company profile of user</returns>
        public static MembershipProfile GetMembershipProfile(string userId)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("MembershipProfile_GetDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@ProfileStatus", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@chapter", SqlDbType.Int).Value = 0;
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            MembershipProfile membershipProfile = new MembershipProfile(rdr.GetInt32(0).ToString(), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), rdr.GetValue(7).ToString(), rdr.GetString(8), rdr.GetString(9), rdr.GetString(10), rdr.GetString(11), rdr.GetString(12), rdr.GetString(13), rdr.GetString(14), rdr.GetString(15), rdr.GetString(16), rdr.GetString(17), rdr.GetString(18), rdr.GetString(19), rdr.GetString(20), rdr.GetString(21), rdr.GetDateTime(22), rdr.GetDateTime(23));
                            return membershipProfile;
                        }
                    }
                }
            }

            return new MembershipProfile("Profile Details Not Found");
        }

        /// <summary>
        /// Get user.
        /// </summary>
        /// <param name="userId">Id of the User.</param>
        /// <param name="profileStatus">ProfileStatus</param>
        /// <param name="chapter">Chapter</param>
        /// <returns>company profile of user</returns>
        public static List<MembershipProfile> GetMembershipProfile(string userId, dynamic profileStatus, dynamic chapter)
        {
            var membershipProfileList = new List<MembershipProfile>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("MembershipProfile_GetDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@ProfileStatus", SqlDbType.Int).Value = profileStatus != null ? profileStatus : 0;
                    cmd.Parameters.Add("@chapter", SqlDbType.Int).Value = chapter != null ? chapter : 0;
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            MembershipProfile membershipProfile = new MembershipProfile(rdr.GetInt32(0).ToString(), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), rdr.GetValue(7).ToString(), rdr.GetString(8), rdr.GetString(9), rdr.GetString(10), rdr.GetString(11), rdr.GetString(12), rdr.GetString(13), rdr.GetString(14), rdr.GetString(15), rdr.GetString(16), rdr.GetString(17), rdr.GetString(18), rdr.GetString(19), rdr.GetString(20), rdr.GetString(21), rdr.GetDateTime(22), rdr.GetDateTime(23));
                            membershipProfileList.Add(membershipProfile);
                        }
                    }
                }
            }

            return membershipProfileList;
        }

        /// <summary>
        /// Inserts a company profile
        /// </summary>
        /// <param name="membershipProfile">UserCompanyProfile</param>
        public static void InsertMembershipProfile(MembershipProfile membershipProfile)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("MembershipProfile_InsertDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = membershipProfile.Id;
                        cmd.Parameters.Add("@ProfileStatus", SqlDbType.Int).Value = membershipProfile.ProfileStatus;
                        cmd.Parameters.Add("@Chapter", SqlDbType.Int).Value = membershipProfile.Chapter;
                        cmd.Parameters.Add("@UnitName", SqlDbType.VarChar).Value = membershipProfile.UnitName != null ? membershipProfile.UnitName : string.Empty;
                        cmd.Parameters.Add("@GSTIN", SqlDbType.VarChar).Value = membershipProfile.GSTIN != null ? membershipProfile.GSTIN : string.Empty;
                        cmd.Parameters.Add("@GSTcertpath", SqlDbType.VarChar).Value = membershipProfile.GSTcertpath != null ? membershipProfile.GSTcertpath : string.Empty;
                        cmd.Parameters.Add("@IndustryStatus", SqlDbType.VarChar).Value = membershipProfile.IndustryStatus != null ? membershipProfile.IndustryStatus : string.Empty;
                        cmd.Parameters.Add("@Address", SqlDbType.Text).Value = membershipProfile.Address != null ? membershipProfile.Address : string.Empty;
                        cmd.Parameters.Add("@District", SqlDbType.VarChar).Value = membershipProfile.District != null ? membershipProfile.District : string.Empty;
                        cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = membershipProfile.City != null ? membershipProfile.City : string.Empty;
                        cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = membershipProfile.State != null ? membershipProfile.State : string.Empty;
                        cmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = membershipProfile.Country != null ? membershipProfile.Country : string.Empty;
                        cmd.Parameters.Add("@Pincode", SqlDbType.VarChar).Value = membershipProfile.Pincode != null ? membershipProfile.Pincode : string.Empty;
                        cmd.Parameters.Add("@WebsiteUrl", SqlDbType.VarChar).Value = membershipProfile.WebsiteUrl != null ? membershipProfile.WebsiteUrl : string.Empty;
                        cmd.Parameters.Add("@ProductCategory", SqlDbType.VarChar).Value = membershipProfile.ProductCategory != null ? membershipProfile.ProductCategory : string.Empty;
                        cmd.Parameters.Add("@ProductSubCategory", SqlDbType.VarChar).Value = membershipProfile.ProductSubCategory != null ? membershipProfile.ProductSubCategory : string.Empty;
                        cmd.Parameters.Add("@MajorProducts", SqlDbType.Text).Value = membershipProfile.MajorProducts != null ? membershipProfile.MajorProducts : string.Empty;
                        cmd.Parameters.Add("@AnnualTurnOver", SqlDbType.VarChar).Value = membershipProfile.AnnualTurnOver != null ? membershipProfile.AnnualTurnOver : string.Empty;
                        cmd.Parameters.Add("@EnterpriseType", SqlDbType.VarChar).Value = membershipProfile.EnterpriseType != null ? membershipProfile.EnterpriseType : string.Empty;
                        cmd.Parameters.Add("@Exporter", SqlDbType.VarChar).Value = membershipProfile.Exporter != null ? membershipProfile.Exporter : string.Empty;
                        cmd.Parameters.Add("@Classification", SqlDbType.VarChar).Value = membershipProfile.Classification != null ? membershipProfile.Classification : string.Empty;
                        cmd.Parameters.Add("@SignaturePath", SqlDbType.VarChar).Value = membershipProfile.SignaturePath != null ? membershipProfile.SignaturePath : string.Empty;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Updates a MembershipProfile Details
        /// </summary>
        /// <param name="membershipProfile">MembershipProfile</param>
        public static void UpdateMembershipProfile(MembershipProfile membershipProfile)
        {
            using (var conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("MembershipProfile_UpdateDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = membershipProfile.Id;
                        cmd.Parameters.Add("@ProfileStatus", SqlDbType.Int).Value = membershipProfile.ProfileStatus;
                        cmd.Parameters.Add("@Chapter", SqlDbType.Int).Value = membershipProfile.Chapter;
                        cmd.Parameters.Add("@UnitName", SqlDbType.VarChar).Value = membershipProfile.UnitName != null ? membershipProfile.UnitName : string.Empty;
                        cmd.Parameters.Add("@GSTIN", SqlDbType.VarChar).Value = membershipProfile.GSTIN != null ? membershipProfile.GSTIN : string.Empty;
                        cmd.Parameters.Add("@GSTcertpath", SqlDbType.VarChar).Value = membershipProfile.GSTcertpath != null ? membershipProfile.GSTcertpath : string.Empty;
                        cmd.Parameters.Add("@IndustryStatus", SqlDbType.VarChar).Value = membershipProfile.IndustryStatus != null ? membershipProfile.IndustryStatus : string.Empty;
                        cmd.Parameters.Add("@Address", SqlDbType.Text).Value = membershipProfile.Address != null ? membershipProfile.Address : string.Empty;
                        cmd.Parameters.Add("@District", SqlDbType.VarChar).Value = membershipProfile.District != null ? membershipProfile.District : string.Empty;
                        cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = membershipProfile.City != null ? membershipProfile.City : string.Empty;
                        cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = membershipProfile.State != null ? membershipProfile.State : string.Empty;
                        cmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = membershipProfile.Country != null ? membershipProfile.Country : string.Empty;
                        cmd.Parameters.Add("@Pincode", SqlDbType.VarChar).Value = membershipProfile.Pincode != null ? membershipProfile.Pincode : string.Empty;
                        cmd.Parameters.Add("@WebsiteUrl", SqlDbType.VarChar).Value = membershipProfile.WebsiteUrl != null ? membershipProfile.WebsiteUrl : string.Empty;
                        cmd.Parameters.Add("@ProductCategory", SqlDbType.VarChar).Value = membershipProfile.ProductCategory != null ? membershipProfile.ProductCategory : string.Empty;
                        cmd.Parameters.Add("@ProductSubCategory", SqlDbType.VarChar).Value = membershipProfile.ProductSubCategory != null ? membershipProfile.ProductSubCategory : string.Empty;
                        cmd.Parameters.Add("@MajorProducts", SqlDbType.Text).Value = membershipProfile.MajorProducts != null ? membershipProfile.MajorProducts : string.Empty;
                        cmd.Parameters.Add("@AnnualTurnOver", SqlDbType.VarChar).Value = membershipProfile.AnnualTurnOver != null ? membershipProfile.AnnualTurnOver : string.Empty;
                        cmd.Parameters.Add("@EnterpriseType", SqlDbType.VarChar).Value = membershipProfile.EnterpriseType != null ? membershipProfile.EnterpriseType : string.Empty;
                        cmd.Parameters.Add("@Exporter", SqlDbType.VarChar).Value = membershipProfile.Exporter != null ? membershipProfile.Exporter : string.Empty;
                        cmd.Parameters.Add("@Classification", SqlDbType.VarChar).Value = membershipProfile.Classification != null ? membershipProfile.Classification : string.Empty;
                        cmd.Parameters.Add("@SignaturePath", SqlDbType.VarChar).Value = membershipProfile.SignaturePath != null ? membershipProfile.SignaturePath : string.Empty;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
        }

        /// <summary>
        /// Inserts a MembershipPayment
        /// </summary>
        /// <param name="membershipPayment">MembershipPayment</param>
        public static void InsertMembershipPayment(MembershipPayment membershipPayment)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("MembershipPayment_InsertDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = membershipPayment.UserId;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.Int).Value = membershipPayment.PaymentMode;
                        cmd.Parameters.Add("@ChequeNumber", SqlDbType.VarChar).Value = membershipPayment.ChequeNumber != null ? membershipPayment.ChequeNumber : string.Empty;
                        cmd.Parameters.Add("@FullAmount", SqlDbType.Int).Value = membershipPayment.FullAmount;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Creates a news
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="sourceLink">Source Link</param>
        /// <param name="category">Category</param>
        /// <param name="imagePath">Image Path</param>
        /// <param name="creatorAdminId">Creator Admin ID</param>
        public static void CreateNews(string title, string description, string sourceLink, string category, string imagePath, int creatorAdminId)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("News_CreateNews", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = title;
                    cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = description != null ? description : string.Empty;
                    cmd.Parameters.Add("@Link", SqlDbType.VarChar).Value = sourceLink != null ? sourceLink : string.Empty;
                    cmd.Parameters.Add("@ImagePath", SqlDbType.VarChar).Value = imagePath != null ? imagePath : string.Empty;
                    cmd.Parameters.Add("@Category", SqlDbType.VarChar).Value = category;
                    cmd.Parameters.Add("@CreatorAdminId", SqlDbType.Int).Value = creatorAdminId;
                    cmd.Parameters.Add("@CreationTime", SqlDbType.DateTime).Value = DateTime.Now;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Creates a Offers
        /// </summary>
        /// <param name="title">Title</param>
        public static void CreateOffers(string catgId, string organisationName, string title, string percentageDiscount, string fixedDiscount, string organisationAddress, string city, string email, string phone, bool NationalValidity, string startDate, string expiryDate, string imagePath, string OfferDescription)
        {
            using ( var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Offers_CreateOffers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CategoryId", SqlDbType.VarChar).Value = catgId;
                    cmd.Parameters.Add("@OrganisationName", SqlDbType.VarChar).Value = organisationName;
                    cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = title;
                    cmd.Parameters.Add("@PercentageDiscount", SqlDbType.VarChar).Value = percentageDiscount;
                    cmd.Parameters.Add("@FixedDiscount", SqlDbType.VarChar).Value = fixedDiscount;
                    cmd.Parameters.Add("@OrganisationAddress", SqlDbType.VarChar).Value = organisationAddress;
                    cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = city;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = phone;
                    cmd.Parameters.Add("@NationalValidity", SqlDbType.Bit).Value = NationalValidity;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = startDate;
                    cmd.Parameters.Add("@ExpiryDate", SqlDbType.VarChar).Value = expiryDate;
                    cmd.Parameters.Add("@ImagePath", SqlDbType.VarChar).Value = imagePath;
                    cmd.Parameters.Add("@OfferDescription", SqlDbType.VarChar).Value = OfferDescription;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }

        }

        /// <summary>
        /// Get all the news for the given category
        /// </summary>
        /// <param name="category">Category to filter</param>
        /// <returns>List of news items</returns>
        public static List<News> GetNews(string category)
        {
            var newsList = new List<News>();
            var sproc = "News_GetNewsForAllCategory";
            if (!string.IsNullOrEmpty(category))
            {
                sproc = "News_GetNewsForSpecificCategory";
            }

            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(sproc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(category))
                    {
                        cmd.Parameters.Add("@Category", SqlDbType.VarChar).Value = category;
                    }

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var news = new News(
                                rdr.GetInt32(0).ToString(),
                                rdr.GetString(1).ToString(),
                                rdr.GetString(2).ToString(),
                                rdr.GetString(3).ToString(),
                                rdr.GetString(4).ToString(),
                                rdr.GetString(5).ToString(),
                                rdr.GetString(6).ToString(),
                                rdr.GetDateTime(7),
                                rdr.GetString(8).ToString());
                            newsList.Add(news);
                        }
                    }
                }
            }

            return newsList;
        }




        /// <summary>
        /// Get all the Chapters
        /// </summary>
        /// <returns>List of Chapters</returns>
        public static List<Chapter> GetChapters()
        {
            var chaptersList = new List<Chapter>();

            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Chapters_GetChapters", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var chapter = new Chapter(
                                rdr.GetInt32(0).ToString(),
                                rdr.GetString(1).ToString());
                            chaptersList.Add(chapter);
                        }
                    }
                }
            }

            return chaptersList;
        }

        /// <summary>
        /// Get if admin exists or not
        /// </summary>
        /// <returns>List of Chapters</returns>
        public static bool CheckIfAdminExistsOrNot(string phoneNumber)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Admin_CheckIfAdminExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = phoneNumber;

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a Ticket
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="category">Category</param>
        /// <param name="userid">User ID</param>
        public static void CreateTicket(string title, string description, string category, string userid)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_CreateTickets", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = title;
                    cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = description;
                    cmd.Parameters.Add("@Category", SqlDbType.VarChar).Value = category;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userid;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Adds a comment to existing Ticket
        /// </summary>
        /// <param name="ticketNumber">TicketNumber</param>
        /// <param name="comments">Comment</param>
        /// <param name="userId">UserId</param>
        /// <param name="adminId">AdminId</param>
        public static void AddComment(string ticketNumber, string comments, string userId, string adminId)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_AddComment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TicketNumber", SqlDbType.Int).Value = ticketNumber;
                    cmd.Parameters.Add("@Comment", SqlDbType.VarChar).Value = comments;
                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@AdminId", SqlDbType.Int).Value = adminId;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Adds an attachmetn to existing Ticket
        /// </summary>
        /// <param name="ticketNumber">TicketNumber</param>
        /// <param name="attachmenturl">AttachmentURL</param>
        public static void AddAttachment(string ticketNumber, string attachmenturl)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_AddAttachment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TicketNumber", SqlDbType.Int).Value = ticketNumber;
                    cmd.Parameters.Add("@AttachmentURL", SqlDbType.VarChar).Value = attachmenturl;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get Ticket Details for a particular ticket
        /// </summary>
        /// <param name="ticketnumber">TicketNumber</param>
        /// <returns>Ticket Details</returns>
        public static List<Tickets> GetTicket(string ticketnumber)
        {
            var ticketList = new List<Tickets>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_GetTicketDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(ticketnumber))
                    {
                        cmd.Parameters.Add("@TicketNumber", SqlDbType.Int).Value = ticketnumber;
                    }

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var comments = GetComment(ticketnumber);
                            var attachments = GetAttachment(ticketnumber);
                            var ticket = new Tickets(
                                rdr.GetInt32(0).ToString(),
                                rdr.GetString(1).ToString(),
                                rdr.GetString(2).ToString(),
                                rdr.GetString(3),
                                rdr.GetInt32(4).ToString(),
                                rdr.GetString(5).ToString(),
                                rdr.GetDateTime(6),
                                rdr.GetInt32(7).ToString(),
                                comments,
                                attachments);
                            ticketList.Add(ticket);
                        }
                    }
                }
            }

            return ticketList;
        }

        /// <summary>
        /// Get Comments for a particular ticket
        /// </summary>
        /// <param name="ticketnumber">TicketNumber</param>
        /// <returns>Ticket comment Details</returns>
        public static List<Comment> GetComment(string ticketnumber)
        {
            var commentList = new List<Comment>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_GetComments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(ticketnumber))
                    {
                        cmd.Parameters.Add("@TicketNumber", SqlDbType.Int).Value = ticketnumber;
                    }

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var comment = new Comment(
                                rdr.GetInt32(0).ToString(),
                                SafeGetString(rdr, 1),
                                rdr.GetString(2).ToString(),
                                rdr.GetDateTime(3),
                                SafeGetString(rdr, 4));
                            commentList.Add(comment);
                        }
                    }
                }
            }

            return commentList;
        }

        /// <summary>
        /// Get Attachment for a particular ticket
        /// </summary>
        /// <param name="ticketnumber">TicketNumber</param>
        /// <returns>Ticket attachment Details</returns>
        public static List<Attachment> GetAttachment(string ticketnumber)
        {
            var attachmentList = new List<Attachment>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_GetAttachment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(ticketnumber))
                    {
                        cmd.Parameters.Add("@TicketNumber", SqlDbType.Int).Value = ticketnumber;
                    }

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var attachment = new Attachment(
                                rdr.GetInt32(0).ToString(),
                                SafeGetString(rdr, 1),
                                SafeGetString(rdr, 2),
                                rdr.GetString(3).ToString(),
                                rdr.GetDateTime(4));
                            attachmentList.Add(attachment);
                        }
                    }
                }
            }

            return attachmentList;
        }

        /// <summary>
        /// Close an existing Ticket
        /// </summary>
        /// <param name="ticketNumber">TicketNumber</param>
        public static void CloseTicket(string ticketNumber)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_CloseTicket", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TicketNumber", SqlDbType.Int).Value = ticketNumber;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Change chapter of an existing Ticket
        /// </summary>
        /// <param name="ticketnumber">TicketNumber</param>
        /// <param name="chapterid">ChapterId</param>
        public static void ChangeChapter(string ticketnumber, string chapterid)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_ChangeChapter", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TicketNumber", SqlDbType.Int).Value = ticketnumber;
                    cmd.Parameters.Add("@ChapterId", SqlDbType.Int).Value = chapterid;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get Ticket Summary for User
        /// </summary>
        /// <param name="userid">UserId</param>
        /// <returns>Ticket summary details</returns>
        public static List<Tickets> GetSummaryForUser(string userid)
        {
            var summaryList = new List<Tickets>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_GetSummaryForUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(userid))
                    {
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userid;
                    }

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var ticket = new Tickets(
                                rdr.GetInt32(0).ToString(),
                                rdr.GetString(1).ToString(),
                                rdr.GetString(2).ToString(),
                                rdr.GetString(3),
                                rdr.GetInt32(4).ToString(),
                                rdr.GetString(5).ToString(),
                                rdr.GetDateTime(6),
                                rdr.GetInt32(7).ToString(),
                                null,
                                null);
                            summaryList.Add(ticket);
                        }
                    }
                }
            }

            return summaryList;
        }

        /// <summary>
        /// Change status of an existing Ticket
        /// </summary>
        /// <param name="ticketnumber">TicketNumber</param>
        /// <param name="status">Status</param>
        public static void ChangeStatus(string ticketnumber, string status)
        {
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_ChangeStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@TicketNumber", SqlDbType.Int).Value = ticketnumber;
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get ticket summary chapter wise
        /// </summary>
        /// <param name="chapterid">ChapterId</param>
        /// <returns>Ticket summary chapter wise Details</returns>
        public static List<Tickets> GetSummaryForChapters(string chapterid)
        {
            var chapterticketList = new List<Tickets>();
            using (var conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Tickets_GetSummaryForChapters", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(chapterid))
                    {
                        cmd.Parameters.Add("@ChapterId", SqlDbType.Int).Value = chapterid;
                    }

                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var ticket = new Tickets(
                                rdr.GetInt32(0).ToString(),
                                rdr.GetString(1).ToString(),
                                rdr.GetString(2).ToString(),
                                rdr.GetString(3),
                                rdr.GetInt32(4).ToString(),
                                rdr.GetString(5).ToString(),
                                rdr.GetDateTime(6),
                                rdr.GetInt32(7).ToString(),
                                null,
                                null);
                            chapterticketList.Add(ticket);
                        }
                    }
                }
            }

            return chapterticketList;
        }

        /// <summary>
        /// Handles null values
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="colIndex">CollIndex</param>
        /// <returns>handling null values</returns>
        private static string SafeGetString(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
            {
                return reader.GetString(colIndex);
            }

            return string.Empty;
        }

        private static SqlConnection GetConnection()
        {
            return new SqlConnection(Environment.GetEnvironmentVariable("DatabaseEndpoint"));
        }
    }
}
