using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
//using Newtonsoft.Json;
using Android.Graphics;
using Android.Content;
using System.Net;
using Android.Provider;
using Java.IO;
using System.Text.RegularExpressions;

namespace travelogue
{
	public static class Helper
	{
		private static async Task<string> HandleResponseAsync(HttpResponseMessage response)
		{
			string bb = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			return bb;
		}

		public static async Task<Bitmap> GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}

		public static async Task<string> Upload(byte[] image)
		{
			using (var client = new HttpClient())
			{
				using (var content =
					new MultipartFormDataContent())
				{
					content.Add(new StreamContent(new MemoryStream(image)), "bilddatei", "upload.jpg");

					using (
						var message =
						await client.PostAsync("http://www.directupload.net/index.php?mode=upload", content))
					{
						var input = await message.Content.ReadAsStringAsync();

						return !string.IsNullOrWhiteSpace(input) ? Regex.Match(input, @"http://\w*\.directupload\.net/images/\d*/\w*\.[a-z]{3}").Value : null;
					}
				}
			}
		}

	/*public static async Task<string> UploadImage(string uri)
		{
			using (var client = new HttpClient ()) {
				var content = new MultipartFormDataContent();
				var imageContent=new ByteArrayContent (File.ReadAllBytes (fileUri)); // Here fileuri 
				imageContent.Headers­.ContentType = MediaTypeHeaderValue­.Parse("image/­jpeg"); content.Add (imageContent, "profile_image",Path­.GetFileName(fileUri­)); var postResponse =await HttpPostForJson (postUrl,content);
				postResponse =await HttpPostForJson ("http://www.fantacode.com/TempRest/upload",content);
				return postResponse.file;
			}

		}*/

		/*public static async Task <ServieReturnModel> HttpPostForJson(string url, HttpContent postContent) {
			ServieReturnModel result = new ServieReturnModel();
			using(var client = new HttpClient()) {
				try {
					using(var response = await client.PostAsync(url, postContent)) {
						using(var responseContent = response.Content) {
							var responseString = await responseContent.ReadAsStringAsync();
							result.ServieReturnModel = JsonConvert.Deseria­lizeObject<ServiceLa­yer>(resp­onseString);
							result.isSuccess = tru­e;
						}
					}
				} catch (Exception ex) {
					result.exception = ex;
				}
				return result;
			}
		}*/




	}
}

