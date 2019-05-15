using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restaurant.Core._Base
{
    public class HttpFile
    {
        // --------------------------------------------------------------------------
        // Constants
        // --------------------------------------------------------------------------
        private const int FileStartIndex = 0;

        // --------------------------------------------------------------------------
        // Properties
        // --------------------------------------------------------------------------

        public string FileName { get; set; }
        public string ContentType { get; set; }

        public long DataLength { get; set; }

        public byte[] Data { get; set; }

        // --------------------------------------------------------------------------
        // Constructors
        // --------------------------------------------------------------------------
        public static implicit operator HttpFile(FormFile httpPostedFileBase)
        {
            return new HttpFile(httpPostedFileBase);
        }

        private HttpFile(IFormFile httpPostedFileBase)
        {
            byte[] data;
            using (var ms = new MemoryStream())
            {
                httpPostedFileBase.CopyTo(ms);
                data = ms.ToArray();
            }

            FileName = httpPostedFileBase.FileName;
            ContentType = httpPostedFileBase.ContentType;
            DataLength = httpPostedFileBase.Length;

            Data = data;
        }

        public HttpFile(byte[] data, string fileName, string contentType = default(string))
        {
            Data = data;
            FileName = fileName;
            DataLength = data.Length;
            ContentType = contentType;
        }

        public HttpFile()
        {
        }

        // --------------------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------------------

        public static IList<HttpFile> GetList(IList<IFormFile> list)
        {
            return list.Select(x => x as HttpFile).ToList();
        }

        public bool SaveData(string fileName) // todo: this should not be here
        {
            BinaryWriter writer = null;

            try
            {
                writer = new BinaryWriter(File.OpenWrite(fileName));
                writer.Write(Data);

                writer.Flush();
                writer.Close();
            }
            catch (Exception)
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }

                return false;
            }

            return true;
        }
    }
}