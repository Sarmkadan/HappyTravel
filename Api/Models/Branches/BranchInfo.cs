using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Branches
{
    public readonly struct BranchInfo
    {
        [JsonConstructor]
        public BranchInfo(string title)
        {
            Title = title;
        }


        /// <summary>
        ///     Title of the company branch.
        /// </summary>
        [Required]
        public string Title { get; }
    }
}