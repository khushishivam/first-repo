using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IIABackend
{
    /// <summary>
    /// News Create function
    /// </summary>
    public static class CreateOffers
    {
        /// <summary>
        /// Function
        /// </summary>
        /// <param name="req">request body</param>
        /// <param name="log">logger</param>
        /// <returns>HTTP result</returns>
        [FunctionName("CreateOffers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get rates request!");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string token = data?.token;

            var id = JWTTokenBuilder.ValidateJwtToken(token);

            if (string.IsNullOrEmpty(id))
            {
                return new UnauthorizedResult();
            }

            string CategoryId = data?.CategoryId;
            string OrganisationName = data?.OrganisationName;
            string Title = data?.Title;
            string PercentageDiscount = data?.PercentageDiscount; //should be int or string????????
            string FixedDiscount = data?.FixedDiscount;
            string OrganisationAddress = data?.OrganisationAddress;
            string City = data?.City;
            string Email = data?.Email;
            string Phone = data?.Phone;
            bool NationalValidty = data?.NationalValidity;
            string StartDate = data?.StartDate;
            string ExpiryDate = data?.ExpiryDate;
            string ImagePath = data?.ImagePath;
            string OfferDescription = data?.OfferDescription;

            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(OfferDescription) || string.IsNullOrEmpty(ImagePath) || string.IsNullOrEmpty(StartDate) || string.IsNullOrEmpty(ExpiryDate) || string.IsNullOrEmpty(Phone))
            {
                return new BadRequestObjectResult("Invalid Inputs");
            }

            /*

            if (!News.Categories.Contains(category))
            {
                return new BadRequestObjectResult("Invalid Category");
            }
            */

            Database.CreateOffers(CategoryId, OrganisationName, Title, PercentageDiscount, FixedDiscount, OrganisationAddress, City, Email, Phone, NationalValidty, StartDate, ExpiryDate, ImagePath, OfferDescription);
            return new OkObjectResult(new BaseResponse(token, "Offer created successfuly!!!!"));
        }
    }
}
