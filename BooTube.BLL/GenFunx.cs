using BooTube.DAL;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BooTube.BLL
{
    public static class GenFunx
    {
        public static string CleanStringForUse(string text)
        {
            return text.Trim(' ', '.', ';', '&', '*', '/');
        }

        /// <summary>
        /// Upload posted image with new name. Returns string full file path
        /// </summary>
        /// <param name="newImage"></param>
        /// <param name="fullFolderPath">Full path of folder to save. For example ~/Content/Images/ExampleFolder</param>
        /// <param name="isUploaded"></param>
        /// <returns></returns>
        public static string ImageUpload(HttpPostedFileBase newImage, string fullFolderPath, out bool isUploaded)
        {
            try
            {
                if (newImage.ContentType.Contains("image"))
                {
                    if (newImage.ContentLength < 1000000)
                    {
                        string fileName = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                        string path = string.Format("{0}/{1}.{2}", fullFolderPath, fileName, newImage.ContentType.Split('/')[1]);
                        newImage.SaveAs(HttpContext.Current.Server.MapPath(path));
                        isUploaded = true;
                        return path;
                    }
                    else
                    {
                        isUploaded = false;
                        return "Yüklemeye çalıştığınız resim çok büyük.";
                    }
                }
                else
                {
                    isUploaded = false;
                    return "Yüklemeye çalıştığınız dosya bir resim değil.";
                }
            }
            catch (Exception ex)
            {
                isUploaded = false;
                return ex.Message;
            }
        }

        /// <summary>
        /// Saves image with new name and selected Image Format. Returns saved file path and isUploaded
        /// </summary>
        /// <param name="newImage">Image to save</param>
        /// <param name="fullFolderPath">Full path of folder to save. For example ~/Content/Images/ExampleFolder</param>
        /// <param name="format">Format to be saved</param>
        /// <param name="isUploaded">out Is save successfull?</param>
        /// <returns></returns>
        public static string ImageUpload(Image newImage, string fullFolderPath, ImageFormat format, out bool isUploaded)
        {
            try
            {
                ImageFormatConverter imgConverter = new ImageFormatConverter();
                string fileName = Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower();
                string extension = imgConverter.ConvertToString(format);
                string path = string.Format("{0}/{1}.{2}", fullFolderPath, fileName, extension);
                newImage.Save(HttpContext.Current.Server.MapPath(path));
                isUploaded = true;
                return path;
            }
            catch (Exception ex)
            {
                isUploaded = false;
                return ex.Message;
            }
        }

        /// <summary>
        /// Saves image with given full name that includes extension. Returnes saved file path
        /// </summary>
        /// <param name="newImage">Image to save</param>
        /// <param name="fullFolderPath">Full path of folder to save. For example ~/Content/Images/ExampleFolder</param>
        /// <param name="isUploaded">Format to be saved</param>
        /// <param name="fullFileName">Desired full name of the file includes extension</param>
        /// <returns></returns>
        public static string ImageUpload(Image newImage, string fullFolderPath, string fullFileName, out bool isUploaded)
        {
            try
            {
                string path = string.Format("{0}/{1}", fullFolderPath, fullFileName);
                newImage.Save(HttpContext.Current.Server.MapPath(path));
                isUploaded = true;
                return path;
            }
            catch (Exception ex)
            {
                isUploaded = false;
                return ex.Message;
            }
        }

        /// <summary>
        /// Resizes Image with desired sizes
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static Image ResizeImage(string imgPath, int newWidth, int newHeight)
        {
            Image imgToResize = Image.FromFile(imgPath);
            Bitmap b = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, newWidth, newHeight);
            g.Dispose();
            imgToResize.Dispose();
            return (Image)b;
        }

        /// <summary>
        /// Resizes Image without changing original sizes
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image ResizeImage(string imgPath, Size size)
        {
            Image imgToResize = Image.FromFile(HttpContext.Current.Server.MapPath(imgPath));
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            imgToResize.Dispose();
            return b;

        }

        /// <summary>
        /// Saves video with new name to specified folder
        /// </summary>
        /// <param name="newVideo">File to upload</param>
        /// <param name="fullFolderPath">Path to upload the file</param>
        /// <param name="isUploaded">Upload state</param>
        /// <returns>Full path of upladed file</returns>
        public static string VideoUpload(HttpPostedFileBase newVideo, string fullFolderPath, out bool isUploaded)
        {
            try
            {
                if (newVideo.ContentType.Contains("video"))
                {
                    if (newVideo.ContentLength < 1000000000)
                    {
                        string fileName = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                        string path = string.Format("{0}/{1}.{2}", fullFolderPath, fileName, newVideo.ContentType.Split('/')[1]);
                        newVideo.SaveAs(HttpContext.Current.Server.MapPath(path));
                        isUploaded = true;
                        return path;
                    }
                    else
                    {
                        isUploaded = false;
                        return "Yüklemeye çalıştığınız dosya çok büyük";
                    }
                }
                else
                {
                    isUploaded = false;
                    return "Yüklemeye çalıştığınız dosya bir video değil.";
                }

            }
            catch (Exception ex)
            {
                isUploaded = false;
                return ex.Message;

            }
        }

        /// <summary>
        /// Saves the video with specified name to specified folder
        /// </summary>
        /// <param name="newVideo">File to upload</param>
        /// <param name="fullFolderPath">Path to upload the file</param>
        /// <param name="fullFileName">Name to give the file</param>
        /// <param name="isUploaded">Upload state</param>
        /// <returns>Full path of uploaded file</returns>
        public static string VideoUpload(HttpPostedFileBase newVideo, string fullFolderPath, string fullFileName, out bool isUploaded)
        {
            try
            {
                if (newVideo.ContentType.Contains("video"))
                {
                    if (newVideo.ContentLength < 1000000000)
                    {
                        string fileName = fullFileName;
                        string path = string.Format("{0}/{1}.{2}", fullFolderPath, fileName, newVideo.ContentType.Split('/')[1]);
                        newVideo.SaveAs(HttpContext.Current.Server.MapPath(path));
                        isUploaded = true;
                        return path;
                    }
                    else
                    {
                        isUploaded = false;
                        return "Yüklemeye çalıştığınız dosya çok büyük";
                    }
                }
                else
                {
                    isUploaded = false;
                    return "Yüklemeye çalıştığınız dosya bir video değil.";
                }

            }
            catch (Exception ex)
            {
                isUploaded = false;
                return ex.Message;

            }
        }

        /// <summary>
        /// Takes video on given path and relocates it with its framesize. Deletes sources video after the proccess.
        /// </summary>
        /// <param name="videoPath">Source video path</param>
        /// <param name="isCopied">is copy successfull</param>
        /// <returns></returns>
        public static string RelocateVideo(string videoPath, out bool isCopied)
        {
            isCopied = false;
            if (string.IsNullOrEmpty(videoPath))
            {
                return "Video yolu boş bırakılamaz";
            }
            else
            {
                try
                {
                    if (IsVideoOK(videoPath))
                    {

                        MediaFile file = new MediaFile(HttpContext.Current.Server.MapPath(videoPath));

                        Engine engine = GetEngine();
                        engine.GetMetadata(file);

                        string newFullPath;
                        string fileName = videoPath.Split('/').Last();
                        switch (file.Metadata.VideoData.FrameSize)
                        {

                            case "426x240":
                                newFullPath = "~/Content/Videos/240p/" + fileName;
                                File.Copy(HttpContext.Current.Server.MapPath(videoPath), HttpContext.Current.Server.MapPath(newFullPath));
                                break;
                            case "640x360":
                                newFullPath = "~/Content/Videos/360p/" + fileName;
                                File.Copy(HttpContext.Current.Server.MapPath(videoPath), HttpContext.Current.Server.MapPath(newFullPath));
                                break;
                            case "854x480":
                                newFullPath = "~/Content/Videos/480p/" + fileName;
                                File.Copy(HttpContext.Current.Server.MapPath(videoPath), HttpContext.Current.Server.MapPath(newFullPath));
                                break;
                            case "1280x720":
                                newFullPath = "~/Content/Videos/720p/" + fileName;
                                File.Copy(HttpContext.Current.Server.MapPath(videoPath), HttpContext.Current.Server.MapPath(newFullPath));
                                break;
                            case "1920x1080":
                                newFullPath = "~/Content/Videos/1080p/" + fileName;
                                File.Copy(HttpContext.Current.Server.MapPath(videoPath), HttpContext.Current.Server.MapPath(newFullPath));
                                break;
                            default:
                                newFullPath = "~/Content/Videos/OtherSize/" + fileName;
                                File.Copy(HttpContext.Current.Server.MapPath(videoPath), HttpContext.Current.Server.MapPath(newFullPath));
                                break;
                        }

                        isCopied = true;

                        //delete old file
                        FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(videoPath));
                        fi.Delete();

                        return newFullPath;
                    }
                    else
                    {
                        isCopied = false;
                        return "Video 20 Saniyeden kısa olamaz";
                    }
                }
                catch (Exception ex)
                {
                    isCopied = false;
                    return ex.Message;
                }
            }
        }

        public static string GetThumbnailFromVideo(string videoPath, out bool isDone)
        {
            try
            {
                MediaFile file = new MediaFile(HttpContext.Current.Server.MapPath(videoPath));

                string fileName = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                string thumbnailPath = "~/Content/Images/Thumbnails/" + fileName + ".jpg";

                MediaFile thumbnailFile = new MediaFile(HttpContext.Current.Server.MapPath(thumbnailPath));
                Engine engine = GetEngine();

                ConversionOptions options = new ConversionOptions { Seek = TimeSpan.FromSeconds(15) };
                engine.GetThumbnail(file, thumbnailFile, options);

                isDone = true;
                return thumbnailPath;
            }
            catch (Exception ex)
            {
                isDone = false;
                return ex.Message;
            }

        }

        /// <summary>
        /// Checks if the video is suitable for relocate and use
        /// </summary>
        /// <param name="videoPath"></param>
        /// <returns>true if okey and false if not</returns>
        public static bool IsVideoOK(string videoPath)
        {
            MediaFile file = new MediaFile(HttpContext.Current.Server.MapPath(videoPath));
            Engine engine = GetEngine();
            engine.GetMetadata(file);

            bool durationOK = file.Metadata.Duration.TotalSeconds > 20;

            //Kontroller arttırılabilir. Kalite bitrate vs
            return durationOK;

        }

        public static Video GetVideoFromMediaFile(string videoPath)
        {
            Engine engine = GetEngine();
            MediaFile file = new MediaFile(HttpContext.Current.Server.MapPath(videoPath));
            engine.GetMetadata(file);
            Video video = new Video();
            video.VideoDurationSeconds = file.Metadata.Duration.TotalSeconds;
            switch (file.Metadata.VideoData.FrameSize)
            {

                case "426x240":
                    video.VideoFramzeSize = "240p";
                    break;
                case "640x360":
                    video.VideoFramzeSize = "360p";
                    break;
                case "854x480":
                    video.VideoFramzeSize = "480p";
                    break;
                case "1280x720":
                    video.VideoFramzeSize = "720p";
                    break;
                case "1920x1080":
                    video.VideoFramzeSize = "1080p";
                    break;
                default:
                    video.VideoFramzeSize = "OtherSize";
                    break;
            }
            video.VideoFormat = videoPath.Split('.').Last();
            video.VideoName = videoPath.Split('/').Last().Split('.')[0];
            return video;
        }

        private static List<Video> GenerateQualities(string videoPath)
        {
            return null;
        }

        //Making Engine Singleton
        private static Engine _engine;
        public static Engine GetEngine()
        {
            if (_engine == null)
                _engine = new Engine();

            return _engine;
        }
    }
}
