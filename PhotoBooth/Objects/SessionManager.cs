using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoBooth
{
    //TODO: Finish this up
    class SessionManager
    {
        public void CreateSessionFiles(SessionTemplateCollection sessionPhotos)
        {
            //Get current sessions
            var sessions = Directory.GetDirectories("Images");
            var newDirId = 0;
            if (sessions.Any())//Get last sessionId
            {
                newDirId = sessions.Select(dir => int.Parse(dir.Split('\\').Last().Split('_').Last())).Max();
            }

            var newDir = Directory.CreateDirectory(@"Images\Session_" + (newDirId + 1));

            SaveFrameToFile(Path.Combine(newDir.FullName, "SessionTemplate.jpg"), sessionPhotos.SessionTemplatePhoto);

            var gifStream = GenerateGifStream(sessionPhotos);

            SaveStreamToFile(Path.Combine(newDir.FullName, "session.gif"), gifStream);
                
            var i = 1;
            foreach (Photo photo in sessionPhotos)
            {
                SaveFrameToFile(Path.Combine(newDir.FullName, "img_" + i + ".jpg"), photo.Image);
                i++;
            }
        }

        private void SaveFrameToFile(string path, BitmapFrame image)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(image);
            encoder.Save(fs);
            fs.Close();
        }

        public MemoryStream GenerateGifStream(SessionTemplateCollection sessionPhotos)
        {
            GifBitmapEncoder gifEncoder = new GifBitmapEncoder();

            foreach (Photo photo in sessionPhotos)
            {
                gifEncoder.Frames.Add(photo.Image);
            }

            MemoryStream memory = new MemoryStream();

            gifEncoder.Save(memory);

            return memory;
        }
        
        public BitmapImage ImageFromStream(Stream stream)
        {
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = stream;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();

            return bitmapimage;
        }

        public void SaveStreamToFile(string path, MemoryStream stream)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var fs = new FileStream(path, FileMode.Create);
                fs.Write(stream.ToArray(), 0, (int)stream.Length);
                fs.Close();
            }
        }
    }
}
