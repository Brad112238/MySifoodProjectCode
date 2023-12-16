﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sifoodproject.Areas.Users.Models.ViewModels;
using sifoodproject.Models;

namespace sifoodproject.Areas.Users.Controllers
{
    [Area("Users")]

    public class HomeController : Controller
    {
        Sifood3Context _context;

        public HomeController(Sifood3Context context)
        {
            _context = context;
        }

        public IActionResult Main()
        {

            return View();
        }

        public IActionResult MapFind()
        {
            return View();
        }
        [HttpGet]

        public IActionResult FAQ()
        {
            return View();

        }

        [HttpGet]
        public IActionResult JoinUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> JoinUsSubmit(JoinUsVM joinus)
        {
            //擋重複email
            // 檢查電子郵件是否已存在
            var existingStore = await _context.Stores.FirstOrDefaultAsync(s => s.Email == joinus.Email);
            if (existingStore != null)
            {
                // 電子郵件已存在，返回錯誤訊息
                return "Email重複";
            }

            // 格式化營業時間
            string formattedOpeningTime = $"平日 {joinus.WeekdayStartTime} - {joinus.WeekdayEndTime}，週末 {joinus.WeekendStartTime} - {joinus.WeekendEndTime}";
            //string formattedOpeningTime = $"平日 {joinus.WeekdayStartTime:HH:mm} - {joinus.WeekdayEndTime:HH:mm}，週末 {joinus.WeekendStartTime:HH:mm} - {joinus.WeekendEndTime:HH:mm}";


            // 處理 Logo 圖片上傳
            string logoPathInDb = await SavePhoto(joinus.LogoPath, "logo");

            // 處理三張店家照片上傳
            string photoPathInDb = await SavePhoto(joinus.PhotosPath, "photo");
            string photoPath2InDb = await SavePhoto(joinus.PhotosPath2, "photo");
            string photoPath3InDb = await SavePhoto(joinus.PhotosPath3, "photo");

            //創建store實體

            var store = new Store
            {
                // 將joinus 的數據映射到 store 實體
                // 賦值
                StoreName = joinus.StoreName,
                ContactName = joinus.ContactName,
                Email = joinus.Email,
                Phone = joinus.Phone,
                TaxId = joinus.TaxId,
                City = joinus.City,
                Region = joinus.Region,
                Address = joinus.Address,
                Description = joinus.Description,
                ClosingDay = joinus.ClosingDay,
                OpeningTime = formattedOpeningTime,
                LogoPath = logoPathInDb,
                PhotosPath = photoPathInDb,
                PhotosPath2 = photoPath2InDb,
                PhotosPath3 = photoPath3InDb,
                Latitude = joinus.Latitude,
                Longitude = joinus.Longitude,
            };
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(store);
                    await _context.SaveChangesAsync();
                    return "成功";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"錯誤: {ex.Message}");
            }
            return "失敗";

        }


        //儲存照片專用方法SavePhoto()
        private async Task<string> SavePhoto(IFormFile photo, string folderName)
        {
            if (photo != null)
            {
                var fileName = Path.GetFileName(photo.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Stores", folderName, fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                return $"/images/Stores/{folderName}/{fileName}";
            }

            return null;
        }


        public IActionResult MemberShip()
        {
            return View();
        }

        public IActionResult Stores()
        {
            return View();
        }

        private Dictionary<int, ProductVM> ProductCache = new Dictionary<int, ProductVM>();
        [Route("/Users/Home/Products/{ProductId?}")]

        public async Task<IActionResult> Products(int ProductId)
        {
            var IdToString = ProductId.ToString();
            List<string> ProductList =GetCookieProductList();//讀取
            ProductList.Remove(IdToString);
            if (IdToString != "0") { ProductList.Add(IdToString); }
            SetCookieProductList(ProductList);//取陣列最後一個以外的值
            //倒敘且只留最後五個
            ProductList.Reverse();
            ProductList = ProductList.Take(5).ToList();
            //===========================================================================
            List<int> productIds = ProductList.Select(int.Parse).ToList();
            var products = productIds.Select(productId =>
            {
                if (!ProductCache.TryGetValue(productId, out var cachedProduct))
                {
                    var productFromDb = _context.Products
                        .Where(p => p.ProductId == productId)
                        .FirstOrDefault();
                    if (productFromDb != null)
                    {
                        cachedProduct = new ProductVM
                        {
                            ProductId = productFromDb.ProductId,
                            ProductName = productFromDb.ProductName,
                            StoreName = productFromDb.Store.StoreName,
                            PhotoPath = productFromDb.PhotoPath,
                            UnitPrice = Math.Round(productFromDb.UnitPrice, 2)
                        };
                        ProductCache[productId] = cachedProduct;
                    }
                }
                return cachedProduct;
            }).OrderBy(p => ProductList.IndexOf(p.ProductId.ToString())).ToList();

            ViewBag.CookieProduct = products;

            return View();
        }
        private List<string> GetCookieProductList()
        {
            string? ProductCookieValue = Request.Cookies["Records"];
            List<string> ProductList = new List<string>();
            if (ProductCookieValue != null && ProductCookieValue != "")
            {
                ProductList.AddRange(ProductCookieValue.Split(','));
                ProductList = ProductList.Skip(Math.Max(0, ProductList.Count - 5)).ToList();
            }
            return ProductList;//{"32","33","34"}
        }
        private void SetCookieProductList(List<string> product)
        {
            string RecentBrowse = string.Join(",", product);
            CookieOptions CO = new CookieOptions();
            CO.Expires = DateTime.Now.AddDays(1);
            CO.HttpOnly = true;
            CO.Secure = true;
            Response.Cookies.Append("Records", RecentBrowse, CO);
        }
        [Authorize]
        [HttpGet]
        public IActionResult RealTimeOrders()
        {
            return View();
        }


    }
}


