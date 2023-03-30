using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Captcha.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public string GenerateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, 6)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }
        public ActionResult CaptchaImage()
        {
            string captchaCode = GenerateRandomString();
            HttpContext.Session["CaptchaCode"] = captchaCode;
            // Create a bitmap image of the captcha code
            Bitmap bitmap = new Bitmap(150, 50, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
                graphics.Clear(Color.Gray);
                // Draw the captcha code on the image
                Font font = new Font("Arial", 20, FontStyle.Bold);
                graphics.DrawString(captchaCode, font, new SolidBrush(Color.Black), 10, 10);
            Random random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var x1 = random.Next(0, 200);
                var y1 = random.Next(0, 70);
                var x2 = random.Next(0, 200);
                var y2 = random.Next(0, 70);
                graphics.DrawLine(Pens.SeaGreen, x1, y1, x2, y2);
            }
            // Convert the bitmap image to a byte array and return it as an image
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            byte[] captchaBytes = memoryStream.ToArray();
            return File(captchaBytes, "image/png");
        }

    }
}