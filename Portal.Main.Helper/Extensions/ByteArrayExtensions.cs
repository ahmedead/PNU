// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByteArrayExtensions.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   Defines the various compression types that are available
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Extensions
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;


    /// <summary>
    ///     Defines the various compression types that are available
    /// </summary>
    public enum CompressionType
    {
        /// <summary>
        ///     The g zip.
        /// </summary>
        GZip = 0, 

        /// <summary>
        ///     The deflate.
        /// </summary>
        Deflate = 1
    }

    /// <summary>
    ///     The byte array extensions.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts a byte array to a stringUtils
        /// </summary>
        /// <param name="buffer">
        /// raw string byte data
        /// </param>
        /// <param name="encoding">
        /// Character encoding to use. Defaults to Unicode
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToString(this byte[] buffer, Encoding encoding = null)
        {
            if (buffer == null)
            {
                return null;
            }

            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

            return encoding.GetString(buffer);
        }

        /// <summary>
        /// Convert bytes to Hex string
        /// </summary>
        /// <param name="source">
        /// Byte array
        /// </param>
        /// <param name="addSpace">
        /// Whether add space between Hex
        /// </param>
        /// <returns>
        /// Hex string
        /// </returns>
        public static string ToHexString(this byte[] source, bool addSpace = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Length == 0)
            {
                return string.Empty;
            }

            var result = new StringBuilder(source.Length * 2);

            if (addSpace)
            {
                foreach (var hex in source)
                {
                    result.AppendFormat("{0:X2}", hex);
                    result.Append(" ");
                }
            }
            else
            {
                foreach (var hex in source)
                {
                    result.AppendFormat("{0:X2}", hex);
                }
            }

            return result.ToString().Trim();
        }

        /// <summary>
        /// Convert byte to Hex string
        /// </summary>
        /// <param name="source">
        /// Byte
        /// </param>
        /// <returns>
        /// Hex string
        /// </returns>
        public static string ToHexString(this byte source)
        {
            return Convert.ToString(source, 16).PadLeft(2, '0');
        }

        /// <summary>
        /// Convert bytes to Encoding string by using Encoding.Default
        /// </summary>
        /// <param name="source">
        /// Byte array
        ///     Byte array
        /// </param>
        /// <returns>
        /// Encoding string by using Encoding.Default
        /// </returns>
        /// <summary>
        /// Convert bytes to Image
        /// </summary>
        /// <returns>
        /// Image object
        /// </returns>
        public static Image ToImage(this byte[] source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            MemoryStream memoryStream = null;
            Image result;

            try
            {
                memoryStream = new MemoryStream(source);
                memoryStream.Write(source, 0, source.Length);
                result = Image.FromStream(memoryStream);
                memoryStream.Flush();
            }
            finally
            {
                memoryStream?.Close();
            }

            return result;
        }

        /// <summary>
        /// Convert bytes to object
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// Byte array
        /// </param>
        /// <returns>
        /// Object
        /// </returns>
        public static T ToObject<T>(this byte[] source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Length == 0)
            {
                return default(T);
            }

            MemoryStream memoryStream = null;
            T result;

            try
            {
                memoryStream = new MemoryStream(source) { Position = 0 };
                var binaryFormatter = new BinaryFormatter();
                result = (T)binaryFormatter.Deserialize(memoryStream);
                memoryStream.Flush();
            }
            finally
            {
                memoryStream?.Close();
            }

            return result;
        }

        /// <summary>
        /// Convert bytes to object
        /// </summary>
        /// <param name="source">
        /// Byte array
        /// </param>
        /// <returns>
        /// Object
        /// </returns>
        public static object ToObject(this byte[] source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Length == 0)
            {
                return null;
            }

            MemoryStream memoryStream = null;
            object result;

            try
            {
                memoryStream = new MemoryStream(source) { Position = 0 };
                var binaryFormatter = new BinaryFormatter();
                result = binaryFormatter.Deserialize(memoryStream);
                memoryStream.Flush();
            }
            finally
            {
                memoryStream?.Close();
            }

            return result;
        }

        /// <summary>
        /// Compresses byte array using CompressionType
        /// </summary>
        /// <param name="source">
        /// Byte array to compress
        /// </param>
        /// <param name="compressionType">
        /// Compression Type
        /// </param>
        /// <returns>
        /// A compressed byte array
        /// </returns>
        public static byte[] Compress(this byte[] source, CompressionType compressionType = CompressionType.GZip)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            MemoryStream outputStream = null;
            Stream zipStream = null;
            byte[] result;

            try
            {
                outputStream = new MemoryStream();
                zipStream = GetZipStream(outputStream, CompressionMode.Compress, compressionType);
                zipStream.Write(source, 0, source.Length);
                result = outputStream.ToArray();
                zipStream.Flush();
                outputStream.Flush();
            }
            finally
            {
                zipStream?.Close();

                outputStream?.Close();
            }

            return result;
        }

        /// <summary>
        /// Decompresses byte array using CompressionType
        /// </summary>
        /// <param name="source">
        /// Byte array to decompress
        /// </param>
        /// <param name="compressionType">
        /// Compression Type
        /// </param>
        /// <returns>
        /// A decompressed byte array
        /// </returns>
        public static byte[] Decompress(this byte[] source, CompressionType compressionType = CompressionType.GZip)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            MemoryStream outputStream = null;
            MemoryStream inputStream = null;
            Stream zipStream = null;
            byte[] result;

            try
            {
                outputStream = new MemoryStream();
                inputStream = new MemoryStream(source);
                zipStream = GetZipStream(inputStream, CompressionMode.Decompress, compressionType);
                zipStream.CopyTo(outputStream);
                result = outputStream.ToArray();
                zipStream.Flush();
                inputStream.Flush();
                outputStream.Flush();
            }
            finally
            {
                zipStream?.Close();

                inputStream?.Close();

                outputStream?.Close();
            }

            return result;
        }

        /// <summary>
        /// Get Zip Stream by type
        /// </summary>
        /// <param name="memoryStream">
        /// </param>
        /// <param name="compressionMode">
        /// </param>
        /// <param name="compressionType">
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        private static Stream GetZipStream(
            MemoryStream memoryStream, 
            CompressionMode compressionMode, 
            CompressionType compressionType)
        {
            if (compressionType == CompressionType.GZip)
            {
                return new GZipStream(memoryStream, compressionMode);
            }

            return new DeflateStream(memoryStream, compressionMode);
        }
    }
}