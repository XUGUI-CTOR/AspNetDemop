using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace GuiTwo.Api.Controllers
{
    public class UpLoadFileController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage UpLoadFile(string FileName)
        {
            if (string.IsNullOrWhiteSpace(FileName))
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"{nameof(FileName)}不能为空");
            string UploadPath = WebConfigurationManager.AppSettings.Get("UploadPath");
            if (!Directory.Exists(UploadPath))
                Directory.CreateDirectory(UploadPath);
            var FullFilePath = Path.Combine(UploadPath, FileName);
            using (var fileStream = HttpContext.Current.Request.InputStream)
            {
                SaveAs(FullFilePath, fileStream);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void SaveAs(string FileName, Stream FileStream)
        {
            long StartPos = 0;
            int StartPosition = 0, Endposition = 0;
          
            var ContentRange = HttpContext.Current.Request.Headers["Content-Range"];
            if (!string.IsNullOrWhiteSpace(ContentRange))
            {
                ContentRange = ContentRange.Trim().Replace("bytes", "").Substring(0, ContentRange.IndexOf('/'));
                var ranges = ContentRange.Split('-');
                int.TryParse(ranges[0], out StartPosition);
                int.TryParse(ranges[1], out Endposition);
            }
            if (StartPosition > Endposition)
                return;
            //var IsFileExist = File.Exists(FullPath);
            using (FileStream WriteFileStream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                StartPos = WriteFileStream.Length;
                if (StartPos > Endposition)
                    return;
                WriteFileStream.Seek(StartPos, SeekOrigin.Current);
                byte[] datas = new byte[1024];
                int nReadSize = FileStream.Read(datas, 0, datas.Length);
                while (nReadSize > 0)
                {
                    WriteFileStream.Write(datas, 0, nReadSize);
                    nReadSize = FileStream.Read(datas, 0, datas.Length);
                }
            }
        }
    }
}