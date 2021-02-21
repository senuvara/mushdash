using System;
using System.IO;
using System.Xml.Serialization;

namespace Assets.Scripts.PeroTools.Commons
{
	public class XmlUtils
	{
		public static object Deserialize(Type type, string xml)
		{
			try
			{
				using (StringReader textReader = new StringReader(xml))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(type);
					return xmlSerializer.Deserialize(textReader);
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static object Deserialize(Type type, Stream stream)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return xmlSerializer.Deserialize(stream);
		}

		public static string Serialize(Type type, object obj)
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			try
			{
				xmlSerializer.Serialize(memoryStream, obj);
			}
			catch (InvalidOperationException)
			{
				throw;
			}
			memoryStream.Position = 0L;
			StreamReader streamReader = new StreamReader(memoryStream);
			string result = streamReader.ReadToEnd();
			streamReader.Dispose();
			memoryStream.Dispose();
			return result;
		}
	}
}
