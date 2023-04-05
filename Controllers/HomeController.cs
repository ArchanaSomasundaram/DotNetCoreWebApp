using DotNetCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace DotNetCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public string? dbconnectionStr { get; private set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]   
        public IActionResult Index()
        {
            //Product();
            //return View();

            List<Product> list = ProductViewModel();
            return View(list);
        }
        public List<Product> ProductViewModel()
        {
            List<Product> items = new List<Product>();

            for (int i = 0; i < 10; i++)
            {
                items.Add(new Product() { Id = i, ProductName = $"Name_{i}" });
            }

            return items;
        }
        //List<Product> productList;
        public List<Product> productDetails()
        {
            List<Product> productList = new List<Product>();
            productList.Add(new Product { Id = 1, ProductCost = 200, ProductDescription = "Ice Cream", ProductName = "Ice Cream", Stock = 5 });
            productList.Add(new Product { Id = 2, ProductCost = 200, ProductDescription = "Chocolate", ProductName = "Chocolate", Stock = 5 });
            productList.Add(new Product { Id = 3, ProductCost = 200, ProductDescription = "Cookie", ProductName = "Cookie", Stock = 5 });
            productList.Add(new Product { Id = 4, ProductCost = 200, ProductDescription = "French Fries", ProductName = "French Fries", Stock = 5 });

            return productList;
        }
        [HttpPost]
        public IActionResult Product(List<Product> items)
        {
            //List<Product> productList;
            //var dbconfig = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json").Build();
            try
            {
                //string dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                //string sql = "SP_Get_ProductList";
                //using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                //{
                //    SqlCommand command = new SqlCommand(sql, connection);
                //    connection.Open();
                //    using (SqlDataReader dataReader = command.ExecuteReader())
                //    {
                //        while (dataReader.Read())
                //        {
                //            Product product = new Product();
                //            product.Id = Convert.ToInt32(dataReader["Id"]);
                //            product.ProductName = Convert.ToString(dataReader["ProductName"]);
                //            product.ProductDescription = Convert.ToString(dataReader["ProductDescription"]);
                //            product.ProductCost = Convert.ToDecimal(dataReader["ProductCost"]);
                //            product.Stock = Convert.ToInt32(dataReader["Stock"]);
                //            productList.Add(product);
                //        }
                //    }
                //}

                //productList.Add(new Product { Id = 1,ProductCost = 200,ProductDescription = "Ice Cream",ProductName= "Ice Cream", Stock=5 });
                //productList.Add(new Product { Id = 2, ProductCost = 200, ProductDescription = "Chocolate", ProductName = "Chocolate", Stock = 5 });
                //productList.Add(new Product { Id = 3, ProductCost = 200, ProductDescription = "Cookie", ProductName = "Cookie", Stock = 5 });
                //productList.Add(new Product { Id = 4, ProductCost = 200, ProductDescription = "French Fries", ProductName = "French Fries", Stock = 5 });

                //var data = TempData["productList"] as List<Product>;
                //productList = productDetails();
                return View(items);

            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ProductCreate(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbconfig = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();



                    //if (!string.IsNullOrEmpty(dbconfig.ToString()))
                    //{
                    //    dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                    //    using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                    //    {
                    //        string sql = "SP_Add_New_Product";
                    //        using (SqlCommand cmd = new SqlCommand(sql, connection))
                    //        {
                    //            cmd.CommandType = CommandType.StoredProcedure;
                    //            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    //            cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                    //            cmd.Parameters.AddWithValue("@ProductCost", product.ProductCost);
                    //            cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    //            connection.Open();
                    //            cmd.ExecuteNonQuery();
                    //            connection.Close();
                    //        }
                    //    }
                    //}

                    //List<Product> productList = new List<Product>();
                    //TempData["productList"] = productList;
                    
                    List<Product> productList = productDetails();

                    foreach (var v in productList)
                    {
                        if (v.ProductName != product.ProductName)
                        {
                            productList.Add(product);
                            break;
                        }
                    }
                    TempData["productList"] = productList;
                }
            }
            catch (Exception)
            {
                throw;
            }
           return Json(product);
        }

        public IActionResult ProductUpdate(int id)
        {
            var dbconfig = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();
            dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
            Product product = new Product();
            using (SqlConnection connection = new SqlConnection(dbconnectionStr))
            {
                string sql = "SP_Get_Product_By_Id";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            product.Id = Convert.ToInt32(dataReader["Id"]);
                            product.ProductName = Convert.ToString(dataReader["ProductName"]);
                            product.ProductDescription = Convert.ToString(dataReader["ProductDescription"]);
                            product.ProductCost = Convert.ToDecimal(dataReader["ProductCost"]);
                            product.Stock = Convert.ToInt32(dataReader["Stock"]);
                        }
                    }
                }
                connection.Close();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult ProductUpdate(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbconfig = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
                    if (!string.IsNullOrEmpty(dbconfig.ToString()))
                    {
                        dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
                        using (SqlConnection connection = new SqlConnection(dbconnectionStr))
                        {
                            string sql = "SP_Update_Product";
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@id", product.Id);
                                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                                cmd.Parameters.AddWithValue("@ProductCost", product.ProductCost);
                                cmd.Parameters.AddWithValue("@Stock", product.Stock);
                                connection.Open();
                                cmd.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Product");
        }

        [HttpPost]
        public IActionResult ProductDelete(int id)
        {
            var dbconfig = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();
            dbconnectionStr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(dbconnectionStr))
            {
                string sql = "SP_Delete_Product_By_Id";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {

                    }
                    connection.Close();
                }
            }
            return RedirectToAction("Product");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}

namespace DotNetCoreWebApp
{
    class List : List<Product>
    {
    }
}