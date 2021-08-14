using System.Collections.Generic;
using Newtonsoft.Json;

namespace IIABackend
{
    /// <summary>
    /// Ofers Object
    /// </summary>
    public class Offer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Offer"/> class.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="link">Link</param>
        public Offer(string id, string catgId, string organisationName, string title, string percentageDiscount, string fixedDiscount, string organisationAddress, string city, string email, string phone, bool NationalValidity, string startDate, string expiryDate, string imagePath, string OfferDescription)
        {
            this.Id = id;
            this.CategoryId = catgId;
            this.OrganisationName = organisationName;
            this.Title = title;
            this.PercentageDiscount = percentageDiscount;
            this.FixedDiscount = fixedDiscount;
            this.OrganisationAddress = organisationAddress;
            this.City = city;
            this.Email = email;
            this.Phone = phone;
            this.NationalValidity = NationalValidity;
            this.StartDate = startDate;
            this.ExpiryDate = expiryDate;
            this.ImagePath = imagePath;
            this.OfferDescription = OfferDescription;
        }

        /// <summary>
        /// Gets or sets id of the offers
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets catgId of the offers
        /// </summary>
        [JsonProperty(PropertyName = "CatrgoryId")]
        public string CategoryId { get; set; }

        ///<summary>
        /// Gets or sets Category id of the offers
        /// </summary>
        [JsonProperty(PropertyName = "OrganisationName")]
        public string OrganisationName { get; set; }
        /// <summary>
        /// Gets or sets title of the offers
        /// </summary>
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets percentageDiscount of the offers
        /// </summary>
        [JsonProperty(PropertyName = "PercentageDiscount")]
        public string PercentageDiscount { get; set; }

        /// <summary>
        /// Gets or set s fixedDiscount of the offers
        /// </summary>
        [JsonProperty(PropertyName = "FixedDiscount")]
        public string FixedDiscount { get; set; }

        /// <summary>
        /// Gets or sets organisationAddress of the offers
        /// </summary>
        [JsonProperty(PropertyName = "OrganisationAddress")]
        public string OrganisationAddress { get; set; }

        /// <summary>
        /// Gets or sets city of the offers
        /// </summary>
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets email of the offers
        /// </summary>
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets phone of the offers
        /// </summary>
        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets NationalValidity of the offers
        /// </summary>
        [JsonProperty(PropertyName = "NationalValidity")]
        public bool NationalValidity { get; set; }

        /// <summary>
        /// Gets or sets startDate of the offers
        /// </summary>
        [JsonProperty(PropertyName = "StartDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets expiryDate of the offers
        /// </summary>
        [JsonProperty(PropertyName = "ExpiryDate")]
        public string ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets imagepath of the offers
        /// </summary>
        [JsonProperty(PropertyName = "ImagePath")]
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets description of the offers
        /// </summary>
        [JsonProperty(PropertyName = "OfferDescription")]
        public string OfferDescription { get; set; }
    }
}
