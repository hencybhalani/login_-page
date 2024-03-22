using Npgsql;
using System.Net.Mail;
using System.Net;
using System.Security.Principal;

namespace login__pag1.Models
{
    public class MyDBContext
    {
        
          string cs = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=admin123;";
        NpgsqlConnection conn = null;
        public List<User> Getuser() //retrive data
        {
            List<User> user = new List<User>();
            conn = new NpgsqlConnection(cs);
            conn.Open();
            string select = "select * from  public.users";
            NpgsqlCommand cmd = new NpgsqlCommand(select, conn);
            NpgsqlDataReader da = cmd.ExecuteReader();
            while (da.Read())
            {

                User car = new  User();
                car.Id = Convert.ToInt32(da.GetValue(3).ToString());
                car.Emailid = da.GetValue(0).ToString();
                car.Password = da.GetValue(1).ToString();
                car.Name = da.GetValue(2).ToString();
              
                user.Add(car);
            }
            return user;
            conn.Close();
        }
        public bool AddUser(User user) //insert data
        {
            conn = new NpgsqlConnection(cs);

            //string  insert = "insert into public.cars VALUES(@brand,@model,@year)";
            string insert = "insert into public.users values(@Emailid ,@Password ,@Name)";
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(insert, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@Emailid", user.Emailid);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            //    cmd.Parameters.AddWithValue("@Action", "Insert");
            int count = cmd.ExecuteNonQuery();

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            conn.Close();
        }
        public bool UpdateUser(User user) //insert data
        {
            conn = new NpgsqlConnection(cs);

            //string  insert = "insert into public.cars VALUES(@brand,@model,@year)";
            string insert = "Update public.users set Emailid=@Emailid,Password=@Password,Name=@Name where Id=@Id;";
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(insert, conn);
            cmd.CommandType = System.Data.CommandType.Text;
  
            cmd.Parameters.AddWithValue("@Emailid", user.Emailid);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Id", user.Id);
            

            //    cmd.Parameters.AddWithValue("@Action", "Insert");
            int count = cmd.ExecuteNonQuery();

            if (count > 0)
            {
                return true;
            }
            else
            {
                
                return false;
            }
            conn.Close();
        }
        public bool DeleteUser(int Id) //delete data
        {
            conn = new NpgsqlConnection(cs);

            //string delete = "delete  public.cars where car_id=@car_id";
            string delete = "delete from public.users where Id=@Id;";
            NpgsqlCommand cmd = new NpgsqlCommand(delete, conn);
            conn.Open();
            cmd.Parameters.AddWithValue("@Id", Id);
            int count = cmd.ExecuteNonQuery();

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            conn.Close();
        }
        public void sendEmailToClient(String Email,string otp)
        {

            SMTPConfiguration sMTPConfiguration = new SMTPConfiguration
            {
                server = "smtp.gmail.com",
                hostName = "bhavymunjani1418@gmail.com",
                password = "bqqwhpqlxlvhnwuk",
                port = 587
            };

            MailMessage message = new MailMessage();
/*
            Random r=new Random();
            int num = r.Next();*/

            message.From = new MailAddress(sMTPConfiguration.hostName);
            message.Subject = "Welcome to NextGen Technology 🎉";
            
            message.To.Add(new MailAddress(Email.Trim().ToLower().ToString()));
            message.Body = @$"
                        <!DOCTYPE html>
                        <html lang='en'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        </head>
                        <body style='font-family: Arial, sans-serif; background-color: transparent; margin: 0; padding: 0;  color:white;'>
                            <div class='container' style='width: 80%; margin: 20px auto; max-width: 600px;background-color: #0a3d62; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); padding: 20px;'>
                                <img src='https://storage.googleapis.com/nextgen_technology/Assets/logos/logo4-light.png' alt='Company Logo' style='max-width: 100px;height: auto; display: block; margin: 0 auto 20px;'>
                                <div class='content' style='text-align: center;'>
                                    <p>Hi Otp Is<span class='name' style='font-weight: bold;'><b>{otp}<p>Do not Otp share with others!!</p></b></span>,</p>
                                    <p>We're thrilled you reached out to us! At NextGen Technology, our team will connect with you as soon as possible.</p>
                                </div>
                            </div>
                        </body>
                        </html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient(sMTPConfiguration.server)
            {
                Port = sMTPConfiguration.port,
                Credentials = new NetworkCredential(sMTPConfiguration.hostName, sMTPConfiguration.password),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }
    }
}

