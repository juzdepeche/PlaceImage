using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Omu.Drawing;
using PlaceImage.Utils;

namespace PlaceImage.Controllers
{
    [Route("")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        // GET /width/height
        [HttpGet("{width}/{height}")]
        public ActionResult<Stream> GetImage(int width, int height)
        {
            Stream returnedImage = null;

            Stream stream = System.IO.File.Open("./Images/andre.png", FileMode.Open);
            try
            {
                var img = Bitmap.FromStream(stream);

                var image = width > height ? Imager.Resize(img, width, width, false) : Imager.Resize(img, height, height, false);

                //Canvas Rectangle
                Rectangle rect;
                if (width < height)
                {
                    rect = new Rectangle(image.Width / 4, 0, width, height);
                }
                else if (width > height)
                {
                    rect = new Rectangle(0, image.Height / 4, width, height);
                }
                else
                {
                    rect = new Rectangle(0, 0, width, height);
                }

                //rect.Intersect(new Rectangle(0, 0, image.Width, image.Height));

                //crop
                image = ((Bitmap)image).Clone(rect, System.Drawing.Imaging.PixelFormat.DontCare);

                //image to stream
                returnedImage = ImageUtils.ToStream(image, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch(Exception ex)
            {
                stream.Dispose();
                return System.IO.File.Open("./Images/andre.png", FileMode.Open);
            }
            stream.Dispose();
            return returnedImage;
        }
    }
}
